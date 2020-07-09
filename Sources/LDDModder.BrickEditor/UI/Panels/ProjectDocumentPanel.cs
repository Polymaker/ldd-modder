using LDDModder.BrickEditor.ProjectHandling;
using LDDModder.Modding.Editing;
using LDDModder.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace LDDModder.BrickEditor.UI.Panels
{
    public /*abstract*/ class ProjectDocumentPanel : DockContent
    {
        public ProjectManager ProjectManager { get; private set; }

        public PartProject CurrentProject => ProjectManager?.CurrentProject;

        protected FlagManager FlagManager { get; }

        public ProjectDocumentPanel()
        {
            FlagManager = new FlagManager();
        }

        protected ProjectDocumentPanel(ProjectManager projectManager)
        {
            FlagManager = new FlagManager();
            InitializeProjectManager(projectManager);
        }

        protected virtual void InitializeProjectManager(ProjectManager projectManager)
        {
            ProjectManager = projectManager;

            ProjectManager.ProjectClosed += ProjectManager_ProjectClosed;
            ProjectManager.ProjectChanged += ProjectManager_ProjectChanged;
            ProjectManager.ElementCollectionChanged += ProjectManager_ElementCollectionChanged;
            ProjectManager.ProjectElementsChanged += ProjectManager_ProjectElementsChanged;
            ProjectManager.ElementPropertyChanged += ProjectManager_ElementPropertyChanged;
            ProjectManager.SelectionChanged += ProjectManager_SelectionChanged;
            ProjectManager.UndoHistoryChanged += ProjectManager_UndoHistoryChanged;
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

        private void ProjectManager_UndoHistoryChanged(object sender, EventArgs e)
        {
            OnProjectChangeApplied();
        }

        protected virtual void OnProjectChangeApplied()
        {

        }

        protected void ToggleControlsEnabled(bool enabled, params System.Windows.Forms.Control[] controls)
        {
            foreach (var ctrl in controls)
                ctrl.Enabled = enabled;
        }

        protected void ExecuteOnThread(Action action)
        {
            if (InvokeRequired)
                BeginInvoke(action);
            else
                action();
        }

        protected void SetControlDoubleBuffered(Control control)
        {
            control.GetType().InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic, null,
            control, new object[] { true });
        }
    }
}
