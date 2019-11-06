using LDDModder.Modding.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.EditModels
{
    public class ProjectManager
    {
        public PartProject CurrentProject { get; private set; }

        public bool IsProjectOpen => CurrentProject != null;

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

        public event EventHandler ProjectClosed;

        public event EventHandler ProjectChanged;

        public event EventHandler<CollectionChangedEventArgs> ProjectElementsChanged;

        public ProjectManager()
        {
            
        }

        public void SetCurrentProject(PartProject project)
        {
            if (CurrentProject != project)
            {
                CloseCurrentProject();

                CurrentProject = project;

                if (project != null)
                    AttachPartProject(project);

                ProjectChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public void CloseCurrentProject()
        {
            if (CurrentProject != null)
            {
                DettachPartProject(CurrentProject);
                ProjectClosed?.Invoke(this, EventArgs.Empty);
                CurrentProject = null;
            }
        }

        private void AttachPartProject(PartProject project)
        {
            project.ElementCollectionChanged += Project_ElementCollectionChanged;
        }

        private void DettachPartProject(PartProject project)
        {
            project.ElementCollectionChanged -= Project_ElementCollectionChanged;
        }

        private void Project_ElementCollectionChanged(object sender, CollectionChangedEventArgs e)
        {
            //if (e.Action == System.ComponentModel.CollectionChangeAction.Add)
            //{
            //    foreach (var elem in e.AddedElements)
            //        OnProjectElementAdded(elem);
            //}
            //else if (e.Action == System.ComponentModel.CollectionChangeAction.Remove)
            //{
            //    foreach (var elem in e.RemovedElements)
            //        OnProjectElementRemoved(elem);
            //}

            ProjectElementsChanged?.Invoke(this, e);
        }
    }
}
