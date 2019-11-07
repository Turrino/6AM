using Assets.Scripts.Resources;
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

            if (StaticHelpers.Flip())
                AddPlantRule(manual, StaticHelpers.Flip(), 1);
            else
                AddPaintingRule(manual, StaticHelpers.Flip(), 1);

            return manual;
        }

        // Two rules Works for up to three locations, I think
        public static ManualParts ComplexityOne()
        {
            var manual = new ManualParts();

            //Specific rule 1
            manual.Add(AddVaseRule(StaticHelpers.Flip(), manual, 1));
            //Specific rule 2
            if (StaticHelpers.Flip())
            {
                manual.Add(AddVaseRule(StaticHelpers.Flip(), manual, 2));
            }
            else
            {
                // TODO simplify the choice between rules and regulated types, move to a "getrandomrule" method
                if (StaticHelpers.Flip())
                    AddPlantRule(manual, StaticHelpers.Flip(), 2);
                else
                    AddPaintingRule(manual, StaticHelpers.Flip(), 2);
            }

            return manual;
        }

        // Three rules, Works for up to four locations
        public static ManualParts ComplexityTwo()
        {
            var manual = new ManualParts();

            var ranks = Enumerable.Range(1, 3).ToList();

            manual.Add(AddVaseRule(StaticHelpers.Flip(), manual, ranks[0]));
            manual.Add(AddVaseRule(StaticHelpers.Flip(), manual, ranks[1]));

            if (StaticHelpers.Flip())
                AddPlantRule(manual, StaticHelpers.Flip(), ranks[2]);
            else
                AddPaintingRule(manual, StaticHelpers.Flip(), ranks[2]);

            return manual;
        }

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

        private static ManualPart AddVaseRule(bool liar, List<ManualPart> existingRules, int rank)
        {
            var noOverlap = existingRules
                .Select(r => r.Classifier)
                .Where(c => c is Shape)
                .Cast<Shape>();

            var objectType = ObjectType.am6vase; // TODO add more
            var classifier = StaticHelpers.RandomEnumValue(noOverlap);
            return ManualPart.ManualPartAboutShape(objectType, classifier, liar, rank);
        }
    }
}
