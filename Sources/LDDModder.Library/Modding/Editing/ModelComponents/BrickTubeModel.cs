using LDDModder.LDD.Meshes;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using System.Diagnostics;

namespace LDDModder.Modding.Editing
{
    public class BrickTubeModel : PartCullingModel
    {
        public override ModelComponentType ComponentType => ModelComponentType.BrickTube;

        private StudReference _TubeStud;

        public StudReference TubeStud
        {
            get => _TubeStud;
            set => SetPropertyValue(ref _TubeStud, value);
        }

        public ElementCollection<StudReference> AdjacentStuds { get; set; }

        public BrickTubeModel()
        {
            AdjacentStuds = new ElementCollection<StudReference>(this);
        }

        internal override void LoadCullingInformation(MeshCulling culling)
        {
            base.LoadCullingInformation(culling);
            if (culling.Studs.Count == 1)
            {
                TubeStud = new StudReference(culling.Studs[0]);
            }
            else
                Debug.WriteLine("Tube culling does not reference a stud!");

            if (culling.AdjacentStuds.Any())
            {
                foreach (var fIdx in culling.AdjacentStuds[0].FieldIndices)
                {
                    var studRef = new StudReference(
                        culling.AdjacentStuds[0].ConnectorIndex,
                        fIdx.Index, fIdx.Value2, fIdx.Value4
                    );
                    AdjacentStuds.Add(studRef);
                }
            }
        }

        public override XElement SerializeToXml()
        {
            var elem = base.SerializeToXml();
            if (TubeStud != null)
                elem.Add(TubeStud.SerializeToXml());

            elem.Add(new XComment("The following 4 studs are adjacent to the tube"));
            var studsElem = elem.AddElement("AdjacentStuds");
            foreach (var stud in AdjacentStuds)
                studsElem.Add(stud.SerializeToXml());
            return elem;
        }

        protected internal override void LoadFromXml(XElement element)
        {
            base.LoadFromXml(element);
            if (element.Element(StudReference.NODE_NAME) != null)
                TubeStud = StudReference.FromXml(element.Element(StudReference.NODE_NAME));

            if (element.Element("AdjacentStuds") != null)
            {
                foreach (var studElem in element.Element("AdjacentStuds").Elements(StudReference.NODE_NAME))
                    AdjacentStuds.Add(StudReference.FromXml(studElem));
            }
        }
    }
}
