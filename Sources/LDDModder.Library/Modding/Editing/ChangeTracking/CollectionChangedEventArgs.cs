using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.Modding.Editing
{
    public class CollectionChangedEventArgs : EventArgs
    {
        public IElementCollection Collection { get; }
        public Type ElementType { get; }
        public System.ComponentModel.CollectionChangeAction Action { get; }
        public PartElement[] AddedElements { get; }
        public PartElement[] RemovedElements { get; }

        public CollectionChangedEventArgs(
            IElementCollection collection, 
            System.ComponentModel.CollectionChangeAction action, 
            IEnumerable<PartElement> elements)
        {
            
            Collection = collection;

            var collectionType = collection.GetType().GetGenericArguments();
            ElementType = collectionType.Length > 0 ? collectionType[0] : null;

            Action = action;
            if (action == System.ComponentModel.CollectionChangeAction.Add)
            {
                AddedElements = elements.ToArray();
                RemovedElements = new PartElement[0];
            }
            else
            {
                RemovedElements = elements.ToArray();
                AddedElements = new PartElement[0];
            }
        }

        public IEnumerable<PartElement> GetElementHierarchy()
        {
            var elems = AddedElements.Concat(RemovedElements);
            return elems.SelectMany(x => x.GetChildsHierarchy(true));
        }
    }
}
