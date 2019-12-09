using LDDModder.BrickEditor.Resources;
using LDDModder.Modding.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Models.Navigation
{
    public class BaseProjectNode
    {
        public string NodeID { get; set; }

        public virtual PartProject Project { get; }

        public string Text { get; set; }

        public bool IsDirty { get; set; } = true;

        public BaseProjectNode Parent { get; set; }

        public BaseProjectNode RootNode => Parent == null ? this : Parent.RootNode;

        public int Level => Parent == null ? 0 : Parent.Level + 1;

        public ProjectNodeCollection Childrens { get; private set; }

        public string ImageKey { get; set; }

        protected BaseProjectNode()
        {
            Childrens = new ProjectNodeCollection(this);
        }

        public BaseProjectNode(PartProject project)
        {
            Project = project;
            Childrens = new ProjectNodeCollection(this);
        }

        public BaseProjectNode(PartProject project, string text) : this(project)
        {
            Text = text;
            Childrens = new ProjectNodeCollection(this);
        }

        public IEnumerable<BaseProjectNode> GetParents(bool includeSelf = false)
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

        public virtual bool HasChildrens()
        {
            if (IsDirty)
            {
                RebuildChildrens();
                IsDirty = false;
            }
            return Childrens.Any();
        }

        public virtual void RebuildChildrens()
        {

        }

        public IEnumerable<BaseProjectNode> GetChildHierarchy(bool includeSelf = false)
        {
            if (includeSelf)
                yield return this;

            if (HasChildrens())
            {
                foreach (var directChild in Childrens)
                {
                    foreach (var subChild in directChild.GetChildHierarchy(true))
                        yield return subChild;
                }
            }
        }

        protected void AutoGroupElements(IEnumerable<PartElement> elements, string groupTitle, int groupWhen, int maxGroupSize, bool groupOnSameLevel = false)
        {
            int totalElements = elements.Count();

            if (totalElements > groupWhen)
            {
                BaseProjectNode groupNode = this;

                if (!groupOnSameLevel)
                {
                    groupNode = new BaseProjectNode(Project, groupTitle)
                    {
                        IsDirty = false
                    };
                    Childrens.Add(groupNode);
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

                        var rangeNode = new ElementGroupNode(Project, rangeText);

                        if (groupOnSameLevel)
                            rangeNode.Text = groupTitle + " " + rangeNode.Text;

                        rangeNode.Elements.AddRange(elements.Skip(currIdx).Take(maxGroupSize));
                        groupNode.Childrens.Add(rangeNode);
                        currIdx += takeCount;
                        remaining -= takeCount;
                    }
                }
                else
                {
                    foreach (var elem in elements)
                        groupNode.Childrens.Add(ProjectElementNode.CreateDefault(elem));
                }
            }
            else
            {
                foreach (var elem in elements)
                    Childrens.Add(ProjectElementNode.CreateDefault(elem));
            }
        }
    
        public virtual bool CanDragDrop()
        {
            return false;
        }

        public virtual bool CanDropOn(BaseProjectNode node)
        {
            return false;
        }

        public virtual bool CanDropBefore(BaseProjectNode node)
        {
            return false;
        }

        public virtual bool CanDropAfter(BaseProjectNode node)
        {
            return false;
        }
    }
}
