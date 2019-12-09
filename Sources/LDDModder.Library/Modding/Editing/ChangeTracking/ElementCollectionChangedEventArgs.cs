using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.Modding.Editing
{
    public class ElementCollectionChangedEventArgs : EventArgs
    {
        public IElementCollection Collection { get; }
        public Type ElementType { get; }
        public CollectionChangeAction Action { get; }

        public CollectionChangeItemInfo[] ChangedItems { get; }

        public IEnumerable<PartElement> ChangedElements => ChangedItems.Select(x => x.Element);

        public IEnumerable<PartElement> AddedElements => ChangedItems.Where(x => x.OldIndex == -1).Select(x => x.Element);

        public IEnumerable<PartElement> RemovedElements => ChangedItems.Where(x => x.NewIndex == -1).Select(x => x.Element);

        public ElementCollectionChangedEventArgs(IElementCollection collection, 
            CollectionChangeAction action, 
            IEnumerable<CollectionChangeItemInfo> changedItems)
        {
            Collection = collection;
            Action = action;
            ChangedItems = changedItems.ToArray();
        }

        public IEnumerable<PartElement> GetElementHierarchy()
        {
            return ChangedElements.SelectMany(x => x.GetChildsHierarchy(true));
        }
    }
}
