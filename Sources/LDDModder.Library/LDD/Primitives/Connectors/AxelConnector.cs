using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LDDModder.LDD.Primitives.Connectors
{
    public class AxelConnector : Connector
    {
        public override ConnectorType Type => ConnectorType.Axel;

        public float Length { get; set; }

        public bool IsGrabbingRequired { get; set; }

        public bool Grabbing { get; set; }

        public bool StartCapped { get; set; }

        public bool EndCapped { get; set; }

        protected override void SerializeBeforeTransform(XElement element)
        {
            element.AddNumberAttribute("length", Length);
            element.AddBooleanAttribute(IsGrabbingRequired ? "requireGrabbing" : "grabbing", Grabbing);
            element.AddBooleanAttribute("startCapped", StartCapped);
            element.AddBooleanAttribute("endCapped", EndCapped);
        }

        public override void LoadFromXml(XElement element)
        {
            base.LoadFromXml(element);

            if (element.TryReadAttribute("length", out float length))
                Length = length;

            if (element.HasAttribute("requireGrabbing"))
            {
                IsGrabbingRequired = true;
                Grabbing = element.GetBoolAttribute("requireGrabbing");
            }
            else if (element.HasAttribute("grabbing"))
            {
                IsGrabbingRequired = false;
                Grabbing = element.GetBoolAttribute("grabbing");
            }
            else //XML error
            {
                IsGrabbingRequired = false;
                Grabbing = false;
            }

            StartCapped = element.GetBoolAttribute("startCapped");
            EndCapped = element.GetBoolAttribute("endCapped");
        }
    }
}
