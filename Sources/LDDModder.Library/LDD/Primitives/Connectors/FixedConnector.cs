using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LDDModder.LDD.Primitives.Connectors
{
    public class FixedConnector : Connector
    {
        private int _Axes;
        private string _Tag;

        public override ConnectorType Type => ConnectorType.Fixed;

        public int Axes
        {
            get => _Axes;
            set => SetPropertyValue(ref _Axes, value);
        }

        public string Tag
        {
            get => _Tag;
            set => SetPropertyValue(ref _Tag, value);
        }

        protected override void SerializeBeforeTransform(XElement element)
        {
            if (Axes > 0)
                element.AddNumberAttribute("axes", Axes);

            if (!string.IsNullOrEmpty(Tag))
                element.Add(new XAttribute("tag", Tag));
        }

        public override void LoadFromXml(XElement element)
        {
            base.LoadFromXml(element);
            if (element.TryReadAttribute("axes", out int axes))
                Axes = axes;
            Tag = element.ReadAttribute("tag", string.Empty);
        }

        public override Connector Clone()
        {
            return new FixedConnector()
            {
                Axes = Axes,
                Tag = Tag,
                SubType = SubType,
                Transform = Transform.Clone()
            };
        }
    }
}
