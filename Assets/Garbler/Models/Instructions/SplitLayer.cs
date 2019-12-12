using System;
using System.Collections.Generic;
using System.Text;

namespace BayeuxBundle.Models.Instructions
{
    public class SplitLayer<T>
    {
        public ImgPoint Anchor;
        public IImage<T> image;
        public int Queue;
    }
}
