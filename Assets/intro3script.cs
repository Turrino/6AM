using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class intro3script : MonoBehaviour
{
    public Text Iwondertext;
    public Text pomasurprise;
    public SpriteRenderer ShakeCereal;
    public SpriteRenderer PomaSurp;
    public intro4script Intro4;

    bool shaking;
    float speed = 30f; //how fast it shakes
    float amount = 0.2f; //how much it shakes

    public void Begin()
    {
        Iwondertext.enabled = true;
        Invoke("ShowCereals", 0.5f);
    }

    void ShowCereals()
    {
        ShakeCereal.enabled = true;
        Invoke("ShakeCereals", 1);
    }

    void ShakeCereals()
    {
        shaking = true;
        Invoke("SurprisePoma", 1.5f);
    }

    void SurprisePoma()
    {
        shaking = false;
        PomaSurp.enabled = true;
        Iwondertext.enabled = false;
        Invoke("PomaText", 1.5f);
    }

    void PomaText()
    {
        ShakeCereal.enabled = false;
        pomasurprise.enabled = true;
        Invoke("BeginIntro4", 2.5f);
    }

    void BeginIntro4()
    {
        Intro4.Begin();
    }


    void Update()
    {
        if (shaking)
        {
            ShakeCereal.transform.position = ShakeCereal.transform.position + new Vector3(0, Mathf.Sin(Time.time * speed) * amount, 0);
        }
    }
}
