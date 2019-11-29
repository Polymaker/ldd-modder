using LDDModder.LDD.Primitives.Collisions;
using LDDModder.Simple3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace LDDModder.Modding.Editing
{
    [XmlInclude(typeof(PartBoxCollision)), XmlInclude(typeof(PartSphereCollision))]
    public abstract class PartCollision : PartElement, IPhysicalElement
    {
        public const string NODE_NAME = "Collision";

        private ItemTransform _Transform;

        [XmlElement]
        public ItemTransform Transform
        {
            get => _Transform;
            set => SetPropertyValue(ref _Transform, value);
        }

        [XmlAttribute]
        public abstract CollisionType CollisionType { get; }

        public PartCollision()
        {
            Transform = new ItemTransform();
        }

        public abstract void SetSize(Vector3 size);

        public abstract Vector3 GetSize();

        public abstract Collision GenerateLDD();

        public static PartCollision FromLDD(Collision collision)
        {
            if (collision is CollisionBox box)
            {
                return new PartBoxCollision()
                {
                    Transform = ItemTransform.FromLDD(box.Transform),
                    Size = box.Size
                };
            }
            else if (collision is CollisionSphere sphere)
            {
                return new PartSphereCollision()
                {
                    Transform = ItemTransform.FromLDD(sphere.Transform),
                    Radius = sphere.Radius
                };
            }
            return null;
        }

        public static PartCollision FromXml(XElement element)
        {
            var connectorType = element.ReadAttribute<CollisionType>("Type");
            PartCollision collision;
            if (connectorType == CollisionType.Box)
                collision = new PartBoxCollision();
            else
                collision = new PartSphereCollision();

            collision.LoadFromXml(element);
            return collision;
        }

        protected internal override void LoadFromXml(XElement element)
        {
            base.LoadFromXml(element);
            if (element.HasElement(nameof(Transform), out XElement transElem))
                Transform = ItemTransform.FromXml(transElem);
        }

        public override XElement SerializeToXml()
        {
            var elem = SerializeToXmlBase(NODE_NAME);
            elem.Add(new XAttribute("Type", CollisionType));
            elem.Add(new XComment(Transform.GetLddXml().ToString()));
            elem.Add(Transform.SerializeToXml(nameof(Transform)));
            return elem;
        }
    }
}
