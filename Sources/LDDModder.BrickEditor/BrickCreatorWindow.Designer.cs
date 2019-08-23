namespace LDDModder.BrickEditor
{
    partial class BrickCreatorWindow
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.IDTextbox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.NameTextbox = new System.Windows.Forms.TextBox();
            this.PlatformLabel = new System.Windows.Forms.Label();
            this.PlatformCombo = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.BrickMeshGridView = new System.Windows.Forms.DataGridView();
            this.AddMeshButton = new System.Windows.Forms.Button();
            this.RemoveMeshButton = new System.Windows.Forms.Button();
            this.CreateBrickButton = new System.Windows.Forms.Button();
            this.MeshFileColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MeshNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MeshInfoColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsTexturedColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.MainModelChkColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.DecorationNumberColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ClearAllButton = new System.Windows.Forms.Button();
            this.ImportExportProgress = new System.Windows.Forms.ProgressBar();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BrickMeshGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.84615F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 76.15385F));
            this.tableLayoutPanel1.Controls.Add(this.IDTextbox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.NameTextbox, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.PlatformLabel, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.PlatformCombo, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.comboBox2, 1, 3);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(302, 111);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // IDTextbox
            // 
            this.IDTextbox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.IDTextbox.Location = new System.Drawing.Point(75, 3);
            this.IDTextbox.Name = "IDTextbox";
            this.IDTextbox.Size = new System.Drawing.Size(100, 20);
            this.IDTextbox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Brick ID";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(34, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Name";
            // 
            // NameTextbox
            // 
            this.NameTextbox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.NameTextbox.Location = new System.Drawing.Point(75, 29);
            this.NameTextbox.Name = "NameTextbox";
            this.NameTextbox.Size = new System.Drawing.Size(200, 20);
            this.NameTextbox.TabIndex = 3;
            // 
            // PlatformLabel
            // 
            this.PlatformLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.PlatformLabel.AutoSize = true;
            this.PlatformLabel.Location = new System.Drawing.Point(24, 59);
            this.PlatformLabel.Name = "PlatformLabel";
            this.PlatformLabel.Size = new System.Drawing.Size(45, 13);
            this.PlatformLabel.TabIndex = 4;
            this.PlatformLabel.Text = "Platform";
            // 
            // PlatformCombo
            // 
            this.PlatformCombo.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.PlatformCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PlatformCombo.FormattingEnabled = true;
            this.PlatformCombo.Location = new System.Drawing.Point(75, 55);
            this.PlatformCombo.Name = "PlatformCombo";
            this.PlatformCombo.Size = new System.Drawing.Size(150, 21);
            this.PlatformCombo.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(33, 86);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Group";
            // 
            // comboBox2
            // 
            this.comboBox2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(75, 82);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(214, 21);
            this.comboBox2.TabIndex = 7;
            // 
            // BrickMeshGridView
            // 
            this.BrickMeshGridView.AllowUserToAddRows = false;
            this.BrickMeshGridView.AllowUserToDeleteRows = false;
            this.BrickMeshGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BrickMeshGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.BrickMeshGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.MeshFileColumn,
            this.MeshNameColumn,
            this.MeshInfoColumn,
            this.IsTexturedColumn,
            this.MainModelChkColumn,
            this.DecorationNumberColumn});
            this.BrickMeshGridView.Location = new System.Drawing.Point(12, 127);
            this.BrickMeshGridView.Name = "BrickMeshGridView";
            this.BrickMeshGridView.RowHeadersWidth = 20;
            this.BrickMeshGridView.Size = new System.Drawing.Size(619, 207);
            this.BrickMeshGridView.TabIndex = 1;
            this.BrickMeshGridView.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.BrickMeshGridView_CellEndEdit);
            this.BrickMeshGridView.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.BrickMeshGridView_CellValidating);
            this.BrickMeshGridView.SelectionChanged += new System.EventHandler(this.BrickMeshGridView_SelectionChanged);
            // 
            // AddMeshButton
            // 
            this.AddMeshButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AddMeshButton.Location = new System.Drawing.Point(12, 341);
            this.AddMeshButton.Name = "AddMeshButton";
            this.AddMeshButton.Size = new System.Drawing.Size(90, 23);
            this.AddMeshButton.TabIndex = 2;
            this.AddMeshButton.Text = "Add Mesh…";
            this.AddMeshButton.UseVisualStyleBackColor = true;
            this.AddMeshButton.Click += new System.EventHandler(this.AddMeshButton_Click);
            // 
            // RemoveMeshButton
            // 
            this.RemoveMeshButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.RemoveMeshButton.Enabled = false;
            this.RemoveMeshButton.Location = new System.Drawing.Point(108, 341);
            this.RemoveMeshButton.Name = "RemoveMeshButton";
            this.RemoveMeshButton.Size = new System.Drawing.Size(90, 23);
            this.RemoveMeshButton.TabIndex = 3;
            this.RemoveMeshButton.Text = "Remove Mesh";
            this.RemoveMeshButton.UseVisualStyleBackColor = true;
            this.RemoveMeshButton.Click += new System.EventHandler(this.RemoveMeshButton_Click);
            // 
            // CreateBrickButton
            // 
            this.CreateBrickButton.Location = new System.Drawing.Point(320, 14);
            this.CreateBrickButton.Name = "CreateBrickButton";
            this.CreateBrickButton.Size = new System.Drawing.Size(90, 23);
            this.CreateBrickButton.TabIndex = 4;
            this.CreateBrickButton.Text = "Create Brick";
            this.CreateBrickButton.UseVisualStyleBackColor = true;
            this.CreateBrickButton.Click += new System.EventHandler(this.CreateBrickButton_Click);
            // 
            // MeshFileColumn
            // 
            this.MeshFileColumn.DataPropertyName = "MeshFile";
            this.MeshFileColumn.HeaderText = "File";
            this.MeshFileColumn.Name = "MeshFileColumn";
            this.MeshFileColumn.ReadOnly = true;
            this.MeshFileColumn.Width = 150;
            // 
            // MeshNameColumn
            // 
            this.MeshNameColumn.DataPropertyName = "MeshName";
            this.MeshNameColumn.HeaderText = "Mesh Name";
            this.MeshNameColumn.Name = "MeshNameColumn";
            this.MeshNameColumn.ReadOnly = true;
            // 
            // MeshInfoColumn
            // 
            this.MeshInfoColumn.DataPropertyName = "Info";
            this.MeshInfoColumn.HeaderText = "Info";
            this.MeshInfoColumn.Name = "MeshInfoColumn";
            this.MeshInfoColumn.ReadOnly = true;
            // 
            // IsTexturedColumn
            // 
            this.IsTexturedColumn.DataPropertyName = "IsTextured";
            this.IsTexturedColumn.HeaderText = "Textured";
            this.IsTexturedColumn.Name = "IsTexturedColumn";
            this.IsTexturedColumn.ReadOnly = true;
            this.IsTexturedColumn.Width = 80;
            // 
            // MainModelChkColumn
            // 
            this.MainModelChkColumn.DataPropertyName = "IsMainModel";
            this.MainModelChkColumn.HeaderText = "Main Model";
            this.MainModelChkColumn.Name = "MainModelChkColumn";
            this.MainModelChkColumn.Width = 80;
            // 
            // DecorationNumberColumn
            // 
            this.DecorationNumberColumn.DataPropertyName = "DecorationID";
            this.DecorationNumberColumn.HeaderText = "Decoration #";
            this.DecorationNumberColumn.Name = "DecorationNumberColumn";
            this.DecorationNumberColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.DecorationNumberColumn.Width = 80;
            // 
            // ClearAllButton
            // 
            this.ClearAllButton.Location = new System.Drawing.Point(320, 40);
            this.ClearAllButton.Name = "ClearAllButton";
            this.ClearAllButton.Size = new System.Drawing.Size(90, 23);
            this.ClearAllButton.TabIndex = 5;
            this.ClearAllButton.Text = "Clear All";
            this.ClearAllButton.UseVisualStyleBackColor = true;
            this.ClearAllButton.Click += new System.EventHandler(this.ClearAllButton_Click);
            // 
            // ImportExportProgress
            // 
            this.ImportExportProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ImportExportProgress.Location = new System.Drawing.Point(472, 338);
            this.ImportExportProgress.Name = "ImportExportProgress";
            this.ImportExportProgress.Size = new System.Drawing.Size(159, 23);
            this.ImportExportProgress.TabIndex = 6;
            this.ImportExportProgress.Visible = false;
            // 
            // BrickCreatorWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(643, 373);
            this.Controls.Add(this.ImportExportProgress);
            this.Controls.Add(this.ClearAllButton);
            this.Controls.Add(this.CreateBrickButton);
            this.Controls.Add(this.RemoveMeshButton);
            this.Controls.Add(this.AddMeshButton);
            this.Controls.Add(this.BrickMeshGridView);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "BrickCreatorWindow";
            this.Text = "LDD Brick Creator";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BrickMeshGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox IDTextbox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox NameTextbox;
        private System.Windows.Forms.Label PlatformLabel;
        private System.Windows.Forms.ComboBox PlatformCombo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.DataGridView BrickMeshGridView;
        private System.Windows.Forms.Button AddMeshButton;
        private System.Windows.Forms.Button RemoveMeshButton;
        private System.Windows.Forms.Button CreateBrickButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn MeshFileColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn MeshNameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn MeshInfoColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IsTexturedColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn MainModelChkColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn DecorationNumberColumn;
        private System.Windows.Forms.Button ClearAllButton;
        private System.Windows.Forms.ProgressBar ImportExportProgress;
    }
}