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
