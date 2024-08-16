using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;

public class StoryScriptVisualizer : MonoBehaviour
{
    public TextMeshProUGUI storyText;
    
    public void GetProgressAndShowStory()
    {
        StartCoroutine(GetProgressAndShowStoryRoutine());
    }
    
    private IEnumerator GetProgressAndShowStoryRoutine()
    {
        Debug.Log("GetProgressAndShowStoryRoutine : " + ProgressChecker.Instance.currentProgress);
        var operation = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("StoryTranslations", ProgressChecker.Instance.currentProgress.ToString());
        yield return operation;
        
        if (operation.Status == AsyncOperationStatus.Succeeded)
        {
            storyText.text = operation.Result;
        }
        else
        {
            Debug.LogWarning($"Failed to get localized story script for progress {ProgressChecker.Instance.currentProgress}");
            storyText.text = "Failed to get localized story script for progress " + ProgressChecker.Instance.currentProgress;
        }
    }
}
