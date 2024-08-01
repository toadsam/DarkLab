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
        Spawn,
        SpawnAndPlayAnimation,
        MoveShadow,
        FootstepDelay
    }

    public class ScaryStateEffect : ScaryEffect
    {
        [Header("State Settings")]
        public StateType stateType;
        public bool frontCreation = true;
        public bool isDisappearance = true;
        public bool createBigObject = false;
        public float deactiveDelay = 0;
        public AnimationClip[] animationClips;
        public GameObject objectToSpawn;
        public Vector3 targetPosition = Vector3.zero;
        public bool isRelative;
        public LoopType doTweenLoopType = LoopType.Restart;
        public int doTweenLoops = 1;

        private GameObject playerTransform;

        void Awake()
        {

        }

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
                case StateType.Spawn:
                    StartCoroutine(Spawn());
                    break;
                case StateType.SpawnAndPlayAnimation:
                    StartCoroutine(SpawnAndPlayAnimation());
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

        private IEnumerator Spawn()
        {
            var a = targetSource.GetCurrentTarget<Transform>("transform");
            GameObject targetObject = Instantiate(objectToSpawn, a.position, a.rotation);
            
            if(isDisappearance)
            {
                yield return new WaitForSeconds(deactiveDelay);

                targetObject.SetActive(false);
            }
        }

        private IEnumerator SpawnAndPlayAnimation()
        {
            // var a = targetSource.GetCurrentTarget<Transform>("transform");
            Debug.Log("안녕");
            Transform playerTransform = MainManager.Instance.player.transform;
            GameObject targetObject;
            if(frontCreation)
            {
                Camera mainCamera = playerTransform.GetComponentInChildren<Camera>();
                if (mainCamera == null)
                {
                    Debug.LogError("메인 카메라가 타겟 오브젝트에 부착되어 있지 않습니다.");
                    yield break;
                }
                // new Vector3(player.position.x, currentPosition.y, player.position.z) +
                Debug.Log(playerTransform.position);
                Vector3 spawnPosition = new Vector3(playerTransform.forward.x, playerTransform.position.y, playerTransform.forward.z);

                Debug.Log(spawnPosition);

                targetObject = Instantiate(objectToSpawn, spawnPosition, playerTransform.rotation * Quaternion.Euler(0, 180, 0));

                if(createBigObject)
                {
                    targetObject.transform.position -= new Vector3(0, 1.4f, 0);
                }
            }
            else 
            {
                targetObject = Instantiate(objectToSpawn, new Vector3(playerTransform.position.x, playerTransform.position.y + 0.15f, playerTransform.position.z), playerTransform.rotation);
            }

            Animator animator = targetObject.GetComponent<Animator>();

            if (animator != null && animationClips.Length > 0)
            {
                int randomIndex = Random.Range(0, animationClips.Length);
                AnimationClip selectedClip = animationClips[randomIndex];

                animator.Play(selectedClip.name);
            }

            if(isDisappearance)
            {
                yield return new WaitForSeconds(deactiveDelay);

                targetObject.SetActive(false);
            }
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
                .OnComplete(() => targetObject.SetActive(false));
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
