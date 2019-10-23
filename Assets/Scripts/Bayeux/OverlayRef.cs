using BayeuxBundle.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Bayeux
{
    public static class OverlayRef
    {
        public static Dictionary<ObjectType, string> Am6RefDict = new Dictionary<ObjectType, string>()
        {
            { ObjectType.am6hat, "00FF26" },
            { ObjectType.am6face, "FF00E5" },
            { ObjectType.am6tail, "00EAFF" },
            { ObjectType.am6cabinetdoor, "00FF26" },
            { ObjectType.am6peg, "FF00E5" },
            { ObjectType.am6trinket, "00EAFF" }
        };

        public static Dictionary<ObjectType, string> Am6RefDictWHash = Am6RefDict
            .ToDictionary(
                x => x.Key,
                x => $"#{x.Value}");
    }
}
