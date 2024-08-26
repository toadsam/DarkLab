using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Tutorial
{
    public class TutorialElement : MonoBehaviour
    {
        public string tutorialElementId = "tutorialElementId";
        
        public void CheckTutorialDone()
        {
            if (PlayerPrefs.GetInt(tutorialElementId, 0) != 1 && PlayerPrefs.GetInt("prestigeCount123", 0) == 0)
            {
                StartAndSetTutorial();
            }
        }

        public Button[] tutorialDoneTriggerButtons;
        public TutorialElementTrigger[] tutorialElementTriggers;
        public List<GameObject> tutorialPanel;
        private int currentTutorialIndex = 0;

        public void StartAndSetTutorial()
        {
            foreach (var button in tutorialDoneTriggerButtons)
            {
                button.onClick.AddListener(NextTutorial);
            }
            
            foreach (var tutorialElementTrigger in tutorialElementTriggers)
            {
                tutorialElementTrigger.onTutorialElementTriggered.AddListener(NextTutorial);
            }
            
            tutorialPanel[currentTutorialIndex].SetActive(true);
        }
        
        public void NextTutorial()
        {
            if (currentTutorialIndex == tutorialPanel.Count - 1)
            {
                tutorialPanel[currentTutorialIndex].SetActive(false);
                TutorialDone();
                return;
            }
            
            tutorialPanel[currentTutorialIndex].SetActive(false);
            currentTutorialIndex++;
            tutorialPanel[currentTutorialIndex].SetActive(true);
        }
        
        void TutorialDone()
        {
            PlayerPrefs.SetInt(tutorialElementId, 1);
            currentTutorialIndex = 0;
            
            foreach (var button in tutorialDoneTriggerButtons)
            {
                button.onClick.RemoveListener(NextTutorial);
            }
        }

        private void Start()
        {
            foreach (var pGameObject in tutorialPanel)
            {
                pGameObject.SetActive(false);
            }
        }
    }
}