using LDDModder.LDD.Primitives.Collisions;
using LDDModder.Serialization;
using LDDModder.Simple3D;
using System.Xml.Linq;

namespace LDDModder.Modding.Editing
{
    public class PartBoxCollision : PartCollision
    {
        //private PartProperty<Vector3> SizeProperty;
        private Vector3 _Size;

        public Vector3 Size
        {
            get => _Size;
            set => SetPropertyValue(ref _Size, value);
        }

        //public Vector3 Size
        //{
        //    get => SizeProperty.Value;
        //    set => SizeProperty.SetValue(value);
        //}

        public override CollisionType CollisionType => CollisionType.Box;

        public PartBoxCollision()
        {
        }

        public PartBoxCollision(Vector3 size)
        {
            //_Size = size;
            Size = size;
        }

        //protected override void DefineProperties()
        //{
        //    SizeProperty = Properties.DefineProperty<Vector3>("Size");
        //}

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
                Size = new Vector3(
                    sizeElem.ReadAttribute("X", 0f),
                    sizeElem.ReadAttribute("Y", 0f),
                    sizeElem.ReadAttribute("Z", 0f)
                );
            }
            
        }
    }
}
