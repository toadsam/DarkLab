

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WakeUp : MonoBehaviour
{
    [Header("Player")]
    public Transform playerTransform;  // 플레이어의 트랜스폼
    public Transform bedPosition;      // 침대 위치
    public Rigidbody rb;               // 플레이어의 리지드바디
    public Collider cd;                // 플레이어의 콜라이더

    [Header("Blinking Effect")]
    public Image blackScreen;          // UI 이미지로 사용되는 블랙스크린
    public float blinkDuration = 0.1f; // 눈 깜빡임 시간
    public int blinkCount = 3;         // 눈 깜빡임 횟수

    [Header("Transition Settings")]
    public float transitionTime = 5f;  // 회전 시간
    public float turningHeadDuration = 2f; // 고개 돌리기 한 방향의 시간
    public int headTurnCount = 2;      // 고개 돌리기 횟수
    public static bool isWakeUp;

    void Start()
    {
        isWakeUp = false;  
        if (rb != null)
        {
            rb.isKinematic = true; // 회전 중 물리적 상호작용 배제
        }
        if (cd != null)
        {
            cd.enabled = false; // 회전 중 물리적 상호작용 배제
        }

        // 플레이어를 침대 위치로 이동
        playerTransform.position = bedPosition.position;
        playerTransform.rotation = bedPosition.rotation;

        // 눈 깜빡임 효과 시작
        StartCoroutine(BlinkAndWakeUp());
    }

    IEnumerator BlinkAndWakeUp()
    {
        yield return StartCoroutine(BlinkScreen());

        // 플레이어의 회전을 천천히 X축 기준으로 -90도 변경
        yield return StartCoroutine(RotatePlayerX());

        // 고개 돌리기 효과
        StartCoroutine(LookAround(headTurnCount));
    }

    IEnumerator BlinkScreen()
    {
        for (int i = 0; i < blinkCount; i++)
        {
            // 깜빡임 시작: 화면을 검게
            yield return FadeScreen(1f);

            // 잠시 대기
            yield return new WaitForSeconds(blinkDuration);

            // 깜빡임 끝: 화면을 원래대로
            yield return FadeScreen(0f);

            // 잠시 대기
            yield return new WaitForSeconds(blinkDuration);
        }
    }

    IEnumerator FadeScreen(float targetAlpha)
    {
        Color currentColor = blackScreen.color;
        float startAlpha = currentColor.a;
        float elapsedTime = 0f;

        while (elapsedTime < blinkDuration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / blinkDuration);
            blackScreen.color = new Color(currentColor.r, currentColor.g, currentColor.b, newAlpha);
            yield return null;
        }

        // 최종적으로 정확한 알파 값 설정
        blackScreen.color = new Color(currentColor.r, currentColor.g, currentColor.b, targetAlpha);
    }

    IEnumerator RotatePlayerX()
    {
        float elapsedTime = 0f;
        Quaternion startRotation = playerTransform.rotation;

        // 목표 회전: 현재 회전에서 X축을 기준으로 -90도 회전한 쿼터니언
        Quaternion targetRotation = Quaternion.Euler(startRotation.eulerAngles.x + 90f, startRotation.eulerAngles.y, startRotation.eulerAngles.z);

        while (elapsedTime < transitionTime)
        {
            elapsedTime += Time.deltaTime;
            Quaternion newRotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / transitionTime);

            if (rb != null)
            {
                rb.MoveRotation(newRotation); // Rigidbody의 MoveRotation을 사용하여 회전
            }
            else
            {
                playerTransform.rotation = newRotation;
            }

            yield return null; // 다음 프레임까지 대기
        }

        // 최종 회전 설정 및 물리적 상호작용 활성화
        if (rb != null)
        {
            rb.MoveRotation(targetRotation);
            rb.velocity = Vector3.zero; // 불필요한 이동 방지
            rb.angularVelocity = Vector3.zero; // 불필요한 회전 방지
            rb.isKinematic = false; // 물리적 상호작용 다시 활성화
        }
        else
        {
            playerTransform.rotation = targetRotation;
        }

        if (cd != null)
        {
            cd.enabled = true; // Collider 다시 활성화
        }

       // isWakeUp = true;
    }

    IEnumerator LookAround(int turns)
    {
        for (int i = 0; i < turns; i++)
        {
            float elapsedTime = 0f;
            Quaternion initialRotation = playerTransform.rotation;

            // 고개를 왼쪽으로 돌리는 목표 회전
            Quaternion leftRotation = Quaternion.Euler(initialRotation.eulerAngles.x, initialRotation.eulerAngles.y - 50f, initialRotation.eulerAngles.z);
            // 고개를 오른쪽으로 돌리는 목표 회전
            Quaternion rightRotation = Quaternion.Euler(initialRotation.eulerAngles.x, initialRotation.eulerAngles.y + 50f, initialRotation.eulerAngles.z);

            // 고개를 왼쪽으로 돌리기
            while (elapsedTime < turningHeadDuration / 2)
            {
                elapsedTime += Time.deltaTime;
                Quaternion newRotation = Quaternion.Slerp(initialRotation, leftRotation, elapsedTime / (turningHeadDuration / 2));
                playerTransform.rotation = newRotation;
                yield return null;
            }

            // 고개를 오른쪽으로 돌리기
            elapsedTime = 0f;
            while (elapsedTime < turningHeadDuration / 2)
            {
                elapsedTime += Time.deltaTime;
                Quaternion newRotation = Quaternion.Slerp(leftRotation, rightRotation, elapsedTime / (turningHeadDuration / 2));
                playerTransform.rotation = newRotation;
                yield return null;
            }

            // 최종적으로 원래 위치로 복귀
            elapsedTime = 0f;
            while (elapsedTime < turningHeadDuration / 2)
            {
                elapsedTime += Time.deltaTime;
                Quaternion newRotation = Quaternion.Slerp(rightRotation, initialRotation, elapsedTime / (turningHeadDuration / 2));
                playerTransform.rotation = newRotation;
                yield return null;
            }
        }
        isWakeUp = true;
    }
}



