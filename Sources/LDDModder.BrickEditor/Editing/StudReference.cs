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

        public LDD.Primitives.Connectors.Custom2DFieldConnector Connector => 
            ConnectorNode?.Connection as LDD.Primitives.Connectors.Custom2DFieldConnector;



        public override XElement SerializeToXml()
        {
            var elem = new XElement("StudRef");
            elem.Add(new XAttribute("ConnectorID", ConnectorNode.ID));
            elem.Add(new XAttribute("ArrayIndex", StudIndex));
            return elem;
        }
    }
}
