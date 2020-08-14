using LDDModder.BrickEditor.ProjectHandling;
using LDDModder.BrickEditor.Resources;
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
    public partial class ConnectionEditorPanel : ProjectDocumentPanel
    {
        private SortableBindingList<PartConnection> Connections;
        private SortableBindingList<ConnectorInfo> SubTypeList { get; set; }
        //private PartConnection _SelectedElement;

        public PartConnection SelectedElement { get; private set; }

        public ConnectionEditorPanel()
        {
            InitializeComponent();
            Initialize();
        }

        public ConnectionEditorPanel(ProjectManager projectManager) : base(projectManager)
        {
            InitializeComponent();
            Initialize();
        }

        #region Initialization

        private void Initialize()
        {
            Connections = new SortableBindingList<PartConnection>();
            SubTypeList = new SortableBindingList<ConnectorInfo>();
            SubTypeList.ApplySort("SubType", ListSortDirection.Ascending);

            ElementsComboBox.ComboBox.DataSource = Connections;
            ElementsComboBox.ComboBox.DisplayMember = "Name";
            ElementsComboBox.ComboBox.ValueMember = "ID";
            CloseButtonVisible = false;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            FindNameForFunction(null, false);
            AddConnectionDropDown.Enabled = false;
            BuildAddMenu();
        }

        private void BuildAddMenu()
        {
            foreach(ConnectorType connectorType in Enum.GetValues(typeof(ConnectorType)))
            {
                if (connectorType == ConnectorType.Custom2DField)
                    continue;

                string menuText = ModelLocalizations.ResourceManager.GetString($"ConnectorType_{connectorType}");
                menuText = menuText.Replace("&", "&&");
                var addMenuItem = AddConnectionDropDown.DropDownItems.Add(menuText);
                addMenuItem.Tag = connectorType;
                addMenuItem.Click += AddMenuItem_Click;
            }
        }

        private void AddMenuItem_Click(object sender, EventArgs e)
        {
            var menu = sender as ToolStripItem;
            var newConnection = ProjectManager.AddConnection((ConnectorType)menu.Tag, null);
            FindNameForFunction(newConnection, false);
        }

        #endregion


        #region Project events

        protected override void OnProjectChanged()
        {
            base.OnProjectChanged();
            UpdateElementList(true);
            AddConnectionDropDown.Enabled = CurrentProject != null;
        }

        protected override void OnElementCollectionChanged(ElementCollectionChangedEventArgs e)
        {
            base.OnElementCollectionChanged(e);

            if (e.ElementType == typeof(PartConnection))
                UpdateElementList(false);
        }

        protected override void OnElementSelectionChanged()
        {
            base.OnElementSelectionChanged();
            ExecuteOnThread(() =>
            {
                if (SyncSelectionCheckBox.Checked)
                    SyncToCurrentSelection();
            });
        }

        #endregion

        #region Elements Combobox

        private void UpdateElementList(bool rebuild)
        {

            string prevSelectedID = ElementsComboBox.ComboBox.SelectedValue as string;

            using (FlagManager.UseFlag("UpdateElementList"))
            {
                if (rebuild || CurrentProject == null)
                    Connections.Clear();

                if (CurrentProject != null)
                {
                    var studConnectors = CurrentProject.GetAllElements<PartConnection>(x =>
                            x.ConnectorType != LDD.Primitives.Connectors.ConnectorType.Custom2DField);

                    if (rebuild)
                        Connections.AddRange(studConnectors);
                    else
                        Connections.SyncItems(studConnectors);
                }
            }

            string currentSelectedID = ElementsComboBox.ComboBox.SelectedValue as string;
            if (prevSelectedID != currentSelectedID)
                FindNameForFunction(ElementsComboBox.SelectedItem as PartConnection, false);
        }


        private void ElementsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FlagManager.IsSet(nameof(UpdateElementList)) || 
                FlagManager.IsSet(nameof(FindNameForFunction)))
                return;

            FindNameForFunction(ElementsComboBox.SelectedItem as PartConnection, true);
        }

        #endregion

        #region Connection Subtypes Combobox

        private void FillSubTypeComboBox(ConnectorType connectorType)
        {
            //ConnectionSubTypeCombo.DataSource = null;
            SubTypeList.Clear();

            SubTypeList.AddRange(ResourceHelper.Connectors
                .Where(x => x.Type == connectorType));
            //ConnectionSubTypeCombo.DataSource = SubTypeList;
            //ConnectionSubTypeCombo.DisplayMember = "SubTypeDisplayText";
            //ConnectionSubTypeCombo.ValueMember = "SubType";
        }

        #endregion

        #region MyRegion

        private void FindNameForFunction(PartConnection connection, bool fromComboBox)
        {
            using (FlagManager.UseFlag(nameof(FindNameForFunction)))
            {
                if (SelectedElement != null)
                {
                    SelectedElement.PropertyChanged -= SelectedElement_PropertyChanged;
                    SelectedElement = null;
                }

                SelectedElement = connection;

                if (SelectedElement != null)
                {
                    SelectedElement.PropertyChanged += SelectedElement_PropertyChanged;

                    if (SyncSelectionCheckBox.Checked && fromComboBox
                        && !FlagManager.IsSet(nameof(SyncToCurrentSelection)))
                    {
                        ProjectManager.SelectElement(connection);
                    }
                }

                if (!fromComboBox && ElementsComboBox.SelectedItem != SelectedElement)
                {
                    ElementsComboBox.SelectedItem = SelectedElement;
                }
            }
        }

        private void SelectedElement_PropertyChanged(object sender, ElementValueChangedEventArgs e)
        {
            
        }

        #endregion


        private void SyncToCurrentSelection()
        {
            var selectedConnectors = ProjectManager.GetSelectionHierarchy()
                .OfType<PartConnection>().Where(x => x.ConnectorType != ConnectorType.Custom2DField);

            if (selectedConnectors.Count() == 1)
            {
                using (FlagManager.UseFlag(nameof(SyncToCurrentSelection)))
                {
                    //ElementsComboBox.SelectedItem = selectedConnectors.FirstOrDefault();
                    FindNameForFunction(selectedConnectors.FirstOrDefault(), false);
                }
            }
        }

        
    }
}
