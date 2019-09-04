using LDDModder.LDD.Primitives.Collisions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LDDModder.BrickEditor.Editing
{
    public class CollisionNode : PartNode
    {
        public Collision Collision { get; set; }

        public override XElement SerializeToXml()
        {
            var elem = base.SerializeToXml();
            elem.Add(Collision.SerializeToXml());
            return elem;
        }

        public static CollisionNode Create(Collision collision)
        {
            var node = new CollisionNode()
            {
                Collision = collision
            };
            node.GenerateID();
            return node;
        }
    }
}
