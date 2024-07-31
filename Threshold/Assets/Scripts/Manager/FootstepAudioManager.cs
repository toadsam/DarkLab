using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepAudioManager : MonoBehaviour
{
    public AudioSource footstepAudioSource;
    public AudioClip[] footstepClips;
    public float delayAmount = 0.5f; // 지연 시간 설정
    private bool isDelayed = false;

    public void SetDelayMode(bool shouldDelay)
    {
        isDelayed = shouldDelay;
    }

    public void PlayFootstepSound()
    {
        if (isDelayed)
        {
            StartCoroutine(PlayDelayedFootstep());
        }
        else
        {
            PlayImmediateFootstep();
        }
    }

    private IEnumerator PlayDelayedFootstep()
    {
        yield return new WaitForSeconds(delayAmount);
        PlayImmediateFootstep();
    }

    private void PlayImmediateFootstep()
    {
        if (footstepClips.Length > 0)
        {
            footstepAudioSource.clip = footstepClips[Random.Range(0, footstepClips.Length)];
            footstepAudioSource.Play();
        }
    }
}
