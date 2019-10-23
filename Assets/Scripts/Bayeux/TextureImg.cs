using BayeuxBundle.Models;
using UnityEngine;

namespace Assets.Scripts.Bayeux
{
    public class TextureImg : IImage<Texture2D>
    {
        public TextureImg(Texture2D texture)
        {
            Image = texture;
            Ymin = Image.height;
            Xmin = Image.width;
        }

        public Texture2D Image { get; }

        public int Height => Image.height;
        public int Width => Image.width;
        public int Xmin { get; set; }
        public int Xmax { get; set; }
        public int Ymin { get; set; }
        public int Ymax { get; set; }

        public void Apply()
        {
            Image.Apply();
        }

        public PixelInfo GetPixel(int x, int y)
        {
            Color32 color = Image.GetPixel(x, y);
            return new PixelInfo()
            {
                IsTransparent = color.a == 0,
                R = color.r,
                G = color.g,
                B = color.b
            };
        }

        public void SetPixel(int x, int y, PixelInfo color)
        {
            var c32 = new Color32(color.R, color.G, color.B, (byte)(color.IsTransparent? 0 : 255));
            Image.SetPixel(x, y, c32);
        }
    }
}
