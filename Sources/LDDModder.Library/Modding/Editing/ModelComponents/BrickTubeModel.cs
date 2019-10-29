using LDDModder.LDD.Meshes;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;

namespace LDDModder.Modding.Editing
{
    public class BrickTubeModel : PartCullingModel
    {
        public override ModelComponentType ComponentType => ModelComponentType.BrickTube;

        private StudReference _TubeStud;

        public StudReference TubeStud
        {
            get => _TubeStud;
            set
            {
                if (_TubeStud != value)
                {
                    if (_TubeStud != null)
                        StudReferences.Remove(_TubeStud);

                    if (value != null)
                        StudReferences.Add(value);

                    _TubeStud = value;
                }
            }
        }

        public ElementCollection<StudReference> AdjacentStuds { get; set; }

        public BrickTubeModel()
        {
            AdjacentStuds = new ElementCollection<StudReference>(this);
        }

        public override IEnumerable<StudReference> GetStudReferences()
        {
            if (TubeStud != null)
                yield return TubeStud;
            foreach (var stud in AdjacentStuds)
                yield return stud;
        }

        protected override IEnumerable<PartElement> GetAllChilds()
        {
            return base.GetAllChilds().Concat(AdjacentStuds);
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
