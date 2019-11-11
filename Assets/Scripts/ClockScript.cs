using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ClockScript : MonoBehaviour
{
    public Text countdown;
    private int _secondsLeft = 500;

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

    void Update()
    {
        countdown.text = TimeSpan.FromSeconds(_secondsLeft).ToString(@"mm\:ss");
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
