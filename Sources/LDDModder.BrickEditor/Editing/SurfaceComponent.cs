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
    public class SurfaceComponent : PartNode
    {
        public MeshCullingType ComponentType { get; set; }

        public PartNodeCollection<MeshElement> Meshes { get; set; }

        public PartNodeCollection<MeshElement> AlternateMeshes { get; set; }

        public PartNodeCollection<StudReference> LinkedStuds { get; set; }

        public PartSurface Surface => Parent as PartSurface;

        public SurfaceComponent()
        {
            Meshes = new PartNodeCollection<MeshElement>(this);
            AlternateMeshes = new PartNodeCollection<MeshElement>(this);
            LinkedStuds = new PartNodeCollection<StudReference>(this);
        }

        public IEnumerable<MeshElement> GetAllMeshes()
        {
            return Meshes.Concat(AlternateMeshes);
        }

        public override string GetDisplayName()
        {
            if (!string.IsNullOrEmpty(Description))
                return Description;

            switch (ComponentType)
            {
                case MeshCullingType.MainModel:
                    return PartResources.Component_MainModel;
                case MeshCullingType.Stud:
                    return PartResources.Component_StudMale;
                case MeshCullingType.FemaleStud:
                    return PartResources.Component_StudFemale;
                case MeshCullingType.Tube:
                    return PartResources.Component_Tube;
            }

            return base.GetDisplayName();
        }

        public override XElement SerializeToXml()
        {
            var rootElem = new XElement("Component");
            rootElem.Add(new XAttribute("ID", ID));
            rootElem.Add(new XAttribute("Type", ComponentType));
            if (!string.IsNullOrEmpty(Description))
                rootElem.Add(new XAttribute("Description", Description));
            
            if (Meshes.Any())
            {
                //var meshNode = new XElement("Meshes");
                //foreach (var meshElem in Meshes)
                //    meshNode.Add(new XElement("MainMesh", new XAttribute("MeshID", meshElem.ID)));
                //rootElem.Add(meshNode);

                foreach (var meshElem in Meshes)
                    rootElem.Add(new XElement("MainMesh", new XAttribute("MeshID", meshElem.ID)));
            }

            if (AlternateMeshes.Any())
            {
                //var altMeshNode = new XElement("AlternateMeshes");
                //foreach (var meshElem in AlternateMeshes)
                //    altMeshNode.Add(meshElem.SerializeHierarchy());
                //rootElem.Add(altMeshNode);


                foreach (var meshElem in AlternateMeshes)
                    rootElem.Add(new XElement("AltMesh", new XAttribute("MeshID", meshElem.ID)));
            }

            if (LinkedStuds.Any())
            {
                //var studNode = new XElement("Studs");
                //foreach (var studElem in LinkedStuds)
                //    studNode.Add(studElem.SerializeToXml());
                //rootElem.Add(studNode);

                foreach (var studElem in LinkedStuds)
                    rootElem.Add(studElem.SerializeToXml());
            }

            return rootElem;
        }

        public override XElement SerializeHierarchy()
        {
            return SerializeToXml();
        }
    }
}
