using System;
using System.Collections.Generic;

namespace BayeuxBundle.Models.Instructions
{
    public class Instructions : List<InstructionLayer>
    {
        public Instructions(ObjectType mainType, bool useOnce, SkeletonType skeletonType,
            Dictionary<PixelInfo, PixelInfo> palette = null)
        {
            MainType = mainType;
            UseOnce = useOnce;
            Palette = palette;
            SkeletonType = skeletonType;
        }

        public ObjectType MainType;
        public bool UseOnce;
        public Dictionary<PixelInfo, PixelInfo> Palette;
        public SkeletonType SkeletonType;
    }

    public class InstructionLayer
    {
        public InstructionLayer(Enum type, string subtype = null, bool hasOverlay = false, bool applyPalette = true,
            bool isPattern = false, int chance = 100, int depthSplit = 0) :
            this(new Enum[] { type }, subtype, hasOverlay, applyPalette, isPattern, chance, depthSplit)
        {
        }

        public InstructionLayer(Enum[] types, string subtype = null, bool hasOverlay = false, bool applyPalette = true,
            bool isPattern = false, int chance = 100, int depthSplit = 0)
        {
            Types = types;
            SubType = subtype;
            Chance = chance;
            HasOverlay = hasOverlay;
            DepthSplit = depthSplit;
            ApplyPalette = applyPalette;
            IsPattern = isPattern;
        }


        //public InstructionLayer(params Enum[] types)
        //{
        //    Types = types.Select(t => new InstructionType(t)).ToArray();
        //}

        public Enum[] Types;
        public int Chance;
        public bool HasOverlay;
        public bool ApplyPalette;
        public bool IsPattern;
        public string SubType;
        /// <summary>
        /// Applicable to layers that have multiple parts.
        /// The parts are kept separate and the first batch (half of them) are inserted at the current layer,
        /// the rest are are kept on hold for the amount of layers specified by this property.
        /// </summary>
        public int DepthSplit;
    }

    //public class InstructionType
    //{
    //    public InstructionType(Enum type, bool hasOverlay = false, int chance = 100)
    //    {
    //        Type = type;
    //        Chance = chance;
    //    }

    //    public Enum Type;
    //    public int Chance;
    //}

    public static class InstructionsPrefabs
    {
        //public static Instructions Vase(Shape shape, bool includeDeco)
        //{
        //    var instructions = new Instructions(ObjectType.vase, true, SkeletonType.Overlay)
        //        {
        //            new InstructionLayer(new Enum[] { ObjectType.vase, shape })
        //        };
        //    if (includeDeco)
        //    {
        //        instructions.Add(new InstructionLayer(ObjectType.decoration));
        //    }

        //    return instructions;
        //}

        public static Instructions TestChar(bool usePartsOnce)
        {
            var instructions = new Instructions(ObjectType.am6body, usePartsOnce, SkeletonType.Overlay)
                {
                    new InstructionLayer(ObjectType.am6tail),
                    new InstructionLayer(ObjectType.am6body, null, true),
                    new InstructionLayer(ObjectType.am6face),
                    new InstructionLayer(ObjectType.am6hat),
                };
            
            return instructions;
        }
    }

    //public static ModelRequest am6character = new ModelRequest(
    //new List<LayerInfo>
    //{
    //            new LayerInfo(ObjectType.am6legs, 100),
    //            new LayerInfo(ObjectType.am6arms, 100),
    //            new LayerInfo(ObjectType.am6tail, 100),
    //            new LayerInfo(ObjectType.am6body, 100),
    //            new LayerInfo(ObjectType.am6chardeco, 100),
    //            new LayerInfo(ObjectType.am6mouth, 100),
    //            new LayerInfo(ObjectType.am6eyes, 100),
    //            new LayerInfo(ObjectType.am6nose, 100),
    //            new LayerInfo(ObjectType.am6hat, 100),
    //}
    //);
}
