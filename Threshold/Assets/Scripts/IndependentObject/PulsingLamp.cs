using UnityEngine;
using System.Collections;

public class PulsingLamp : MonoBehaviour
{
    public Light lampLight;
    public AudioSource heartbeatSound;

    public float pulseSpeed = 1f;
    public float maxIntensity = 3f;
    public float minIntensity = 0.5f;
    public Color normalColor = Color.white;
    public Color pulsingColor = Color.red;
    public float colorChangeSpeed = 2f;
    public float shakeAmount = 0.1f;
    public float shakeSpeed = 20f;

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
