using LDDModder.LDD.Meshes;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace LDDModder.Modding.Editing
{
    public class FemaleStudModel : PartCullingModel
    {
        public override ModelComponentType ComponentType => ModelComponentType.FemaleStud;

        //public MeshGeometry ReplacementGeometry { get; set; }
        public ElementCollection<ModelMesh> ReplacementGeometries { get; set; }

        public ElementCollection<StudReference> Studs
        {
            get => StudReferences;
            set => StudReferences = value;
        }

        public FemaleStudModel()
        {
            //Studs = new ComponentCollection<StudReference>(this);
            ReplacementGeometries = new ElementCollection<ModelMesh>(this);
        }

        //public override IEnumerable<StudReference> GetStudReferences()
        //{
        //    return Studs;
        //}

        protected override IEnumerable<PartElement> GetAllChilds()
        {
            return base.GetAllChilds().Concat(ReplacementGeometries);
        }

        public override IEnumerable<ModelMesh> GetAllMeshes()
        {
            return base.GetAllMeshes().Concat(ReplacementGeometries);
        }

        public override XElement SerializeToXml()
        {
            var elem = base.SerializeToXml();
            elem.Add(new XComment("This geometry is used when all the studs (defined bellow) are connected"));
            var geomElem = elem.AddElement("ReplacementGeometries");
            foreach (var geom in ReplacementGeometries)
                geomElem.Add(geom.SerializeToXml());

            var studsElem = elem.AddElement("Studs");
            foreach (var stud in Studs)
                studsElem.Add(stud.SerializeToXml());

            return elem;
        }

        protected internal override void LoadFromXml(XElement element)
        {
            base.LoadFromXml(element);

            var geomElem = element.Element("ReplacementGeometries");
            if (geomElem != null)
            {
                foreach (var elem in geomElem.Elements(ModelMesh.NODE_NAME))
                {
                    var mesh = new ModelMesh();
                    mesh.LoadFromXml(elem);
                    ReplacementGeometries.Add(mesh);
                }
            }

            if (element.Element("Studs") != null)
            {
                foreach (var studElem in element.Element("Studs").Elements(StudReference.NODE_NAME))
                    Studs.Add(StudReference.FromXml(studElem));
            }
        }
    }
}
