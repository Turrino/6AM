using Assets.Scripts.Resources;
using BayeuxBundle;
using BayeuxBundle.Models;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.ScenarioLogic
{
    public static class RulesLevels
    {
        // Entry level, one rule only
        public static ManualParts Complexity0()
        {
            var manual = new ManualParts();

            AddRandomObjectRule(manual, StaticHelpers.Flip(), 1, false);

            return manual;
        }

        // Two rules Works for up to three locations, I think
        public static ManualParts Complexity1()
        {
            var manual = new ManualParts();

            //Specific rule 1
            AddVaseRule(manual, StaticHelpers.Flip(), 1);
            //Specific rule 2
            AddRandomObjectRule(manual, StaticHelpers.Flip(), 2, true);

            return manual;
        }

        //Cab means cabinets are included
        public static ManualParts Complexity1Cab()
        {
            var manual = new ManualParts();

            //Specific rule 1
            AddCabinetItemRule(manual, StaticHelpers.Flip(), 1);
            //Specific rule 2
            AddRandomObjectRule(manual, StaticHelpers.Flip(), 2, true);

            return manual;
        }

        // Three rules, Works for up to four locations
        public static ManualParts Complexity2()
        {
            var manual = new ManualParts();

            var ranks = Enumerable.Range(1, 3).ToList();

            AddVaseRule(manual, StaticHelpers.Flip(), ranks[0]);
            AddVaseRule(manual, StaticHelpers.Flip(), ranks[1]);
            AddRandomObjectRule(manual, StaticHelpers.Flip(), ranks[2], false);

            return manual;
        }

        // Like C2, includes cabinet items though
        public static ManualParts Complexity2Cab()
        {
            var manual = new ManualParts();

            var ranks = Enumerable.Range(1, 3).ToList();

            AddVaseRule(manual, StaticHelpers.Flip(), ranks[0]);
            AddRandomObjectRule(manual, StaticHelpers.Flip(), ranks[1], false);
            AddCabinetItemRule(manual, StaticHelpers.Flip(), ranks[2]);

            return manual;
        }

        private static void AddRandomObjectRule(ManualParts manual, bool liar, int rank, bool includeVase)
        {
            var optionsCount = includeVase ? 3 : 2;
            var option = Enumerable.Range(1, optionsCount).ToList().PickRandom();
            RandomRulesOptions[option](manual, liar, rank);
        }

        private static void AddCabinetItemRule(ManualParts manual, bool liar, int rank)
        {
            var rule = ManualPart.ManualPartAboutCabItems(manual.AvailableCabItems.PickRandom(), liar, rank);
            manual.Add(rule);
        }

        private delegate void RuleAdd(ManualParts manual, bool liar, int rank);

        private static Dictionary<int, RuleAdd> RandomRulesOptions = new Dictionary<int, RuleAdd>()
        {
            { 1, AddPlantRule },
            { 2, AddPaintingRule },
            { 3, AddVaseRule }
        };

        private static void AddPlantRule(ManualParts manual, bool liar, int rank)
        {
            var rule = ManualPart.ManualPartAboutObjectType(ObjectType.am6plant, liar, rank);
            manual.Add(rule);
            manual.RegulatedTypes.Add(ObjectType.am6plant);
        }

        private static void AddPaintingRule(ManualParts manual, bool liar, int rank)
        {
            var rule = ManualPart.ManualPartAboutObjectType(ObjectType.am6painting, liar, rank);
            manual.Add(rule);
            manual.RegulatedTypes.Add(ObjectType.am6painting);
        }

        private static void AddVaseRule(ManualParts manual, bool liar, int rank)
        {
            var noOverlap = manual
                .Select(r => r.Classifier)
                .Where(c => c is Shape)
                .Cast<Shape>();

            var objectType = ObjectType.am6vase; // TODO add more
            var classifier = StaticHelpers.RandomEnumValue(noOverlap);
            manual.Add(ManualPart.ManualPartAboutShape(objectType, classifier, liar, rank));
        }
    }
}
