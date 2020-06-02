using LDDModder.LDD.Meshes;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace LDDModder.Modding.Editing
{
    public class FemaleStudModel : PartCullingModel
    {
        public override ModelComponentType ComponentType => ModelComponentType.FemaleStud;

        public ElementCollection<ModelMeshReference> ReplacementMeshes { get; set; }

        public ElementCollection<StudReference> Studs { get; private set; }

        public FemaleStudModel()
        {
            Studs = new ElementCollection<StudReference>(this);
            ReplacementMeshes = new ElementCollection<ModelMeshReference>(this);
        }

        internal override void LoadCullingInformation(MeshCulling culling)
        {
            base.LoadCullingInformation(culling);

            //Studs.AddRange(ConvertFromRefs(culling.Studs));
        }

        internal override void FillCullingInformation(MeshCulling culling)
        {
            base.FillCullingInformation(culling);
            //culling.Studs.AddRange(Studs.Select(x => ConvertToRef(x)));

            if (ReplacementMeshes.Any())
            {
                var builder = new GeometryBuilder();
                foreach(var meshRef in ReplacementMeshes)
                    builder.CombineGeometry(meshRef.GetGeometry(true));

                if (Surface.SurfaceID == 0)
                    builder.RemoveTextureCoords();

                culling.ReplacementMesh = builder.GetGeometry();
            }
        }

        public override IEnumerable<ModelMeshReference> GetAllMeshReferences()
        {
            return base.GetAllMeshReferences().Concat(ReplacementMeshes);
        }

        public override XElement SerializeToXml()
        {
            var elem = base.SerializeToXml();
            //elem.Add(new XComment("This geometry is used when all the studs (defined bellow) are connected"));
            var geomElem = elem.AddElement(nameof(ReplacementMeshes));
            foreach (var geom in ReplacementMeshes)
                geomElem.Add(geom.SerializeToXml());

            //var studsElem = elem.AddElement(nameof(Studs));
            //foreach (var stud in Studs)
            //    studsElem.Add(stud.SerializeToXml2());

            return elem;
        }

        protected internal override void LoadFromXml(XElement element)
        {
            base.LoadFromXml(element);

            if (element.HasElement(nameof(ReplacementMeshes), out XElement geomElem))
            {
                foreach (var elem in geomElem.Elements(ModelMeshReference.NODE_NAME))
                {
                    var mesh = new ModelMeshReference();
                    mesh.LoadFromXml(elem);
                    ReplacementMeshes.Add(mesh);
                }
            }

            //LEGACY
            if (element.HasElement("Studs", out XElement studsElem))
            {
                foreach (var studElem in studsElem.Elements(StudReference.NODE_NAME))
                {
                    var studRef = StudReference.FromXml(studElem);
                    if (!string.IsNullOrEmpty(LegacyConnectionID))
                        studRef.ConnectionID = LegacyConnectionID;
                    ReferencedStuds.Add(studRef);
                }
            }
        }

        public override List<ValidationMessage> ValidateElement()
        {
            var messages = base.ValidateElement();

            void AddMessage(string code, ValidationLevel level, params object[] args)
            {
                messages.Add(new ValidationMessage(this, code, level)
                {
                    MessageArguments = args
                });
            }

            if (!ReferencedStuds.Any())
            {
                if (ReplacementMeshes.Any())
                    AddMessage("MODEL_STUDS_NOT_DEFINED_ALT", ValidationLevel.Warning);
                else
                    AddMessage("MODEL_STUDS_NOT_DEFINED", ValidationLevel.Warning);
            }
            else
                messages.AddRange(ReferencedStuds.SelectMany(x => x.ValidateElement()));

            return messages;
        }
    }
}
