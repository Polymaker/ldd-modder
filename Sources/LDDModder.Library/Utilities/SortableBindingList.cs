using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Generic
{
    public class SortableBindingList<T> : BindingList<T>
    {
        private readonly Dictionary<Type, PropertyComparer<T>> comparers;
        public bool IsSorted { get; private set; }
        public ListSortDirection SortDirection { get; private set; }
        public PropertyDescriptor PropertyDescriptor { get; private set; }

        public SortableBindingList()
            : base(new List<T>())
        {
            this.comparers = new Dictionary<Type, PropertyComparer<T>>();
        }

        public SortableBindingList(IEnumerable<T> enumeration)
            : base(new List<T>(enumeration))
        {
            this.comparers = new Dictionary<Type, PropertyComparer<T>>();
        }

        protected override bool SupportsSortingCore
        {
            get { return true; }
        }

        protected override bool IsSortedCore
        {
            get { return this.IsSorted; }
        }

        protected override PropertyDescriptor SortPropertyCore
        {
            get { return this.PropertyDescriptor; }
        }

        protected override ListSortDirection SortDirectionCore
        {
            get { return this.SortDirection; }
        }

        protected override bool SupportsSearchingCore
        {
            get { return true; }
        }

        public void ApplySort(string propertyName, ListSortDirection direction)
        {

            var properties = TypeDescriptor.GetProperties(typeof(T));
            var propdesc = properties.Find(propertyName, true);

            if (propdesc != null)
            {
                if (!(PropertyDescriptor == propdesc && direction == SortDirection))
                    ApplySortCore(propdesc, direction);
            }
        }

        public void ApplySort(PropertyDescriptor property, ListSortDirection direction)
        {
            ApplySortCore(property, direction);
        }

        public void ClearSort()
        {
            if (IsSorted)
            {
                RemoveSortCore();
            }
        }

        protected override void ApplySortCore(PropertyDescriptor property, ListSortDirection direction)
        {
            List<T> itemsList = (List<T>)this.Items;

            Type propertyType = property.PropertyType;
            PropertyComparer<T> comparer;
            if (!this.comparers.TryGetValue(propertyType, out comparer))
            {
                comparer = new PropertyComparer<T>(property, direction);
                this.comparers.Add(propertyType, comparer);
            }

            comparer.SetPropertyAndDirection(property, direction);
            itemsList.Sort(comparer);

            this.PropertyDescriptor = property;
            this.SortDirection = direction;
            this.IsSorted = true;

            this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }

        protected override void RemoveSortCore()
        {
            this.IsSorted = false;
            this.PropertyDescriptor = base.SortPropertyCore;
            this.SortDirection = base.SortDirectionCore;

            this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }

        protected override int FindCore(PropertyDescriptor property, object key)
        {
            int count = this.Count;
            for (int i = 0; i < count; ++i)
            {
                T element = this[i];
                if (property.GetValue(element).Equals(key))
                {
                    return i;
                }
            }

            return -1;
        }

        public void RemoveAll(Predicate<T> match)
        {
            for (int i = 0; i < Count; i++)
            {
                if (match(this[i]))
                {
                    RemoveAt(i);
                    i--;
                }
            }
        }

        public void AddSorted(T item)
        {
            if (IsSorted)
            {
                List<T> itemsList = (List<T>)this.Items;
                itemsList.Add(item);
                ApplySort(PropertyDescriptor, SortDirection);
            }
            else
                Add(item);
        }

        public void InsertSorted(int index, T item)
        {
            if (IsSorted)
            {
                List<T> itemsList = (List<T>)this.Items;
                itemsList.Insert(index, item);
                ApplySort(PropertyDescriptor, SortDirection);
            }
            else
                Insert(index, item);
        }

        public void AddRange(IEnumerable<T> items)
        {
            if (IsSorted)
            {
                List<T> itemsList = (List<T>)this.Items;
                itemsList.AddRange(items);
                ApplySort(PropertyDescriptor, SortDirection);
            }
            else
            {
                RaiseListChangedEvents = false;
                foreach (var item in items)
                    Add(item);
                RaiseListChangedEvents = true;
                OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
            }
        }

        public void InsertRange(int index, IEnumerable<T> items)
        {
            RaiseListChangedEvents = false;
            foreach (var item in items)
                Insert(index++, item);
            RaiseListChangedEvents = true;
            OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }

        protected override void OnListChanged(ListChangedEventArgs e)
        {
            base.OnListChanged(e);
        }
    }

    public class PropertyComparer<T> : IComparer<T>
    {
        private readonly IComparer comparer;
        private PropertyDescriptor propertyDescriptor;
        private int reverse;

        public PropertyComparer(PropertyDescriptor property, ListSortDirection direction)
        {
            this.propertyDescriptor = property;
            Type comparerForPropertyType = typeof(Comparer<>).MakeGenericType(property.PropertyType);
            this.comparer = (IComparer)comparerForPropertyType.InvokeMember("Default", BindingFlags.Static | BindingFlags.GetProperty | BindingFlags.Public, null, null, null);
            this.SetListSortDirection(direction);
        }

        #region IComparer<T> Members

        public int Compare(T x, T y)
        {
            return this.reverse * this.comparer.Compare(this.propertyDescriptor.GetValue(x), this.propertyDescriptor.GetValue(y));
        }

        #endregion

        private void SetPropertyDescriptor(PropertyDescriptor descriptor)
        {
            this.propertyDescriptor = descriptor;
        }

        private void SetListSortDirection(ListSortDirection direction)
        {
            this.reverse = direction == ListSortDirection.Ascending ? 1 : -1;
        }

        public void SetPropertyAndDirection(PropertyDescriptor descriptor, ListSortDirection direction)
        {
            this.SetPropertyDescriptor(descriptor);
            this.SetListSortDirection(direction);
        }
    }
}
