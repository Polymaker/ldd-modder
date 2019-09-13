using LDDModder.LDD.Primitives.Collisions;
using LDDModder.Simple3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LDDModder.Modding.Editing
{
    public abstract class PartCollision : PartComponent
    {
        public ItemTransform Transform { get; set; }

        public PartCollision()
        {
            Transform = new ItemTransform();
        }

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

        public override XElement SerializeToXml()
        {
            var elem = SerializeToXmlBase("Collision");
            elem.Add(Transform.SerializeToXml());
            return elem;
        }
    }

    public class PartBoxCollision : PartCollision
    {
        public Vector3 Size { get; set; }

        public override Collision GenerateLDD()
        {
            return new CollisionBox(Size, Transform.ToLDD());
        }

        public override XElement SerializeToXml()
        {
            var elem = base.SerializeToXml();
            elem.Add(new XAttribute("Type", "Box"));
            elem.AddNumberAttribute("SizeX", Size.X);
            elem.AddNumberAttribute("SizeY", Size.Y);
            elem.AddNumberAttribute("SizeZ", Size.Z);
            return elem;
        }
    }

    public class PartSphereCollision : PartCollision
    {
        public float Radius { get; set; }

        public override Collision GenerateLDD()
        {
            return new CollisionSphere(Radius, Transform.ToLDD());
        }

        public override XElement SerializeToXml()
        {
            var elem = base.SerializeToXml();
            elem.Add(new XAttribute("Type", "Sphere"));
            elem.AddNumberAttribute("Radius", Radius);
            return elem;
        }
    }
}
