using System.Collections;
using UnityEngine;

public class BedEvent : MonoBehaviour
{
    public Transform childObject;      // 자식 오브젝트
    public Transform parentObject;     // 부모 오브젝트
    public Transform specialObject;    // 특정 오브젝트
    public AudioSource audioSource;    // 단일 오디오 소스
    public AudioClip shakeClip;        // 흔들림 소리 클립
    public AudioClip specialClip;      // 특별 오브젝트 소리 클립
    public float shakeAmount = 0.1f;   // 흔들림의 정도
    public float shakeSpeed = 5f;      // 흔들림 속도
    public float shakeDuration = 1f;   // 흔들림 지속 시간
    public float shakeInterval = 2f;   // 흔들림 주기
    public float rotationSpeed = 90f;  // 부모 오브젝트의 회전 속도
    public float targetRotationZ = 90f; // 목표 회전각 (Z축)
    public float specialObjectDelay = 1f; // 특정 오브젝트가 나타나는 지연 시간
    public bool triggerEvent = false;  // 이벤트를 실행할 조건 변수

    private Vector3 originalChildPosition;
    private Quaternion originalParentRotation; // 부모 오브젝트의 원래 회전
    private bool rotationComplete = false; // 회전 완료 여부 체크
    private bool shaking = true; // 자식 오브젝트가 흔들리고 있는지 여부

    private void Start()
    {
        originalChildPosition = childObject.localPosition;
        originalParentRotation = parentObject.localRotation;
        StartCoroutine(ShakeRoutine());
    }

    private void Update()
    {
        if (triggerEvent)
        {
            if (!rotationComplete)
            {
                // 부모 오브젝트의 회전
                Quaternion targetRotation = Quaternion.Euler(parentObject.localEulerAngles.x, parentObject.localEulerAngles.y, targetRotationZ);
                parentObject.localRotation = Quaternion.RotateTowards(parentObject.localRotation, targetRotation, rotationSpeed * Time.deltaTime);

                // 목표 회전에 도달했는지 체크
                if (Quaternion.Angle(parentObject.localRotation, targetRotation) < 0.1f)
                {
                    rotationComplete = true; // 회전 완료 표시
                    shaking = false; // 흔들림 중지
                    StartCoroutine(ActivateSpecialObject());
                }
            }
        }
    }

    IEnumerator ShakeRoutine()
    {
        while (shaking)
        {
            // 일정 시간 대기
            yield return new WaitForSeconds(shakeInterval);

            // 흔들림 효과 시작
            float elapsedTime = 0f;
            audioSource.clip = shakeClip;
            audioSource.Play();

            while (elapsedTime < shakeDuration)
            {
                elapsedTime += Time.deltaTime;
                Vector3 shakePosition = originalChildPosition + Random.insideUnitSphere * shakeAmount;
                childObject.localPosition = Vector3.Lerp(childObject.localPosition, shakePosition, Time.deltaTime * shakeSpeed);
                yield return null;
            }

            // 흔들림 효과 종료
            audioSource.Stop();

            // 원래 위치로 복귀
            childObject.localPosition = originalChildPosition;
        }
    }

    IEnumerator ActivateSpecialObject()
    {
        yield return new WaitForSeconds(specialObjectDelay); // 특정 오브젝트 활성화 전 지연 시간

        if (specialObject != null && !specialObject.gameObject.activeSelf)
        {
            specialObject.gameObject.SetActive(true);
            audioSource.clip = specialClip;
            audioSource.Play();
        }

        // 효과 종료 후 원래 상태로 복귀
        yield return new WaitForSeconds(specialObjectDelay);
        ResetEvent();
    }

    private void ResetEvent()
    {
        // 부모 오브젝트 회전을 원래대로
        parentObject.localRotation = originalParentRotation;

        // 자식 오브젝트 위치 초기화
        childObject.localPosition = originalChildPosition;

        // 사운드 정지
        audioSource.Stop();

        // 특별 오브젝트 비활성화
        if (specialObject != null)
        {
            specialObject.gameObject.SetActive(false);
        }

        // 상태 초기화
        rotationComplete = false;
        shaking = true;
        triggerEvent = false;
    }
}
