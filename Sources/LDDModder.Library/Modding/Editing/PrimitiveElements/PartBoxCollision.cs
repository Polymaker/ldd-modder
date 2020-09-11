using LDDModder.LDD.Primitives.Collisions;
using LDDModder.Serialization;
using LDDModder.Simple3D;
using System.Xml.Linq;

namespace LDDModder.Modding.Editing
{
    public class PartBoxCollision : PartCollision
    {
        private Vector3d _Size;

        public Vector3d Size
        {
            get => _Size;
            set => SetPropertyValue(ref _Size, value);
        }

        public override CollisionType CollisionType => CollisionType.Box;

        public PartBoxCollision()
        {
            _Size = new Vector3d(1d);
        }

        public PartBoxCollision(Vector3d size)
        {
            _Size = size;
        }

        public override void SetSize(Vector3d size)
        {
            Size = size;
        }

        public override Vector3d GetSize()
        {
            return Size;
        }

        public override Collision GenerateLDD()
        {
            return new CollisionBox(Size, Transform.ToLDD());
        }

        public override XElement SerializeToXml()
        {
            var elem = base.SerializeToXml();
            elem­.Add(new XElement("Size", Size.Rounded(6).ToXmlAttributes()));
            return elem;
        }

        protected internal override void LoadFromXml(XElement element)
        {
            base.LoadFromXml(element);
            if (element.HasElement("Size", out XElement sizeElem))
            {
                _Size = new Vector3d(
                    sizeElem.ReadAttribute("X", 0d),
                    sizeElem.ReadAttribute("Y", 0d),
                    sizeElem.ReadAttribute("Z", 0d)
                );
                //_Size = _Size.Rounded(6);
            }
            
        }
    }
}
