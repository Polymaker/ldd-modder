using LDDModder.Modding.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.EditModels
{
    public class ProjectDocument
    {
        public PartProject Project { get; set; }

        private PartElement _SelectedComponent;

        public PartElement SelectedComponent
        {
            get => _SelectedComponent;
            set
            {
                if (value != _SelectedComponent)
                {
                    _SelectedComponent = value;
                    SelectedComponentChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler SelectedComponentChanged;

        public ProjectDocument(PartProject project)
        {
            Project = project;
            Project.ElementPropertyChanged += Project_ComponentPropertyChanged;
        }

        private void Project_ComponentPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            
        }


    }
}
