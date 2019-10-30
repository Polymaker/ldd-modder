using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LDDModder.LDD.Primitives.Connectors
{
    public class FixedConnector : Connector
    {
        public override ConnectorType Type => ConnectorType.Fixed;

        public int Axes { get; set; }

        public string Tag { get; set; }

        protected override void SerializeBeforeTransform(XElement element)
        {
            if (Axes > 0)
                element.AddNumberAttribute("axes", Axes);

            if (!string.IsNullOrEmpty(Tag))
                element.Add(new XAttribute("tag", Tag));
        }

        public override void LoadFromXml(XElement element)
        {
            base.LoadFromXml(element);
            if (element.TryReadAttribute("axes", out int axes))
                Axes = axes;
            Tag = element.ReadAttribute("tag", string.Empty);
        }
    }
}
