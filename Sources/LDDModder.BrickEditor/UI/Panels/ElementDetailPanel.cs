using LDDModder.BrickEditor.ProjectHandling;
using LDDModder.BrickEditor.Resources;
using LDDModder.LDD.Primitives.Connectors;
using LDDModder.Modding;
using LDDModder.Modding;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LDDModder.BrickEditor.UI.Panels
{
    public partial class ElementDetailPanel : ProjectDocumentPanel
    {
        private PartElement SelectedElement;
        private SortableBindingList<ConnectorComboItem> ConnectorList;

        public ElementDetailPanel()
        {
            InitializeComponent();
            StudRefGridView.AutoGenerateColumns = false;
            CloseButtonVisible = false;
            CloseButton = false;
            ConnectorList = new SortableBindingList<ConnectorComboItem>();
        }

        public ElementDetailPanel(ProjectManager projectManager) : base(projectManager)
        {
            InitializeComponent();
            StudRefGridView.AutoGenerateColumns = false;
            CloseButtonVisible = false;
            CloseButton = false;
            ConnectorList = new SortableBindingList<ConnectorComboItem>();
            DockAreas ^= WeifenLuo.WinFormsUI.Docking.DockAreas.Document;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            SetControlDoubleBuffered(PropertiesTableLayout);
            InitializeStudRefGrid();
            FillSelectionDetails(null);
        }

        protected override void OnProjectClosed()
        {
            base.OnProjectClosed();
            FillSelectionDetails(null);
        }

        protected override void OnElementSelectionChanged()
        {
            base.OnElementSelectionChanged();
            //Trace.WriteLine($"SelectedElement => {ProjectManager.SelectedElement}");
            ExecuteOnThread(() =>
            {
                FillSelectionDetails(ProjectManager.SelectedElement);
            });
        }

        private bool PartConnectionChanged;

        protected override void OnElementCollectionChanged(ElementCollectionChangedEventArgs e)
        {
            base.OnElementCollectionChanged(e);

            if (e.ElementType == typeof(PartConnection))
                PartConnectionChanged = true;
        }


        protected override void OnProjectElementsChanged()
        {
            base.OnProjectElementsChanged();

            if (PartConnectionChanged)
            {
                ExecuteOnThread(UpdateStudConnectorList);
                PartConnectionChanged = false;
            }
        }

        private void FillSelectionDetails(PartElement selection)
        {
            if (SelectedElement != null)
            {
                SelectedElement.PropertyChanged -= SelectedElement_PropertyChanged;
                SelectedElement = null;
            }

            using (FlagManager.UseFlag("FillSelectionDetails"))
            {
                FillElementProperties(selection);

                if (ProjectManager.SelectedElements.Count > 1)
                {
                    FillStudRefDetails(null);
                }
                else
                {
                    FillStudRefDetails(selection as PartCullingModel);
                }

                SelectedElement = selection;
                if (SelectedElement != null)
                    SelectedElement.PropertyChanged += SelectedElement_PropertyChanged;
            }
        }

        private void SelectedElement_PropertyChanged(object sender, ElementValueChangedEventArgs e)
        {
            ExecuteOnThread(() =>
                {
                    using (FlagManager.UseFlag("UpdatePropertyValue"))
                    {
                        if (e.PropertyName == nameof(PartElement.Name))
                            NameTextBox.Text = e.Element.Name;
                        if (e.PropertyName == nameof(PartSphereCollision.Radius))
                            CollisionRadiusBox.Value = (e.Element as PartSphereCollision).Radius;
                        else if (e.PropertyName == nameof(PartBoxCollision.Size) && e.Element is PartBoxCollision boxColl)
                            CollisionSizeEditor.Value = boxColl.Size * 2d;
                    }
                });
        }

        private string GetElementTypeName(PartElement element)
        {
            switch (element)
            {
                case PartSurface _:
                    return ModelLocalizations.Label_Surface;
                case PartBone _:
                    return ModelLocalizations.Label_Bone;
                case MaleStudModel _:
                    return ModelLocalizations.ModelComponentType_MaleStud;
                case FemaleStudModel _:
                    return ModelLocalizations.ModelComponentType_FemaleStud;
                case BrickTubeModel _:
                    return ModelLocalizations.ModelComponentType_BrickTube;
                case PartModel _:
                    return ModelLocalizations.ModelComponentType_Part;

                case ModelMeshReference _:
                    return ModelLocalizations.Label_Mesh;
                case PartConnection _:
                    return ModelLocalizations.Label_Connection;
                case PartCollision _:
                    return ModelLocalizations.Label_Collision;
            }

            return element.GetType().Name;
        }

        private string GetElementTypeName2(PartElement element)
        {
            if (element is PartSurface)
                return ModelLocalizations.Label_Surface;

            switch (element)
            {
                case PartSurface _:
                    return ModelLocalizations.Label_Surface;
                case PartBone _:
                    return ModelLocalizations.Label_Bone;
                case MaleStudModel _:
                    return ModelLocalizations.ModelComponentType_MaleStud;
                case FemaleStudModel _:
                    return ModelLocalizations.ModelComponentType_FemaleStud;
                case BrickTubeModel _:
                    return ModelLocalizations.ModelComponentType_BrickTube;
                case PartModel _:
                    return ModelLocalizations.ModelComponentType_Part;

                case ModelMeshReference _:
                    return ModelLocalizations.Label_Mesh;
                case PartConnection conn:
                    return $"{ModelLocalizations.Label_Connection} <{conn.ConnectorType.ToString()}>";
                //return ModelLocalizations.ResourceManager.GetString($"ConnectorType_{conn.ConnectorType}");
                case PartCollision coll:
                    string collType = ModelLocalizations.ResourceManager.GetString($"CollisionType_{coll.CollisionType}");
                    return $"{ModelLocalizations.Label_Collision} ({collType})";
            }

            return element.GetType().Name;
        }
        

        #region Main Properties Handling

        private void FillElementProperties(PartElement element)
        {
            ToggleControlsEnabled(element != null,
                NameTextBox,
                NameLabel);

            NameTextBox.Text = element?.Name ?? string.Empty;

            SubMaterialIndexLabel.Visible = element is PartSurface;
            SubMaterialIndexBox.Visible = element is PartSurface;

            CollisionRadiusLabel.Visible = element is PartSphereCollision;
            CollisionRadiusBox.Visible = element is PartSphereCollision;

            CollisionSizeLabel.Visible = element is PartBoxCollision;
            CollisionSizeEditor.Visible = element is PartBoxCollision;

            if (SelectionTransformEdit.Tag != null)
                SelectionTransformEdit.BindPhysicalElement(null);

            if (element != null)
            {
                SelectionInfoLabel.Text = GetElementTypeName(element);

                switch (element)
                {
                    case PartSurface surface:
                        SubMaterialIndexBox.Value = surface.SubMaterialIndex;
                        break;

                    case PartBoxCollision boxCollision:
                        CollisionSizeEditor.Value = boxCollision.Size * 2d;
                        break;

                    case PartSphereCollision sphereCollision:
                        CollisionRadiusBox.Value = sphereCollision.Radius;
                        break;
                }
            }
            else
            {
                if (ProjectManager.SelectedElements.Count > 1)
                    SelectionInfoLabel.Text = MultiSelectionMsg.Text;
                else
                    SelectionInfoLabel.Text = NoSelectionMsg.Text;
            }

            if (element is IPhysicalElement physicalElement)
            {
                SelectionTransformEdit.BindPhysicalElement(physicalElement);
                SelectionTransformEdit.Tag = physicalElement;
                SelectionTransformEdit.Enabled = true;
            }
            else
                SelectionTransformEdit.Enabled = false;
        }


        private void CollisionRadiusBox_ValueChanged(object sender, EventArgs e)
        {
            if (FlagManager.IsSet("FillSelectionDetails") || 
                FlagManager.IsSet("UpdatePropertyValue"))
                return;

            if (SelectedElement is PartSphereCollision sphereCollision)
            {
                sphereCollision.Radius = CollisionRadiusBox.Value;
            }
        }

        private void CollisionSizeEditor_ValueChanged(object sender, EventArgs e)
        {
            if (FlagManager.IsSet("FillSelectionDetails") ||
                FlagManager.IsSet("UpdatePropertyValue"))
                return;

            if (SelectedElement is PartBoxCollision boxCollision)
            {
                boxCollision.Size = CollisionSizeEditor.Value / 2d;
            }
        }

        private void NameTextBox_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(NameTextBox.Text))
            {
                e.Cancel = true;
            }
        }

        private void NameTextBox_Validated(object sender, EventArgs e)
        {
            if (SelectedElement == null)
                return;
            var newName = CurrentProject.RenameElement(ProjectManager.SelectedElement, NameTextBox.Text);
            NameTextBox.Text = newName;
        }


        private void SubMaterialIndexBox_ValueChanged(object sender, EventArgs e)
        {
            if (FlagManager.IsSet("FillSelectionDetails") ||
                FlagManager.IsSet("UpdatePropertyValue"))
                return;

            if (SelectedElement is PartSurface surface)
            {
                surface.SubMaterialIndex = (int)SubMaterialIndexBox.Value;
            }
        }

        #endregion

        #region Stud References Handling

        private void SetStudRefVisibility(bool visible)
        {
            bool isVisible = tabControl1.TabPages.Contains(StudRefTab);

            if (isVisible != visible)
            {
                if (visible)
                    tabControl1.TabPages.Add(StudRefTab);
                else
                    tabControl1.TabPages.Remove(StudRefTab);
            }
        }

        private class ConnectorComboItem
        {
            public string ID { get; set; }

            public PartConnection Connection { get; set; }
            public int SubType { get; set; }
            //public string Name { get; set; }
            public string DisplayText => (Connection?.Name ?? "DELETED") + $" ({ConnTypeText})";
            public string ConnTypeText { get; set; }
            public ConnectorComboItem(PartConnection connection)
            {
                ID = connection.ID;
                Connection = connection;
                SubType = connection.SubType;
            }
        }

        private void UpdateStudConnectorList()
        {
            if (CurrentProject == null)
            {
                ConnectorList.Clear();
            }
            else
            {
                var studConnections = CurrentProject.GetAllElements<PartConnection>()
                    .Where(x => x.ConnectorType == ConnectorType.Custom2DField).ToList();

                foreach (var studConn in studConnections)
                {
                    if (string.IsNullOrEmpty(studConn.ID))
                        continue;

                    var comboItem = ConnectorList.FirstOrDefault(x => x.ID == studConn.ID);
                    if (comboItem== null)
                    {
                        comboItem = new ConnectorComboItem(studConn);
                        comboItem.ConnTypeText = studConn.SubType == 22 ? BottomStudsLabel.Text : TopStudsLabel.Text;
                        ConnectorList.Add(comboItem);
                    }
                }

                var validConnectionIDs = studConnections.Select(x => x.ID).ToList();
                var usedConnectionIDs = CurrentProject.GetAllElements<StudReference>()
                    .Select(x => x.ConnectionID).Distinct().ToList();

                foreach (var comboItem in ConnectorList.ToArray())
                {
                    if (!validConnectionIDs.Contains(comboItem.ID) && 
                        !usedConnectionIDs.Contains(comboItem.ID))
                    {
                        ConnectorList.Remove(comboItem);
                    }
                }
            }
        }

        private void FillStudRefDetails(PartCullingModel model)
        {
            SetStudRefVisibility(model != null);

            StudRefGridView.DataSource = null;

            if (model == null)
                return;

            UpdateStudConnectorList();

            StudRefGridView.DataSource = model.GetStudReferences().ToList();
            AdjStudColumn.Visible = model != null && model.ComponentType == ModelComponentType.BrickTube;
        }

        private void InitializeStudRefGrid()
        {
            ConnectionColumn.DataSource = ConnectorList;
            ConnectionColumn.DisplayMember = "DisplayText";
            ConnectionColumn.ValueMember = "ID";
        }
        private void StudRefGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;

        }

        private void StudRefGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && 
                StudRefGridView.Rows[e.RowIndex].DataBoundItem is StudReference studRef)
            {

                if (e.ColumnIndex == FieldPositionColumn.Index)
                {
                    e.Value =  $"{studRef.PositionX}, {studRef.PositionY}";
                }
                else if (e.ColumnIndex == AdjStudColumn.Index && studRef.Parent is BrickTubeModel tubeModel)
                {
                    e.Value = tubeModel.AdjacentStuds.Contains(studRef);
                }
            }
        }

        #endregion

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                if (NameTextBox.ContainsFocus && NameTextBox.Modified)
                {
                    NameTextBox.Text = ProjectManager.SelectedElement?.Name ?? string.Empty;
                    return true;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        

        //private void TestClonePatterns()
        //{
        //    //var elem = CurrentProject.Collisions.FirstOrDefault();
        //    //if (elem != null)
        //    //{
        //    //    //var pattern = new LinearPattern();
        //    //    //pattern.Elements.Add(elem);
        //    //    //pattern.Direction = new Simple3D.Vector3(1,0,1).Normalized();
        //    //    //pattern.Offset = 2f;
        //    //    //pattern.Repetitions = 3;
        //    //    var pattern = new CircularPattern();
        //    //    pattern.Elements.Add(elem);
        //    //    pattern.Axis = new Simple3D.Vector3(0, 0, 1).Normalized();
        //    //    pattern.Repetitions = 6;
        //    //    pattern.EqualSpacing = true;
        //    //    pattern.Angle = (float)Math.PI * 2f;
        //    //    CurrentProject.ClonePatterns.Add(pattern);
        //    //    ProjectManager.ViewportWindow.RebuildModels();
        //    //}
        //}
    }
}
