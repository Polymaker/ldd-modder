namespace LDDModder.BrickEditor.UI.Panels
{
    partial class ElementDetailPanel
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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.SelectedElementLabel = new System.Windows.Forms.ToolStripLabel();
            this.SelectedElementComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.SyncSelectionCheckBox = new LDDModder.BrickEditor.UI.Controls.ToolStripCheckBox();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SelectedElementLabel,
            this.SelectedElementComboBox,
            this.SyncSelectionCheckBox});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(618, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // SelectedElementLabel
            // 
            this.SelectedElementLabel.Name = "SelectedElementLabel";
            this.SelectedElementLabel.Size = new System.Drawing.Size(100, 22);
            this.SelectedElementLabel.Text = "Selected element:";
            // 
            // SelectedElementComboBox
            // 
            this.SelectedElementComboBox.Name = "SelectedElementComboBox";
            this.SelectedElementComboBox.Size = new System.Drawing.Size(150, 25);
            this.SelectedElementComboBox.SelectedIndexChanged += new System.EventHandler(this.SelectedElementComboBox_SelectedIndexChanged);
            // 
            // SyncSelectionCheckBox
            // 
            this.SyncSelectionCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.SyncSelectionCheckBox.Name = "SyncSelectionCheckBox";
            this.SyncSelectionCheckBox.Size = new System.Drawing.Size(165, 22);
            this.SyncSelectionCheckBox.Text = "Synchronise with viewport";
            // 
            // ElementDetailPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(618, 325);
            this.Controls.Add(this.toolStrip1);
            this.Name = "ElementDetailPanel";
            this.Text = "ElementDetailPanel";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel SelectedElementLabel;
        private System.Windows.Forms.ToolStripComboBox SelectedElementComboBox;
        private Controls.ToolStripCheckBox SyncSelectionCheckBox;
    }
}