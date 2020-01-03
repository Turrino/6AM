using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ClockScript : MonoBehaviour
{
    public Text countdownMinutes;
    public Text countdownSeconds;
    public Text countdownFull;
    private int _secondsTotal;
    private int _secondsLeft = 500;
    public SpriteRenderer Panel;
    public bool Final;

    public void StartTimer(int seconds)
    {
        _secondsTotal = seconds;
        _secondsLeft = seconds;
        StartCoroutine("LoseTime");
        Time.timeScale = 1;
    }

    public void FinalTimer()
    {
        Final = true;
        StopCoroutine("LoseTime");
        countdownMinutes.text = "?";
        countdownSeconds.text = "??";
        countdownFull.text = "??:??";
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

    public TimeSpan GetTimeSpent()
    {
        return TimeSpan.FromSeconds(_secondsTotal - _secondsLeft);
    }

    void Update()
    {
        if (Final)
            return;

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
