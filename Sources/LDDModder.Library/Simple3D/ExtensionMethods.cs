using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.Simple3D
{
    public static class ExtensionMethods
    {
        public static IEnumerable<Vector2> DistinctValues(this IEnumerable<Vector2> list, float tolerence = 0.00001f)
        {
            //var values = new List<Vector2>();
            var hashList = new Dictionary<Vector2, int>();
            foreach (var v in list)
            {
                if (!hashList.ContainsKey(v))
                {
                    hashList.Add(v, 1);
                    yield return v;
                }
                //if (!values.Any(x => x.Equals(v, tolerence)))
                //{
                //    values.Add(v);
                //    yield return v;
                //}
            }
        }

        public static IEnumerable<Vector3> DistinctValues(this IEnumerable<Vector3> list, float tolerence = 0.00001f)
        {
            //var values = new List<Vector3>();
            var hashList = new Dictionary<Vector3, int>();
            foreach (var v in list)
            {
                if (!hashList.ContainsKey(v))
                {
                    hashList.Add(v, 1);
                    yield return v;
                }
                //if (!values.Any(x => x.Equals(v, tolerence)))
                //{
                //    values.Add(v);
                //    yield return v;
                //}
            }
        }
    }
}
