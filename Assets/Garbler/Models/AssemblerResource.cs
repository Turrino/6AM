using BayeuxBundle.Models.Instructions;
using System.Collections.Generic;
using System.Linq;

namespace BayeuxBundle.Models
{
    public class AssemblerResource<T>
    {
        private AssemblerResource(IImage<T> canvas)
        {
            Data = canvas;
            Definition = new ResourceDefinition() { ObjectType = ObjectType.na };
            IsEmpty = true;
            Instruction = new InstructionLayer(ObjectType.na);
            SplitLayers = new List<SplitLayer<T>>();
        }

        public AssemblerResource(ResourceDefinition definition, IImage<T> image, InstructionLayer instruction)
        {
            Definition = definition;
            Data = image;
            Instruction = instruction;
        }

        public AssemblerResource(ResourceDefinition definition, List<IImage<T>> images, InstructionLayer instruction)
        {
            Definition = definition;
            AdditionalImages = images;
            Data = images.First();
            Instruction = instruction;
        }

        public bool IsEmpty;
        public IImage<T> Data;
        public List<IImage<T>> AdditionalImages;
        public ResourceDefinition Definition { get; set; }
        public InstructionLayer Instruction { get; set; }
        // List of layers that have been split and are currently in a queue to be merged
        public List<SplitLayer<T>> SplitLayers { get; set; }
        public Dictionary<PixelInfo, int> Colors { get; set; }

        //public AssemblerResource<T> Merge(AssemblerResource<T> layer, Overlay overlay = null)
        //{
        //    // If we need to keep track of individual layers,
        //    // have the canvas store their metadata in a list
        //    TextureTools.Merge(this, layer, overlay);

        //    if (!IsCanvas)
        //    {
        //        Definition.Merge(layer.Definition);
        //    }

        //    return this;
        //}

        public static AssemblerResource<T> Canvas(IImage<T> canvas)
        {
            return new AssemblerResource<T>(canvas);
        }
    }
}
