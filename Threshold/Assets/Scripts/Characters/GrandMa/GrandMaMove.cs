
/*using System.Collections;
using UnityEngine;

public class GrandMaMove : MonoBehaviour
{
    public float moveDistance = 10f;  // 이동할 거리
    public float moveSpeed = 1f;      // 이동 속도
    public float fadeDelay = 2f;      // 투명화 전 지연 시간
    public float fadeDuration = 1f;   // 투명화 시간
    public Transform playerTransform; // 플레이어의 위치 참조
    public float chaseSpeed = 5f;     // 플레이어를 쫓는 속도
    public float rotationSpeed = 360f; // 회전 속도 (도/초)
    public GameObject playerCamera;       // 플레이어의 카메라 참조

    public static bool isGrandEvent;

    public AudioSource audioSource;  // 단일 오디오 소스
    public AudioClip moveSoundClip;  // 이동 시 소리
    public AudioClip chaseSoundClip; // 추격 시 소리
    public float soundDelay = 1f;     // 이동 사운드 재생 지연 시간

    private Vector3 initialPosition;  // 초기 위치
    private Vector3 targetPosition;   // 목표 위치
    private bool moving = false;      // 이동 중 여부
    private Renderer[] childRenderers;

    void Start()
    {
        isGrandEvent = true;
        initialPosition = transform.position;
        targetPosition = initialPosition + transform.forward * moveDistance;
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
            material.EnableKeyword("_ALPHABLEND_ON");
            material.renderQueue = 3000;
        }

        StartCoroutine(PlayMoveSound());
    }

    IEnumerator PlayMoveSound()
    {
        yield return new WaitForSeconds(soundDelay); // 일정 시간 지연 후
        audioSource.clip = moveSoundClip;
        audioSource.Play(); // 이동 사운드 재생
    }

    void Update()
    {
        if (moving)
        {
            // 목표 위치에 도달할 때까지 오브젝트 이동 (Y축 이동 억제)
            Vector3 currentPosition = transform.position;
            currentPosition = Vector3.MoveTowards(currentPosition, new Vector3(targetPosition.x, currentPosition.y, targetPosition.z), moveSpeed * Time.deltaTime);
            transform.position = currentPosition;

            // 플레이어의 카메라가 할머니를 바라보도록 설정
            if (playerCamera != null)
            {
                Vector3 directionToGrandma = transform.position - playerCamera.transform.position;
                Quaternion lookRotation = Quaternion.LookRotation(directionToGrandma);
                playerCamera.transform.rotation = Quaternion.Slerp(playerCamera.transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
            }

            // 목표 위치에 도달했는지 확인
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                isGrandEvent = false;
                moving = false; // 이동 완료
                audioSource.Stop(); // 이동 사운드 정지
                StartCoroutine(FaceAndChasePlayer());
            }
        }
    }

    IEnumerator FaceAndChasePlayer()
    {
        // 잠시 대기 후 플레이어를 바라보도록 회전 (오브젝트의 Z축을 플레이어 방향으로 설정)
        yield return new WaitForSeconds(fadeDelay);

        Vector3 directionToPlayer = playerTransform.position - transform.position;
        directionToPlayer.y = 0; // Y축 회전을 방지하여 수평으로만 회전하게 함
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer); // Z축을 플레이어 쪽으로

        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }

        // 1초 대기 후 추격 사운드 재생
        yield return new WaitForSeconds(1f);
        audioSource.clip = chaseSoundClip;
        audioSource.Play();

        // 플레이어를 향해 이동 및 투명화 시작
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;

            // 플레이어 쪽으로 이동 (Y축 이동 억제)
            Vector3 newPosition = Vector3.MoveTowards(transform.position, new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z), chaseSpeed * Time.deltaTime);
            transform.position = newPosition;

            // 투명화
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
    }
}*/

using System.Collections;
using UnityEngine;

public class GrandMaMove : MonoBehaviour
{
    public float moveDistance = 10f;  // 이동할 거리
    public float moveSpeed = 1f;      // 이동 속도
    public float fadeDelay = 2f;      // 투명화 전 지연 시간
    public float fadeDuration = 1f;   // 투명화 시간
    public Transform playerTransform; // 플레이어의 위치 참조
    public float chaseSpeed = 5f;     // 플레이어를 쫓는 속도
    public float rotationSpeed = 360f; // 회전 속도 (도/초)
    public GameObject playerCamera;   // 플레이어의 카메라 참조

    public static bool isGrandEvent;

    public AudioSource audioSource;  // 단일 오디오 소스
    public AudioClip moveSoundClip;  // 이동 시 소리
    public AudioClip chaseSoundClip; // 추격 시 소리
    public float soundDelay = 1f;    // 이동 사운드 재생 지연 시간

    private Vector3 initialPosition;  // 초기 위치
    private Vector3 targetPosition;   // 목표 위치
    private bool moving = false;      // 이동 중 여부
    private Renderer[] childRenderers;

    void Start()
    {
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
    }

    public void StartGrandmaEvent()
    {
        isGrandEvent = true;
        initialPosition = transform.position;
        targetPosition = initialPosition + transform.forward * moveDistance;
        StartCoroutine(GrandmaEventSequence());
    }

    

    IEnumerator GrandmaEventSequence()
    {
        moving = true;
        yield return StartCoroutine(PlayMoveSound());

        while (moving)
        {
            // 목표 위치에 도달할 때까지 오브젝트 이동 (Y축 이동 억제)
            Vector3 currentPosition = transform.position;
            currentPosition = Vector3.MoveTowards(currentPosition, new Vector3(targetPosition.x, currentPosition.y, targetPosition.z), moveSpeed * Time.deltaTime);
            transform.position = currentPosition;

            // 플레이어의 카메라가 할머니를 바라보도록 설정
            if (playerCamera != null)
            {
                Vector3 directionToGrandma = transform.position - playerCamera.transform.position;
                Quaternion lookRotation = Quaternion.LookRotation(directionToGrandma);
                playerCamera.transform.rotation = Quaternion.Slerp(playerCamera.transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
            }

            // 목표 위치에 도달했는지 확인
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                isGrandEvent = false;
                moving = false; // 이동 완료
                audioSource.Stop(); // 이동 사운드 정지
            }

            yield return null;
        }

        yield return StartCoroutine(FaceAndChasePlayer());
    }

    IEnumerator PlayMoveSound()
    {
        yield return new WaitForSeconds(soundDelay); // 일정 시간 지연 후
        audioSource.clip = moveSoundClip;
        audioSource.Play(); // 이동 사운드 재생
    }

    IEnumerator FaceAndChasePlayer()
    {
        // 잠시 대기 후 플레이어를 바라보도록 회전 (오브젝트의 Z축을 플레이어 방향으로 설정)
        yield return new WaitForSeconds(fadeDelay);

        Vector3 directionToPlayer = playerTransform.position - transform.position;
        directionToPlayer.y = 0; // Y축 회전을 방지하여 수평으로만 회전하게 함
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer); // Z축을 플레이어 쪽으로

        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }

        // 1초 대기 후 추격 사운드 재생
        yield return new WaitForSeconds(1f);
        audioSource.clip = chaseSoundClip;
        audioSource.Play();

        // 플레이어를 향해 이동 및 투명화 시작
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;

            // 플레이어 쪽으로 이동 (Y축 이동 억제)
            Vector3 newPosition = Vector3.MoveTowards(transform.position, new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z), chaseSpeed * Time.deltaTime);
            transform.position = newPosition;

            // 투명화
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
    }
}


