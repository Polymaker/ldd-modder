using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LDDModder.BrickEditor.ProjectHandling;
using LDDModder.BrickEditor.Resources;
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

            UpdateCategoriesCombobox();

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
            PartIDTextBox.Enabled = CurrentProject != null;
            PartIDTextBox.Enabled = CurrentProject != null;
            DescriptionTextBox.Enabled = CurrentProject != null;
            DescriptionTextBox.DataBindings.Clear();

            PlatformComboBox.Enabled = CurrentProject != null;
            CategoryComboBox.Enabled = CurrentProject != null;
            boundingBoxEditor1.Enabled = CurrentProject != null;
            boundingBoxEditor1.DataBindings.Clear();

            CalculateBoundingButton.Enabled = CurrentProject != null;

            InternalSet = true;

            if (CurrentProject != null)
            {
                PartIDTextBox.Value = CurrentProject.PartID;
                PartIDTextBox.ReadOnly = !(ProjectManager.IsNewProject);

                DescriptionTextBox.DataBindings.Add(new Binding("Text", 
                    CurrentProject.Properties, nameof(PartProperties.Description), 
                    true, DataSourceUpdateMode.OnValidation));

                //DescriptionTextBox.Text = CurrentProject.PartDescription;
                PlatformComboBox.SelectedValue = CurrentProject.Platform?.ID ?? 0;
                UpdateCategoriesCombobox();
                CategoryComboBox.SelectedValue = CurrentProject.MainGroup?.ID ?? 0;

                boundingBoxEditor1.DataBindings.Add(new Binding("Value",
                    CurrentProject.Properties, nameof(CurrentProject.Properties.Bounding), 
                    false, DataSourceUpdateMode.OnPropertyChanged));

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
    }
}
