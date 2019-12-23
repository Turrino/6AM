using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class intro : MonoBehaviour
{

    public Text SleepText;
    public SpriteRenderer clock1;
    public SpriteRenderer clock2;
    public SpriteRenderer hand;
    public SpriteRenderer ScimattaSleep;
    public GameObject Text1;

    void Start()
    {
        InvokeRepeating("Zzz", 0, 0.5f);
        Invoke("AlarmOff", 3);
        Invoke("Alarm", 5);
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
        SleepText.enabled = false;
        Invoke("Hand", 1);
    }

    void Hand()
    {
        hand.enabled = true;
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
            }
        }
    }


}
