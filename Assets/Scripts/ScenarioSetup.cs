using Assets.Scripts.Resources;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Random = UnityEngine.Random;
using BayeuxBundle.Models;
using Assets.Scripts.ScenarioLogic;
using Assets.Scripts.Bayeux;
using BayeuxBundle;
using BayeuxDemo2Assets;
using Assets.Scripts.Generators;
using Newtonsoft.Json;

public class ScenarioSetup
{
    public ScenarioSetup(int noOfMusicClips)
    {
        TextAsset jsonObj = Resources.Load("resources_fwY") as TextAsset;
        //var test1 = @"{""MainType"":""am6vase""}";
        //var test2 = @"[{""MainType"":""am6vase"",""MainAnchor"":{""X"":75,""Y"":84}}, {""MainType"":""am6vase"",""MainAnchor"":{""X"":75,""Y"":84}}]";
        //var pt = JsonConvert.DeserializeObject<Dictionary<string, string>>(test2);
        //Debug.Log(pt.Keys.FirstOrDefault());
        //Debug.Log(pt.Values.FirstOrDefault());
        //var parsed = JsonConvert.DeserializeObject<Dictionary<string, ParsedMetaPoco>>(jsonObj.text);
        //var test = JsonUtility.FromJson<List<ParsedMeta>>(test2);
        var parsed = TinyJson.FromJson<Dictionary<string, ParsedMeta>>(jsonObj.text);
        //var parsed = JsonUtility.FromJson<Dictionary<string, ParsedMetaPoco>>(jsonObj.text);
        Debug.Log(parsed.Values.FirstOrDefault(v => v.MainType == "am6body").Overlay.Points.FirstOrDefault().Key);
        TextureTools = new TextureTools(OverlayRef.Am6RefDictWHash);
        Assembler = new Assembler<Texture2D>(TextureTools, Demo2ResourcesAssembly.BundleAssembly, null);
        TypesInfo.CabinetItemTypes = Assembler.GetAllSubtypes(ObjectType.am6item);
        // Skip 0, that's for the map        
        AvailableMusicClips = Enumerable.Range(1, noOfMusicClips - 1).ToList();
        // This is valid for demo2 only. If re-using this code this probably will change.
        DefaultLocationAnchor = new Vector2(-2.7f, 2.3f);
        //Characteristics = new Characteristics();

        //// TODO this will depend on the level!
        //// Do we need a new ScenarioLogic class every time the level changes?
        //// double check what's happening wrt used up resources from the assembler
        //LocationsCount = 3;
        //// Maybe this can vary in levels later on..
        //ManualLinesCount = LocationsCount - 1;
        //NumberOfLiars = Random.Range(1, LocationsCount + 1);
    }

    public void OnNewLevel(LevelInfo level)
    {
        //Characteristics = new Characteristics();
        LocationsCount = level.LocationsCount;
        // TODO not in use, this is really dependant on the complexity level
        ManualLinesCount = level.ManualLinesCount;
        NumberOfLiars = Random.Range(1, LocationsCount + 1);
    }

    public TextureTools TextureTools;
    public int NumberOfLiars;
    public int LocationsCount;
    public int ManualLinesCount;
    public ManualParts ManualLines;
    public string Manual => $"STIRCTLY IN ORDER OF IMPORTENCE:{Environment.NewLine}•{string.Join($"{Environment.NewLine}•", ManualLines)}";
    //public Characteristics Characteristics;
    public Vector2 DefaultLocationAnchor;
    public float Scale = 0.7f;
    public float CharacterScale = 1f;

    public Assembler<Texture2D> Assembler;
    private List<int> AvailableMusicClips;

    public ScenarioData GenerateScenarioData(LevelInfo level)
    {
        var startTime = Time.realtimeSinceStartup;
        OnNewLevel(level);

        var names = NamesList.PersonNames.ShuffledClone();
        var thiefIdx = Random.Range(0, LocationsCount);
        var locations = new List<LocationInfo>();
        ManualLines = level.GetManual();

        var mainMap = new LocationInfo(CreateMap(level.MapName));
        var locationsPlacement = mainMap.Assets.OverlayData.Points[ColorsList.OverlayMapLocation];

        for (int i = 0; i < LocationsCount; i++)
        {
            var palettes = RandomPaletteInfo();
            var character = CreateCharacterResource(palettes.CharacterPalette);

            var charSprite = character.Data.Image;
            var diagSprite = TextureTools.FlipTexture(charSprite);
            var person = new CharacterInfo(
                thiefIdx == i,
                names.Dequeue(),
                null,
                charSprite.ToSprite(StaticHelpers.BottomCenterPivot, CharacterScale),
                diagSprite.ToSprite(StaticHelpers.BottomCenterPivot, CharacterScale));

            var locationAssets = CreateLocation("locations", person, palettes);

            var location = new LocationInfo(
                person,
                locationAssets,
                locationsPlacement[i].Anchor(mainMap.Assets.Anchor),
                AvailableMusicClips.PickRandom());

            locations.Add(location);
        }
        ScenarioLogic.CreateProps(ManualLines, locations, Assembler, Scale);

        bool success = false;
        var tries = 0;
        while (!success)
        {
            if (tries > 4) throw new InvalidOperationException("Too many tries");
            tries++;
            Debug.Log($"Generating dialogues take {tries}");
            success = ScenarioLogic.SetupDialogue(locations);            
        }

        Debug.Log($"Scen gen took: {Time.realtimeSinceStartup - startTime} seconds");
        Debug.Log(Manual);
        Debug.Log($"Spoiler: the theif is {locations.SingleOrDefault(l => l.Person.IsThief).Person.Name}");

        return new ScenarioData(mainMap, locations, Manual);
    }

    private LocationAssets CreateMap(string mapName)
    {
        var resource = Assembler.AssembleNamedPoly(mapName);
        // Slightly below the middle vertically and a center horizontally
        var anchor = new Vector2(-((float)resource.OverlayData.Width / 100 / 2),
            (float)resource.OverlayData.Height / 100 / 2.5f);

        return new LocationAssets(
            anchor,
            resource.Backgrounds.Select(i => i.Image.ToSprite(StaticHelpers.TopLeftPivot)).ToList(),
            resource.Foregrounds?.Select(i => i.Image.ToSprite(StaticHelpers.TopLeftPivot))?.ToList(),
            resource.Collider.Image.ToSprite(StaticHelpers.TopLeftPivot),
            resource.OverlayData);
    }

    private LocationAssets CreateLocation(string type, CharacterInfo character, PaletteInfo palettes)
    {
        var resource = Assembler.AssemblePoly("locations");
        var mainMapSprite = Assembler.AssembleGeneric("smallbuilding");

        ApplyPalette(resource.Backgrounds, palettes.BackgroundPalette);

        //Not really in use, use the default anchor since all locations have the same size
        //var calculatedSpawnPoint = new Vector2(-(resource.OverlayData.Width / 100 / 2), resource.OverlayData.Height / 100 / 2);
        return new LocationAssets(
            DefaultLocationAnchor,
            resource.Backgrounds.Select(i => i.ToSprite(StaticHelpers.TopLeftPivot)).ToList(),
            resource.Foregrounds?.Select(i => i.ToSprite(StaticHelpers.TopLeftPivot))?.ToList(),
            resource.Collider?.ToSprite(StaticHelpers.TopLeftPivot),
            mainMapSprite.ToSprite(StaticHelpers.BottomCenterPivot + new Vector2(0, 0.2f)),
            resource.OverlayData,
            palettes);
    }

    private void ApplyPalette(IImage<Texture2D> applyTo, Dictionary<PixelInfo, PixelInfo> palette)
     => ApplyPalette(new List<IImage<Texture2D>>() { applyTo }, palette);

    private void ApplyPalette(List<IImage<Texture2D>> applyTo, Dictionary<PixelInfo, PixelInfo> palette)
    {
        if (palette == null)
            return;

        foreach (var item in applyTo)
        {
            TextureTools.ApplyPalette(item, palette);
            item.Apply();
        }
    }

    private PaletteInfo RandomPaletteInfo()
    {
        // null is the default palette
        var availablePalettes = new List<Dictionary<PixelInfo, PixelInfo>>
            { Palettes.Replacement0, Palettes.Replacement1, Palettes.Replacement2 };
        var chosen = availablePalettes.PopRandom();

        return new PaletteInfo()
        {
            PropsPalette = chosen,
            CharacterPalette = chosen,
            BackgroundPalette = chosen,
        };
        //return new PaletteInfo()
        //{
        //    PropsPalette = availablePalettes.PopRandom(),
        //    CharacterPalette = availablePalettes.PopRandom(),
        //    BackgroundPalette = availablePalettes.PopRandom(),
        //};
    }

    private AssemblerResource<Texture2D> CreateCharacterResource(Dictionary<PixelInfo, PixelInfo> palette)
    {
        return Assembler.Assemble(Demo2Instructions.CharacterInstructions(false, palette));
    }
}
