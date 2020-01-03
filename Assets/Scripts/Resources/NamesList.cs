
using BayeuxBundle;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Resources
{
    public static class NamesList
    {
        // Sorting layers
        public const string SortingLayerBackground = "background";
        public const string SortingLayerForeground = "foreground";
        public const string SortingLayerWorld = "world";

        public const string MainScenario = "Main";
        public const string MainMenu = "MainMenu";
        public const string Intro = "Intro";
        public const string Loading = "Loading";
        public const string Location = "Location";
        public const string ExitToMain = "Exit";
        public const string Outro = "Outro";
        public const string Player = "Player";

        // Tags
        public const string LocationBounds = "LocationBoundary";
        public const string Character = "Character";
        public const string Container = "Container";

        // Overlay usage (location)
        //public static string OverlaySpawn = "00EAFF"; (player isn't placed on location anymore)
        public static string OverlayProps = "#00FF26";
        public static string OverlayWallProps = "#0D00FF";
        public static string OverlayCharacter = "#FF00E5";
        //public static string OverlayExit = "000000";

        // Overlay usage (map)
        public static string OverlayMapLocation = "#00FF26";
        public static string OverlayMapSpawn = "#FF00E5";



        public static List<string> GetNamesForLevel(bool hurdMode, int count)
        {
            if (hurdMode)
            {
                if (CurrentDifficultNames == null || CurrentDifficultNames.Count == 0)
                {
                    CurrentDifficultNames = DifficultNames.Select(x => new List<string>(x)).ToList();
                    foreach (var item in CurrentDifficultNames)
                    {
                        item.Shuffle();
                    }
                }
                return CurrentDifficultNames.PopRandom();
            }

            if (CurrentPersonNames == null || CurrentPersonNames.Count < count)
            {
                CurrentPersonNames = new List<string>(PersonNames);
            }

            var names = new List<string>();
            for (int i = 0; i < count; i++)
            {
                names.Add(CurrentPersonNames.PopRandom());
            }

            return names;
        }

        private static List<List<string>> CurrentDifficultNames;
        private static List<string> CurrentPersonNames;

        private static List<List<string>> DifficultNames = new List<List<string>>()
        {
            new List<string>() { "Bob", "Bobbea", "Bob Jr.", "Bobby", "Bobert", "B. O. Bert" },
            new List<string>() { "Ribbity", "Rabbity", "Wobbity", "Wabbity" },
            new List<string>() { "Fruffu", "Friffi", "Cricri", "Cracra", "Furfra", "Frafri" },
            new List<string>() { "Claire", "Klare", "Klaire", "Clara", "Klaar", "Claro" },
            new List<string>() { "Purli", "Purla", "Perela", "Pereli", "Perlila" },
            new List<string>() { "Nini", "Nana", "Nunu", "Nene", "Nono" },
            new List<string>() { "Lila", "Lala", "Lollo", "Lola", "Lalla", "Lalli" },
            new List<string>() { "Ubu", "Aba", "Uba", "Bau", "Boa", "Abu" }
        };

        private static List<string> PersonNames = new List<string>()
        {
            "Ribbity", "Bobbea", "'Not Mathilda'", "Poppy", "Yogurta",
            "Yolanda", "Polentina", "Giuseppo", "Wobbity", "Perela", "Nana",
            "Ubu", "Aba", "Ululi", "Mappo", "Fra", "Mr. No",
            "Chapatty", "Ms. Maybe", "Dear Prudence", "Carlito",
            "Juanita", "Wally", "U-uuu", "Aha", "Oh-Yeah", "Chipo",
            "Yayai", "Araffa", "Stealino", "Thiefella", "Honestina",
            "Yettr"
        };

        /// <summary>
        /// Only valid for value types!
        /// </summary>
        public static Queue<T> ShuffledClone<T>(this List<T> originalList)
        {
            originalList.Shuffle();
            return new Queue<T>(originalList);
        }
    }
}
