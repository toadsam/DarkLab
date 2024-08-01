using UnityEngine;
using System.Collections;

public class TeleportToPlayer : MonoBehaviour
{
    public float moveDistance = 10f;  // 이동할 거리
    public float moveSpeed = 1f;      // 이동 속도
    public float lookAtDuration = 1f; // 플레이어를 바라보는 시간
    public float disappearDelay = 2f; // 사라지기 전 지연 시간
    public Transform playerTransform; // 플레이어의 위치 참조
    public float teleportOffset = 2f; // 플레이어 앞에서 떨어진 거리
    public float teleportHeightOffset = 0f; // Y축 위치 조절
    public float fadeDuration = 1f;   // 투명화 시간

    public AudioClip moveSound;       // 이동 시 사운드 클립
    public AudioClip teleportSound;   // 순간이동 시 사운드 클립
    private AudioSource audioSource;  // 오디오 소스

    private Vector3 initialPosition;  // 초기 위치
    private Vector3 targetPosition;   // 목표 위치
    private Renderer[] childRenderers;

    void Start()
    {
        initialPosition = transform.position;
        targetPosition = initialPosition + transform.forward * moveDistance;

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
            material.EnableKeyword("_ALPHABLEND_ON");
            material.renderQueue = 3000;
        }

        // AudioSource 컴포넌트 추가
        audioSource = gameObject.AddComponent<AudioSource>();

        StartCoroutine(MoveAndTeleport());
    }

    IEnumerator MoveAndTeleport()
    {
        // 목표 위치로 이동
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            Vector3 currentPosition = transform.position;
            currentPosition = Vector3.MoveTowards(currentPosition, new Vector3(targetPosition.x, currentPosition.y, targetPosition.z), moveSpeed * Time.deltaTime);
            transform.position = currentPosition;
            yield return null;
        }

        // 플레이어를 바라보기
        yield return new WaitForSeconds(lookAtDuration);

        Vector3 directionToPlayer = playerTransform.position - transform.position;
        directionToPlayer.y = 0; // 수평으로만 회전하게 함
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * 360f);
            yield return null;
        }

        // 순간이동 사운드 재생 (조금 더 일찍 재생)
        PlaySound(teleportSound);

        // 플레이어 앞에 순간이동
        Vector3 teleportPosition = playerTransform.position + playerTransform.forward * teleportOffset;
        teleportPosition.y = playerTransform.position.y + teleportHeightOffset; // Y축 위치 조정
        transform.position = teleportPosition;

        // 사라짐
        yield return StartCoroutine(FadeOutAndDisappear(disappearDelay));
    }


    IEnumerator FadeOutAndDisappear(float delay)
    {
        yield return new WaitForSeconds(delay); // 사라지기 전 지연 시간

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;

            foreach (Renderer renderer in childRenderers)
            {
                Material material = renderer.material;
                Color originalColor = material.color;
                float alpha = Mathf.Lerp(originalColor.a, 0, elapsedTime / fadeDuration);
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

        // 오브젝트를 비활성화하거나 파괴
        gameObject.SetActive(false);
    }

    void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}
