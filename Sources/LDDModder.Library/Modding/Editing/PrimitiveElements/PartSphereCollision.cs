using LDDModder.LDD.Primitives.Collisions;
using LDDModder.Simple3D;
using System.Xml.Linq;

namespace LDDModder.Modding.Editing
{
    public class PartSphereCollision : PartCollision
    {
        private double _Radius;

        public double Radius
        {
            get => _Radius;
            set => SetPropertyValue(ref _Radius, value);
        }

        public override CollisionType CollisionType => CollisionType.Sphere;

        public PartSphereCollision()
        {
            _Radius = 1d;
        }

        public PartSphereCollision(double radius)
        {
            _Radius = radius;
        }

        public override void SetSize(Vector3 size)
        {
            Radius = size.X;
        }

        public override Vector3 GetSize()
        {
            return new Vector3((float)Radius);
        }

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
