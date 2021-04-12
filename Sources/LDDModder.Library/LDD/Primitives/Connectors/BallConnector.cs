using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        private double[] _FlexAttributes;

        /// <summary>
        /// Only used when the node is a descendant of Flex
        /// </summary>
        public double[] FlexAttributes
        {
            get => _FlexAttributes;
            set => SetPropertyValue(ref _FlexAttributes, value);
        }

        public override void LoadFromXml(XElement element)
        {
            base.LoadFromXml(element);

            string flexAttrs = element.ReadAttribute("flexAttributes", string.Empty);

            _FlexAttributes = null;
            if (!string.IsNullOrEmpty(flexAttrs))
            {
                if (TryParseFlexAttributes(flexAttrs, out double[] result))
                    FlexAttributes = result;
                else
                    Trace.WriteLine("Error parsing FlexAttributes");
            }
        }

        protected override void SerializeBeforeTransform(XElement element)
        {
            if (FlexAttributes != null)
            {
                string flexAttrStr = string.Join(",", FlexAttributes.Select(x => string.Format(CultureInfo.InvariantCulture, "{0}", x)));
                element.Add(new XAttribute("flexAttributes", flexAttrStr));
            }
        }

        public static bool TryParseFlexAttributes(string flexAttributes, out double[] result)
        {
            result = null;

            if (!string.IsNullOrWhiteSpace(flexAttributes))
            {
                var strValues = flexAttributes.Trim().Split(',');
                if (strValues.Length != 5)
                    return false;
                result = new double[5];
                for (int i = 0; i < strValues.Length; i++)
                {
                    if (double.TryParse(strValues[i].Trim(), out double fV))
                        result[i] = fV;
                    else
                        return false;
                }

                return true;
            }
            return false;
        }

        public void SetFlexValues(string values)
        {
            if (TryParseFlexAttributes(values, out double[] result))
                FlexAttributes = result;
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
