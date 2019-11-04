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

    private bool _shown;

    public void Start()
    {
        DrawerRenderer = GetComponent<SpriteRenderer>();
    }

    public void HideContents()
    {
        if (_shown)
        {
            _shown = false;
            foreach (var item in SpawnPoints)
            {
                item.enabled = false;
            }

            DrawerRenderer.enabled = false;
            Xbutton.SetActive(false);
        }
    }

    public void ShowContents(List<Sprite> contents)
    {
        //var spawnPoints = Master.GM.Contents.GetComponentsInChildren<SpriteRenderer>(true);
        //spawnPoints.Where(x => x.sprite.name != "xbutton" && x.sprite.name != "drawer").ToList().PickRandoms(Contents.Count);
        //var contentsAndPoints = spawnPoints.Zip(Contents, (s, c) => new { s, c });
        //foreach (var cs in contentsAndPoints)
        //{
        //    cs.s.sprite = cs.c;
        //    cs.s.enabled = true;
        //}

        SpawnPoints.Shuffle();
        for (int i = 0; i < SpawnPoints.Length; i++)
        {
            if (contents.Count > i)
            {
                SpawnPoints[i].sprite = contents[i];
                SpawnPoints[i].enabled = true;
            }
            else
            {
                SpawnPoints[i].enabled = false;
            }
        }

        DrawerRenderer.enabled = true;
        Xbutton.SetActive(true);
        _shown = true;
    }
}
