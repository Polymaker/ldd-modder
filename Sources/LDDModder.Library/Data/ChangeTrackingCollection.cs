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
            StartBatch();

            for (int i = 0; i < Count; i++)
                AddChange(base[i], i, -1);

            base.ClearItems();

            RaiseBatch();
        }

        protected override void InsertItem(int index, T item)
        {
            bool fireEvent = StartBatch();
            AddChange(item, -1, index);

            base.InsertItem(index, item);

            if (fireEvent)
                RaiseBatch();
        }

        protected override void SetItem(int index, T item)
        {
            bool fireEvent = StartBatch();
            AddChange(base[index], index, -1);
            AddChange(item, -1, index);

            base.SetItem(index, item);

            if (fireEvent)
                RaiseBatch();
        }

        protected override void RemoveItem(int index)
        {
            bool fireEvent = StartBatch();
            AddChange(base[index], index, -1);

            base.RemoveItem(index);

            if (fireEvent)
                RaiseBatch();
        }

        private bool StartBatch()
        {
            if (ChangeBatch != null)
                return false;
            ChangeBatch = new List<CollectionItemChange>();
            return true;
        }

        private void RaiseBatch()
        {
            if (ChangeBatch != null && ChangeBatch.Any())
            {
                CollectionChanged?.Invoke(this, new CollectionChangedEventArgs(this, ChangeBatch));
            }

            ChangeBatch = null;
        }

        private void AddChange(T item, int oldIndex, int newIndex)
        {
            if (ChangeBatch != null)
                ChangeBatch.Add(new CollectionItemChange(item, oldIndex, newIndex));
        }

        #region Linq Overrides

        public void RemoveAll(Func<T, bool> predicate)
        {
            StartBatch();
            int indexOffset = 0;
            for (int i = 0; i < Count; i++)
            {
                if (predicate(base[i]))
                {
                    AddChange(base[i], (indexOffset++) + (i--), -1);
                    RemoveItem(i);
                }
            }
            RaiseBatch();
        }

        #endregion
    }
}
