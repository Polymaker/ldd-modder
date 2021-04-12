using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.ComponentModel
{
    public interface IChangeTrackingCollection
    {
        event CollectionChangedEventHandler CollectionChanged;
    }

    public class ChangeTrackingCollection<T> : Collection<T>, IChangeTrackingCollection
    {
        public event CollectionChangedEventHandler CollectionChanged;

        private List<CollectionItemChange> ChangeBatch;

        public ChangeTrackingCollection()
        {
            
        }
         
        public ChangeTrackingCollection(ChangeTrackingObject trackingObject)
        {
            trackingObject.AttachCollection(this);
        }

        protected override void ClearItems()
        {
            BeginCollectionChanges();

            for (int i = 0; i < Count; i++)
            {
                AddChange(base[i], i, -1);
                BeforeRemoveItem(base[i]);
            }

            var tmp = this.ToArray();

            base.ClearItems();

            for (int i = 0; i < tmp.Length; i++)
                AfterRemoveItem(tmp[i]);

            RaiseCollectionChanges();
        }

        protected override void InsertItem(int index, T item)
        {
            bool fireEvent = BeginCollectionChanges();
            AddChange(item, -1, index);

            BeforeAddItem(item);
            base.InsertItem(index, item);
            AfterAddItem(item);

            if (fireEvent)
                RaiseCollectionChanges();
        }

        protected virtual void BeforeAddItem(T item)
        {

        }

        protected virtual void AfterAddItem(T item)
        {

        }

        protected override void SetItem(int index, T item)
        {
            bool fireEvent = BeginCollectionChanges();
            AddChange(base[index], index, -1);
            AddChange(item, -1, index);

            base.SetItem(index, item);

            if (fireEvent)
                RaiseCollectionChanges();
        }

        protected override void RemoveItem(int index)
        {
            bool fireEvent = BeginCollectionChanges();
            var item = base[index];
            AddChange(item, index, -1);
            BeforeRemoveItem(item);
            base.RemoveItem(index);
            AfterRemoveItem(item);
            if (fireEvent)
                RaiseCollectionChanges();
        }

        protected virtual void BeforeRemoveItem(T item)
        {

        }

        protected virtual void AfterRemoveItem(T item)
        {

        }

        public void ReorderItem(T item, int newIndex)
        {
            int curIndex = IndexOf(item);
            if (curIndex >= 0 && curIndex != newIndex)
            {
                BeginCollectionChanges();
                RemoveAt(curIndex);
                if (newIndex > curIndex)
                    Insert(newIndex - 1, item);
                else
                    Insert(newIndex, item);

                RaiseCollectionChanges();
            }
        }
        protected bool BeginCollectionChanges()
        {
            if (ChangeBatch != null)
                return false;
            ChangeBatch = new List<CollectionItemChange>();
            return true;
        }

        protected void RaiseCollectionChanges()
        {
            if (ChangeBatch != null && ChangeBatch.Any())
                OnCollectionChanged(new CollectionChangedEventArgs(this, ChangeBatch));

            ChangeBatch = null;
        }

        protected virtual void OnCollectionChanged(CollectionChangedEventArgs ccea)
        {
            CollectionChanged?.Invoke(this, ccea);
        }

        private void AddChange(T item, int oldIndex, int newIndex)
        {
            if (ChangeBatch != null)
                ChangeBatch.Add(new CollectionItemChange(item, oldIndex, newIndex));
        }

        #region Linq Overrides

        public void RemoveAll(Func<T, bool> predicate)
        {
            BeginCollectionChanges();
            int indexOffset = 0;
            for (int i = 0; i < Count; i++)
            {
                if (predicate(base[i]))
                {
                    AddChange(base[i], (indexOffset++) + (i--), -1);
                    RemoveItem(i);
                }
            }
            RaiseCollectionChanges();
        }

        public void AddRange(IEnumerable<T> items)
        {
            BeginCollectionChanges();
            foreach (var item in items)
                Add(item);
            RaiseCollectionChanges();
        }

        #endregion

        class SortHelper
        {
            public int OldIndex { get; set; }
            public T Item { get; set; }
        }

        public void Sort<TKey>(Func<T, TKey> keySelector)
        {
            var test = new List<SortHelper>();
            for (int i = 0; i < Count; i++)
                test.Add(new SortHelper { OldIndex = i, Item = this[i] });
            //var sortedList = this.OrderBy(keySelector).ToList();
            var sortedList = test.OrderBy(t => keySelector(t.Item)).ToList();

            BeginCollectionChanges();

            for (int i = 0; i < sortedList.Count; i++)
            {
                AddChange(sortedList[i].Item, sortedList[i].OldIndex, i);
                base.SetItem(i, sortedList[i].Item);
            }

            RaiseCollectionChanges();
        }
    }
}
