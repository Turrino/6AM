using BayeuxBundle.Models;
using BayeuxBundle.Models.Instructions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BayeuxBundle.ImageTools
{
    public abstract class BaseImageTools<T> : IImageTools<T>
    {
        public BaseImageTools(Dictionary<ObjectType, string> overlayColorRef, string overlay, string skeleton, bool restrictedPoints = false)
        {
            AnchorOverlay = StringToColor(overlay);
            AnchorSkeleton = StringToColor(skeleton);
            OverlayColorRef = overlayColorRef
                .ToDictionary(kv => kv.Key, kv => StringToColor(kv.Value));
            RestrictedPoints = restrictedPoints;
        }

        public bool RestrictedPoints;
        public PixelInfo AnchorOverlay;
        public PixelInfo AnchorSkeleton;
        public Dictionary<ObjectType, PixelInfo> OverlayColorRef;

        public abstract IImage<T> BlankCanvas(int width, int height);
        public abstract IImage<T> BytesToImage(byte[] bytes, bool multiplyAlpha = false);
        public abstract PixelInfo StringToColor(string colorString);

        public Dictionary<PixelInfo, PixelInfo> ConvertPalette(Dictionary<string, string> original)
        {
            return original
                .ToDictionary(kv => StringToColor(kv.Key), kv => StringToColor(kv.Value));
        }

        public ImgPoint ExtractAnchor(IImage<T> image, PixelInfo anchorColor)
        {
            var point = ExtractAnchors(image, new[] { anchorColor })
                .FirstOrDefault();

            return point.Item2 ?? new ImgPoint(image.Width / 2, image.Height / 2);
        }

        public List<(PixelInfo, ImgPoint)> ExtractAnchors(IImage<T> image, PixelInfo[] anchorColors = null)
        {
            anchorColors = anchorColors ?? new[] { AnchorOverlay, AnchorSkeleton };
            var anchorPoints = new List<(PixelInfo, ImgPoint)>();

            for (var y = 0; y < image.Height; y++)
            {
                for (var x = 0; x < image.Width; x++)
                {
                    var color = image.GetPixel(x, y);
                    // "00FF26" & "FF00E5" are the default anchor colors - just an arbitrary choice. Change if needed.
                    if (anchorColors.Contains(color))
                    {
                        image.SetPixel(x, y, image.GetPixel(x - 1, y));
                        anchorPoints.Add((color, new ImgPoint(x, y)));
                        if (anchorColors.Length == anchorPoints.Count())
                        {
                            image.Apply();
                            return anchorPoints;
                        }
                    }
                }
            }

            return anchorPoints;
        }

        /// <param name="patternMode">Means the layer being applied to the canvas will only be applied if the pixel below is not transparent</param>
        /// <returns></returns>
        public IImage<T> Merge(IImage<T> canvas, IImage<T> layer,
            int xOffset = 0, int yOffset = 0, int canvasXOffset = 0, int canvasYOffset = 0,
            bool patternMode = false)
        {
            for (int y = yOffset, cY = canvasYOffset; y < layer.Height && cY < canvas.Height; y++, cY++)
            {
                for (int x = xOffset, cX = canvasXOffset; x < layer.Width && cX < canvas.Width; x++, cX++)
                {
                    var pixel = layer.GetPixel(x, y);
                    if (!pixel.IsTransparent)
                    {
                        if (!patternMode || patternMode && !canvas.GetPixel(cX, cY).IsTransparent)
                        {
                            canvas.SetPixel(cX, cY, pixel);
                        }
                    }
                }
            }
            return canvas;
        }

        public void ApplyPalette(IImage<T> image, Dictionary<PixelInfo, PixelInfo> palette)
        {
            for (var y = 0; y < image.Height; y++)
            {
                for (var x = 0; x < image.Width; x++)
                {
                    var color = image.GetPixel(x, y);
                    if (!color.IsTransparent && palette.ContainsKey(color))
                    {
                        image.SetPixel(x, y, palette[color]);
                    }
                }
            }
        }

        //TODO cleanup this, not readable and does too many things. Split up the methods by skeleton type and by overlay (with overlay/without)
        public void Merge(AssemblerResource<T> canvas, AssemblerResource<T> layer, SkeletonType skeletonType, Overlay overlay)
        {
            var newParts = new List<(ImgPoint point, IImage<T> Image, bool isPattern)>();

            if (overlay != null && overlay.SourceType.ToObjectType() != layer.Definition.ObjectType)
            {
                // Each overlay color will tell us how many parts we need,
                // i.e. if there are N overlay point for color X, then we need N parts
                // If the resource has multiple images, use those, otherwise just use one image multiple times.
                var offSets = overlay.Points[OverlayColorRef[layer.Definition.ObjectType]].Cast<ImgPoint>().ToList();

                if (offSets.Count > 1 && layer.Definition.Meta.CannotDuplicate)
                {
                    ImgPoint newOffset;
                    if (offSets.Count > 2)
                        newOffset = offSets.PickRandom();
                    else
                        newOffset = new ImgPoint((offSets[0].X + offSets[1].X) / 2, (offSets[0].Y + offSets[1].Y) / 2);

                    offSets = new List<ImgPoint>() { newOffset };
                }

                // "00FF26" & "FF00E5" are the default anchor colors - just an arbitrary choice. Change if needed.
                // TODO rewrite this, see anchor system overlay in garbler todo
                var anchorInUse = skeletonType == SkeletonType.Overlay ? AnchorOverlay : AnchorSkeleton;

                if (!layer.Definition.Meta.IsSet)
                {
                    var anchor = layer.Definition.Meta.MainAnchor
                        ?? ExtractAnchor(layer.Data, anchorInUse);

                    for (int i = 0; i < offSets.Count; i++)
                    {
                        AddPart(offSets[i], anchor, layer.Data, i > offSets.Count / 2);
                    }
                }
                else
                {
                    // TODO rewrite with new anchor system
                    throw new NotImplementedException("currently disabled, re-enable when needed & after anchor system overhaul");
                    //var imagesAndAnchors = layer.AdditionalImages.Select(t =>
                    //    new {
                    //        anchor = ExtractAnchor(t, anchorInUse),
                    //        image = t
                    //    }).ToArray();
                    //for (int i = 0, j = 0; i < offSets.Count; i++, j++)
                    //{
                    //    if (j >= imagesAndAnchors.Length)
                    //        j = 0;

                    //    var item = imagesAndAnchors[j];
                    //    AddPart(offSets[i], item.anchor, item.image, i >= offSets.Count / 2);
                    //}
                }

                // PastSplit: At the moment we only allow to split a layer in two halves. Improve this if needed.
                void AddPart(ImgPoint offset, ImgPoint anchor, IImage<T> image, bool PastSplit)
                {
                    var offsetAnchor = new ImgPoint(offset.X - anchor.X, offset.Y - anchor.Y);
                    if (PastSplit && layer.Instruction.DepthSplit > 0)
                    {
                        canvas.SplitLayers.Add(new SplitLayer<T>()
                        {
                            Anchor = offsetAnchor,
                            image = image,
                            Queue = layer.Instruction.DepthSplit
                        });
                    }
                    else
                    {
                        newParts.Add((offsetAnchor, image, layer.Instruction.IsPattern));
                    }
                }
            }
            else if (skeletonType == SkeletonType.Anchors)
            {
                // TODO rewrite with new anchor system
                if (canvas.IsEmpty)
                {
                    newParts.Add((new ImgPoint(
                        canvas.Data.Width / 2 - layer.Data.Width / 2,
                        canvas.Data.Height - layer.Data.Height),
                        layer.Data,
                        false));
                }
                else
                {
                    var layerAnchor = layer.Definition.Meta.MainAnchor ?? ExtractAnchor(layer.Data, AnchorOverlay);
                    var canvasAnchor = canvas.Definition.Meta.MainAnchor ?? ExtractAnchor(canvas.Data, AnchorOverlay);
                    var offsetAnchor = new ImgPoint(canvasAnchor.X - layerAnchor.X, canvasAnchor.Y - layerAnchor.Y, RestrictedPoints);
                    newParts.Add((offsetAnchor, layer.Data, layer.Instruction.IsPattern));
                }
            }
            else
            {
                newParts.Add((new ImgPoint(0, 0), layer.Data, layer.Instruction.IsPattern));
            }

            if (canvas.SplitLayers != null && canvas.SplitLayers.Any())
            {
                var insertions = canvas.SplitLayers
                .Where(l => l.Queue == 0);
                // Split layers out of the queue come out first
                newParts.InsertRange(0, insertions.Select(l => (l.Anchor, l.image, false)));
                canvas.SplitLayers = canvas.SplitLayers.Except(insertions).ToList();

                foreach (var splitLayer in canvas.SplitLayers)
                {
                    splitLayer.Queue--;
                }
            }

            foreach (var part in newParts)
            {
                Merge(canvas.Data, part.Image, 0, 0, part.point.X, part.point.Y, part.isPattern);
                canvas.IsEmpty = false;
            }
        }
        
        public IImage<T> MergeToImage(byte[] canvas, byte[] layer)
        {
            return Merge(BytesToImage(canvas), BytesToImage(layer));
        }

        public Overlay ReadOverlayData(byte[] bytes, ObjectType sourceType)
        {
            var image = BytesToImage(bytes);

            var colors = new Dictionary<PixelInfo, List<OverlayPoint>>();

            for (var y = 0; y < image.Height; y++)
            {
                for (var x = 0; x < image.Width; x++)
                {
                    var color = image.GetPixel(x, y);
                    if (!color.IsTransparent)
                    {
                        var point = new OverlayPoint(x, y, image.Height);
                        if (colors.ContainsKey(color))
                        {
                            colors[color].Add(point);
                        }
                        else
                        {
                            colors[color] = new List<OverlayPoint>() { point };
                        }
                    }
                }
            }

            return new Overlay(colors, image.Width, image.Height, sourceType.ToString());
        }

        public IImage<T> ResizeCanvas(IImage<T> source, int newWidth, int newHeight)
        {
            var newCanvas = BlankCanvas(newWidth, newHeight);
            return Merge(newCanvas, source, newWidth / 2 - source.Width / 2, newHeight - source.Height);
        }

        public IImage<T> Trim(IImage<T> source)
        {            
            for (int y = 0; y < source.Height; y++)
            {
                for (int x = 0; x < source.Width; x++)
                {
                    var pxl = source.GetPixel(x, y);
                    if (!pxl.IsTransparent)
                    {
                        if (x < source.Xmin)
                            source.Xmin = x - 1;
                        else if (x > source.Xmax)
                            source.Xmax = x + 1;
                        if (y > source.Ymax)
                            source.Ymax = y + 1;
                        else if (y < source.Ymin)
                            source.Ymin = y - 1;
                    }
                }
            }

            source.Ymin = source.Ymin >= 0 ? source.Ymin : 0;
            source.Xmin = source.Xmin >= 0 ? source.Xmin : 0;

            var canvas = BlankCanvas(source.Xmax - source.Xmin, source.Ymax - source.Ymin);

            return Merge(canvas, source, source.Xmin, source.Ymin);
        }

        public IImage<T> PixelPerfectCircle(int radius, PixelInfo color)
        {
            var image = BlankCanvas(radius*2, radius*2);
            var center = image.Width / 2;
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    if (Math.Pow(x - center, 2) + Math.Pow(y - center, 2) < Math.Pow(radius, 2))
                        image.SetPixel(x-1, y-1, color);
                }
            }
            return image;
        }

        public Dictionary<PixelInfo, int> ColorInfo(IImage<T> image)
        {
            var colorDict = new Dictionary<PixelInfo, int>();

            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    var color = image.GetPixel(x, y);

                    if (color.IsTransparent)
                        continue;
                    if (colorDict.ContainsKey(color))
                        colorDict[color]++;
                    else
                        colorDict[color] = 1;
                }
            }

            return colorDict;
        }
    }
}
