using LDDModder.LDD.Primitives.Collisions;
using LDDModder.LDD.Primitives.Connectors;
using LDDModder.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace LDDModder.LDD.Primitives
{
    public class FlexBone : IXmlObject
    {
        public int ID { get; set; }
        public Transform Transform { get; set; }
        public PhysicsAttributes PhysicsAttributes { get; set; }
        public BoundingBox Bounding { get; set; }

        /// <summary>
        /// Item 1: Number of Connectivity elements (excluding types 99900X)
        /// Item 2: The previous bone ID (the one it connects to)
        /// Item 3: No clue
        /// </summary>
        public Tuple<int,int,int> ConnectionCheck { get; set; }

        public List<Connector> Connectors { get; }

        public List<Collision> Collisions { get; }

        public FlexBone()
        {
            Connectors = new List<Connectors.Connector>();
            Collisions = new List<Collisions.Collision>();
            Transform = new Transform();
        }

        public XElement SerializeToXml()
        {
            var boneElem = new XElement("Bone", new XAttribute("boneId", ID));

            if (ConnectionCheck != null)
                boneElem.Add(new XAttribute("flexCheckConnection", $"{ConnectionCheck.Item1},{ConnectionCheck.Item2},{ConnectionCheck.Item3}"));

            boneElem.Add(Transform.ToXmlAttributes());

            if (Collisions.Any())
            {
                var collisionElem = boneElem.AddElement("Collision");
                foreach (var col in Collisions)
                    collisionElem.Add(col.SerializeToXml());
            }

            if (Connectors.Any())
            {
                var connectivityElem = boneElem.AddElement("Connectivity");
                foreach (var con in Connectors)
                    connectivityElem.Add(con.SerializeToXml());
            }

            if (PhysicsAttributes != null)
                boneElem.Add(PhysicsAttributes.SerializeToXml());

            if (Bounding != null)
            {
                var boundingElem = boneElem.AddElement("Bounding");
                boundingElem.Add(XmlHelper.DefaultSerialize(Bounding, "AABB"));
            }

            return boneElem;
        }

        public void LoadFromXml(XElement element)
        {
            ID = element.ReadAttribute("boneId", 0);
            Transform = Transform.FromElementAttributes(element);

            if (element.HasElement("PhysicsAttributes", out XElement physicsElem))
                PhysicsAttributes = XmlHelper.DefaultDeserialize<PhysicsAttributes>(physicsElem);

            if (element.HasElement("Bounding", out XElement boundingElem))
                Bounding = XmlHelper.DefaultDeserialize<BoundingBox>(boundingElem.Element("AABB"));

            if (element.HasAttribute("flexCheckConnection", out XAttribute checkAttr))
            {
                var checkValues = checkAttr.Value.Trim().Split(',');
                if (checkValues.Length == 3)
                    ConnectionCheck = new Tuple<int, int, int>(int.Parse(checkValues[0]), int.Parse(checkValues[1]), int.Parse(checkValues[2]));
            }

            if (element.HasElement("Collision", out XElement collisionElem))
            {
                foreach (var colElem in collisionElem.Elements())
                    Collisions.Add(Collision.DeserializeCollision(colElem));
            }

            if (element.HasElement("Connectivity", out XElement connectorElem))
            {
                foreach (var conElem in connectorElem.Elements())
                    Connectors.Add(Connector.DeserializeConnector(conElem));
            }
        }
    }
}
