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

        public CollectionChangeActions Action
        {
            get
            {
                if (OldIndex == -1 && NewIndex >= 0)
                    return CollectionChangeActions.Add;
                else if (NewIndex == -1 && OldIndex >= 0)
                    return CollectionChangeActions.Remove;
                else if (NewIndex >= 0 && OldIndex >= 0 && OldIndex != NewIndex)
                    return CollectionChangeActions.Move;
                return CollectionChangeActions.None;
            }
        }

        public CollectionItemChange(object item, int oldIndex, int newIndex)
        {
            Item = item;
            OldIndex = oldIndex;
            NewIndex = newIndex;
        }
    }
}
