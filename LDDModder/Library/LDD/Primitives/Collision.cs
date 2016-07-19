﻿using LDDModder.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace LDDModder.LDD.Primitives
{
    [Serializable]
    public abstract class Collision
    {
        internal static string[] AttributeOrder = new string[] { "angle", "ax", "ay", "az", "tx", "ty", "tz" };

        [XmlAttribute("angle")]
        public double Angle { get; set; }
        [XmlAttribute("ax")]
        public double Ax { get; set; }
        [XmlAttribute("ay")]
        public double Ay { get; set; }
        [XmlAttribute("az")]
        public double Az { get; set; }
        [XmlAttribute("tx")]
        public double Tx { get; set; }
        [XmlAttribute("ty")]
        public double Ty { get; set; }
        [XmlAttribute("tz")]
        public double Tz { get; set; }

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
                    return LDDModder.Serialization.XSerializationHelper.DefaultDeserialize<CollisionBox>(node);
                case "Sphere":
                    return LDDModder.Serialization.XSerializationHelper.DefaultDeserialize<CollisionSphere>(node);
            }
            return null;
        }

        //public static T Deserialize<T>(XElement node)
        //{
        //    var xmlSer = new XmlSerializer(typeof(T));
        //    return (T)xmlSer.Deserialize(node.CreateReader());
        //}

        //public static XElement Serialize(Collision collision)
        //{
        //    //XSerializationHelper.Serialize(
        //    var xmlSer = new XmlSerializer(collision.GetType());
        //    XDocument d = new XDocument();
        //    using (XmlWriter xw = d.CreateWriter())
        //        xmlSer.Serialize(xw, collision);
        //    return d.Root;
        //}

        public static IEnumerable<XElement> Serialize(IEnumerable<Collision> collisions)
        {
            foreach (var colObj in collisions)
            {
                var result = LDDModder.Serialization.XSerializationHelper.Serialize(colObj);
                if (result != null)
                {
                    result.SortAttributes(a => Array.IndexOf(AttributeOrder, a.Name.LocalName));
                    yield return result;
                }
            }
        }
    }
}
