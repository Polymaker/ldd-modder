﻿using LDDModder.LDD.Meshes;
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

            if (culling.Studs.Count >= 1)
                Stud = new StudReference(culling.Studs[0]);
            else
                Debug.WriteLine("Stud culling does not reference a stud!");
        }

        internal override void FillCullingInformation(MeshCulling culling)
        {
            if (Stud != null)
                culling.Studs.Add(GetFieldReference(Stud));
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
