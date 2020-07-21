namespace LDDModder.BrickEditor.UI.Panels
{
    partial class StudConnectionPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StudConnectionPanel));
            this.studConnectionEditor1 = new LDDModder.BrickEditor.UI.Editors.StudConnectionEditor();
            this.SelectionToolStrip = new System.Windows.Forms.ToolStrip();
            this.CurrentSelectionLabel = new System.Windows.Forms.ToolStripLabel();
            this.ConnectorComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.SyncSelectionCheckBox = new LDDModder.BrickEditor.UI.Controls.ToolStripCheckBox();
            this.SelectionToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // studConnectionEditor1
            // 
            resources.ApplyResources(this.studConnectionEditor1, "studConnectionEditor1");
            this.studConnectionEditor1.Name = "studConnectionEditor1";
            // 
            // SelectionToolStrip
            // 
            this.SelectionToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.SelectionToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CurrentSelectionLabel,
            this.ConnectorComboBox,
            this.SyncSelectionCheckBox});
            resources.ApplyResources(this.SelectionToolStrip, "SelectionToolStrip");
            this.SelectionToolStrip.Name = "SelectionToolStrip";
            // 
            // CurrentSelectionLabel
            // 
            this.CurrentSelectionLabel.Name = "CurrentSelectionLabel";
            resources.ApplyResources(this.CurrentSelectionLabel, "CurrentSelectionLabel");
            // 
            // ConnectorComboBox
            // 
            this.ConnectorComboBox.Name = "ConnectorComboBox";
            resources.ApplyResources(this.ConnectorComboBox, "ConnectorComboBox");
            this.ConnectorComboBox.SelectedIndexChanged += new System.EventHandler(this.ConnectorComboBox_SelectedIndexChanged);
            // 
            // SyncSelectionCheckBox
            // 
            this.SyncSelectionCheckBox.Margin = new System.Windows.Forms.Padding(6, 1, 0, 2);
            this.SyncSelectionCheckBox.Name = "SyncSelectionCheckBox";
            resources.ApplyResources(this.SyncSelectionCheckBox, "SyncSelectionCheckBox");
            this.SyncSelectionCheckBox.Click += new System.EventHandler(this.SyncSelectionCheckBox_Click);
            // 
            // StudConnectionPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.studConnectionEditor1);
            this.Controls.Add(this.SelectionToolStrip);
            this.Name = "StudConnectionPanel";
            this.SelectionToolStrip.ResumeLayout(false);
            this.SelectionToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Editors.StudConnectionEditor studConnectionEditor1;
        private System.Windows.Forms.ToolStrip SelectionToolStrip;
        private System.Windows.Forms.ToolStripLabel CurrentSelectionLabel;
        private System.Windows.Forms.ToolStripComboBox ConnectorComboBox;
        private Controls.ToolStripCheckBox SyncSelectionCheckBox;
    }
}