using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;



public class ProgressChecker : Singleton<ProgressChecker>
{
    public int currentProgress = 0;
    
    public float currentHealth = 0;
    public float maxHealth = 3;

    public float currentSeconds = 0;
    public float maxSeconds = 180;
    // 시간 효과 관련
    private DateTime startTime;
    private TimeSpan gameDuration;
    
    // UI components
    private Image healthBar;
    private TextMeshProUGUI timerText;
    
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
    
    public void AssignUIComponents(Image inputHealthBar, TextMeshProUGUI inputTimerText)
    {
        healthBar = inputHealthBar;
        timerText = inputTimerText;
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
        timerText.text = currentGameTime.ToString("hh:mm tt");
    }
    
    // Related to Health
    public void TakeDamage(float damage)
    {
        if (!isGameStarted)
            return;
        
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            GameDone();
        }
        
        healthBar.fillAmount = currentHealth / maxHealth;
    }
    
    public void GameDone()
    {
        isGameStarted = false;
        
        // 게임 종료 Animation 등장 + Title Scene 으로 이동.
    }
}
