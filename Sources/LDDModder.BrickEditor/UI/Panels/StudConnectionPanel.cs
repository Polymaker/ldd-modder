using LDDModder.BrickEditor.ProjectHandling;
using LDDModder.LDD.Primitives.Connectors;
using LDDModder.Modding.Editing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LDDModder.BrickEditor.UI.Panels
{
    public partial class StudConnectionPanel : ProjectDocumentPanel
    {
        private SortableBindingList<PartConnection> Connections;

        public StudConnectionPanel()
        {
            InitializeComponent();
            Initialize();
        }

        public StudConnectionPanel(ProjectManager projectManager) : base(projectManager)
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            Connections = new SortableBindingList<PartConnection>();
            ConnectorComboBox.ComboBox.DataSource = Connections;
            ConnectorComboBox.ComboBox.DisplayMember = "Name";
            ConnectorComboBox.ComboBox.ValueMember = "ID";
            CloseButtonVisible = false;
            studConnectionEditor1.AssignManager(ProjectManager);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            FillSelectedConnector(false);
            //studConnectionEditor1.GridEditor.DataChanged += GridEditor_DataChanged;
            studConnectionEditor1.GridEditor.ConnectorSizeChanged += GridEditor_ConnectorSizeChanged;
        }

        private void GridEditor_ConnectorSizeChanged(object sender, EventArgs e)
        {
            if (DockState == WeifenLuo.WinFormsUI.Docking.DockState.Float)
                ProjectManager.ViewportWindow.ForceRender();
        }

        //private void GridEditor_DataChanged(object sender, EventArgs e)
        //{
        //    if (DockState == WeifenLuo.WinFormsUI.Docking.DockState.Float)
        //        ProjectManager.ViewportWindow.ForceRender();
        //}

        protected override void OnProjectChanged()
        {
            base.OnProjectChanged();
            UpdateConnectorList(true);
            //FillSelectedConnector(false);
        }

        protected override void OnElementCollectionChanged(ElementCollectionChangedEventArgs e)
        {
            base.OnElementCollectionChanged(e);

            if (e.ElementType == typeof(PartConnection) &&
                e.ChangedElements.OfType<PartConnection>().Any(x => x.ConnectorType == LDD.Primitives.Connectors.ConnectorType.Custom2DField))
            {
                UpdateConnectorList(false);
            }
        }

        #region ConnectorComboBox

        private void UpdateConnectorList(bool rebuild)
        {

            string prevSelectedID = ConnectorComboBox.ComboBox.SelectedValue as string;

            using (FlagManager.UseFlag("UpdateConnectorList"))
            {
                if (rebuild)
                    Connections.Clear();

                if (CurrentProject != null)
                {
                    var studConnectors = CurrentProject.GetAllElements<PartConnection>(x =>
                            x.ConnectorType == LDD.Primitives.Connectors.ConnectorType.Custom2DField);

                    if (rebuild)
                        Connections.AddRange(studConnectors);
                    else
                        Connections.SyncItems(studConnectors);
                }
            }

            string currentSelectedID = ConnectorComboBox.ComboBox.SelectedValue as string;
            if (prevSelectedID != currentSelectedID)
                FillSelectedConnector(false);
        }

        private void ConnectorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillSelectedConnector(true);
        }

        #endregion

        protected override void OnElementSelectionChanged()
        {
            base.OnElementSelectionChanged();
            ExecuteOnThread(() =>
            {
                if (SyncSelectionCheckBox.Checked)
                    SyncToCurrentSelection();
            });
        }

        private void SyncSelectionCheckBox_Click(object sender, EventArgs e)
        {
            if (SyncSelectionCheckBox.Checked)
                SyncToCurrentSelection();
        }

        private void SyncToCurrentSelection()
        {
            var selectedConnectors = ProjectManager.GetSelectionHierarchy()
                .OfType<PartConnection>().Where(x => x.ConnectorType == ConnectorType.Custom2DField);

            if (selectedConnectors.Count() == 1)
            {
                using(FlagManager.UseFlag("SyncToCurrentSelection"))
                    ConnectorComboBox.SelectedItem = selectedConnectors.FirstOrDefault();
            }
        }

        private void FillSelectedConnector(bool fromCombo)
        {
            if (FlagManager.IsSet("UpdateConnectorList"))
                return;

            if (ConnectorComboBox.SelectedItem is PartConnection connection)
            {
                studConnectionEditor1.StudConnector = connection.GetConnector<Custom2DFieldConnector>();
                studConnectionEditor1.Enabled = true;

                if (SyncSelectionCheckBox.Checked && fromCombo && !FlagManager.IsSet("SyncToCurrentSelection"))
                {
                    ProjectManager.SelectElement(connection);
                }
            }
            else
            {
                studConnectionEditor1.StudConnector = null;
                studConnectionEditor1.Enabled = false;
            }

            
        }

        protected override void OnDockStateChanged(EventArgs e)
        {
            base.OnDockStateChanged(e);
            if (DockState == WeifenLuo.WinFormsUI.Docking.DockState.Float)
                CloseButtonVisible = true;
            else
                CloseButtonVisible = false;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (DockState == WeifenLuo.WinFormsUI.Docking.DockState.Float)
            {
                e.Cancel = true;
                Show(DockHandler.DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document);
            }
            base.OnClosing(e);
        }
    }
}
