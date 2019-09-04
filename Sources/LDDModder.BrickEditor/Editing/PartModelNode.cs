using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LDDModder.BrickEditor.Editing
{
    public class PartModelNode : PartNode
    {
        public int DecorationNumber { get; set; }
        public bool IsMainModel => DecorationNumber <= 0;

        public override string GetName()
        {
            return IsMainModel ? "Main model" : $"Decoration {DecorationNumber}";
        }

        public override XElement SerializeToXml()
        {
            var elem = new XElement("Model", new XAttribute("ID", ID));
            if (IsMainModel)
                elem.Add(new XAttribute("MainModel", 1));
            else
                elem.Add(new XAttribute("Decoration", DecorationNumber));
            return elem;
        }
    }
}
