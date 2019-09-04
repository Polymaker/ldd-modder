using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Editing
{
    public class PartNodeCollection : IList<PartNode>
    {
        private readonly PartNode Owner;
        private List<PartNode> Nodes;

        public PartNodeCollection(PartNode owner)
        {
            Owner = owner;
            Nodes = new List<PartNode>();
        }

        public PartNode this[int index] { get => Nodes[index]; set => Nodes[index] = value; }

        public int Count => ((IList<PartNode>)Nodes).Count;

        public bool IsReadOnly => ((IList<PartNode>)Nodes).IsReadOnly;

        public void Add(PartNode item)
        {
            Nodes.Add(item);
            item.Parent = Owner;
        }

        public void Clear()
        {
            Nodes.ForEach(n => n.Parent = null);
            Nodes.Clear();
        }

        public bool Contains(PartNode item)
        {
            return Nodes.Contains(item);
        }

        public void CopyTo(PartNode[] array, int arrayIndex)
        {
            Nodes.CopyTo(array, arrayIndex);
        }

        public IEnumerator<PartNode> GetEnumerator()
        {
            return Nodes.GetEnumerator();
        }

        public int IndexOf(PartNode item)
        {
            return Nodes.IndexOf(item);
        }

        public void Insert(int index, PartNode item)
        {
            Nodes.Insert(index, item);
            item.Parent = Owner;
        }

        public bool Remove(PartNode item)
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
            return ((IList<PartNode>)Nodes).GetEnumerator();
        }
    }
}
