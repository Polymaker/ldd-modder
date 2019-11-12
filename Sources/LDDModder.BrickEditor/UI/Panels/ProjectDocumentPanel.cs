using LDDModder.BrickEditor.EditModels;
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
            ProjectManager.ProjectElementsChanged += ProjectManager_ProjectElementsChanged;
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

        private void ProjectManager_ProjectElementsChanged(object sender, CollectionChangedEventArgs e)
        {
            OnProjectElementsChanged(e);
        }

        protected virtual void OnProjectElementsChanged(CollectionChangedEventArgs e)
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
