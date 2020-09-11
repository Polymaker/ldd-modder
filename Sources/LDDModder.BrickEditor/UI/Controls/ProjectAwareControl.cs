using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LDDModder.BrickEditor.ProjectHandling;
using LDDModder.Modding.Editing;
using LDDModder.Utilities;

namespace LDDModder.BrickEditor.UI.Controls
{
    public partial class ProjectAwareControl : UserControl
    {
        public ProjectManager ProjectManager { get; private set; }

        public PartProject CurrentProject => ProjectManager?.CurrentProject;

        protected FlagManager FlagManager { get; }

        public ProjectAwareControl()
        {
            InitializeComponent();
            FlagManager = new FlagManager();
        }

        #region Project Manager Events

        public void AssignManager(ProjectManager projectManager)
        {
            if (ProjectManager != null)
            {
                ProjectManager.ProjectClosed -= ProjectManager_ProjectClosed;
                ProjectManager.ProjectChanged -= ProjectManager_ProjectChanged;
                //ProjectManager.ElementCollectionChanged -= ProjectManager_ElementCollectionChanged;
                //ProjectManager.ProjectElementsChanged -= ProjectManager_ProjectElementsChanged;
                //ProjectManager.ElementPropertyChanged -= ProjectManager_ElementPropertyChanged;
                ProjectManager.SelectionChanged -= ProjectManager_SelectionChanged;
                //ProjectManager.UndoHistoryChanged -= ProjectManager_UndoHistoryChanged;
            }


            ProjectManager = projectManager;

            if (ProjectManager != null)
            {
                ProjectManager.ProjectClosed += ProjectManager_ProjectClosed;
                ProjectManager.ProjectChanged += ProjectManager_ProjectChanged;
                //ProjectManager.ElementCollectionChanged += ProjectManager_ElementCollectionChanged;
                //ProjectManager.ProjectElementsChanged += ProjectManager_ProjectElementsChanged;
                //ProjectManager.ElementPropertyChanged += ProjectManager_ElementPropertyChanged;
                ProjectManager.SelectionChanged += ProjectManager_SelectionChanged;
                //ProjectManager.UndoHistoryChanged += ProjectManager_UndoHistoryChanged;
            }

        }

        private void ProjectManager_ProjectClosed(object sender, EventArgs e)
        {
            ExecuteOnThread(OnProjectClosed);
        }

        protected virtual void OnProjectClosed() { }

        private void ProjectManager_ProjectChanged(object sender, EventArgs e)
        {
            ExecuteOnThread(() =>
            {
                OnProjectChanged();
                if (ProjectManager.CurrentProject != null)
                    OnProjectLoaded(ProjectManager.CurrentProject);
            });
        }

        protected virtual void OnProjectChanged() { }

        protected virtual void OnProjectLoaded(PartProject project)
        {

        }

        private void ProjectManager_SelectionChanged(object sender, EventArgs e)
        {
            BeginInvokeOnce(OnElementSelectionChanged, nameof(OnElementSelectionChanged));
        }

        protected virtual void OnElementSelectionChanged()
        {

        }

        #endregion


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
    }
}
