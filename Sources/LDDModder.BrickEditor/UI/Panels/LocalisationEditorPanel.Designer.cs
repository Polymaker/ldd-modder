namespace LDDModder.BrickEditor.UI.Panels
{
    partial class LocalisationEditorPanel
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.AvailableLanguagesGrid = new System.Windows.Forms.DataGridView();
            this.LanguageKeyColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LanguageNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LanguageEnabledColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.LanguageMaterialExistColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.LanguageAppExistColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.UpdateAvailableLanguagesButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.AppTextKeyColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.MatTextKeyColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.AvailableLanguagesGrid)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // AvailableLanguageGrid
            // 
            this.AvailableLanguagesGrid.AllowUserToAddRows = false;
            this.AvailableLanguagesGrid.AllowUserToDeleteRows = false;
            this.AvailableLanguagesGrid.AllowUserToResizeRows = false;
            this.AvailableLanguagesGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AvailableLanguagesGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.AvailableLanguagesGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.LanguageKeyColumn,
            this.LanguageNameColumn,
            this.LanguageEnabledColumn,
            this.LanguageMaterialExistColumn,
            this.LanguageAppExistColumn});
            this.AvailableLanguagesGrid.Location = new System.Drawing.Point(6, 19);
            this.AvailableLanguagesGrid.Name = "AvailableLanguageGrid";
            this.AvailableLanguagesGrid.RowHeadersVisible = false;
            this.AvailableLanguagesGrid.Size = new System.Drawing.Size(472, 117);
            this.AvailableLanguagesGrid.TabIndex = 1;
            this.AvailableLanguagesGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.AvailableLanguagesGrid_CellContentClick);
            // 
            // LanguageKeyColumn
            // 
            this.LanguageKeyColumn.DataPropertyName = "Key";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.LanguageKeyColumn.DefaultCellStyle = dataGridViewCellStyle1;
            this.LanguageKeyColumn.HeaderText = "Key";
            this.LanguageKeyColumn.Name = "LanguageKeyColumn";
            this.LanguageKeyColumn.ReadOnly = true;
            this.LanguageKeyColumn.Width = 60;
            // 
            // LanguageNameColumn
            // 
            this.LanguageNameColumn.DataPropertyName = "Name";
            this.LanguageNameColumn.HeaderText = "Name";
            this.LanguageNameColumn.Name = "LanguageNameColumn";
            this.LanguageNameColumn.Width = 140;
            // 
            // LanguageEnabledColumn
            // 
            this.LanguageEnabledColumn.DataPropertyName = "Enabled";
            this.LanguageEnabledColumn.HeaderText = "Enabled";
            this.LanguageEnabledColumn.Name = "LanguageEnabledColumn";
            this.LanguageEnabledColumn.Width = 65;
            // 
            // LanguageMaterialExistColumn
            // 
            this.LanguageMaterialExistColumn.DataPropertyName = "MatFileExist";
            this.LanguageMaterialExistColumn.HeaderText = "Materials Translated";
            this.LanguageMaterialExistColumn.Name = "LanguageMaterialExistColumn";
            this.LanguageMaterialExistColumn.ReadOnly = true;
            this.LanguageMaterialExistColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.LanguageMaterialExistColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // LanguageAppExistColumn
            // 
            this.LanguageAppExistColumn.DataPropertyName = "AppFileExist";
            this.LanguageAppExistColumn.HeaderText = "Application Translated";
            this.LanguageAppExistColumn.Name = "LanguageAppExistColumn";
            this.LanguageAppExistColumn.ReadOnly = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.UpdateAvailableLanguagesButton);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.AvailableLanguagesGrid);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(589, 142);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Available Languages";
            // 
            // UpdateAvailableLanguagesButton
            // 
            this.UpdateAvailableLanguagesButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.UpdateAvailableLanguagesButton.Location = new System.Drawing.Point(484, 48);
            this.UpdateAvailableLanguagesButton.Name = "UpdateAvailableLanguagesButton";
            this.UpdateAvailableLanguagesButton.Size = new System.Drawing.Size(96, 23);
            this.UpdateAvailableLanguagesButton.TabIndex = 3;
            this.UpdateAvailableLanguagesButton.Text = "Apply";
            this.UpdateAvailableLanguagesButton.UseVisualStyleBackColor = true;
            this.UpdateAvailableLanguagesButton.Click += new System.EventHandler(this.UpdateAvailableLanguagesButton_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(484, 18);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(96, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Add language";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(595, 404);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 151);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(589, 250);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dataGridView2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(581, 224);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Application texts";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.AllowUserToDeleteRows = false;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.AppTextKeyColumn});
            this.dataGridView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView2.Location = new System.Drawing.Point(3, 3);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.Size = new System.Drawing.Size(575, 218);
            this.dataGridView2.TabIndex = 1;
            // 
            // AppTextKeyColumn
            // 
            this.AppTextKeyColumn.DataPropertyName = "Key";
            this.AppTextKeyColumn.HeaderText = "Key";
            this.AppTextKeyColumn.Name = "AppTextKeyColumn";
            this.AppTextKeyColumn.ReadOnly = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dataGridView1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(581, 224);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Material names";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.MatTextKeyColumn});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(575, 218);
            this.dataGridView1.TabIndex = 0;
            // 
            // MatTextKeyColumn
            // 
            this.MatTextKeyColumn.HeaderText = "Key";
            this.MatTextKeyColumn.Name = "MatTextKeyColumn";
            this.MatTextKeyColumn.ReadOnly = true;
            // 
            // LocalisationEditorPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(595, 404);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "LocalisationEditorPanel";
            this.Text = "LocalisationEditorPanel";
            ((System.ComponentModel.ISupportInitialize)(this.AvailableLanguagesGrid)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView AvailableLanguagesGrid;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button UpdateAvailableLanguagesButton;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn MatTextKeyColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn LanguageKeyColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn LanguageNameColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn LanguageEnabledColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn LanguageMaterialExistColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn LanguageAppExistColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn AppTextKeyColumn;
    }
}