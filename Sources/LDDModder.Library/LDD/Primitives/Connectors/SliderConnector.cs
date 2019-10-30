using LDDModder.Simple3D;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LDDModder.LDD.Primitives.Connectors
{
    public class SliderConnector : Connector
    {
        public override ConnectorType Type => ConnectorType.Slider;

        public float Length { get; set; }

        public bool StartCapped { get; set; }

        public bool EndCapped { get; set; }

        public bool Cylindrical { get; set; }

        public Vector3? Spring { get; set; }

        public string Tag { get; set; }

        protected override void SerializeBeforeTransform(XElement element)
        {
            element.AddNumberAttribute("length", Length);
            if (Cylindrical)
                element.AddBooleanAttribute("cylindrical", Cylindrical);
            if (!string.IsNullOrEmpty(Tag))
                element.Add(new XAttribute("tag", Tag));
            element.AddBooleanAttribute("startCapped", StartCapped);
            element.AddBooleanAttribute("endCapped", EndCapped);

            if (Spring.HasValue)
            {
                element.Add(new XAttribute("spring", 
                    string.Format(CultureInfo.InvariantCulture, "{0},{1},{2}", Spring.Value.X, Spring.Value.Y, Spring.Value.Z)
                    ));
            }
        }

        public override void LoadFromXml(XElement element)
        {
            base.LoadFromXml(element);
            Length = element.ReadAttribute("length", 0f);
            Cylindrical = element.ReadAttribute("cylindrical", false);
            StartCapped = element.ReadAttribute("startCapped", false);
            EndCapped = element.ReadAttribute("endCapped", false);
            Tag = element.ReadAttribute("tag", string.Empty);

            if (element.HasAttribute("spring", out XAttribute springAttr))
            {
                var springValues = springAttr.Value.Split(',');
                Spring = new Vector3(
                    float.Parse(springValues[0].Trim(), CultureInfo.InvariantCulture),
                    float.Parse(springValues[1].Trim(), CultureInfo.InvariantCulture),
                    float.Parse(springValues[2].Trim(), CultureInfo.InvariantCulture));
            }
        }
    }
}
