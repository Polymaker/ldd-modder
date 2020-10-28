using LDDModder.BrickEditor.ProjectHandling;
using LDDModder.Modding.Editing;
using LDDModder.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
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

        public Windows.BrickEditorWindow EditorWindow => DockPanel?.Parent as Windows.BrickEditorWindow;

        public ProjectDocumentPanel()
        {
            FlagManager = new FlagManager();
        }

        protected ProjectDocumentPanel(ProjectManager projectManager)
        {
            FlagManager = new FlagManager();
            InitializeProjectManager(projectManager);
        }

        protected override void OnDockStateChanged(EventArgs e)
        {
            base.OnDockStateChanged(e);

            if (DockState == DockState.DockLeft || DockState == DockState.DockLeftAutoHide)
            {
                BackColor = Color.FromArgb(232, 236, 239);
            }
            else
            {
                BackColor = Color.White;
            }

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

        private void DettachProjectManager()
        {
            if (ProjectManager == null)
                return;

            ProjectManager.ProjectClosed -= ProjectManager_ProjectClosed;
            ProjectManager.ProjectChanged -= ProjectManager_ProjectChanged;
            ProjectManager.ElementCollectionChanged -= ProjectManager_ElementCollectionChanged;
            ProjectManager.ProjectElementsChanged -= ProjectManager_ProjectElementsChanged;
            ProjectManager.ElementPropertyChanged -= ProjectManager_ElementPropertyChanged;
            ProjectManager.SelectionChanged -= ProjectManager_SelectionChanged;
            ProjectManager.UndoHistoryChanged -= ProjectManager_UndoHistoryChanged;

            ProjectManager = null;
        }

        public virtual void DefferedInitialization()
        {

        }

        public virtual async Task InitializeAsync()
        {
            await Task.Delay(0);
        }

        public virtual void OnInitializationFinished()
        {

        }

        protected override void OnClosed(EventArgs e)
        {
            DettachProjectManager();
            base.OnClosed(e);
        }

        private void ProjectManager_ProjectClosed(object sender, EventArgs e)
        {
            OnProjectClosed();
        }

        protected virtual void OnProjectClosed() { }

        private void ProjectManager_ProjectChanged(object sender, EventArgs e)
        {
            if (ProjectManager.CurrentProject != null)
                OnProjectLoaded(ProjectManager.CurrentProject);
            OnProjectChanged();
        }

        protected virtual void OnProjectChanged() { }

        protected virtual void OnProjectLoaded(PartProject project)
        {

        }

        private void ProjectManager_ElementCollectionChanged(object sender, ElementCollectionChangedEventArgs e)
        {
            OnElementCollectionChanged(e);
        }

        /// <summary>
        /// Raised when a collection in the project has changed.
        /// </summary>
        /// <remarks>Not always invoked on main thread</remarks>
        /// <param name="e"></param>
        protected virtual void OnElementCollectionChanged(ElementCollectionChangedEventArgs e)
        {

        }

        private void ProjectManager_ProjectElementsChanged(object sender, EventArgs e)
        {
            OnProjectElementsChanged();
        }

        /// <summary>
        /// Raised when one or more collections in the project has changed. <br/>
        /// Raised only after a all changes are applied
        /// </summary>
        /// <remarks>Not always invoked on main thread</remarks>
        protected virtual void OnProjectElementsChanged()
        {

        }

        private void ProjectManager_ElementPropertyChanged(object sender, ElementValueChangedEventArgs e)
        {
            OnElementPropertyChanged(e);
        }

        /// <summary>
        /// Raised when a property of a project element has changed. <br/>
        /// </summary>
        /// <remarks>Not always invoked on main thread</remarks>
        protected virtual void OnElementPropertyChanged(ElementValueChangedEventArgs e)
        {

        }


        private void ProjectManager_SelectionChanged(object sender, EventArgs e)
        {
            BeginInvokeOnce(OnElementSelectionChanged, nameof(OnElementSelectionChanged));
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

        protected void BeginInvokeOnce(Action action, string actionName)
        {
            if (InvokeRequired)
            {
                if (!FlagManager.IsSet($"Invoke{actionName}"))
                {
                    FlagManager.Set($"Invoke{actionName}");
                    BeginInvoke((Action)(() =>
                    {
                        action();
                        FlagManager.Unset($"Invoke{actionName}");
                    }));
                }
            }
            else
                action();
        }

        protected void SetControlDoubleBuffered(Control control)
        {
            control.GetType().InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic, null,
            control, new object[] { true });
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ProjectDocumentPanel
            // 
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(279, 264);
            this.Name = "ProjectDocumentPanel";
            this.ResumeLayout(false);

        }
    }
}
