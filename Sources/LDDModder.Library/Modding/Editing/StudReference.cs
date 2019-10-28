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
    public class StudReference : PartComponent
    {
        public const string NODE_NAME = "StudRef";

        //public string RefID { get; set; }

        [XmlIgnore]
        public string ConnectionID { get; set; }

        [XmlIgnore]
        public int ConnectorIndex { get; set; } = -1;

        [XmlIgnore]
        public PartConnection Connection => (Parent as PartCullingModel)?.GetLinkedConnection();

        [XmlAttribute]
        public int FieldIndex { get; set; }

        [XmlAttribute]
        public int Value1 { get; set; }

        [XmlAttribute]
        public int Value2 { get; set; }

        [XmlIgnore]
        public Custom2DFieldConnector Connector => Connection?.GetConnector< Custom2DFieldConnector>();

        [XmlIgnore]
        public Custom2DFieldConnector.FieldNode FieldNode => Connector?.GetNode(FieldIndex);

        public StudReference()
        {
        }

        public StudReference(int connectorIndex, int studIndex, int value1, int value2)
        {
            ConnectorIndex = connectorIndex;
            FieldIndex = studIndex;
            Value1 = value1;
            Value2 = value2;
        }

        public StudReference(string refID, int studIndex, int value1, int value2)
        {
            RefID = refID;
            FieldIndex = studIndex;
            Value1 = value1;
            Value2 = value2;
        }

        //public StudReference(PartConnection/*<Custom2DFieldConnector>*/ connection, int studIndex, int value1, int value2)
        //{
        //    RefID = connection.RefID;
        //    Connection = connection;
        //    FieldIndex = studIndex;
        //    Value1 = value1;
        //    Value2 = value2;
        //}

        public StudReference(Custom2DFieldReference fieldReference)
        {
            ConnectorIndex = fieldReference.ConnectorIndex;
            FieldIndex = fieldReference.FieldIndices[0].Index;
            Value1 = fieldReference.FieldIndices[0].Value2;
            Value2 = fieldReference.FieldIndices[0].Value4;
        }

        protected internal override void LoadFromXml(XElement element)
        {
            base.LoadFromXml(element);
            ConnectionID = element.ReadAttribute("ConnectionID", "");

            if (element.TryGetIntAttribute("FieldIndex", out int v1))
                FieldIndex = v1;
            if (element.TryGetIntAttribute("Value1", out int v2))
                Value1 = v2;
            if (element.TryGetIntAttribute("Value2", out int v3))
                Value2 = v3;
        }

        public override XElement SerializeToXml()
        {
            var elem = SerializeToXmlBase(NODE_NAME);
            elem.Add(XmlHelper.ToXml(() => ConnectionID));
            //elem.Add(XmlHelper.ToXml(() => ConnectorIndex));
            elem.Add(XmlHelper.ToXml(() => FieldIndex));
            elem.Add(XmlHelper.ToXml(() => Value1));
            elem.Add(XmlHelper.ToXml(() => Value2));

            return elem;
        }

        public static StudReference FromXml(XElement element)
        {
            var stud = new StudReference();
            stud.LoadFromXml(element);
            return stud;
        }
    }
}
