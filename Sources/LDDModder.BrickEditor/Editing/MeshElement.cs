using LDDModder.BrickEditor.Resources;
using LDDModder.LDD.Meshes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LDDModder.BrickEditor.Editing
{
    public class MeshElement : PartNode
    {
        public MeshGeometry Geometry { get; set; }

        public bool IsTextured { get; set; }

        public bool IsFlexible { get; set; }

        public MeshElement()
        {

        }

        //public override string GetDisplayName()
        //{
        //    if (string.IsNullOrEmpty(Description))
        //    {
        //        if (Parent is SurfaceComponent component)
        //        {
        //            if (component.Meshes.Contains(this))
        //                return $"{PartResources.Mesh} {component.Meshes.IndexOf(this) + 1}";
        //            if (component.AlternateMeshes.Contains(this))
        //                return $"{PartResources.Mesh} {component.AlternateMeshes.IndexOf(this) + 1}";
        //        }
        //    }
        //    return base.GetDisplayName();
        //}

        public MeshElement(MeshGeometry geometry)
        {
            Geometry = geometry;
            IsTextured = geometry.IsTextured;
            IsFlexible = geometry.IsFlexible;
            GenerateID();
        }

        public override XElement SerializeToXml()
        {
            var elem = new XElement("Mesh");
            elem.Add(new XAttribute("ID", ID));
            elem.Add(new XAttribute("IsTextured", IsTextured));
            elem.Add(new XAttribute("IsFlexible", IsFlexible));
            if (!string.IsNullOrEmpty(Description))
                elem.Add(new XAttribute("Description", Description));
            return elem;
        }
    }
}
