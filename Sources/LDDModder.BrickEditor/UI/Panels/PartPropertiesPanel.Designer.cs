namespace LDDModder.BrickEditor.UI.Panels
{
    partial class PartPropertiesPanel
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PartPropertiesPanel));
            this.BoundingsPanel = new LDDModder.BrickEditor.UI.Controls.CollapsiblePanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.LabelBounding = new System.Windows.Forms.Label();
            this.CalcBoundingButton = new System.Windows.Forms.Button();
            this.BoundingEditor = new LDDModder.BrickEditor.UI.Controls.BoundingBoxEditor();
            this.LabelGeomBounding = new System.Windows.Forms.Label();
            this.CalcGeomBoundingButton = new System.Windows.Forms.Button();
            this.GeomBoundingEditor = new LDDModder.BrickEditor.UI.Controls.BoundingBoxEditor();
            this.DescriptionPanel = new LDDModder.BrickEditor.UI.Controls.CollapsiblePanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.LabelAliases = new System.Windows.Forms.Label();
            this.LabelPlatform = new System.Windows.Forms.Label();
            this.DescriptionTextBox = new System.Windows.Forms.TextBox();
            this.LabelPartID = new System.Windows.Forms.Label();
            this.CategoryComboBox = new System.Windows.Forms.ComboBox();
            this.PartIDTextBox = new LDDModder.BrickEditor.UI.Controls.NumberTextBox();
            this.LabelDescription = new System.Windows.Forms.Label();
            this.LabelCategory = new System.Windows.Forms.Label();
            this.PlatformComboBox = new System.Windows.Forms.ComboBox();
            this.AliasesButtonBox = new LDDModder.BrickEditor.UI.Controls.BrowseTextBox();
            this.PhysicalPanel = new LDDModder.BrickEditor.UI.Controls.CollapsiblePanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.LabelInertiaTensor = new System.Windows.Forms.Label();
            this.LabelCenterOfMass = new System.Windows.Forms.Label();
            this.InertiaTensorTextBox = new System.Windows.Forms.TextBox();
            this.LabelMass = new System.Windows.Forms.Label();
            this.CenterOfMassEditor = new LDDModder.BrickEditor.UI.Editors.VectorEditor();
            this.MassNumberBox = new LDDModder.BrickEditor.UI.Controls.NumberTextBox();
            this.LabelFriction = new System.Windows.Forms.Label();
            this.FrictionCheckBox = new System.Windows.Forms.CheckBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.BoundingsPanel.ContentPanel.SuspendLayout();
            this.BoundingsPanel.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.DescriptionPanel.ContentPanel.SuspendLayout();
            this.DescriptionPanel.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.PhysicalPanel.ContentPanel.SuspendLayout();
            this.PhysicalPanel.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // BoundingsPanel
            // 
            this.BoundingsPanel.AutoSizeHeight = true;
            // 
            // BoundingsPanel.ContentPanel
            // 
            this.BoundingsPanel.ContentPanel.Controls.Add(this.tableLayoutPanel2);
            resources.ApplyResources(this.BoundingsPanel.ContentPanel, "BoundingsPanel.ContentPanel");
            this.BoundingsPanel.ContentPanel.Name = "ContentPanel";
            this.BoundingsPanel.DisplayStyle = LDDModder.BrickEditor.UI.Controls.CollapsiblePanel.HeaderDisplayStyle.Button;
            resources.ApplyResources(this.BoundingsPanel, "BoundingsPanel");
            this.BoundingsPanel.Name = "BoundingsPanel";
            this.BoundingsPanel.PanelHeight = 220;
            this.BoundingsPanel.CollapsedChanged += new System.EventHandler(this.Panels_CollapsedChanged);
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.LabelBounding, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.CalcBoundingButton, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.BoundingEditor, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.LabelGeomBounding, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.CalcGeomBoundingButton, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.GeomBoundingEditor, 0, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // LabelBounding
            // 
            resources.ApplyResources(this.LabelBounding, "LabelBounding");
            this.LabelBounding.Name = "LabelBounding";
            // 
            // CalcBoundingButton
            // 
            resources.ApplyResources(this.CalcBoundingButton, "CalcBoundingButton");
            this.CalcBoundingButton.Name = "CalcBoundingButton";
            this.CalcBoundingButton.UseVisualStyleBackColor = true;
            this.CalcBoundingButton.Click += new System.EventHandler(this.CalcBoundingButton_Click);
            // 
            // BoundingEditor
            // 
            resources.ApplyResources(this.BoundingEditor, "BoundingEditor");
            this.tableLayoutPanel2.SetColumnSpan(this.BoundingEditor, 2);
            this.BoundingEditor.Name = "BoundingEditor";
            // 
            // LabelGeomBounding
            // 
            resources.ApplyResources(this.LabelGeomBounding, "LabelGeomBounding");
            this.LabelGeomBounding.Name = "LabelGeomBounding";
            // 
            // CalcGeomBoundingButton
            // 
            resources.ApplyResources(this.CalcGeomBoundingButton, "CalcGeomBoundingButton");
            this.CalcGeomBoundingButton.Name = "CalcGeomBoundingButton";
            this.CalcGeomBoundingButton.UseVisualStyleBackColor = true;
            this.CalcGeomBoundingButton.Click += new System.EventHandler(this.CalcGeomBoundingButton_Click);
            // 
            // GeomBoundingEditor
            // 
            resources.ApplyResources(this.GeomBoundingEditor, "GeomBoundingEditor");
            this.tableLayoutPanel2.SetColumnSpan(this.GeomBoundingEditor, 2);
            this.GeomBoundingEditor.Name = "GeomBoundingEditor";
            // 
            // DescriptionPanel
            // 
            this.DescriptionPanel.AutoSizeHeight = true;
            // 
            // DescriptionPanel.ContentPanel
            // 
            this.DescriptionPanel.ContentPanel.Controls.Add(this.tableLayoutPanel1);
            resources.ApplyResources(this.DescriptionPanel.ContentPanel, "DescriptionPanel.ContentPanel");
            this.DescriptionPanel.ContentPanel.Name = "ContentPanel";
            this.DescriptionPanel.DisplayStyle = LDDModder.BrickEditor.UI.Controls.CollapsiblePanel.HeaderDisplayStyle.Button;
            resources.ApplyResources(this.DescriptionPanel, "DescriptionPanel");
            this.DescriptionPanel.Name = "DescriptionPanel";
            this.DescriptionPanel.PanelHeight = 169;
            this.DescriptionPanel.CollapsedChanged += new System.EventHandler(this.Panels_CollapsedChanged);
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.LabelAliases, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.LabelPlatform, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.DescriptionTextBox, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.LabelPartID, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.CategoryComboBox, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.PartIDTextBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.LabelDescription, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.LabelCategory, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.PlatformComboBox, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.AliasesButtonBox, 3, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // LabelAliases
            // 
            resources.ApplyResources(this.LabelAliases, "LabelAliases");
            this.LabelAliases.Name = "LabelAliases";
            // 
            // LabelPlatform
            // 
            resources.ApplyResources(this.LabelPlatform, "LabelPlatform");
            this.LabelPlatform.Name = "LabelPlatform";
            // 
            // DescriptionTextBox
            // 
            resources.ApplyResources(this.DescriptionTextBox, "DescriptionTextBox");
            this.tableLayoutPanel1.SetColumnSpan(this.DescriptionTextBox, 4);
            this.DescriptionTextBox.Name = "DescriptionTextBox";
            this.DescriptionTextBox.Validated += new System.EventHandler(this.DescriptionTextBox_Validated);
            // 
            // LabelPartID
            // 
            resources.ApplyResources(this.LabelPartID, "LabelPartID");
            this.LabelPartID.Name = "LabelPartID";
            // 
            // CategoryComboBox
            // 
            resources.ApplyResources(this.CategoryComboBox, "CategoryComboBox");
            this.tableLayoutPanel1.SetColumnSpan(this.CategoryComboBox, 4);
            this.CategoryComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CategoryComboBox.FormattingEnabled = true;
            this.CategoryComboBox.Name = "CategoryComboBox";
            this.CategoryComboBox.SelectedValueChanged += new System.EventHandler(this.CategoryComboBox_SelectedValueChanged);
            // 
            // PartIDTextBox
            // 
            this.PartIDTextBox.AllowDecimals = false;
            resources.ApplyResources(this.PartIDTextBox, "PartIDTextBox");
            this.PartIDTextBox.MaximumValue = 9999999D;
            this.PartIDTextBox.Name = "PartIDTextBox";
            this.PartIDTextBox.ValueChanged += new System.EventHandler(this.PartIDTextBox_ValueChanged);
            // 
            // LabelDescription
            // 
            resources.ApplyResources(this.LabelDescription, "LabelDescription");
            this.tableLayoutPanel1.SetColumnSpan(this.LabelDescription, 2);
            this.LabelDescription.Name = "LabelDescription";
            // 
            // LabelCategory
            // 
            resources.ApplyResources(this.LabelCategory, "LabelCategory");
            this.LabelCategory.Name = "LabelCategory";
            // 
            // PlatformComboBox
            // 
            resources.ApplyResources(this.PlatformComboBox, "PlatformComboBox");
            this.tableLayoutPanel1.SetColumnSpan(this.PlatformComboBox, 4);
            this.PlatformComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PlatformComboBox.FormattingEnabled = true;
            this.PlatformComboBox.Name = "PlatformComboBox";
            this.PlatformComboBox.SelectedValueChanged += new System.EventHandler(this.PlatformComboBox_SelectedValueChanged);
            // 
            // AliasesButtonBox
            // 
            resources.ApplyResources(this.AliasesButtonBox, "AliasesButtonBox");
            this.AliasesButtonBox.AutoSizeButton = true;
            this.AliasesButtonBox.ButtonWidth = 26;
            this.AliasesButtonBox.Name = "AliasesButtonBox";
            this.AliasesButtonBox.ReadOnly = true;
            this.AliasesButtonBox.Value = "";
            this.AliasesButtonBox.BrowseButtonClicked += new System.EventHandler(this.AliasesButtonBox_BrowseButtonClicked);
            // 
            // PhysicalPanel
            // 
            this.PhysicalPanel.AutoSizeHeight = true;
            // 
            // PhysicalPanel.ContentPanel
            // 
            this.PhysicalPanel.ContentPanel.Controls.Add(this.tableLayoutPanel3);
            resources.ApplyResources(this.PhysicalPanel.ContentPanel, "PhysicalPanel.ContentPanel");
            this.PhysicalPanel.ContentPanel.Name = "ContentPanel";
            this.PhysicalPanel.DisplayStyle = LDDModder.BrickEditor.UI.Controls.CollapsiblePanel.HeaderDisplayStyle.Button;
            resources.ApplyResources(this.PhysicalPanel, "PhysicalPanel");
            this.PhysicalPanel.Name = "PhysicalPanel";
            this.PhysicalPanel.PanelHeight = 107;
            this.PhysicalPanel.CollapsedChanged += new System.EventHandler(this.Panels_CollapsedChanged);
            // 
            // tableLayoutPanel3
            // 
            resources.ApplyResources(this.tableLayoutPanel3, "tableLayoutPanel3");
            this.tableLayoutPanel3.Controls.Add(this.LabelInertiaTensor, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.LabelCenterOfMass, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.InertiaTensorTextBox, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.LabelMass, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.CenterOfMassEditor, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.MassNumberBox, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.LabelFriction, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.FrictionCheckBox, 1, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            // 
            // LabelInertiaTensor
            // 
            resources.ApplyResources(this.LabelInertiaTensor, "LabelInertiaTensor");
            this.LabelInertiaTensor.Name = "LabelInertiaTensor";
            // 
            // LabelCenterOfMass
            // 
            resources.ApplyResources(this.LabelCenterOfMass, "LabelCenterOfMass");
            this.LabelCenterOfMass.Name = "LabelCenterOfMass";
            // 
            // InertiaTensorTextBox
            // 
            resources.ApplyResources(this.InertiaTensorTextBox, "InertiaTensorTextBox");
            this.InertiaTensorTextBox.Name = "InertiaTensorTextBox";
            this.InertiaTensorTextBox.Validated += new System.EventHandler(this.InertiaTensorTextBox_Validated);
            // 
            // LabelMass
            // 
            resources.ApplyResources(this.LabelMass, "LabelMass");
            this.LabelMass.Name = "LabelMass";
            // 
            // CenterOfMassEditor
            // 
            resources.ApplyResources(this.CenterOfMassEditor, "CenterOfMassEditor");
            this.CenterOfMassEditor.Name = "CenterOfMassEditor";
            // 
            // MassNumberBox
            // 
            resources.ApplyResources(this.MassNumberBox, "MassNumberBox");
            this.MassNumberBox.MaximumValue = 99999D;
            this.MassNumberBox.Name = "MassNumberBox";
            // 
            // LabelFriction
            // 
            resources.ApplyResources(this.LabelFriction, "LabelFriction");
            this.LabelFriction.Name = "LabelFriction";
            // 
            // FrictionCheckBox
            // 
            resources.ApplyResources(this.FrictionCheckBox, "FrictionCheckBox");
            this.FrictionCheckBox.Name = "FrictionCheckBox";
            this.FrictionCheckBox.UseVisualStyleBackColor = true;
            this.FrictionCheckBox.CheckedChanged += new System.EventHandler(this.FrictionCheckBox_CheckedChanged);
            // 
            // flowLayoutPanel1
            // 
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.flowLayoutPanel1.Controls.Add(this.DescriptionPanel);
            this.flowLayoutPanel1.Controls.Add(this.PhysicalPanel);
            this.flowLayoutPanel1.Controls.Add(this.BoundingsPanel);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // PartPropertiesPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "PartPropertiesPanel";
            this.SizeChanged += new System.EventHandler(this.PartPropertiesPanel_SizeChanged);
            this.BoundingsPanel.ContentPanel.ResumeLayout(false);
            this.BoundingsPanel.ContentPanel.PerformLayout();
            this.BoundingsPanel.ResumeLayout(false);
            this.BoundingsPanel.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.DescriptionPanel.ContentPanel.ResumeLayout(false);
            this.DescriptionPanel.ContentPanel.PerformLayout();
            this.DescriptionPanel.ResumeLayout(false);
            this.DescriptionPanel.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.PhysicalPanel.ContentPanel.ResumeLayout(false);
            this.PhysicalPanel.ContentPanel.PerformLayout();
            this.PhysicalPanel.ResumeLayout(false);
            this.PhysicalPanel.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox DescriptionTextBox;
        private System.Windows.Forms.Label LabelPartID;
        private System.Windows.Forms.Label LabelDescription;
        private System.Windows.Forms.Label LabelCategory;
        private System.Windows.Forms.Label LabelPlatform;
        private System.Windows.Forms.ComboBox PlatformComboBox;
        private System.Windows.Forms.ComboBox CategoryComboBox;
        private Controls.NumberTextBox PartIDTextBox;
        private System.Windows.Forms.Label LabelBounding;
        private Controls.BoundingBoxEditor BoundingEditor;
        private System.Windows.Forms.Button CalcBoundingButton;
        private System.Windows.Forms.Label LabelGeomBounding;
        private Controls.BoundingBoxEditor GeomBoundingEditor;
        private System.Windows.Forms.Button CalcGeomBoundingButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private Controls.CollapsiblePanel BoundingsPanel;
        private Controls.CollapsiblePanel DescriptionPanel;
        private Controls.CollapsiblePanel PhysicalPanel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label LabelInertiaTensor;
        private System.Windows.Forms.Label LabelCenterOfMass;
        private System.Windows.Forms.TextBox InertiaTensorTextBox;
        private System.Windows.Forms.Label LabelMass;
        private Controls.NumberTextBox MassNumberBox;
        private System.Windows.Forms.Label LabelFriction;
        private System.Windows.Forms.CheckBox FrictionCheckBox;
        private System.Windows.Forms.Label LabelAliases;
        private Controls.BrowseTextBox AliasesButtonBox;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private Editors.VectorEditor CenterOfMassEditor;
    }
}