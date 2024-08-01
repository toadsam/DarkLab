using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MovingBook : MonoBehaviour
{
    public Vector3 targetPosition;
    public float duration = 1.0f;
    public Ease ease = Ease.Linear;
    public bool isRelative;
    public LoopType doTweenLoopType = LoopType.Restart;
    public int doTweenLoops = 1;

    public void Moving()
    {
        transform.DOMove(new Vector3(targetPosition.x,targetPosition.y,targetPosition.z), duration)
            .SetEase(ease)
            .SetRelative(isRelative)
            .SetLoops(doTweenLoops, doTweenLoopType);
    }
}