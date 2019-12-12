using bayeux;
using BayeuxBundle.Models;
using BayeuxBundle.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BayeuxBundle
{
    public class LoaderV2
    {
        //TODO fix the additional metadata bullshit
        public LoaderV2(List<GarblerItem> resources,
            Dictionary<string, ParsedMetaPoco> additonalMeta)
        {
            ResDict = new ResourceDictionary();

            //var resourcesFile = useFlipWiseY ?
            //    names.SingleOrDefault(r => r.EndsWith("resources_fwY.json"))
            //    : names.SingleOrDefault(r => r.EndsWith("resources.json"));

            //ParsedAdditionalMeta = JsonConvert
            //    .DeserializeObject<Dictionary<string, ParsedMetaPoco>>(
            //    LoadText(resourcesFile));

            ParsedAdditionalMeta = additonalMeta;

            _allResources = resources;

            _contentsByResourceName = _allResources.ToDictionary(k => k.Path, k => k.Content);

            // Animations currently on hold..
            //_allResourceNames = names.Where(i => !AnimationPattern.IsMatch(i)).ToArray();
            //_allAnimationFrames = names.Where(i => AnimationPattern.IsMatch(i)).ToArray();
            _genericResources = _allResources.Where(n => n.Path.Contains("GenericResources")).ToList();

            TypedResourcesOverlays = new Dictionary<string, string>();
            //ResourceSets = new Dictionary<string, List<ResourceDefinition>>();
            ParsedResourceDefinitions = new Dictionary<string, ResourceDefinition>();

            TypeResourceDefinitions = GetTypeResourcesDefinitions();
            ResourcesByType = GetObjectTypesDictionary();
            PolyResources = GetPolyResources();
            GenericResources = GetGenericResources();
        }

        public Dictionary<ObjectType, List<ResourceDefinition>> ResourcesByType;
        public Dictionary<string, List<List<GarblerItem>>> PolyResources;
        public Dictionary<string, List<List<GarblerItem>>> GenericResources;
        public List<ResourceDefinition> TypeResourceDefinitions;
        public Dictionary<string, ResourceDefinition> ParsedResourceDefinitions;
        public Dictionary<string, string> TypedResourcesOverlays;
        //public Dictionary<string, List<ResourceDefinition>> ResourceSets;
        public List<ResourceDefinition> AllResourcesDefinitions;
        // TODO remove text definitions, they're part of the poco now
        //public Dictionary<ObjectType, Dictionary<string, TextDefinitions>> ResourcesMetaData;
        public Dictionary<string, ParsedMetaPoco> ParsedAdditionalMeta;

        private Regex FlagPattern = new Regex("([A-Za-z]{1}[0-9]{1,9}|[Uu]{1}[Qq]{1})");
        // Match anything that isn't the first frame
        //private Regex AnimationPattern = new Regex("[aA]{1}([2-9]{1}|[0-9]{2}).[A-Za-z]{3}$");
        private ResourceDictionary ResDict;
        public readonly List<GarblerItem> _genericResources;
        private readonly List<GarblerItem> _allResources;
        private readonly Dictionary<string, string> _contentsByResourceName;
        //private readonly string[] _allAnimationFrames;

        //public Dictionary<ObjectType, Dictionary<string, TextDefinitions>> ReadResourceMetaData(IEnumerable<string> jsonMetadata)
        //{
        //    var definitions = jsonMetadata
        //        .Select(j => new { name = j, contents = JsonConvert.DeserializeObject<Dictionary<string, TextDefinitions>>(LoadText(j)) } )
        //        .ToDictionary(kv => (ObjectType)Enum.Parse(typeof(ObjectType), kv.name.Split('.')[3]), kv => kv.contents);

        //    return definitions;
        //}

        public string LoadText(GarblerItem resource)
        {
            byte[] data = Convert.FromBase64String(resource.Content);
            return Encoding.UTF8.GetString(data);
        }

        public byte[] LoadImage(string resourceContents)
        {
            return Convert.FromBase64String(resourceContents);
        }

        public byte[] LoadImageByName(string resourceName)
        {
            return LoadImage(_contentsByResourceName[resourceName]);
        }

        //public byte[] GetMatchingOverlay(string resourceName)
        //{
        //    return LoadImage(TypedResourcesOverlays[resourceName]);
        //}

        //public Dictionary<string, List<string>> GetEmbeddedResources()
        //{
        //    var allResourceNames = _allResourceNames
        //        .Where(r => r.Contains("Resources"));

        //    var allResourceNamesDict = allResourceNames
        //        .GroupBy(r => r.Split('.')[2])
        //        .ToDictionary(
        //        kv => kv.Key,
        //        kv => kv.ToList());
        //    return allResourceNamesDict;
        //}

        public List<ResourceDefinition> GetTypeResourcesDefinitions()
        {
            var resourceDefinitionsUngrouped = _allResources
                .Where(r => r.Path.Contains("TypeResources") || r.Path.Contains("ResourceGroups"))
                .Select(r => GetDefinition(r.Path));

            var resourcesGrouped = resourceDefinitionsUngrouped
                .GroupBy(r => string.IsNullOrEmpty(r.Meta.SetName));

            AllResourcesDefinitions = resourcesGrouped.Single(g => g.Key == true).ToList();

            var resourceSets = resourcesGrouped.SingleOrDefault(g => g.Key == false);

            if (resourceSets != null)
            {
                var groupedSets = resourceSets
                    .GroupBy(i => i.Meta.SetName)
                    .Select(g => g.Select(r => r.Meta.ResourceName).OrderBy(r => r).ToList());

                foreach (var set in groupedSets)
                {
                    var updatedDefinition = ParsedResourceDefinitions[set.FirstOrDefault()];
                    updatedDefinition.Meta.ResourceNames = set;
                    AllResourcesDefinitions.Add(updatedDefinition);
                }
            }

            return AllResourcesDefinitions;
        }

        public Dictionary<ObjectType, List<ResourceDefinition>> GetObjectTypesDictionary()
        {
            return AllResourcesDefinitions
                .GroupBy(d => d.ObjectType)
                .ToDictionary(
                kv => kv.Key,
                kv => kv.ToList());
        }

        public Dictionary<string, List<List<GarblerItem>>> GetGenericResources()
        {
            var allResources = _genericResources
                .Select(r => new { split = r.Path.Split('.'), fullname = r });

            // TODO create a specific class for this type of resource that has a list of available components
            return allResources
                .GroupBy(r => new { type = r.split[r.split.Length - 4] })
                .ToDictionary(
                kv => kv.Key.type,
                kv => kv
                    .GroupBy(r => r.split[r.split.Length - 3])
                    .Select(g => g.Select(e => e.fullname).ToList()).ToList());
        }

        public Dictionary<string, List<List<GarblerItem>>> GetPolyResources()
        {
            var allResourceNames = _allResources
                .Where(r => r.Path.Contains("PolyResources"))
                .Select(r => new { split = r.Path.Split('.'), fullname = r });

            return allResourceNames
                .GroupBy(r => new { type = r.split[2] })
                .ToDictionary(
                kv => kv.Key.type,
                kv => kv
                    .GroupBy(r => r.split[3])
                    .Select(g => g.Select(e => e.fullname).ToList()).ToList());
        }

        public ResourceDefinition GetDefinition(string resourceName)
        {
            if (ParsedResourceDefinitions.ContainsKey(resourceName))
                return ParsedResourceDefinitions[resourceName];

            var fullnameSplit = resourceName.Split('.');
            var groupName = fullnameSplit[fullnameSplit.Length - 4];
            var filename = fullnameSplit[fullnameSplit.Length - 2];
            var folder = fullnameSplit[fullnameSplit.Length - 3];
            var splitFlags = FlagPattern.Matches(filename);
            var setSplit = filename.Split('_');

            var resourceDefinition = new ResourceDefinition(resourceName,
                $"{filename}.{fullnameSplit[fullnameSplit.Length - 1]}");

            ParsedResourceDefinitions.Add(resourceName, resourceDefinition);

            if (ParsedAdditionalMeta.ContainsKey(resourceName))
            {
                // TODO will need to update when there are more fields coming from here  
                var meta = ParsedAdditionalMeta[resourceName];
                resourceDefinition.Meta.MainAnchor = meta.MainAnchor;
                resourceDefinition.Meta.Overlay = meta.Overlay;
                resourceDefinition.Description = meta.Description;
                resourceDefinition.SubTypes = meta.Types;
            }

            if (resourceName.EndsWith("o.png"))
            {
                if (setSplit.Length > 2)
                    throw new ArgumentException($"File {resourceName}: overlays and partitioned resources are not currently compatible");

                TypedResourcesOverlays[resourceName.Replace("o.png", ".png")] = resourceName;
                resourceDefinition.ObjectType = ObjectType.overlay;
                return resourceDefinition; // Don't need to read the rest of the flags, the twin resource will have them
            }

            if (groupName == "ResourceGroups" || splitFlags.Count == 0)
            {
                resourceDefinition.ObjectType = (ObjectType)Enum.Parse(typeof(ObjectType), folder);
                return resourceDefinition;
            }

            if (setSplit.Length > 2)
            {
                var rootName = resourceName.Replace($"_{setSplit[2]}.png", "");
                resourceDefinition.Meta.SetName = rootName;
                // Not needed - if a resource is not a set, and we need more than one, then we just do it.
                // The UQ flag can be used to prevent that.
                //if (setSplit[2].ToLower() == "r")
                //{
                //    resourceDefinition.Meta.IsRepeatingSet = true;
                //}
            }

            foreach (var flag in splitFlags)
            {
                var flagType = flag.ToString().Substring(0, 1).ToUpper();
                var flagValue = flag.ToString().Substring(1).ToUpper();

                if (flagType == "A")
                {
                    resourceDefinition.Meta.IsFrame = true;
                }
                else if (flagType == "U" && flagValue == "Q")
                {
                    resourceDefinition.Meta.CannotDuplicate = true;
                }
                else
                {
                    var enumType = ResDict.Flags[flagType];
                    var propValue = Enum.Parse(enumType, flagValue);
                    var propInfo = ResourceDefinition.DefinitionPropInfo.FirstOrDefault(p => p.Name == enumType.Name);
                    propInfo.SetValue(resourceDefinition, propValue, null);
                }
            }

            return resourceDefinition;
        }
    }
}
