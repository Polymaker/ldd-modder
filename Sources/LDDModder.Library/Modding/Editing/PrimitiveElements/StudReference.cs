using LDDModder.LDD.Meshes;
using LDDModder.LDD.Primitives.Connectors;
using LDDModder.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace LDDModder.Modding.Editing
{
    public class StudReference : PartElement
    {
        public const string NODE_NAME = "StudRef";


        public string ConnectionID { get; set; }

        public int ConnectionIndex { get; set; } = -1;

        [XmlIgnore]
        public PartConnection Connection => GetLinkedConnection();

        [XmlAttribute]
        public int FieldIndex { get; set; }

        [XmlAttribute]
        public int Value1 { get; set; }

        [XmlAttribute]
        public int Value2 { get; set; }

        [XmlIgnore]
        public Custom2DFieldConnector Connector => GetCustom2DField();

        [XmlIgnore]
        public Custom2DFieldNode FieldNode => Connector?.GetNode(FieldIndex);

        public StudReference()
        {
            ConnectionIndex = -1;
            FieldIndex = -1;
        }

        //public StudReference(int fieldIndex, int value1, int value2)
        //{
        //    FieldIndex = fieldIndex;
        //    Value1 = value1;
        //    Value2 = value2;
        //}

        public StudReference(int connectionIndex, int fieldIndex, int value1, int value2)
        {
            ConnectionIndex = connectionIndex;
            FieldIndex = fieldIndex;
            Value1 = value1;
            Value2 = value2;
        }

        public StudReference(string connectionID, int fieldIndex, int value1, int value2)
        {
            ConnectionID = connectionID;
            FieldIndex = fieldIndex;
            Value1 = value1;
            Value2 = value2;
        }

        public StudReference(Custom2DFieldReference fieldReference)
        {
            ConnectionIndex = fieldReference.ConnectorIndex;
            FieldIndex = fieldReference.FieldIndices[0].Index;
            Value1 = fieldReference.FieldIndices[0].Value2;
            Value2 = fieldReference.FieldIndices[0].Value4;
        }

        public PartConnection GetLinkedConnection()
        {
            if (Project != null)
                return Project.Connections.FirstOrDefault(x => x.ID == ConnectionID);
            return null;
        }

        public Custom2DFieldConnector GetCustom2DField()
        {
            return GetLinkedConnection()?.GetConnector<Custom2DFieldConnector>();
        }

        protected internal override void LoadFromXml(XElement element)
        {
            base.LoadFromXml(element);

            ConnectionID = element.ReadAttribute(nameof(ConnectionID), string.Empty);
            FieldIndex = element.ReadAttribute(nameof(FieldIndex), -1);
            Value1 = element.ReadAttribute(nameof(Value1), 0);
            Value2 = element.ReadAttribute(nameof(Value2), 0);

            //if (element.TryGetIntAttribute(nameof(FieldIndex), out int v1))
            //    FieldIndex = v1;
            //if (element.TryGetIntAttribute(nameof(Value1), out int v2))
            //    Value1 = v2;
            //if (element.TryGetIntAttribute(nameof(Value2), out int v3))
            //    Value2 = v3;
        }

        public override XElement SerializeToXml()
        {
            var elem = SerializeToXmlBase(NODE_NAME);
            if (!string.IsNullOrEmpty(ConnectionID))
                elem.WriteAttribute(nameof(ConnectionID), ConnectionID);
            elem.AddNumberAttribute(nameof(FieldIndex), FieldIndex);
            elem.AddNumberAttribute(nameof(Value1), Value1);
            elem.AddNumberAttribute(nameof(Value2), Value2);

            return elem;
        }

        public XNode[] SerializeToXml2()
        {
            var studElem = SerializeToXml();

            if (FieldNode == null)
                return new XNode[] { studElem };

            var info = new XComment($"Stud position X: {FieldNode.X} Y: {FieldNode.Y}");
            return new XNode[] { info, studElem };
        }

        public static StudReference FromXml(XElement element)
        {
            var stud = new StudReference();
            stud.LoadFromXml(element);
            return stud;
        }

        public override List<ValidationMessage> ValidateElement()
        {
            var messages = base.ValidateElement();

            void AddMessage(string code, ValidationLevel level, params object[] args)
            {
                messages.Add(new ValidationMessage(this, code, level)
                {
                    MessageArguments = args
                });
            }

            if (Connector == null)
                AddMessage("STUD_CONNECTION_NOT_DEFINED", ValidationLevel.Error);

            if (Connector != null && FieldNode == null)
            {
                if (FieldIndex < 0)
                    AddMessage("STUD_CONNECTION_FIELD_NOT_DEFINED", ValidationLevel.Error);
                else
                    AddMessage("STUD_CONNECTION_FIELD_INVALID", ValidationLevel.Error);
            }

            return messages;
        }
    }
}
