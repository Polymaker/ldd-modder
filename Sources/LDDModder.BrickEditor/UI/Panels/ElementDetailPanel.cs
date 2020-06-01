﻿using LDDModder.BrickEditor.ProjectHandling;
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
    public partial class ElementDetailPanel : ProjectDocumentPanel
    {
        private PartElement SelectedElement;

        public ElementDetailPanel()
        {
            InitializeComponent();
        }

        public ElementDetailPanel(ProjectManager projectManager) : base(projectManager)
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            SetControlDoubleBuffered(PropertiesTableLayout);
            FillSelectionDetails(null);
            //this.TabText = Text;
        }

        protected override void OnElementSelectionChanged()
        {
            base.OnElementSelectionChanged();
 
            ExecuteOnThread(() =>
            {
                FillSelectionDetails(ProjectManager.SelectedElement);
            });
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
                SetStudRefVisibility(selection is PartCullingModel);
                SetConnectionInfoVisibility(selection is PartConnection);

                FillElementProperties(selection);
                if (selection is PartConnection connection)
                    FillConnectionDetails(connection);
                if (selection is PartCullingModel cullingModel)
                    FillStudRefDetails(cullingModel);

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
                        {
                            CollisionRadiusBox.Value = (e.Element as PartSphereCollision).Radius;
                        }
                        else if (e.PropertyName == nameof(PartBoxCollision.Size))
                        {
                            CollisionSizeEditor.Value = (e.Element as PartBoxCollision).Size * 2d;
                        }
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
            CommentsTextBox.DataBindings.Clear();

            ToggleControlsEnabled(element != null,
                NameTextBox, CommentsTextBox,
                NameLabel, CommentsLabel);

            NameTextBox.Text = element?.Name ?? string.Empty;
            CommentsTextBox.Text = element?.Comments ?? string.Empty;

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
            var newName = CurrentProject.RenameElement(ProjectManager.SelectedElement, NameTextBox.Text);
            NameTextBox.Text = newName;
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
            public string Name { get; set; }
            public string DisplayText { get; set; }

            public ConnectorComboItem(string iD, string name, string displayText)
            {
                ID = iD;
                Name = name;
                DisplayText = displayText;
            }
        }

        private void FillStudRefDetails(PartCullingModel model)
        {
            var connectorList = model.Project.GetAllElements<PartConnection>()
                .Where(x => x.ConnectorType == ConnectorType.Custom2DField).ToList();

            var comboItemList = new List<ConnectorComboItem>();
            comboItemList.Add(new ConnectorComboItem("NULL", string.Empty, NoConnectorRefLabel.Text));
            comboItemList.AddRange(connectorList.Select(x =>
                new ConnectorComboItem(
                    x.ID, x.Name,
                    $"{x.Name} ({(x.SubType == 22 ? BottomStudsLabel.Text : TopStudsLabel.Text)})"
                    )
                )
            );

            ConnectionRefCombo.DataSource = comboItemList;
            ConnectionRefCombo.DisplayMember = "DisplayText";
            ConnectionRefCombo.ValueMember = "ID";
            if (!string.IsNullOrEmpty(model.ConnectionID))
                ConnectionRefCombo.SelectedValue = model.ConnectionID;
        }

        private void ConnectionRefCombo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        #endregion

        #region Connection Details Handling

        private void SetConnectionInfoVisibility(bool visible)
        {
            bool isVisible = tabControl1.TabPages.Contains(ConnectionInfoTab);

            if (isVisible != visible)
            {
                if (visible)
                    tabControl1.TabPages.Add(ConnectionInfoTab);
                else
                    tabControl1.TabPages.Remove(ConnectionInfoTab);
            }
        }

        private void FillConnectionDetails(PartConnection connection)
        {
            ConnectionTypeValueLabel.Text = connection.ConnectorType.ToString();
            ConnectionSubTypeCombo.Text = connection.SubType.ToString();

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
    }
}
