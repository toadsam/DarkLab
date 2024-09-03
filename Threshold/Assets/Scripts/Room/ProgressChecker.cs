using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using UnityEngine.SceneManagement;
using System.Collections;
using DG.Tweening;
using UnityEngine.Rendering;

public class ProgressChecker : Singleton<ProgressChecker>
{
    public int currentProgress = 0;
    
    public float currentHealth = 0;
    public float maxHealth = 3;

    public float currentSeconds = 0;
    public float maxSeconds = 180;

    //Death Animation
    public float fadeDuration = 0.3f;
    public float fallBackSpeed = 2f;
    private Transform cameraTransform;

    // 시간 효과 관련
    private DateTime startTime;
    private TimeSpan gameDuration;
    
    // UI components
    private Image healthBar;
    private TextMeshProUGUI timerText;
    private Image fadeOverlay;
    private Volume damageVolume;

    // Transition settings
    private float finalWeight = 1f; // 최대 weight 값
    private float onTransitionDuration = 0.5f; // 효과 적용 시간
    private float offTransitionDuration = 0.5f; // 효과 사라지는 시간
    
    // inner logic
    private bool isGameStarted = false;
    
    private DateTime startDate = new DateTime(2015, 9, 1); // 시작 날짜 설정
    
    public void StartGameSession()
    {
        currentProgress = PlayerPrefs.GetInt("Progress", currentProgress) + 1;
        PlayerPrefs.SetInt("Progress", currentProgress);
        Debug.Log("Progress : " + currentProgress);

        maxHealth = 3 + Mathf.Round(currentProgress / 5f);
        currentHealth = maxHealth;
        
        maxSeconds = 180 + Mathf.Round(currentProgress / 5f) * 60;
        currentSeconds = maxSeconds;
        startTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 0, 0); // 11:00 PM
        gameDuration = TimeSpan.FromHours(7); // 7시간 (11:00 PM to 6:00 AM)
    }
    
    // 진행도에 따른 날짜 반환
    public string GetCurrentProgressDate()
    {
        DateTime currentDate = startDate.AddDays(currentProgress - 1);
        return currentDate.ToString("yyyy.MM.dd");
    }
    
    public void AssignUIComponents(Image inputHealthBar, TextMeshProUGUI inputTimerText, Image inputFadeOverlay, Volume inputVolume)
    {
        healthBar = inputHealthBar;
        timerText = inputTimerText;
        fadeOverlay = inputFadeOverlay;
        damageVolume = inputVolume;
        isGameStarted = true;
    }
    
    // Related to Time
    public void Update()
    {
        if (!isGameStarted)
            return;
        
        currentSeconds -= Time.deltaTime;
        if (currentSeconds <= 0)
        {
            currentSeconds = 0;
            GameDone();
        }

        // 경과 시간 계산
        float elapsedRatio = 1 - (currentSeconds / maxSeconds);
        TimeSpan elapsedTime = TimeSpan.FromTicks((long)(gameDuration.Ticks * elapsedRatio));
        DateTime currentGameTime = startTime.Add(elapsedTime);

        // 시간 표시 업데이트
        timerText.text = currentGameTime.ToString("hh:mm tt", System.Globalization.CultureInfo.InvariantCulture);

        cameraTransform = MainManager.Instance.player.transform;
    }
    
    // Related to Health
    public void TakeDamage(float damage)
    {
        if (!isGameStarted)
            return;
        
        currentHealth -= damage;
        healthBar.fillAmount = currentHealth / maxHealth;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            GameDone();
        }
        else
        {
            StartCoroutine(DamageEffect());
        }
    }

    private IEnumerator DamageEffect()
    {
        // Increase weight to show effect
        yield return DOTween.To(() => damageVolume.weight, x => damageVolume.weight = x, finalWeight, onTransitionDuration)
            .SetEase(Ease.InOutQuad)
            .WaitForCompletion();

        // Wait for effect duration
        yield return new WaitForSeconds(1f);

        // Decrease weight to hide effect
        yield return DOTween.To(() => damageVolume.weight, x => damageVolume.weight = x, 0f, offTransitionDuration)
            .SetEase(Ease.InOutQuad)
            .WaitForCompletion();
    }
    
    public void GameDone()
    {
        isGameStarted = false;

        // 게임 종료 Animation 등장 + Title Scene 으로 이동.
        StartCoroutine(GameOverSequence());
    }

    private IEnumerator GameOverSequence()
    {
        float elapsedTime = 0f;
        Vector3 initialPosition = cameraTransform.position;
        Quaternion initialRotation = cameraTransform.rotation;

        // 카메라의 현재 방향을 기준으로 뒤로 넘어지도록 설정
        Vector3 backwardOffset = -cameraTransform.forward * 2f;
        Vector3 targetPosition = initialPosition + backwardOffset;
        //Quaternion targetRotation = Quaternion.Euler(cameraTransform.eulerAngles.x - 90f, cameraTransform.eulerAngles.y, cameraTransform.eulerAngles.z);
        Quaternion targetRotation = initialRotation * Quaternion.Euler(-90f, 0, 0);

        yield return new WaitForSeconds(0.3f);

        while (elapsedTime < fallBackSpeed)
        {
            float t = elapsedTime / fallBackSpeed;

            // 카메라의 위치와 회전 보간
            Vector3 currentPosition = Vector3.Lerp(initialPosition, targetPosition, t);
            Quaternion currentRotation = Quaternion.Lerp(initialRotation, targetRotation, t);

            // 현재 위치와 회전을 적용
            cameraTransform.position = currentPosition;
            cameraTransform.rotation = currentRotation;
            // cameraTransform.localPosition = Vector3.Lerp(initialPosition, initialPosition + Vector3.back * 2f, elapsedTime / fallBackSpeed);
            // cameraTransform.localRotation = Quaternion.Lerp(initialRotation, targetRotation, elapsedTime / fallBackSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // cameraTransform.localPosition = initialPosition + Vector3.back * 2f;
        // cameraTransform.localRotation = targetRotation;

        // 애니메이션이 끝나면 정확한 최종 위치와 회전 설정
        cameraTransform.position = targetPosition;
        cameraTransform.rotation = targetRotation;

        elapsedTime = 0f;
        Color originalColor = fadeOverlay.color;
        Color targetColor = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);

        while (elapsedTime < fadeDuration)
        {
            fadeOverlay.color = Color.Lerp(originalColor, targetColor, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        fadeOverlay.color = targetColor;

        // 타이틀 씬으로 이동
        UnityEngine.SceneManagement.SceneManager.LoadScene("Title");

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
