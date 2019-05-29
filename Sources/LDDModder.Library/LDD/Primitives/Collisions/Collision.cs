using LDDModder.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LDDModder.LDD.Primitives.Collisions
{
    public abstract class Collision : IXmlObject
    {
        public Transform Transform { get; set; }

        public abstract void LoadFromXml(XElement element);

        public abstract XElement SerializeToXml();

        public static Collision DeserializeCollision(XElement element)
        {
            if (element.Name.LocalName == "Box")
            {
                var col = new CollisionBox();
                col.LoadFromXml(element);
                return col;
            }
            else if (element.Name.LocalName == "Sphere")
            {
                var col = new CollisionSphere();
                col.LoadFromXml(element);
                return col;
            }
            return null;
        }
    }
}
