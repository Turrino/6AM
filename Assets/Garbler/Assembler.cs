using BayeuxBundle.ImageTools;
using BayeuxBundle.Models;
using BayeuxBundle.Models.Instructions;
using BayeuxBundle.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace BayeuxBundle
{
    public class Assembler<T>
    {
        public LoaderV2 Loader;
        public IImageTools<T> ImageTools;

        public Assembler(IImageTools<T> imageTools, List<GarblerItem> res, Dictionary<string, ParsedMetaPoco> additonalMeta)
        {
            Loader = new LoaderV2(res, additonalMeta);
            ImageTools = imageTools;
        }

        private IImage<T> LoadOrRegenerate(string componentType, int componentIndex, List<GarblerItem> components)
        {
            if (!components.Any())
            {
                components = Loader.GetGenericResources()[componentType][componentIndex];
            }

            var bytes = Loader.LoadImage(components.PopRandom().Content);

            return ImageTools.BytesToImage(bytes);
        }

        public List<string> GetAllSubtypes(ObjectType mainType)
        {
            var allitemsWithType = Loader.AllResourcesDefinitions.Where(r => r.ObjectType == mainType).ToList();

            return allitemsWithType.Select(x => x.SubTypes)
                .SelectMany(x => x)
                .Distinct()
                .ToList();
        }

        public AssemblerResource<T> Assemble(Instructions instructions)
        {
            var resources = new List<AssemblerResource<T>>();
            Overlay overlay = null;

            foreach (var instructionLayer in instructions)
            {
                // This goes through the current layer ie new List<Enum>() { Shape.round, ObjectType.vase }
                var props = instructionLayer.Types.Select(l =>
                new
                    {
                        propInfo = ResourceDefinition.DefinitionPropInfo.SingleOrDefault(
                               p => p.PropertyType == l.GetType()),
                        value = l
                    }
                );

                // Then loops through all the resources definitions until it finds one
                // that has ALL the required properties set as described.
                // If we end up with a lot of assets, have the loader create indexes for individual properties in a definition.
                var definitions = Loader.TypeResourceDefinitions.Where(d => props
                    .All(p => Equals(p.propInfo.GetValue(d), p.value)))
                    .ToList();

                var definition = string.IsNullOrEmpty(instructionLayer.SubType) ?
                    definitions.PickRandom() :
                    definitions.Where(d => d.SubTypes.Contains(instructionLayer.SubType)).ToList().PickRandom();

                if (instructions.UseOnce)
                {
                    Loader.TypeResourceDefinitions.Remove(definition);
                }

                AssemblerResource<T> resource;

                if (!definition.Meta.IsSet)
                    resource = new AssemblerResource<T>(definition,
                        ImageTools.BytesToImage(Loader.LoadImageByName(definition.Meta.ResourceName)),
                        instructionLayer);
                else
                    resource = new AssemblerResource<T>(definition,
                        definition.Meta.ResourceNames.Select(r =>
                            ImageTools.BytesToImage(Loader.LoadImageByName(r))).ToList(),
                        instructionLayer);

                resources.Add(resource);

                
                if (instructionLayer.HasOverlay)
                {
                    overlay = definition.Meta.Overlay;
                    // deprecated, remove
                    //var overlayBytes = _loader.GetMatchingOverlay(definition.Meta.ResourceName);
                    //overlay = _imageTools.ReadOverlayData(overlayBytes, definition.ObjectType);
                }

            }

            //TODO all these parameters are stupid, fix it
            return Assemble(resources, instructions.SkeletonType, overlay, instructions.MainType, instructions.Palette);
        }

        public AssemblerResource<T> Assemble(List<AssemblerResource<T>> resources,
            SkeletonType skeletonType,
            Overlay overlay = null,
            ObjectType mainType = ObjectType.na,
            Dictionary<PixelInfo, PixelInfo> palette = null)
        {
            var useAnchors = skeletonType == SkeletonType.Anchors;

            if (overlay != null || skeletonType == SkeletonType.Anchors)
            {                
                var width = !useAnchors ? overlay.Width : resources.Max(r => r.Data.Width);
                var height = !useAnchors ? overlay.Height : resources.Max(r => r.Data.Height);
                var canvas = ImageTools.BlankCanvas(width, height);
                resources.Insert(0, AssemblerResource<T>.Canvas(canvas));
            }
            var assembled = resources.Aggregate((res, next) => Merge(res, next, skeletonType, overlay));
            CleanupLayers(assembled, skeletonType);

            // TODO, IMPORTANT - this somehow breaks the loader's definitions!
            // It replaces the type in the original resource definition 
            if (mainType != ObjectType.na)
                assembled.Definition.ObjectType = mainType;

            assembled.Data = ImageTools.Trim(assembled.Data);
            assembled.Colors = ImageTools.ColorInfo(assembled.Data);

            if (palette != null)
                ImageTools.ApplyPalette(assembled.Data, palette);

            assembled.Data.Apply();

            return assembled;
        }

        public AssemblerResource<T> Merge(AssemblerResource<T> resource, AssemblerResource<T> layer, SkeletonType skeletonType, Overlay overlay = null)
        {
            // If we need to keep track of individual layers,
            // have the canvas store their metadata in a list
            ImageTools.Merge(resource, layer, skeletonType, overlay);
            if (!resource.IsEmpty)
            {
                resource.Definition.Merge(layer.Definition);
            }
            return resource;
        }

        public void CleanupLayers(AssemblerResource<T> res, SkeletonType skeletonType)
        {
            if (skeletonType == SkeletonType.Anchors)
            {
                ImageTools.ExtractAnchors(res.Data);
            }

            if (res.SplitLayers != null && res.SplitLayers.Any())
            {
                foreach (var layer in res.SplitLayers)
                {
                    ImageTools.Merge(res.Data, layer.image, 0, 0, layer.Anchor.X, layer.Anchor.Y);
                }
            }
        }

        /// <summary>
        /// Used for simple images that have the same size and are sorted in layers (0-n)
        /// </summary>
        public IImage<T> AssembleGeneric(string nameOfType)
        {
            var componentsLists = Loader.GenericResources[nameOfType];
            var layersCount = componentsLists.Count;
            IImage<T> canvas = null;

            for (int i = 0; i < layersCount; i++)
            {
                var nextLayer = LoadOrRegenerate(nameOfType, i, componentsLists[i]);
                if (canvas == null)
                {
                    canvas = nextLayer;
                }
                ImageTools.Merge(canvas, nextLayer);
            }

            // Apply changes outside the loop to save re-drawing the asset on every pass
            canvas.Apply();
            //if (trim)
            //    canvas.Trim();

            return canvas;
        }

        public ParsedPolyRes<T> AssemblePoly(string resourceType)
        {
            var pickRandom = Loader.PolyResources[resourceType].PopRandom();
            return AssemblePoly(pickRandom);
        }

        public ParsedPolyRes<T> AssembleNamedPoly(string resourceName)
        {
            // TODO have the loader save the resource name (= folder name) as well instead of searching for it..
            var result = Loader.PolyResources
                .SelectMany(l => l.Value)
                .FirstOrDefault(l => l.Any(s => s.Path.Contains(resourceName)));

            return AssemblePoly(result);
        }

        public ParsedPolyRes<T> AssemblePoly(List<GarblerItem> polyResList)
        {
            var overlayResource = polyResList.SingleOrDefault(i => i.Path.EndsWith("o.png"));
            var overlayData = overlayResource != null ?
                ImageTools.ReadOverlayData(Loader.LoadImage(overlayResource.Content), ObjectType.overlay) : null;
            polyResList.Remove(overlayResource);

            var colliderResource = polyResList.SingleOrDefault(i => i.Path.EndsWith("c.png"));
            var collider = colliderResource != null ?
                ImageTools.BytesToImage(Loader.LoadImage(colliderResource.Content)) : null;
            polyResList.Remove(colliderResource);

            var backgroundAndForeGroundImages = polyResList.GroupBy(r => r.Path.EndsWith("t.png"));

            var backgrounds = backgroundAndForeGroundImages.SingleOrDefault(g => !g.Key)
                .Select(r => ImageTools.BytesToImage(Loader.LoadImage(r.Content)))
                .ToList();

            var foregrounds = backgroundAndForeGroundImages.SingleOrDefault(g => g.Key)?
                .Select(r => ImageTools.BytesToImage(Loader.LoadImage(r.Content)))?
                .ToList();

            return new ParsedPolyRes<T>(backgrounds, foregrounds, collider, overlayData);
        }
    }
}
