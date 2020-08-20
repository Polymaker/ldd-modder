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
        private int _TargetBoneID;
        private string _SourceConnectionID;
        private string _TargetConnectionID;
        private ItemTransform _Transform;
        private PhysicsAttributes _PhysicsAttributes;
        private BoundingBox _Bounding;

        public int BoneID
        {
            get => _BoneID;
            set => SetPropertyValue(ref _BoneID, value);
        }

        public int SourceConnectionIndex { get; set; } = -1;

        public string SourceConnectionID
        {
            get => _SourceConnectionID;
            set => SetPropertyValue(ref _SourceConnectionID, value);
        }

        public int TargetBoneID
        {
            get => _TargetBoneID;
            set => SetPropertyValue(ref _TargetBoneID, value);
        }

        public int TargetConnectionIndex { get; set; } = -1;

        public string TargetConnectionID
        {
            get => _TargetConnectionID;
            set => SetPropertyValue(ref _TargetConnectionID, value);
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
            TargetBoneID = -1;
            TargetConnectionIndex = -1;
        }

        public PartBone(int boneID)
        {
            Connections = new ElementCollection<PartConnection>(this);
            Collisions = new ElementCollection<PartCollision>(this);
            Transform = new ItemTransform();
            _BoneID = boneID;
            _TargetBoneID = -1;
            TargetConnectionIndex = -1;
            InternalSetName($"Bone{boneID}");
        }

        protected override void OnPropertyChanged(ElementValueChangedEventArgs args)
        {
            base.OnPropertyChanged(args);
            if (args.PropertyName == nameof(Transform))
                TranformChanged?.Invoke(this, EventArgs.Empty);
        }

        #region Convertion from/to LDD

        internal void LoadFromLDD(FlexBone flexBone)
        {
            BoneID = flexBone.ID;
            Transform = ItemTransform.FromLDD(flexBone.Transform);
            Bounding = flexBone.Bounding;
            PhysicsAttributes = flexBone.PhysicsAttributes;

            if (flexBone.ConnectionCheck != null)
            {
                SourceConnectionIndex = flexBone.ConnectionCheck.Item1;
                TargetBoneID = flexBone.ConnectionCheck.Item2;
                TargetConnectionIndex = flexBone.ConnectionCheck.Item3;
            }
            else
            {
                SourceConnectionIndex = -1;
                TargetBoneID = -1;
                TargetConnectionIndex = -1;
            }

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

        public FlexBone GenerateLDD()
        {
            var bone = new FlexBone()
            {
                ID = BoneID,
                Bounding = Bounding,
                PhysicsAttributes = PhysicsAttributes,
                Transform = Transform.ToLDD()
            };

            bone.Collisions.AddRange(Collisions.Select(x => x.GenerateLDD()));
            bone.Connectors.AddRange(Connections.Select(x => x.GenerateLDD()));

            if (BoneID > 0)
            {
                bone.ConnectionCheck = new Tuple<int, int, int>(
                    SourceConnectionIndex, 
                    TargetBoneID,
                    TargetConnectionIndex);
            }

            return bone;
        }

        #endregion

        #region Convertion from/to XML

        public override XElement SerializeToXml()
        {
            var elem = SerializeToXmlBase(NODE_NAME);

            elem.Add(new XAttribute(nameof(BoneID), BoneID));
            
            if (TargetBoneID >= 0)
            {
                //if (BoneID > 0)
                //{
                //    elem.Add(new XComment($"flexCheckConnection=\"{SourceConnectionIndex},{TargetBoneID},{TargetConnectionIndex}\""));
                //}
                
                elem.Add(new XElement("ConnectsTo",
                    new XAttribute("SourceConnectionID", SourceConnectionID ?? string.Empty),
                    new XAttribute("TargetBoneID", TargetBoneID),
                    new XAttribute("TargetConnectionID", TargetConnectionID ?? string.Empty)));
            }

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

            if (element.HasElement("ConnectsTo", out XElement connInfo))
            {
                SourceConnectionID = connInfo.ReadAttribute("SourceConnectionID", string.Empty);
                TargetBoneID = connInfo.ReadAttribute("TargetBoneID", -1);
                TargetConnectionID = connInfo.ReadAttribute("TargetConnectionID", string.Empty);
            }

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

        #endregion

        public PartBone GetLinkedBone()
        {
            return Project.Bones.FirstOrDefault(x => x.TargetBoneID == BoneID);
        }

        public IEnumerable<PartBone> GetLinkedBones()
        {
            return Project.Bones.Where(x => x.TargetBoneID == BoneID);
        }

        public PartBone GetTargetBone()
        {
            return Project.Bones.FirstOrDefault(x => x.BoneID == TargetBoneID);
        }

        public override List<ValidationMessage> ValidateElement()
        {
            var validationMessages = new List<ValidationMessage>();

            void AddMessage(string code, ValidationLevel level, params object[] args)
            {
                validationMessages.Add(new ValidationMessage(this, code, level, args));
            }

            if (Bounding == null || Bounding.IsEmpty)
                AddMessage("BONE_NO_BOUNDING", ValidationLevel.Warning);

            if (BoneID > 1)
            {
                var targetBone = GetTargetBone();
                if (targetBone == null)
                    AddMessage("BONE_NOT_LINKED", ValidationLevel.Error);
                else
                {
                    var myConn = Connections.FirstOrDefault(x => x.ID == SourceConnectionID);
                    var targetConn = targetBone.Connections.FirstOrDefault(x => x.ID == TargetConnectionID);
                    if (myConn == null || targetConn == null)
                        AddMessage("BONE_CONNECTION_NOT_SET", ValidationLevel.Error);
                }
            }

            if (GetLinkedBones().Count() > 1)
                AddMessage("BONE_MULTIPLE_LINKS", ValidationLevel.Warning);

            return validationMessages;
        }
    }
}
