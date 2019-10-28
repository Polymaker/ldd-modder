using LDDModder.LDD.Meshes;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace LDDModder.Modding.Editing
{
    public class MaleStudModel : PartCullingModel
    {
        public override ModelComponentType ComponentType => ModelComponentType.MaleStud;

        public StudReference Stud
        {
            get => StudReferences.FirstOrDefault();
            set
            {
                if (StudReferences.Any() && StudReferences[0] != value)
                    StudReferences.Clear();

                if (value != null)
                    StudReferences.Add(value);
            }
        }

        public override IEnumerable<StudReference> GetStudReferences()
        {
            if (Stud != null)
                yield return Stud;
        }

        public override XElement SerializeToXml()
        {
            var elem = base.SerializeToXml();
            if (Stud != null)
                elem.Add(Stud.SerializeToXml());

            return elem;
        }

        protected internal override void LoadFromXml(XElement element)
        {
            base.LoadFromXml(element);
            if (element.HasElement(StudReference.NODE_NAME, out XElement studElem))
                Stud = StudReference.FromXml(studElem);
        }
    }
}
