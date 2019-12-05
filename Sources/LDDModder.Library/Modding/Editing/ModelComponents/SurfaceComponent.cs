using LDDModder.LDD.Files;
using LDDModder.LDD.Meshes;
using LDDModder.LDD.Primitives.Connectors;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace LDDModder.Modding.Editing
{
    public abstract class SurfaceComponent : PartElement
    {
        public const string NODE_NAME = "Component";

        public abstract ModelComponentType ComponentType { get; }

        public ElementCollection<ModelMeshReference> Meshes { get; }

        public SurfaceComponent()
        {
            Meshes = new ElementCollection<ModelMeshReference>(this);
        }

        public PartSurface Surface => Parent as PartSurface;

        internal virtual void LoadCullingInformation(MeshCulling culling)
        {
            
        }

        internal virtual void FillCullingInformation(MeshCulling culling)
        {

        }

        public static SurfaceComponent CreateFromLDD(MeshCulling culling, ModelMesh mainModel)
        {
            SurfaceComponent modelComponent = null;

            switch (culling.Type)
            {
                case MeshCullingType.MainModel:
                    modelComponent = new PartModel();
                    break;
                case MeshCullingType.MaleStud:
                    modelComponent = new MaleStudModel();
                    break;
                case MeshCullingType.FemaleStud:
                    modelComponent = new FemaleStudModel();
                    break;
                case MeshCullingType.BrickTube:
                    modelComponent = new BrickTubeModel();
                    break;
            }

            if (modelComponent != null)
            {
                modelComponent.LoadCullingInformation(culling);
                modelComponent.Meshes.Add(new ModelMeshReference(mainModel, culling));
            }

            return modelComponent;
        }

        public MeshCullingType GetCullingType()
        {
            switch (ComponentType)
            {
                default:
                case ModelComponentType.Part:
                    return MeshCullingType.MainModel;
                case ModelComponentType.MaleStud:
                    return MeshCullingType.MaleStud;
                case ModelComponentType.FemaleStud:
                    return MeshCullingType.FemaleStud;
                case ModelComponentType.BrickTube:
                    return MeshCullingType.BrickTube;
            }
        }

        public virtual IEnumerable<ModelMeshReference> GetAllMeshReferences()
        {
            return Meshes;
        }

        public virtual IEnumerable<ModelMesh> GetAllModelMeshes()
        {
            return GetAllMeshReferences().Select(s => s.ModelMesh).Distinct();
        }

        #region Xml Serialization

        public override XElement SerializeToXml()
        {
            var elem = SerializeToXmlBase(NODE_NAME);
            elem.Add(new XAttribute("Type", ComponentType.ToString()));

            var geomElem = elem.AddElement(nameof(Meshes));

            foreach (var geom in Meshes)
                geomElem.Add(geom.SerializeToXml());

            return elem;
        }

        protected internal override void LoadFromXml(XElement element)
        {
            base.LoadFromXml(element);
            var geomElem = element.Element(nameof(Meshes));

            if (geomElem != null)
            {
                foreach (var elem in geomElem.Elements(ModelMeshReference.NODE_NAME))
                {
                    var mesh = new ModelMeshReference();
                    mesh.LoadFromXml(elem);
                    Meshes.Add(mesh);
                }
            }
        }

        public static SurfaceComponent FromXml(XElement element)
        {
            SurfaceComponent component = null;
            var componentType = element.ReadAttribute<ModelComponentType>("Type");
            switch (componentType)
            {
                case ModelComponentType.Part:
                    component = new PartModel();
                    break;
                case ModelComponentType.MaleStud:
                    component = new MaleStudModel();
                    break;
                case ModelComponentType.FemaleStud:
                    component = new FemaleStudModel();
                    break;
                case ModelComponentType.BrickTube:
                    component = new BrickTubeModel();
                    break;
            }

            if (component != null)
                component.LoadFromXml(element);

            return component;
        }

        #endregion
    }
}
