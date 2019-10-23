using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ClockScript : MonoBehaviour
{
    public Text countdown;
    private int _secondsLeft = 60;

    public void StartTimer(int seconds)
    {
        _secondsLeft = seconds;
        StartCoroutine("LoseTime");
        Time.timeScale = 1;
    }

    void Update()
    {
        countdown.text = TimeSpan.FromSeconds(_secondsLeft).ToString(@"mm\:ss");
    }
    //Simple Coroutine
    IEnumerator LoseTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            _secondsLeft--;
        }
    }
}
