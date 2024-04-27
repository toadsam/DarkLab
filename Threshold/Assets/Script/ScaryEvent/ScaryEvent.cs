using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
//public enum scaryEventTier { Low, Medium, High };

public enum scaryEventTier { Low, Medium, High };
public enum scaryEventWhen { OnAwake, OnProximity, OnViewInteractionStart, OnFocusInteractionStart, OnSustainedFocusInteraction };
public class ScaryEvent : MonoBehaviour
{
    public scaryEventTier scaryEventTier;
    public scaryEventWhen scaryEventWhen;
    
    public ObjectInfoHolder currentEventTarget;
    private Dictionary <string, int> currentIndexForTargets = new Dictionary<string, int>();

    public UnityEvent onStart;
    public UnityEvent onPlay;
    public UnityEvent onUpdate;
    public UnityEvent onComplete;

    private void Awake()
    {
        currentIndexForTargets.Add("light", 0);
        currentIndexForTargets.Add("audio", 0);
        currentIndexForTargets.Add("transform", 0);
    }

    public T GetCurrentTarget<T>(string targetType) where T : UnityEngine.Object {
        int index = currentIndexForTargets.ContainsKey(targetType) ? currentIndexForTargets[targetType] : 0;
        var targetList = currentEventTarget.GetType().GetField(targetType + "Targets").GetValue(currentEventTarget) as List<T>;
        currentIndexForTargets[targetType] +=1;
        return targetList != null && index < targetList.Count ? targetList[index] : null;
    }

    public void ResetIndexForTargets()
    {
        var keys = new List<string>(currentIndexForTargets.Keys);
        foreach (var key in keys)
            currentIndexForTargets[key] = 0;
    }
    
    public void StartEvent()
    {
        onStart.Invoke();
        onPlay.Invoke();
        onUpdate.Invoke();
        onComplete.Invoke();
    }
}