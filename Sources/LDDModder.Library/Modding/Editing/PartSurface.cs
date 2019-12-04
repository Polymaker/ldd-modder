﻿using LDDModder.LDD.Files;
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
            Name = $"Surface{surfaceID}";
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

            Name = $"Surface{SurfaceID}";

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
            {
                foreach (var v in builder.Vertices)
                    v.TexCoord = Simple3D.Vector2.Empty;
            }

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
                builder.CombineGeometry(meshRef.GetGeometry());

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
    }
}
