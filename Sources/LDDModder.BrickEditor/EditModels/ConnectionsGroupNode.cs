using LDDModder.BrickEditor.Resources;
using LDDModder.Modding.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.EditModels
{
    public class ConnectionsGroupNode : ComponentGroupNode
    {
        public ComponentCollection<PartConnector> Connections { get; }

        public ConnectionsGroupNode(PartProject project) : base(project.Connections)
        {
            Connections = project.Connections;
            Name = ModelLocalizations.Label_Connections;
        }

        public ConnectionsGroupNode(ComponentCollection<PartConnector> connections) : base (connections)
        {
            Connections = connections;
            Name = ModelLocalizations.Label_Connections;
        }

        public override void RebuildChildrens()
        {
            Childrens.Clear();
            foreach (var conn in Connections)
                Childrens.Add(new PartConnectionNode(conn));
        }
    }
}
