using System.Collections.Generic;

namespace BayeuxBundle.Models
{
    public class PolyResource
    {
        public PolyResource(List<byte[]> images, byte[] collider, byte[] overlayData)
        {
            Images = images;
            Collider = collider;
            OverlayData = overlayData;
        }

        /// <summary>
        /// The layers composing the image, from background (0) for foreground (inf.)
        /// </summary>
        public List<byte[]> Images { get; set; }
        public byte[] Collider { get; set; }
        public byte[] OverlayData { get; set; }
    }
}
