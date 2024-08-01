using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace ScaryEvents.ScaryEffects
{
    public enum StateType
    {
        None,
        Active,
        Deactive,
        SpawnAndDestroy,
        MoveShadow,
        FootstepDelay
    }

    public class ScaryStateEffect : ScaryEffect
    {
        [Header("State Settings")]
        public StateType stateType;
        public GameObject objectToSpawn;
        public Vector3 targetPosition = Vector3.zero;
        public bool isRelative;
        public LoopType doTweenLoopType = LoopType.Restart;
        public int doTweenLoops = 1;

        public override void StartEffectInternal()
        {
            switch (stateType)
            {
                case StateType.Active:
                    Active();
                    break;
                case StateType.Deactive:
                    StartCoroutine(Deactive());
                    break;
                case StateType.SpawnAndDestroy:
                    StartCoroutine(SpawnAndDestroy());
                    break;
                case StateType.MoveShadow:
                    ShadowMove();
                    break;
                case StateType.FootstepDelay:
                    //FootstepDelay();
                    break;
            }
        }

        private void Active()
        {
            GameObject a = targetSource.GetCurrentTarget<Transform>("transform").gameObject;

            a.SetActive(true);
            
            DelayAndStopEffect();
        }

        private IEnumerator Deactive()
        {
            GameObject a = targetSource.GetCurrentTarget<Transform>("transform").gameObject;

            yield return new WaitForSeconds(delay);

            a.SetActive(false);

            StopEffect();
        }

        private IEnumerator SpawnAndDestroy()
        {
            var a = targetSource.GetCurrentTarget<Transform>("transform");
            GameObject targetObject = Instantiate(objectToSpawn, a.position, a.rotation);
            
            yield return new WaitForSeconds(delay);

            Destroy(targetObject);
        }

        private void ShadowMove()
        {
            var a = targetSource.GetCurrentTarget<Transform>("transform");
            Vector3 spawnPosition = new Vector3(a.position.x, a.position.y - 0.5f, a.position.z);
            GameObject targetObject = Instantiate(objectToSpawn, spawnPosition, a.rotation);

            var b = targetObject.transform;
            b.DOMove(new Vector3(targetPosition.x,targetPosition.y,targetPosition.z), duration)
                .SetEase(ease)
                .SetRelative(isRelative)
                .SetLoops(doTweenLoops, doTweenLoopType)
                .OnComplete(() => Destroy(targetObject));
        }

        // private void FootstepDelay()
        // {
        //     var footstepManager = inputObject.GetComponent<FootstepAudioManager>();
        //     if (footstepManager != null)
        //     {
        //         footstepManager.SetDelayMode(true);
        //         StartCoroutine(StopFootstepDelayAfterDuration(footstepManager));
        //     }
        // }

        // private IEnumerator StopFootstepDelayAfterDuration(FootstepAudioManager footstepManager)
        // {
        //     yield return new WaitForSeconds(duration);
        //     footstepManager.SetDelayMode(false);
        //     StopEffect();
        // }
    }
}
