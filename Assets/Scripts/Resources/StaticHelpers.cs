using Assets.Scripts.Bayeux;
using Assets.Scripts.ScenarioLogic;
using BayeuxBundle;
using BayeuxBundle.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Resources
{
    public static class StaticHelpers
    {
        public static bool Either() => Random.Range(0, 2) == 1;

        public static Vector2 TopLeftPivot = new Vector2(0.0f, 1.0f);
        public static Vector2 CentralPivot = new Vector2(0.5f, 0.5f);
        public static Vector2 BottomCenterPivot = new Vector2(0.5f, 0);

        public static List<Enum> EnumValues(Type type)
        {
            return Enum.GetValues(type)
                .Cast<Enum>()
                .FilterNa()
                .ToList();
        }

        public static T RandomEnumValue<T>()
        {
            var v = Enum.GetValues(typeof(T))
                .Cast<T>()
                .FilterNa();
            return v.PickRandom();
        }

        public static Enum RandomEnumValue(Type type)
        {
            var v = Enum.GetValues(type)
                .Cast<Enum>()
                .FilterNa();
            return v.PickRandom();
        }

        public static T RandomEnumValue<T>(IEnumerable<T> exclude)
        {
            var v = Enum.GetValues(typeof(T))
                .Cast<T>()
                .FilterNa()
                .Where(x => !exclude.Any(e => e.ToString() == x.ToString()))
                .ToList();
            return v.PickRandom();
        }        

        /// <param name="loadedPct">Can range from -50 (always false) to +50 (always true)</param>
        /// <returns></returns>
        public static bool Flip(int loadedPct = 0)
        {
            return Random.Range(0 + loadedPct, 100 ) > 49;
        }

        public static List<T> PickRandoms<T>(this List<T> list, int count)
        {
            if (count > list.Count)
                throw new ArgumentException($"{count} elements requested from a list with {list.Count} elements");

            var randoms = new List<T>();
            var indexes = new List<int>();

            for (int i = 0; i < count; i++)
            {
                var index = 0;

                do
                {
                    index = Random.Range(0, list.Count);
                } while (indexes.Contains(index));

                // Keep track of the indexes instead of checking the list we built
                // to ensure the method works both for reference and non-ref types
                indexes.Add(index);
                randoms.Add(list[index]);
            }

            return randoms;
        }

        public static string ReplaceIdx(this string original, string newPart, int index)
        {
            var sb = new StringBuilder(original);
            sb.Remove(index, 1);
            sb.Insert(index, newPart);
            return sb.ToString();
        }

        //public static T PopRandom<T>(this List<T> list)
        //{
        //    var idx = Random.Range(0, list.Count);
        //    var item = list[idx];
        //    list.RemoveAt(idx);

        //    return item;
        //}

        //public static Sprite BytesToSprite(this byte[] bytes, Vector2 pivot)
        //{
        //    return bytes.BytesToTexture(true).ToSprite(pivot);
        //}


        //public static T PickRandom<T>(this List<T> list) => list[Random.Range(0, list.Count)];

        //public static T PickRandom<T>(this T[] array) => array[Random.Range(0, array.Length)];

        public static Vector2 Anchor(this Vector2 overlayPoint, Vector2 anchor, int pixelsPerUnit = 100)
        {
            var relativePoint = new Vector2(overlayPoint.x / pixelsPerUnit, overlayPoint.y / pixelsPerUnit * -1);
            return anchor + relativePoint;
        }

        public static Vector2 Anchor(this OverlayPoint overlayPoint, Vector2 anchor, bool flipwise = true, int pixelsPerUnit = 100) =>
            new Vector2(overlayPoint.X, flipwise? overlayPoint.FlipWiseY : overlayPoint.Y).Anchor(anchor, pixelsPerUnit);

        public static Vector2 Anchor(this ImgPoint overlayPoint, Vector2 anchor, int pixelsPerUnit = 100) =>
            overlayPoint.ToVector().Anchor(anchor, pixelsPerUnit);

        public static Vector2 ToVector(this ImgPoint point) => new Vector2(point.X, point.Y);

        public static ImgPoint ToPoint(this Vector2 point) => new ImgPoint((int)point.x, (int)point.y);

        public static int GetSortingOrder(this SpriteRenderer renderer, int adjustment = 0)
        {
            var spriteHeightInUnits = renderer.sprite.rect.height / 100;
            // -1 is to reverse the Y, since the axis is pointing "south"
            return (int)(Math.Round(renderer.transform.position.y - spriteHeightInUnits, 2) * 100) * -1 + adjustment;
        }

        public static int GetSortingOrder(this CapsuleCollider2D collider)
        {
            return (int)(collider.bounds.center.y * 100) * -1;
        }

        public static void SetSortingOrder(this SpriteRenderer renderer, int adjustment = 0)
        {
            var newSortingOrder = renderer.GetSortingOrder(adjustment);       
            if (Math.Abs(newSortingOrder - renderer.sortingOrder) > 5) // Only change on more than trivial variation to avoid flip-flopping
                renderer.sortingOrder = newSortingOrder;
        }

        public static void Shuffle<T>(this List<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = Random.Range(0, n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        private static List<T> FilterNa<T>(this IEnumerable<T> list)
        {
            return list.Where(x => x.ToString() != "na").ToList();
        }

        public static PropInfo ToProp(this AssemblerResource<Texture2D> resource, TextureTools tools, float scale = 1)
        {
            return ToProp(resource, tools, BottomCenterPivot, scale);
        }

        public static PropInfo ToProp(this AssemblerResource<Texture2D> resource, TextureTools tools, Vector2 pivot, float scale = 1)
        {
            var image = resource.Data.Image;
            var imgSize = image.width > image.height ? image.width : image.height;
            var circleSize = (int)(imgSize / 1.7);
            var color = PixelInfo.White;
            var circle = tools.PixelPerfectCircle(circleSize, color);
            circle.Image.Apply();

            return new PropInfo(
                    resource.Data.Image.ToSprite(pivot, scale),
                    circle.ToSprite(pivot, scale),
                    resource.Definition
                );
        }

        public static Sprite ToSprite(this Texture2D tex, Vector2 pivot, float scale = 1)
        {
            return Sprite.Create(tex,
                new Rect(0, 0, tex.width, tex.height),
                pivot,
                100 / scale);
        }

        public static Sprite ToSprite(this IImage<Texture2D> tex, Vector2 pivot, float scale = 1)
        {
            return tex.Image.ToSprite(pivot, scale);
        }
    }
}
