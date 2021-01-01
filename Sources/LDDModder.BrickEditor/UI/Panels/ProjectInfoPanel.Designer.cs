namespace LDDModder.BrickEditor.UI.Panels
{
    partial class ProjectInfoPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProjectInfoPanel));
            this.CreatedByBox = new System.Windows.Forms.TextBox();
            this.OriginalAuthorBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.CommentBox = new System.Windows.Forms.TextBox();
            this.DerivedFromCombo = new System.Windows.Forms.ComboBox();
            this.OriginalAuthorLabel = new LDDModder.BrickEditor.UI.Controls.ControlLabel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.DerivedFromLabel = new LDDModder.BrickEditor.UI.Controls.ControlLabel();
            this.CreatedByLabel = new LDDModder.BrickEditor.UI.Controls.ControlLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.OriginalAuthorLabel.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.DerivedFromLabel.SuspendLayout();
            this.CreatedByLabel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // CreatedByBox
            // 
            resources.ApplyResources(this.CreatedByBox, "CreatedByBox");
            this.CreatedByBox.Name = "CreatedByBox";
            // 
            // OriginalAuthorBox
            // 
            resources.ApplyResources(this.OriginalAuthorBox, "OriginalAuthorBox");
            this.OriginalAuthorBox.Name = "OriginalAuthorBox";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // CommentBox
            // 
            this.CommentBox.AcceptsReturn = true;
            this.CommentBox.AcceptsTab = true;
            resources.ApplyResources(this.CommentBox, "CommentBox");
            this.CommentBox.Name = "CommentBox";
            // 
            // DerivedFromCombo
            // 
            this.DerivedFromCombo.FormattingEnabled = true;
            this.DerivedFromCombo.Items.AddRange(new object[] {
            resources.GetString("DerivedFromCombo.Items")});
            resources.ApplyResources(this.DerivedFromCombo, "DerivedFromCombo");
            this.DerivedFromCombo.Name = "DerivedFromCombo";
            this.DerivedFromCombo.TextChanged += new System.EventHandler(this.DerivedFromCombo_TextChanged);
            // 
            // OriginalAuthorLabel
            // 
            this.OriginalAuthorLabel.AutoSizeWidth = true;
            this.OriginalAuthorLabel.Controls.Add(this.OriginalAuthorBox);
            this.OriginalAuthorLabel.LabelWidth = 90;
            resources.ApplyResources(this.OriginalAuthorLabel, "OriginalAuthorLabel");
            this.OriginalAuthorLabel.Name = "OriginalAuthorLabel";
            // 
            // flowLayoutPanel1
            // 
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.flowLayoutPanel1.Controls.Add(this.DerivedFromLabel);
            this.flowLayoutPanel1.Controls.Add(this.OriginalAuthorLabel);
            this.flowLayoutPanel1.Controls.Add(this.CreatedByLabel);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // DerivedFromLabel
            // 
            this.DerivedFromLabel.AutoSizeWidth = true;
            this.DerivedFromLabel.Controls.Add(this.DerivedFromCombo);
            this.DerivedFromLabel.LabelWidth = 90;
            resources.ApplyResources(this.DerivedFromLabel, "DerivedFromLabel");
            this.DerivedFromLabel.Name = "DerivedFromLabel";
            // 
            // CreatedByLabel
            // 
            this.CreatedByLabel.AutoSizeWidth = true;
            this.CreatedByLabel.Controls.Add(this.CreatedByBox);
            this.CreatedByLabel.LabelWidth = 90;
            resources.ApplyResources(this.CreatedByLabel, "CreatedByLabel");
            this.CreatedByLabel.Name = "CreatedByLabel";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.CommentBox);
            this.panel1.Controls.Add(this.label3);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // ProjectInfoPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "ProjectInfoPanel";
            this.OriginalAuthorLabel.ResumeLayout(false);
            this.OriginalAuthorLabel.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.DerivedFromLabel.ResumeLayout(false);
            this.CreatedByLabel.ResumeLayout(false);
            this.CreatedByLabel.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox CreatedByBox;
        private System.Windows.Forms.TextBox OriginalAuthorBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox CommentBox;
        private System.Windows.Forms.ComboBox DerivedFromCombo;
        private Controls.ControlLabel OriginalAuthorLabel;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private Controls.ControlLabel DerivedFromLabel;
        private Controls.ControlLabel CreatedByLabel;
        private System.Windows.Forms.Panel panel1;
    }
}