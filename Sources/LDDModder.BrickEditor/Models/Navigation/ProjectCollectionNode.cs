using LDDModder.BrickEditor.ProjectHandling;
using LDDModder.BrickEditor.Resources;
using LDDModder.Modding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Models.Navigation
{
    public class ProjectCollectionNode : ProjectTreeNode
    {
        //public override PartProject Project => Collection.Project;

        //public ProjectManager Manager { get; set; }

        public IElementCollection Collection { get; }

        public Type ElementType => Collection.ElementType;

        public ProjectCollectionNode(IElementCollection collection)
        {
            Collection = collection;
            NodeID = collection.GetHashCode().ToString();
        }

        public ProjectCollectionNode(IElementCollection collection, string text) : base (text)
        {
            Collection = collection;
            NodeID = collection.GetHashCode().ToString();
            Text = text;
        }

        protected override void RebuildChildrens()
        {
            base.RebuildChildrens();

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
                    Nodes.Add(ProjectElementNode.CreateDefault(elem));
            }
        }

        public override VisibilityState GetVisibilityState()
        {
            if (Collection == Manager.CurrentProject.Surfaces)
                return Manager.ShowPartModels ? VisibilityState.Visible : VisibilityState.Hidden;
            if (Collection == Manager.CurrentProject.Collisions)
                return Manager.ShowCollisions ? VisibilityState.Visible : VisibilityState.Hidden;
            if (Collection == Manager.CurrentProject.Connections)
                return Manager.ShowConnections ? VisibilityState.Visible : VisibilityState.Hidden;

            return VisibilityState.None;
        }

        //public override void UpdateVisibilityIcon()
        //{
        //    base.UpdateVisibilityIcon();
        //    if (Manager != null)
        //    {
        //        if (Collection == Manager.CurrentProject.Surfaces)
        //            VisibilityImageKey = Manager.ShowPartModels ? "Visible" : "Hidden";
        //        if (Collection == Manager.CurrentProject.Collisions)
        //            VisibilityImageKey = Manager.ShowCollisions ? "Visible" : "Hidden";
        //        if (Collection == Manager.CurrentProject.Connections)
        //            VisibilityImageKey = Manager.ShowConnections ? "Visible" : "Hidden";
        //        //if (Collection == Manager.CurrentProject.Bones)
        //        //    VisibilityImageKey = Manager.ShowBones ? "Visible" : "Hidden";
        //    }
        //}

        protected override bool CanToggleVisibilityCore()
        {
            return Collection == Manager.CurrentProject.Surfaces ||
                Collection == Manager.CurrentProject.Collisions ||
                Collection == Manager.CurrentProject.Connections;
        }

        public override void ToggleVisibility()
        {
            if (Collection == Manager.CurrentProject.Surfaces)
                Manager.ShowPartModels = !Manager.ShowPartModels;
            if (Collection == Manager.CurrentProject.Collisions)
                Manager.ShowCollisions = !Manager.ShowCollisions;
            if (Collection == Manager.CurrentProject.Connections)
                Manager.ShowConnections = !Manager.ShowConnections;
        }
    }
}
