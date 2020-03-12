using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LDDModder.LDD.Primitives.Connectors
{
    public class GearConnector : Connector
    {
        private int _ToothCount;
        private double _Radius;

        public override ConnectorType Type => ConnectorType.Gear;

        public int ToothCount
        {
            get => _ToothCount;
            set => SetPropertyValue(ref _ToothCount, value);
        }

        public double Radius
        {
            get => _Radius;
            set => SetPropertyValue(ref _Radius, value);
        }

        protected override void SerializeBeforeTransform(XElement element)
        {
            element.AddNumberAttribute("toothCount", ToothCount);
            element.AddNumberAttribute("radius", Radius);
        }

        public override void LoadFromXml(XElement element)
        {
            base.LoadFromXml(element);

            if (element.TryReadAttribute("toothCount", out int toothCount))
                ToothCount = toothCount;

            if (element.TryReadAttribute("radius", out double radius))
                Radius = radius;
        }
    }
}
