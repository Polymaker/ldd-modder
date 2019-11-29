using LDDModder.LDD.Primitives.Collisions;
using LDDModder.Simple3D;
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

        public PartSphereCollision()
        {
            _Radius = 1f;
        }

        public PartSphereCollision(float radius)
        {
            _Radius = radius;
        }

        public override void SetSize(Vector3 size)
        {
            Radius = size.X;
        }

        public override Vector3 GetSize()
        {
            return new Vector3(Radius);
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
