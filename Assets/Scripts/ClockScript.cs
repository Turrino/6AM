using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ClockScript : MonoBehaviour
{
    public Text countdownMinutes;
    public Text countdownSeconds;
    public Text countdownFull;
    private int _secondsLeft = 500;
    public SpriteRenderer Panel;

    public void StartTimer(int seconds)
    {
        _secondsLeft = seconds;
        StartCoroutine("LoseTime");
        Time.timeScale = 1;
    }

    public void StopTimer()
    {
        StopCoroutine("LoseTime");
    }

    public void DialogueMode(bool enabled)
    {
        countdownMinutes.enabled = !enabled;
        countdownSeconds.enabled = !enabled;
        countdownFull.enabled = enabled;
    }

    public void ToggleBadge(bool enabled)
    {
        Panel.enabled = enabled;
    }

    void Update()
    {
        var timeLeft = TimeSpan.FromSeconds(_secondsLeft);
        countdownMinutes.text = timeLeft.Minutes.ToString();
        countdownSeconds.text = timeLeft.Seconds.ToString();
        countdownFull.text = timeLeft.ToString(@"mm\:ss");
        if (_secondsLeft == 0)
        {
            Master.GM.WDCTimeout();
            _secondsLeft = -1;
        }            
    }

    //Simple Coroutine
    IEnumerator LoseTime()
    {
        while (_secondsLeft > 0)
        {
            yield return new WaitForSeconds(1);
            _secondsLeft--;
        }
    }
}
