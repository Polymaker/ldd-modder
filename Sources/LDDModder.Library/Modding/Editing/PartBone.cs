using LDDModder.LDD.Primitives;
using LDDModder.Serialization;
using LDDModder.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace LDDModder.Modding.Editing
{
    public class PartBone : PartElement
    {
        public const string NODE_NAME = "Bone";

        public int BoneID { get; set; }

        public ItemTransform Transform { get; set; }

        public PhysicsAttributes PhysicsAttributes { get; set; }

        public BoundingBox Bounding { get; set; }

        public ElementCollection<PartConnection> Connections { get; }

        public ElementCollection<PartCollision> Collisions { get; }

        public PartBone()
        {
            Connections = new ElementCollection<PartConnection>(this);
            Collisions = new ElementCollection<PartCollision>(this);
            Transform = new ItemTransform();
        }

        protected override IEnumerable<PartElement> GetAllChilds()
        {
            return Connections.AsEnumerable<PartElement>().Concat(Collisions);
        }

        public static PartBone FromLDD(FlexBone flexBone)
        {
            var bone = new PartBone()
            {
                BoneID = flexBone.ID,
                Transform = ItemTransform.FromLDD(flexBone.Transform)
            };

            foreach (var collision in flexBone.Collisions)
                bone.Collisions.Add(PartCollision.FromLDD(collision));

            foreach (var lddConn in flexBone.Connectors)
            {
                var partConn = PartConnection.FromLDD(lddConn);
                if (partConn.ConnectorType == LDD.Primitives.Connectors.ConnectorType.Custom2DField)
                {
                    int connIdx = flexBone.Connectors.IndexOf(lddConn);
                    partConn.ID = StringUtils.GenerateUUID($"Bone{flexBone.ID}_{connIdx}", 8);
                }
                bone.Connections.Add(partConn);
            }

            return bone;
        }

        public override XElement SerializeToXml()
        {
            var elem = SerializeToXmlBase(NODE_NAME);
            elem.Add(XmlHelper.ToXml(() => BoneID));
            elem.Add(Transform.SerializeToXml());

            if (PhysicsAttributes != null || Bounding != null)
            {
                var propsElem = elem.AddElement("Properties");
                if (PhysicsAttributes != null)
                    propsElem.Add(PhysicsAttributes.SerializeToXml());
                if (Bounding != null)
                    propsElem.Add(XmlHelper.DefaultSerialize(Bounding, "Bounding"));
            }

            if (Connections.Any())
            {
                var connectionsElem = elem.AddElement("Connections");
                foreach (var conn in Connections)
                    connectionsElem.Add(conn.SerializeToXml());
            }

            if (Collisions.Any())
            {
                var collisionsElem = elem.AddElement("Collisions");
                foreach (var col in Collisions)
                    collisionsElem.Add(col.SerializeToXml());
            }

            return elem;
        }

        protected internal override void LoadFromXml(XElement element)
        {
            base.LoadFromXml(element);
            BoneID = element.ReadAttribute<int>("BoneID");

            if (element.HasElement("Transform", out XElement transElem))
                Transform = ItemTransform.FromXml(transElem);

            if (element.HasElement("Properties", out XElement propsElem))
            {
                if (propsElem.HasElement("PhysicsAttributes", out XElement pA))
                {
                    PhysicsAttributes = new PhysicsAttributes();
                    PhysicsAttributes.LoadFromXml(pA);
                }

                if (propsElem.HasElement("Bounding", out XElement gb))
                    Bounding = XmlHelper.DefaultDeserialize<BoundingBox>(gb);
            }

            if (element.HasElement("Connections", out XElement connectionsElem))
            {
                foreach (var connElem in connectionsElem.Elements(PartConnection.NODE_NAME))
                    Connections.Add(PartConnection.FromXml(connElem));
            }

            if (element.HasElement("Collisions", out XElement collisionsElem))
            {
                foreach (var connElem in collisionsElem.Elements(PartCollision.NODE_NAME))
                    Collisions.Add(PartCollision.FromXml(connElem));
            }
        }
    }
}
