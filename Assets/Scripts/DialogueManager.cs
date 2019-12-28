using Assets.Scripts.Resources;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    public GameObject DialogueScreen;
    public GameObject MenuScreen;
    public Image DialogueIcon;
    public Text DialogueText;
    public bool DialogueEnabled;
    public GameObject DialogueFace;
    public ClockScript Clock;
    public LocationButtons LocButtons;

    // This is the name of the location at the top left
    public Image InfoBox;
    public Text InfoText;
    public bool InfoEnabled;

    public Button DiagButton1;
    public Button DiagButton2;
    public Text DiagButton1Text;
    public Text DiagButton2Text;

    // Actions menu
    public Button MenuButton1;
    public Button MenuButton2;
    public Text MenuButton1Text;
    public Text MenuButton2Text;
    public Image MenuBgImage;
    public Text MenuText;
    public bool MenuEnabled;

    public GameObject TotalTimeCounter;

    public GameObject InfoboxLarge;
    public Text InfoboxText;

    private IEnumerator InfoDisplayCr;

    private Color32 SuccessClr = new Color32(90, 200, 84, 255); 
    private Color32 FailClr = new Color32(244, 85, 110, 255);

    bool CanArrestMayo;
    public void Setup()
    {
        InfoboxLarge.SetActive(false);
        LocButtons.Setup();
    }

    public void ShowDialogue(string text = null)
    {
        Master.GM.HidePoma();
        Master.GM.Clock.DialogueMode(true);
        DialogueFace.GetComponent<Image>().sprite = Master.GM.CurrentLocation.Person.DialogueImg;
        DialogueText.text = text ?? Master.GM.CurrentLocation.Person.DialogueLine;
        DialogueFunction();
        TriggerDialogue(true);
    }

    public void FinalSceneSetup()
    {
        FinalDialogueIdx = 0;
        LocButtons.FinalScene();
    }

    private int FinalDialogueIdx;
    private string[] FinalDialogueSequence = new string[] {
        "Why are you looking at me like I stole something?",
        "Have you got nothing better to do!?",
        "Whoops, did my mask fall off?"
    };

    public void FinalSceneDialogue()
    {
        ShowDialogue(FinalDialogueSequence[FinalDialogueIdx]);

        DialogueFace.GetComponent<Image>().sprite = FinalDialogueIdx == 2 ? Master.GM.FinalScript.MayoNoMask : Master.GM.FinalScript.MayoWithMask;

        if (FinalDialogueIdx == 2)
        {
            Master.GM.FinalScript.LocationSpriteNoMask();
            CanArrestMayo = true;
        }
        else
        {
            FinalDialogueIdx++;
        }

        DialogueText.fontSize = 50;
    }

    private Queue<string> OutroStory = new Queue<string>(new string[]
    {
        $"Well, well, well!{Environment.NewLine}What a story.",
        "The fiendish Sbarlo was posing as Mayor Mayo and pocketing all the cookies in Biscotja.",
        "Sbarlo sent the Polis Manual to Scimatta thinking the crime would never be solved.",
        "Boy oh boy, you showed them, didn't you.",
        "Now, where might the actual Mayor Mayo be?",
        "No one really knows.",
        "To be honest, no one really cares.",
        $"Why should anyone?{Environment.NewLine}It's time to celebrate the REAL hero of this story..."
    });

    public void FinalSceneArrest()
    {
        if (!CanArrestMayo)
        {
            ShowDialogue("YOU CAN'T ARREST ME.");
            DialogueText.fontSize = 58;
            DialogueFace.GetComponent<Image>().sprite = Master.GM.FinalScript.MayoWithMask;
        }
        else
        {
            ShowDialogue("Oh snap.");
            DialogueText.fontSize = 58;
            DialogueFace.GetComponent<Image>().sprite = Master.GM.FinalScript.MayoNoMask;
            DiagButton1Text.text = "Continue";
            DiagButton1.onClick.RemoveAllListeners();
            DiagButton1.onClick.AddListener(() => {
                Master.GM.MayoIsCaughtMusic();
                CloseDialogue();
                Master.GM.HidePoma();
                InfoboxText.text = OutroStory.Dequeue();
                InfoboxLarge.SetActive(true);
                MenuEnabled = true;
                MenuScreen.SetActive(true);
                MenuButton1Text.text = "Continue";
                MenuButton2.gameObject.SetActive(false);

                MenuButton1.onClick.RemoveAllListeners();
                MenuButton1.onClick.AddListener(() =>
                {
                    InfoboxText.text = OutroStory.Dequeue();
                    if (OutroStory.Count == 0)
                    {
                        MenuButton1.onClick.RemoveAllListeners();
                        MenuButton1.onClick.AddListener(() =>
                        {
                            Master.GM.ToOutro();
                            SceneManager.LoadScene(NamesList.Outro, LoadSceneMode.Single);
                        });
                    }
                });
            });
            TriggerButtonOne(true);

        }
    }

    public void CaptureMenu(DiagButtonsFunction function, string message)
    {
        Master.GM.HidePoma();
        MenuText.text = message;
        EnableButtons(function);
    }

    public void CloseDialogue()
    {
        DisableButtons();
        Master.GM.Clock.DialogueMode(false);
        TriggerDialogue(false);
    }
    
    public void ShowInfo(string text, float displayTime = 0)
    {
        InfoText.text = text;
        TriggerInfo(true);
        if (displayTime > 0)
        {
            InfoDisplayCr = InfoFadeout(displayTime);
            StartCoroutine(InfoDisplayCr);
        }
    }

    public void HideInfo()
    {
        TriggerInfo(false);
    }

    public void EnableButtons(DiagButtonsFunction function)
    {
        switch (function)
        {
            case DiagButtonsFunction.ArrestSuccess:
                CaptureSuccessFunction();
                break;
            case DiagButtonsFunction.ArrestFailed:
                CaptureFailedFunction();
                break;
            default:
                break;
        }
    }

    public void DialogueFunction()
    {
        DiagButton1Text.text = "Continue";
        DiagButton1.onClick.RemoveAllListeners();
        DiagButton1.onClick.AddListener(() => {
            CloseDialogue();
        });
        TriggerButtonOne(true);
    }

    public void CaptureFailedFunction(bool failedColor = true)
    {
        MenuBgImage.color = failedColor? FailClr : SuccessClr;
        EnableMenuScreen();

        MenuButton1Text.text = "Restart";
        MenuButton2Text.text = "Ragequit";

        MenuButton1.onClick.RemoveAllListeners();
        MenuButton1.onClick.AddListener(() =>
        {
            DisableMenuScreen();
            Master.GM.RestartButton();
        });
        MenuButton2.onClick.RemoveAllListeners();
        MenuButton2.onClick.AddListener(() =>
        {
            Master.GM.QuitButton();
        });
        MenuButton2.gameObject.SetActive(true);
    }

    public void GameEnd()
    {
        MenuBgImage.color = SuccessClr;
        MenuText.text = $"You did it!!! The world is now a safer place for cookies." +
            $"{Environment.NewLine}This is as far the 6AM Alpha version goes.";
        EnableMenuScreen();
        CaptureFailedFunction(false);
        MenuButton2Text.text = "Quit";
    }

    void Update()
    {
        if (AnimateTotalTimeWidget)
        {
            var aPos = TotalTimeCounter.transform.position;
            aPos.x = aPos.z = 0;
            aPos.y = 10 * Time.deltaTime;
            TotalTimeCounter.transform.position -= aPos;
            if (TotalTimeCounter.transform.position.y < 2.05f)
            {
                AnimateTotalTimeWidget = false;
                Master.GM.Sfx.Success();
            }
        }
    }

    private bool AnimateTotalTimeWidget;

    public void CaptureSuccessFunction()
    {
        TotalTimeCounter.transform.position = new Vector2(3.66f,5.3f);
        TotalTimeCounter.transform.Find("totaltime")
            .GetComponent<Text>().text = Master.GM.TotalTimeSpent.ToString(@"mm\:ss");

        TotalTimeCounter.SetActive(true);
        AnimateTotalTimeWidget = true;

        MenuBgImage.color = SuccessClr;
        EnableMenuScreen();
        MenuButton1Text.text = "Next!";
        MenuButton2.gameObject.SetActive(false);

        MenuButton1.onClick.RemoveAllListeners();
        MenuButton1.onClick.AddListener(() =>
        {
            DisableMenuScreen();
            Master.GM.NextLevel();
        });
    }
    private void TriggerDialogue(bool on)
    {
        DialogueScreen.SetActive(on);
        //DialogueIcon.enabled = on;
        //DialogueText.enabled = on;
        DialogueEnabled = on;
    }


    void EnableMenuScreen()
    {
        MenuEnabled = true;
        Clock.StopTimer();
        MenuScreen.SetActive(true);
    }

    void DisableMenuScreen()
    {
        // no need to re-enable the timer, master will do that when restarting the level
        MenuEnabled = false;
        TotalTimeCounter.SetActive(false);
        MenuScreen.SetActive(false);
    }

    public void DisableButtons()
    {
        TriggerButtons(false);
    }

    public void TriggerButtonOne(bool on)
    {
        DiagButton1.gameObject.SetActive(on);
        DiagButton2.gameObject.SetActive(false);
        return;
    }

    public void TriggerButtons(bool on)
    {
        DiagButton1.gameObject.SetActive(on);
        DiagButton2.gameObject.SetActive(on);
        return;
    }

    private IEnumerator InfoFadeout(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            // TODO how to fade out? cba for now 
            //InfoBox.CrossFadeAlpha(1, 1, false);
            //InfoText.CrossFadeAlpha(1, 1, false);
            TriggerInfo(false);
            StopCoroutine(InfoDisplayCr);
        }
    }

    public void TriggerInfo(bool on)
    {
        InfoBox.enabled = on;
        InfoText.enabled = on;
        InfoEnabled = on;
    }

    public enum DiagButtonsFunction
    {
        ArrestSuccess,
        ArrestFailed
    }
}
