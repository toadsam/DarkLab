using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace ScaryEvents.ScaryEffects
{
    public abstract class ScaryEffect : MonoBehaviour
    {
        [HideInInspector] public ScaryEvent targetSource;
    
        // animation settings
        public float duration = 1;
        public float delay = 0;
        public Ease ease = Ease.Linear;
        public int loops = 1;
        private int loopCount = 1;
    
        // events
        public UnityEvent onStart;
        public UnityEvent onPlay;
        public UnityEvent onUpdate;
        public UnityEvent onComplete;
    
        // internal logics
        private bool isUpdating = false;
        protected List<Coroutine> effectCoroutines; // lifecycle 에 의해 관리되는 coroutine 이므로, sub class 에서도 사용 가능.

        // Inspector UI
        [HideInInspector] public bool showProperties = false;
        
        #region Effect Functions
    
        /// <summary>
        /// StartEffect will be invoked by ScaryEvent.
        /// </summary>
        public void StartEffect()
        {
            loopCount = 1;
            effectCoroutines.Add(StartCoroutine(StartEffectCoroutine()));
        }
    
        private IEnumerator StartEffectCoroutine()
        {
            yield return new WaitForSeconds(delay);
            StartEffectInternalWrapper();
        }
    
        private void StartEffectInternalWrapper()
        {
            if (loopCount == 1)
                onStart.Invoke();
            isUpdating = true;
            onPlay.Invoke();
            StartEffectInternal();
        }
    
        public abstract void StartEffectInternal();
    
        /// <summary>
        /// 상속 받은 클래스에서는 각 Effects가 끝난 후, 이 함수를 무조건 호출하거나.
        /// Effects를 duration 의 시간이 지난 후 종료시키고 싶은 경우, DelayAndStopEffect 를 호출해야 함.
        /// </summary>
        public void StopEffect()
        {
            if (loops > loopCount)
            {
                loopCount++;
                StartEffectInternalWrapper();
            } 
            else
            {
                isUpdating = false;
                onComplete.Invoke();

                foreach (var effectCoroutine in effectCoroutines)
                {
                    StopCoroutine(effectCoroutine);
                }
            }
        }
    
        /// <summary>
        /// Effects를 duration 의 시간이 지난 후 종료시킬 수 있음. effectCoroutines 와 Coroutine 을 신경쓰지 않고 사용할 수 있도록 하기 위한 wrapper.
        /// </summary>
        /// <returns></returns>
        protected void DelayAndStopEffect()
        {
            effectCoroutines.Add(StartCoroutine(DelayByDuration()));
        }
    
        /// <summary>
        /// Effects를 duration 의 시간이 지난 후 종료시킬 수 있음. effectCoroutines 와 Coroutine 을 신경쓰지 않고 사용할 수 있도록 하기 위한 wrapper.
        /// </summary>
        /// <returns></returns>
        protected void DelayAndStopEffect(float inputDuration)
        {
            effectCoroutines.Add(StartCoroutine(DelayByDuration(inputDuration)));
        }
    
        private IEnumerator DelayByDuration()
        {
            yield return new WaitForSeconds(duration);
            StopEffect();
        }
    
        private IEnumerator DelayByDuration(float inputDuration)
        {
            yield return new WaitForSeconds(inputDuration);
            StopEffect();
        }

        #endregion

        #region Lifecycle Event Functions
    
        private void Awake()
        {
            targetSource = transform.parent.GetComponent<ScaryEvent>();
        }

        public void Update()
        {
            if (isUpdating)
            {
                onUpdate.Invoke();
            }
        }
    
        public void OnDestroy()
        {
            StopEffect();
        }
    
        public void OnDisable()
        {
            StopEffect();
        }

        #endregion
    }
}