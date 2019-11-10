using BayeuxBundle.Models;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Bayeux
{
    public static class Palettes
    {
        public static Dictionary<PixelInfo, PixelInfo> Replacement0;
        public static Dictionary<PixelInfo, PixelInfo> Replacement1;
        public static Dictionary<PixelInfo, PixelInfo> Replacement2;
        public static List<Color32> BgColors;

        public static Color32[] Backgrounds = new[] {
            new Color32(29, 200, 234, 255),
            new Color32(41, 39, 60, 255),
            new Color32(222, 90, 123, 255),
            new Color32(127, 168, 125, 255),
            new Color32(100, 113, 165, 255)
        };

        public static List<string> BgStringColors = new List<string>()
        {
            "#29273C",
            "#508953",
            "#9A50A2",
            "#289ED9",
            "#D68F39",
            "#1C404E",
            "#1C0734",
            "#24032C",
            "#B02853",
            "#A52E13"
        };

        public static Dictionary<string, string> Replacement0String = new Dictionary<string, string>()
        {
            { "4C6049", "3AC8C7" },
            { "460C23", "312832" },
            { "5F4F5F", "38383E" },
            { "809B9F", "446b46" },
            { "556952", "60D24F" }
        };

        public static Dictionary<string, string> Replacement1String = new Dictionary<string, string>()
        {
            { "F4546E", "F48954" },
            { "3B6AE2", "80A0F2" },
            { "E7C801", "F9ED26" },
            { "ADBFA8", "9651A2" },
            { "BA6767", "C164CB" },
            { "80A3C7", "5AC854" },
            { "464668", "684661" },
            { "460C23", "5B4D52" },
            { "5F4F5F", "963F4E" },
            { "809B9F", "5D3761" },
            { "556952", "402B37" }
        };

        public static Dictionary<string, string> Replacement2String = new Dictionary<string, string>()
        {
            { "F4546E", "5124DB" },
            { "3B6AE2", "75A882" },
            { "E7C801", "DE3962" },
            { "ADBFA8", "C2AB84" },
            { "BA6767", "52E0EF" },
            { "80A3C7", "EC94EE" },
            { "556952", "34895A" },
            { "464668", "3A2F28" },
            { "5F4F5F", "AF6D76" },
            { "460C23", "65526F" },
            { "809B9F", "D8EAC6" }
        };
    }
}
