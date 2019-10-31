using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropScript : MonoBehaviour
{
    public SpriteRenderer Background;

    void OnMouseOver()
    {
        // TODO add sound effect and description
        Background.enabled = true;
    }

    void OnMouseExit()
    {
        Background.enabled = false;
    }
}
