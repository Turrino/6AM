using Assets.Scripts.Resources;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{

    public GameObject Logo;
    public GameObject LoadingScreen;
    float speed = 1f;
    float amount = 0.003f;
    bool playintro = true;
    public Text DifficultyBtnText;
    public Text DifficultyDescriptionText;
    private int DifficultyIdx = 1;

    private int LevelChangeFlag = int.MaxValue;
    private Action LevelChange;
    private string[] Difficulties = new string[] {
        @"<color=""#460C23"">Difficulty:</color> Easy-Peasy",
        @"<color=""#460C23"">Difficulty:</color> El Normal",
        @"<color=""#460C23"">Difficulty:</color> Hurd" };

    private string[] DifficultiesDesc = new string[] {
        $"Easy mode.{Environment.NewLine}Like normal,{Environment.NewLine}but can re-try levels.",
        $"Regular Difficulty.{Environment.NewLine}Cannot re-try levels.",
        $"Hurd mode.{Environment.NewLine}Cannot re-try levels + extra challenge." };

    public void StartGame()
    {
        Master.Difficulty = DifficultyIdx;
        LoadingScreen.SetActive(true);
        if (playintro)
        {
            SceneManager.LoadScene(NamesList.Intro, LoadSceneMode.Single);
        }
        else
        {
            LevelChangeFlag = Time.frameCount;

            if (Master.GM != null)
                LevelChange = () => Master.GM.LoadLevel();
            else
                LevelChange = () => SceneManager.LoadScene(NamesList.MainScenario, LoadSceneMode.Single);
        }        
    }

    public void PlayIntroToggle() {
        playintro = !playintro;
    }

    void DisableLoad()
    {
        if (Master.GM != null)
        {
            Master.GM.SetLoadingScreenOff();
        }
    }

    public void ChangeDifficulty()
    {
        DifficultyIdx = DifficultyIdx == 2 ? 0 : DifficultyIdx + 1;

        DifficultyBtnText.text = Difficulties[DifficultyIdx];
        DifficultyDescriptionText.text = DifficultiesDesc[DifficultyIdx];

        Master.Difficulty = DifficultyIdx;
    }

    void Update()
    {
        Invoke("DisableLoad", 0.1f);
        Logo.transform.position = Logo.transform.position + new Vector3(0, Mathf.Sin(Time.time * speed) * amount, 0);

        if (Time.frameCount - LevelChangeFlag > 3)
        {
            LevelChangeFlag = int.MaxValue;
            LevelChange();
        }
    }
}
