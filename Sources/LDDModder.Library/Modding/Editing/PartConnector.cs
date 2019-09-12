using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LDDModder.LDD.Primitives;
using LDDModder.LDD.Primitives.Connectors;
using LDDModder.Utilities;

namespace LDDModder.Modding.Editing
{
    public abstract class PartConnector
    {
        //public abstract LDD.Primitives.Connectors.Connector Connector { get; set; }
        public ItemTransform Transform { get; set; }

        public static PartConnector FromLDD(Primitive primitive, Connector connector)
        {
            if (connector.GetType() == typeof(Custom2DFieldConnector))
            {
                var studConn = new StudConnection()
                {
                    Transform = ItemTransform.FromLDD(connector.Transform),
                    Connector = connector as Custom2DFieldConnector,
                    RefID = StringUtils.GenerateUUID($"{primitive.ID}_{primitive.Connectors.IndexOf(connector)}", 8)
                };
                return studConn;
            }
            else
            {
                var genType = typeof(PartConnector<>).MakeGenericType(connector.GetType());
                return (PartConnector)Activator.CreateInstance(genType, connector);
            }
        }

        public static PartConnector FromLDD(Connector connector)
        {
            if (connector.GetType() == typeof(Custom2DFieldConnector))
            {
                var studConn = new StudConnection()
                {
                    Transform = ItemTransform.FromLDD(connector.Transform),
                    Connector = connector as Custom2DFieldConnector
                };
                return studConn;
            }
            else
            {
                var genType = typeof(PartConnector<>).MakeGenericType(connector.GetType());
                return (PartConnector)Activator.CreateInstance(genType, connector);
            }
        }
    }

    public class PartConnector<T> : PartConnector where T : Connector
    {
        public T Connector { get; set; }

        public PartConnector()
        {
        }

        public PartConnector(T connector)
        {
            Connector = connector;
            Transform = ItemTransform.FromLDD(connector.Transform);
        }
    }
}
