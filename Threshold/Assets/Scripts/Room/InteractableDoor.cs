using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableDoor : MonoBehaviour
{
    public bool isLocked = true;
    public UnityEvent onDoorOpen;
    public UnityEvent onDoorClose;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isLocked)
            {
                onDoorOpen.Invoke();
            }
            else
            {
                Debug.Log("The door is locked.");
                onDoorClose.Invoke();
            }
        }
    }
}
