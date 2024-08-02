using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrandmaLaugh : MonoBehaviour
{
    private Transform playerTransform;
    public float triggerDistance = 1f;
    public float lookAtDuration = 1f;
    public float moveToPlayerDuration = 1f;
    public Vector3 offsetFromPlayer = new Vector3(0, 0, 1f);
    public AudioClip laughSound;
    public AnimationClip laughAnimation;
    private AudioSource audioSource;
    private Animator animator;
    private bool hasTriggered = false;

    private void Start()
    {
        playerTransform = MainManager.Instance.player.transform;
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = laughSound;

        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!hasTriggered)
        {
            float distance = Vector3.Distance(playerTransform.position, transform.position);
            if (distance <= triggerDistance)
            {
                StartCoroutine(LookAtPlayerAndLaugh());
                hasTriggered = true;
            }
        }
    }

    IEnumerator LookAtPlayerAndLaugh()
    {
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

        float elapsedTime = 0;
        Quaternion initialRotation = transform.rotation;

        while (elapsedTime < lookAtDuration)
        {
            transform.rotation = Quaternion.Slerp(initialRotation, lookRotation, (elapsedTime / lookAtDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = lookRotation;

        // 플레이어 앞으로 이동
        Vector3 targetPosition = playerTransform.position + playerTransform.forward * offsetFromPlayer.z;
        elapsedTime = 0;
        Vector3 startPosition = transform.position;

        while (elapsedTime < moveToPlayerDuration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, (elapsedTime / moveToPlayerDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;

        // 웃는 애니메이션 재생
        if (animator != null && laughAnimation != null)
        {
            animator.Play(laughAnimation.name);
        }

        if (audioSource != null && laughSound != null)
        {
            audioSource.Play();
            yield return new WaitForSeconds(3f);
            this.gameObject.SetActive(false);
        }
    }
}
