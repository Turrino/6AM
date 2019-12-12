using System.Collections.Generic;

namespace BayeuxBundle.Models
{
    public class ParsedPolyRes<T>
    {
        public ParsedPolyRes(List<IImage<T>> backgrounds, List<IImage<T>> foregrounds,
            IImage<T> collider, Overlay overlayData)
        {
            Backgrounds = backgrounds;
            Foregrounds = foregrounds;
            Collider = collider;
            OverlayData = overlayData;
        }

        /// <summary>
        /// The layers composing the image, from background (0) for foreground (inf.)
        /// </summary>
        public List<IImage<T>> Backgrounds { get; set; }
        public List<IImage<T>> Foregrounds { get; set; }
        public IImage<T> Collider { get; set; }
        public Overlay OverlayData { get; set; }
    }
}
