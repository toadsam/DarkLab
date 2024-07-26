using DG.Tweening;
using UnityEngine;
using Utils;

namespace ScaryEvents.ScaryEffects
{
    public enum LightEffectType
    {
        None,
        Flicker,
        ColorChange,
        IntensityChange,
        RangeChange,
        ShadowStrengthChange,
        SpotAngleChange
    }

    public class ScaryLightEffect : ScaryEffect
    {
        [Header("Light Settings")]
        public LightEffectType effectType;

        // Light variables
        public Color targetColor = Color.white;
        public float targetIntensity = 1.0f;
        public float targetRange = 10.0f;
        public float targetSpotAngle = 30.0f;
        public float targetShadowStrength = 1.0f;

        // Flicker specific
        public int flickerCount = 10;
        public float flickerDuration = 0.1f;
        
        public override void StartEffectInternal()
        {
            Light currentLight = targetSource.GetCurrentTarget<Light>("light");
            if (currentLight == null) return;

            switch (effectType)
            {
                case LightEffectType.Flicker:
                    Flicker(currentLight);
                    break;
                case LightEffectType.ColorChange:
                    ColorChange(currentLight);
                    break;
                case LightEffectType.IntensityChange:
                    IntensityChange(currentLight);
                    break;
                case LightEffectType.RangeChange:
                    RangeChange(currentLight);
                    break;
                case LightEffectType.ShadowStrengthChange:
                    ShadowStrengthChange(currentLight);
                    break;
                case LightEffectType.SpotAngleChange:
                    SpotAngleChange(currentLight);
                    break;
                default:
                    break;
            }
            
            DelayAndStopEffect();
        }
        
        #region Light Functions

        private void Flicker(Light inputLight)
        {
            float originalIntensity = inputLight.intensity;
            DOTween.Sequence()
                .Append(DOTween.To(() => inputLight.intensity, x => inputLight.intensity = x, targetIntensity, flickerDuration).SetEase(Ease.Flash))
                .AppendInterval(flickerDuration)
                .SetLoops(flickerCount, LoopType.Yoyo)
                .OnComplete(() => inputLight.intensity = originalIntensity);
        }

        private void ColorChange(Light inputLight)
        {
            inputLight.DOColor(targetColor, duration).SetEase(ease);
        }

        private void IntensityChange(Light inputLight)
        {
            inputLight.DOIntensity(targetIntensity, duration).SetEase(ease);
        }

        private void RangeChange(Light inputLight)
        {
            inputLight.DORange(targetRange, duration).SetEase(ease);
        }

        private void ShadowStrengthChange(Light inputLight)
        {
            inputLight.DOShadowStrength(targetShadowStrength, duration).SetEase(ease);
        }

        private void SpotAngleChange(Light inputLight)
        {
            inputLight.DOSpotAngle(targetSpotAngle, duration).SetEase(ease);
        }
        
        #endregion
    }
}
