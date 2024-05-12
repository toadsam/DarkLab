using DG.Tweening;
using UnityEngine;

namespace ScaryEvents.ScaryEffects
{
    public enum DoTweenType
    {
        None,
        Move,
        Rotate,
        Scale,
        Shake,
        Fade
    }

    public class ScaryDoTweenEffect : ScaryEffect
    {
        [Space(10)] [Header("DoTween Settings")]
        public DoTweenType doTweenType;
        public bool isRelative;
        public LoopType doTweenLoopType = LoopType.Restart;
        public int doTweenLoops = 1;
    
        // DoTween variables
        // targetPosition and targetRotation is based on World Space. local Space 나 Relative 한 case 또한 고려하면 좋을 듯.
        public Vector3 targetPosition = Vector3.zero;
        public Vector3 targetRotation = Vector3.zero;
        public Vector3 targetScale = Vector3.one;
        public float shakePosition = 1;
        
        public override void StartEffectInternal()
        {
            switch (doTweenType)
            {
                case DoTweenType.Move:
                    Position();
                    break;
                case DoTweenType.Rotate:
                    Rotation();
                    break;
                case DoTweenType.Scale:
                    Scale();
                    break;
                case DoTweenType.Shake:
                    Shaking();
                    break;
                case DoTweenType.Fade:
                    Fade();
                    break;
            }
            
            DelayAndStopEffect();
        }

        #region Dotween Functions
    
        public void Position()
        {
            var a = targetSource.GetCurrentTarget<Transform>("transform");
            a.DOMove(new Vector3(targetPosition.x,targetPosition.y,targetPosition.z), duration)
                .SetEase(ease)
                .SetRelative(isRelative)
                .SetLoops(doTweenLoops, doTweenLoopType);
        }

        public void Rotation()
        {
            var a = targetSource.GetCurrentTarget<Transform>("transform");
            a.DORotate(new Vector3(targetRotation.x,targetRotation.y,targetRotation.z), duration, RotateMode.FastBeyond360)
                .SetEase(ease)
                .SetRelative(isRelative)
                .SetLoops(doTweenLoops, doTweenLoopType);
        }

        public void Scale()
        {
            var a = targetSource.GetCurrentTarget<Transform>("transform");
            a.DOScale(new Vector3(targetScale.x, targetScale.y, targetScale.z), duration)
                .SetEase(ease)
                .SetRelative(isRelative)
                .SetLoops(doTweenLoops, doTweenLoopType);
        }

        //우선 위치로 흔들리게 했는데, rotate/scale도 있어서 이건 상의 하면 좋을듯!
        public void Shaking()
        {
            var a = targetSource.GetCurrentTarget<Transform>("transform");
            a.DOShakePosition(shakePosition, duration)
                .SetRelative(isRelative)
                .SetLoops(doTweenLoops, doTweenLoopType);;
        }

        public void Fade()
        {
            //음.. 머테리얼 가져와야하는데,, ObjectInfoHolder에 추가할까,,,?
        }

        #endregion
    
    }
}