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

    public void Setup()
    {
        LeaveBtn.gameObject.SetActive(true);
        TalkBtn.onClick.RemoveAllListeners();
        LeaveBtn.onClick.RemoveAllListeners();
        ArrestBtn.onClick.RemoveAllListeners();

        TalkBtn.onClick.AddListener(() => {
            _dialogueOn = !_dialogueOn;
            Master.GM.DialogueManager.ShowDialogue();
            Master.GM.Contents.HideContents();
        });

        LeaveBtn.onClick.AddListener(() => {
            Master.GM.ChangeScene(Master.GM.ScenarioData.Map);
            Master.GM.Contents.HideContents();
        });

        ArrestBtn.onClick.AddListener(() => {
            Master.GM.ArrestAttempt();
            Master.GM.Contents.HideContents();
        });
    }

    public void Start()
    {
        Setup();
    }

    public void FinalScene()
    {
        TalkBtn.onClick.RemoveAllListeners();
        LeaveBtn.onClick.RemoveAllListeners();
        ArrestBtn.onClick.RemoveAllListeners();

        TalkBtn.onClick.AddListener(() => {
            _dialogueOn = !_dialogueOn;
            Master.GM.DialogueManager.FinalSceneDialogue();
            Master.GM.Contents.HideContents();
        });

        LeaveBtn.gameObject.SetActive(false);
        //LeaveBtn.onClick.AddListener(() => {
        //    Master.GM.DialogueManager.FinalSceneLeave();
        //    Master.GM.Contents.HideContents();
        //});

        ArrestBtn.onClick.AddListener(() => {
            Master.GM.DialogueManager.FinalSceneArrest();
            Master.GM.Contents.HideContents();
        });

    }


}
