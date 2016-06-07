using LDDModder.Utilities;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.Serialization;


namespace LDDModder.LDD.Primitives
{
    [Serializable]
    public abstract class Connectivity
    {
        [XmlAttribute("type")]
        public int Type { get; set; }
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

        /*
        Connectivity sub-types:

        Custom2DField (stud connection)
            Attributes = type, width, height, angle, ax, ay, az, tx, ty, tz
        Hinge
            Attributes = type, oriented, angle, ax, ay, az, tx, ty, tz, LimMin, LimMax, FlipLimMin, FlipLimMax, tag
        Axel
            Attributes = type, length, requireGrabbing, startCapped, endCapped, angle, ax, ay, az, tx, ty, tz, grabbing
        Fixed
            Attributes = type, axes, angle, ax, ay, az, tx, ty, tz, tag
        Gear
            Attributes = type, toothCount, radius, angle, ax, ay, az, tx, ty, tz
        Slider
            Attributes = type, length, startCapped, endCapped, angle, ax, ay, az, tx, ty, tz, cylindrical, spring, tag
        Ball
            Attributes = type, angle, ax, ay, az, tx, ty, tz
        Rail
            Attributes = type, angle, ax, ay, az, tx, ty, tz, length

        */

        public static IEnumerable<Connectivity> Deserialize(IEnumerable<XElement> nodes)
        {
            foreach (var node in nodes)
            {
                var result = Deserialize(node);
                if (result != null)
                    yield return result;
            }
        }

        public static IEnumerable<XElement> Serialize(IEnumerable<Connectivity> collisions)
        {
            foreach (var conObj in collisions)
            {
                var result = XSerializationHelper.Serialize(conObj);
                if (result != null)
                {
                    OrderAttributes(result);
                    yield return result;
                }
            }
        }

        private static void OrderAttributes(XElement elem)
        {
            string[] attributeNamesOrdered = null;
            switch (elem.Name.LocalName)
            {
                case "Custom2DField":
                    attributeNamesOrdered = ConnectivityCustom2DField.AttributeOrder;
                    break;
                case "Hinge":
                    attributeNamesOrdered = ConnectivityHinge.AttributeOrder;
                    break;
                case "Axel":
                    attributeNamesOrdered = ConnectivityAxel.AttributeOrder;
                    break;
                case "Slider":
                    attributeNamesOrdered = ConnectivitySlider.AttributeOrder;
                    break;
            }
            if (attributeNamesOrdered != null)
                elem.SortAttributes(a => Array.IndexOf(attributeNamesOrdered, a.Name.LocalName));
        }

        public static Connectivity Deserialize(XElement node)
        {
            switch (node.Name.LocalName)
            {
                case "Custom2DField":
                    return XSerializationHelper.DefaultDeserialize<ConnectivityCustom2DField>(node);
                case "Hinge":
                    return XSerializationHelper.DefaultDeserialize<ConnectivityHinge>(node);
                case "Axel":
                    return XSerializationHelper.DefaultDeserialize<ConnectivityAxel>(node);
                case "Slider":
                    return XSerializationHelper.DefaultDeserialize<ConnectivitySlider>(node);
            }
            return null;
        }

    }
}
