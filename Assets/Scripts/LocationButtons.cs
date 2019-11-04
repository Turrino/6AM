using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LocationButtons : MonoBehaviour
{
    public Button TalkBtn;
    public Button LeaveBtn;
    public Button ArrestBtn;

    private bool _dialogueOn;

    public void Start()
    {
        TalkBtn.onClick.AddListener(() => {
            _dialogueOn = !_dialogueOn;
            Master.GM.DialogueManager.ShowDialogue();
            Master.GM.Contents.HideContents();
        });

        LeaveBtn.onClick.AddListener(() => {
            Master.GM.ChangeScene(Master.GM.ScenarioData.Main);
            Master.GM.Contents.HideContents();
        });

        ArrestBtn.onClick.AddListener(() => {
            Master.GM.ArrestAttempt();
            Master.GM.Contents.HideContents();
        });
    }


}
