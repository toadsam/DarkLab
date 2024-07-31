using UnityEngine;
using System.Collections;

public class GrandMaMove : MonoBehaviour
{
    public float moveDistance = 10f;  // 이동할 거리
    public float moveSpeed = 1f;      // 이동 속도
    public float fadeDelay = 2f;      // 투명화 전 지연 시간
    public float fadeDuration = 1f;   // 투명화 시간

    private Vector3 initialPosition;  // 초기 위치
    private Vector3 targetPosition;   // 목표 위치
    private bool moving = false;      // 이동 중 여부
    private Renderer[] childRenderers;

    void Start()
    {
        initialPosition = transform.position;
        targetPosition = -(initialPosition + transform.forward * moveDistance);
        moving = true;  // 시작 시 이동을 활성화

        // 모든 자식의 Renderer 컴포넌트 가져오기
        childRenderers = GetComponentsInChildren<Renderer>();

        // 각 자식의 Material 설정 변경
        foreach (Renderer renderer in childRenderers)
        {
            Material material = renderer.material;
            material.SetOverrideTag("RenderType", "Transparent");
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
            material.DisableKeyword("_ALPHATEST_ON");
            material.DisableKeyword("_ALPHABLEND_ON");
            material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = 3000;
        }
    }

    void Update()
    {
        if (moving)
        {
            // 오브젝트가 목표 위치로 이동
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // 목표 위치에 도달했는지 확인
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                moving = false; // 이동 완료
                StartCoroutine(FadeOut(fadeDelay, fadeDuration)); // 일정 시간 후 투명화 시작
            }
        }
    }

    IEnumerator FadeOut(float delay, float duration)
    {
        yield return new WaitForSeconds(delay); // 투명화 전 지연 시간

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            foreach (Renderer renderer in childRenderers)
            {
                Material material = renderer.material;
                Color originalColor = material.color;
                float alpha = Mathf.Lerp(originalColor.a, 0, elapsedTime / duration);
                material.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            }
            yield return null;
        }

        // 최종적으로 완전히 투명하게 설정
        foreach (Renderer renderer in childRenderers)
        {
            Material material = renderer.material;
            Color originalColor = material.color;
            material.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
        }
    }
}
