using BayeuxBundle.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.ScenarioLogic
{
    public class ManualParts : List<ManualPart>
    {
        public ManualParts()
        {
            ReservedVaseShapes = new List<Shape>();
            RegulatedTypes = new List<ObjectType>();
            AvailableTypes = DefaultTypes.ToList();
        }

        public List<Shape> ReservedVaseShapes;
        public List<ObjectType> RegulatedTypes;
        public List<ObjectType> AvailableTypes;
        public static ObjectType[] DefaultTypes = new[] { ObjectType.am6plant, ObjectType.am6vase };

        public new void Add(ManualPart item)
        {
            if (item.ObjectType == ObjectType.am6vase)
            {
                ReservedVaseShapes.Add((Shape)item.Classifier);
            }

            base.Add(item);
        }
    }
}
