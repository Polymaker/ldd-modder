using LDDModder.LDD.Meshes;
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
        public StudConnection Connection { get; set; }
        public int FieldIndex { get; set; }

        public int Value1 { get; set; }

        public int Value2 { get; set; }

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

        public StudReference(StudConnection connection, int studIndex, int value1, int value2)
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

        public XElement SerializeToXml()
        {
            return new XElement("Stud",
                    new XAttribute("ConnectionID", RefID),
                    new XAttribute("FieldIndex", FieldIndex),
                    new XAttribute("Value1", Value1),
                    new XAttribute("Value2", Value2)
                    );
        }
    }
}
