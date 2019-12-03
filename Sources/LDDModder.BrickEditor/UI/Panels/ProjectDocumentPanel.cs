using LDDModder.BrickEditor.ProjectHandling;
using LDDModder.Modding.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeifenLuo.WinFormsUI.Docking;

namespace LDDModder.BrickEditor.UI.Panels
{
    public /*abstract*/ class ProjectDocumentPanel : DockContent
    {
        public ProjectManager ProjectManager { get; }

        public PartProject CurrentProject => ProjectManager?.CurrentProject;

        public ProjectDocumentPanel()
        {

        }

        protected ProjectDocumentPanel(ProjectManager projectManager)
        {
            ProjectManager = projectManager;

            ProjectManager.ProjectClosed += ProjectManager_ProjectClosed;
            ProjectManager.ProjectChanged += ProjectManager_ProjectChanged;
            ProjectManager.ElementCollectionChanged += ProjectManager_ElementCollectionChanged;
            ProjectManager.ProjectElementsChanged += ProjectManager_ProjectElementsChanged;
            ProjectManager.ElementPropertyChanged += ProjectManager_ElementPropertyChanged;
            ProjectManager.SelectionChanged += ProjectManager_SelectionChanged;
        }

        private void ProjectManager_ProjectClosed(object sender, EventArgs e)
        {
            OnProjectClosed();
        }

        protected virtual void OnProjectClosed() { }

        private void ProjectManager_ProjectChanged(object sender, EventArgs e)
        {
            OnProjectChanged();
            if (ProjectManager.CurrentProject != null)
                OnProjectLoaded(ProjectManager.CurrentProject);
        }

        protected virtual void OnProjectChanged() { }

        protected virtual void OnProjectLoaded(PartProject project)
        {

        }

        private void ProjectManager_ElementCollectionChanged(object sender, ElementCollectionChangedEventArgs e)
        {
            //if (!(ProjectManager.IsExecutingBatchChanges || ProjectManager.IsExecutingUndoRedo))
                OnElementCollectionChanged(e);
        }

        protected virtual void OnElementCollectionChanged(ElementCollectionChangedEventArgs e)
        {

        }

        private void ProjectManager_ProjectElementsChanged(object sender, EventArgs e)
        {
            OnProjectElementsChanged();
        }

        protected virtual void OnProjectElementsChanged()
        {

        }

        private void ProjectManager_ElementPropertyChanged(object sender, ElementValueChangedEventArgs e)
        {
            OnElementPropertyChanged(e);
        }

        protected virtual void OnElementPropertyChanged(ElementValueChangedEventArgs e)
        {

        }

        private void ProjectManager_SelectionChanged(object sender, EventArgs e)
        {
            OnElementSelectionChanged();
        }

        protected virtual void OnElementSelectionChanged()
        {

        }
    }
}
