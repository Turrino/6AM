using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BayeuxBundle.Models.Palettes
{
    public static class Samples
    {
        public static Dictionary<string, string> AddHashes(this Dictionary<string, string> input)
            => input.ToDictionary(kv => $"#{kv.Key}", kv => $"#{kv.Value}");

        public static Dictionary<string, string> Replacement1 = new Dictionary<string, string>()
        {
            { "F4546E", "F48954" },
            { "3B6AE2", "80A0F2" },
            { "E7C801", "F9ED26" },
            { "ADBFA8", "9651A2" },
            { "BA6767", "C164CB" },
            { "80A3C7", "5AC854" },
            { "556952", "4D4741" },
            { "464668", "684661" },
            { "5F4F5F", "88D992" },
            { "460C23", "5B4D52" },
            { "809B9F", "D6EDDA" }
        };

        public static Dictionary<string, string> Replacement2 = new Dictionary<string, string>()
        {
            { "F4546E", "5124DB" },
            { "3B6AE2", "75A882" },
            { "E7C801", "DE3962" },
            { "ADBFA8", "C2AB84" },
            { "BA6767", "52E0EF" },
            { "80A3C7", "EEA2F0" },
            { "556952", "34895A" },
            { "464668", "3A2F28" },
            { "5F4F5F", "AF6D76" },
            { "460C23", "65526F" },
            { "809B9F", "D0C2B1" }
        };

        // OLD palettes
        //public static Dictionary<string, string> Replacement1 = new Dictionary<string, string>()
        //{
        //    { "F4546E", "F48954" },
        //    { "3B6AE2", "80A0F2" },
        //    { "E7C801", "ACEF68" },
        //    { "ADBFA8", "A8BFBE" },
        //    { "BA6767", "BFB7A4" },
        //    { "80A3C7", "818196" },
        //    { "556952", "4D4741" },
        //    { "464668", "684661" },
        //    {"5F4F5F", "A2AAA3" },
        //    {"460C23", "82797C" },
        //    {"809B9F", "A4C3A9" }
        //};

        //public static Dictionary<string, string> Replacement2 = new Dictionary<string, string>()
        //{
        //    { "F4546E", "8C72DD" },
        //    { "3B6AE2", "75A882" },
        //    { "E7C801", "DE3962" },
        //    { "ADBFA8", "C2AB84" },
        //    { "BA6767", "9BC9CE" },
        //    { "80A3C7", "C580C7" },
        //    { "556952", "525312" },
        //    { "464668", "3A2F28" },
        //    {"5F4F5F", "AF6D76" },
        //    {"460C23", "65526F" },
        //    {"809B9F", "D0C2B1" }
        //};
    }
}
