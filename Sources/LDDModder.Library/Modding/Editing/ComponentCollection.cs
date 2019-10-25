using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.Modding.Editing
{
    public interface IComponentCollection/* : IEnumerable<PartComponent>*/
    {
        PartProject Project { get; }

    }

    public class ComponentCollection<T> : ObservableCollection<T>, IList<T>, IComponentCollection 
        where T : PartComponent
    {
        public PartComponent Owner { get; }

        private PartProject _Project;

        public PartProject Project => _Project ?? Owner?.Project;

        private bool PreventEvents;

        public ComponentCollection(PartComponent owner)
        {
            Owner = owner;
        }

        public ComponentCollection(PartProject project)
        {
            _Project = project;
        }

        protected override void InsertItem(int index, T item)
        {
            base.InsertItem(index, item);
            UpdateItemParent(item, true);
        }

        protected override void SetItem(int index, T item)
        {
            UpdateItemParent(this[index], false);
            base.SetItem(index, item);
            UpdateItemParent(item, true);
        }

        protected override void RemoveItem(int index)
        {
            UpdateItemParent(this[index], false);
            base.RemoveItem(index);
        }

        protected override void ClearItems()
        {
            foreach (var item in this)
                UpdateItemParent(item, false);
            base.ClearItems();
        }

        private void UpdateItemParent(PartComponent item, bool adding)
        {
            if (Project != null)
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
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, addedItems));
            }
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (!PreventEvents)
                base.OnCollectionChanged(e);
        }

        public IEnumerable<V> SelectMany<V>(Func<T, IEnumerable<V>> selector)
        {
            foreach (T item in this)
            {
                foreach (var item2 in selector(item))
                    yield return item2;
            }
        }
    }
}
