using LDDModder.LDD.Data;
using LDDModder.LDD.Primitives;
using LDDModder.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        private string _Authors;
        private string _OriginalPart;
        private string _ChangeLog;

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

        public string Authors
        {
            get => _Authors;
            set => SetPropertyValue(ref _Authors, value);
        }

        public string OriginalPart
        {
            get => _OriginalPart;
            set => SetPropertyValue(ref _OriginalPart, value);
        }

        public string ChangeLog
        {
            get => _ChangeLog;
            set => SetPropertyValue(ref _ChangeLog, value);
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

        private PartProperties()
        {
            Authors = string.Empty;
            ChangeLog = string.Empty;
            Description = string.Empty;
        }

        public PartProperties(PartProject project) : this()
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

        public void Initialize(LDD.Parts.PartWrapper part)
        {
            PartID = part.PartID;
            Description = part.Primitive.Name;
            PartVersion = part.Primitive.PartVersion;
            Decorated = part.IsDecorated;
            Flexible = part.IsFlexible;
            Aliases.AddRange(part.Primitive.Aliases);
            if (Aliases.Contains(PartID))
                Aliases.Remove(PartID);
            Platform = part.Primitive.Platform;
            MainGroup = part.Primitive.MainGroup;
            PhysicsAttributes = part.Primitive.PhysicsAttributes;
            GeometryBounding = part.Primitive.GeometryBounding;
            Bounding = part.Primitive.Bounding;
            if (part.Primitive.DefaultOrientation != null)
                DefaultOrientation = ItemTransform.FromLDD(part.Primitive.DefaultOrientation);

            if (part.Primitive.ExtraElements.Any())
            {

            }

            if (!string.IsNullOrEmpty(part.Primitive.Comments))
                ParseComments(part.Primitive.Comments);
        }

        public void ParseComments(string comments)
        {
            ChangeLog = string.Empty;

            bool IsMatch(string input, string pattern, out string result)
            {
                result = null;
                var match = Regex.Match(input, pattern, RegexOptions.IgnoreCase);
                if (match.Success && match.Groups.Count > 1)
                    result = match.Groups[1].Value;
                return match.Success;
            }

            using (var sr = new StringReader(comments))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    if (IsMatch(line, "(?:created|updated|LDD).+?by\\s?:? (.+)", out string author))
                    {
                        Authors = author;
                    }
                    else if (IsMatch(line, "derived from\\s?:? (.+)", out string derivedFrom))
                    {

                    }
                    else if (IsMatch(line, "orginal author\\s?:? (.+)", out string originalAuthor))
                    {

                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(ChangeLog))
                            ChangeLog += Environment.NewLine;
                        ChangeLog += line;
                    }
                }
            }
        }

        private void UnBindPhysicsAttributes()
        {
            if (PhysicsAttributes != null)
                PhysicsAttributes.PropertyValueChanged -= PhysicsAttributes_PropertyValueChanged;
        }

        private void BindPhysicsAttributes()
        {
            if (PhysicsAttributes != null)
                PhysicsAttributes.PropertyValueChanged += PhysicsAttributes_PropertyValueChanged;
        }

        private void PhysicsAttributes_PropertyValueChanged(object sender, System.ComponentModel.PropertyValueChangedEventArgs e)
        {
            var changeArgs = new ElementValueChangedEventArgs(this, PhysicsAttributes, e.PropertyName, e.OldValue, e.NewValue)
            {
                Index = e.Index
            };
            RaisePropertyValueChanged(changeArgs);
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
                propsElem.Add(XmlHelper.DefaultSerialize(Bounding.Rounded(6), nameof(Bounding)));

            if (GeometryBounding != null)
                propsElem.Add(XmlHelper.DefaultSerialize(GeometryBounding.Rounded(6), nameof(GeometryBounding)));

            if (DefaultOrientation != null)
                propsElem.Add(DefaultOrientation.SerializeToXml(nameof(DefaultOrientation)));

            if (DefaultCamera != null)
                propsElem.Add(XmlHelper.DefaultSerialize(DefaultCamera, nameof(DefaultCamera)));

            if (!string.IsNullOrWhiteSpace(Authors))
                propsElem.Add(new XElement(nameof(Authors), Authors));

            if (!string.IsNullOrWhiteSpace(OriginalPart))
                propsElem.Add(new XElement(nameof(OriginalPart), OriginalPart));

            if (!string.IsNullOrWhiteSpace(ChangeLog))
                propsElem.Add(new XElement(nameof(ChangeLog), ChangeLog));

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

            Authors = element.ReadElement(nameof(Authors), string.Empty);
            ChangeLog = element.ReadElement(nameof(ChangeLog), string.Empty);

            Decorated = element.ReadElement(nameof(Decorated), false);
            Flexible = element.ReadElement(nameof(Flexible), false);

            UnBindPhysicsAttributes();
            if (element.HasElement(nameof(PhysicsAttributes), out XElement pA))
            {
                PhysicsAttributes = new PhysicsAttributes();
                PhysicsAttributes.LoadFromXml(pA);
            }
            else
                PhysicsAttributes = new PhysicsAttributes();
            BindPhysicsAttributes();

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
