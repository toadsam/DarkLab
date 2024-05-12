using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    // public GameObject[] group1;
    // public GameObject[] group2;
    // public GameObject[] group3;
    // public GameObject[] group4;

    public List<GameObject> group1 = new List<GameObject>();
    public List<GameObject> group2 = new List<GameObject>();
    public List<GameObject> group3 = new List<GameObject>();
    public List<GameObject> group4 = new List<GameObject>();

    private GameObject[] activeObjects = new GameObject[4]; // 각 그룹의 현재 활성화된 오브젝트
    private bool[][] usedObjects; // 각 그룹의 사용된 오브젝트 추적

    void Start()
    {
        // 각 그룹별 사용된 오브젝트 배열 초기화
        usedObjects = new bool[4][];
        usedObjects[0] = new bool[group1.Count];
        usedObjects[1] = new bool[group2.Count];
        usedObjects[2] = new bool[group3.Count];
        usedObjects[3] = new bool[group4.Count];

        ActivateRandomObject(group1, 0);
        ActivateRandomObject(group2, 1);
        ActivateRandomObject(group3, 2);
        ActivateRandomObject(group4, 3);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeRandomObject();  // 스페이스바를 누르면 이 함수 실행
        }
    }

    void ActivateRandomObject(List<GameObject> group, int groupIndex)
    {
        List<int> availableIndices = new List<int>();
        for (int i = 0; i < group.Count; i++)
        {
            if (!usedObjects[groupIndex][i])  // 아직 사용되지 않은 오브젝트 인덱스 추가
            {
                availableIndices.Add(i);
            }
        }

        if (availableIndices.Count > 0)
        {
            int randomIndex = Random.Range(0, availableIndices.Count);
            int selectedIndex = availableIndices[randomIndex];
            GameObject selectedObject = group[selectedIndex];
            selectedObject.SetActive(true);
            usedObjects[groupIndex][selectedIndex] = true; // 사용된 오브젝트로 표시

            // 이전에 활성화된 오브젝트가 있으면 비활성화
            if (activeObjects[groupIndex] != null)
            {
                activeObjects[groupIndex].SetActive(false);
            }
            activeObjects[groupIndex] = selectedObject; // 새로 활성화된 오브젝트 저장
        }
    }

    public void ChangeRandomObject()
    {
        List<int> availableGroupIndices = new List<int>();
        for (int i = 0; i < usedObjects.Length; i++)
        {
            // 모든 오브젝트가 사용되었는지 확인
            bool allUsed = true;
            foreach (bool used in usedObjects[i])
            {
                if (!used)
                {
                    allUsed = false;
                    break;
                }
            }

            if (!allUsed)
            {
                availableGroupIndices.Add(i);
            }
        }

        if (availableGroupIndices.Count > 0)
        {
            int groupIndex = availableGroupIndices[Random.Range(0, availableGroupIndices.Count)]; // 사용 가능한 그룹 중 하나를 랜덤 선택
            ActivateRandomObject(GetGroupByIndex(groupIndex), groupIndex);
        }
        else
        {
            Debug.Log("All objects have been used. No more changes possible.");
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
                return null; // 유효하지 않은 인덱스
        }
    }
}
