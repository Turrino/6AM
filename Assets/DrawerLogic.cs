using Assets.Scripts.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DrawerLogic : MonoBehaviour
{
    public SpriteRenderer DrawerRenderer;
    public GameObject Xbutton;
    public SpriteRenderer[] SpawnPoints;
    public bool Open;

    public void Start()
    {
        DrawerRenderer = GetComponent<SpriteRenderer>();
    }

    public void HideContents()
    {
        if (Open)
        {
            Open = false;
            foreach (var item in SpawnPoints)
            {
                item.gameObject.SetActive(false);
            }

            DrawerRenderer.enabled = false;
            Xbutton.SetActive(false);
        }
    }

    public void ShowContents(List<PropInfo> contents)
    {
        Master.GM.Sfx.Drawer();

        SpawnPoints.Shuffle();
        for (int i = 0; i < SpawnPoints.Length; i++)
        {
            if (contents.Count > i)
            {
                SpawnPoints[i].sprite = contents[i].Sprite;
                // silly unity GetComponentInChildren finds the parent component too..
                var bg = SpawnPoints[i].gameObject.GetComponentsInChildren<SpriteRenderer>()[1];
                bg.sprite = contents[i].BgSprite;
                // Configure the script... TODO, this would be better off during scenario setup and not here
                var itemScript = SpawnPoints[i].gameObject.GetComponent<PropScript>();
                itemScript.Prop = SpawnPoints[i];
                itemScript.Background = bg;
                itemScript.IsContents = true;
                itemScript.ToolTipText = contents[i].Description;

                SpawnPoints[i].gameObject.AddComponent<PolygonCollider2D>();
                SpawnPoints[i].gameObject.SetActive(true);
            }
        }

        DrawerRenderer.enabled = true;
        Xbutton.SetActive(true);
        Open = true;
    }
}
