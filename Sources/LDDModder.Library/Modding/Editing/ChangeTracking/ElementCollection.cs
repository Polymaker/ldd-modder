using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.Modding.Editing
{
    public interface IElementCollection
    {
        PartProject Project { get; }
        PartElement Owner { get; }
        Type ElementType { get; }

        int Count { get; }
        IEnumerable<PartElement> GetElements();

        void Add(PartElement element);

        void AddRange(IEnumerable<PartElement> elements);

        void Insert(int index, PartElement element);

        void InsertAllAt(int index, IEnumerable<PartElement> elements);

        int IndexOf(PartElement element);

        void Remove(PartElement element);

        bool Contains(PartElement element);
    }

    [Serializable]
    public class ElementCollection<T> : IList<T>, IElementCollection 
        where T : PartElement
    {
        public PartElement Owner { get; }

        private PartProject _Project;

        public PartProject Project => _Project ?? Owner?.Project;

        public Type ElementType => typeof(T);

        private List<T> InnerList;

        public int Count => InnerList.Count;

        public bool IsReadOnly => ((IList<T>)InnerList).IsReadOnly;

        public T this[int index] { get => InnerList[index]; set => InnerList[index] = value; }

        public event EventHandler<ElementCollectionChangedEventArgs> CollectionChanged;

        private bool PreventEvents;

        public ElementCollection(PartElement owner)
        {
            Owner = owner;
            Owner.Collections.Add(this);
            InnerList = new List<T>();
        }

        public ElementCollection(PartProject project)
        {
            _Project = project;
            InnerList = new List<T>();
        }

        private void UpdateItemParent(PartElement item, bool adding)
        {
            if (_Project != null)
                item.AssignProject(adding ? Project : null);
            else if (Owner != null)
            {
                item.AssignParent(adding ? Owner : null);
            }
        }

        private void UpdateItemParent(PartElement item, bool adding, Action listAction)
        {
            if (_Project != null)
            {
                item.AssignProject(adding ? Project : null);
                listAction();
            }
            else if (Owner != null)
            {
                item.BeginChangeParent(Owner);
                listAction();
                item.AssignParent(adding ? Owner : null);
            }
        }


        protected void RaiseCollectionChanged(
            CollectionChangeAction action,
            IEnumerable<CollectionChangeItemInfo> changedItems)
        {
            OnCollectionChanged(new ElementCollectionChangedEventArgs(this, action, changedItems));
        }

        protected void RaiseCollectionChanged(
            CollectionChangeAction action,
            CollectionChangeItemInfo changedItem)
        {
            OnCollectionChanged(new ElementCollectionChangedEventArgs(this, action, 
                new CollectionChangeItemInfo[] { changedItem }));
        }

        protected void OnCollectionChanged(ElementCollectionChangedEventArgs e)
        {
            if (!PreventEvents)
            {
                if (Project != null)
                    Project.OnElementCollectionChanged(e);
                CollectionChanged?.Invoke(this, e);
            }
        }

        public IEnumerable<V> SelectMany<V>(Func<T, IEnumerable<V>> selector)
        {
            foreach (T item in this)
            {
                foreach (var item2 in selector(item))
                    yield return item2;
            }
        }

        public int IndexOf(T item)
        {
            return InnerList.IndexOf(item);
        }

        protected CollectionChangeItemInfo InsertItem(int index, T item)
        {
            if (index > Count)
                index = Count;

            UpdateItemParent(item, true, ()=> InnerList.Insert(index, item));

            return new CollectionChangeItemInfo(item, -1, index);
        }

        protected CollectionChangeItemInfo AddItem(T item)
        {
            UpdateItemParent(item, true, ()=> InnerList.Add(item));
            return new CollectionChangeItemInfo(item, -1, Count - 1);
        }

        protected CollectionChangeItemInfo RemoveItem(T item, bool runDry = false)
        {
            int itemIndex = IndexOf(item);

            if (itemIndex >= 0)
            {
                if (!runDry)
                {
                    //InnerList.RemoveAt(itemIndex);
                    UpdateItemParent(item, false, ()=> InnerList.RemoveAt(itemIndex));
                }
                
                return new CollectionChangeItemInfo(item, itemIndex, -1);
            }
            return null;
        }

        protected CollectionChangeItemInfo RemoveItemAt(int index, bool runDry = false)
        {
            var item = InnerList[index];
            if (item != null)
            {
                if (!runDry)
                {
                    UpdateItemParent(item, false, ()=> InnerList.RemoveAt(index));
                }
                
                return new CollectionChangeItemInfo(item, index, -1);
            }
            return null;
        }

        public void RemoveAt(int index)
        {
            var removedItem = RemoveItemAt(index, true);

            if (removedItem != null)
            {
                InnerList.Remove((T)removedItem.Element);

                //Raise event before dettaching parent
                RaiseCollectionChanged(CollectionChangeAction.Remove, removedItem);

                UpdateItemParent(removedItem.Element, false);
            }
        }

        public bool Remove(T item)
        {
            var removedItem = RemoveItem(item, true);
            if (removedItem != null)
            {
                InnerList.Remove((T)removedItem.Element);

                //Raise event before dettaching parent
                RaiseCollectionChanged(CollectionChangeAction.Remove, removedItem);

                UpdateItemParent(removedItem.Element, false);
            }

            return false;
        }

        public void Remove(IEnumerable<T> elements)
        {
            var removedItems = new List<CollectionChangeItemInfo>();
            //var removedElements = new List<T>();

            foreach(var item in elements)
            {
                var removedItem = RemoveItem(item, true);

                if (removedItem != null)
                    removedItems.Add(removedItem);

                InnerList.Remove(item);
            }

            //Raise event before dettaching parent
            if (removedItems.Any())
                RaiseCollectionChanged(CollectionChangeAction.Remove, removedItems);

            foreach (var item in removedItems)
                UpdateItemParent(item.Element, false);
        }

        public void RemoveAll(Func<T, bool> predicate)
        {
            var removedItems = new List<CollectionChangeItemInfo>();

            foreach (var item in InnerList.ToArray())
            {
                if (predicate(item))
                {
                    removedItems.Add(RemoveItem(item, true));
                    InnerList.Remove(item);
                }
            }

            //Raise event before dettaching parent
            if (removedItems.Any())
                RaiseCollectionChanged(CollectionChangeAction.Remove, removedItems);

            foreach (var item in removedItems)
                UpdateItemParent(item.Element, false);
        }

        public void Insert(int index, T item)
        {
            var addedItem = InsertItem(index, item);
            RaiseCollectionChanged(CollectionChangeAction.Add, addedItem);
        }

        public void InsertAllAt(int index, IEnumerable<T> items)
        {
            var addedItems = new List<CollectionChangeItemInfo>();
            int curIndex = index;
            foreach(var item in items)
                addedItems.Add(InsertItem(curIndex++, item));

            if (addedItems.Any())
                RaiseCollectionChanged(CollectionChangeAction.Add, addedItems);
        }

        public void Add(T item)
        {
            var addedItem = AddItem(item);
            RaiseCollectionChanged(CollectionChangeAction.Add, addedItem);
        }

        public void AddRange(IEnumerable<T> items)
        {
            var addedItems = new List<CollectionChangeItemInfo>();
            try
            {
                foreach (var item in items)
                    addedItems.Add(AddItem(item));
            }
            finally
            {
                if (addedItems.Any())
                    RaiseCollectionChanged(CollectionChangeAction.Add, addedItems);
            }
        }

        public void ReorderItem(T item, int newIndex)
        {
            int curIndex = IndexOf(item);
            if (curIndex >= 0 && curIndex != newIndex)
            {
                InnerList.RemoveAt(curIndex);
                if (newIndex > curIndex)
                    InnerList.Insert(newIndex - 1, item);
                else
                    InnerList.Insert(newIndex, item);

                var changedItem = new CollectionChangeItemInfo(item, curIndex, newIndex);
                RaiseCollectionChanged(CollectionChangeAction.Refresh, changedItem);
            }
        }

        public void Clear()
        {
            var removedItems = new List<CollectionChangeItemInfo>();

            for (int i = 0; i < Count; i++)
                removedItems.Add(new CollectionChangeItemInfo(InnerList[i], i, -1));

            InnerList.Clear();

            //Raise event before dettaching parent
            if (removedItems.Any())
                RaiseCollectionChanged(CollectionChangeAction.Remove, removedItems);
            
            foreach (var itm in removedItems)
                UpdateItemParent(itm.Element, false);
        }

        public bool Contains(T item)
        {
            return InnerList.Contains(item);
        }

        public bool Contains(PartElement item)
        {
            if (!(item is T))
                return false;
            return InnerList.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            InnerList.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return InnerList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return InnerList.GetEnumerator();
        }

        public IEnumerable<PartElement> GetElements()
        {
            return InnerList;
        }

        public void Add(PartElement element)
        {
            if (element is T typedElem)
                Add(typedElem);
        }

        public void AddRange(IEnumerable<PartElement> elements)
        {
            AddRange(elements.OfType<T>());
        }

        public void Insert(int index, PartElement element)
        {
            if (element is T typedElem)
                Insert(index, typedElem);
        }

        public void InsertAllAt(int index, IEnumerable<PartElement> elements)
        {
            InsertAllAt(index, elements.OfType<T>());
        }

        public void Remove(PartElement element)
        {
            if (element is T typedElem)
                Remove(typedElem);
        }

        public int IndexOf(PartElement element)
        {
            if (element is T typedElem)
                return IndexOf(typedElem);
            return -1;
        }

        //protected override IEnumerator<PartElement> GetEnumeratorBase()
        //{
        //    return InnerList.GetEnumerator();
        //}
    }
}
