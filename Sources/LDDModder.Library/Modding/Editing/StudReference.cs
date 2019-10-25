using LDDModder.LDD.Meshes;
using LDDModder.LDD.Primitives.Connectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LDDModder.Modding.Editing
{
    public class StudReference
    {
        public string RefID { get; set; }
        public int ConnectorIndex { get; set; } = -1;

        public PartConnector<Custom2DFieldConnector> Connection { get; set; }

        public int FieldIndex { get; set; }

        public int Value1 { get; set; }

        public int Value2 { get; set; }

        public Custom2DFieldConnector Connector => Connection?.Connector;

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

        public StudReference(PartConnector<Custom2DFieldConnector> connection, int studIndex, int value1, int value2)
        {
            RefID = connection.RefID;
            Connection = connection;
            FieldIndex = studIndex;
            Value1 = value1;
            Value2 = value2;
        }

        public StudReference(Custom2DFieldReference fieldReference)
        {
            ConnectorIndex = fieldReference.ConnectorIndex;
            FieldIndex = fieldReference.FieldIndices[0].Index;
            Value1 = fieldReference.FieldIndices[0].Value2;
            Value2 = fieldReference.FieldIndices[0].Value4;
        }

        public XElement SerializeToXml(string elementName = "Stud")
        {
            return new XElement(elementName,
                    new XAttribute("ConnectionID", RefID),
                    new XAttribute("FieldIndex", FieldIndex),
                    new XAttribute("Value1", Value1),
                    new XAttribute("Value2", Value2)
                    );
        }

        public static StudReference FromXml(XElement element)
        {
            var stud = new StudReference
            {
                RefID = element.Attribute("ConnectionID")?.Value
            };
            if (element.TryGetIntAttribute("FieldIndex", out int v1))
                stud.FieldIndex = v1;
            if (element.TryGetIntAttribute("Value1", out int v2))
                stud.Value1 = v1;
            if (element.TryGetIntAttribute("Value2", out int v3))
                stud.Value2 = v1;
            return stud;
        }
    }
}
