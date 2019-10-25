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

        public ProjectDocument(PartProject project)
        {
            Project = project;
            Project.ComponentPropertyChanged += Project_ComponentPropertyChanged;
        }

        private void Project_ComponentPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            
        }
    }
}
