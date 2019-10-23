//using Assets.Scripts.Resources;
//using BayeuxBundle.Models;
//using BayeuxBundle.Models.Instructions;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace Assets.Scripts.ScenarioLogic
//{
//    public class Characteristics
//    {
//        public Characteristics()
//        {
//            // TODO potentially add a variation and/or manual rules wrt the decoration
//            var allAvailableVases = StaticHelpers.EnumValues(typeof(Shape)).Cast<Shape>()
//                .Select(s => InstructionsPrefabs.Vase(s, true));

//            // TODO add the other resources here

//            ImplementedResources = new List<Instructions>();
//            ImplementedResources.AddRange(allAvailableVases);
//            ImplementedResources.Shuffle();
//        }

//        // The list of available instructions
//        public List<Instructions> ImplementedResources;
//    }
//}
