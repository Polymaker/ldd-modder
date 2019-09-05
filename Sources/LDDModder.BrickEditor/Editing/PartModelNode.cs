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
        public int SurfaceID { get; set; }

        public int SubMaterialID { get; set; }

        public bool IsMainModel => SurfaceID == 0;

        public override string GetName()
        {
            return IsMainModel ? "Main model" : $"Decoration {SurfaceID}";
        }

        public override XElement SerializeToXml()
        {
            var elem = new XElement("Model", new XAttribute("ID", ID));
            //if (IsMainModel)
            //    elem.Add(new XAttribute("MainModel", 1));

            elem.Add(new XAttribute("SurfaceID", SurfaceID));
            elem.Add(new XAttribute("SubMaterialID", SubMaterialID));

            return elem;
        }
    }
}
