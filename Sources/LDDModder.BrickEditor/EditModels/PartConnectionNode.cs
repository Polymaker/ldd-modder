using LDDModder.Modding.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.EditModels
{
    public class PartConnectionNode : ProjectComponentNode<PartConnector>
    {
        public PartConnector Connection => Component;

        public PartConnectionNode(PartConnector connection) : base(connection)
        {
            Name = $"{connection.ConnectorType}";
        }
    }
}
