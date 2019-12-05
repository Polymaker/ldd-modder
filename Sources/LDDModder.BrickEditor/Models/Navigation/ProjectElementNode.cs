using LDDModder.BrickEditor.Resources;
using LDDModder.Modding.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Models.Navigation
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
                {
                    string groupTitle = ModelLocalizations.ResourceManager.GetString($"Label_{elemGroup.Key}Components");
                    
                    int itemCount = elemGroup.Count();
                    int groupSize = 10;
                    if (itemCount >= 50)
                        groupSize = 20;
                    else if (itemCount >= 100)
                        groupSize = 50;
                    AutoGroupElements(elemGroup, groupTitle, 5, groupSize);
                }
            }
            else if (Element is SurfaceComponent surfaceComponent)
            {
                if (surfaceComponent is FemaleStudModel femaleStud && 
                    femaleStud.ReplacementMeshes.Any())
                {
                    AutoGroupElements(femaleStud.Meshes,
                        ModelLocalizations.Label_DefaultMeshes, 0, 10, false);
                    AutoGroupElements(femaleStud.ReplacementMeshes,
                        ModelLocalizations.Label_AlternateMeshes, 0, 10, false);
                }
                else
                {
                    AutoGroupElements(surfaceComponent.Meshes,
                        ModelLocalizations.Label_Models, 10, 10, true);
                }
            }
            else
            {

                foreach (var elemCollection in Element.ElementCollections)
                {
                    if (elemCollection.ElementType == typeof(StudReference))
                        continue;

                    AutoGroupElements(elemCollection.GetElements(), "Items", 5, 10);
                }

                foreach (var childElem in Element.ChildElements)
                {
                    if (childElem is StudReference)
                        continue;
                    Childrens.Add(CreateDefault(childElem));
                }
            }
        }

        public static ProjectElementNode CreateDefault(PartElement element)
        {
            var node = new ProjectElementNode(element);

            if (element is PartSurface surface)
            {
                if (surface.SurfaceID == 0)
                {
                    node.Text = ModelLocalizations.Label_MainSurface;
                    node.ImageKey = "Surface_Main";
                }
                else
                {
                    node.Text = string.Format(ModelLocalizations.Label_DecorationSurfaceNumber, surface.SurfaceID);
                    node.ImageKey = "Surface_Decoration";
                }
            }
            else
            {
                node.Text = element.Name;
                if (element is SurfaceComponent component)
                    node.ImageKey = $"Model_{component.ComponentType}";
                else if (element is PartConnection connection)
                    node.ImageKey = $"Connection_{connection.ConnectorType}";
                else if (element is PartCollision collision)
                    node.ImageKey = $"Collision_{collision.CollisionType}";
                else if (element is ModelMeshReference || element is ModelMesh)
                    node.ImageKey = "Mesh";
            }
            return node;
        }
    }
}
