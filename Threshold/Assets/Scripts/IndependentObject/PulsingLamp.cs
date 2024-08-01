using UnityEngine;
using System.Collections;

public class PulsingLamp : MonoBehaviour
{
    public Light lampLight;           // 램프의 빛 컴포넌트
    public AudioSource heartbeatSound; // 심장박동 소리 오디오 소스

    public float pulseSpeed = 1f;     // 고동치는 속도
    public float maxIntensity = 3f;   // 최대 빛 강도
    public float minIntensity = 0.5f; // 최소 빛 강도
    public Color normalColor = Color.white; // 기본 빛 색상
    public Color pulsingColor = Color.red;  // 고동칠 때의 빛 색상
    public float colorChangeSpeed = 2f; // 색상 변화 속도
    public float shakeAmount = 0.1f;   // 빛과 오브젝트의 위치 흔들림 정도
    public float shakeSpeed = 20f;     // 빛과 오브젝트의 흔들림 속도

    private Vector3 originalPosition;
    private Vector3 originalRotation;
    private Vector3 originalLightPosition;
    private Vector3 originalLightRotation;

    void Start()
    {
        if (lampLight == null)
        {
            lampLight = GetComponentInChildren<Light>();
        }
        originalPosition = transform.localPosition;
        originalRotation = transform.localEulerAngles;
        originalLightPosition = lampLight.transform.localPosition;
        originalLightRotation = lampLight.transform.localEulerAngles;
        StartCoroutine(HeartbeatEffect());
    }

    void Update()
    {
        // 빛의 고동 효과
        float intensity = Mathf.Lerp(minIntensity, maxIntensity, Mathf.PingPong(Time.time * pulseSpeed, 1));
        lampLight.intensity = intensity;

        // 빛의 색상 변화 효과
        float t = Mathf.PingPong(Time.time * colorChangeSpeed, 1);
        lampLight.color = Color.Lerp(normalColor, pulsingColor, t);

        // 빛의 위치와 회전 흔들림 효과
        Vector3 shakePosition = originalLightPosition + Random.insideUnitSphere * shakeAmount;
        lampLight.transform.localPosition = Vector3.Lerp(lampLight.transform.localPosition, shakePosition, Time.deltaTime * shakeSpeed);

        Vector3 shakeRotation = originalLightRotation + new Vector3(Random.Range(-shakeAmount, shakeAmount), Random.Range(-shakeAmount, shakeAmount), 0);
        lampLight.transform.localEulerAngles = Vector3.Lerp(lampLight.transform.localEulerAngles, shakeRotation, Time.deltaTime * shakeSpeed);

        // 오브젝트의 위치와 회전 흔들림 효과
        Vector3 objShakePosition = originalPosition + Random.insideUnitSphere * shakeAmount;
        transform.localPosition = Vector3.Lerp(transform.localPosition, objShakePosition, Time.deltaTime * shakeSpeed);

        Vector3 objShakeRotation = originalRotation + new Vector3(Random.Range(-shakeAmount, shakeAmount), Random.Range(-shakeAmount, shakeAmount), 0);
        transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, objShakeRotation, Time.deltaTime * shakeSpeed);
    }

    IEnumerator HeartbeatEffect()
    {
        while (true)
        {
            if (!heartbeatSound.isPlaying)
            {
                heartbeatSound.Play();
            }
            heartbeatSound.volume = Mathf.Lerp(0, 1, Mathf.PingPong(Time.time * pulseSpeed, 1));
            yield return null;
        }
    }
}
