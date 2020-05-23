using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Reflection;
using System.Windows.Forms;

namespace LDDModder.BrickEditor.Localization
{
    [Designer(typeof(LocalizableStringListDesigner))]
    public class LocalizableStringList : Component, IDisposable
    {
        private LocalizableStringCollection _Items;
        private System.ComponentModel.IContainer components = null;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [MergableProperty(false)]
        public LocalizableStringCollection Items
        {
            get
            {
                if (_Items == null)
                    _Items = new LocalizableStringCollection();
                return _Items;
            }
        }

        public LocalizableStringList()
        {
            InitializeComponent();
        }

        public LocalizableStringList(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            if (Items.Count > 0)
            {
                foreach(var item in Items)
                    item.Dispose();
            }

            base.Dispose(disposing);
        }
    }


    public class LocalizableStringCollection : 
        IList<LocalizableString>, IEnumerable<LocalizableString>, ICollection<LocalizableString>, 
        ICollection, IList, IEnumerable
    {
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        private ObservableCollection<LocalizableString> InnerList;

        public int Count => InnerList.Count;

        bool IList.IsReadOnly => ((IList)InnerList).IsReadOnly;

        bool ICollection<LocalizableString>.IsReadOnly => ((IList)InnerList).IsReadOnly;

        bool IList.IsFixedSize => ((IList)InnerList).IsFixedSize;

        object ICollection.SyncRoot => ((ICollection)InnerList).SyncRoot;

        bool ICollection.IsSynchronized => ((ICollection)InnerList).IsSynchronized;

        object IList.this[int index] { get => InnerList[index]; set => ((IList)InnerList)[index] = value; }
       
        public LocalizableString this[int index] { get => InnerList[index]; set => InnerList[index] = value; }

        public LocalizableStringCollection()
        {
            InnerList = new ObservableCollection<LocalizableString>();
            InnerList.CollectionChanged += InnerList_CollectionChanged;
        }

        private void InnerList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
        }

        public void AddRange(params LocalizableString[] localizableStrings)
        {
            for (int i = 0; i < localizableStrings.Length; i++)
                Add(localizableStrings[i]);
        }

        public int IndexOf(LocalizableString item)
        {
            return InnerList.IndexOf(item);
        }

        public void Insert(int index, LocalizableString item)
        {
            InnerList.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            InnerList.RemoveAt(index);
        }

        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public void Add(LocalizableString item)
        {
            InnerList.Add(item);
        }

        public void Clear()
        {
            InnerList.Clear();
        }

        public bool Contains(LocalizableString item)
        {
            return InnerList.Contains(item);
        }

        public void CopyTo(LocalizableString[] array, int arrayIndex)
        {
            InnerList.CopyTo(array, arrayIndex);
        }

        public bool Remove(LocalizableString item)
        {
            return InnerList.Remove(item);
        }

        public IEnumerator<LocalizableString> GetEnumerator()
        {
            return InnerList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return InnerList.GetEnumerator();
        }

        int IList.Add(object value)
        {
            return ((IList)InnerList).Add(value);
        }

        bool IList.Contains(object value)
        {
            return ((IList)InnerList).Contains(value);
        }

        int IList.IndexOf(object value)
        {
            return ((IList)InnerList).IndexOf(value);
        }

        void IList.Insert(int index, object value)
        {
            ((IList)InnerList).Insert(index, value);
        }

        void IList.Remove(object value)
        {
            ((IList)InnerList).Remove(value);
        }

        void ICollection.CopyTo(Array array, int index)
        {
            ((ICollection)InnerList).CopyTo(array, index);
        }
    }

    internal class LocalizableStringListDesigner : ComponentDesigner
    {
        private DesignerActionListCollection _ActionList;
        public override DesignerActionListCollection ActionLists
        {
            get
            {
                if (_ActionList == null)
                {
                    _ActionList = new DesignerActionListCollection();
                    _ActionList.Add(new LocalizableStringsActions(this));
                }
                return _ActionList;
            }
        }

        public override ICollection AssociatedComponents => 
            (Component as LocalizableStringList).Items;

        public override void Initialize(IComponent component)
        {
            base.Initialize(component);

            if (Component is LocalizableStringList stringList)
            {
                stringList.Items.CollectionChanged += Items_CollectionChanged;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Component is LocalizableStringList stringList)
                {
                    stringList.Items.CollectionChanged -= Items_CollectionChanged;
                }

            }
            base.Dispose(disposing);

        }
        private void Items_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (Component is LocalizableStringList stringList)
            {
                var listContainer = stringList.Site?.Container;

                foreach (var item in stringList.Items)
                {
                    var itemContainer = item.Site?.Container;
                    if (itemContainer != listContainer && listContainer != null)
                    {
                        itemContainer?.Remove(item);
                        listContainer.Add(item);
                    }
                }
            }
        }
    }

    internal class LocalizableStringsActions : DesignerActionList
    {
        private ComponentDesigner _Designer;

        public LocalizableStringsActions(ComponentDesigner designer)
            : base(designer.Component)
        {
            _Designer = designer;
        }

        public override DesignerActionItemCollection GetSortedActionItems()
        {
            return new DesignerActionItemCollection
            {
                new DesignerActionMethodItem(this, "InvokeItemsDialog", "Edit strings...", "Properties", "Edit localizable strings.", true),
            };
        }

        public void InvokeItemsDialog()
        {
            ShowCollectionEditor(_Designer, base.Component, "Items");
        }

        private static MethodInfo EditValueMI;

        private static void ShowCollectionEditor(ComponentDesigner designer, object objectToChange, string propName)
        {
            if (EditValueMI == null)
            {
                var systemDesignAssem = Assembly.GetAssembly(typeof(ComponentDesigner));
                var editorServiceType = systemDesignAssem.GetType("System.Windows.Forms.Design.EditorServiceContext");
                EditValueMI = editorServiceType.GetMethod("EditValue", BindingFlags.Static | BindingFlags.Public);
            }
            if (EditValueMI != null)
            {
                EditValueMI.Invoke(null, new object[] { designer, objectToChange, propName });
            }
        }
    }
}
