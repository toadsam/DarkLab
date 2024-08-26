using System.Collections;
using System.Collections.Generic;
using Tutorial;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public TutorialElement[] tutorialElements;
    private Dictionary<string, TutorialElement> tutorialElementDictionary = new Dictionary<string, TutorialElement>();
    
    // Start is called before the first frame update
    void Awake()
    {
        foreach (var tutorialElement in tutorialElements)
        {
            if (!tutorialElementDictionary.TryAdd(tutorialElement.tutorialElementId, tutorialElement))
            {
                Debug.LogError("Tutorial element with id " + tutorialElement.tutorialElementId + " already exists.");
                continue;
            }
        }
    }

    // Update is called once per frame
    public void StartTutorialById(string id)
    {
        if (tutorialElementDictionary.ContainsKey(id))
        {
            tutorialElementDictionary[id].StartAndSetTutorial();
        }
        else
        {
            Debug.LogError("Tutorial element with id " + id + " not found.");
        }
    }
    
    public void CheckAndStartTutorialById(string id)
    {
        if (tutorialElementDictionary.ContainsKey(id))
        {
            tutorialElementDictionary[id].StartAndSetTutorial();
        }
        else
        {
            Debug.LogError("Tutorial element with id " + id + " not found.");
        }
    }
}
