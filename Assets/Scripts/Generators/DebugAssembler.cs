//using BayeuxBundle;
//using BayeuxBundle.ImageTools;
//using BayeuxBundle.Models;
//using BayeuxBundle.Models.Instructions;
//using System.Collections.Generic;
//using System.Linq;

//namespace Assets.Scripts.Generators
//{
//    public class DebugAssembler<T>
//    {
//        private ResourceLoader _loader;
//        private IImageTools<T> _imageTools;

//        public DebugAssembler(IImageTools<T> imageTools)
//        {
//            _loader = new ResourceLoader();
//            _imageTools = imageTools;
//        }

//        private Resource LoadResource(ObjectType objectType)
//        {
//            var name = _loader.ResourcesByType[objectType].PopRandom().Meta.ResourceName;
//            return _loader.LoadResource(name);
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

//        //public AssemblerResource Assemble(IEnumerable<ObjectType> layers,
//        //    ObjectType mainType = ObjectType.na)
//        //{
//        //    // TODO from the instructions, count the resources name.
//        //    // Verify that the required resources exist and have enough items
//        //    // If they don't, return a useful message back
//        //    var resources = layers.Select(l => LoadResource(l)).ToList();
//        //    return Assemble(resources, null, mainType);
//        //}

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
//                           p => p.PropertyType == l.GetType()),
//                    value = l
//                }
//                );

//                // Then loops through all the resources definitions until it finds one
//                // that has ALL the required properties set as described.
//                // If we end up with a lot of assets, have the loader create indexes for individual properties in a definition.
//                var definition = _loader.TypeResourceDefinitions.Where(d => props
//                    .All(p => Equals(p.propInfo.GetValue(d), p.value)))
//                    .ToList().PickRandom();

//                if (instructions.UseOnce)
//                {
//                    _loader.TypeResourceDefinitions.Remove(definition);
//                }

//                var baseResource = _loader.LoadResource(definition.Meta.ResourceName);
//                AssemblerResource<T> resource;

//                if (!baseResource.Definition.Meta.IsSet)
//                    resource = new AssemblerResource<T>(baseResource.Definition,
//                        _imageTools.BytesToImage(baseResource.Image),
//                        instructionLayer);
//                else
//                    resource = new AssemblerResource<T>(baseResource.Definition,
//                        baseResource.AdditionalImages.Select(i => _imageTools.BytesToImage(i)).ToList(),
//                        instructionLayer);

//                resources.Add(resource);

//                if (instructionLayer.HasOverlay)
//                {
//                    var overlayBytes = _loader.GetMatchingOverlay(definition.Meta.ResourceName);
//                    overlay = _imageTools.ReadOverlayData(overlayBytes, definition.ObjectType);
//                }
//            }

//            return Assemble(resources, instructions.SkeletonType, overlay, instructions.MainType);
//        }

//        public AssemblerResource<T> Assemble(List<AssemblerResource<T>> resources,
//            SkeletonType skeletonType,
//            Overlay overlay = null,
//            ObjectType mainType = ObjectType.na)
//        {
//            if (overlay != null)
//            {
//                var canvas = _imageTools.BlankCanvas(overlay.Width, overlay.Height);
//                resources.Insert(0, AssemblerResource<T>.Canvas(canvas));
//            }
//            var assembled = resources.Aggregate((res, next) => Merge(res, next, skeletonType, overlay));
//            CleanupLayers(assembled);

//            if (mainType != ObjectType.na)
//                assembled.Definition.ObjectType = mainType;

//            assembled.Data = _imageTools.Trim(assembled.Data);
//            assembled.Data.Apply();

//            return assembled;
//        }

//        public AssemblerResource<T> Merge(AssemblerResource<T> resource, AssemblerResource<T> layer, SkeletonType skeletonType, Overlay overlay = null)
//        {
//            // If we need to keep track of individual layers,
//            // have the canvas store their metadata in a list
//            _imageTools.Merge(resource, layer, skeletonType, overlay);
//            if (!resource.IsCanvas)
//            {
//                resource.Definition.Merge(layer.Definition);
//            }
//            return resource;
//        }

//        public void CleanupLayers(AssemblerResource<T> res)
//        {
//            if (res.SplitLayers != null && res.SplitLayers.Any())
//                _imageTools.MergeParts(res.SplitLayers.Select(l => (l.Anchor, l.image)), res);
//        }

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

//        public ParsedPolyRes<T> AssemblePoly(string resourceName)
//        {
//            var polyResList = _loader.PolyResources[resourceName].PopRandom();

//            var overlayResource = polyResList.SingleOrDefault(i => i.EndsWith("o.png"));
//            var overlayData = overlayResource != null ?
//                _imageTools.ReadOverlayData(_loader.LoadImage(overlayResource), ObjectType.overlay) : null;
//            polyResList.Remove(overlayResource);

//            var colliderResource = polyResList.SingleOrDefault(i => i.EndsWith("c.png"));
//            var collider = colliderResource != null ?
//                _imageTools.BytesToImage(_loader.LoadImage(colliderResource)) : null;
//            polyResList.Remove(colliderResource);


//            var polyList = polyResList.Select(r => _imageTools.BytesToImage(_loader.LoadImage(r)))
//                .ToList();

//            return new ParsedPolyRes<T>(polyList, collider, overlayData);
//        }
//    }
//}
