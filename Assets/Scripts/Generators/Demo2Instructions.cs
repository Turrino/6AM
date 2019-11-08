using BayeuxBundle.Models;
using BayeuxBundle.Models.Instructions;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Generators
{
    public class Demo2Instructions
    {
        public static Instructions CabinetInstructions(Dictionary<PixelInfo, PixelInfo> palette)
            => new Instructions(ObjectType.am6cabinet, true, SkeletonType.Overlay, palette)
                {
                    new InstructionLayer(ObjectType.am6peg),
                    new InstructionLayer(ObjectType.am6cabinet, null, true),
                    new InstructionLayer(ObjectType.am6cabinetdoor),
                    new InstructionLayer(ObjectType.am6trinket),
                };

        public static Instructions PottedPlantInstructions(Dictionary<PixelInfo, PixelInfo> palette)
            => new Instructions(ObjectType.am6plant, false, SkeletonType.Anchors, palette)
                {
                    new InstructionLayer(ObjectType.am6pot),
                    new InstructionLayer(ObjectType.am6plant),
                };

        public static Instructions ItemInstructions(string itemType, Dictionary<PixelInfo, PixelInfo> palette)
            => new Instructions(ObjectType.am6item, false, SkeletonType.None, palette)
        {
            new InstructionLayer(ObjectType.am6item, itemType, false, true, true),
        };

        public static Instructions PaintingInstructions(Dictionary<PixelInfo, PixelInfo> palette)
            => new Instructions(ObjectType.am6painting, false, SkeletonType.Anchors, palette)
                {
                    new InstructionLayer(ObjectType.am6painting),
                };

        public static Instructions CharacterInstructions(bool usePartsOnce, Dictionary<PixelInfo, PixelInfo> palette) =>
            new Instructions(ObjectType.am6body, usePartsOnce, SkeletonType.Overlay, palette)
                {
                    new InstructionLayer(ObjectType.am6tail),
                    new InstructionLayer(ObjectType.am6body, null, true),
                    new InstructionLayer(ObjectType.am6face),
                    new InstructionLayer(ObjectType.am6hat),
                };

        public static Instructions VaseInstructions(Shape shape)
        {
            // Note: use once = true was added with the old scenario setup logic. might be safe to remove now, check though
            return new Instructions(ObjectType.am6vase, true, SkeletonType.Anchors)
                {
                    new InstructionLayer(new Enum[] { ObjectType.am6vase, shape }),
                    new InstructionLayer(ObjectType.am6deco, isPattern: true)
                };
        }
    }
}
