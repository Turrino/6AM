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
    public Settings Set;
    float speed = 1f;
    float amount = 0.003f;
    bool playintro = true;
    public Text DifficultyBtnText;
    public Text DifficultyDescriptionText;
    private int DifficultyIdx = 1;
    private string[] Difficulties = new string[] {
        @"<color=""#460C23"">Difficulty:</color> Easy-Peasy",
        @"<color=""#460C23"">Difficulty:</color> El Normal",
        @"<color=""#460C23"">Difficulty:</color> Hurd" };

    private string[] DifficultiesDesc = new string[] {
        $"Easy mode.{Environment.NewLine}Like normal,{Environment.NewLine}but can re-try levels.",
        $"Regular Difficulty.{Environment.NewLine}Cannot re-try levels.",
        $"Hurd mode.{Environment.NewLine}Extra challenge." };

    //Cannot re-try levels.
    public void StartGame()
    {
        LoadingScreen.SetActive(true);
        SceneManager.LoadScene(playintro? NamesList.Intro : NamesList.MainScenario, LoadSceneMode.Single);
    }

    public void PlayIntroToggle() {
        playintro = !playintro;
    }

    public void ChangeDifficulty()
    {
        DifficultyIdx = DifficultyIdx == 2 ? 0 : DifficultyIdx + 1;

        DifficultyBtnText.text = Difficulties[DifficultyIdx];
        DifficultyDescriptionText.text = DifficultiesDesc[DifficultyIdx];

        Set.DifficultySetting = DifficultyIdx;
    }

    void Update()
    {
        Logo.transform.position = Logo.transform.position + new Vector3(0, Mathf.Sin(Time.time * speed) * amount, 0);
    }
}
