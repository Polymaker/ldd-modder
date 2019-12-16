using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LDDModder.Modding.Editing;

namespace LDDModder.BrickEditor.ProjectHandling
{
    public class StudAltElement : ElementExtention
    {
        private bool _ShowAlternateModels;

        public bool ShowAlternateModels
        {
            get => _ShowAlternateModels;
            set
            {
                if (_ShowAlternateModels != value)
                {
                    _ShowAlternateModels = value;
                    CalculateVisibillity();
                }
            }
        }

        internal StudAltElement(ProjectManager manager, FemaleStudModel element) : base(manager, element)
        {
            _ShowAlternateModels = false;
        }

        protected override void PropagateVisibillity(PartElement element, bool parentIsVisible)
        {
            if (Element is FemaleStudModel femaleStud)
            {
                if (femaleStud.Meshes.Contains(element))
                    parentIsVisible &= !ShowAlternateModels;
                else if (femaleStud.ReplacementMeshes.Contains(element))
                    parentIsVisible &= ShowAlternateModels;
            }

            base.PropagateVisibillity(element, parentIsVisible);
        }
    }
}
