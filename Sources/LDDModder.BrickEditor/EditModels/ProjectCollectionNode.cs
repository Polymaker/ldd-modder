using LDDModder.Modding.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.EditModels
{
    public class ProjectCollectionNode : BaseProjectNode
    {
        public override PartProject Project => Collection.Project; 

        public IElementCollection Collection { get; }

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
                    AddGrouppedChildrens(elemGroup, elemGroup.Key.ToString(), 4, 10);
            }
            else if (Collection.ElementType == typeof(PartCollision))
            {
                foreach (var elemGroup in Collection.GetElements().OfType<PartCollision>().GroupBy(x => x.CollisionType))
                    AddGrouppedChildrens(elemGroup, elemGroup.Key.ToString(), 10, 10);
            }
            else
            {
                foreach (var elem in Collection.GetElements())
                    Childrens.Add(ProjectElementNode.CreateDefault(elem));
            }
        }
    }
}
