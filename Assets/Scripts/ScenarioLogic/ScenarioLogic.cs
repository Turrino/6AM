using Assets.Scripts.Bayeux;
using Assets.Scripts.Generators;
using Assets.Scripts.Resources;
using BayeuxBundle;
using BayeuxBundle.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.ScenarioLogic
{
    public class ScenarioLogic
    {
        /// <summary>
        /// Should only be called AFTER the manual has been created.
        /// The dialogues depend on the manual to avoid having scenarios that are either too easy, or unsolvable
        /// </summary>
        public static bool SetupDialogue(List<LocationInfo> locations)
        {
            // For those not in the manual: they are a red-herring.
            // It doesn't really matter what they do, or do not say, the player does not know how many liars there are.
            var notDescribed = locations.Where(l => !l.Person.DescribedByManual);
            foreach (var location in notDescribed)
            {
                var otherCharacter = locations.Where(l => l != location).ToList().PickRandom().Person;
                var dialogueType = StaticHelpers.RandomEnumValue(exclude: new[] { DialogueType.Na });
                location.Person.DialogueLine = TextResources.GetDialogue(location.Person, dialogueType, otherCharacter);
            }

            var described = locations.Where(l => l.Person.DescribedByManual).Select(l => l.Person);
            // Make sure we don't have too much redundant info
            // e.g. characters can be cleared twice but ONLY if there's enough info to 
            var cleared = new List<CharacterInfo>();
            var neededForSolution = locations.Count - 1; // To be solvable, all except one must be cleared
            foreach (var person in described)
            {
                // The other person in this scenario must not be the thief, otherwise we'd be giving it away
                var otherCharacters =
                    locations.Select(l => l.Person)
                    .Where(p => p != person && !p.IsThief && (!cleared.Contains(p) || cleared.Count >= neededForSolution))
                    .ToList();
                var otherCharacter = otherCharacters.Any()? otherCharacters.PickRandom() : null;

                var talkAboutOthers = person.IsThief || (otherCharacter != null && StaticHelpers.Flip())
                    // If this character has been cleared, they can only talk about someone else
                    || cleared.Contains(person) && cleared.Count < neededForSolution; 

                // If the person IS the thief and they are described in the manual, they MUST talk about someone else.
                if (person.IsThief && otherCharacter == null)
                {
                    return false;
                }

                if (talkAboutOthers)
                {
                    cleared.Add(otherCharacter);  
                    person.DialogueLine = TextResources
                        .GetDialogue(person, DialogueType.TalkOthers, otherCharacter);
                }
                else
                {
                    cleared.Add(person);
                    // We also can't have the thief talk about themselves
                    // They MAY admit to the crime, but only if they're not in the manual (see above)
                    person.DialogueLine = TextResources
                        .GetDialogue(person, DialogueType.TalkSelf);                    
                }
            }

            return true;
        }


        /// <summary>
        /// Important! This also sets the liar status on each character
        /// </summary>
        /// <param name="locations"></param>
        public static void CreateProps(ManualParts manual, List<LocationInfo> locations, Assembler<Texture2D> assembler, float scale)
        {
            var tools = new TextureTools(OverlayRef.Am6RefDictWHash);

            var specificRules = manual.OrderBy(r => r.Rank).ToList();
            var specificLocations = locations.PickRandoms(specificRules.Count());
            var rulesAndLocations = specificRules.Zip(specificLocations, (r, l) => new { r, l });

            // Each rule should apply to one location
            foreach (var rl in rulesAndLocations)
            {
                var palette = rl.l.Assets.PaletteInfo.PropsPalette;
                var props = new List<PropInfo>();

                var propType = rl.r.ObjectType;
                var propClassifier = rl.r.Classifier;

                if (rl.r.IsCabinetItem)
                {
                    props.Add(SetupCabinet(palette, new[] { rl.r.ItemType }));
                }
                else if (rl.r.ObjectType == ObjectType.am6vase)
                {
                    //var instructions = InstructionsPrefabs.Vase((Shape)rl.r.Classifier, StaticHelpers.Flip());
                    var instructions = Demo2Instructions.VaseInstructions((Shape)rl.r.Classifier);
                    instructions.Palette = palette;
                    var vase = assembler.Assemble(instructions).ToProp(tools, scale);
                    props.Add(vase);
                }
                else if (rl.r.ObjectType == ObjectType.am6plant)
                {
                    var prop = GetProp(ObjectType.am6plant, palette);
                    props.Add(prop);
                }
                else if (rl.r.ObjectType == ObjectType.am6painting)
                {
                    var prop = GetProp(ObjectType.am6painting, palette);
                    props.Add(prop);
                }

                // Super important! Flip the liar status here or there's a 50% chance they will have the wrong dialogues
                rl.l.Person.Liar = rl.r.AreLiars; 
                rl.l.RelevantRules.Add(rl.r);
                rl.l.Person.DescribedByManual = true;
                rl.l.Assets.AddProps(props);
            }

            // Finally, add a bit of colour with items that do not belong to a rule and do not affect anything
            foreach (var location in locations)
            {
                var props = new List<PropInfo>();

                // Stuff that always goes in regardless (does not feature in the manual)
                // (there's none right now, but might be added later: lamps, rugs, trinkets, ...

                // The rest
                var currentTypes = location.Assets.Props.Select(p => p.Definition.ObjectType).ToList();
                var doesNotHave = manual.AvailableTypes.Except(currentTypes).ToList();
                foreach (var item in doesNotHave)
                {
                    bool canBeAdded = StaticHelpers.Flip(25);
                    // Find out if it is regulated. It may be admissible if it is, but a relevant higher ranking rule exists in that location
                    if (manual.RegulatedTypes.Contains(item))
                    {
                        var regulatedRanks = manual.Where(r => r.ObjectType == item);
                        var regulatedRank = regulatedRanks.Any() ? regulatedRanks.Max(r => r.Rank) : 0;
                        var locationRules = location.RelevantRules.Where(r => r.ObjectType != item);
                        var locationRulesRank = locationRules.Any() ? locationRules.Max(r => r.Rank) : 0;
                        canBeAdded = locationRulesRank > regulatedRank;
                    }

                    if (canBeAdded)
                    {
                        props.Add(GetProp(item, location.Assets.PaletteInfo.PropsPalette));
                    }
                }
                // Also check if we can add a cabinet - special type since it contains more items
                if (!location.Assets.Props.Any(p => p.Definition.ObjectType == ObjectType.am6cabinet))
                {
                    props.Add(SetupCabinet(location.Assets.PaletteInfo.PropsPalette));
                }

                location.Assets.AddProps(props);

            }

            // Done!

            // Local methods
            PropInfo SetupCabinet(Dictionary<PixelInfo, PixelInfo> palette, string[] typesToInclude = null)
            {
                var cabinet = assembler.Assemble(Demo2Instructions.CabinetInstructions(palette)).ToProp(tools, scale * 1.5f);
                var contents = new List<PropInfo>();
                var typesQueue = typesToInclude != null? new Queue<string>(typesToInclude) : new Queue<string>();
                //var allowableTypes = TypesInfo.CabinetItemTypes.Except(typesToExclude.Concat(typesToInclude)).ToList();

                for (int i = 0; i < 3; i++)
                {
                    var itemType = typesQueue.Any() ? typesQueue.Dequeue() : manual.AvailableCabItems.PickRandom();

                    var item = assembler.Assemble(Demo2Instructions.ItemInstructions(itemType, palette))
                        .ToItemProp(tools, 2);

                    contents.Add(item);
                }
                cabinet.Contents = contents;

                return cabinet;
            }

            PropInfo GetProp(ObjectType objectType, Dictionary<PixelInfo, PixelInfo> palette)
            {                
                if (objectType == ObjectType.am6plant)
                {
                    var plant = assembler.Assemble(Demo2Instructions.PottedPlantInstructions(palette)).ToProp(tools, scale);
                    return plant;
                }
                if(objectType == ObjectType.am6vase)
                {
                    var availableShape = StaticHelpers.RandomEnumValue(manual.ReservedVaseShapes);
                    var instructions = Demo2Instructions.VaseInstructions(availableShape);
                    instructions.UseOnce = false;
                    instructions.Palette = palette;
                    var vase = assembler.Assemble(instructions).ToProp(tools, scale);
                    return vase;
                }
                if (objectType == ObjectType.am6painting)
                {
                    var paint = assembler.Assemble(
                            Demo2Instructions.PaintingInstructions(palette));
                    return paint.ToProp(tools, StaticHelpers.CentralPivot, PixelInfo.White, scale);
                }
                throw new InvalidOperationException("missing prop!GetProp()");
            }
        }
    }
}
