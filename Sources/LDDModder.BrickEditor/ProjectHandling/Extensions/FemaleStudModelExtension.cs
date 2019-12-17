using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LDDModder.Modding.Editing;

namespace LDDModder.BrickEditor.ProjectHandling
{
    public class FemaleStudModelExtension : ModelElementExtension
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
                    CalculateVisibility();
                }
            }
        }

        internal FemaleStudModelExtension(ProjectManager manager, FemaleStudModel element) : base(manager, element)
        {
            _ShowAlternateModels = false;
        }

        public FemaleStudModelExtension(PartElement element) : base(element)
        {
            _ShowAlternateModels = false;
        }

        protected override void PropagateVisibility(PartElement element, bool parentIsVisible)
        {
            if (Element is FemaleStudModel femaleStud)
            {
                if (femaleStud.Meshes.Contains(element))
                    parentIsVisible &= !ShowAlternateModels;
                else if (femaleStud.ReplacementMeshes.Contains(element))
                    parentIsVisible &= ShowAlternateModels;
            }

            base.PropagateVisibility(element, parentIsVisible);
        }
    }
}
