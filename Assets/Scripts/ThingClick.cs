using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThingClick : MonoBehaviour
{
    void OnMouseDown()
    {
        Master.GM.Contents.HideContents();
    }
}
