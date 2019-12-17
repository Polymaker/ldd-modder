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
    public class ProjectCollectionNode : BaseProjectNode
    {
        public override PartProject Project => Collection.Project;

        public ProjectManager Manager { get; set; }

        public IElementCollection Collection { get; }

        public Type ElementType => Collection.ElementType;

        public ProjectCollectionNode(IElementCollection collection)
        {
            Collection = collection;
            NodeID = collection.GetHashCode().ToString();
        }

        public ProjectCollectionNode(IElementCollection collection, string text)
        {
            Collection = collection;
            NodeID = collection.GetHashCode().ToString();
            Text = text;
        }

        public override void RebuildChildrens()
        {
            base.RebuildChildrens();

            Childrens.Clear();

            if (Collection.ElementType == typeof(PartConnection))
            {
                foreach (var elemGroup in Collection.GetElements().OfType<PartConnection>().GroupBy(x => x.ConnectorType))
                {
                    string groupTitle = ModelLocalizations.ResourceManager.GetString($"Label_{elemGroup.Key}Connectors");
                    
                    AutoGroupElements(elemGroup, groupTitle, 4, 10);
                }
            }
            else if (Collection.ElementType == typeof(PartCollision))
            {
                foreach (var elemGroup in Collection.GetElements().OfType<PartCollision>().GroupBy(x => x.CollisionType))
                {
                    string groupTitle = elemGroup.Key == LDD.Primitives.Collisions.CollisionType.Box ?
                        ModelLocalizations.Label_CollisionBoxes : ModelLocalizations.Label_CollisionSpheres;
                    
                    AutoGroupElements(elemGroup, groupTitle, 10, 10, true);
                }
            }
            else
            {
                foreach (var elem in Collection.GetElements())
                    Childrens.Add(ProjectElementNode.CreateDefault(elem));
            }
        }

        public override bool CanToggleVisibility()
        {
            return true;
        }

        public override bool GetIsVisible()
        {
            if (Manager != null)
            {
                if (Collection == Project.Surfaces)
                    return Manager.ShowPartModels;
                if (Collection == Project.Collisions)
                    return Manager.ShowCollisions;
                if (Collection == Project.Connections)
                    return Manager.ShowConnections;
            }
            return base.GetIsVisible();
        }
    }
}
