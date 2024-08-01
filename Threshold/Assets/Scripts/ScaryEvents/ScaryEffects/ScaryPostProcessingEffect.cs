using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;

namespace ScaryEvents.ScaryEffects
{
    public enum PostProcessingEffectType
    {
        ChromaticAberration,
        CloseEye,
        CameraMove,
        WobbleSpace
    }
    
    public class ScaryPostProcessingEffect : ScaryEffect
    {
        [Header("Post Processing Settings")]
        public PostProcessingEffectType effectType;
        public float finalWeight = 1f;
        public float onTransitionDuration = 1f;
        public float offTransitionDuration = 1f;
        
        public bool useWobbleOnActivate = true;
        public bool useWobbleOnDeactivate = false;
        public float wobbleMinWeight = 0.2f;
        public float wobbleMaxWeight = 0.4f;
        public int wobbleCount = 3;
        public float wobbleDuration = 0.5f;
        
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
            targetVolume.gameObject.SetActive(true);

            if (useWobbleOnActivate)
            {
                yield return StartCoroutine(WobbleEffect(wobbleMinWeight, wobbleMaxWeight, wobbleCount, true));
            }

            // 최종 weight로 부드럽게 전환
            yield return DOTween.To(() => targetVolume.weight, x => targetVolume.weight = x, finalWeight, onTransitionDuration)
                .SetEase(Ease.InOutQuad)
                .WaitForCompletion();

            yield return new WaitForSeconds(duration);

            if (useWobbleOnDeactivate)
            {
                yield return StartCoroutine(WobbleEffect(wobbleMinWeight, finalWeight, wobbleCount, false));
            }

            // 효과 비활성화
            yield return DOTween.To(() => targetVolume.weight, x => targetVolume.weight = x, 0f, offTransitionDuration)
                .SetEase(Ease.InOutQuad)
                .OnComplete(() => {
                    targetVolume.gameObject.SetActive(false);
                    StopEffect();
                })
                .WaitForCompletion();
        }

        private IEnumerator WobbleEffect(float minWeight, float maxWeight, int count, bool finalIncrease)
        {
            for (int i = 0; i < count; i++)
            {
                yield return DOTween.To(() => targetVolume.weight, x => targetVolume.weight = x, maxWeight, wobbleDuration / wobbleCount)
                    .SetEase(Ease.InOutQuad)
                    .WaitForCompletion();
                
                yield return DOTween.To(() => targetVolume.weight, x => targetVolume.weight = x, minWeight, wobbleDuration / wobbleCount)
                    .SetEase(Ease.InOutQuad)
                    .WaitForCompletion();
            }

            if (finalIncrease)
            {
                yield return DOTween.To(() => targetVolume.weight, x => targetVolume.weight = x, maxWeight, wobbleDuration / wobbleCount)
                    .SetEase(Ease.InOutQuad)
                    .WaitForCompletion();
            }
        }
    }
}