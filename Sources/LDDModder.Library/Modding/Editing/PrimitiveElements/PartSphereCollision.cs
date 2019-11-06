using LDDModder.LDD.Primitives.Collisions;
using System.Xml.Linq;

namespace LDDModder.Modding.Editing
{
    public class PartSphereCollision : PartCollision
    {
        private float _Radius;

        public float Radius
        {
            get => _Radius;
            set => SetPropertyValue(ref _Radius, value);
        }

        public override CollisionType CollisionType => CollisionType.Sphere;

        public override Collision GenerateLDD()
        {
            return new CollisionSphere(Radius, Transform.ToLDD());
        }

        public override XElement SerializeToXml()
        {
            var elem = base.SerializeToXml();
            elem.Add(new XElement("Size", new XAttribute("Radius", Radius)));
            return elem;
        }

        protected internal override void LoadFromXml(XElement element)
        {
            base.LoadFromXml(element);
            if (element.HasElement("Size", out XElement sizeElem))
                Radius = sizeElem.ReadAttribute("Radius", 1f);
        }
    }
}
