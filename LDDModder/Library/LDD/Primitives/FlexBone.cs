using LDDModder.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml.Linq;

namespace LDDModder.LDD.Primitives
{
    [XmlRoot("Bone")]
    public class FlexBone : LDDModder.Serialization.XSerializable
    {
        private List<Connectivity> _Connections;
        private List<Collision> _Collisions;

        [XmlAttribute("boneId")]
        public int Id { get; set; }

        //TODO: change type for class to represent value (string value = 3 bone Id (ref) seperated by comma)
        [XmlAttribute("flexCheckConnection")]
        public string FlexCheckConnection { get; set; }

        public List<Collision> Collisions
        {
            get { return _Collisions; }
        }

        public List<Connectivity> Connections
        {
            get { return _Connections; }
        }

        public Transform Transform { get; set; }

        public PhysicsAttributes PhysicsAttributes { get; set; }

        public BoundingBox Bounding { get; set; }

        public FlexBone()
        {
            _Connections = new List<Connectivity>();
            _Collisions = new List<Collision>();
        }

        protected override XElement SerializeToXElement()
        {
            var root = new XElement("Bone");
            //Attributes

            root.Add(new XAttribute("boneId", Id));

            if(!string.IsNullOrEmpty(FlexCheckConnection))
                root.Add(new XAttribute("flexCheckConnection", FlexCheckConnection));

            var tranformElem = LDDModder.Serialization.XSerializationHelper.Serialize(Transform);
            foreach (var transformAttr in tranformElem.Attributes())
                root.Add(transformAttr);

            //Collisions
            if (Collisions.Count > 0)
            {
                var collElem = new XElement("Collision");
                root.Add(collElem);
                collElem.Add(Collision.Serialize(Collisions).ToArray());
            }

            //Connectivity
            if (Connections.Count > 0)
            {
                var connElem = new XElement("Connectivity");
                root.Add(connElem);
                connElem.Add(Connectivity.Serialize(Connections).ToArray());
            }

            //Other simple elements
            if (PhysicsAttributes != null)
                root.Add(LDDModder.Serialization.XSerializationHelper.Serialize(PhysicsAttributes, "PhysicsAttributes"));

            if (Bounding != null)
                root.Add(new XElement("Bounding", LDDModder.Serialization.XSerializationHelper.Serialize(Bounding, "AABB")));

            return root;
        }

        protected override void DeserializeFromXElement(XElement element)
        {
            Id = element.GetIntAttribute("boneId");

            if (element.Attribute("flexCheckConnection") != null)
                FlexCheckConnection = element.Attribute("flexCheckConnection").Value;

            var collisionsElem = element.Element("Collision");
            if (collisionsElem != null)
                _Collisions.AddRange(Collision.Deserialize(collisionsElem.Elements()));

            var conectivityElem = element.Element("Connectivity");
            if (conectivityElem != null)
                _Connections.AddRange(Connectivity.Deserialize(conectivityElem.Elements()));

            var boundingElem = element.Element("Bounding");
            if (boundingElem != null && boundingElem.HasElements)
                Bounding = LDDModder.Serialization.XSerializationHelper.DefaultDeserialize<BoundingBox>(boundingElem.Elements().Single());//.First() or .Single() ??

            var physAttrElem = element.Element("PhysicsAttributes");
            if (physAttrElem != null)
                PhysicsAttributes = LDDModder.Serialization.XSerializationHelper.DefaultDeserialize<PhysicsAttributes>(physAttrElem);

            Transform = LDDModder.Serialization.XSerializationHelper.DefaultDeserialize<Transform>(element);
            
        }
    }
}
