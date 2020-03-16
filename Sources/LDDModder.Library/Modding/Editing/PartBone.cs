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
    public class PartBone : PartElement, IPhysicalElement
    {
        public const string NODE_NAME = "Bone";

        private int _BoneID;
        private ItemTransform _Transform;
        private PhysicsAttributes _PhysicsAttributes;
        private BoundingBox _Bounding;

        public int BoneID
        {
            get => _BoneID;
            set => SetPropertyValue(ref _BoneID, value);
        }

        public ItemTransform Transform
        {
            get => _Transform;
            set => SetPropertyValue(ref _Transform, value);
        }

        public PhysicsAttributes PhysicsAttributes
        {
            get => _PhysicsAttributes;
            set => SetPropertyValue(ref _PhysicsAttributes, value);
        }

        public BoundingBox Bounding
        {
            get => _Bounding;
            set => SetPropertyValue(ref _Bounding, value);
        }

        public ElementCollection<PartConnection> Connections { get; }

        public ElementCollection<PartCollision> Collisions { get; }

        public event EventHandler TranformChanged;

        public PartBone()
        {
            Connections = new ElementCollection<PartConnection>(this);
            Collisions = new ElementCollection<PartCollision>(this);
            Transform = new ItemTransform();
        }

        public PartBone(int boneID)
        {
            Connections = new ElementCollection<PartConnection>(this);
            Collisions = new ElementCollection<PartCollision>(this);
            Transform = new ItemTransform();
            _BoneID = boneID;
            InternalSetName($"Bone{boneID}");
        }

        protected override void OnPropertyChanged(ElementValueChangedEventArgs args)
        {
            base.OnPropertyChanged(args);
            if (args.PropertyName == nameof(Transform))
                TranformChanged?.Invoke(this, EventArgs.Empty);
        }

        internal void LoadFromLDD(FlexBone flexBone)
        {
            BoneID = flexBone.ID;
            Transform = ItemTransform.FromLDD(flexBone.Transform);
            Bounding = flexBone.Bounding;
            PhysicsAttributes = flexBone.PhysicsAttributes;

            int elementIndex = 0;

            foreach (var collision in flexBone.Collisions)
            {
                var collisionElem = PartCollision.FromLDD(collision);
                collisionElem.ID = StringUtils.GenerateUUID($"Part{Project.PartID}_Bone{BoneID}_Collision{elementIndex++}", 8);
                Collisions.Add(collisionElem);
            }

            elementIndex = 0;
            foreach (var lddConn in flexBone.Connectors)
            {
                var connectionElem = PartConnection.FromLDD(lddConn);
                connectionElem.ID = StringUtils.GenerateUUID($"Part{Project.PartID}_Bone{BoneID}_Connection{elementIndex++}", 8);
                Connections.Add(connectionElem);
            }
        }

        public override XElement SerializeToXml()
        {
            var elem = SerializeToXmlBase(NODE_NAME);

            elem.Add(new XAttribute(nameof(BoneID), BoneID));
            elem.Add(Transform.SerializeToXml());

            if (PhysicsAttributes != null || Bounding != null)
            {
                var propsElem = elem.AddElement("Properties");
                if (PhysicsAttributes != null)
                    propsElem.Add(PhysicsAttributes.SerializeToXml(nameof(PhysicsAttributes)));
                if (Bounding != null)
                    propsElem.Add(XmlHelper.DefaultSerialize(Bounding, nameof(Bounding)));
            }

            if (Connections.Any())
            {
                var connectionsElem = elem.AddElement(nameof(Connections));
                foreach (var conn in Connections)
                    connectionsElem.Add(conn.SerializeToXml());
            }

            if (Collisions.Any())
            {
                var collisionsElem = elem.AddElement(nameof(Collisions));
                foreach (var col in Collisions)
                    collisionsElem.Add(col.SerializeToXml());
            }

            return elem;
        }

        public static PartBone FromXml(XElement element)
        {
            var bone = new PartBone();
            bone.LoadFromXml(element);
            return bone;
        }

        protected internal override void LoadFromXml(XElement element)
        {
            base.LoadFromXml(element);
            BoneID = element.ReadAttribute<int>(nameof(BoneID));

            if (element.HasElement(nameof(Transform), out XElement transElem))
                Transform = ItemTransform.FromXml(transElem);

            if (element.HasElement("Properties", out XElement propsElem))
            {
                if (propsElem.HasElement(nameof(PhysicsAttributes), out XElement pA))
                {
                    PhysicsAttributes = new PhysicsAttributes();
                    PhysicsAttributes.LoadFromXml(pA);
                }

                if (propsElem.HasElement(nameof(Bounding), out XElement gb))
                    Bounding = XmlHelper.DefaultDeserialize<BoundingBox>(gb);
            }

            if (element.HasElement(nameof(Connections), out XElement connectionsElem))
            {
                foreach (var connElem in connectionsElem.Elements(PartConnection.NODE_NAME))
                    Connections.Add(PartConnection.FromXml(connElem));
            }

            if (element.HasElement(nameof(Collisions), out XElement collisionsElem))
            {
                foreach (var connElem in collisionsElem.Elements(PartCollision.NODE_NAME))
                    Collisions.Add(PartCollision.FromXml(connElem));
            }
        }
    }
}
