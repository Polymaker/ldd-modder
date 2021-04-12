using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.Modding
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

        void SetIndex(int index, PartElement element);

        int IndexOf(PartElement element);

        void Remove(PartElement element);

        bool Contains(PartElement element);
    }

    [Serializable]
    public class ElementCollection<T> : ChangeTrackingCollection<T>, IElementCollection 
        where T : PartElement
    {
        public PartElement Owner { get; }

        private PartProject _Project;

        public PartProject Project => _Project ?? Owner?.Project;

        public Type ElementType => typeof(T);

        public ElementCollection(PartElement owner) : base(owner)
        {
            Owner = owner;
            Owner.Collections.Add(this);
        }

        public ElementCollection(PartProject project)
        {
            _Project = project;
            _Project.AttachCollection(this);
        }

        protected override void BeforeRemoveItem(T item)
        {
            base.BeforeRemoveItem(item);
            if (Owner != null)
                item.BeginChangeParent(Owner);
            else
                item.AssignProject(null);
        }

        protected override void AfterRemoveItem(T item)
        {
            base.AfterRemoveItem(item);
            if (Owner != null)
                item.AssignParent(null);
        }

        protected override void BeforeAddItem(T item)
        {
            base.BeforeAddItem(item);
            if (Owner != null)
                item.BeginChangeParent(Owner);
            else
                item.AssignProject(_Project);
        }

        protected override void AfterAddItem(T item)
        {
            base.AfterAddItem(item);
            if (Owner != null)
                item.AssignParent(Owner);
        }

        //public IEnumerable<V> SelectMany<V>(Func<T, IEnumerable<V>> selector)
        //{
        //    foreach (T item in this)
        //    {
        //        foreach (var item2 in selector(item))
        //            yield return item2;
        //    }
        //}

        public void InsertAllAt(int index, IEnumerable<T> items)
        {
            BeginCollectionChanges();

            int curIndex = index;
            foreach (var item in items)
            {
                Insert(curIndex++, item);
            }
            RaiseCollectionChanges();
        }
        public void SetIndex(int index, PartElement element)
        {
            ReorderItem(element as T, index);
        }

        public bool Contains(PartElement item)
        {
            if (!(item is T typeditem))
                return false;

            return base.Contains(typeditem);
        }

        public IEnumerable<PartElement> GetElements()
        {
            return this;
        }

        public void Add(PartElement element)
        {
            if (element is T typedElem)
                base.Add(typedElem);
        }

        public void AddRange(IEnumerable<PartElement> elements)
        {
            base.AddRange(elements.OfType<T>());
        }

        public void Insert(int index, PartElement element)
        {
            if (element is T typedElem)
                base.Insert(index, typedElem);
        }

        public void InsertAllAt(int index, IEnumerable<PartElement> elements)
        {
            InsertAllAt(index, elements.OfType<T>());
        }

        public void Remove(PartElement element)
        {
            if (element is T typedElem)
                base.Remove(typedElem);
        }

        public int IndexOf(PartElement element)
        {
            if (element is T typedElem)
                return base.IndexOf(typedElem);
            return -1;
        }
    }
}
