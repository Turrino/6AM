using Assets.Scripts.ScenarioLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Resources
{
    public static class Levels
    {
        public static LevelInfo Level1 = new LevelInfo()
        {
            MapName = "pond3x",
            ManualLinesCount = 3,
            LocationsCount = 3,
            SecondsAvailable = 109,
            GetManual = RulesLevels.ComplexityOne
        };

        public static LevelInfo Level2 = new LevelInfo()
        {
            MapName = "flower3x",
            ManualLinesCount = 3,
            LocationsCount = 3,
            SecondsAvailable = 139,
            GetManual = RulesLevels.ComplexityTwo
        };

        public static LevelInfo Level3 = new LevelInfo()
        {
            MapName = "flower4x",
            ManualLinesCount = 3,
            LocationsCount = 4,
            SecondsAvailable = 199,
            GetManual = RulesLevels.ComplexityTwo
        };

        public static LevelInfo[] LevelList = new[] { Level1, Level2, Level3 };
    }
}
