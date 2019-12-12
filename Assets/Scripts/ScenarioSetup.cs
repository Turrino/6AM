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
using Assets.Scripts.Generators;
using BayeuxBundle.Serialization;

public class ScenarioSetup
{
    public ScenarioSetup(int noOfMusicClips)
    {
        var metaData = DataReader.ReadMeta();
        TextureTools = new TextureTools(OverlayRef.Am6RefDictWHash);
        var data = new Data();
        var resP = JsonHelper.getJsonArray<SerializedItem>(data.Contents);
        var res = resP.Select(x => new GarblerItem() { Path = x.Path, Content = x.Content }).ToList();
        Assembler = new Assembler<Texture2D>(TextureTools, res, metaData);

        //if (Master.GM.RenderLowResAssets)
        //{
        //    Scale *= 1.3f;
        //    CharacterScale *= 1.3f;
        //    LocationScale *= 1.3f;
        //}

        TypesInfo.CabinetItemTypes = Assembler.GetAllSubtypes(ObjectType.am6item);
        // Skip 0, that's for the map        
        AvailableMusicClips = Enumerable.Range(1, noOfMusicClips - 1).ToList();
        // This is valid for demo2 only. If re-using this code this probably will change.
        //DefaultLocationAnchor = Master.GM.RenderLowResAssets ? new Vector2(-3.5f, 3f) : new Vector2(-2.7f, 2.3f);
        DefaultLocationAnchor = new Vector2(-3.5f, 2.8f);
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
    public string Manual => $"•{string.Join($"{Environment.NewLine}•", ManualLines)}";
    //public Characteristics Characteristics;
    public Vector2 DefaultLocationAnchor;
    public float Scale = 1f;
    public float CharacterScale = 1.3f;
    public float LocationScale = 1.3f;

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
        //var resAdjustment = Master.GM.RenderLowResAssets ? 1.3f : 1f;
        var resAdjustment = 1.3f;
        // Slightly below the middle vertically and a center horizontally
        var anchor = new Vector2(-((float)resource.OverlayData.Width / 100 / 2 * resAdjustment),
            (float)resource.OverlayData.Height / 100 / 2.5f * resAdjustment);

        return new LocationAssets(
            anchor,
            resource.Backgrounds.Select(i => i.Image.ToSprite(StaticHelpers.TopLeftPivot, LocationScale)).ToList(),
            resource.Foregrounds?.Select(i => i.Image.ToSprite(StaticHelpers.TopLeftPivot, LocationScale))?.ToList(),
            resource.Collider.Image.ToSprite(StaticHelpers.TopLeftPivot, LocationScale),
            AdjustOverlayForResolution(resource.OverlayData));
    }

    private Overlay AdjustOverlayForResolution(Overlay overlay)
    {
        //if (Master.GM.RenderLowResAssets)
        //{
            overlay.Width = (int)(1.3 * overlay.Width);
            overlay.Height = (int)(1.3 * overlay.Height);

            foreach (var point in overlay.Points.SelectMany(p => p.Value))
            {
                point.X = (int)(1.3 * point.X);
                // Y isn't really in use here, it's the flipwise Y
                //point.Y = (int)(1.3 * point.Y);
                point.FlipWiseY = (int)(1.3 * point.FlipWiseY);
            }
        //}

        return overlay;
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
            resource.Backgrounds.Select(i => i.ToSprite(StaticHelpers.TopLeftPivot, LocationScale)).ToList(),
            resource.Foregrounds?.Select(i => i.ToSprite(StaticHelpers.TopLeftPivot, LocationScale))?.ToList(),
            resource.Collider?.ToSprite(StaticHelpers.TopLeftPivot, LocationScale),
            mainMapSprite.ToSprite(StaticHelpers.BottomCenterPivot + new Vector2(0, 0.2f)),
            AdjustOverlayForResolution(resource.OverlayData),
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
