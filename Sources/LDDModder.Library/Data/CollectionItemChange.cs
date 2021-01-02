using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.ComponentModel
{
    public sealed class CollectionItemChange
    {
        public object Item { get; }
        public int OldIndex { get; }
        public int NewIndex { get; }

        public CollectionItemChange(object item, int oldIndex, int newIndex)
        {
            Item = item;
            OldIndex = oldIndex;
            NewIndex = newIndex;
        }
    }
}
