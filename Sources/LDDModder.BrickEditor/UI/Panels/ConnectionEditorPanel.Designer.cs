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
            this.ElementNameLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ElementNameTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.SelectionToolStrip.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
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
            this.tableLayoutPanel1.Controls.Add(this.ElementNameLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.ElementNameTextBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label3, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.comboBox1, 1, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // ElementNameLabel
            // 
            resources.ApplyResources(this.ElementNameLabel, "ElementNameLabel");
            this.ElementNameLabel.Name = "ElementNameLabel";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // ElementNameTextBox
            // 
            resources.ApplyResources(this.ElementNameTextBox, "ElementNameTextBox");
            this.ElementNameTextBox.Name = "ElementNameTextBox";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            resources.ApplyResources(this.comboBox1, "comboBox1");
            this.comboBox1.Name = "comboBox1";
            // 
            // ConnectionEditorPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.SelectionToolStrip);
            this.Name = "ConnectionEditorPanel";
            this.SelectionToolStrip.ResumeLayout(false);
            this.SelectionToolStrip.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
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
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox ElementNameTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}