using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Models.Navigation
{
    public class ProjectNodeCollection : IList<BaseProjectNode>
    {
        private List<BaseProjectNode> InnerList;

        public BaseProjectNode Owner { get; private set; }

        public BaseProjectNode this[int index] { get => ((IList<BaseProjectNode>)InnerList)[index]; set => ((IList<BaseProjectNode>)InnerList)[index] = value; }

        public int Count => ((IList<BaseProjectNode>)InnerList).Count;

        public bool IsReadOnly => ((IList<BaseProjectNode>)InnerList).IsReadOnly;

        public ProjectNodeCollection(BaseProjectNode owner)
        {
            Owner = owner;
            InnerList = new List<BaseProjectNode>();
        }

        public void Add(BaseProjectNode item)
        {
            item.Parent = Owner;
            ((IList<BaseProjectNode>)InnerList).Add(item);
        }

        public void Clear()
        {
            InnerList.ForEach(x => x.Parent = null);
            ((IList<BaseProjectNode>)InnerList).Clear();
        }

        public bool Contains(BaseProjectNode item)
        {
            return ((IList<BaseProjectNode>)InnerList).Contains(item);
        }

        public void CopyTo(BaseProjectNode[] array, int arrayIndex)
        {
            ((IList<BaseProjectNode>)InnerList).CopyTo(array, arrayIndex);
        }

        public IEnumerator<BaseProjectNode> GetEnumerator()
        {
            return ((IList<BaseProjectNode>)InnerList).GetEnumerator();
        }

        public int IndexOf(BaseProjectNode item)
        {
            return ((IList<BaseProjectNode>)InnerList).IndexOf(item);
        }

        public void Insert(int index, BaseProjectNode item)
        {
            item.Parent = Owner;
            ((IList<BaseProjectNode>)InnerList).Insert(index, item);
        }

        public bool Remove(BaseProjectNode item)
        {
            item.Parent = null;
            return ((IList<BaseProjectNode>)InnerList).Remove(item);
        }

        public void RemoveAt(int index)
        {
            InnerList[index].Parent = null;
            ((IList<BaseProjectNode>)InnerList).RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IList<BaseProjectNode>)InnerList).GetEnumerator();
        }
    }
}
