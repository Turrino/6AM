using Assets.Scripts.ScenarioLogic;

namespace Assets.Scripts.Resources
{
    public class LevelInfo
    {
        public string MapName;
        public int ManualLinesCount;
        public int LocationsCount;
        public delegate ManualParts ManualMethod();
        public ManualMethod GetManual;
        public int SecondsAvailable;
    }
}
