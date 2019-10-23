using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class ResourcesList {
    public static string[] CharacterSpritesList = Directory.GetFiles(Application.dataPath + "/Sprites/Character")
        .Select(n => Path.GetFileName(n).Split('.')[0]).ToArray();
}
