using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LDDModder.LDD.Primitives.Connectors
{
    public class RailConnector : Connector
    {
        public override ConnectorType Type => ConnectorType.Rail;

        public float Length { get; set; }

        protected override void SerializeBeforeTransform(XElement element)
        {
            if (Length > 0)
                element.AddNumberAttribute("length", Length);
        }

        public override void LoadFromXml(XElement element)
        {
            base.LoadFromXml(element);
            if (element.TryReadAttribute("length", out float length))
                Length = length;
        }
    }
}
