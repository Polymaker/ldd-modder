using LDDModder.Simple3D;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using LDDModder.Serialization;

namespace LDDModder.LDD.Primitives.Collisions
{
    public class CollisionBox : Collision
    {
        private Vector3d _Size;

        public Vector3d Size
        {
            get => _Size;
            set => SetPropertyValue(ref _Size, value);
        }

        public CollisionBox()
        {
        }

        public CollisionBox(Vector3d size, Transform transform)
        {
            Size = size;
            Transform = transform;
        }

        public override Vector3d GetSize()
        {
            return Size;
        }

        public override XElement SerializeToXml()
        {
            var elem = new XElement("Box");
            elem.Add(Size.ToXmlAttributes("sX", "sY", "sZ"));
            elem.Add(Transform.ToXmlAttributes());
            return elem;
        }

        public override void LoadFromXml(XElement element)
        {
            Transform = Transform.FromElementAttributes(element);
            Size = new Vector3d(
                    element.ReadAttribute<double>("sX"),
                    element.ReadAttribute<double>("sY"),
                    element.ReadAttribute<double>("sZ"));
        }
    }
}
