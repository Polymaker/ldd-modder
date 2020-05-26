using LDDModder.LDD.Files;
using LDDModder.LDD.Meshes;
using LDDModder.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace LDDModder.Modding.Editing
{
    public class PartSurface : PartElement
    {
        public const string NODE_NAME = "Surface";

        public int SurfaceID { get; set; }

        private int _SubMaterialIndex;

        public int SubMaterialIndex
        {
            get => _SubMaterialIndex;
            set => SetPropertyValue(ref _SubMaterialIndex, value);
        }

        public ElementCollection<SurfaceComponent> Components { get; set; }

        public PartSurface()
        {
            Components = new ElementCollection<SurfaceComponent>(this);
        }

        public PartSurface(int surfaceID, int subMaterialIndex)
        {
            SurfaceID = surfaceID;
            SubMaterialIndex = subMaterialIndex;
            Components = new ElementCollection<SurfaceComponent>(this);
            InternalSetName($"Surface{surfaceID}");
        }

        public override bool TryRemove()
        {
            if (SurfaceID == 0)
                return false;
            return base.TryRemove();
        }

        public IEnumerable<ModelMeshReference> GetAllMeshReferences()
        {
            return Components.SelectMany(c => c.GetAllMeshReferences());
        }

        public IEnumerable<ModelMesh> GetAllModelMeshes()
        {
            return Components.SelectMany(c => c.GetAllModelMeshes());
        }

        #region Xml Serialization

        public override XElement SerializeToXml()
        {
            var elem = SerializeToXmlBase(NODE_NAME);
            elem.RemoveAttributes();

            elem.Add(new XAttribute(nameof(SurfaceID), SurfaceID));
            elem.Add(new XAttribute(nameof(SubMaterialIndex), SubMaterialIndex));

            //if (Project != null)
            //    elem.Add(new XAttribute("OutputFile", GetTargetFilename()));

            foreach (var comp in Components)
                elem.Add(comp.SerializeToXml());

            return elem;
        }

        protected internal override void LoadFromXml(XElement element)
        {
            base.LoadFromXml(element);
            
            if (element.TryGetIntAttribute(nameof(SurfaceID), out int surfID))
                SurfaceID = surfID;

            InternalSetName($"Surface{SurfaceID}");

            if (element.TryGetIntAttribute(nameof(SubMaterialIndex), out int matIDX))
                SubMaterialIndex = matIDX;

            foreach (var compElem in element.Elements(SurfaceComponent.NODE_NAME))
                Components.Add(SurfaceComponent.FromXml(compElem));
        }

        public static PartSurface FromXml(XElement element)
        {
            var surface = new PartSurface();
            surface.LoadFromXml(element);
            return surface;
        }

        #endregion

        public string GetTargetFilename()
        {
            if (Project != null)
            {
                if (SurfaceID > 0)
                    return $"{Project.PartID}.g{SurfaceID}";
                return $"{Project.PartID}.g";
            }
            return string.Empty;
        }

        public MeshFile GenerateMeshFile()
        {
            var builder = new GeometryBuilder();
            var cullings = new List<MeshCulling>();
            var notLoadedModels = GetAllModelMeshes().Where(x => !x.IsModelLoaded).ToList();

            foreach (var meshComp in Components)
            {
                var cullingInfo = CombineMeshes(builder, meshComp.Meshes, meshComp.GetCullingType());
                meshComp.FillCullingInformation(cullingInfo);
                cullings.Add(cullingInfo);
            }

            if (SurfaceID == 0)
                builder.RemoveTextureCoords();
            else
                builder.ForceTextureCoords();

            notLoadedModels.ForEach(x => x.UnloadModel());

            var file = new MeshFile(builder.GetGeometry());
            
            file.Cullings.AddRange(cullings);
            return file;
        }
    
        private static MeshCulling CombineMeshes(GeometryBuilder builder, IEnumerable<ModelMeshReference> models, MeshCullingType cullingType)
        {
            int fromIndex = builder.IndexCount;
            int fromVertex = builder.VertexCount;

            foreach (var meshRef in models)
                builder.CombineGeometry(meshRef.GetGeometry(true));

            int indexCount = builder.IndexCount - fromIndex;
            int vertexCount = builder.VertexCount - fromVertex;

            return new MeshCulling(cullingType)
            {
                FromIndex = fromIndex,
                FromVertex = fromVertex,
                IndexCount = indexCount,
                VertexCount = vertexCount
            };
        }

        public override List<ValidationMessage> ValidateElement()
        {
            var messages = new List<ValidationMessage>();

            void AddMessage(string code, ValidationLevel level, params object[] args)
            {
                messages.Add(new ValidationMessage(this, code, level)
                {
                    MessageArguments = args
                });
            }

            var allModels = GetAllModelMeshes();
            var standardMeshes = Components.SelectMany(x => x.Meshes);

            //if (!Components.Any())
            //{
            //    AddMessage("SURFACE_NO_COMPONENTS",
            //        SurfaceID == 0 ? ValidationLevel.Error : ValidationLevel.Warning, SurfaceID);
            //}

            if (!standardMeshes.Any())
            {
                AddMessage("SURFACE_NO_MODELS", 
                    SurfaceID == 0 ? ValidationLevel.Error : ValidationLevel.Warning, SurfaceID);
            }
            else
            {
                if (SurfaceID > 0)
                {
                    if (allModels.Any(x => !x.IsTextured))
                        AddMessage("SURFACE_NO_TEXTURED_MODEL", ValidationLevel.Warning, SurfaceID);
                }
                else
                {
                    if (allModels.Any(x => x.IsTextured))
                        AddMessage("SURFACE_IGNORE_TEXTURED_MODEL", ValidationLevel.Info, SurfaceID);
                }
            }

            if (Project.Flexible)
            {
                int notFlexibleModels = allModels.Count(x => !x.IsFlexible);
                if (notFlexibleModels == allModels.Count())
                    AddMessage("SURFACE_NO_FLEXIBLE_MODEL", ValidationLevel.Error, SurfaceID);
                else if(notFlexibleModels > 0)
                    AddMessage("SURFACE_MISSING_BONEWEIGHTS", ValidationLevel.Warning, SurfaceID);

                if (Components.Any(x => x.ComponentType != ModelComponentType.Part))
                    AddMessage("SURFACE_INVALID_COMPONENT", ValidationLevel.Error, SurfaceID);
            }

            foreach (var component in Components)
                messages.AddRange(component.ValidateElement());

            return messages;
        }
    }
}
