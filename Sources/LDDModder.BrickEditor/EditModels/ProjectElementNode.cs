using LDDModder.BrickEditor.Resources;
using LDDModder.Modding.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.EditModels
{
    public class ProjectElementNode : BaseProjectNode
    {
        public override PartProject Project => Element.Project; 

        public PartElement Element { get; set; }

        public ProjectElementNode(PartElement element)
        {
            Element = element;
            NodeID = element.ID;
        }

        public ProjectElementNode(PartElement element, string text)
        {
            Element = element;
            NodeID = element.ID;
            Text = text;
        }

        public override void RebuildChildrens()
        {
            base.RebuildChildrens();
            Childrens.Clear();

            if (Element is PartSurface surface)
            {
                foreach (var elemGroup in surface.Components.GroupBy(x => x.ComponentType))
                    AddGrouppedChildrens(elemGroup, elemGroup.Key.ToString(), 5, 50);
            }
            
            else
            {

                foreach(var elemCollection in Element.ElementCollections)
                {
                    if (elemCollection.Count > 4)
                    {
                        Childrens.Add(new ProjectGroupNode(elemCollection, "Items"));
                    }
                    else
                    {
                        foreach (var elem in elemCollection.GetElements())
                            Childrens.Add(CreateDefault(elem));
                    }
                }

                foreach(var chilElem in Element.ChildElements)
                    Childrens.Add(CreateDefault(chilElem));
            }
        }

        public static ProjectElementNode CreateDefault(PartElement element)
        {
            var node = new ProjectElementNode(element);

            if (element is PartSurface surface)
            {
                if (surface.SurfaceID == 0)
                    node.Text = ModelLocalizations.Label_MainSurface;
                else
                    node.Text = string.Format(ModelLocalizations.Label_DecorationSurfaceNumber, surface.SurfaceID);
            }
            else
                node.Text = element.Name;
            return node;
        }
    }
}
