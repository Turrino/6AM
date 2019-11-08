using Assets.Scripts.Resources;
using BayeuxBundle;
using BayeuxBundle.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.ScenarioLogic
{
    public static class RulesLevels
    {
        //private static List<ObjectType> DefaultNotRegulatedTypes = new List<ObjectType> { ObjectType.am6plant, ObjectType.am6vase };

        // Demo level, one rule only
        public static ManualParts ComplexityZero()
        {
            var manual = new ManualParts();

            AddRandomObjectRule(manual, StaticHelpers.Flip(), 1);

            return manual;
        }

        // Two rules Works for up to three locations, I think
        public static ManualParts ComplexityOne()
        {
            var manual = new ManualParts();

            //Specific rule 1
            AddVaseRule(manual, StaticHelpers.Flip(), 1);
            //Specific rule 2
            AddRandomObjectRule(manual, StaticHelpers.Flip(), 2, true);

            return manual;
        }

        // Three rules, Works for up to four locations
        public static ManualParts ComplexityTwo()
        {
            var manual = new ManualParts();

            var ranks = Enumerable.Range(1, 3).ToList();

            AddVaseRule(manual, StaticHelpers.Flip(), ranks[0]);
            AddVaseRule(manual, StaticHelpers.Flip(), ranks[1]);
            AddRandomObjectRule(manual, StaticHelpers.Flip(), ranks[2]);

            return manual;
        }

        // Like C2, includes cabinet items though
        public static ManualParts ComplexityThree()
        {
            var manual = new ManualParts();

            var ranks = Enumerable.Range(1, 3).ToList();

            return manual;
        }

        private static void AddRandomObjectRule(ManualParts manual, bool liar, int rank, bool includeVase = false)
        {
            var optionsCount = includeVase ? 3 : 2;
            var option = Enumerable.Range(1, optionsCount).ToList().PickRandom();
            RandomRulesOptions[option](manual, liar, rank);
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
