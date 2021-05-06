using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LDDModder.Modding
{
    public class OutlinesGroupConfig : PartElement
    {
        public const string NODE_NAME = "ClonePattern";

        private double _BreakAngle;

        public double BreakAngle
        {
            get => _BreakAngle;
            set => SetPropertyValue(ref _BreakAngle, value);
        }

        public ElementReferenceCollection Elements { get; set; }

        public OutlinesGroupConfig()
        {
            _BreakAngle = 35;
            Elements = new ElementReferenceCollection(this);
            Elements.SupportedTypes.Add(typeof(ModelMeshReference));
            TrackCollectionChanges(Elements);
        }

        public override XElement SerializeToXml()
        {
            var elem = SerializeToXmlBase(NODE_NAME);

            elem.WriteAttribute(nameof(BreakAngle), BreakAngle);

            elem.Add(Elements.Serialize(nameof(Elements)));

            return elem;
        }

        protected internal override void LoadFromXml(XElement element)
        {
            base.LoadFromXml(element);

            BreakAngle = element.ReadAttribute(nameof(BreakAngle), 35d);
            Elements.Clear();
            if (element.HasElement(nameof(Elements), out XElement elemsElem))
                Elements.LoadFromXml(elemsElem);
        }

        public static OutlinesGroupConfig FromXml(XElement element)
        {
            var config = new OutlinesGroupConfig();
            config.LoadFromXml(element);
            return config;
        }
    }
}
