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
        public static LevelInfo Level0 = new LevelInfo()
        {
            MapName = "flower2x",
            ManualLinesCount = 1,
            LocationsCount = 2,
            SecondsAvailable = 109,
            GetManual = RulesLevels.Complexity0
        };

        public static LevelInfo Level1 = new LevelInfo()
        {
            MapName = "plant3x",
            ManualLinesCount = 3,
            LocationsCount = 3,
            SecondsAvailable = 109,
            GetManual = RulesLevels.Complexity1
        };

        public static LevelInfo Level2 = new LevelInfo()
        {
            MapName = "pond3x",
            ManualLinesCount = 3,
            LocationsCount = 3,
            SecondsAvailable = 109,
            GetManual = RulesLevels.Complexity1Cab
        };

        public static LevelInfo Level3 = new LevelInfo()
        {
            MapName = "flower3x",
            ManualLinesCount = 3,
            LocationsCount = 3,
            SecondsAvailable = 139,
            GetManual = RulesLevels.Complexity2
        };

        public static LevelInfo Level4 = new LevelInfo()
        {
            MapName = "plant4x",
            ManualLinesCount = 3,
            LocationsCount = 4,
            SecondsAvailable = 199,
            GetManual = RulesLevels.Complexity2
        };

        public static LevelInfo Level5 = new LevelInfo()
        {
            MapName = "flower4x",
            ManualLinesCount = 3,
            LocationsCount = 4,
            SecondsAvailable = 199,
            GetManual = RulesLevels.Complexity2Cab
        };

        public static LevelInfo[] LevelList = new[] { Level0, Level1, Level2, Level3, Level4, Level5 };
    }
}
