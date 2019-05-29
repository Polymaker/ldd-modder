using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LDDModder.LDD.Primitives.Connectors
{
    public class HingeConnector : Connector
    {
        public override ConnectorType Type => ConnectorType.Hinge;

        public bool Oriented { get; set; }

        public float LimitMin { get; set; }

        public float LimitMax { get; set; }

        public float FlipLimitMin { get; set; }

        public float FlipLimitMax { get; set; }

        public string Tag { get; set; }

        protected override void SerializeBeforeTransform(XElement element)
        {
            element.AddBooleanAttribute("oriented", Oriented);
            if (!string.IsNullOrEmpty(Tag))
                element.Add(new XAttribute("tag", Tag));
        }

        protected override void SerializeAfterTransform(XElement element)
        {
            if (LimitMin != 0)
                element.AddNumberAttribute("LimMin", LimitMin);
            if (LimitMax != 0)
                element.AddNumberAttribute("LimMax", LimitMax);
            if (FlipLimitMin != 0)
                element.AddNumberAttribute("FlipLimMin", FlipLimitMin);
            if (FlipLimitMax != 0)
                element.AddNumberAttribute("FlipLimMax", FlipLimitMax);
        }

        public override void LoadFromXml(XElement element)
        {
            base.LoadFromXml(element);

            if (element.TryReadAttribute("oriented", out bool oriented))
                Oriented = oriented;

            if (element.TryReadAttribute("LimMin", out float LimMin))
                LimitMin = LimMin;

            if (element.TryReadAttribute("LimMax", out float LimMax))
                LimitMax = LimMax;

            if (element.TryReadAttribute("FlipLimMin", out float FlipLimMin))
                FlipLimitMin = FlipLimMin;

            if (element.TryReadAttribute("FlipLimMax", out float FlipLimMax))
                FlipLimitMax = FlipLimMax;

            Tag = element.Attribute("tag")?.Value ?? string.Empty;
        }

    }
}
