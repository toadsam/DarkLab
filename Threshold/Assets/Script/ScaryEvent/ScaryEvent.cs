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
    public List<ScaryEffect> scaryEffects;
    // public List<ScaryEffect> startEffects;
    public scaryEventTier scaryEventTier;
    public scaryEventWhen scaryEventWhen;

    public ObjectInfoHolder currentEventTarget;
    private Dictionary<string, int> currentIndexForTargets = new Dictionary<string, int>();


    private void Start()
    {
        currentIndexForTargets.Add("light", 0);
        currentIndexForTargets.Add("audio", 0);
        currentIndexForTargets.Add("transform", 0);

        for (int i = 0; i < transform.childCount; i++)
            scaryEffects[i] = transform.GetChild(i).GetComponent<ScaryEffect>();
    }

    public void StartEvent()
    {
        scaryEffects[0].StartEffect();
    }

    public T GetCurrentTarget<T>(string targetType) where T : UnityEngine.Object
    {
        int index = currentIndexForTargets.ContainsKey(targetType) ? currentIndexForTargets[targetType] : 0;
        var targetList = currentEventTarget.GetType().GetField(targetType + "Targets").GetValue(currentEventTarget) as List<T>;
        currentIndexForTargets[targetType] += 1;
        return targetList != null && index < targetList.Count ? targetList[index] : null;
    }
}