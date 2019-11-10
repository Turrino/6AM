//using BayeuxBundle;
//using BayeuxBundle.ImageTools;
//using BayeuxBundle.Models;
//using BayeuxBundle.Models.Instructions;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Threading.Tasks;

//namespace Assets.Scripts.Bayeux
//{    public class Assembler2<T>
//    {
//        public bool FlipAnchors;
//        private ResourceLoader _loader;
//        private IImageTools<T> _imageTools;

//        public Assembler2(IImageTools<T> imageTools, Assembly assembly, bool flipAnchors)
//        {
//            _loader = new ResourceLoader(assembly);
//            _imageTools = imageTools;
//            if (flipAnchors)
//                FlipAllAnchors();
//        }

//        private void FlipAllAnchors()
//        {
//            FlipAnchors = true;
//            foreach (var def in _loader.AllResourcesDefinitions)
//            {
//                if (def.Meta.Overlay != null)
//                {
//                    foreach (var points in def.Meta.Overlay.Points.Values)
//                    {
//                        foreach (var point in points)
//                        {
//                            point.Y = point.FlipWiseY;
//                        }
//                    }
//                }
//            }
//        }

//        private IImage<T> LoadOrRegenerate(string componentType, int componentIndex, List<string> components)
//        {
//            if (!components.Any())
//            {
//                components = _loader.GetGenericResources()[componentType][componentIndex];
//            }

//            var bytes = _loader.LoadImage(components.PopRandom());

//            return _imageTools.BytesToImage(bytes);
//        }

//        public List<string> GetAllSubtypes(ObjectType mainType)
//        {
//            return _loader.ResourcesMetaData[mainType]
//                .Select(x => x.Value.Types)
//                .SelectMany(x => x)
//                .Distinct()
//                .ToList();
//        }

//        public AssemblerResource<T> Assemble(Instructions instructions)
//        {
//            var resources = new List<AssemblerResource<T>>();
//            Overlay overlay = null;

//            foreach (var instructionLayer in instructions)
//            {
//                // This goes through the current layer ie new List<Enum>() { Shape.round, ObjectType.vase }
//                var props = instructionLayer.Types.Select(l =>
//                new
//                {
//                    propInfo = ResourceDefinition.DefinitionPropInfo.SingleOrDefault(
//                               p => p.PropertyType == l.GetType()),
//                    value = l
//                }
//                );

//                // Then loops through all the resources definitions until it finds one
//                // that has ALL the required properties set as described.
//                // If we end up with a lot of assets, have the loader create indexes for individual properties in a definition.
//                var definitions = _loader.TypeResourceDefinitions.Where(d => props
//                    .All(p => Equals(p.propInfo.GetValue(d), p.value)))
//                    .ToList();

//                var definition = string.IsNullOrEmpty(instructionLayer.SubType) ?
//                    definitions.PickRandom() :
//                    definitions.Where(d => d.SubTypes.Contains(instructionLayer.SubType)).ToList().PickRandom();

//                if (instructions.UseOnce)
//                {
//                    _loader.TypeResourceDefinitions.Remove(definition);
//                }

//                AssemblerResource<T> resource;

//                if (!definition.Meta.IsSet)
//                    resource = new AssemblerResource<T>(definition,
//                        _imageTools.BytesToImage(_loader.LoadImage(definition.Meta.ResourceName)),
//                        instructionLayer);
//                else
//                    resource = new AssemblerResource<T>(definition,
//                        definition.Meta.ResourceNames.Select(r =>
//                            _imageTools.BytesToImage(_loader.LoadImage(r))).ToList(),
//                        instructionLayer);

//                if (resource.Definition.Meta.MainAnchor != null && FlipAnchors)
//                {
//                    resource.Definition.Meta.MainAnchor.Y = resource.Data.Height - resource.Definition.Meta.MainAnchor.Y;
//                }


//                resources.Add(resource);


//                if (instructionLayer.HasOverlay)
//                {
//                    overlay = definition.Meta.Overlay;
//                    // deprecated, remove
//                    //var overlayBytes = _loader.GetMatchingOverlay(definition.Meta.ResourceName);
//                    //overlay = _imageTools.ReadOverlayData(overlayBytes, definition.ObjectType);
//                }

//            }

//            //TODO all these parameters are stupid, fix it
//            return Assemble(resources, instructions.SkeletonType, overlay, instructions.MainType, instructions.Palette);
//        }

//        public AssemblerResource<T> Assemble(List<AssemblerResource<T>> resources,
//            SkeletonType skeletonType,
//            Overlay overlay = null,
//            ObjectType mainType = ObjectType.na,
//            Dictionary<PixelInfo, PixelInfo> palette = null)
//        {
//            var useAnchors = skeletonType == SkeletonType.Anchors;

//            if (overlay != null || skeletonType == SkeletonType.Anchors)
//            {
//                var width = !useAnchors ? overlay.Width : resources.Max(r => r.Data.Width);
//                var height = !useAnchors ? overlay.Height : resources.Max(r => r.Data.Height);
//                var canvas = _imageTools.BlankCanvas(width, height);
//                resources.Insert(0, AssemblerResource<T>.Canvas(canvas));
//            }
//            var assembled = resources.Aggregate((res, next) => Merge(res, next, skeletonType, overlay));
//            CleanupLayers(assembled, skeletonType);

//            if (mainType != ObjectType.na)
//                assembled.Definition.ObjectType = mainType;

//            assembled.Data = _imageTools.Trim(assembled.Data);
//            assembled.Colors = _imageTools.ColorInfo(assembled.Data);

//            if (palette != null)
//                _imageTools.ApplyPalette(assembled.Data, palette);

//            assembled.Data.Apply();

//            return assembled;
//        }

//        public AssemblerResource<T> Merge(AssemblerResource<T> resource, AssemblerResource<T> layer, SkeletonType skeletonType, Overlay overlay = null)
//        {
//            // If we need to keep track of individual layers,
//            // have the canvas store their metadata in a list
//            _imageTools.Merge(resource, layer, skeletonType, overlay);
//            if (!resource.IsEmpty)
//            {
//                resource.Definition.Merge(layer.Definition);
//            }
//            return resource;
//        }

//        public void CleanupLayers(AssemblerResource<T> res, SkeletonType skeletonType)
//        {
//            if (skeletonType == SkeletonType.Anchors)
//            {
//                _imageTools.ExtractAnchors(res.Data);
//            }

//            if (res.SplitLayers != null && res.SplitLayers.Any())
//            {
//                foreach (var layer in res.SplitLayers)
//                {
//                    _imageTools.Merge(res.Data, layer.image, 0, 0, layer.Anchor.X, layer.Anchor.Y);
//                }
//            }
//        }

//        /// <summary>
//        /// Used for simple images that have the same size and are sorted in layers (0-n)
//        /// </summary>
//        public IImage<T> AssembleGeneric(string nameOfType)
//        {
//            var componentsLists = _loader.GenericResources[nameOfType];
//            var layersCount = componentsLists.Count;
//            IImage<T> canvas = null;

//            for (int i = 0; i < layersCount; i++)
//            {
//                var nextLayer = LoadOrRegenerate(nameOfType, i, componentsLists[i]);
//                if (canvas == null)
//                {
//                    canvas = nextLayer;
//                }
//                _imageTools.Merge(canvas, nextLayer);
//            }

//            // Apply changes outside the loop to save re-drawing the asset on every pass
//            canvas.Apply();
//            //if (trim)
//            //    canvas.Trim();

//            return canvas;
//        }

//        public ParsedPolyRes<T> AssemblePoly(string resourceType)
//        {
//            var pickRandom = _loader.PolyResources[resourceType].PopRandom();
//            return AssemblePoly(pickRandom);
//        }

//        public ParsedPolyRes<T> AssembleNamedPoly(string resourceName)
//        {
//            // TODO have the loader save the resource name (= folder name) as well instead of searching for it..
//            var result = _loader.PolyResources
//                .SelectMany(l => l.Value)
//                .FirstOrDefault(l => l.Any(s => s.Contains(resourceName)));

//            return AssemblePoly(result);
//        }

//        public ParsedPolyRes<T> AssemblePoly(List<string> polyResList)
//        {
//            var overlayResource = polyResList.SingleOrDefault(i => i.EndsWith("o.png"));
//            var overlayData = overlayResource != null ?
//                _imageTools.ReadOverlayData(_loader.LoadImage(overlayResource), ObjectType.overlay) : null;
//            polyResList.Remove(overlayResource);

//            var colliderResource = polyResList.SingleOrDefault(i => i.EndsWith("c.png"));
//            var collider = colliderResource != null ?
//                _imageTools.BytesToImage(_loader.LoadImage(colliderResource)) : null;
//            polyResList.Remove(colliderResource);

//            var backgroundAndForeGroundImages = polyResList.GroupBy(r => r.EndsWith("t.png"));

//            var backgrounds = backgroundAndForeGroundImages.SingleOrDefault(g => !g.Key)
//                .Select(r => _imageTools.BytesToImage(_loader.LoadImage(r)))
//                .ToList();

//            var foregrounds = backgroundAndForeGroundImages.SingleOrDefault(g => g.Key)?
//                .Select(r => _imageTools.BytesToImage(_loader.LoadImage(r)))?
//                .ToList();

//            return new ParsedPolyRes<T>(backgrounds, foregrounds, collider, overlayData);
//        }
//    }
//}
