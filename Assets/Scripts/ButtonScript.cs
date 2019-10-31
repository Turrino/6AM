using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        Master.GM.PomaButtonOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Master.GM.PomaButtonOver = false;
    }
}
