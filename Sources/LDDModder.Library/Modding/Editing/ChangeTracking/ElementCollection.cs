using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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

        public event EventHandler<CollectionChangedEventArgs> CollectionChanged;

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
                item._Project = adding ? Project : null;
            else if (Owner != null)
                item.Parent = adding ? Owner : null;
        }

        public void AddRange(IEnumerable<T> items)
        {
            PreventEvents = true;
            List<T> addedItems = new List<T>();

            try
            {
                foreach (var item in items)
                {
                    addedItems.Add(item);
                    Add(item);
                }
            }
            finally
            {
                PreventEvents = false;
                OnCollectionChanged(new CollectionChangedEventArgs(this, 
                    System.ComponentModel.CollectionChangeAction.Add, addedItems));
            }
        }

        protected void OnCollectionChanged(CollectionChangedEventArgs e)
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

        public void Insert(int index, T item)
        {
            UpdateItemParent(item, true);
            InnerList.Insert(index, item);

            OnCollectionChanged(new CollectionChangedEventArgs(this,
                    System.ComponentModel.CollectionChangeAction.Add, 
                    new PartElement[] { item }));
        }

        public void RemoveAt(int index)
        {
            var removedItem = InnerList[index];
            UpdateItemParent(removedItem, true);

            InnerList.RemoveAt(index);

            OnCollectionChanged(new CollectionChangedEventArgs(this,
                    System.ComponentModel.CollectionChangeAction.Remove,
                    new PartElement[] { removedItem }));
        }

        public bool Remove(T item)
        {
            if (InnerList.Remove(item))
            {
                UpdateItemParent(item, false);

                OnCollectionChanged(new CollectionChangedEventArgs(this,
                    System.ComponentModel.CollectionChangeAction.Remove,
                    new PartElement[] { item }));

                return true;
            }

            return false;
        }

        public void Remove(IEnumerable<T> elements)
        {
            var removedElements = new List<T>();
            PreventEvents = true;

            foreach(var item in elements)
            {
                if (Remove(item))
                    removedElements.Add(item);
            }

            PreventEvents = false;

            OnCollectionChanged(new CollectionChangedEventArgs(this,
                    System.ComponentModel.CollectionChangeAction.Remove,
                    removedElements.ToArray()));
        }

        public void Add(T item)
        {
            UpdateItemParent(item, true);
            InnerList.Add(item);

            OnCollectionChanged(new CollectionChangedEventArgs(this,
                    System.ComponentModel.CollectionChangeAction.Add,
                    new PartElement[] { item }));
        }

        public void Clear()
        {
            var oldItems = this.ToArray();

            InnerList.Clear();

            if (oldItems.Length > 0)
            {
                OnCollectionChanged(new CollectionChangedEventArgs(this,
                    System.ComponentModel.CollectionChangeAction.Remove, oldItems));

                foreach (var itm in oldItems)
                    UpdateItemParent(itm, false);
            }
        }

        public bool Contains(T item)
        {
            return InnerList.Contains(item);
        }

        public bool Contains(PartElement item)
        {
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

        public void Remove(PartElement element)
        {
            if (element is T typedElem)
                Remove(typedElem);
        }

        //protected override IEnumerator<PartElement> GetEnumeratorBase()
        //{
        //    return InnerList.GetEnumerator();
        //}
    }
}
