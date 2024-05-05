using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectEventHandler : MonoBehaviour
{
    public ScaryEvent[] scaryEvents;
    [SerializeField] private ObjectInfoHolder[] startTargets;
    public ObjectInfoHolder targrt;  
    
    private void Start()
    {
        scaryEvents = FindObjectsOfType<ScaryEvent>();
        if (startTargets == null)
            Debug.Log("����");
        else
        {             
            for (int i = 0; i <startTargets.Length; i++)              
                Match(startTargets[i], scaryEventWhen.OnAwake);
        }
    }
   
    public void Match(ObjectInfoHolder objectInfoHolder,scaryEventWhen eventWhen) 
    {
        for (int i = 0; i < scaryEvents.Length; i++)
        {
            if (objectInfoHolder.ObjectTier == scaryEvents[i].scaryEventTier && eventWhen  == scaryEvents[i].scaryEventWhen)
            {
                scaryEvents[i].currentEventTarget = objectInfoHolder;
                //if (scaryEvents[i].currentEventTarget != null)
                Debug.Log(scaryEvents[i].currentEventTarget);
                scaryEvents[i].ResetIndexForTargets();
              //  Debug.Log(objectInfoHolder.name);
                scaryEvents[i].StartEvent();
               
            }
        }
    }
}
