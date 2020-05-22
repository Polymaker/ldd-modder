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
            this.Step1GroupBox = new System.Windows.Forms.GroupBox();
            this.TxtPartID = new System.Windows.Forms.TextBox();
            this.SearchPartButton = new System.Windows.Forms.Button();
            this.PartNameLabel = new System.Windows.Forms.Label();
            this.PartIDLabel = new System.Windows.Forms.Label();
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
            this.Step1GroupBox.Controls.Add(this.TxtPartID);
            this.Step1GroupBox.Controls.Add(this.SearchPartButton);
            this.Step1GroupBox.Controls.Add(this.PartNameLabel);
            this.Step1GroupBox.Controls.Add(this.PartIDLabel);
            this.Step1GroupBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.Step1GroupBox.Location = new System.Drawing.Point(3, 3);
            this.Step1GroupBox.Name = "Step1GroupBox";
            this.Step1GroupBox.Size = new System.Drawing.Size(266, 64);
            this.Step1GroupBox.TabIndex = 0;
            this.Step1GroupBox.TabStop = false;
            this.Step1GroupBox.Text = "1 - Part to export";
            // 
            // TxtPartID
            // 
            this.TxtPartID.Location = new System.Drawing.Point(55, 20);
            this.TxtPartID.Name = "TxtPartID";
            this.TxtPartID.Size = new System.Drawing.Size(80, 20);
            this.TxtPartID.TabIndex = 4;
            this.TxtPartID.Validated += new System.EventHandler(this.TxtPartID_Validated);
            // 
            // SearchPartButton
            // 
            this.SearchPartButton.Location = new System.Drawing.Point(139, 19);
            this.SearchPartButton.Name = "SearchPartButton";
            this.SearchPartButton.Size = new System.Drawing.Size(77, 22);
            this.SearchPartButton.TabIndex = 3;
            this.SearchPartButton.Text = "Select part...";
            this.SearchPartButton.UseVisualStyleBackColor = true;
            this.SearchPartButton.Click += new System.EventHandler(this.SearchPartButton_Click);
            // 
            // PartNameLabel
            // 
            this.PartNameLabel.AutoSize = true;
            this.PartNameLabel.Location = new System.Drawing.Point(52, 44);
            this.PartNameLabel.Name = "PartNameLabel";
            this.PartNameLabel.Size = new System.Drawing.Size(35, 13);
            this.PartNameLabel.TabIndex = 2;
            this.PartNameLabel.Text = "label2";
            // 
            // PartIDLabel
            // 
            this.PartIDLabel.AutoSize = true;
            this.PartIDLabel.Location = new System.Drawing.Point(9, 23);
            this.PartIDLabel.Name = "PartIDLabel";
            this.PartIDLabel.Size = new System.Drawing.Size(40, 13);
            this.PartIDLabel.TabIndex = 0;
            this.PartIDLabel.Text = "Part ID";
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
            this.Step2GroupBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.Step2GroupBox.Location = new System.Drawing.Point(3, 73);
            this.Step2GroupBox.Name = "Step2GroupBox";
            this.Step2GroupBox.Size = new System.Drawing.Size(266, 108);
            this.Step2GroupBox.TabIndex = 1;
            this.Step2GroupBox.TabStop = false;
            this.Step2GroupBox.Text = "2 - Export options";
            // 
            // ChkRoundEdge
            // 
            this.ChkRoundEdge.AutoSize = true;
            this.ChkRoundEdge.Location = new System.Drawing.Point(12, 84);
            this.ChkRoundEdge.Name = "ChkRoundEdge";
            this.ChkRoundEdge.Size = new System.Drawing.Size(159, 17);
            this.ChkRoundEdge.TabIndex = 6;
            this.ChkRoundEdge.Text = "RoundEdge data (UV maps)";
            this.ChkRoundEdge.UseVisualStyleBackColor = true;
            // 
            // ChkAltMeshes
            // 
            this.ChkAltMeshes.AutoSize = true;
            this.ChkAltMeshes.Checked = true;
            this.ChkAltMeshes.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ChkAltMeshes.Location = new System.Drawing.Point(144, 62);
            this.ChkAltMeshes.Name = "ChkAltMeshes";
            this.ChkAltMeshes.Size = new System.Drawing.Size(117, 17);
            this.ChkAltMeshes.TabIndex = 5;
            this.ChkAltMeshes.Text = "Include alt. meshes";
            this.ChkAltMeshes.UseVisualStyleBackColor = true;
            // 
            // ChkBones
            // 
            this.ChkBones.AutoSize = true;
            this.ChkBones.Checked = true;
            this.ChkBones.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ChkBones.Location = new System.Drawing.Point(12, 62);
            this.ChkBones.Name = "ChkBones";
            this.ChkBones.Size = new System.Drawing.Size(93, 17);
            this.ChkBones.TabIndex = 4;
            this.ChkBones.Text = "Include bones";
            this.ChkBones.UseVisualStyleBackColor = true;
            // 
            // ChkCollisions
            // 
            this.ChkCollisions.AutoSize = true;
            this.ChkCollisions.Location = new System.Drawing.Point(144, 40);
            this.ChkCollisions.Name = "ChkCollisions";
            this.ChkCollisions.Size = new System.Drawing.Size(106, 17);
            this.ChkCollisions.TabIndex = 3;
            this.ChkCollisions.Text = "Include collisions";
            this.ChkCollisions.UseVisualStyleBackColor = true;
            // 
            // ChkConnections
            // 
            this.ChkConnections.AutoSize = true;
            this.ChkConnections.Checked = true;
            this.ChkConnections.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ChkConnections.Location = new System.Drawing.Point(12, 40);
            this.ChkConnections.Name = "ChkConnections";
            this.ChkConnections.Size = new System.Drawing.Size(122, 17);
            this.ChkConnections.TabIndex = 2;
            this.ChkConnections.Text = "Include connections";
            this.ChkConnections.UseVisualStyleBackColor = true;
            // 
            // RbAdvancedExport
            // 
            this.RbAdvancedExport.AutoSize = true;
            this.RbAdvancedExport.Checked = true;
            this.RbAdvancedExport.Location = new System.Drawing.Point(144, 18);
            this.RbAdvancedExport.Name = "RbAdvancedExport";
            this.RbAdvancedExport.Size = new System.Drawing.Size(103, 17);
            this.RbAdvancedExport.TabIndex = 1;
            this.RbAdvancedExport.TabStop = true;
            this.RbAdvancedExport.Text = "Advanced mode";
            this.RbAdvancedExport.UseVisualStyleBackColor = true;
            this.RbAdvancedExport.CheckedChanged += new System.EventHandler(this.RbAdvancedExport_CheckedChanged);
            // 
            // RbSimpleExport
            // 
            this.RbSimpleExport.AutoSize = true;
            this.RbSimpleExport.Location = new System.Drawing.Point(12, 18);
            this.RbSimpleExport.Name = "RbSimpleExport";
            this.RbSimpleExport.Size = new System.Drawing.Size(97, 17);
            this.RbSimpleExport.TabIndex = 0;
            this.RbSimpleExport.Text = "Part model only";
            this.RbSimpleExport.UseVisualStyleBackColor = true;
            // 
            // RbCollada
            // 
            this.RbCollada.AutoSize = true;
            this.RbCollada.Checked = true;
            this.RbCollada.Location = new System.Drawing.Point(144, 19);
            this.RbCollada.Name = "RbCollada";
            this.RbCollada.Size = new System.Drawing.Size(94, 17);
            this.RbCollada.TabIndex = 2;
            this.RbCollada.TabStop = true;
            this.RbCollada.Text = "Collada (*.dae)";
            this.RbCollada.UseVisualStyleBackColor = true;
            this.RbCollada.CheckedChanged += new System.EventHandler(this.RbCollada_CheckedChanged);
            // 
            // RbWavefront
            // 
            this.RbWavefront.AutoSize = true;
            this.RbWavefront.Location = new System.Drawing.Point(12, 19);
            this.RbWavefront.Name = "RbWavefront";
            this.RbWavefront.Size = new System.Drawing.Size(105, 17);
            this.RbWavefront.TabIndex = 1;
            this.RbWavefront.Text = "Wavefront (*.obj)";
            this.RbWavefront.UseVisualStyleBackColor = true;
            // 
            // ExportButton
            // 
            this.ExportButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ExportButton.Enabled = false;
            this.ExportButton.Location = new System.Drawing.Point(2, 238);
            this.ExportButton.Margin = new System.Windows.Forms.Padding(2, 3, 3, 3);
            this.ExportButton.Name = "ExportButton";
            this.ExportButton.Size = new System.Drawing.Size(75, 23);
            this.ExportButton.TabIndex = 3;
            this.ExportButton.Text = "Export";
            this.ExportButton.UseVisualStyleBackColor = true;
            this.ExportButton.Click += new System.EventHandler(this.ExportButton_Click);
            // 
            // ReturnButton
            // 
            this.ReturnButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ReturnButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ReturnButton.Location = new System.Drawing.Point(195, 238);
            this.ReturnButton.Margin = new System.Windows.Forms.Padding(3, 3, 2, 3);
            this.ReturnButton.Name = "ReturnButton";
            this.ReturnButton.Size = new System.Drawing.Size(75, 23);
            this.ReturnButton.TabIndex = 3;
            this.ReturnButton.Text = "Back";
            this.ReturnButton.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.ReturnButton, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.Step2GroupBox, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.ExportButton, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.Step1GroupBox, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(6, 6);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(272, 264);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // groupBox1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.groupBox1, 2);
            this.groupBox1.Controls.Add(this.RbCollada);
            this.groupBox1.Controls.Add(this.RbWavefront);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(3, 187);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(266, 42);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "3 - Export format";
            // 
            // ExportPartModelWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ReturnButton;
            this.ClientSize = new System.Drawing.Size(284, 276);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MinimumSize = new System.Drawing.Size(300, 315);
            this.Name = "ExportPartModelWindow";
            this.Padding = new System.Windows.Forms.Padding(6);
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Export LDD Model";
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
        private System.Windows.Forms.Label PartIDLabel;
        private System.Windows.Forms.GroupBox Step2GroupBox;
        private System.Windows.Forms.CheckBox ChkBones;
        private System.Windows.Forms.CheckBox ChkCollisions;
        private System.Windows.Forms.CheckBox ChkConnections;
        private System.Windows.Forms.RadioButton RbAdvancedExport;
        private System.Windows.Forms.RadioButton RbSimpleExport;
        private System.Windows.Forms.TextBox TxtPartID;
        private System.Windows.Forms.Button SearchPartButton;
        private System.Windows.Forms.Label PartNameLabel;
        private System.Windows.Forms.CheckBox ChkAltMeshes;
        private System.Windows.Forms.RadioButton RbWavefront;
        private System.Windows.Forms.RadioButton RbCollada;
        private System.Windows.Forms.Button ExportButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button ReturnButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox ChkRoundEdge;
    }
}