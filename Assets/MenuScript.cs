using Assets.Scripts.Resources;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{

    public GameObject Logo;
    public GameObject LoadingScreen;
    float speed = 1f;
    float amount = 0.003f;

    public void StartGame()
    {
        LoadingScreen.SetActive(true);
        SceneManager.LoadScene(NamesList.MainScenario, LoadSceneMode.Single);
    }

    void Update()
    {
        Logo.transform.position = Logo.transform.position + new Vector3(0, Mathf.Sin(Time.time * speed) * amount, 0);
    }
}
