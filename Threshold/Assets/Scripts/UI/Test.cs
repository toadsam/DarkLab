using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Test : MonoBehaviour
{
    public void gameStart()
    {
        SceneManager.LoadScene("MainScene");
        ProgressChecker.Instance.StartGameSession(); // progress checker 를 활성화 (singleton이라 부르면 활성화됨.)
    }
    
    public void gameEnd()
    {
        Application.Quit();
    }
}