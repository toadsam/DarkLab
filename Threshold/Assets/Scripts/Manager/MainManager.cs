using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum scaryEventTier { Low, Medium, High };
public enum scaryEventWhen { OnAwake, OnProximity, OnViewInteractionStart, OnFocusInteractionStart, OnSustainedFocusInteraction };
public class MainManager : MonoBehaviour
{

    public scaryEventWhen PlayerEventWhen;
    public static MainManager Instance { get; private set; }

  //  public AudioManager audioManager;
    //public UIManager uiManager;
    //public GameManager gameManager;
    public ObjectEventHandler objectEventHandler;

    private void Awake()
    {
        // ½Ì±ÛÅæ ÆÐÅÏ ±¸Çö
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
