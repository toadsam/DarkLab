using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoScaryEvent : MonoBehaviour
{
    public GameObject canvasForVideo;
    public VideoPlayer videoPlayer;
    public VideoClip[] videoClips;
    public float duration = 5.0f;
    public AudioSource audioSource;
    
    // Start is called before the first frame update
    void Start()
    {
        canvasForVideo.SetActive(false);    
    }
    
    public void PlayRandomVideo()
    {
        var randomIndex = Random.Range(0, videoClips.Length);
        videoPlayer.clip = videoClips[randomIndex];
        canvasForVideo.SetActive(true);
        videoPlayer.Play();
        audioSource.Play();
        StartCoroutine(StopVideo());
    }
    
    IEnumerator StopVideo()
    {
        yield return new WaitForSeconds(duration);
        videoPlayer.Stop();
        audioSource.Stop();
        canvasForVideo.SetActive(false);
    }
}
