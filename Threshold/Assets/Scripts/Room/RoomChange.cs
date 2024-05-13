using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomChange : MonoBehaviour
{
    public GameObject[] As;
    public GameObject[] Bs;
    public GameObject[] Cs;
    public GameObject[] Ds;

    public int stage = 0;
    public List<GameObject> curObjects = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        RandomSelect();


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private List<GameObject> RandomSelect()
    {
        int num = UnityEngine.Random.Range(0, 2);
        As[num].SetActive(true);
        curObjects.Add(As[num]);
        num = UnityEngine.Random.Range(0, 2);
        Bs[num].SetActive(true);
        curObjects.Add(Bs[num]);
        num = UnityEngine.Random.Range(0, 2);
        Cs[num].SetActive(true);
        curObjects.Add(Cs[num]);
        num = UnityEngine.Random.Range(0, 2);
        Ds[num].SetActive(true);
        curObjects.Add(Ds[num]);
        return curObjects;
    }

    void RoomStage(List<GameObject> gameObjects) 
    {
        int num = Random.Range(0, gameObjects.Count);


    }
}
