using System;
using System.Collections.Generic;
using System.Text;

namespace BayeuxBundle.Models
{
    public interface IImage<T>
    {
        T Image { get; }
        int Height { get; }
        int Width { get; }
        int Xmin { get; set; }
        int Xmax { get; set; }
        int Ymin { get; set; }
        int Ymax { get; set; }
        PixelInfo GetPixel(int x, int y);
        void SetPixel(int x, int y, PixelInfo color);
        void Apply();

    }
}
