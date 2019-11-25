using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LDDModder.Modding.Editing;

namespace LDDModder.BrickEditor.EditModels
{
    public class ElementGroupNode : BaseProjectNode
    {
        public List<PartElement> Elements { get; set; }

        public ElementGroupNode(PartProject project) : base(project)
        {
            Elements = new List<PartElement>();
        }

        public ElementGroupNode(PartProject project, string text) : base(project, text)
        {
            Elements = new List<PartElement>();
        }

        public override void RebuildChildrens()
        {
            base.RebuildChildrens();

            Childrens.Clear();

            foreach (var elem in Elements)
                Childrens.Add(ProjectElementNode.CreateDefault(elem));
        }
    }
}
