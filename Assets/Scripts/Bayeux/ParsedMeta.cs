using BayeuxBundle.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Bayeux
{
    // This was meant to be a JSON file, but I can't get the Newtonsoft parser to work on core, for some reason
    // TODO move back to JSON whenever possible..
    //public class ParsedMeta
    //{
    //    Dictionary<string, BayeuxBundle.Models.ParsedMetaPoco> Data = new Dictionary<string, BayeuxBundle.Models.ParsedMetaPoco>()
    //    {
    //            new System.Collections.Generic.KeyValuePair<string, BayeuxBundle.Models.ParsedMetaPoco>
    //{
    //[Serializable]
    //public class MetaContainer
    //{
    //    public ParsedMeta[] Meta;
    //}

    //[Serializable]
    //public class ParsedMeta
    //{
    //    public string MainType;
    //    public AnchorPoint MainAnchor;
    //}

    //[Serializable]
    //public class AnchorPoint
    //{
    //    public int X;
    //    public int Y;
    //}

    
    public class ParsedMeta
    {
        public string MainType;
        public ParsedPoint MainAnchor;
        public ParsedOverlay Overlay;
        public string Description;
        public string[] Types;
    }

    public class ParsedOverlay
    {
        public string SourceType;
        public Dictionary<string, List<ParsedPoint>> Points;
        public int Width;
        public int Height;
    }

    public class ParsedPoint
    {
        public int X;
        public int Y;
    }
}
