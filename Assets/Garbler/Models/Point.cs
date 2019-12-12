using System;
using System.Collections.Generic;
using System.Text;

namespace BayeuxBundle.Models
{
    public class ImgPoint
    {
        public ImgPoint(int x, int y, bool restricted = false)
        {
            if(!restricted)
            {
                X = x;
                Y = y;
            }
            else
            {
                X = x > 0 ? x : 0;
                Y = y > 0 ? y : 0;
            }
        }

        public int X;
        public int Y;
    }

    public class OverlayPoint : ImgPoint
    {
        public OverlayPoint(int x, int y, int imageHeight = 0) : base(x, y)
        {
            FlipWiseY = imageHeight - y;
        }

        /// <summary>
        /// For engines like Unity that have y=0 at the bottom of the image.
        /// </summary>
        public int FlipWiseY;
    }
}
