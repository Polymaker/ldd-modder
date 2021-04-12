using LDDModder.LDD.Primitives.Collisions;
using LDDModder.Simple3D;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace LDDModder.Modding
{
    [XmlInclude(typeof(PartBoxCollision)), XmlInclude(typeof(PartSphereCollision))]
    public abstract class PartCollision : PartElement, IPhysicalElement, IClonableElement
    {
        public const string NODE_NAME = "Collision";

        private ItemTransform _Transform;

        public ItemTransform Transform
        {
            get => _Transform;
            set => SetPropertyValue(ref _Transform, value);
        }

        public abstract CollisionType CollisionType { get; }

        public event EventHandler TranformChanged;

        public PartCollision()
        {
            Transform = new ItemTransform();
        }

        //protected override void OnPropertyChanged(ElementValueChangedEventArgs args)
        //{
        //    base.OnPropertyChanged(args);
        //    if (args.PropertyName == nameof(Transform))
        //        TranformChanged?.Invoke(this, EventArgs.Empty);
        //}

        protected override void OnPropertyValueChanged(PropertyValueChangedEventArgs args)
        {
            base.OnPropertyValueChanged(args);
            if (args.PropertyName == nameof(Transform))
                TranformChanged?.Invoke(this, EventArgs.Empty);
        }

        public void SetSize(Vector3 size)
        {
            var sizeD = (Vector3d)size;
            SetSize(sizeD.Rounded(6));
        }

        public abstract void SetSize(Vector3d size);

        public abstract Vector3d GetSize();

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

        public static PartCollision Create(CollisionType collisionType, double size = 1d)
        {
            if (collisionType == CollisionType.Box)
                return new PartBoxCollision(new Vector3d(size));
            return new PartSphereCollision(size);
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

        public PartCollision Clone()
        {
            if (this is PartBoxCollision boxCollision)
                return new PartBoxCollision(boxCollision.Size) { Transform = Transform.Clone() };
            if (this is PartSphereCollision sphereCollision)
                return new PartSphereCollision(sphereCollision.Radius) { Transform = Transform.Clone() };
            return null;
        }

        PartElement IClonableElement.Clone()
        {
            return Clone();
        }
    }
}
