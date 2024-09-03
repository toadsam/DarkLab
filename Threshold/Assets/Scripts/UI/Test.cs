using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Test : MonoBehaviour
{
    public void gameStart()
    {
        SceneManager.LoadScene("MainScene");
    }
    
    public void gameEnd()
    {
        Application.Quit();
    }

    public void goTitle()
    {
        SceneManager.LoadScene("Title");
    }
}