using System;
using System.Collections;
using System.Collections.Generic;
using ScaryEvents;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomController : MonoBehaviour
{
    public ChangeRoomInfo[] changeRoomCandidates;
    private ChangeRoomInfo currentChangeRoomInfo;
    
    // Start is called before the first frame update
    void Start()
    {
        // Select one room
        var currentRoomIndex = UnityEngine.Random.Range(0, changeRoomCandidates.Length);
        currentChangeRoomInfo = changeRoomCandidates[currentRoomIndex];
        
        currentChangeRoomInfo.sectionIndices = new int[currentChangeRoomInfo.sections.Length];
            
        for (int i = 0; i < currentChangeRoomInfo.sections.Length; i++)
        {
            currentChangeRoomInfo.sections[i].sectionCandidates[currentChangeRoomInfo.sectionIndices[i]].SetActive(true);
        }
    }

    // 현재 선택된 room 안의 section 중 하나를 비활성화하고, 다른 section을 활성화. 단, sectionIndices 들이 서로 2 이상 차이나면 안 됨. 
    public void ChangeRandomSectionSequence()
    {
        FindObjectOfType<ObjectEventHandler>().ChangeRoom(Array.Empty<ObjectInfoHolder>());
        
        int minIndex = int.MaxValue;
        List<Section> minIndexSections = new List<Section>();
        
        for (int i = 0; i < currentChangeRoomInfo.sections.Length; i++)
        {
            if (currentChangeRoomInfo.sectionIndices[i] <= minIndex)
            {
                if (currentChangeRoomInfo.sectionIndices[i] >= currentChangeRoomInfo.sections[i].sectionCandidates.Length)
                {
                    continue;
                }
                
                minIndex = currentChangeRoomInfo.sectionIndices[i];
                minIndexSections.Add(currentChangeRoomInfo.sections[i]);
            }
        }
        
        if (minIndexSections.Count == 0)
        {
            SceneManager.LoadScene(currentChangeRoomInfo.nextRoomSceneName);
            return;
        }
        
        // Select random section
        var selectedSectionIndex = UnityEngine.Random.Range(0, minIndexSections.Count);
        var selectedSection = minIndexSections[selectedSectionIndex];
        selectedSection.sectionCandidates[currentChangeRoomInfo.sectionIndices[selectedSectionIndex]].SetActive(false);
        selectedSection.sectionCandidates[currentChangeRoomInfo.sectionIndices[selectedSectionIndex] + 1].SetActive(true);
        
        currentChangeRoomInfo.sectionIndices[selectedSectionIndex]++;
        MainManager.Instance.player.transform.position = MainManager.Instance.resetPos;
        
    }
}

[Serializable]
public class ChangeRoomInfo
{
    public string nextRoomSceneName; // The name of the scene to be loaded when all sections are activated
    [HideInInspector] public int[] sectionIndices; 
    public Section[] sections;
}

[Serializable]
public class Section
{
    public GameObject[] sectionCandidates;
}