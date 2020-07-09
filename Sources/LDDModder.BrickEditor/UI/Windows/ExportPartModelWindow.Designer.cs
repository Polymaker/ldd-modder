namespace LDDModder.BrickEditor.UI.Windows
{
    partial class ExportPartModelWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExportPartModelWindow));
            this.Step1GroupBox = new System.Windows.Forms.GroupBox();
            this.PartBrowseTextBox = new LDDModder.BrickEditor.UI.Controls.BrowseTextBox();
            this.SelectPartRb = new System.Windows.Forms.RadioButton();
            this.CurrentProjectRb = new System.Windows.Forms.RadioButton();
            this.PartNameLabel = new System.Windows.Forms.Label();
            this.Step2GroupBox = new System.Windows.Forms.GroupBox();
            this.ChkRoundEdge = new System.Windows.Forms.CheckBox();
            this.ChkAltMeshes = new System.Windows.Forms.CheckBox();
            this.ChkBones = new System.Windows.Forms.CheckBox();
            this.ChkCollisions = new System.Windows.Forms.CheckBox();
            this.ChkConnections = new System.Windows.Forms.CheckBox();
            this.RbAdvancedExport = new System.Windows.Forms.RadioButton();
            this.RbSimpleExport = new System.Windows.Forms.RadioButton();
            this.RbCollada = new System.Windows.Forms.RadioButton();
            this.RbWavefront = new System.Windows.Forms.RadioButton();
            this.ExportButton = new System.Windows.Forms.Button();
            this.ReturnButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Step1GroupBox.SuspendLayout();
            this.Step2GroupBox.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Step1GroupBox
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.Step1GroupBox, 2);
            this.Step1GroupBox.Controls.Add(this.PartBrowseTextBox);
            this.Step1GroupBox.Controls.Add(this.SelectPartRb);
            this.Step1GroupBox.Controls.Add(this.CurrentProjectRb);
            this.Step1GroupBox.Controls.Add(this.PartNameLabel);
            resources.ApplyResources(this.Step1GroupBox, "Step1GroupBox");
            this.Step1GroupBox.Name = "Step1GroupBox";
            this.Step1GroupBox.TabStop = false;
            // 
            // PartBrowseTextBox
            // 
            this.PartBrowseTextBox.AutoSizeButton = true;
            this.PartBrowseTextBox.ButtonText = "...";
            this.PartBrowseTextBox.ButtonWidth = 26;
            resources.ApplyResources(this.PartBrowseTextBox, "PartBrowseTextBox");
            this.PartBrowseTextBox.Name = "PartBrowseTextBox";
            this.PartBrowseTextBox.Value = "";
            this.PartBrowseTextBox.BrowseButtonClicked += new System.EventHandler(this.PartBrowseTextBox_BrowseButtonClicked);
            this.PartBrowseTextBox.ValueChanged += new System.EventHandler(this.PartBrowseTextBox_ValueChanged);
            // 
            // SelectPartRb
            // 
            resources.ApplyResources(this.SelectPartRb, "SelectPartRb");
            this.SelectPartRb.Name = "SelectPartRb";
            this.SelectPartRb.TabStop = true;
            this.SelectPartRb.UseVisualStyleBackColor = true;
            this.SelectPartRb.CheckedChanged += new System.EventHandler(this.PartToExportRb_CheckedChanged);
            // 
            // CurrentProjectRb
            // 
            resources.ApplyResources(this.CurrentProjectRb, "CurrentProjectRb");
            this.CurrentProjectRb.Name = "CurrentProjectRb";
            this.CurrentProjectRb.TabStop = true;
            this.CurrentProjectRb.UseVisualStyleBackColor = true;
            this.CurrentProjectRb.CheckedChanged += new System.EventHandler(this.PartToExportRb_CheckedChanged);
            // 
            // PartNameLabel
            // 
            resources.ApplyResources(this.PartNameLabel, "PartNameLabel");
            this.PartNameLabel.Name = "PartNameLabel";
            // 
            // Step2GroupBox
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.Step2GroupBox, 2);
            this.Step2GroupBox.Controls.Add(this.ChkRoundEdge);
            this.Step2GroupBox.Controls.Add(this.ChkAltMeshes);
            this.Step2GroupBox.Controls.Add(this.ChkBones);
            this.Step2GroupBox.Controls.Add(this.ChkCollisions);
            this.Step2GroupBox.Controls.Add(this.ChkConnections);
            this.Step2GroupBox.Controls.Add(this.RbAdvancedExport);
            this.Step2GroupBox.Controls.Add(this.RbSimpleExport);
            resources.ApplyResources(this.Step2GroupBox, "Step2GroupBox");
            this.Step2GroupBox.Name = "Step2GroupBox";
            this.Step2GroupBox.TabStop = false;
            // 
            // ChkRoundEdge
            // 
            resources.ApplyResources(this.ChkRoundEdge, "ChkRoundEdge");
            this.ChkRoundEdge.Name = "ChkRoundEdge";
            this.ChkRoundEdge.UseVisualStyleBackColor = true;
            // 
            // ChkAltMeshes
            // 
            resources.ApplyResources(this.ChkAltMeshes, "ChkAltMeshes");
            this.ChkAltMeshes.Checked = true;
            this.ChkAltMeshes.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ChkAltMeshes.Name = "ChkAltMeshes";
            this.ChkAltMeshes.UseVisualStyleBackColor = true;
            // 
            // ChkBones
            // 
            resources.ApplyResources(this.ChkBones, "ChkBones");
            this.ChkBones.Checked = true;
            this.ChkBones.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ChkBones.Name = "ChkBones";
            this.ChkBones.UseVisualStyleBackColor = true;
            // 
            // ChkCollisions
            // 
            resources.ApplyResources(this.ChkCollisions, "ChkCollisions");
            this.ChkCollisions.Name = "ChkCollisions";
            this.ChkCollisions.UseVisualStyleBackColor = true;
            // 
            // ChkConnections
            // 
            resources.ApplyResources(this.ChkConnections, "ChkConnections");
            this.ChkConnections.Checked = true;
            this.ChkConnections.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ChkConnections.Name = "ChkConnections";
            this.ChkConnections.UseVisualStyleBackColor = true;
            // 
            // RbAdvancedExport
            // 
            resources.ApplyResources(this.RbAdvancedExport, "RbAdvancedExport");
            this.RbAdvancedExport.Checked = true;
            this.RbAdvancedExport.Name = "RbAdvancedExport";
            this.RbAdvancedExport.TabStop = true;
            this.RbAdvancedExport.UseVisualStyleBackColor = true;
            this.RbAdvancedExport.CheckedChanged += new System.EventHandler(this.RbAdvancedExport_CheckedChanged);
            // 
            // RbSimpleExport
            // 
            resources.ApplyResources(this.RbSimpleExport, "RbSimpleExport");
            this.RbSimpleExport.Name = "RbSimpleExport";
            this.RbSimpleExport.UseVisualStyleBackColor = true;
            // 
            // RbCollada
            // 
            resources.ApplyResources(this.RbCollada, "RbCollada");
            this.RbCollada.Checked = true;
            this.RbCollada.Name = "RbCollada";
            this.RbCollada.TabStop = true;
            this.RbCollada.UseVisualStyleBackColor = true;
            this.RbCollada.CheckedChanged += new System.EventHandler(this.RbCollada_CheckedChanged);
            // 
            // RbWavefront
            // 
            resources.ApplyResources(this.RbWavefront, "RbWavefront");
            this.RbWavefront.Name = "RbWavefront";
            this.RbWavefront.UseVisualStyleBackColor = true;
            // 
            // ExportButton
            // 
            resources.ApplyResources(this.ExportButton, "ExportButton");
            this.ExportButton.Name = "ExportButton";
            this.ExportButton.UseVisualStyleBackColor = true;
            this.ExportButton.Click += new System.EventHandler(this.ExportButton_Click);
            // 
            // ReturnButton
            // 
            resources.ApplyResources(this.ReturnButton, "ReturnButton");
            this.ReturnButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ReturnButton.Name = "ReturnButton";
            this.ReturnButton.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.ReturnButton, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.Step2GroupBox, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.ExportButton, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.Step1GroupBox, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // groupBox1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.groupBox1, 2);
            this.groupBox1.Controls.Add(this.RbCollada);
            this.groupBox1.Controls.Add(this.RbWavefront);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // ExportPartModelWindow
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ReturnButton;
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ExportPartModelWindow";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Step1GroupBox.ResumeLayout(false);
            this.Step1GroupBox.PerformLayout();
            this.Step2GroupBox.ResumeLayout(false);
            this.Step2GroupBox.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox Step1GroupBox;
        private System.Windows.Forms.GroupBox Step2GroupBox;
        private System.Windows.Forms.CheckBox ChkBones;
        private System.Windows.Forms.CheckBox ChkCollisions;
        private System.Windows.Forms.CheckBox ChkConnections;
        private System.Windows.Forms.RadioButton RbAdvancedExport;
        private System.Windows.Forms.RadioButton RbSimpleExport;
        private System.Windows.Forms.Label PartNameLabel;
        private System.Windows.Forms.CheckBox ChkAltMeshes;
        private System.Windows.Forms.RadioButton RbWavefront;
        private System.Windows.Forms.RadioButton RbCollada;
        private System.Windows.Forms.Button ExportButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button ReturnButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox ChkRoundEdge;
        private System.Windows.Forms.RadioButton SelectPartRb;
        private System.Windows.Forms.RadioButton CurrentProjectRb;
        private Controls.BrowseTextBox PartBrowseTextBox;
    }
}