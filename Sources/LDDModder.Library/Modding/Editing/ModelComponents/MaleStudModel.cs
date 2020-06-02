using LDDModder.LDD.Meshes;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;

namespace LDDModder.Modding.Editing
{
    public class MaleStudModel : PartCullingModel
    {
        public override ModelComponentType ComponentType => ModelComponentType.MaleStud;

        private StudReference _Stud;

        public StudReference Stud
        {
            get => _Stud;
            set => SetPropertyValue(ref _Stud, value);
        }

        internal override void LoadCullingInformation(MeshCulling culling)
        {
            base.LoadCullingInformation(culling);

            var referencedStuds = ConvertFromRefs(culling.Studs).ToList();
            Stud = referencedStuds.FirstOrDefault();

            if (referencedStuds.Count > 1)
                Debug.WriteLine("Stud culling references more than one stud!");
            else if(referencedStuds.Count == 0)
                Debug.WriteLine("Stud culling does not reference a stud!");
        }

        internal override void FillCullingInformation(MeshCulling culling)
        {
            if (Stud != null)
                culling.Studs.Add(ConvertToRef(Stud));
        }

        public override XElement SerializeToXml()
        {
            var elem = base.SerializeToXml();

            if (Stud != null)
                elem.Add(Stud.SerializeToXml2());

            return elem;
        }

        protected internal override void LoadFromXml(XElement element)
        {
            base.LoadFromXml(element);
            if (element.HasElement(StudReference.NODE_NAME, out XElement studElem))
                Stud = StudReference.FromXml(studElem);

            if (!string.IsNullOrEmpty(LegacyConnectionID) && Stud != null)
                Stud.ConnectionID = LegacyConnectionID;
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

            if (Stud == null)
                AddMessage("MODEL_STUDS_NOT_DEFINED", ValidationLevel.Warning);
            else
                messages.AddRange(Stud.ValidateElement());

            return messages;
        }
    }
}
