using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PropScript : MonoBehaviour
{
    // Set at runtime
    public SpriteRenderer Prop;
    public SpriteRenderer Background;
    public string ToolTipText;
    public bool HasContents;
    public bool IsContents;
    public List<PropInfo> Contents;

    private int _regularOrder;
    private int _regularOrderBg;
    private bool _mouseOver;

    void OnMouseDown()
    {
        if (HasContents && Master.GM.TooltipEnabled())
        {
            Master.GM.Contents.ShowContents(Contents);
        }
    }

    bool IsToolTipEnabled() => IsContents? Master.GM.DrawerTooltipEnabled() : Master.GM.TooltipEnabled();

    void OnMouseOver()
    {
        if (!_mouseOver && IsToolTipEnabled())
        {
            Master.GM.Sfx.Tooltip();
            _regularOrder = Prop.sortingOrder;
            _regularOrderBg = Background.sortingOrder;
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
        Background.sortingOrder = _regularOrderBg;
        Background.enabled = false;
        Master.GM.Tooltip.SetActive(false);
        _mouseOver = false;
    }
}
