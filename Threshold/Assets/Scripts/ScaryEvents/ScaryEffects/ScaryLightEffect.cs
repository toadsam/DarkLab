using DG.Tweening;
using UnityEngine;

namespace ScaryEvents.ScaryEffects
{
    public enum LightEffectType
    {
        None,
        Flicker,
        ColorChange,
        IntensityChange
    }

    public class ScaryLightEffect : ScaryEffect
    {
        
        public LightEffectType effectType;
        
        // Light variables
        public Color targetColor;
        public float targetIntensity;
        public float targetIndirectMultiplier;
        public LightShadows targetShadowType;
        public bool targetDrawHalo;

        public override void StartEffectInternal()
        {
            switch (effectType)
            {
                case LightEffectType.Flicker:
                    Flicker();
                    break;
                case LightEffectType.ColorChange:
                    ColorChange();
                    break;
                case LightEffectType.IntensityChange:
                    IntensityChange();
                    break;
            }
            
            DelayAndStopEffect();
        }
        
        //Light도 DoTween 이용해서 쉽게 구현할건지
        //아니면 코루틴 이용해서 구현할건지 정해야할 것 같습니다!!!
        //근데 코드가 깔끔한거는 DoTween인 것 같아요,,훗,, => ㅎㅎ 좋은 것 같아요!

        #region Light Functions
        
        public void Flicker()
        {
            var a = targetSource.GetCurrentTarget<Light>("light");
            DOTween.To(() => a.intensity, x => a.intensity = x, targetIntensity, duration)
                .SetEase(Ease.InOutQuad)
                .SetLoops(-1, LoopType.Yoyo)
                .SetDelay(0.5f);
        }

        public void ColorChange()
        {
            var a = targetSource.GetCurrentTarget<Light>("light");
            a.DOColor(targetColor, duration);
        }

        //DoTween 이용
        public void IntensityChange()
        {
            var a = targetSource.GetCurrentTarget<Light>("light");
            a.DOIntensity(targetIntensity, duration)
                .SetEase(Ease.InOutSine);
        }
        
        #endregion
    }
}