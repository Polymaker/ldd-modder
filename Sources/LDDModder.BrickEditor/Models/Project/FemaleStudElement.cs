using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LDDModder.BrickEditor.ProjectHandling;
using LDDModder.Modding.Editing;

namespace LDDModder.BrickEditor.Models.Project
{
    public class FemaleStudElement : RenderableElement
    {
        private bool _DisplayReplacementMesh;

        public bool DisplayReplacementMesh
        {
            get => _DisplayReplacementMesh;
            set
            {
                if (_DisplayReplacementMesh != value)
                {
                    _DisplayReplacementMesh = value;
                    UpdateModelsVisibility();
                    OnPropertyChanged(nameof(DisplayReplacementMesh));
                }
            }
        }

        public FemaleStudElement(ProjectManager manager, PartElement element) : base(manager, element)
        {

        }

        private void UpdateModelsVisibility()
        {
            if (Element is FemaleStudModel femaleStud)
            {
                foreach (var model in femaleStud.Meshes)
                {

                }
            }
        }
    }
}
