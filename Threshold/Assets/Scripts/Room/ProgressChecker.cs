using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using UnityEngine.SceneManagement;
using System.Collections;

public class ProgressChecker : Singleton<ProgressChecker>
{
    public int currentProgress = 0;
    
    public float currentHealth = 0;
    public float maxHealth = 3;

    public float currentSeconds = 0;
    public float maxSeconds = 180;

    //Death Animation
    public float fadeDuration = 0.7f;
    public float fallBackSpeed = 1f;
    private Transform cameraTransform;

    // 시간 효과 관련
    private DateTime startTime;
    private TimeSpan gameDuration;
    
    // UI components
    private Image healthBar;
    private TextMeshProUGUI timerText;
    private Image fadeOverlay;
    
    // inner logic
    private bool isGameStarted = false;
    
    public void StartGameSession()
    {
        currentProgress = PlayerPrefs.GetInt("Progress", currentProgress) + 1;
        PlayerPrefs.SetInt("Progress", currentProgress);

        maxHealth = 3 + Mathf.Round(currentProgress / 5f);
        currentHealth = maxHealth;
        
        maxSeconds = 180 + Mathf.Round(currentProgress / 5f) * 60;
        currentSeconds = maxSeconds;
        startTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 0, 0); // 11:00 PM
        gameDuration = TimeSpan.FromHours(7); // 7시간 (11:00 PM to 6:00 AM)
    }
    
    public void AssignUIComponents(Image inputHealthBar, TextMeshProUGUI inputTimerText, Image inputFadeOverlay)
    {
        healthBar = inputHealthBar;
        timerText = inputTimerText;
        fadeOverlay = inputFadeOverlay;
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
        Vector3 backwardOffset = -cameraTransform.forward * 5f;
        Vector3 targetPosition = initialPosition + backwardOffset;
        //Quaternion targetRotation = Quaternion.Euler(cameraTransform.eulerAngles.x - 90f, cameraTransform.eulerAngles.y, cameraTransform.eulerAngles.z);
        Quaternion targetRotation = initialRotation * Quaternion.Euler(-75f, 0, 0);

        yield return new WaitForSeconds(0.3f);

        while (elapsedTime < fallBackSpeed)
        {
            cameraTransform.localPosition = Vector3.Lerp(initialPosition, initialPosition + Vector3.back * 2f, elapsedTime / fallBackSpeed);
            cameraTransform.localRotation = Quaternion.Lerp(initialRotation, targetRotation, elapsedTime / fallBackSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cameraTransform.localPosition = initialPosition + Vector3.back * 2f;
        cameraTransform.localRotation = targetRotation;

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
    }
}
