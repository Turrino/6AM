﻿using System.Collections;
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

    int frames = 0;
    bool ringing;
    bool rolloMoving;

    public void Begin()
    {
        PolisSuprText.enabled = false;
        PhoneRing.enabled = true;
        ringing = true;
        Invoke("PhoneReact", 2);
    }

    void PhoneReact()
    {
        Scimatta1.enabled = true;
        Invoke("SciReaction", 0.5f);
    }

    void SciReaction()
    {
        ScimattaBalloon.enabled = true;
        ScimattaText.enabled = true;
        PomaSurp.enabled = false;
        Invoke("PickUpPhone", 1.5f);
    }

    void PickUpPhone()
    {
        ScimattaText.text = "Hello?";
        PhoneHand.enabled = true;
        PhoneRing.enabled = false;
        PhoneOff.enabled = true;
        ringing = false;
        Ring.enabled = false;
        Invoke("PhoneReply", 1.5f);
    }

    void PhoneReply()
    {
        PhoneBalloon.enabled = true;
        PhoneText.enabled = true;
        PhoneOff.enabled = false;
        Invoke("PhoneReply2", 1.5f);
    }

    void PhoneReply2()
    {
        ScimattaText.text = "...";
        PhoneText.text = "Yuo are the Polis now.";
        Scimatta1.enabled = false;
        Scimatta2.enabled = true;
        Invoke("DoorKnock", 1.5f);
    }

    void DoorKnock()
    {
        DoorClosed.enabled = true;
        //TODO knocking sound
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
        ScimattaText.text = "..........";
        Invoke("RolloDelivered", 3f);
    }
    void RolloDelivered()
    {
        PhoneText.text = "He brought you a letter.";
        ScimattaText.text = ".............";
        Invoke("RolloDelivered2", 2f);
    }

    void RolloDelivered2()
    {
        PhoneText.text = "I think you should read it.";
        ScimattaText.text = "................";
        Invoke("ShowLetter", 4f);
    }

    void ShowLetter()
    {
        //TODO letter sound
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
