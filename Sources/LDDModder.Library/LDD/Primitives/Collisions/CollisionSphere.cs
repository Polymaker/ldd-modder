using System.Xml.Linq;

namespace LDDModder.LDD.Primitives.Collisions
{
    public class CollisionSphere : Collision
    {
        private double _Radius;
        
        public double Radius
        {
            get => _Radius;
            set => SetPropertyValue(ref _Radius, value);
        }

        public CollisionSphere()
        {
        }

        public CollisionSphere(double radius, Transform transform)
        {
            Radius = radius;
            Transform = transform;
        }

        public override XElement SerializeToXml()
        {
            var elem = new XElement("Sphere");
            elem.AddNumberAttribute("radius", Radius);
            elem.Add(Transform.ToXmlAttributes());
            return elem;
        }

        public override void LoadFromXml(XElement element)
        {
            Transform = Transform.FromElementAttributes(element);
            Radius = element.ReadAttribute("radius", 1d);
        }
    }
}
