using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace LDDModder.LDD.Primitives
{
    [Serializable]
    public abstract class Collision
    {
        [XmlAttribute("angle")]
        public float Angle { get; set; }
        [XmlAttribute("ax")]
        public float Ax { get; set; }
        [XmlAttribute("ay")]
        public float Ay { get; set; }
        [XmlAttribute("az")]
        public float Az { get; set; }
        [XmlAttribute("tx")]
        public float Tx { get; set; }
        [XmlAttribute("ty")]
        public float Ty { get; set; }
        [XmlAttribute("tz")]
        public float Tz { get; set; }

        public static IEnumerable<Collision> Deserialize(IEnumerable<XElement> nodes)
        {
            foreach (var node in nodes)
            {
                var result = Deserialize(node);
                if (result != null)
                    yield return result;
            }
        }

        public static Collision Deserialize(XElement node)
        {
            switch (node.Name.LocalName)
            {
                case "Box":
                    return Deserialize<CollisionBox>(node);
                case "Sphere":
                    return Deserialize<CollisionSphere>(node);
            }
            return null;
        }

        public static T Deserialize<T>(XElement node)
        {
            var xmlSer = new XmlSerializer(typeof(T));
            return (T)xmlSer.Deserialize(node.CreateReader());
        }

        public static XElement Serialize(Collision collision)
        {
            var xmlSer = new XmlSerializer(collision.GetType());
            XDocument d = new XDocument();
            using (XmlWriter xw = d.CreateWriter())
                xmlSer.Serialize(xw, collision);
            return d.Root;
        }

        public static IEnumerable<XElement> Serialize(IEnumerable<Collision> collisions)
        {
            foreach (var col in collisions)
            {
                var result = Serialize(col);
                if (result != null)
                    yield return result;
            }
        }
    }
}
