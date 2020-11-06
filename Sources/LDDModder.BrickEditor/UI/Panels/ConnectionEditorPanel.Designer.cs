namespace LDDModder.BrickEditor.UI.Panels
{
    partial class ConnectionEditorPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConnectionEditorPanel));
            this.SelectionToolStrip = new System.Windows.Forms.ToolStrip();
            this.CurrentSelectionLabel = new System.Windows.Forms.ToolStripLabel();
            this.ElementsComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.SyncSelectionCheckBox = new LDDModder.BrickEditor.UI.Controls.ToolStripCheckBox();
            this.AddConnectionDropDown = new System.Windows.Forms.ToolStripDropDownButton();
            this.SpringCheckBox = new System.Windows.Forms.CheckBox();
            this.TypeValueLabel = new System.Windows.Forms.Label();
            this.ConnectionSubTypeCombo = new System.Windows.Forms.ComboBox();
            this.LengthBox = new LDDModder.BrickEditor.UI.Controls.NumberTextBox();
            this.ElementNameTextBox = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.NameControlLabel = new LDDModder.BrickEditor.UI.Controls.ControlLabel();
            this.TypeControlLabel = new LDDModder.BrickEditor.UI.Controls.ControlLabel();
            this.SubtypeControlLabel = new LDDModder.BrickEditor.UI.Controls.ControlLabel();
            this.LengthControlLabel = new LDDModder.BrickEditor.UI.Controls.ControlLabel();
            this.AxesControlLabel = new LDDModder.BrickEditor.UI.Controls.ControlLabel();
            this.AxesNumberBox = new LDDModder.BrickEditor.UI.Controls.NumberTextBox();
            this.CapLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.CylindricalCheckBox = new System.Windows.Forms.CheckBox();
            this.EndCappedCheckBox = new System.Windows.Forms.CheckBox();
            this.StartCappedCheckBox = new System.Windows.Forms.CheckBox();
            this.GrabbingLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.GrabbingRequiredCheckBox = new System.Windows.Forms.CheckBox();
            this.GrabbingCheckBox = new System.Windows.Forms.CheckBox();
            this.SpringPanel = new System.Windows.Forms.Panel();
            this.SpringEditor = new LDDModder.BrickEditor.UI.Editors.VectorEditor();
            this.GearLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.RadiusLabel = new System.Windows.Forms.Label();
            this.ToothCountLabel = new System.Windows.Forms.Label();
            this.ToothNumBox = new LDDModder.BrickEditor.UI.Controls.NumberTextBox();
            this.RadiusNumBox = new LDDModder.BrickEditor.UI.Controls.NumberTextBox();
            this.HingeLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.LimitMinBox = new LDDModder.BrickEditor.UI.Controls.NumberTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.OrientedCheckBox = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.LimitMaxBox = new LDDModder.BrickEditor.UI.Controls.NumberTextBox();
            this.FlipLimitMaxBox = new LDDModder.BrickEditor.UI.Controls.NumberTextBox();
            this.FlipLimitMinBox = new LDDModder.BrickEditor.UI.Controls.NumberTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.TagControlLabel = new LDDModder.BrickEditor.UI.Controls.ControlLabel();
            this.TagTextBox = new System.Windows.Forms.TextBox();
            this.TransformEdit = new LDDModder.BrickEditor.UI.Controls.TransformEditor();
            this.SelectionToolStrip.SuspendLayout();
            this.panel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.NameControlLabel.SuspendLayout();
            this.TypeControlLabel.SuspendLayout();
            this.SubtypeControlLabel.SuspendLayout();
            this.LengthControlLabel.SuspendLayout();
            this.AxesControlLabel.SuspendLayout();
            this.CapLayoutPanel.SuspendLayout();
            this.GrabbingLayoutPanel.SuspendLayout();
            this.SpringPanel.SuspendLayout();
            this.GearLayoutPanel.SuspendLayout();
            this.HingeLayoutPanel.SuspendLayout();
            this.TagControlLabel.SuspendLayout();
            this.SuspendLayout();
            // 
            // SelectionToolStrip
            // 
            this.SelectionToolStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.SelectionToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.SelectionToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CurrentSelectionLabel,
            this.ElementsComboBox,
            this.SyncSelectionCheckBox,
            this.AddConnectionDropDown});
            resources.ApplyResources(this.SelectionToolStrip, "SelectionToolStrip");
            this.SelectionToolStrip.Name = "SelectionToolStrip";
            // 
            // CurrentSelectionLabel
            // 
            this.CurrentSelectionLabel.Name = "CurrentSelectionLabel";
            resources.ApplyResources(this.CurrentSelectionLabel, "CurrentSelectionLabel");
            // 
            // ElementsComboBox
            // 
            this.ElementsComboBox.Name = "ElementsComboBox";
            resources.ApplyResources(this.ElementsComboBox, "ElementsComboBox");
            this.ElementsComboBox.SelectedIndexChanged += new System.EventHandler(this.ElementsComboBox_SelectedIndexChanged);
            // 
            // SyncSelectionCheckBox
            // 
            this.SyncSelectionCheckBox.Margin = new System.Windows.Forms.Padding(6, 1, 0, 2);
            this.SyncSelectionCheckBox.Name = "SyncSelectionCheckBox";
            resources.ApplyResources(this.SyncSelectionCheckBox, "SyncSelectionCheckBox");
            this.SyncSelectionCheckBox.Click += new System.EventHandler(this.SyncSelectionCheckBox_Click);
            // 
            // AddConnectionDropDown
            // 
            this.AddConnectionDropDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(this.AddConnectionDropDown, "AddConnectionDropDown");
            this.AddConnectionDropDown.Name = "AddConnectionDropDown";
            // 
            // SpringCheckBox
            // 
            resources.ApplyResources(this.SpringCheckBox, "SpringCheckBox");
            this.SpringCheckBox.Name = "SpringCheckBox";
            this.SpringCheckBox.UseVisualStyleBackColor = true;
            this.SpringCheckBox.CheckedChanged += new System.EventHandler(this.SpringCheckBox_CheckedChanged);
            // 
            // TypeValueLabel
            // 
            resources.ApplyResources(this.TypeValueLabel, "TypeValueLabel");
            this.TypeValueLabel.Name = "TypeValueLabel";
            // 
            // ConnectionSubTypeCombo
            // 
            this.ConnectionSubTypeCombo.FormattingEnabled = true;
            resources.ApplyResources(this.ConnectionSubTypeCombo, "ConnectionSubTypeCombo");
            this.ConnectionSubTypeCombo.Name = "ConnectionSubTypeCombo";
            this.ConnectionSubTypeCombo.SelectedIndexChanged += new System.EventHandler(this.ConnectionSubTypeCombo_SelectedIndexChanged);
            this.ConnectionSubTypeCombo.Validating += new System.ComponentModel.CancelEventHandler(this.ConnectionSubTypeCombo_Validating);
            this.ConnectionSubTypeCombo.Validated += new System.EventHandler(this.ConnectionSubTypeCombo_Validated);
            // 
            // LengthBox
            // 
            resources.ApplyResources(this.LengthBox, "LengthBox");
            this.LengthBox.MaximumValue = 1000D;
            this.LengthBox.Name = "LengthBox";
            // 
            // ElementNameTextBox
            // 
            resources.ApplyResources(this.ElementNameTextBox, "ElementNameTextBox");
            this.ElementNameTextBox.Name = "ElementNameTextBox";
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.flowLayoutPanel1);
            this.panel1.Name = "panel1";
            // 
            // flowLayoutPanel1
            // 
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.flowLayoutPanel1.Controls.Add(this.NameControlLabel);
            this.flowLayoutPanel1.Controls.Add(this.TypeControlLabel);
            this.flowLayoutPanel1.Controls.Add(this.SubtypeControlLabel);
            this.flowLayoutPanel1.Controls.Add(this.LengthControlLabel);
            this.flowLayoutPanel1.Controls.Add(this.AxesControlLabel);
            this.flowLayoutPanel1.Controls.Add(this.CapLayoutPanel);
            this.flowLayoutPanel1.Controls.Add(this.GrabbingLayoutPanel);
            this.flowLayoutPanel1.Controls.Add(this.SpringPanel);
            this.flowLayoutPanel1.Controls.Add(this.GearLayoutPanel);
            this.flowLayoutPanel1.Controls.Add(this.HingeLayoutPanel);
            this.flowLayoutPanel1.Controls.Add(this.TagControlLabel);
            this.flowLayoutPanel1.Controls.Add(this.TransformEdit);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Layout += new System.Windows.Forms.LayoutEventHandler(this.flowLayoutPanel1_Layout);
            // 
            // NameControlLabel
            // 
            this.NameControlLabel.Controls.Add(this.ElementNameTextBox);
            this.NameControlLabel.LabelWidth = 80;
            resources.ApplyResources(this.NameControlLabel, "NameControlLabel");
            this.NameControlLabel.Name = "NameControlLabel";
            // 
            // TypeControlLabel
            // 
            this.TypeControlLabel.Controls.Add(this.TypeValueLabel);
            this.TypeControlLabel.LabelWidth = 80;
            resources.ApplyResources(this.TypeControlLabel, "TypeControlLabel");
            this.TypeControlLabel.Name = "TypeControlLabel";
            // 
            // SubtypeControlLabel
            // 
            this.SubtypeControlLabel.Controls.Add(this.ConnectionSubTypeCombo);
            this.SubtypeControlLabel.LabelWidth = 80;
            resources.ApplyResources(this.SubtypeControlLabel, "SubtypeControlLabel");
            this.SubtypeControlLabel.Name = "SubtypeControlLabel";
            // 
            // LengthControlLabel
            // 
            this.LengthControlLabel.Controls.Add(this.LengthBox);
            this.LengthControlLabel.LabelWidth = 80;
            resources.ApplyResources(this.LengthControlLabel, "LengthControlLabel");
            this.LengthControlLabel.Name = "LengthControlLabel";
            // 
            // AxesControlLabel
            // 
            this.AxesControlLabel.Controls.Add(this.AxesNumberBox);
            this.AxesControlLabel.LabelWidth = 80;
            resources.ApplyResources(this.AxesControlLabel, "AxesControlLabel");
            this.AxesControlLabel.Name = "AxesControlLabel";
            // 
            // AxesNumberBox
            // 
            this.AxesNumberBox.AllowDecimals = false;
            resources.ApplyResources(this.AxesNumberBox, "AxesNumberBox");
            this.AxesNumberBox.MaximumValue = 6D;
            this.AxesNumberBox.Name = "AxesNumberBox";
            // 
            // CapLayoutPanel
            // 
            resources.ApplyResources(this.CapLayoutPanel, "CapLayoutPanel");
            this.CapLayoutPanel.Controls.Add(this.CylindricalCheckBox, 0, 1);
            this.CapLayoutPanel.Controls.Add(this.EndCappedCheckBox, 1, 0);
            this.CapLayoutPanel.Controls.Add(this.StartCappedCheckBox, 0, 0);
            this.CapLayoutPanel.Name = "CapLayoutPanel";
            // 
            // CylindricalCheckBox
            // 
            resources.ApplyResources(this.CylindricalCheckBox, "CylindricalCheckBox");
            this.CapLayoutPanel.SetColumnSpan(this.CylindricalCheckBox, 2);
            this.CylindricalCheckBox.Name = "CylindricalCheckBox";
            this.CylindricalCheckBox.UseVisualStyleBackColor = true;
            // 
            // EndCappedCheckBox
            // 
            resources.ApplyResources(this.EndCappedCheckBox, "EndCappedCheckBox");
            this.EndCappedCheckBox.Name = "EndCappedCheckBox";
            this.EndCappedCheckBox.UseVisualStyleBackColor = true;
            // 
            // StartCappedCheckBox
            // 
            resources.ApplyResources(this.StartCappedCheckBox, "StartCappedCheckBox");
            this.StartCappedCheckBox.Name = "StartCappedCheckBox";
            this.StartCappedCheckBox.UseVisualStyleBackColor = true;
            // 
            // GrabbingLayoutPanel
            // 
            resources.ApplyResources(this.GrabbingLayoutPanel, "GrabbingLayoutPanel");
            this.GrabbingLayoutPanel.Controls.Add(this.GrabbingRequiredCheckBox, 1, 0);
            this.GrabbingLayoutPanel.Controls.Add(this.GrabbingCheckBox, 0, 0);
            this.GrabbingLayoutPanel.Name = "GrabbingLayoutPanel";
            // 
            // GrabbingRequiredCheckBox
            // 
            resources.ApplyResources(this.GrabbingRequiredCheckBox, "GrabbingRequiredCheckBox");
            this.GrabbingRequiredCheckBox.Name = "GrabbingRequiredCheckBox";
            this.GrabbingRequiredCheckBox.UseVisualStyleBackColor = true;
            // 
            // GrabbingCheckBox
            // 
            resources.ApplyResources(this.GrabbingCheckBox, "GrabbingCheckBox");
            this.GrabbingCheckBox.Name = "GrabbingCheckBox";
            this.GrabbingCheckBox.UseVisualStyleBackColor = true;
            // 
            // SpringPanel
            // 
            this.SpringPanel.Controls.Add(this.SpringEditor);
            this.SpringPanel.Controls.Add(this.SpringCheckBox);
            resources.ApplyResources(this.SpringPanel, "SpringPanel");
            this.SpringPanel.Name = "SpringPanel";
            // 
            // SpringEditor
            // 
            resources.ApplyResources(this.SpringEditor, "SpringEditor");
            this.SpringEditor.Name = "SpringEditor";
            this.SpringEditor.ValueChanged += new System.EventHandler(this.SpringEditor_ValueChanged);
            this.SpringEditor.SizeChanged += new System.EventHandler(this.SpringEditor_SizeChanged);
            // 
            // GearLayoutPanel
            // 
            resources.ApplyResources(this.GearLayoutPanel, "GearLayoutPanel");
            this.GearLayoutPanel.Controls.Add(this.RadiusLabel, 2, 0);
            this.GearLayoutPanel.Controls.Add(this.ToothCountLabel, 0, 0);
            this.GearLayoutPanel.Controls.Add(this.ToothNumBox, 1, 0);
            this.GearLayoutPanel.Controls.Add(this.RadiusNumBox, 3, 0);
            this.GearLayoutPanel.Name = "GearLayoutPanel";
            // 
            // RadiusLabel
            // 
            resources.ApplyResources(this.RadiusLabel, "RadiusLabel");
            this.RadiusLabel.Name = "RadiusLabel";
            // 
            // ToothCountLabel
            // 
            resources.ApplyResources(this.ToothCountLabel, "ToothCountLabel");
            this.ToothCountLabel.Name = "ToothCountLabel";
            // 
            // ToothNumBox
            // 
            this.ToothNumBox.AllowDecimals = false;
            resources.ApplyResources(this.ToothNumBox, "ToothNumBox");
            this.ToothNumBox.MaximumValue = 1000D;
            this.ToothNumBox.Name = "ToothNumBox";
            // 
            // RadiusNumBox
            // 
            resources.ApplyResources(this.RadiusNumBox, "RadiusNumBox");
            this.RadiusNumBox.MaximumValue = 1000D;
            this.RadiusNumBox.Name = "RadiusNumBox";
            // 
            // HingeLayoutPanel
            // 
            resources.ApplyResources(this.HingeLayoutPanel, "HingeLayoutPanel");
            this.HingeLayoutPanel.Controls.Add(this.LimitMinBox, 0, 2);
            this.HingeLayoutPanel.Controls.Add(this.label1, 0, 1);
            this.HingeLayoutPanel.Controls.Add(this.OrientedCheckBox, 0, 0);
            this.HingeLayoutPanel.Controls.Add(this.label2, 1, 1);
            this.HingeLayoutPanel.Controls.Add(this.LimitMaxBox, 1, 2);
            this.HingeLayoutPanel.Controls.Add(this.FlipLimitMaxBox, 3, 2);
            this.HingeLayoutPanel.Controls.Add(this.FlipLimitMinBox, 2, 2);
            this.HingeLayoutPanel.Controls.Add(this.label3, 2, 0);
            this.HingeLayoutPanel.Controls.Add(this.label4, 3, 0);
            this.HingeLayoutPanel.Name = "HingeLayoutPanel";
            // 
            // LimitMinBox
            // 
            resources.ApplyResources(this.LimitMinBox, "LimitMinBox");
            this.LimitMinBox.MaximumValue = 360D;
            this.LimitMinBox.MinimumValue = -360D;
            this.LimitMinBox.Name = "LimitMinBox";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // OrientedCheckBox
            // 
            resources.ApplyResources(this.OrientedCheckBox, "OrientedCheckBox");
            this.HingeLayoutPanel.SetColumnSpan(this.OrientedCheckBox, 2);
            this.OrientedCheckBox.Name = "OrientedCheckBox";
            this.OrientedCheckBox.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // LimitMaxBox
            // 
            resources.ApplyResources(this.LimitMaxBox, "LimitMaxBox");
            this.LimitMaxBox.MaximumValue = 360D;
            this.LimitMaxBox.MinimumValue = -360D;
            this.LimitMaxBox.Name = "LimitMaxBox";
            // 
            // FlipLimitMaxBox
            // 
            resources.ApplyResources(this.FlipLimitMaxBox, "FlipLimitMaxBox");
            this.FlipLimitMaxBox.MaximumValue = 360D;
            this.FlipLimitMaxBox.MinimumValue = -360D;
            this.FlipLimitMaxBox.Name = "FlipLimitMaxBox";
            // 
            // FlipLimitMinBox
            // 
            resources.ApplyResources(this.FlipLimitMinBox, "FlipLimitMinBox");
            this.FlipLimitMinBox.MaximumValue = 360D;
            this.FlipLimitMinBox.MinimumValue = -360D;
            this.FlipLimitMinBox.Name = "FlipLimitMinBox";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            this.HingeLayoutPanel.SetRowSpan(this.label3, 2);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            this.HingeLayoutPanel.SetRowSpan(this.label4, 2);
            // 
            // TagControlLabel
            // 
            this.TagControlLabel.Controls.Add(this.TagTextBox);
            this.TagControlLabel.LabelWidth = 80;
            resources.ApplyResources(this.TagControlLabel, "TagControlLabel");
            this.TagControlLabel.Name = "TagControlLabel";
            // 
            // TagTextBox
            // 
            resources.ApplyResources(this.TagTextBox, "TagTextBox");
            this.TagTextBox.Name = "TagTextBox";
            // 
            // TransformEdit
            // 
            resources.ApplyResources(this.TransformEdit, "TransformEdit");
            this.TransformEdit.Name = "TransformEdit";
            // 
            // ConnectionEditorPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.SelectionToolStrip);
            this.Name = "ConnectionEditorPanel";
            this.SelectionToolStrip.ResumeLayout(false);
            this.SelectionToolStrip.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.NameControlLabel.ResumeLayout(false);
            this.NameControlLabel.PerformLayout();
            this.TypeControlLabel.ResumeLayout(false);
            this.TypeControlLabel.PerformLayout();
            this.SubtypeControlLabel.ResumeLayout(false);
            this.LengthControlLabel.ResumeLayout(false);
            this.LengthControlLabel.PerformLayout();
            this.AxesControlLabel.ResumeLayout(false);
            this.AxesControlLabel.PerformLayout();
            this.CapLayoutPanel.ResumeLayout(false);
            this.CapLayoutPanel.PerformLayout();
            this.GrabbingLayoutPanel.ResumeLayout(false);
            this.GrabbingLayoutPanel.PerformLayout();
            this.SpringPanel.ResumeLayout(false);
            this.GearLayoutPanel.ResumeLayout(false);
            this.GearLayoutPanel.PerformLayout();
            this.HingeLayoutPanel.ResumeLayout(false);
            this.HingeLayoutPanel.PerformLayout();
            this.TagControlLabel.ResumeLayout(false);
            this.TagControlLabel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip SelectionToolStrip;
        private System.Windows.Forms.ToolStripLabel CurrentSelectionLabel;
        private System.Windows.Forms.ToolStripComboBox ElementsComboBox;
        private Controls.ToolStripCheckBox SyncSelectionCheckBox;
        private System.Windows.Forms.ToolStripDropDownButton AddConnectionDropDown;
        private System.Windows.Forms.TextBox ElementNameTextBox;
        private System.Windows.Forms.Label TypeValueLabel;
        private System.Windows.Forms.ComboBox ConnectionSubTypeCombo;
        private System.Windows.Forms.Panel panel1;
        private Controls.NumberTextBox LengthBox;
        private System.Windows.Forms.CheckBox SpringCheckBox;
        private Controls.ControlLabel NameControlLabel;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private Controls.ControlLabel TypeControlLabel;
        private Controls.ControlLabel LengthControlLabel;
        private Controls.ControlLabel SubtypeControlLabel;
        private System.Windows.Forms.TextBox TagTextBox;
        private System.Windows.Forms.TableLayoutPanel HingeLayoutPanel;
        private Controls.NumberTextBox FlipLimitMinBox;
        private Controls.NumberTextBox FlipLimitMaxBox;
        private System.Windows.Forms.Label label3;
        private Controls.NumberTextBox LimitMaxBox;
        private System.Windows.Forms.Label label2;
        private Controls.NumberTextBox LimitMinBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox OrientedCheckBox;
        private System.Windows.Forms.Panel SpringPanel;
        private Editors.VectorEditor SpringEditor;
        private Controls.ControlLabel TagControlLabel;
        private Controls.TransformEditor TransformEdit;
        private Controls.ControlLabel AxesControlLabel;
        private Controls.NumberTextBox AxesNumberBox;
        private System.Windows.Forms.TableLayoutPanel GearLayoutPanel;
        private System.Windows.Forms.Label RadiusLabel;
        private System.Windows.Forms.Label ToothCountLabel;
        private Controls.NumberTextBox ToothNumBox;
        private Controls.NumberTextBox RadiusNumBox;
        private System.Windows.Forms.TableLayoutPanel GrabbingLayoutPanel;
        private System.Windows.Forms.CheckBox GrabbingCheckBox;
        private System.Windows.Forms.CheckBox GrabbingRequiredCheckBox;
        private System.Windows.Forms.TableLayoutPanel CapLayoutPanel;
        private System.Windows.Forms.CheckBox EndCappedCheckBox;
        private System.Windows.Forms.CheckBox StartCappedCheckBox;
        private System.Windows.Forms.CheckBox CylindricalCheckBox;
    }
}