using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class WakeUp : MonoBehaviour
{
    [Header("Player")]
    public Transform playerTransform;  // 플레이어의 트랜스폼
    public Transform bedPosition;      // 침대 위치
    public Rigidbody rb;               // 플레이어의 리지드바디
    public Collider cd;
    [Header("Transition Settings")]
    public float transitionTime = 5f;  // 회전 시간

    void Start()
    {
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

        // 플레이어의 회전을 천천히 X축 기준으로 -90도 변경
        StartCoroutine(RotatePlayerX());
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
            cd.enabled = true; // 회전 중 물리적 상호작용 배제
            var rb = cd.AddComponent<Rigidbody>();
            
        }
    }
}
