using Assets.Scripts.Bayeux;
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
            AvailableCabItems = new List<string>(TypesInfo.CabinetItemTypes);
            AvailableCabItems.Remove("decorative"); // Too broad a category, would be confusing
            RegulatedTypes = new List<ObjectType>();
            AvailableTypes = DefaultTypes.ToList();
        }

        public List<Shape> ReservedVaseShapes;
        public List<string> AvailableCabItems;
        public List<ObjectType> RegulatedTypes;
        public List<ObjectType> AvailableTypes;
        public static ObjectType[] DefaultTypes = new[] { ObjectType.am6plant, ObjectType.am6vase, ObjectType.am6painting };

        public new void Add(ManualPart item)
        {
            if (item.ObjectType == ObjectType.am6vase)
            {
                ReservedVaseShapes.Add((Shape)item.Classifier);
            }

            if (item.IsCabinetItem)
            {
                AvailableCabItems.Remove(item.ItemType);
            }

            base.Add(item);
        }
    }
}
