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
        Fade,
        WavyTexture,
        MoveMeshVertices,
        MoveAllRoomObjectsUp
    }

    public class ScaryDoTweenEffect : ScaryEffect
    {
        [Header("DoTween Settings")]
        public DoTweenType doTweenType;
        public bool isRelative;
        public LoopType doTweenLoopType = LoopType.Restart;
        public int doTweenLoops = 1;
    
        // DoTween variables
        // targetPosition and targetRotation is based on World Space. local Space �� Relative �� case ���� ����ϸ� ���� ��.
        public Vector3 targetPosition = Vector3.zero;
        public Vector3 targetRotation = Vector3.zero;
        public Vector3 targetScale = Vector3.one;
        public float shakePosition = 1;
        public float amplitude = 0.1f;
        public float frequency = 1.0f;
        public float speed = 1.0f;
        public Material material;
        public float vertexOffset = 0.1f;

        private Material originalMaterial;
        
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
                case DoTweenType.WavyTexture:
                    WavyTexture();
                    break;
                case DoTweenType.MoveMeshVertices:
                    MoveMeshVertices();
                    break;
                case DoTweenType.MoveAllRoomObjectsUp:
                    MoveAllRoomObjectsUp();
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

        public void Shaking()
        {
            var a = targetSource.GetCurrentTarget<Transform>("transform");
            a.DOShakePosition(shakePosition, duration)
                .SetRelative(isRelative)
                .SetLoops(doTweenLoops, doTweenLoopType);;
        }

        public void Fade()
        {
            var renderer = targetSource.GetCurrentTarget<Renderer>("renderer");
            var material = renderer.material;

            Color color = material.color;
            color.a = 0;
            material.color = color;

            material.DOFade(1, duration)
                .SetEase(ease)
                .SetLoops(doTweenLoops, doTweenLoopType);
        }

        public void WavyTexture()
        {
            var material = targetSource.GetCurrentTarget<Renderer>("renderer").material;
            Vector2 originalOffset = material.mainTextureOffset;

            material.DOOffset(new Vector2(originalOffset.x, originalOffset.y + amplitude), frequency)
                .SetEase(Ease.InOutSine)
                .SetLoops(doTweenLoops, doTweenLoopType)
                .OnStepComplete(() => material.mainTextureOffset = originalOffset);
        }

        public void MoveMeshVertices()
        {
            var targetTransform = targetSource.GetCurrentTarget<Transform>("transform");
            var meshFilter = targetTransform.GetComponent<MeshFilter>();
            if (meshFilter == null) return;

            Mesh mesh = meshFilter.mesh;
            Vector3[] originalVertices = mesh.vertices;
            Vector3[] movedVertices = new Vector3[originalVertices.Length];
            float[] randomOffsets = new float[originalVertices.Length];

            // 각 버텍스의 임의의 오프셋 생성
            for (int i = 0; i < originalVertices.Length; i++)
            {
                randomOffsets[i] = Random.Range(0.0f, vertexOffset);
                movedVertices[i] = originalVertices[i] + Vector3.down * randomOffsets[i];
            }

            // 원래 위치로 돌아오는 애니메이션
            Sequence vertexSequence = DOTween.Sequence();
            vertexSequence.Append(DOVirtual.Float(0, 1, duration, value =>
            {
                for (int i = 0; i < originalVertices.Length; i++)
                {
                    movedVertices[i] = Vector3.Lerp(originalVertices[i], originalVertices[i] + Vector3.down * randomOffsets[i], value);
                }
                mesh.vertices = movedVertices;
                mesh.RecalculateBounds();
            }))
            .Append(DOVirtual.Float(1, 0, duration, value =>
            {
                for (int i = 0; i < originalVertices.Length; i++)
                {
                    movedVertices[i] = Vector3.Lerp(originalVertices[i] + Vector3.down * randomOffsets[i], originalVertices[i], value);
                }
                mesh.vertices = movedVertices;
                mesh.RecalculateBounds();
            }))
            .SetEase(ease)
            .SetLoops(doTweenLoops, doTweenLoopType);
        }

        public void MoveAllRoomObjectsUp()
        {
            GameObject[] roomObjects = GameObject.FindGameObjectsWithTag("RoomObject");
            foreach (GameObject parentObject in roomObjects)
            {
                foreach (Transform child in parentObject.transform)
                {
                    child.DOMove(child.position + targetPosition, duration)
                        .SetEase(ease)
                        .SetRelative(isRelative)
                        .SetLoops(doTweenLoops, doTweenLoopType);

                    child.DOShakeRotation(duration, shakePosition)
                        .SetRelative(isRelative)
                        .SetLoops(doTweenLoops, doTweenLoopType);
                }
            }
        }

        #endregion
    
    }
}