using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LDDModder.BrickEditor.Editing
{
    public class StudReference : PartNode
    {
        public ConnectionNode ConnectorNode { get; set; }

        public int StudIndex { get; set; }

        public int Value1 { get; set; }

        public int Value2 { get; set; }

        public LDD.Primitives.Connectors.Custom2DFieldConnector Connector => 
            ConnectorNode?.Connection as LDD.Primitives.Connectors.Custom2DFieldConnector;

        public override XElement SerializeToXml()
        {
            var elem = new XElement("StudRef");
            //elem.Add(new XAttribute("ID", ID));
            elem.Add(new XAttribute("ConnectorID", ConnectorNode.ID));
            elem.Add(new XAttribute("ArrayIndex", StudIndex));
            elem.Add(new XAttribute("Value1", Value1));
            elem.Add(new XAttribute("Value2", Value2));
            return elem;
        }
    }
}
