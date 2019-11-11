using System;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Image healthBar;
    public Text timer;

    private float _timeElapsed;

    private void Awake()
    {
        _timeElapsed = 0;
    }

    public void Update()
    {
        healthBar.fillAmount = GameManager.Instance.CurrentLife / Settings.Instance.lifeMax;
        _timeElapsed += Time.deltaTime;
        TimeSpan t = TimeSpan.FromSeconds(_timeElapsed);

        timer.text = string.Format("{0:D2}:{1:D2}:{2:D2}:{3:D3}",
                        t.Hours,
                        t.Minutes,
                        t.Seconds,
                        t.Milliseconds);
    }
}
