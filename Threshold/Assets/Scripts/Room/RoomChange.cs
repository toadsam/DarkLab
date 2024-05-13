using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomChange : MonoBehaviour
{
    public GameObject[] As;
    public GameObject[] Bs;
    public GameObject[] Cs;
    public GameObject[] Ds;

    [SerializeField]private int stage;
    public List<GameObject> curObjects = new List<GameObject>();
    public List<GameObject> setObjects = new List<GameObject>();
    public bool[] isSelectObject = new bool[4] { false, false, false, false };
    // Start is called before the first frame update
    void Start()
    {
        setObjects = RandomSelect();
        Copy();
        stage = 1;
        



    }
    void Copy() 
    {
        for (int i = 0; i < setObjects.Count; i++) 
        {
            curObjects.Add(setObjects[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RoomStage(curObjects);  // 스페이스바를 누르면 이 함수 실행
        }
    }

    private List<GameObject> RandomSelect()
    {
        List<GameObject> curObjects = new List<GameObject>();
        int num = UnityEngine.Random.Range(0, 2);
        As[num].SetActive(true);
        curObjects.Add(As[num]);    
        Bs[num].SetActive(true);
        curObjects.Add(Bs[num]);   
        Cs[num].SetActive(true);
        curObjects.Add(Cs[num]);
        Ds[num].SetActive(true);
        curObjects.Add(Ds[num]);
        return curObjects;
       
    }
    bool isAllSelect() 
    {
        for(int i = 0; i < isSelectObject.Length;i++) 
        {
            if (isSelectObject[i] == false) return false;
        }
        return true;

    }

    private void ResetBool ()
    {
        for (int i = 0; i < isSelectObject.Length; i++)
        {
            isSelectObject[i] = false;
        }


    }


    void RoomStage(List<GameObject> gameObjects) 
    {
        if (!isAllSelect())
        {
            int num = Random.Range(0, gameObjects.Count);
            if(isSelectObject[num] == true) 
            {
                RoomStage(curObjects);
            }
            if (stage == 1)
            {
                gameObjects[num].transform.GetChild(0).gameObject.SetActive(false);
                gameObjects[num].transform.GetChild(1).gameObject.SetActive(true);
                Debug.Log("1단계 왔어요");
            }
            if (stage == 2)
            {
                gameObjects[num].transform.GetChild(1).gameObject.SetActive(false);
                gameObjects[num].transform.GetChild(2).gameObject.SetActive(true);
            }
            isSelectObject[num] = true;
        }
        else 
        {
            Debug.Log("값이 없습니다.");
            if (stage != 2)
            {
                stage++;
                Debug.Log("스테이지가 증가합니다.");
                ResetBool();
                RoomStage(curObjects);
            }
            else 
            {
                Debug.Log("최고 단계입니다!");
            }
        }

        //gameObjects.Remove(As)

    }
}
