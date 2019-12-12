using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BayeuxBundle.Models
{
    public class ResourceDefinition
    {
        public ResourceDefinition(string resourceName, string fileName)
        {
            Meta = new Meta
            {
                ResourceName = resourceName,
                FileName = fileName,
            };
        }

        /// <summary>
        /// Use for requests
        /// </summary>
        public ResourceDefinition() { }

        public static PropertyInfo[] DefinitionPropInfo =
            typeof(ResourceDefinition).GetProperties()
            .Where(prop =>
             !new[] { nameof(Meta), nameof(LayersType), nameof(Description), nameof(SubTypes) }.Contains(prop.Name)).ToArray();

        /// <summary>`
        /// The list of layers added onto the original canvas, excluding the canvas itself.
        /// </summary>
        public List<ObjectType> LayersType { get; set; }
        public Meta Meta { get; set; }

        /// <summary>
        /// Custom description (not generated from type)
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Custom description (not generated from type)
        /// </summary>
        public string[] SubTypes { get; set; }

        /// <summary>
        /// The base object type
        /// </summary>
        public ObjectType ObjectType { get; set; }
        public Shape Shape { get; set; }

        /// <summary>
        /// NOTE: overlapping definitions are not supported!
        /// If canvas and mergee have the same property set, the canvas property will be kept and the other ignored.
        /// Add support for resources with overlapping definitions if needed.
        /// </summary>
        /// <param name="otherDefinition"></param>
        public void Merge(ResourceDefinition otherDefinition)
        {
            // This is by convention - use meta from the layer, not from the canvas
            Meta = otherDefinition.Meta;

            foreach (var propInfo in DefinitionPropInfo)
            {
                var otherValue = propInfo.GetValue(otherDefinition, null);

                if (propInfo.Name == nameof(ObjectType))
                {
                    if (LayersType == null)
                        LayersType = new List<ObjectType>();
                    LayersType.Add((ObjectType)otherValue);
                }

                if ((int)otherValue > 0 && (int)propInfo.GetValue(this, null) == 0)
                {
                    propInfo.SetValue(this, otherValue, null);
                }
            }
        }
    }
}
