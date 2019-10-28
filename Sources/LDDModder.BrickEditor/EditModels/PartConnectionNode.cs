using LDDModder.Modding.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.EditModels
{
    public class PartConnectionNode : ProjectComponentNode<PartConnection>
    {
        public PartConnection Connection => Component;

        public PartConnectionNode(PartConnection connection) : base(connection)
        {
            Name = $"{connection.ConnectorType}";
        }
    }
}
