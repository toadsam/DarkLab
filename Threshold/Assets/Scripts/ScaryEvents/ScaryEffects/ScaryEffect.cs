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
        protected List<Coroutine> effectCoroutines = new List<Coroutine>(); // lifecycle �� ���� �����Ǵ� coroutine �̹Ƿ�, sub class ������ ��� ����.

        // Inspector UI
        [HideInInspector] public bool showProperties = false;
        protected bool eventDone = false;
        
        #region Effect Functions
    
        /// <summary>
        /// StartEffect will be invoked by ScaryEvent.
        /// </summary>
        public void StartEffect()
        {
            eventDone = false;
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
        /// ��� ���� Ŭ���������� �� Effects�� ���� ��, �� �Լ��� ������ ȣ���ϰų�.
        /// Effects�� duration �� �ð��� ���� �� �����Ű�� ���� ���, DelayAndStopEffect �� ȣ���ؾ� ��.
        /// </summary>
        public virtual void StopEffect()
        {
            if (eventDone) return;
            
            if (loops > loopCount)
            {
                loopCount++;
                StartEffectInternalWrapper();
            } 
            else
            {
                isUpdating = false;
                onComplete.Invoke();
                eventDone = true;

                if (effectCoroutines != null)
                {
                    foreach (var effectCoroutine in effectCoroutines)
                    {
                        StopCoroutine(effectCoroutine);
                    }
                }
            }
        }
    
        /// <summary>
        /// Effects�� duration �� �ð��� ���� �� �����ų �� ����. effectCoroutines �� Coroutine �� �Ű澲�� �ʰ� ����� �� �ֵ��� �ϱ� ���� wrapper.
        /// </summary>
        /// <returns></returns>
        protected void DelayAndStopEffect()
        {
            effectCoroutines.Add(StartCoroutine(DelayByDuration()));
        }
    
        /// <summary>
        /// Effects�� duration �� �ð��� ���� �� �����ų �� ����. effectCoroutines �� Coroutine �� �Ű澲�� �ʰ� ����� �� �ֵ��� �ϱ� ���� wrapper.
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

        #endregion
    }
}