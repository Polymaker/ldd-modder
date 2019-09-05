using LDDModder.LDD.Primitives.Connectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LDDModder.BrickEditor.Editing
{
    public class ConnectionNode : PartNode
    {
        public Connector Connection { get; set; }

        public override XElement SerializeToXml()
        {
            var elem = base.SerializeToXml();
            //var conElem = Connection.SerializeToXml();
            //foreach (var attr in conElem.Attributes())
            //    elem.Add(new XElement(attr.Name.LocalName, attr.Value));
            //if (!string.IsNullOrEmpty(conElem.Value))
            //    elem.Add(new XElement("Content", conElem.Value));
            elem.Add(Connection.SerializeToXml());
            return elem;
        }

        public static ConnectionNode Create(Connector connector)
        {
            var node = new ConnectionNode()
            {
                Connection = connector
            };
            node.GenerateID();
            return node;
        }
    }
}
