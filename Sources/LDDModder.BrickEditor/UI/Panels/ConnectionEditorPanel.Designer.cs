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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.TagTextBox = new System.Windows.Forms.TextBox();
            this.HingeLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.FlipLimitMinBox = new LDDModder.BrickEditor.UI.Controls.NumberTextBox();
            this.FlipLimitMaxBox = new LDDModder.BrickEditor.UI.Controls.NumberTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.LimitMaxBox = new LDDModder.BrickEditor.UI.Controls.NumberTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.LimitMinBox = new LDDModder.BrickEditor.UI.Controls.NumberTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.OrientedCheckBox = new System.Windows.Forms.CheckBox();
            this.ElementNameLabel = new System.Windows.Forms.Label();
            this.ElementTypeLabel = new System.Windows.Forms.Label();
            this.ElementNameTextBox = new System.Windows.Forms.TextBox();
            this.TypeValueLabel = new System.Windows.Forms.Label();
            this.SubtypeLabel = new System.Windows.Forms.Label();
            this.ConnectionSubTypeCombo = new System.Windows.Forms.ComboBox();
            this.TagLabel = new System.Windows.Forms.Label();
            this.LengthLabel = new System.Windows.Forms.Label();
            this.LengthBox = new LDDModder.BrickEditor.UI.Controls.NumberTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.SelectionToolStrip.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.HingeLayoutPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // SelectionToolStrip
            // 
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
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.TagTextBox, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.HingeLayoutPanel, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.ElementNameLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.ElementTypeLabel, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.ElementNameTextBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.TypeValueLabel, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.SubtypeLabel, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.ConnectionSubTypeCombo, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.TagLabel, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.LengthLabel, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.LengthBox, 1, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // TagTextBox
            // 
            resources.ApplyResources(this.TagTextBox, "TagTextBox");
            this.TagTextBox.Name = "TagTextBox";
            // 
            // HingeLayoutPanel
            // 
            resources.ApplyResources(this.HingeLayoutPanel, "HingeLayoutPanel");
            this.HingeLayoutPanel.Controls.Add(this.FlipLimitMinBox, 0, 4);
            this.HingeLayoutPanel.Controls.Add(this.FlipLimitMaxBox, 0, 4);
            this.HingeLayoutPanel.Controls.Add(this.label3, 0, 3);
            this.HingeLayoutPanel.Controls.Add(this.LimitMaxBox, 1, 2);
            this.HingeLayoutPanel.Controls.Add(this.label2, 1, 1);
            this.HingeLayoutPanel.Controls.Add(this.LimitMinBox, 0, 2);
            this.HingeLayoutPanel.Controls.Add(this.label1, 0, 1);
            this.HingeLayoutPanel.Controls.Add(this.label4, 1, 3);
            this.HingeLayoutPanel.Controls.Add(this.OrientedCheckBox, 0, 0);
            this.HingeLayoutPanel.Name = "HingeLayoutPanel";
            // 
            // FlipLimitMinBox
            // 
            resources.ApplyResources(this.FlipLimitMinBox, "FlipLimitMinBox");
            this.FlipLimitMinBox.MaximumValue = 360D;
            this.FlipLimitMinBox.MinimumValue = -360D;
            this.FlipLimitMinBox.Name = "FlipLimitMinBox";
            // 
            // FlipLimitMaxBox
            // 
            resources.ApplyResources(this.FlipLimitMaxBox, "FlipLimitMaxBox");
            this.FlipLimitMaxBox.MaximumValue = 360D;
            this.FlipLimitMaxBox.MinimumValue = -360D;
            this.FlipLimitMaxBox.Name = "FlipLimitMaxBox";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // LimitMaxBox
            // 
            resources.ApplyResources(this.LimitMaxBox, "LimitMaxBox");
            this.LimitMaxBox.MaximumValue = 360D;
            this.LimitMaxBox.MinimumValue = -360D;
            this.LimitMaxBox.Name = "LimitMaxBox";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
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
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // OrientedCheckBox
            // 
            resources.ApplyResources(this.OrientedCheckBox, "OrientedCheckBox");
            this.HingeLayoutPanel.SetColumnSpan(this.OrientedCheckBox, 2);
            this.OrientedCheckBox.Name = "OrientedCheckBox";
            this.OrientedCheckBox.UseVisualStyleBackColor = true;
            // 
            // ElementNameLabel
            // 
            resources.ApplyResources(this.ElementNameLabel, "ElementNameLabel");
            this.ElementNameLabel.Name = "ElementNameLabel";
            // 
            // ElementTypeLabel
            // 
            resources.ApplyResources(this.ElementTypeLabel, "ElementTypeLabel");
            this.ElementTypeLabel.Name = "ElementTypeLabel";
            // 
            // ElementNameTextBox
            // 
            resources.ApplyResources(this.ElementNameTextBox, "ElementNameTextBox");
            this.ElementNameTextBox.Name = "ElementNameTextBox";
            // 
            // TypeValueLabel
            // 
            resources.ApplyResources(this.TypeValueLabel, "TypeValueLabel");
            this.TypeValueLabel.Name = "TypeValueLabel";
            // 
            // SubtypeLabel
            // 
            resources.ApplyResources(this.SubtypeLabel, "SubtypeLabel");
            this.SubtypeLabel.Name = "SubtypeLabel";
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
            // TagLabel
            // 
            resources.ApplyResources(this.TagLabel, "TagLabel");
            this.TagLabel.Name = "TagLabel";
            // 
            // LengthLabel
            // 
            resources.ApplyResources(this.LengthLabel, "LengthLabel");
            this.LengthLabel.Name = "LengthLabel";
            // 
            // LengthBox
            // 
            resources.ApplyResources(this.LengthBox, "LengthBox");
            this.LengthBox.MaximumValue = 1000D;
            this.LengthBox.Name = "LengthBox";
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Name = "panel1";
            // 
            // ConnectionEditorPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.SelectionToolStrip);
            this.Name = "ConnectionEditorPanel";
            this.SelectionToolStrip.ResumeLayout(false);
            this.SelectionToolStrip.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.HingeLayoutPanel.ResumeLayout(false);
            this.HingeLayoutPanel.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip SelectionToolStrip;
        private System.Windows.Forms.ToolStripLabel CurrentSelectionLabel;
        private System.Windows.Forms.ToolStripComboBox ElementsComboBox;
        private Controls.ToolStripCheckBox SyncSelectionCheckBox;
        private System.Windows.Forms.ToolStripDropDownButton AddConnectionDropDown;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label ElementNameLabel;
        private System.Windows.Forms.Label ElementTypeLabel;
        private System.Windows.Forms.TextBox ElementNameTextBox;
        private System.Windows.Forms.Label TypeValueLabel;
        private System.Windows.Forms.Label SubtypeLabel;
        private System.Windows.Forms.ComboBox ConnectionSubTypeCombo;
        private System.Windows.Forms.TableLayoutPanel HingeLayoutPanel;
        private Controls.NumberTextBox FlipLimitMinBox;
        private Controls.NumberTextBox FlipLimitMaxBox;
        private System.Windows.Forms.Label label3;
        private Controls.NumberTextBox LimitMaxBox;
        private System.Windows.Forms.Label label2;
        private Controls.NumberTextBox LimitMinBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label TagLabel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label LengthLabel;
        private Controls.NumberTextBox LengthBox;
        private System.Windows.Forms.CheckBox OrientedCheckBox;
        private System.Windows.Forms.TextBox TagTextBox;
    }
}