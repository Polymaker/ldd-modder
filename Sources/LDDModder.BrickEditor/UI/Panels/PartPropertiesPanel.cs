using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LDDModder.BrickEditor.ProjectHandling;
using LDDModder.BrickEditor.Resources;
using LDDModder.BrickEditor.Utilities;
using LDDModder.LDD.Data;
using LDDModder.Modding.Editing;

namespace LDDModder.BrickEditor.UI.Panels
{
    public partial class PartPropertiesPanel : ProjectDocumentPanel
    {
        private bool InternalSet;
        private List<MainGroup> Categories;
        

        public PartPropertiesPanel()
        {
            InitializeComponent();
        }

        internal PartPropertiesPanel(ProjectManager projectManager) : base(projectManager)
        {
            InitializeComponent();
            CloseButtonVisible = false;
            CloseButton = false;
            
            DockAreas ^= WeifenLuo.WinFormsUI.Docking.DockAreas.Document;

            SetControlDoubleBuffered(tableLayoutPanel2);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            PlatformComboBox.DataSource = ResourceHelper.Platforms.ToList();
            PlatformComboBox.ValueMember = "ID";
            PlatformComboBox.DisplayMember = "Display";
            PlatformComboBox.MouseWheel += ComboBox_MouseWheel;

            Categories = ResourceHelper.Categories.ToList();
            CategoryComboBox.MouseWheel += ComboBox_MouseWheel;
            int test = AliasesButtonBox.Height;

            UpdateCategoriesCombobox();
            InitializeAliasDropDown();
            UpdateControlBindings();
        }

        private void UpdateCategoriesCombobox()
        {
            var selectedPlatform = PlatformComboBox.SelectedItem as Platform;
            int platformID = selectedPlatform?.ID ?? 0;

            CategoryComboBox.DataSource = Categories.Where(x => (x.ID - (x.ID % 100)) == platformID).ToList();
            CategoryComboBox.ValueMember = "ID";
            CategoryComboBox.DisplayMember = "Display";
        }

        private void ComboBox_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = !(sender as ComboBox).DroppedDown;
        }

        protected override void OnProjectChanged()
        {
            base.OnProjectChanged();
            UpdateControlBindings();
        }

        private void UpdateControlBindings()
        {
            ToggleControlsEnabled(CurrentProject != null,
                PartIDTextBox,
                AliasesButtonBox,
                DescriptionTextBox,
                PlatformComboBox,
                CategoryComboBox,
                BoundingEditor,
                GeomBoundingEditor,
                CalcBoundingButton,
                CalcGeomBoundingButton,
                InertiaTensorTextBox,
                MassNumberBox,
                FrictionCheckBox,
                CenterOfMassEditor);

            DescriptionTextBox.DataBindings.Clear();
            BoundingEditor.DataBindings.Clear();
            GeomBoundingEditor.DataBindings.Clear();
            MassNumberBox.DataBindings.Clear();
            CenterOfMassEditor.DataBindings.Clear();

            InternalSet = true;

            if (CurrentProject != null)
            {
                var partProps = CurrentProject.Properties;

                PartIDTextBox.Value = CurrentProject.PartID;
                //PartIDTextBox.ReadOnly = !(ProjectManager.IsNewProject);
                AliasEdit.PartProperties = CurrentProject.Properties;
                AliasesButtonBox.Value = string.Join("; ", CurrentProject.Aliases);

                DescriptionTextBox.DataBindings.Add(new Binding("Text", 
                    CurrentProject.Properties, nameof(PartProperties.Description), 
                    true, DataSourceUpdateMode.OnValidation));

                PlatformComboBox.SelectedValue = CurrentProject.Platform?.ID ?? 0;
                UpdateCategoriesCombobox();
                CategoryComboBox.SelectedValue = CurrentProject.MainGroup?.ID ?? 0;

                BoundingEditor.DataBindings.Add(new Binding("Value",
                    partProps, nameof(CurrentProject.Properties.Bounding), 
                    false, DataSourceUpdateMode.OnPropertyChanged));

                GeomBoundingEditor.DataBindings.Add(new Binding("Value",
                    partProps, nameof(CurrentProject.Properties.GeometryBounding),
                    false, DataSourceUpdateMode.OnPropertyChanged));


                MassNumberBox.DataBindings.Add(new Binding("Value",
                    partProps.PhysicsAttributes, nameof(PartProperties.PhysicsAttributes.Mass),
                    true, DataSourceUpdateMode.OnPropertyChanged));

                CenterOfMassEditor.DataBindings.Add(new Binding("Value",
                    partProps.PhysicsAttributes, nameof(PartProperties.PhysicsAttributes.CenterOfMass),
                    true, DataSourceUpdateMode.OnPropertyChanged));

                var matrixValues = partProps.PhysicsAttributes.InertiaTensor.ToArray();
                string matrixStr = string.Join("; ", matrixValues);
                InertiaTensorTextBox.Text = matrixStr;
                //string matrixValues = partProps.PhysicsAttributes.GetInertiaTensorString();
                //InertiaTensorTextBox.Text = matrixValues.Replace(",", ", ");
            }
            else
            {
                PartIDTextBox.Value = 0;
                DescriptionTextBox.Text = string.Empty;
                AliasesButtonBox.Value = string.Empty;
                AliasEdit.PartProperties = null;
                PlatformComboBox.SelectedIndex = 0;
                CategoryComboBox.SelectedIndex = 0;
                BoundingEditor.Value = new LDD.Primitives.BoundingBox();
                GeomBoundingEditor.Value = new LDD.Primitives.BoundingBox();
                CenterOfMassEditor.Value = Simple3D.Vector3d.Zero;
            }

            InternalSet = false;
        }

        private void CalcBoundingButton_Click(object sender, EventArgs e)
        {
            if (CurrentProject != null && !InternalSet)
            {
                var bounding = CurrentProject.CalculateBoundingBox();
                BoundingEditor.Value = bounding.Rounded(6);
            }
        }

        private void CalcGeomBoundingButton_Click(object sender, EventArgs e)
        {
            if (CurrentProject != null && !InternalSet)
            {
                var bounding = CurrentProject.CalculateBoundingBox();
                GeomBoundingEditor.Value = bounding.Rounded(6);
            }
        }

        private void DescriptionTextBox_Validated(object sender, EventArgs e)
        {
            DescriptionTextBox.ClearUndo();
        }

        private void PartIDTextBox_ValueChanged(object sender, EventArgs e)
        {
            if (CurrentProject != null && !InternalSet)
                CurrentProject.PartID = (int)PartIDTextBox.Value;
        }

        private void PlatformComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            if (CurrentProject != null && !InternalSet)
            {
                ProjectManager.StartBatchChanges();
                CurrentProject.Platform = PlatformComboBox.SelectedItem as Platform;
                UpdateCategoriesCombobox();
                ProjectManager.EndBatchChanges();
            }
        }

        private void CategoryComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            if (CurrentProject != null && !InternalSet)
                CurrentProject.MainGroup = CategoryComboBox.SelectedItem as MainGroup;
        }

        protected override void OnElementPropertyChanged(Modding.Editing.ElementValueChangedEventArgs e)
        {
            base.OnElementPropertyChanged(e);

            if (e.Element == CurrentProject?.Properties)
            {
                UpdateControlBindings();
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (DescriptionTextBox.ContainsFocus)
            {
                if (keyData.HasFlag(Keys.Z) && keyData.HasFlag(Keys.Control) && DescriptionTextBox.CanUndo)
                {
                    DescriptionTextBox.Undo();
                    return true;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private UI.Controls.PartAliasesEditControl AliasEdit;
        private ToolStripDropDown AliasEditDropDown;

        private void InitializeAliasDropDown()
        {
            AliasEdit = new UI.Controls.PartAliasesEditControl();

            var controlHost = new ToolStripControlHost(AliasEdit);
            controlHost.AutoSize = false;
            AliasEdit.Dock = DockStyle.Fill;
            AliasEditDropDown = new ToolStripDropDown();
            AliasEditDropDown.Items.Add(controlHost);
            AliasEditDropDown.Width = AliasesButtonBox.Width;
        }

        private void AliasesButtonBox_BrowseButtonClicked(object sender, EventArgs e)
        {
            AliasEdit.ReloadAliases();
            AliasEditDropDown.Show(AliasesButtonBox, new Point(0, AliasesButtonBox.Height));
        }

        private void PartPropertiesPanel_SizeChanged(object sender, EventArgs e)
        {
            if (Width > Height && flowLayoutPanel1.FlowDirection == FlowDirection.TopDown)
            {
                flowLayoutPanel1.FlowDirection = FlowDirection.LeftToRight;
                flowLayoutPanel1.PerformLayout();
            }
            else if (Width < Height && flowLayoutPanel1.FlowDirection == FlowDirection.LeftToRight)
            {
                flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
                flowLayoutPanel1.PerformLayout();
            }
        }

        private void FillInertiaTensor(Simple3D.Matrix3d matrix)
        {
            var matrixValues = matrix.ToArray();
            string matrixStr = string.Join("; ", matrixValues);
            InertiaTensorTextBox.Text = matrixStr;
        }

        private void InertiaTensorTextBox_Validated(object sender, EventArgs e)
        {
            if (CurrentProject == null)
                return;

            var matValues = InertiaTensorTextBox.Text.Split(';');
            if (matValues.Length == 9)
            {
                var inertiaMatrix = new Simple3D.Matrix3d();
                bool validValues = true;
                for (int i = 0; i < 9; i++)
                {
                    if (NumberHelper.SmartTryParse(matValues[i], out double cellValue))
                        inertiaMatrix[i] = cellValue;
                    else
                    {
                        validValues = false;
                        break;
                    }
                }

                if (validValues)
                {
                    CurrentProject.PhysicsAttributes.InertiaTensor = inertiaMatrix;
                }
                else
                {
                    FillInertiaTensor(CurrentProject.PhysicsAttributes.InertiaTensor);
                }
            }
        }
    }
}
