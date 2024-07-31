using UnityEngine;

public class WaterToBloodEffect : MonoBehaviour
{
    public ParticleSystem waterToBloodParticles;
    public Material wallMaterial;
    public float transitionTime = 5.0f;
    private float elapsedTime = 0.0f;
    private ParticleSystem.MainModule mainModule;
    private ParticleSystem.EmissionModule emissionModule;
    private ParticleSystem.ColorOverLifetimeModule colorModule;
    private Gradient grad;

    void Start()
    {
        mainModule = waterToBloodParticles.main;
        emissionModule = waterToBloodParticles.emission;
        colorModule = waterToBloodParticles.colorOverLifetime;

        // 초기 물 색상 설정
        grad = new Gradient();
        grad.SetKeys(new GradientColorKey[]
        {
            new GradientColorKey(new Color(0.5f, 0.5f, 1f, 1f), 0.0f),
            new GradientColorKey(Color.red, 1.0f)
        }, new GradientAlphaKey[]
        {
            new GradientAlphaKey(1.0f, 0.0f),
            new GradientAlphaKey(1.0f, 1.0f)
        });
        colorModule.color = grad;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime < transitionTime)
        {
            float t = elapsedTime / transitionTime;

            // 파티클 크기, 속도, 수명, 방출 속도 증가
            mainModule.startSize = Mathf.Lerp(0.05f, 0.3f, t);
            mainModule.startSpeed = Mathf.Lerp(1f, 5f, t);
            mainModule.startLifetime = Mathf.Lerp(0.5f, 3f, t);
            emissionModule.rateOverTime = Mathf.Lerp(20f, 100f, t);

            // 벽 텍스처의 색상 변화
            wallMaterial.SetColor("_FlowColor", Color.Lerp(new Color(0.5f, 0.5f, 1f, 1f), Color.red, t));
        }
    }

    public void StartLeakChange()
    {
        // 스크립트를 사용하여 물방울이 피로 변하는 이벤트를 시작
        elapsedTime = 0.0f;
    }
}
