using System.Xml.Linq;

namespace LDDModder.LDD.Primitives.Collisions
{
    public class CollisionSphere : Collision
    {
        public float Radius { get; set; }

        public CollisionSphere()
        {
        }

        public CollisionSphere(float radius, Transform transform)
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
            Radius = element.ReadAttribute<float>("radius");
        }
    }
}
