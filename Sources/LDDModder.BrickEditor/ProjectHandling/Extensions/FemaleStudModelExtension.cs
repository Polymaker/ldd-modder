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
                    InvalidateVisibility(true);
                }
            }
        }

        public FemaleStudModelExtension(PartElement element) : base(element)
        {
            _ShowAlternateModels = false;
        }


        protected override bool OverrideChildVisibility(PartElement element)
        {
            if (Element is FemaleStudModel femaleStud)
            {
                if (femaleStud.Meshes.Contains(element))
                    return ShowAlternateModels;
                else if (femaleStud.ReplacementMeshes.Contains(element))
                    return !ShowAlternateModels;
            }
            return base.OverrideChildVisibility(element);
        }
    }
}
