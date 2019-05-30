using System;
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
            var values = new List<T>();
            var comparator = EqualityComparer<T>.Default;
            foreach (var v in collection)
            {
                if (!values.Any(x => comparator.Equals(v, x)))
                {
                    values.Add(v);
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
    }
}
