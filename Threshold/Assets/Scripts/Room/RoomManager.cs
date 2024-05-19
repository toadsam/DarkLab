using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public List<GameObject> group1 = new List<GameObject>();
    public List<GameObject> group2 = new List<GameObject>();
    public List<GameObject> group3 = new List<GameObject>();
    public List<GameObject> group4 = new List<GameObject>();

    private GameObject[] activeObjects = new GameObject[4];    //다음 오브젝트가 활성화 될 시 비활성화 시켜줄 배열
    private bool[][] usedObjects;                              //각 그룹의 사용된 오브젝트 배열
    private int[] nextObjectIndices;                           //각 그룹의 다음에 사용될 오브젝트 인덱스 저장하는 배열

    public GameObject nextChangeDoor;
    public GameObject curRoomChangeDoor;

    void Start()
    {
        // 각 그룹별 사용된 오브젝트 배열 및 다음에 사용될 오브젝트 인덱스 초기화
        usedObjects = new bool[4][];
        usedObjects[0] = new bool[group1.Count];
        usedObjects[1] = new bool[group2.Count];
        usedObjects[2] = new bool[group3.Count];
        usedObjects[3] = new bool[group4.Count];

        nextObjectIndices = new int[4];

        // 각 그룹의 첫 번째 오브젝트 활성화
        ActivateFirstObject(group1, 0);
        ActivateFirstObject(group2, 1);
        ActivateFirstObject(group3, 2);
        ActivateFirstObject(group4, 3);
    }

    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     ChangeRandomObject();
        // }
    }

    void ActivateFirstObject(List<GameObject> group, int groupIndex)
    {
        if (group.Count > 0)
        {
            GameObject firstObject = group[0];
            firstObject.SetActive(true);
            usedObjects[groupIndex][0] = true;
            nextObjectIndices[groupIndex] = 1;

            activeObjects[groupIndex] = firstObject;
        }
    }

    void ActivateNextObject(List<GameObject> group, int groupIndex)
    {
        int nextIndex = nextObjectIndices[groupIndex];

        if (nextIndex < group.Count)
        {
            GameObject nextObject = group[nextIndex];
            nextObject.SetActive(true);
            usedObjects[groupIndex][nextIndex] = true;
            nextObjectIndices[groupIndex]++;

            // 이전에 활성화된 오브젝트가 있으면 비활성화
            if (activeObjects[groupIndex] != null)
                activeObjects[groupIndex].SetActive(false);

            activeObjects[groupIndex] = nextObject;
            MainManager.Instance.player.GetComponent<Player>().Controller.enabled = false;
            MainManager.Instance.player.transform.position = MainManager.Instance.resetPos;
            MainManager.Instance.player.GetComponent<Player>().Controller.enabled = true;
        }
    }

    public void ChangeRandomObject()
    {
        //랜덤으로 돌릴 수 있는 그룹 배열 저장하는 배열
        List<int> availableIndices = new List<int>();

        //모든 그룹에서 아직 사용되지 않은 오브젝트의 가장 작은 인덱스 찾기
        int minUnusedIndex = 5;
        for (int i = 0; i < usedObjects.Length; i++)
        {
            for (int j = 0; j < usedObjects[i].Length; j++)
            {
                if (!usedObjects[i][j] && j < minUnusedIndex)
                    minUnusedIndex = j;
            }
        }

        //각 그룹에서 minUnusedIndex의 오브젝트를 한 번도 활성화 시키지 않았다면 리스트에 추가
        for (int i = 0; i < usedObjects.Length; i++)
        {
            if (minUnusedIndex < usedObjects[i].Length && !usedObjects[i][minUnusedIndex])
                availableIndices.Add(i);
        }

        if (availableIndices.Count > 0)
        {
            int groupIndex = availableIndices[Random.Range(0, availableIndices.Count)];
            ActivateNextObject(GetGroupByIndex(groupIndex), groupIndex);
        }
        else
        {
            curRoomChangeDoor.SetActive(false);
            nextChangeDoor.SetActive(true);
        }
    }


    List<GameObject> GetGroupByIndex(int index)
    {
        switch (index)
        {
            case 0:
                return group1;
            case 1:
                return group2;
            case 2:
                return group3;
            case 3:
                return group4;
            default:
                return null;
        }
    }
}
