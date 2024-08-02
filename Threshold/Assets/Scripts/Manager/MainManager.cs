using System;
using System.Collections;
using System.Collections.Generic;
using ScaryEvents;
using UnityEngine;

[Flags]
public enum scaryEventTier { None = 0, Tier1 = 1, Tier2 = 2, Tier3 = 4, Tier4 = 8, Tier5 = 16 };
public class MainManager : MonoBehaviour
{
    public static MainManager Instance { get; private set; }

  //  public AudioManager audioManager;
    //public UIManager uiManager;
    //public GameManager gameManager;
    public ObjectEventHandler objectEventHandler;
    public GameObject player;
    public Vector3 resetPos;
    public RandomObjectSelector randomObjectSelector;
    public InteractableDoor interactableDoor;

    private void Awake()
    {
        resetPos = player.transform.position;
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(string soundName)
    {
        //audioManager.PlaySound(soundName);
    }

    public void UpdateScore(int score)
    {
        //uiManager.UpdateScore(score);
    }

    public void GetScore()
    {
        //gameManager.GetSocore();
    }
}
