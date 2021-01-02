using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LDDModder.Modding.Serializers
{
    public abstract class ProjectSerializerBase
    {

        public abstract int ProjectFileVersion { get; }

        public PartProject Project { get; }

        public ProjectSerializerBase(PartProject project)
        {
            Project = project;
        }

        public abstract PartSurface DeserializeSurface(XElement element);
        public abstract XElement SerializeSurface(PartSurface surface);

        public abstract SurfaceComponent DeserializeSurfaceComponent(XElement element);
        protected abstract void DeserializePartModel(PartModel partModel, XElement element);

        protected abstract void DeserializePartCullingModel(PartCullingModel partCullingModel, XElement element);
        protected abstract void DeserializeBrickTubeModel(BrickTubeModel brickTubeModel, XElement element);
        protected abstract void DeserializeMaleStudModel(MaleStudModel maleStudModel, XElement element);
        protected abstract void DeserializeFemaleStudModel(FemaleStudModel femaleStudModel, XElement element);

        public abstract XElement SerializeSurfaceComponent(SurfaceComponent surfaceComponent);
    }
}
