using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.LDD.Primitives.Connectors
{
    public class ConnectorInfo
    {
        public ConnectorType Type { get; set; }
        public int SubType { get; set; }
        public string Description { get; set; }
        public List<int> ConnectsWith { get; set; } = new List<int>();


    }
}
