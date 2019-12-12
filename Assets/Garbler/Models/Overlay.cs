//using Newtonsoft.Json;
//using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace BayeuxBundle.Models
{
    public class Overlay
    {
        public Overlay(Dictionary<PixelInfo, List<OverlayPoint>> overlayData, int width, int height, string sourceType)
        {
            Points = overlayData;
            Width = width;
            Height = height;
            SourceType = sourceType;
        }

        /// <summary>
        /// The type of object this overlay is associated to.
        /// TODO: Move this property to the resource itself (e.g. IsOverlayType)
        /// </summary>
        //[JsonConverter(typeof(StringEnumConverter))]
        public string SourceType;
        public Dictionary<PixelInfo, List<OverlayPoint>> Points;
        public int Width;
        public int Height;
    }
}
