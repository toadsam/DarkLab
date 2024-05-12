using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;

namespace ScaryEvents.ScaryEffects
{
    public enum PostProcessingEffectType
    {
        ChromaticAberration,
        CloseEye
    }
    
    public class ScaryPostProcessingEffect : ScaryEffect
    {
        [Header("Post Processing Settings")]
        public PostProcessingEffectType effectType;
        public float weight = 1f;
        public float onTransitionDuration = 1f;
        public float offTransitionDuration = 1f;
        
        [HideInInspector] public Volume targetVolume;

        public override void StartEffectInternal()
        {
            if (targetVolume == null)
            {
                Debug.LogError("[ScaryPostProcessingEffect] Target Volume is not set for ScaryPostProcessingEffect");
                return;
            }

            effectCoroutines.Add(StartCoroutine(ActivateEffect()));
        }

        private IEnumerator ActivateEffect()
        {
            // Weight를 점진적으로 증가시켜 효과 활성화
            targetVolume.gameObject.SetActive(true);
            DOTween.To(() => targetVolume.weight, x => targetVolume.weight = x, weight, onTransitionDuration).SetEase(Ease.InOutQuad);
            yield return new WaitForSeconds(onTransitionDuration + duration); // 효과가 완전히 활성화된 후 지정된 시간 동안 유지

            // Weight를 점진적으로 감소시켜 효과 비활성화
            DOTween.To(() => targetVolume.weight, x => targetVolume.weight = x, 0f, offTransitionDuration).SetEase(Ease.InOutQuad).OnComplete(StopEffect);
            targetVolume.gameObject.SetActive(false);
        }
        
#if UNITY_EDITOR // Test Code 
        private void Start()
        {
            StartCoroutine(StartEffectForTest());
        }
        
        private IEnumerator StartEffectForTest()
        {
            yield return new WaitForSeconds(3);
            StartEffect();
        }
#endif
    }
}