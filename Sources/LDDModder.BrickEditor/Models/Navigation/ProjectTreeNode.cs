using LDDModder.BrickEditor.ProjectHandling;
using LDDModder.BrickEditor.Resources;
using LDDModder.Modding.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Models.Navigation
{
    public class ProjectTreeNode
    {
        public string NodeID { get; set; }

        public string Text { get; set; }

        public IProjectManager Manager { get; set; }

        public ProjectTreeNode Parent { get; set; }

        public ProjectTreeNode RootNode => Parent == null ? this : Parent.RootNode;

        public int TreeLevel => Parent == null ? 0 : Parent.TreeLevel + 1;

        public ProjectTreeNodeCollection Nodes { get; private set; }

        public string ImageKey { get; set; }

        public string VisibilityImageKey { get; set; }


        public ProjectTreeNode()
        {
            NodeID = GetHashCode().ToString();
            Nodes = new ProjectTreeNodeCollection(this);
            nodesDirty = true;
        }

        public ProjectTreeNode(string text) : this()
        {
            Text = text;
        }

        internal void AssignParent(ProjectTreeNode node)
        {
            Parent = node;
            Manager = node?.Manager;
        }

        public virtual void FreeObjects()
        {
            foreach (var child in Nodes)
                child.FreeObjects();
            Manager = null;
        }

        #region Children Handling

        protected bool nodesDirty;

        public virtual void InvalidateChildrens()
        {
            nodesDirty = true;
            foreach (var childNode in Nodes)
            {
                childNode.nodesDirty = true;
            }
        }

        public bool HasChildrens()
        {
            if (nodesDirty)
                RebuildChildrensCore();

            return Nodes.Any();
        }

        private void RebuildChildrensCore()
        {
            RebuildChildrens();
            nodesDirty = false;
        }

        protected virtual void RebuildChildrens()
        {
            Nodes.Clear();
        }

        public IEnumerable<ProjectTreeNode> GetChildHierarchy(bool includeSelf = false)
        {
            if (includeSelf)
                yield return this;

            if (HasChildrens())
            {
                foreach (var directChild in Nodes)
                {
                    foreach (var subChild in directChild.GetChildHierarchy(true))
                        yield return subChild;
                }
            }
        }

        public IEnumerable<ProjectTreeNode> GetParents(bool includeSelf = false)
        {
            if (includeSelf)
                yield return this;

            if (Parent != null)
            {
                yield return Parent;
                foreach (var other in Parent.GetParents())
                    yield return other;
            }
        }

        protected void AutoGroupElements(IEnumerable<PartElement> elements, string groupTitle, int groupWhen, int maxGroupSize, bool groupOnSameLevel = false)
        {
            int totalElements = elements.Count();

            if (totalElements > groupWhen)
            {
                ProjectTreeNode groupNode = this;

                if (!groupOnSameLevel)
                {
                    groupNode = new ProjectTreeNode(groupTitle)
                    {
                        nodesDirty = false,
                        NodeID = $"{NodeID}_{groupTitle}"
                    };
                    Nodes.Add(groupNode);
                }

                //var parentNode = groupNode.Parent;

                if (totalElements > maxGroupSize)
                {
                    int remaining = totalElements;
                    int currIdx = 0;

                    while (remaining > 0)
                    {
                        int takeCount = Math.Min(remaining, maxGroupSize);

                        string rangeText = string.Empty;
                        if (takeCount / (double)maxGroupSize < 0.5)
                            rangeText = string.Format(ModelLocalizations.NodeRangeFormat2, currIdx + 1);
                        else
                            rangeText = string.Format(ModelLocalizations.NodeRangeFormat1, currIdx + 1, currIdx + takeCount);

                        var rangeNode = new ElementGroupNode(rangeText);
                        rangeNode.NodeID = $"{NodeID}_{groupTitle}_{currIdx + 1}";
                        if (groupOnSameLevel)
                            rangeNode.Text = groupTitle + " " + rangeNode.Text;

                        rangeNode.Elements.AddRange(elements.Skip(currIdx).Take(maxGroupSize));
                        groupNode.Nodes.Add(rangeNode);
                        currIdx += takeCount;
                        remaining -= takeCount;
                    }
                }
                else
                {
                    foreach (var elem in elements)
                        groupNode.Nodes.Add(ProjectElementNode.CreateDefault(elem));
                }
            }
            else
            {
                foreach (var elem in elements)
                    Nodes.Add(ProjectElementNode.CreateDefault(elem));
            }
        }

        #endregion

        public virtual void UpdateVisibility()
        {
            VisibilityImageKey = string.Empty;
        }

        #region Drag & Drop

        public virtual bool CanDragDrop()
        {
            return false;
        }

        public virtual bool CanDropOn(ProjectTreeNode node)
        {
            return false;
        }

        public virtual bool CanDropBefore(ProjectTreeNode node)
        {
            return false;
        }

        public virtual bool CanDropAfter(ProjectTreeNode node)
        {
            return false;
        }

        #endregion
    
    }
}
