namespace LDDModder.BrickEditor.UI.Windows
{
    partial class BoneDataCopyDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BoneDataCopyDialog));
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.CollisionsCheckBox = new System.Windows.Forms.CheckBox();
            this.ConnectionsCheckBox = new System.Windows.Forms.CheckBox();
            this.PhysicAttrCheckBox = new System.Windows.Forms.CheckBox();
            this.BoundingCheckBox = new System.Windows.Forms.CheckBox();
            this.SourceCombo = new System.Windows.Forms.ComboBox();
            this.TargetListBox = new System.Windows.Forms.ListBox();
            this.SourceBoneLabel = new System.Windows.Forms.Label();
            this.TargetBonesLabel = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.label1 = new System.Windows.Forms.Label();
            this.ApplyButton = new System.Windows.Forms.Button();
            this.CloseButton = new System.Windows.Forms.Button();
            this.ClearOldDataCheckBox = new System.Windows.Forms.CheckBox();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.flowLayoutPanel1.Controls.Add(this.CollisionsCheckBox);
            this.flowLayoutPanel1.Controls.Add(this.ConnectionsCheckBox);
            this.flowLayoutPanel1.Controls.Add(this.PhysicAttrCheckBox);
            this.flowLayoutPanel1.Controls.Add(this.BoundingCheckBox);
            this.flowLayoutPanel1.Controls.Add(this.ClearOldDataCheckBox);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // CollisionsCheckBox
            // 
            resources.ApplyResources(this.CollisionsCheckBox, "CollisionsCheckBox");
            this.CollisionsCheckBox.Name = "CollisionsCheckBox";
            this.CollisionsCheckBox.UseVisualStyleBackColor = true;
            // 
            // ConnectionsCheckBox
            // 
            resources.ApplyResources(this.ConnectionsCheckBox, "ConnectionsCheckBox");
            this.ConnectionsCheckBox.Name = "ConnectionsCheckBox";
            this.ConnectionsCheckBox.UseVisualStyleBackColor = true;
            // 
            // PhysicAttrCheckBox
            // 
            resources.ApplyResources(this.PhysicAttrCheckBox, "PhysicAttrCheckBox");
            this.PhysicAttrCheckBox.Name = "PhysicAttrCheckBox";
            this.PhysicAttrCheckBox.UseVisualStyleBackColor = true;
            // 
            // BoundingCheckBox
            // 
            resources.ApplyResources(this.BoundingCheckBox, "BoundingCheckBox");
            this.BoundingCheckBox.Name = "BoundingCheckBox";
            this.BoundingCheckBox.UseVisualStyleBackColor = true;
            // 
            // SourceCombo
            // 
            resources.ApplyResources(this.SourceCombo, "SourceCombo");
            this.SourceCombo.FormattingEnabled = true;
            this.SourceCombo.Name = "SourceCombo";
            this.SourceCombo.SelectedIndexChanged += new System.EventHandler(this.SourceCombo_SelectedIndexChanged);
            // 
            // TargetListBox
            // 
            resources.ApplyResources(this.TargetListBox, "TargetListBox");
            this.TargetListBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.TargetListBox.FormattingEnabled = true;
            this.TargetListBox.Name = "TargetListBox";
            this.TargetListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.TargetListBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.TargetListBox_DrawItem);
            // 
            // SourceBoneLabel
            // 
            resources.ApplyResources(this.SourceBoneLabel, "SourceBoneLabel");
            this.SourceBoneLabel.Name = "SourceBoneLabel";
            // 
            // TargetBonesLabel
            // 
            resources.ApplyResources(this.TargetBonesLabel, "TargetBonesLabel");
            this.TargetBonesLabel.Name = "TargetBonesLabel";
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.TargetBonesLabel);
            this.splitContainer1.Panel1.Controls.Add(this.SourceBoneLabel);
            this.splitContainer1.Panel1.Controls.Add(this.SourceCombo);
            this.splitContainer1.Panel1.Controls.Add(this.TargetListBox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.CloseButton);
            this.splitContainer1.Panel2.Controls.Add(this.ApplyButton);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Panel2.Controls.Add(this.flowLayoutPanel1);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // ApplyButton
            // 
            resources.ApplyResources(this.ApplyButton, "ApplyButton");
            this.ApplyButton.Name = "ApplyButton";
            this.ApplyButton.UseVisualStyleBackColor = true;
            this.ApplyButton.Click += new System.EventHandler(this.ApplyButton_Click);
            // 
            // CloseButton
            // 
            resources.ApplyResources(this.CloseButton, "CloseButton");
            this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.UseVisualStyleBackColor = true;
            // 
            // ClearOldDataCheckBox
            // 
            resources.ApplyResources(this.ClearOldDataCheckBox, "ClearOldDataCheckBox");
            this.ClearOldDataCheckBox.Checked = true;
            this.ClearOldDataCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ClearOldDataCheckBox.Name = "ClearOldDataCheckBox";
            this.ClearOldDataCheckBox.UseVisualStyleBackColor = true;
            // 
            // BoneDataCopyDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CloseButton;
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BoneDataCopyDialog";
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.CheckBox CollisionsCheckBox;
        private System.Windows.Forms.CheckBox ConnectionsCheckBox;
        private System.Windows.Forms.CheckBox PhysicAttrCheckBox;
        private System.Windows.Forms.CheckBox BoundingCheckBox;
        private System.Windows.Forms.ComboBox SourceCombo;
        private System.Windows.Forms.ListBox TargetListBox;
        private System.Windows.Forms.Label SourceBoneLabel;
        private System.Windows.Forms.Label TargetBonesLabel;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.Button ApplyButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox ClearOldDataCheckBox;
    }
}