using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class intro4script : MonoBehaviour
{
    public Text Ring;
    public Text PolisSuprText;
    public SpriteRenderer Scimatta1;
    public SpriteRenderer Scimatta2;
    public SpriteRenderer Scimatta3;
    public SpriteRenderer SciNameTag;
    public SpriteRenderer PomaSurp;
    public SpriteRenderer PhoneRing;
    public SpriteRenderer PhoneOff;
    public SpriteRenderer PhoneHand;
    public SpriteRenderer DoorClosed;
    public SpriteRenderer DoorOpen;
    public SpriteRenderer DoorOverlay;
    public SpriteRenderer RolloWLetter;
    public SpriteRenderer Rollo;
    public SpriteRenderer LetterOnTheGround;
    public GameObject RolloObj;
    public Text ScimattaText;
    public Image ScimattaBalloon;
    public Text PhoneText;
    public Image PhoneBalloon;
    public GameObject LetterObj;
    public GameObject SkipButton;
    public AudioSource AudioSrc;
    public AudioSource BgMusic;
    public AudioClip PhoneRingtone;
    public AudioClip VoiceSfx1;
    public AudioClip VoiceSfx2;
    public AudioClip KnockSfx;
    public AudioClip LetterSfx;

    int frames = 0;
    bool ringing;
    bool rolloMoving;

    private void Voice1()
    {
        AudioSrc.clip = VoiceSfx1;
        AudioSrc.Play();
    }

    private void Voice2()
    {
        AudioSrc.clip = VoiceSfx2;
        AudioSrc.Play();
    }

    public void Begin()
    {
        PolisSuprText.enabled = false;
        PhoneRing.enabled = true;
        ringing = true;
        BgMusic.Stop();
        AudioSrc.clip = PhoneRingtone;
        AudioSrc.Play();
        Invoke("PhoneReact", 2);
    }

    void PhoneReact()
    {
        Scimatta1.enabled = true;
        SciNameTag.enabled = true;
        AudioSrc.Stop();
        BgMusic.Play();
        Invoke("SciReaction", 0.5f);
    }

    void SciReaction()
    {
        Voice1();
        ScimattaBalloon.enabled = true;
        ScimattaText.enabled = true;
        PomaSurp.enabled = false;
        Invoke("PickUpPhone", 1.5f);
    }

    void PickUpPhone()
    {
        ScimattaText.text = "Hello?";
        Voice1();
        PhoneHand.enabled = true;
        PhoneRing.enabled = false;
        PhoneOff.enabled = true;
        ringing = false;
        Ring.enabled = false;
        Invoke("PhoneReply", 1.5f);
    }

    void PhoneReply()
    {
        Voice2();
        PhoneBalloon.enabled = true;
        PhoneText.enabled = true;
        PhoneOff.enabled = false;
        Invoke("PhoneReply2", 1.5f);
    }

    void PhoneReply2()
    {
        ScimattaText.text = "...";
        PhoneText.text = "Yuo are the Polis now.";
        Voice1();
        Scimatta1.enabled = false;
        Scimatta2.enabled = true;
        Invoke("DoorKnock", 1.5f);
    }

    void DoorKnock()
    {
        DoorClosed.enabled = true;
        AudioSrc.clip = KnockSfx;
        AudioSrc.Play();
        Invoke("DoorOpens", 1.5f);
    }

    void DoorOpens()
    {
        DoorClosed.enabled = false;
        DoorOpen.enabled = true;
        DoorOverlay.enabled = true;
        Invoke("RolloEnters", 0.5f);
    }

    void RolloEnters()
    {
        RolloWLetter.enabled = true;
        ScimattaText.text = "......";
        Voice1();
        Scimatta2.enabled = false;
        Scimatta3.enabled = true;
        Invoke("RolloMove", 2f);
    }

    void RolloMove()
    {
        rolloMoving = true;
        Invoke("RolloDeliver", 3f);
    }

    void RolloDeliver()
    {
        rolloMoving = false;
        LetterOnTheGround.enabled = true;
        RolloWLetter.enabled = false;
        Rollo.enabled = true;
        PhoneText.text = "That's Rollo! Your new best friend.";
        Voice2();
        ScimattaText.text = "..........";
        Invoke("RolloDelivered", 3f);
    }
    void RolloDelivered()
    {
        PhoneText.text = "He brought you a letter.";
        Voice2();
        ScimattaText.text = ".............";
        Invoke("RolloDelivered2", 2f);
    }

    void RolloDelivered2()
    {
        PhoneText.text = "I think you should read it.";
        Voice1();
        ScimattaText.text = "................";
        Invoke("ShowLetter", 4f);
    }

    void ShowLetter()
    {
        AudioSrc.clip = LetterSfx;
        AudioSrc.Play();
        SciNameTag.enabled = false;
        LetterOnTheGround.enabled = false;
        DoorOverlay.enabled = false;
        DoorOpen.enabled = false;
        PhoneHand.enabled = false;
        Scimatta3.enabled = false;
        Rollo.enabled = false;
        PhoneText.enabled = false;
        ScimattaText.enabled = false;
        ScimattaBalloon.enabled = false;
        PhoneBalloon.enabled = false;
        SkipButton.SetActive(false);
        LetterObj.SetActive(true);
    }

    void Update()
    {
        frames++;
        if (ringing && frames % 5 == 0)
        {
            Ring.enabled = !Ring.enabled;
        }

        if (rolloMoving)
        {
            RolloObj.transform.position = Vector2.MoveTowards(RolloObj.transform.position, new Vector2(3, 0.6f), 3 * Time.deltaTime);
        }
    }
}
