using BayeuxBundle.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BayeuxBundle.ImageTools
{
    public interface IImageTools<T>
    {
        IImage<T> BytesToImage(byte[] bytes, bool multiplyAlpha = false);

        IImage<T> BlankCanvas(int width, int height);

        IImage<T> MergeToImage(byte[] canvas, byte[] layer);

        IImage<T> Merge(IImage<T> canvas, IImage<T> layer,
            int xOffset = 0, int yOffset = 0, int canvasXOffset = 0, int canvasYOffset = 0,
            bool patternMode = false);

        void Merge(AssemblerResource<T> canvas, AssemblerResource<T> layer, SkeletonType skeletonType, Overlay overlay);

        ImgPoint ExtractAnchor(IImage<T> image, PixelInfo anchorColor);

        void ApplyPalette(IImage<T> resource, Dictionary<PixelInfo, PixelInfo> palette);

        List<(PixelInfo, ImgPoint)> ExtractAnchors(IImage<T> image, PixelInfo[] anchorColors = null);

        //Anchors ExtractAnchors(IImage<T> image);

        Overlay ReadOverlayData(byte[] image, ObjectType sourceType);

        IImage<T> Trim(IImage<T> source);

        Dictionary<PixelInfo, PixelInfo> ConvertPalette(Dictionary<string, string> original);

        PixelInfo StringToColor(string colorString);

        IImage<T> PixelPerfectCircle(int radius, PixelInfo color);

        Dictionary<PixelInfo, int> ColorInfo(IImage<T> image);
    }
}
