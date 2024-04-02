using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class ScaryEffect : MonoBehaviour
{
    public ScaryEvent targetSource;
    public float duration;
    public float delay;
    public Ease ease;
    public int loop;
    public UnityEvent onStart;
    public UnityEvent onPlay;
    public UnityEvent onUpdate;
    public UnityEvent onComplete;

    private void Start()
    {
        // targetSource = this.GetComponent<ScaryEvent>();
    }

    public void StartEffect()
    {
        onStart.Invoke();
        onPlay.Invoke();
        onUpdate.Invoke();
        onComplete.Invoke();
    }
}