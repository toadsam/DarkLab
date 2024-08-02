using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthAndTimer : MonoBehaviour
{
    // UI components
    public Image healthBar;
    public TextMeshProUGUI timerText;
    public Image fadeOverlay;

    public void Awake()
    {
        ProgressChecker.Instance.AssignUIComponents(healthBar, timerText);
        ProgressChecker.Instance.AssignDeathUIComponents(fadeOverlay);
    }
}
