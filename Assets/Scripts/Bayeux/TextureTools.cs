using BayeuxBundle.ImageTools;
using BayeuxBundle.Models;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Bayeux
{
    public class TextureTools : BaseImageTools<Texture2D>
    {
        public TextureTools(Dictionary<ObjectType, string> overlayReference) : base(overlayReference, "#00FF26", "#FF00E5")
        { }

        public override IImage<Texture2D> BlankCanvas(int width, int height)
        {
            var tex = new Texture2D(width, height)
            {
                filterMode = FilterMode.Point
            };
            var alpha = new Color(0, 0, 0, 0);
            for (var y = 0; y < tex.height; y++)
            {
                for (var x = 0; x < tex.width; x++)
                {
                    tex.SetPixel(x, y, alpha);
                }
            }
            tex.Apply();
            return new TextureImg(tex);
        }

        public override IImage<Texture2D> BytesToImage(byte[] bytes, bool multiplyAlpha = false)
        {
            var tex = new Texture2D(1, 1)
            {
                filterMode = FilterMode.Point
            };
            tex.LoadImage(bytes);
            if (multiplyAlpha)
            {
                tex.PremultiplyAlpha();
                tex.Apply();
            }
            return new TextureImg(tex);
        }

        public Color32 StringToColor32(string colorString)
        {
            ColorUtility.TryParseHtmlString(colorString, out Color color);
            return color;
        }

        public override PixelInfo StringToColor(string colorString)
        {
            var parsed = ColorUtility.TryParseHtmlString(colorString, out Color color);
            if (!parsed)
            {
                Debug.Log($"Couldn't parse color string! {colorString}");
            }
            Color32 c32 = color;
            return new PixelInfo(c32.r, c32.g, c32.b, 255);
        }

        public Texture2D FlipTexture(Texture2D original)
        {
            Texture2D flipped = new Texture2D(original.width, original.height)
            {
                filterMode = FilterMode.Point
            };

            int xN = original.width;
            int yN = original.height;


            for (int i = 0; i < xN; i++)
            {
                for (int j = 0; j < yN; j++)
                {
                    flipped.SetPixel(xN - i - 1, j, original.GetPixel(i, j));
                }
            }
            flipped.Apply();

            return flipped;
        }
    }
}
