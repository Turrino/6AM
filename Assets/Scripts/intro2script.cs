using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class intro2script : MonoBehaviour
{
    public Text one;
    public Text two;
    public Text three;
    public Text MainText;

    public SpriteRenderer Cereal;
    public SpriteRenderer Bowl;
    public SpriteRenderer Milk;

    public AudioSource AudioSource;
    public AudioClip[] BreakfastSounds;

    public intro3script intro3;

    public void Begin()
    {
        Invoke("ShowOne", 2);
        Invoke("ShowTwo", 3f);
        Invoke("ShowThree", 4);
        Invoke("Script3", 6);
    }

    public void ShowOne()
    {
        one.enabled = true;
        Cereal.enabled = true;
        AudioSource.clip = BreakfastSounds[0];
        AudioSource.Play();
    }

    public void ShowTwo()
    {
        two.enabled = true;
        Bowl.enabled = true;
        AudioSource.clip = BreakfastSounds[1];
        AudioSource.Play();
    }

    public void ShowThree()
    {
        three.enabled = true;
        Milk.enabled = true;
        AudioSource.clip = BreakfastSounds[2];
        AudioSource.Play();
    }

    public void Script3()
    {
        MainText.enabled = false;
        one.enabled = false;
        two.enabled = false;
        three.enabled = false;
        Cereal.enabled = false;
        Bowl.enabled = false;
        Milk.enabled = false;
        intro3.Begin();
    }
}
