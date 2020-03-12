﻿using LDDModder.Simple3D;
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
        private float _Length;
        private bool _StartCapped;
        private bool _EndCapped;
        private bool _Cylindrical;
        private Vector3? _Spring;
        private string _Tag;

        public override ConnectorType Type => ConnectorType.Slider;

        public float Length
        {
            get => _Length;
            set => SetPropertyValue(ref _Length, value);
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

        public bool Cylindrical
        {
            get => _Cylindrical;
            set => SetPropertyValue(ref _Cylindrical, value);
        }

        public Vector3? Spring
        {
            get => _Spring;
            set => SetPropertyValue(ref _Spring, value);
        }

        public string Tag
        {
            get => _Tag;
            set => SetPropertyValue(ref _Tag, value);
        }

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
            else
                Spring = null;
        }
    }
}
