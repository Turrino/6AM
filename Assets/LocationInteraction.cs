using Assets.Scripts.Resources;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationInteraction : MonoBehaviour
{
    private void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

            if (hit.collider != null && hit.collider.transform.gameObject.tag == NamesList.Container)
            {
                Debug.Log("hit");
            }
        }
    }
}
