using Assets.Scripts.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PropScript : MonoBehaviour
{
    // Set at runtime
    public SpriteRenderer Prop;
    public SpriteRenderer Background;
    public string ToolTipText;
    public bool HasContents;
    public List<Sprite> Contents;

    private int _regularOrder;
    private bool _mouseOver;

    void OnMouseDown()
    {
        if (HasContents)
        {
            Master.GM.Contents.ShowContents(Contents);
        }
    }

    void OnMouseOver()
    {
        if (!_mouseOver)
        {
            // TODO add sound effect
            _regularOrder = Prop.sortingOrder;
            Background.sortingOrder = 999;
            Prop.sortingOrder = 1000;

            Background.enabled = true;

            Master.GM.Tooltip.GetComponentInChildren<Text>().text = ToolTipText;
            Master.GM.Tooltip.SetActive(true);
            _mouseOver = true;
        }
    }

    void OnMouseExit()
    {
        Prop.sortingOrder = _regularOrder;
        Background.enabled = false;
        Master.GM.Tooltip.SetActive(false);
        _mouseOver = false;
    }
}
