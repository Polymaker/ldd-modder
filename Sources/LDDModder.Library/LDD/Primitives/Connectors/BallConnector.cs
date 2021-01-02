using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LDDModder.LDD.Primitives.Connectors
{
    public class BallConnector : Connector
    {
        public override ConnectorType Type => ConnectorType.Ball;

        private string _FlexAttributes;

        /// <summary>
        /// Only used when the node is a descendant of Flex
        /// </summary>
        public string FlexAttributes
        {
            get => _FlexAttributes;
            set => SetPropertyValue(ref _FlexAttributes, value);
        }

        public override void LoadFromXml(XElement element)
        {
            base.LoadFromXml(element);
            FlexAttributes = element.ReadAttribute("flexAttributes", string.Empty);
        }

        protected override void SerializeBeforeTransform(XElement element)
        {
            if (!string.IsNullOrEmpty(FlexAttributes))
                element.Add(new XAttribute("flexAttributes", FlexAttributes));
        }

        public double[] GetFlexValues()
        {
            var values = new double[5];
            if (!string.IsNullOrWhiteSpace(FlexAttributes))
            {
                var strValues = FlexAttributes.Split(',');
                for (int i = 0; i < strValues.Length; i++)
                {
                    if (double.TryParse(strValues[i].Trim(), out double fV))
                        values[i] = fV;
                }
            }
            return values;
        }

        public void SetFlexValues(double[] values)
        {
            if (values.Length == 0)
                FlexAttributes = string.Empty;
            else if (values.Length == 5)
                FlexAttributes = string.Join(",", values.Select(x => string.Format(CultureInfo.InvariantCulture, "{0}", x)));
        }

        public override Connector Clone()
        {
            return new BallConnector()
            {
                FlexAttributes = FlexAttributes,
                SubType = SubType,
                Transform = Transform.Clone()
            };
        }
    }
}
