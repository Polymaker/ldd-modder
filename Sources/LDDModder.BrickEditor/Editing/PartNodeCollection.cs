using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Editing
{
    public class PartNodeCollection<T> : IList<T> where T : PartNode
    {
        private readonly PartNode Owner;
        private List<T> Nodes;

        public PartNodeCollection(PartNode owner)
        {
            Owner = owner;
            Nodes = new List<T>();
        }

        public T this[int index] { get => Nodes[index]; set => Nodes[index] = value; }

        public int Count => ((IList<T>)Nodes).Count;

        public bool IsReadOnly => ((IList<T>)Nodes).IsReadOnly;

        public void Add(T item)
        {
            Nodes.Add(item);
            item.Parent = Owner;
        }

        public void Clear()
        {
            Nodes.ForEach(n => n.Parent = null);
            Nodes.Clear();
        }

        public bool Contains(T item)
        {
            return Nodes.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            Nodes.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Nodes.GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return Nodes.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            Nodes.Insert(index, item);
            item.Parent = Owner;
        }

        public bool Remove(T item)
        {
            bool result = Nodes.Remove(item);
            if (result)
                item.Parent = null;
            return result;
        }

        public void RemoveAt(int index)
        {
            if (index >= 0 && index < Count)
                this[index].Parent = null;
            Nodes.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IList<T>)Nodes).GetEnumerator();
        }
    }


    public class PartNodeCollection : PartNodeCollection<PartNode>
    {
        public PartNodeCollection(PartNode owner) : base(owner)
        {
        }
    }
}
