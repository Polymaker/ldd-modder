using LDDModder.BrickEditor.ProjectHandling;
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

        public override void UpdateVisibility()
        {
            base.UpdateVisibility();

            var femaleModelExt = Element.GetExtension<FemaleStudModelExtension>();
            if (femaleModelExt != null)
            {
                bool isAlternate = (Element as FemaleStudModel).ReplacementMeshes == Collection;
                bool isVisible = isAlternate == femaleModelExt.ShowAlternateModels;

                if (!femaleModelExt.IsVisible)
                    VisibilityImageKey = isVisible ? "NotVisible" : "Hidden2";
                else
                    VisibilityImageKey = isVisible ? "Visible" : "Hidden";
            }
        }
    }
}
