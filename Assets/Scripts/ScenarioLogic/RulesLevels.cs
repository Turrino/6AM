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

        public static ManualParts ComplexityOne()
        {
            var manual = new ManualParts();

            //Specific rule 1
            manual.Add(GetVaseRule(StaticHelpers.Flip(), manual, 1));
            //Specific rule 2
            manual.Add(GetVaseRule(StaticHelpers.Flip(), manual, 2));

            return manual;
        }

        public static ManualParts ComplexityTwo()
        {
            var manual = new ManualParts();

            var ranks = Enumerable.Range(1, 3).ToList();

            manual.Add(GetVaseRule(StaticHelpers.Flip(), manual, ranks[0]));
            manual.Add(GetVaseRule(StaticHelpers.Flip(), manual, ranks[1]));
            manual.Add(GetPlantRule(StaticHelpers.Flip(), ranks[2]));

            manual.RegulatedTypes.Add(ObjectType.am6plant);

            return manual;
        }

        private static ManualPart GetPlantRule(bool liar, int rank)
        {
            return ManualPart.ManualPartAboutObjectType(ObjectType.am6plant, liar, rank);
        }

        private static ManualPart GetVaseRule(bool liar, List<ManualPart> existingRules, int rank)
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
