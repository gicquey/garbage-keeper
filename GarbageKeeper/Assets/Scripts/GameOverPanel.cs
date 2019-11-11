using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameOverPanel : MonoBehaviour
{
    public Text time;

    public void Show()
    {
        this.gameObject.SetActive(true);
        GameManager.Instance.mainScene.hud.gameObject.SetActive(false);

        TimeSpan t = TimeSpan.FromSeconds(GameManager.Instance.mainScene.hud.TimeElapsed);
        time.text = string.Format("{0:D2}h:{1:D2}m:{2:D2}s:{3:D2}ms",
                t.Hours,
                t.Minutes,
                t.Seconds,
                t.Milliseconds);
    }

    public void Restart()
    {
        SceneManager.LoadScene("MapScene");
        GameManager.Instance.Initialize();
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}