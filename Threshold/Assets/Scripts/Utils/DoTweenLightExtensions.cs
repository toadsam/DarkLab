using DG.Tweening;
using UnityEngine;

namespace Utils
{
    public static class DoTweenLightExtensions
    {
        // Range 조절을 위한 확장 메서드
        public static Tweener DORange(this Light light, float endValue, float duration)
        {
            return DOTween.To(() => light.range, x => light.range = x, endValue, duration);
        }

        // Spot angle 조절을 위한 확장 메서드
        public static Tweener DOSpotAngle(this Light light, float endValue, float duration)
        {
            return DOTween.To(() => light.spotAngle, x => light.spotAngle = x, endValue, duration);
        }
    }
}