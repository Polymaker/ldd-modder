using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LDDModder.Modding
{
    public class ElementReference : PartElement
    {
        public const string NODE_NAME = "ElementRef";

        private PartElement _Element;
        public PartElement Element => GetElement();

        public string ElementID { get; protected set; }

        public ElementReference()
        {

        }

        public ElementReference(PartElement element)
        {
            _Element = element;
            ElementID = element.ID;
        }

        public PartElement GetElement()
        {
            if (Project != null && _Element == null)
                _Element = Project.GetElementByID(ElementID);
            return _Element;
        }

        public override XElement SerializeToXml()
        {
            var elem = base.SerializeToXmlBase(NODE_NAME);
            elem.RemoveAttributes();
            elem.WriteAttribute("RefID", ElementID);
            return elem;
        }

        protected internal override void LoadFromXml(XElement element)
        {
            base.LoadFromXml(element);
            ElementID = element.ReadAttribute("RefID", string.Empty);
        }
    }
}
