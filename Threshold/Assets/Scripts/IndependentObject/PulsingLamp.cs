using UnityEngine;
using System.Collections;

public class PulsingLamp : MonoBehaviour
{
    public Light lampLight;
    public AudioSource heartbeatSound;
    public AudioClip heartbeatClip;   // 새로 추가된 AudioClip 변수

    public float pulseSpeed = 1f;
    public float maxIntensity = 3f;
    public float minIntensity = 0.5f;
    public Color normalColor = Color.white;
    public Color pulsingColor = Color.red;
    public float colorChangeSpeed = 2f;
    public float shakeAmount = 0.1f;
    public float shakeSpeed = 20f;

    public float eventDuration = 5f; // 효과 지속 시간

    private Vector3 originalPosition;
    private Vector3 originalRotation;
    private Vector3 originalLightPosition;
    private Vector3 originalLightRotation;

    private Coroutine eventCoroutine;

    void Start()
    {
        if (lampLight == null)
        {
            lampLight = GetComponentInChildren<Light>();
        }

        // AudioSource에 AudioClip 할당
        if (heartbeatSound != null && heartbeatClip != null)
        {
            heartbeatSound.clip = heartbeatClip;
        }

        originalPosition = transform.localPosition;
        originalRotation = transform.localEulerAngles;
        originalLightPosition = lampLight.transform.localPosition;
        originalLightRotation = lampLight.transform.localEulerAngles;
    }

    void Update()
    {
        // 여기에 아무것도 필요하지 않습니다.
    }

    public void StartEvent()
    {
        if (eventCoroutine == null)
        {
            // 이벤트 시작 시 음악이 재생 중이지 않으면 재생
            if (!heartbeatSound.isPlaying && heartbeatSound.clip != null)
            {
                heartbeatSound.Play();
            }
            eventCoroutine = StartCoroutine(HeartbeatEffect(eventDuration));
        }
    }

    IEnumerator HeartbeatEffect(float duration)
    {
        float startTime = Time.time;
        float endTime = startTime + duration;

        while (Time.time < endTime)
        {
            float intensity = Mathf.Lerp(minIntensity, maxIntensity, Mathf.PingPong(Time.time * pulseSpeed, 1));
            lampLight.intensity = intensity;

            float t = Mathf.PingPong(Time.time * colorChangeSpeed, 1);
            lampLight.color = Color.Lerp(normalColor, pulsingColor, t);

            Vector3 shakePosition = originalLightPosition + Random.insideUnitSphere * shakeAmount;
            lampLight.transform.localPosition = Vector3.Lerp(lampLight.transform.localPosition, shakePosition, Time.deltaTime * shakeSpeed);

            Vector3 shakeRotation = originalLightRotation + new Vector3(Random.Range(-shakeAmount, shakeAmount), Random.Range(-shakeAmount, shakeAmount), 0);
            lampLight.transform.localEulerAngles = Vector3.Lerp(lampLight.transform.localEulerAngles, shakeRotation, Time.deltaTime * shakeSpeed);

            Vector3 objShakePosition = originalPosition + Random.insideUnitSphere * shakeAmount;
            transform.localPosition = Vector3.Lerp(transform.localPosition, objShakePosition, Time.deltaTime * shakeSpeed);

            Vector3 objShakeRotation = originalRotation + new Vector3(Random.Range(-shakeAmount, shakeAmount), Random.Range(-shakeAmount, shakeAmount), 0);
            transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, objShakeRotation, Time.deltaTime * shakeSpeed);

            // 심장 박동 소리 볼륨을 깜빡임과 동기화
            heartbeatSound.volume = Mathf.Lerp(0, 1, Mathf.PingPong(Time.time * pulseSpeed, 1));

            yield return null;
        }

        // 효과를 중지하고 원래 상태로 복귀
        lampLight.intensity = minIntensity;
        lampLight.color = normalColor;
        lampLight.transform.localPosition = originalLightPosition;
        lampLight.transform.localEulerAngles = originalLightRotation;
        transform.localPosition = originalPosition;
        transform.localEulerAngles = originalRotation;

        // 효과가 끝날 때 음악 정지
        heartbeatSound.Stop();

        eventCoroutine = null; // 코루틴이 끝났음을 표시
    }
}
