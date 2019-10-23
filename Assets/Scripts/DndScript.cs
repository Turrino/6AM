using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DndScript : MonoBehaviour {

    public static List<string> DndRegistry = new List<string>();

    void Awake () {
        if (DndRegistry.Contains(name))
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            DndRegistry.Add(name);
            DontDestroyOnLoad(gameObject);
        }
    }
}
