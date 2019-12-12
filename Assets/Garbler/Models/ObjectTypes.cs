using System;
using System.Collections.Generic;
using System.Text;

namespace BayeuxBundle.Models
{
    // Used in creating an object with attached metadata.
    // So for instance we can use that data to generate the sentence:
    // "This is a red square vase"
    //TODO - this needs to be moved to the bundle - probably as a list of strings
    public enum ObjectType
    {
        na = 0,
        am6vase = 1,
        am6deco = 2,
        am6body = 3,
        am6face = 4,
        am6hat = 5,
        am6tail = 6,
        smallbuilding2 = 12,
        am6pot = 13,
        am6plant = 14,
        am6painting = 15,
        am6cabinet = 16,
        am6cabinetdoor = 17,
        am6peg = 18,
        am6trinket = 19,
        am6item = 20,
        am6thing = 21,
        overlay = 999 // Special type, use for object-type specific overlays
    }

    public enum Shape
    {
        na = 0,
        round = 1,
        triangular = 2,
        four_sided = 3,
    }

    public class ResourceDictionary
    {
        public Dictionary<string, Type> Flags = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase)
        {
            { "s", typeof(Shape) },
            { "o", typeof(ObjectType) },
        };
    }
}
