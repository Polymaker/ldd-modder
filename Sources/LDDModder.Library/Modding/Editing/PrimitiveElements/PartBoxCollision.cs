using LDDModder.LDD.Primitives.Collisions;
using LDDModder.Serialization;
using LDDModder.Simple3D;
using System.Xml.Linq;

namespace LDDModder.Modding.Editing
{
    public class PartBoxCollision : PartCollision
    {
        private Vector3 _Size;

        public Vector3 Size
        {
            get => _Size;
            set => SetPropertyValue(ref _Size, value);
        }

        public override CollisionType CollisionType => CollisionType.Box;

        public PartBoxCollision()
        {
            _Size = new Vector3(1f);
        }

        public PartBoxCollision(Vector3 size)
        {
            _Size = size;
        }

        public override void SetSize(Vector3 size)
        {
            Size = size;
        }

        public override Vector3 GetSize()
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
            elem­.Add(new XElement("Size", Size.ToXmlAttributes()));
            return elem;
        }

        protected internal override void LoadFromXml(XElement element)
        {
            base.LoadFromXml(element);
            if (element.HasElement("Size", out XElement sizeElem))
            {
                _Size = new Vector3(
                    sizeElem.ReadAttribute("X", 0f),
                    sizeElem.ReadAttribute("Y", 0f),
                    sizeElem.ReadAttribute("Z", 0f)
                );
            }
            
        }
    }
}
