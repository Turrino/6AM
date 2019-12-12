//using Newtonsoft.Json;
//using Newtonsoft.Json.Converters;

using System;

namespace BayeuxBundle.Models
{
    /// <summary>
    /// Todo probably redundant, delete once the metadata is parsed better
    /// </summary>
    [Serializable]
    public class ParsedMetaPoco
    {
        public string Name;
        public string MainType;
        public ImgPoint MainAnchor;
        public Overlay Overlay;
        public string Description;
        public string[] Types;
    }
}
