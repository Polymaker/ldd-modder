using LDDModder.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LDDModder.Modding.Serializers
{
    public class ProjectSerializerV1 : ProjectSerializerBase
    {
        public override int ProjectFileVersion => 1;

        public ProjectSerializerV1(PartProject project) : base(project)
        {

        }

        private void DeserializeBaseElement(PartElement element, XElement xml)
        {
            if (xml.HasAttribute("ID", out XAttribute idAttr))
                element.ID = idAttr.Value;

            if (xml.HasAttribute("Name", out XAttribute nameAttr))
                element.InternalSetName(nameAttr.Value);
        }

        protected XElement SerializeBaseElement(PartElement element, string elementName)
        {
            var elem = new XElement(elementName);

            if (!string.IsNullOrEmpty(element.ID))
                elem.Add(new XAttribute("ID", element.ID));

            if (!string.IsNullOrEmpty(element.Name))
                elem.Add(new XAttribute("Name", element.Name));

            return elem;
        }


        #region Surface

        public override PartSurface DeserializeSurface(XElement element)
        {
            var surface = new PartSurface();
            //DeserializeBaseElement(surface, element);

            if (element.TryGetIntAttribute(nameof(PartSurface.SurfaceID), out int surfID))
                surface.SurfaceID = surfID;

            surface.InternalSetID(StringUtils.GenerateUUID($"Surface{surfID}", 8));
            surface.InternalSetName($"Surface{surfID}");

            if (element.TryGetIntAttribute(nameof(PartSurface.SubMaterialIndex), out int matIDX))
                surface.SubMaterialIndex = matIDX;

            foreach (var compElem in element.Elements(SurfaceComponent.NODE_NAME))
            {
                var comp = DeserializeSurfaceComponent(compElem);
                if (comp != null)
                    surface.Components.Add(comp);
            }

            return null;
        }

        public override XElement SerializeSurface(PartSurface surface)
        {
            var elem = SerializeBaseElement(surface, PartSurface.NODE_NAME);
            elem.RemoveAttributes();

            elem.Add(new XAttribute(nameof(PartSurface.SurfaceID), surface.SurfaceID));
            elem.Add(new XAttribute(nameof(PartSurface.SubMaterialIndex), surface.SubMaterialIndex));

            foreach (var comp in surface.Components)
            {
                var compXml = SerializeSurfaceComponent(comp);
                if (compXml != null)
                    elem.Add(compXml);
            }
            return null;
        }

        #endregion

        #region SurfaceComponent

        public override SurfaceComponent DeserializeSurfaceComponent(XElement element)
        {
            SurfaceComponent component = null;
            var componentType = element.ReadAttribute<ModelComponentType>("Type");

            switch (componentType)
            {
                case ModelComponentType.Part:
                    component = new PartModel();
                    DeserializePartModel(component as PartModel, element);
                    break;
                case ModelComponentType.MaleStud:
                    component = new MaleStudModel();
                    DeserializeMaleStudModel(component as MaleStudModel, element);
                    break;
                case ModelComponentType.FemaleStud:
                    component = new FemaleStudModel();
                    DeserializeFemaleStudModel(component as FemaleStudModel, element);
                    break;
                case ModelComponentType.BrickTube:
                    component = new BrickTubeModel();
                    DeserializeBrickTubeModel(component as BrickTubeModel, element);
                    break;
            }

            if (element.HasElement(nameof(SurfaceComponent.Meshes), out XElement geomElem))
            {
                foreach (var elem in geomElem.Elements(ModelMeshReference.NODE_NAME))
                {
                    var mesh = new ModelMeshReference();
                    mesh.LoadFromXml(elem);
                    component.Meshes.Add(mesh);
                }
            }

            return component;
        }

        protected override void DeserializePartModel(PartModel partModel, XElement element)
        {
            DeserializeBaseElement(partModel, element);
        }

        protected override void DeserializePartCullingModel(PartCullingModel partCullingModel, XElement element)
        {
            DeserializeBaseElement(partCullingModel, element);

            partCullingModel.LegacyConnectionID = element.ReadAttribute("ConnectionID", string.Empty);

            partCullingModel.ReferencedStuds.Clear();

            if (element.HasElement(nameof(PartCullingModel.ReferencedStuds), out XElement studsElem))
            {
                foreach (var studElem in studsElem.Elements(StudReference.NODE_NAME))
                {
                    var studRef = StudReference.FromXml(studElem);
                    if (!string.IsNullOrEmpty(partCullingModel.LegacyConnectionID))
                        studRef.ConnectionID = partCullingModel.LegacyConnectionID;
                    partCullingModel.ReferencedStuds.Add(studRef);
                }
            }
        }

        protected override void DeserializeBrickTubeModel(BrickTubeModel brickTubeModel, XElement element)
        {
            DeserializePartCullingModel(brickTubeModel, element);

            //LEGACY
            if (element.HasElement(StudReference.NODE_NAME, out XElement tubeStudElem))
            {
                var studRef = StudReference.FromXml(tubeStudElem);
                if (!string.IsNullOrEmpty(brickTubeModel.LegacyConnectionID))
                    studRef.ConnectionID = brickTubeModel.LegacyConnectionID;
                brickTubeModel.ReferencedStuds.Add(studRef);
                //TubeStud = StudReference.FromXml(tubeStudElem);
                //if (!string.IsNullOrEmpty(LegacyConnectionID))
                //    TubeStud.ConnectionID = LegacyConnectionID;
            }

            brickTubeModel.TubeStud = brickTubeModel.ReferencedStuds.FirstOrDefault();

            brickTubeModel.AdjacentStuds.Clear();
            if (element.HasElement(nameof(BrickTubeModel.AdjacentStuds), out XElement adjStudsElem))
            {
                foreach (var adjStudElem in adjStudsElem.Elements(StudReference.NODE_NAME))
                {
                    var studRef = StudReference.FromXml(adjStudElem);
                    if (!string.IsNullOrEmpty(brickTubeModel.LegacyConnectionID))
                        studRef.ConnectionID = brickTubeModel.LegacyConnectionID;
                    brickTubeModel.AdjacentStuds.Add(studRef);
                }
            }
        }

        protected override void DeserializeMaleStudModel(MaleStudModel maleStudModel, XElement element)
        {
            DeserializePartCullingModel(maleStudModel, element);
        }

        protected override void DeserializeFemaleStudModel(FemaleStudModel femaleStudModel, XElement element)
        {
            DeserializePartCullingModel(femaleStudModel, element);

            if (element.HasElement(nameof(FemaleStudModel.ReplacementMeshes), out XElement geomElem))
            {
                foreach (var elem in geomElem.Elements(ModelMeshReference.NODE_NAME))
                {
                    var mesh = new ModelMeshReference();
                    mesh.LoadFromXml(elem);
                    femaleStudModel.ReplacementMeshes.Add(mesh);
                }
            }
        }

        public override XElement SerializeSurfaceComponent(SurfaceComponent surfaceComponent)
        {
            var elem = SerializeBaseElement(surfaceComponent, SurfaceComponent.NODE_NAME);
            elem.Add(new XAttribute("Type", surfaceComponent.ComponentType.ToString()));

            var geomElem = elem.AddElement(nameof(SurfaceComponent.Meshes));

            foreach (var geom in surfaceComponent.Meshes)
                geomElem.Add(geom.SerializeToXml());
            return elem;
        }



        #endregion




    }
}
