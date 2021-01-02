using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.ComponentModel
{
    public class CollectionChangedEventArgs : EventArgs
    {
        //public NotifyCollectionChangedAction Action { get; }
        //public IList OldItems { get; }
        //public IList NewItems { get; }
        //public IList<int> OldIndices { get; }
        //public IList<int> NewIndices { get; }

        public ICollection Collection { get; }

        public IList<CollectionItemChange> ChangedItems { get; }

        public IEnumerable<CollectionItemChange> AddedItems => 
            ChangedItems.Where(x => x.NewIndex >= 0 && x.OldIndex < 0);

        public IEnumerable<CollectionItemChange> RemovedItems => 
            ChangedItems.Where(x => x.OldIndex >= 0 && x.NewIndex < 0);

        public IEnumerable<CollectionItemChange> MovedItems => 
            ChangedItems.Where(x => x.OldIndex != x.NewIndex && x.OldIndex >= 0 && x.NewIndex >= 0);

        public bool HasAdded => AddedItems.Any();
        public bool HasRemoved => RemovedItems.Any();
        public bool HasMoved => MovedItems.Any();

        public CollectionChangedEventArgs(IEnumerable<CollectionItemChange> changedItems)
        {
            ChangedItems = changedItems.ToList().AsReadOnly();
        }

        public CollectionChangedEventArgs(ICollection collection, IEnumerable<CollectionItemChange> changedItems)
        {
            Collection = collection;
            ChangedItems = changedItems.ToList().AsReadOnly();
        }

        //public static void FromObservableCollection(ICollection collection, NotifyCollectionChangedEventArgs args)
        //{
        //    switch (args.Action)
        //    {
        //        case NotifyCollectionChangedAction.Add:
        //            args.
        //            break;
        //    }
        //}

        //public CollectionChangedEventArgs(NotifyCollectionChangedAction action)
        //{
        //    if (action != NotifyCollectionChangedAction.Reset)
        //        throw new ArgumentException(nameof(action));

        //    ChangedItems = new CollectionItemChange[0];
        //    Action = action;
        //}


    }

    public delegate void CollectionChangedEventHandler(object sender, CollectionChangedEventArgs ccea);
}
