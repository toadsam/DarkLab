using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Room : MonoBehaviour
{
    public RoomManager rm;
    private Dictionary<string, float> itemsWithProbabilities;

    public Room()
    {
        itemsWithProbabilities = new Dictionary<string, float>();
    }

    private void Awake()
    {
        itemsWithProbabilities.Add("Room", 0.8f);
        itemsWithProbabilities.Add("BathRoom", 0.1f);
        itemsWithProbabilities.Add("WareHouse", 0.1f);

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    private void SelectRoom()
    {


    }

    public string GetRandomItem()
    {
        float totalProbability = 0f;
        foreach (var entry in itemsWithProbabilities)
            totalProbability += entry.Value;

        float randomPoint = UnityEngine.Random.Range(0f, totalProbability);
        float cumulativeProbability = 0f;

        foreach (var entry in itemsWithProbabilities)
        {
            cumulativeProbability += entry.Value;
            if (randomPoint <= cumulativeProbability)
                return entry.Key;
        }
        return null;
    }


    private void CheckRoomName(string sceneName)
    {
        if (sceneName == "StudentRoom")
            SceneManager.LoadScene("GrandmaRoom");
        else if (sceneName == "GrandmaRoom")
            SceneManager.LoadScene("BoyRoom");
        else
            SceneManager.LoadScene("StudentRoom");
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            if(this.gameObject.name == "NextStageDoor"){
                string roomName = GetRandomItem();
                if (roomName == "Room")
                {
                    Scene scene = SceneManager.GetActiveScene();
                    CheckRoomName(scene.name);
                }
                else
                    SceneManager.LoadScene(roomName);
            }else
                rm.ChangeRandomObject();
        }
    }
}
