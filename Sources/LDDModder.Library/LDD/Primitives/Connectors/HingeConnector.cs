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
        private bool _Oriented;
        private float _LimitMin;
        private float _LimitMax;
        private float _FlipLimitMin;
        private float _FlipLimitMax;
        private string _Tag;

        public override ConnectorType Type => ConnectorType.Hinge;

        public bool Oriented
        {
            get => _Oriented;
            set => SetPropertyValue(ref _Oriented, value);
        }

        public float LimitMin
        {
            get => _LimitMin;
            set => SetPropertyValue(ref _LimitMin, value);
        }

        public float LimitMax
        {
            get => _LimitMax;
            set => SetPropertyValue(ref _LimitMax, value);
        }

        public float FlipLimitMin
        {
            get => _FlipLimitMin;
            set => SetPropertyValue(ref _FlipLimitMin, value);
        }

        public float FlipLimitMax
        {
            get => _FlipLimitMax;
            set => SetPropertyValue(ref _FlipLimitMax, value);
        }

        public string Tag
        {
            get => _Tag;
            set => SetPropertyValue(ref _Tag, value);
        }

        public HingeConnector()
        {
            SubType = 2;
        }

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

            Tag = element.ReadAttribute("tag", string.Empty);
        }


        public override Connector Clone()
        {
            return new HingeConnector()
            {
                Oriented = Oriented,
                Tag = Tag,
                FlipLimitMax = FlipLimitMax,
                FlipLimitMin = FlipLimitMin,
                LimitMax = LimitMax,
                LimitMin = LimitMin,
                SubType = SubType,
                Transform = Transform.Clone()
            };
        }
    }
}
