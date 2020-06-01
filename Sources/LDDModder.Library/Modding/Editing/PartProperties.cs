using LDDModder.LDD.Data;
using LDDModder.LDD.Primitives;
using LDDModder.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LDDModder.Modding.Editing
{
    public class PartProperties : PartElement
    {
        private int _PartID;
        private string _PartDescription;
        private List<int> _Aliases;
        private Platform _Platform;
        private MainGroup _MainGroup;
        private PhysicsAttributes _PhysicsAttributes;
        private BoundingBox _Bounding;
        private BoundingBox _GeometryBounding;
        private ItemTransform _DefaultOrientation;
        private Camera _DefaultCamera;
        private VersionInfo _PrimitiveFileVersion;
        private int _PartVersion;
        private bool _Decorated;
        private bool _Flexible;

        public int PartID
        {
            get => _PartID;
            set => SetPropertyValue(ref _PartID, value);
        }

        public string Description
        {
            get => _PartDescription;
            set => SetPropertyValue(ref _PartDescription, value);
        }

        public List<int> Aliases
        {
            get => _Aliases;
            set => SetPropertyValue(ref _Aliases, value);
        }

        public Platform Platform
        {
            get => _Platform;
            set => SetPropertyValue(ref _Platform, value);
        }

        public MainGroup MainGroup
        {
            get => _MainGroup;
            set => SetPropertyValue(ref _MainGroup, value);
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

        public BoundingBox GeometryBounding
        {
            get => _GeometryBounding;
            set => SetPropertyValue(ref _GeometryBounding, value);
        }

        public ItemTransform DefaultOrientation
        {
            get => _DefaultOrientation;
            set => SetPropertyValue(ref _DefaultOrientation, value);
        }

        public Camera DefaultCamera
        {
            get => _DefaultCamera;
            set => SetPropertyValue(ref _DefaultCamera, value);
        }

        public VersionInfo PrimitiveFileVersion
        {
            get => _PrimitiveFileVersion;
            set => SetPropertyValue(ref _PrimitiveFileVersion, value);
        }

        public int PartVersion
        {
            get => _PartVersion;
            set => SetPropertyValue(ref _PartVersion, value);
        }

        public bool Decorated
        {
            get => _Decorated;
            set => SetPropertyValue(ref _Decorated, value);
        }

        public bool Flexible
        {
            get => _Flexible;
            set => SetPropertyValue(ref _Flexible, value);
        }

        public PartProperties(PartProject project)
        {
            _Project = project;
            Platform = new Platform(0, "None");
            MainGroup = new MainGroup(0, "None");
            PrimitiveFileVersion = new VersionInfo(1, 0);
            Aliases = new List<int>();
            PartVersion = 1;
            Bounding = new BoundingBox();
            GeometryBounding = new BoundingBox();
            PhysicsAttributes = new PhysicsAttributes();
        }

        public override XElement SerializeToXml()
        {
            var propsElem = SerializeToXmlBase("Properties");
            propsElem.RemoveAttributes();
            propsElem.Add(new XElement("PartID", PartID));

            if (Aliases.Any())
                propsElem.Add(new XElement("Aliases", string.Join(";", Aliases)));

            propsElem.Add(new XElement(nameof(Description), Description));

            propsElem.Add(new XElement(nameof(PartVersion), PartVersion));

            if (PrimitiveFileVersion != null)
                propsElem.Add(PrimitiveFileVersion.ToXmlElement("PrimitiveVersion"));
            else
                PrimitiveFileVersion = new VersionInfo(1, 0);

            propsElem.Add(new XElement(nameof(Flexible), Flexible));

            propsElem.Add(new XElement(nameof(Decorated), Decorated));

            if (Platform != null)
                propsElem.AddElement(nameof(Platform), new XAttribute("ID", Platform.ID), new XAttribute("Name", Platform.Name));

            if (MainGroup != null)
                propsElem.AddElement(nameof(MainGroup), new XAttribute("ID", MainGroup.ID), new XAttribute("Name", MainGroup.Name));

            if (PhysicsAttributes != null && !PhysicsAttributes.IsEmpty)
                propsElem.Add(PhysicsAttributes.SerializeToXml());

            if (Bounding != null)
                propsElem.Add(XmlHelper.DefaultSerialize(Bounding, nameof(Bounding)));

            if (GeometryBounding != null)
                propsElem.Add(XmlHelper.DefaultSerialize(GeometryBounding, nameof(GeometryBounding)));

            if (DefaultOrientation != null)
                propsElem.Add(DefaultOrientation.SerializeToXml(nameof(DefaultOrientation)));

            if (DefaultCamera != null)
                propsElem.Add(XmlHelper.DefaultSerialize(DefaultCamera, nameof(DefaultCamera)));

            return propsElem;
        }

        protected internal override void LoadFromXml(XElement element)
        {
            base.LoadFromXml(element);
            PartID = int.Parse(element.Element("PartID")?.Value);

            if (element.HasElement("Aliases", out XElement aliasElem))
            {
                foreach (string partAlias in aliasElem.Value.Split(';'))
                {
                    if (int.TryParse(partAlias, out int aliasID))
                        Aliases.Add(aliasID);
                }
            }

            Description = element.ReadElement(nameof(Description), string.Empty);
            PartVersion = element.ReadElement(nameof(PartVersion), 1);

            Decorated = element.ReadElement(nameof(Decorated), false);
            Flexible = element.ReadElement(nameof(Flexible), false);

            if (element.HasElement(nameof(PhysicsAttributes), out XElement pA))
            {
                PhysicsAttributes = new PhysicsAttributes();
                PhysicsAttributes.LoadFromXml(pA);
            }
            else
                PhysicsAttributes = new PhysicsAttributes();

            if (element.HasElement(nameof(GeometryBounding), out XElement gb))
                GeometryBounding = XmlHelper.DefaultDeserialize<BoundingBox>(gb);
            else
                GeometryBounding = new BoundingBox();

            if (element.HasElement(nameof(Bounding), out XElement bb))
                Bounding = XmlHelper.DefaultDeserialize<BoundingBox>(bb);
            else
                Bounding = new BoundingBox();

            if (element.HasElement(nameof(DefaultOrientation), out XElement defori))
                DefaultOrientation = ItemTransform.FromXml(defori);

            if (element.HasElement(nameof(DefaultCamera), out XElement camElem))
                DefaultCamera = XmlHelper.DefaultDeserialize<Camera>(camElem);

            if (element.HasElement("Platform", out XElement platformElem))
            {
                Platform = new Platform(
                    platformElem.ReadAttribute("ID", 0),
                    platformElem.ReadAttribute("Name", string.Empty)
                );
            }

            if (element.HasElement("MainGroup", out XElement groupElem))
            {
                MainGroup = new MainGroup(
                    groupElem.ReadAttribute("ID", 0),
                    groupElem.ReadAttribute("Name", string.Empty)
                );
            }

            if (element.HasElement("PrimitiveVersion", out XElement versionElem))
            {
                PrimitiveFileVersion = VersionInfo.FromXmlElement(versionElem);
            }
        }

        public override List<ValidationMessage> ValidateElement()
        {
            var messages = new List<ValidationMessage>();

            void AddMessage(string code, ValidationLevel level, params object[] args)
            {
                messages.Add(new ValidationMessage(this, code, level)
                {
                    MessageArguments = args
                });
            }

            if (string.IsNullOrEmpty(Description))
                AddMessage("PART_EMPTY_DESCRIPTION", ValidationLevel.Error);

            if (PartID <= 0)
                AddMessage("PART_EMPTY_PART_ID", ValidationLevel.Error);

            if (string.IsNullOrEmpty(Platform?.Name))
                AddMessage("PART_NO_PLATFORM", ValidationLevel.Error);

            if (string.IsNullOrEmpty(MainGroup?.Name))
                AddMessage("PART_NO_MAINGROUP", ValidationLevel.Error);

            if (Bounding is null || Bounding.IsEmpty)
                AddMessage("PART_NO_BOUNDING", ValidationLevel.Error);

            if (GeometryBounding is null || GeometryBounding.IsEmpty)
                AddMessage("PART_NO_GEOMETRYBOUNDING", ValidationLevel.Error);

            return messages;
        }
    }
}
