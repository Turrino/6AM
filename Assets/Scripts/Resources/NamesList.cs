
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
        public const string Loading = "Loading";
        public const string Location = "Location";
        public const string ExitToMain = "Exit";
        public const string Player = "Player";

        // Tags
        public const string LocationBounds = "LocationBoundary";
        public const string Character = "Character";        

        // Overlay usage (location)
        //public static string OverlaySpawn = "00EAFF"; (player isn't placed on location anymore)
        public static string OverlayProps = "#00FF26";
        public static string OverlayWallProps = "#0D00FF";
        public static string OverlayCharacter = "#FF00E5";
        //public static string OverlayExit = "000000";

        // Overlay usage (map)
        public static string OverlayMapLocation = "#00FF26";
        public static string OverlayMapSpawn = "#FF00E5";


        //TODO update
        public static List<string> PersonNames = new List<string>()
        {
            "Ribbity", "Rabbity", "Bob", "'Not Mathilda'", "Poppy"
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
