using System.Collections;
using UnityEngine;

public class ParticleTriggerMovement : MonoBehaviour
{
    public ParticleSystem particleSystem;  // 파티클 시스템 참조
    public GameObject targetObject;  // 활성화하고 움직일 대상 오브젝트
    public float startDelay = 3.0f;  // 파티클 시작 지연 시간
    public float targetYIncrease = 5.0f;  // Y축 증가량
    public float duration = 5.0f;  // Y축 증가 지속 시간

    private Vector3 initialPosition;
    private bool isMoving = false;

    void Start()
    {
        if (targetObject != null)
        {
            // 오브젝트를 비활성화 상태로 시작
            targetObject.SetActive(false);
            initialPosition = targetObject.transform.position;

            // 일정 시간 후에 파티클 시스템 실행 및 이동 시작
            Invoke("StartParticleAndMove", startDelay);
        }
    }

    void StartParticleAndMove()
    {
        if (particleSystem != null && targetObject != null)
        {
            particleSystem.Play();
            targetObject.SetActive(true);
            StartCoroutine(MoveObject());
        }
    }

    IEnumerator MoveObject()
    {
        isMoving = true;
        float elapsedTime = 0f;
        Vector3 targetPosition = new Vector3(initialPosition.x, initialPosition.y + targetYIncrease, initialPosition.z);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            targetObject.transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / duration);
            yield return null;
        }

        // Ensure the object reaches the exact target position
        targetObject.transform.position = targetPosition;
        isMoving = false;
    }
}
