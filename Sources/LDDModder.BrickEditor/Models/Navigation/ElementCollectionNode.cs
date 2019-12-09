using LDDModder.Modding.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Models.Navigation
{
    public class ElementCollectionNode : BaseProjectNode
    {
        public override PartProject Project => Element.Project;

        public PartElement Element { get;}

        public IElementCollection Collection { get; }

        public Type CollectionType => Collection.ElementType;

        public ElementCollectionNode(PartElement element, IElementCollection collection, string text)
        {
            Element = element;
            Collection = collection;
            NodeID = collection.GetHashCode().ToString();
            Text = text;
        }

        public override void RebuildChildrens()
        {
            base.RebuildChildrens();

            Childrens.Clear();

            foreach (var elem in Collection.GetElements())
                Childrens.Add(ProjectElementNode.CreateDefault(elem));
        }
    }
}
