using LDDModder.Modding.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.EditModels
{
    public class BaseProjectNode
    {
        public string NodeID { get; set; }

        public virtual PartProject Project { get; }

        public string Text { get; set; }

        public bool IsDirty { get; set; } = true;

        public List<BaseProjectNode> Childrens { get; set; }

        protected BaseProjectNode()
        {
            Childrens = new List<BaseProjectNode>();
        }

        public BaseProjectNode(PartProject project)
        {
            Project = project;
            Childrens = new List<BaseProjectNode>();
        }

        public BaseProjectNode(PartProject project, string text) : this(project)
        {
            Text = text;
            Childrens = new List<BaseProjectNode>();
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
                foreach(var directChild in Childrens)
                {
                    foreach(var subChild in directChild.GetChildHierarchy(true))
                        yield return subChild;
                }
            }
        }

        protected void AddGrouppedChildrens(IEnumerable<PartElement> elements, string groupKey, int groupWhen, int maxGroupSize)
        {
            int totalElements = elements.Count();
            if (totalElements > groupWhen)
            {
                BaseProjectNode groupNode = null;

                if (groupWhen == maxGroupSize)
                    groupNode = this;
                else
                {
                    groupNode = new BaseProjectNode(Project, groupKey);
                    groupNode.IsDirty = false;
                    Childrens.Add(groupNode);
                }

                if (totalElements > maxGroupSize)
                {
                    int remaining = totalElements;
                    int currIdx = 0;

                    while (remaining > 0)
                    {
                        int takeCount = Math.Min(remaining, maxGroupSize);
                        var rangeNode = new BaseProjectNode(Project, $"{currIdx + 1} to {currIdx + takeCount}");
                        if (groupWhen == maxGroupSize)
                            rangeNode.Text = groupKey + " " + rangeNode.Text;
                        rangeNode.IsDirty = false;
                        groupNode.Childrens.Add(rangeNode);
                        foreach (var elem in elements.Skip(currIdx).Take(maxGroupSize))
                            rangeNode.Childrens.Add(ProjectElementNode.CreateDefault(elem));
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
    }
}
