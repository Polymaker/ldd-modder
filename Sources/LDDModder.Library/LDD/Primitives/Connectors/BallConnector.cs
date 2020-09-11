using System;
using System.Collections.Generic;
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
