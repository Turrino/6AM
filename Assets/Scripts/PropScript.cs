using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PropScript : MonoBehaviour
{
    public SpriteRenderer Prop;
    public SpriteRenderer Background;
    public string ToolTipText;

    private int _regularOrder;
    private bool _mouseOver;

    //void OnMouseDown()
    //{
        
    //}

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
