using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    public GameObject DialogueScreen;
    public GameObject MenuScreen;
    public Image DialogueIcon;
    public Text DialogueText;
    public bool DialogueEnabled;
    public GameObject DialogueFace;
    public ClockScript Clock;

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

    private IEnumerator InfoDisplayCr;

    private Color32 SuccessClr = new Color32(90, 200, 84, 255); 
    private Color32 FailClr = new Color32(244, 85, 110, 255); 

    public void ShowDialogue()
    {
        Master.GM.HidePoma();
        DialogueFace.GetComponent<Image>().sprite = Master.GM.CurrentLocation.Person.DialogueImg;
        DialogueText.text = Master.GM.CurrentLocation.Person.DialogueLine;
        DialogueFunction();
        TriggerDialogue(true);
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
        MenuText.text = "Only two levels exist at the moment :(";
        EnableMenuScreen();
        CaptureFailedFunction(false);
        MenuButton2Text.text = "Quit";
    }

    public void CaptureSuccessFunction()
    {
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
