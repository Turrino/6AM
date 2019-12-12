using BayeuxBundle.Models;
using System;
using System.Collections.Generic;

namespace BayeuxBundle
{
    public static class Utils
    {
        public static ObjectType ToObjectType(this string input) => (ObjectType)Enum.Parse(typeof(ObjectType), input);
        public static T PopRandom<T>(this List<T> list)
        {
            var idx = LocalRandom.Range(0, list.Count);
            var item = list[idx];
            list.RemoveAt(idx);

            return item;
        }

        public static T PickRandom<T>(this List<T> list) => list[LocalRandom.Range(0, list.Count)];

        public static T PickRandom<T>(this T[] array) => array[LocalRandom.Range(0, array.Length)];
    }
}
