using Assets.Scripts.Resources;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class intro : MonoBehaviour
{
    public intro2script Intro2;
    public intro4script skipTo4;
    public Text SleepText;
    public SpriteRenderer clock1;
    public SpriteRenderer clock2;
    public SpriteRenderer hand;
    public SpriteRenderer ScimattaSleep;
    public GameObject Text1;
    bool shakeclock;
    float speed = 50f; //how fast it shakes
    float amount = 0.03f; //how much it shakes
    bool debugskip = false;


    void Start()
    {        
        if (debugskip)
        {
            skipTo4.Begin();
        }
        else
        {
            ScimattaSleep.enabled = true;
            InvokeRepeating("Zzz", 0, 0.5f);
            Invoke("AlarmOff", 2);
            Invoke("Alarm", 4);
        }
    }

    void Zzz()
    {
        if (SleepText.text.Length < 3)
            SleepText.text += "z";
        else
            SleepText.text = "";
    }

    void AlarmOff()
    {
        clock1.enabled = true;
    }

    void Alarm()
    {
        clock1.enabled = false;
        clock2.enabled = true;
        shakeclock = true;
        SleepText.enabled = false;
        Invoke("Hand", 1);
    }

    void Hand()
    {
        hand.enabled = true;
    }

    public void Skip()
    {
        SceneManager.LoadScene(NamesList.MainScenario, LoadSceneMode.Single);
    }

    private void Update()
    {        
        if (hand.enabled && hand.transform.localScale.x < 1.8f)
        {
            hand.transform.localScale = hand.transform.localScale + new Vector3(0.01f, 0, 0);
            hand.transform.position = hand.transform.position - new Vector3(0.03f, 0, 0);
            if (hand.transform.localScale.x >= 1.8f) {
                clock2.enabled = false;
                hand.enabled = false;
                ScimattaSleep.enabled = false;
                Text1.SetActive(true);
                Intro2.Begin();
            }
        }
        if (shakeclock)
        {
            clock2.transform.position = clock2.transform.position + new Vector3(0, Mathf.Sin(Time.time * speed) * amount, 0);
        }
    }
}
