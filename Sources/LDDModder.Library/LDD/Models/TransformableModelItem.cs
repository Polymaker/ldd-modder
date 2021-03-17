using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LDDModder.LDD.Models
{
    public abstract class TransformableModelItem : ModelItem
    {
        public ModelTransform Transform { get; set; }

        public TransformableModelItem()
        {
            Transform = ModelTransform.Identity;
        }

        public override void LoadFromXml(XElement element)
        {
            base.LoadFromXml(element);
            if (element.HasAttribute("transformation", out XAttribute transformAttr))
                Transform = ModelTransform.Parse(transformAttr.Value);
        }

        protected override void SerializeElement(XElement element)
        {
            base.SerializeElement(element);
            element.Add(new XAttribute("transformation", Transform.SerializeToString()));
        }
    }
}
