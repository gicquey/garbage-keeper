using System;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Image healthBar;
    public Text timer;

    public float TimeElapsed { get; private set; }

    private void Awake()
    {
        TimeElapsed = 0;
    }

    public void Update()
    {
        healthBar.fillAmount = GameManager.Instance.CurrentLife / Settings.Instance.lifeMax;
        TimeElapsed += Time.deltaTime;
        TimeSpan t = TimeSpan.FromSeconds(TimeElapsed);

        timer.text = string.Format("{0:D2}:{1:D2}:{2:D2}:{3:D2}",
                        t.Hours,
                        t.Minutes,
                        t.Seconds,
                        t.Milliseconds);
    }
}
