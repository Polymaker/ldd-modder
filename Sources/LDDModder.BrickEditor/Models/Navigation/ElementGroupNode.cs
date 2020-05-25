using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LDDModder.BrickEditor.ProjectHandling;
using LDDModder.Modding.Editing;

namespace LDDModder.BrickEditor.Models.Navigation
{
    public class ElementGroupNode : ProjectTreeNode
    {
        public List<PartElement> Elements { get; set; }

        public ElementGroupNode()
        {
            Elements = new List<PartElement>();
        }

        public ElementGroupNode(string text) : base(text)
        {
            Elements = new List<PartElement>();
        }

        protected override void RebuildChildrens()
        {
            base.RebuildChildrens();

            foreach (var elem in Elements)
                Nodes.Add(ProjectElementNode.CreateDefault(elem));
        }

        public bool SupportsVisibility()
        {
            var childModels = Nodes.OfType<ProjectElementNode>().Select(x => x.Element.GetExtension<ModelElementExtension>()).Where(x => x != null).ToList();

            return childModels.Any();
        }

        public override void UpdateVisibility()
        {
            base.UpdateVisibility();

            //var childModels = Nodes.OfType<ProjectElementNode>().Select(x => x.Element.GetExtension<ModelElementExtension>()).Where(x => x != null).ToList();

            //if (childModels.Any())
            //{
            //    int hiddenModels = childModels.Count(x => x.IsHidden);
            //    int visibleModels = childModels.Count - hiddenModels;
            //    //bool 
            //    VisibilityImageKey = (hiddenModels > visibleModels) ? "Hidden" : "Visible";
            //}
            //else
            //    VisibilityImageKey = string.Empty;
        }

        public void ToggleVisibility()
        {
            var childModels = Nodes.OfType<ProjectElementNode>().Select(x => x.Element.GetExtension<ModelElementExtension>()).Where(x => x != null).ToList();

            if (childModels.Any())
            {
                int hiddenModels = childModels.Count(x => x.IsHidden);
                int visibleModels = childModels.Count - hiddenModels;
                bool setHidden = visibleModels > hiddenModels;
                childModels.ForEach(x => x.IsHidden = setHidden);
            }
        }
    }
}
