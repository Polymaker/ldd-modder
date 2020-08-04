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
        private float _Length;
        private bool _IsGrabbingRequired;
        private bool _Grabbing;
        private bool _StartCapped;
        private bool _EndCapped;

        public override ConnectorType Type => ConnectorType.Axel;

        public float Length
        {
            get => _Length;
            set => SetPropertyValue(ref _Length, value);
        }

        public bool IsGrabbingRequired
        {
            get => _IsGrabbingRequired;
            set => SetPropertyValue(ref _IsGrabbingRequired, value);
        }

        public bool Grabbing
        {
            get => _Grabbing;
            set => SetPropertyValue(ref _Grabbing, value);
        }

        public bool StartCapped
        {
            get => _StartCapped;
            set => SetPropertyValue(ref _StartCapped, value);
        }

        public bool EndCapped
        {
            get => _EndCapped;
            set => SetPropertyValue(ref _EndCapped, value);
        }

        public AxelConnector()
        {
            SubType = 5;
        }

        protected override void SerializeBeforeTransform(XElement element)
        {
            element.WriteAttribute("length", Length);
            element.WriteAttribute(
                IsGrabbingRequired ? "requireGrabbing" : "grabbing", 
                Grabbing, LinqXmlExtensions.BooleanXmlRepresentation.OneZero);
            element.WriteAttribute("startCapped", StartCapped, LinqXmlExtensions.BooleanXmlRepresentation.OneZero);
            element.WriteAttribute("endCapped", EndCapped, LinqXmlExtensions.BooleanXmlRepresentation.OneZero);
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

        public override Connector Clone()
        {
            return new AxelConnector()
            {
                EndCapped = EndCapped,
                IsGrabbingRequired = IsGrabbingRequired,
                Grabbing = Grabbing,
                Length = Length,
                StartCapped = StartCapped,
                SubType = SubType,
                Transform = Transform.Clone()
            };
        }
    }
}
