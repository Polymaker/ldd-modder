﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Generic
{
    public static class CollectionExtensions
    {
        public static void Remove<T>(this List<T> list, IEnumerable<T> elements)
        {
            var elemArray = elements.ToArray();
            foreach (var item in elemArray)
                list.Remove(item);
        }

        public static IEnumerable<T> EqualsDistinct<T>(this IEnumerable<T> collection)
        {
            var hashList = new Dictionary<T, int>();
            foreach (var v in collection)
            {
                if (!hashList.ContainsKey(v))
                {
                    hashList.Add(v, 1);
                    yield return v;
                }
            }
        }

        public static int CountContained<T>(this IEnumerable<T> collection, IEnumerable<T> items)
        {
            return collection.Count(a => items.Contains(a));
        }

        public static bool ContainsAny<T>(this IEnumerable<T> collection, IEnumerable<T> items)
        {
            foreach (var item in items)
                if (collection.Contains(item))
                    return true;
            return false;
        }

        public static int IndexOf<T>(this IEnumerable<T> collection, T item)
        {
            int index = 0;
            foreach (var itm in collection)
            {
                if (itm.Equals(item))
                    return index;
                index++;
            }
            return -1;
        }

        public static Type GetCollectionType(this Type type)
        {
            if (type.IsArray)
                return type.GetElementType();

            var genericArgs = type.GetGenericArguments();

            return genericArgs.Length > 0 ? genericArgs[0] : null;
        }

    }
}
