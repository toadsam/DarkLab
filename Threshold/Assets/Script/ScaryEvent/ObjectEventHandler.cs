using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectEventHandler : MonoBehaviour
{
    public List<ScaryEvent> scaryEvents;

    private void Start()
    {
        scaryEvents = new List<ScaryEvent>();
        Debug.Log(transform.childCount);
        for (int i = 0; i < transform.childCount; i++)
        {
            scaryEvents.Add(transform.GetChild(i).GetComponent<ScaryEvent>());
        }
        //처음 이벤트 시작
    }

    public void Match(ObjectInfoHolder objectInfoHolder, scaryEventWhen eventWhen)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (objectInfoHolder.ObjectTier == scaryEvents[i].scaryEventTier && eventWhen == scaryEvents[i].scaryEventWhen)
            {
                scaryEvents[i].currentEventTarget = objectInfoHolder;
            }
        }



    }


}
