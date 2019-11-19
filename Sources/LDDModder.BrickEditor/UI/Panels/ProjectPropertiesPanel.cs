using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LDDModder.BrickEditor.EditModels;
using LDDModder.BrickEditor.Resources;
using LDDModder.LDD.Data;
using LDDModder.Modding.Editing;

namespace LDDModder.BrickEditor.UI.Panels
{
    public partial class ProjectPropertiesPanel : ProjectDocumentPanel
    {
        private bool InternalSet;

        public ProjectPropertiesPanel()
        {
            InitializeComponent();
        }

        internal ProjectPropertiesPanel(ProjectManager projectManager) : base(projectManager)
        {
            InitializeComponent();
            CloseButtonVisible = false;
            CloseButton = false;
            
            DockAreas ^= WeifenLuo.WinFormsUI.Docking.DockAreas.Document;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            PlatformComboBox.DataSource = ResourceHelper.Platforms.ToList();
            PlatformComboBox.ValueMember = "ID";
            PlatformComboBox.DisplayMember = "Display";
            PlatformComboBox.MouseWheel += ComboBox_MouseWheel;

            CategoryComboBox.DataSource = ResourceHelper.Categories.ToList();
            CategoryComboBox.ValueMember = "ID";
            CategoryComboBox.DisplayMember = "Display";
            CategoryComboBox.MouseWheel += ComboBox_MouseWheel;

            UpdateControlBindings();
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
            PartIDTextBox.Enabled = CurrentProject != null;
            PartIDTextBox.Enabled = CurrentProject != null;
            DescriptionTextBox.Enabled = CurrentProject != null;
            PlatformComboBox.Enabled = CurrentProject != null;
            CategoryComboBox.Enabled = CurrentProject != null;
            boundingBoxEditor1.Enabled = CurrentProject != null;
            CalculateBoundingButton.Enabled = CurrentProject != null;
            InternalSet = true;

            if (CurrentProject != null)
            {
                PartIDTextBox.Value = CurrentProject.PartID;
                PartIDTextBox.ReadOnly = !(CurrentProject.PartID == 0);
                DescriptionTextBox.Text = CurrentProject.PartDescription;
                PlatformComboBox.SelectedValue = CurrentProject.Platform?.ID ?? 0;
                CategoryComboBox.SelectedValue = CurrentProject.MainGroup?.ID ?? 0;
                boundingBoxEditor1.Value = CurrentProject.Bounding ?? new LDD.Primitives.BoundingBox();
            }
            else
            {
                PartIDTextBox.Value = 0;
                DescriptionTextBox.Text = string.Empty;
                PlatformComboBox.SelectedIndex = 0;
                CategoryComboBox.SelectedIndex = 0;
            }

            InternalSet = false;
        }



        private void CalculateBoundingButton_Click(object sender, EventArgs e)
        {
            if (CurrentProject != null && !InternalSet)
            {
                var bounding = CurrentProject.CalculateBoundingBox();
                boundingBoxEditor1.Value = bounding;
            }
        }

        private void boundingBoxEditor1_ValueChanged(object sender, EventArgs e)
        {
            if (CurrentProject != null && !InternalSet)
                CurrentProject.Bounding = boundingBoxEditor1.Value;
        }

        private void DescriptionTextBox_Validated(object sender, EventArgs e)
        {
            if (CurrentProject != null && !InternalSet)
                CurrentProject.PartDescription = DescriptionTextBox.Text;
        }

        private void PartIDTextBox_ValueChanged(object sender, EventArgs e)
        {
            if (CurrentProject != null && !InternalSet)
                CurrentProject.PartID = (int)PartIDTextBox.Value;
        }

        private void PlatformComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            if (CurrentProject != null && !InternalSet)
                CurrentProject.Platform = PlatformComboBox.SelectedItem as Platform;
        }

        private void CategoryComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            if (CurrentProject != null && !InternalSet)
                CurrentProject.MainGroup = CategoryComboBox.SelectedItem as MainGroup;
        }

        protected override void OnElementPropertyChanged(Modding.Editing.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(e);

            if (e.Element == CurrentProject?.Properties)
            {
                UpdateControlBindings();
            }
        }
    }
}
