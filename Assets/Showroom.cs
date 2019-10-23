using Assets.Scripts.Bayeux;
using Assets.Scripts.Generators;
using Assets.Scripts.Resources;
using BayeuxBundle;
using BayeuxBundle.Models;
using BayeuxBundle.Models.Instructions;
using BayeuxDemo2Assets;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

public class Showroom : MonoBehaviour
{
    public SpriteRenderer Token;
    public Location LocationScript;
    public Camera Camera;


    private Assembler<Texture2D> _assembler;
    private Stopwatch _sw;
    //private long LastChange = 0;
    private Instructions instr;
    private Queue<LocationInfo> Locations;
    private GameObject _showRoomTransform;

    // Start is called before the first frame update
    void Start()
    {
        LocalRandom.ConfigureRandom(new System.Random());
        Master.ConfigureColors();
        _showRoomTransform = new GameObject();

        GenerateData();

        _sw = new Stopwatch();
        _sw.Start();

        //_assembler = new Assembler<Texture2D>(new TextureTools(OverlayRef.Am6RefDictWHash), Demo2ResourcesAssembly.BundleAssembly);
        //instr = Demo2Instructions.CharacterInstructions(false, availablePalettes.PickRandom());
    }

    void GenerateData()
    {
        var scenarioGenerator = new ScenarioSetup(10);
        var data = scenarioGenerator.GenerateScenarioData(Levels.Level2);
        Locations = new Queue<LocationInfo>(data.Locations);
    }

    // location showroom
    void Update()
    {
        if (Input.GetKeyUp("space"))
        {
            if (Locations == null || !Locations.Any())
            {
                GenerateData();
            }
            Camera.backgroundColor = Palettes.BgColors.PickRandom();
            Destroy(_showRoomTransform);
            _showRoomTransform = new GameObject();
            _showRoomTransform.transform.SetParent(transform);
            LocationScript.CreateLocation(Locations.Dequeue(), _showRoomTransform.transform);
        }
    }


    // token showroom
    //void Update()
    //{
    //    if (_sw.ElapsedMilliseconds - 400 > LastChange)
    //    {
    //        LastChange = _sw.ElapsedMilliseconds;
    //        var instructions = instr;
    //        //var instructions = InstructionsPrefabs.Vase(StaticHelpers.RandomEnumValue<BayeuxBundle.Models.Shape>(), true);
    //        var resource = _assembler.Assemble(instructions);

    //        var gen = _assembler.AssembleGeneric("smallbuilding");
    //        //Token.sprite = resource.Data.ToSprite(StaticHelpers.CentralPivot);
    //        Token.sprite = gen.ToSprite(StaticHelpers.CentralPivot);
    //    }
    //}
}
