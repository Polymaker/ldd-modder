using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.Utilities
{
    public class ListIndexer<T>
    {
        public IEnumerable<T> BaseList { get; }
        private Dictionary<T, int> Indexer;

        public ListIndexer(IEnumerable<T> baseList)
        {
            BaseList = baseList;
            Indexer = new Dictionary<T, int>();
            Initialize();
        }

        private void Initialize()
        {
            int i = 0;
            foreach (var item in BaseList)
            {
                if (!Indexer.ContainsKey(item))
                    Indexer.Add(item, i);
                i++;
            }
        }

        public int IndexOf(T item)
        {
            return Indexer[item];
            //if (Indexer.ContainsKey(item))
            //{
            //    return Indexer[item];
            //}
            //return -1;
        }
    }
}
