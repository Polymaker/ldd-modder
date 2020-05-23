using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
