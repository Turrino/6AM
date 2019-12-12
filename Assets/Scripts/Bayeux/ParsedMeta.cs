using BayeuxBundle.Models;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Bayeux
{    
    public class ParsedMeta
    {
        public string Name;
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

    public static class DataReader
    {
        private static ImgPoint ToPoint(ParsedPoint point) => point == null? null : new ImgPoint(point.X, point.Y);
        private static OverlayPoint ToOverlayPoint(ParsedPoint point) => new OverlayPoint(point.X, point.Y);
        private static Overlay ToOverlay(ParsedOverlay parsed)
        {
            if (parsed == null || !parsed.Points.Any())
                return null;

            var points = parsed.Points
                .ToDictionary(
                kv => new PixelInfo(kv.Key),
                kv => kv.Value.Select(p => ToOverlayPoint(p)).ToList());
            return new Overlay(points, parsed.Width, parsed.Height, parsed.SourceType);
        }
        public static Dictionary<string, ParsedMetaPoco> ReadMeta()
        {
            var parsed = new MetaDataSource().ParsedMetaPocoDict; 
            var result = new Dictionary<string, ParsedMetaPoco>();

            foreach (var item in parsed)
            {
                result.Add(item.Name, item);
            }

            return result;
        }
    }
}
