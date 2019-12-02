using Assets.Scripts.Resources;
using BayeuxBundle.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class ScenarioData {
    public ScenarioData(
        LocationInfo mainMap,
        List<LocationInfo> locations,
        string informationText)
    {
        Locations = locations;
        Initialised = true;
        Main = mainMap;
        LocationsDict = Locations.ToDictionary(
            kv => kv.Person.Name,
            kv => kv);

        PomaText = informationText;
    }

    public bool Initialised = false;
    public List<LocationInfo> Locations;
    public Dictionary<string, LocationInfo> LocationsDict;
    public LocationInfo Main;
    public string PomaText;
}

[Serializable]
public class CharacterInfo
{
    public CharacterInfo(bool isThief, string name,
        ResourceDefinition definition, Sprite sprite, Sprite dialogueImg)
    {
        IsThief = isThief;
        Name = name;
        Definition = definition;
        Sprite = sprite;
        DialogueImg = dialogueImg;
    }

    public bool IsThief;
    public bool Liar;
    /// <summary>
    /// The person name is important, we use it to name the location as well
    /// </summary>
    public string Name;
    public bool DescribedByManual;

    /// <summary>
    /// NOTE: Can only be called after ALL characters have been created,
    /// otherwise it might not be possible to solve the scenario.
    /// </summary>
    public ResourceDefinition Definition; // not currently being used
    public string DialogueLine;
    public Sprite Sprite;
    public Sprite DialogueImg;
}

[Serializable]
public class LocationInfo
{
    public LocationInfo(
        CharacterInfo character,
        LocationAssets locationAssets,
        Vector2 coordsOnMainMap,
        int musicClipIndex)
    {
        MusicClipIndex = musicClipIndex;
        Person = character;
        IsMap = false;
        Assets = locationAssets;
        var resource = locationAssets.BackgroundSprites.FirstOrDefault().texture;
        CoordsOnMainMap = coordsOnMainMap;
        RelevantRules = new List<ManualPart>();
        //SpawnPoint = Assets.OverlayData.Points[NamesList.OverlaySpawn].Single().Anchor(locationAssets.Anchor);
    }

    /// <summary>
    /// Use for main map only
    /// </summary>
    public LocationInfo(LocationAssets locationAssets)
    {
        MusicClipIndex = 0;
        IsMap = true;
        var resource = locationAssets.BackgroundSprites.FirstOrDefault().texture;
        Assets = locationAssets;
        SpawnPoint = Assets.OverlayData.Points[ColorsList.OverlayMapSpawn].Single().Anchor(locationAssets.Anchor);
    }

    public LocationAssets Assets;
    public Vector2 CoordsOnMainMap;
    public Vector2 SpawnPoint; // main map only
    public CharacterInfo Person;
    public bool IsMap;
    public int MusicClipIndex;
    public float MusicPlaybackTime;
    public List<ManualPart> RelevantRules;
}

[Serializable]
public class PaletteInfo
{
    public Dictionary<PixelInfo, PixelInfo> PropsPalette;
    public Dictionary<PixelInfo, PixelInfo> CharacterPalette;
    public Dictionary<PixelInfo, PixelInfo> BackgroundPalette;
}

[Serializable]
public class LocationAssets
{
    public LocationAssets(
        Vector2 spriteAnchor,
        List<Sprite> backgroundSprites,
        List<Sprite> foreGroundSprites,
        Sprite colliderSprite,
        Sprite mainMapSprite,
        Overlay overlayData,
        PaletteInfo paletteInfo)
    {
        Anchor = spriteAnchor;
        BackgroundSprites = backgroundSprites;
        ForegroundSprites = foreGroundSprites;
        ColliderSprite = colliderSprite;
        OverlayData = overlayData;
        PropSpawnPoints = overlayData.Points[ColorsList.OverlayProps]
            .Select(p => p.Anchor(spriteAnchor)).ToList().ShuffledClone();
        WallPropSpawnPoints = overlayData.Points[ColorsList.OverlayWallProps]
            .Select(p => p.Anchor(spriteAnchor)).ToList().ShuffledClone();
        MainMapSprite = mainMapSprite;
        PaletteInfo = paletteInfo;
        Props = new List<PropInfo>();
    }

    /// <summary>
    /// Main map only, TODO: move to another class
    /// </summary>
    public LocationAssets(
        Vector2 spriteAnchor,
        List<Sprite> backgroundSprites,
        List<Sprite> foreGroundSprites,
        Sprite colliderSprite,
        Overlay overlayData)
    {
        Anchor = spriteAnchor;
        BackgroundSprites = backgroundSprites;
        ForegroundSprites = foreGroundSprites;
        ColliderSprite = colliderSprite;
        OverlayData = overlayData;
    }

    public Vector2 Anchor;
    /// <summary>
    /// This refers to the sprite of the building for this location that appears on the main map
    /// </summary>
    public Sprite MainMapSprite;
    public List<Sprite> BackgroundSprites;
    public List<Sprite> ForegroundSprites;
    public Sprite ColliderSprite;
    public List<PropInfo> Props;
    public Queue<Vector2> PropSpawnPoints;
    public Queue<Vector2> WallPropSpawnPoints;
    public Overlay OverlayData;
    public PaletteInfo PaletteInfo;

    public void AddProps(List<PropInfo> props)
    {
        foreach (var item in props)
        {
            var spawnPoints =
                item.Definition.ObjectType == ObjectType.am6painting ?
                WallPropSpawnPoints : PropSpawnPoints;

            item.SpawnPoint = spawnPoints.Dequeue();
            Props.Add(item);
        }
    }
}

[Serializable]
public class PropInfo
{
    public PropInfo(
        Sprite sprite,
        Sprite bgSprite,
        ResourceDefinition definition,
        List<PropInfo> contents = null)
    {
        Sprite = sprite;
        BgSprite = bgSprite;
        Definition = definition;
        Contents = contents ?? new List<PropInfo>();
        Description = !string.IsNullOrEmpty(definition.Description)?
            definition.Description : TextResources.DefinitionToText(definition);
    }

    private PropInfo()
    {
        NoProp = true;
        Contents = new List<PropInfo>();
    }

    public static PropInfo GetNoProp()
    {
        return new PropInfo();
    }

    public bool NoProp;
    public ResourceDefinition Definition;
    public Sprite Sprite;
    public Sprite BgSprite;
    public Vector2 SpawnPoint;
    public List<PropInfo> Contents;
    public string Description;
}

/// <summary>
/// The sentences found in the manual / info text are generated by this class
/// </summary>
[Serializable]
public class ManualPart
{
    private ManualPart()
    {

    }

    public static ManualPart ManualPartAboutObjectType(ObjectType type, bool isLiar, int rank)
    {
        return new ManualPart()
        {
            ObjectType = type,
            AreLiars = isLiar,
            RuleType = RuleType.type,
            ManualLine = TextResources.ManualObjectType(type.ToString(), isLiar),
            Rank = rank
        };
    }

    public static ManualPart ManualPartAboutShape(ObjectType type, Shape shape, bool isLiar, int rank)
    {
        return new ManualPart()
        {
            //HasOrNot = true,
            ObjectType = type,
            Classifier = shape,
            AreLiars = isLiar,
            RuleType = RuleType.shape,
            //Specific = true,
            ManualLine = TextResources.ManualShape(shape.ToString(), type.ToString(), isLiar),
            Rank = rank
        };            
    }

    public static ManualPart ManualPartAboutCabItems(string itemType, bool isLiar, int rank)
    {
        return new ManualPart()
        {
            //HasOrNot = true,
            ItemType = itemType,
            IsCabinetItem = true,
            AreLiars = isLiar,
            RuleType = RuleType.cabinetItem,
            ManualLine = TextResources.ManualObjectType(itemType, isLiar),
            Rank = rank
        };
    }

    //public static ManualPart ManualPartHasOrNot(bool has, ObjectType type, bool isLiar, int rank)
    //{
    //    return new ManualPart()
    //    {
    //        HasOrNot = has,
    //        ObjectType = type,
    //        Classifier = null,
    //        AreLiars = isLiar,
    //        RuleType = RuleType.hasOrnot,
    //        ManualLine = DialogueResources.ManualHasOrNot(has, type.ToString(), isLiar)
    //    };
    //}

    //public bool HasOrNot;
    public ObjectType ObjectType;
    public string ItemType; // Drawer items only. TODO, merge object types and item types
    public bool IsCabinetItem;
    public Enum Classifier;
    public string ManualLine;
    public bool AreLiars;
    public RuleType RuleType;
    //public bool Specific;
    public int Rank;

    public bool Compare(ManualPart other)
    {
        if (other.ObjectType == ObjectType && Equals(other.Classifier, Classifier))
        {
            return true;
        }
        return false;
    }

    // Doesn't seem to get called :? using "Compare" for now
    //public int CompareTo(ManualPart other)
    //{
    //    if (other.ObjectType == ObjectType && Equals(other.Classifier, Classifier))
    //    {
    //        return 0;
    //    }
    //    return ObjectType.CompareTo(other.ObjectType.ToString() + other.Classifier.ToString());
    //}

    public override string ToString()
    {
        return ManualLine;
    }
}

public enum RuleType
{
    shape,
    type,
    cabinetItem,
}

// What happens when someone talks to the character
public enum DialogueType
{    
    Na,
    TalkSelf, // Will talk about self
    TalkOthers // Will talk about others
}