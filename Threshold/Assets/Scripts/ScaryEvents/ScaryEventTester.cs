using System.Collections;
using System.Collections.Generic;
using ScaryEvents;
using UnityEngine;

public class ScaryEventTester : MonoBehaviour
{
    public ObjectInfoHolder exampleObjectInfoHolder;
    public ScaryEvent testedScaryEvent;
    
    // Start is called before the first frame update
    void Start()
    {
        testedScaryEvent.currentEventTarget = exampleObjectInfoHolder;
        testedScaryEvent.StartEvent();
    }
}
