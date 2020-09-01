using LDDModder.BrickEditor.ProjectHandling;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Models.Navigation
{
    public class ProjectTreeNodeCollection : IList<ProjectTreeNode>
    {
        private List<ProjectTreeNode> InnerList;

        public IProjectManager Manager { get; private set; }

        //public IProjectDocument Document { get; private set; }

        public ProjectTreeNode Owner { get; private set; }

        public ProjectTreeNode this[int index] { get => InnerList[index]; set => InnerList[index] = value; }

        public int Count => ((IList<ProjectTreeNode>)InnerList).Count;

        public bool IsReadOnly => ((IList<ProjectTreeNode>)InnerList).IsReadOnly;

        public ProjectTreeNodeCollection(ProjectTreeNode owner)
        {
            Owner = owner;
            InnerList = new List<ProjectTreeNode>();
        }

        public ProjectTreeNodeCollection(IProjectManager manager)
        {
            Manager = manager;
            InnerList = new List<ProjectTreeNode>();
        }

        //public ProjectTreeNodeCollection(IProjectDocument document)
        //{
        //    Document = document;
        //    InnerList = new List<ProjectTreeNode>();
        //}

        private void UpdateNodeParent(ProjectTreeNode node, bool assign)
        {
            if (assign)
            {
                if (Owner != null)
                    node.AssignParent(Owner);
                else
                {
                    node.Manager = Manager;
                    //node.Document = Document;
                }
            }
            else
                node.AssignParent(null);
        }

        public void Add(ProjectTreeNode item)
        {
            UpdateNodeParent(item, true);
            InnerList.Add(item);
        }

        public void Clear()
        {
            InnerList.ForEach(x => UpdateNodeParent(x, false));
            InnerList.Clear();
        }

        public bool Contains(ProjectTreeNode item)
        {
            return InnerList.Contains(item);
        }

        public void CopyTo(ProjectTreeNode[] array, int arrayIndex)
        {
            InnerList.CopyTo(array, arrayIndex);
        }

        public IEnumerator<ProjectTreeNode> GetEnumerator()
        {
            return InnerList.GetEnumerator();
        }

        public int IndexOf(ProjectTreeNode item)
        {
            return InnerList.IndexOf(item);
        }

        public void Insert(int index, ProjectTreeNode item)
        {
            UpdateNodeParent(item, true);
            item.Parent = Owner;
            InnerList.Insert(index, item);
        }

        public bool Remove(ProjectTreeNode item)
        {
            UpdateNodeParent(item, false);
            return InnerList.Remove(item);
        }

        public void RemoveAt(int index)
        {
            UpdateNodeParent(InnerList[index], false);
            InnerList.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return InnerList.GetEnumerator();
        }
    }
}
