namespace LDDModder.BrickEditor.UI.Windows
{
    partial class ImportModelsDialog
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
            this.browseTextBox1 = new LDDModder.BrickEditor.UI.Controls.BrowseTextBox();
            this.ModelsGridView = new System.Windows.Forms.DataGridView();
            this.SelectionColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ModelNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TriangleCountColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TexturedColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.FlexibleColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.SurfaceColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.ImportButton = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.ReturnButton = new System.Windows.Forms.Button();
            this.CheckUncheckButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ModelsGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.browseTextBox1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.ModelsGridView, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.ImportButton, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.progressBar1, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.ReturnButton, 3, 3);
            this.tableLayoutPanel1.Controls.Add(this.CheckUncheckButton, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(574, 276);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // browseTextBox1
            // 
            this.browseTextBox1.AutoSizeButton = true;
            this.browseTextBox1.ButtonText = "Select model...";
            this.browseTextBox1.ButtonWidth = 87;
            this.tableLayoutPanel1.SetColumnSpan(this.browseTextBox1, 3);
            this.browseTextBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.browseTextBox1.Location = new System.Drawing.Point(75, 3);
            this.browseTextBox1.Name = "browseTextBox1";
            this.browseTextBox1.ReadOnly = true;
            this.browseTextBox1.Size = new System.Drawing.Size(496, 20);
            this.browseTextBox1.TabIndex = 2;
            this.browseTextBox1.Value = "";
            this.browseTextBox1.BrowseButtonClicked += new System.EventHandler(this.browseTextBox1_BrowseButtonClicked);
            // 
            // ModelsGridView
            // 
            this.ModelsGridView.AllowUserToAddRows = false;
            this.ModelsGridView.AllowUserToDeleteRows = false;
            this.ModelsGridView.AllowUserToResizeRows = false;
            this.ModelsGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ModelsGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SelectionColumn,
            this.ModelNameColumn,
            this.TriangleCountColumn,
            this.TexturedColumn,
            this.FlexibleColumn,
            this.SurfaceColumn});
            this.tableLayoutPanel1.SetColumnSpan(this.ModelsGridView, 4);
            this.ModelsGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ModelsGridView.Location = new System.Drawing.Point(3, 58);
            this.ModelsGridView.Name = "ModelsGridView";
            this.ModelsGridView.RowHeadersVisible = false;
            this.ModelsGridView.Size = new System.Drawing.Size(568, 186);
            this.ModelsGridView.TabIndex = 2;
            this.ModelsGridView.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.ModelsGridView_CellEnter);
            this.ModelsGridView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.ModelsGridView_CellValueChanged);
            // 
            // SelectionColumn
            // 
            this.SelectionColumn.DataPropertyName = "Selected";
            this.SelectionColumn.HeaderText = "Import";
            this.SelectionColumn.Name = "SelectionColumn";
            this.SelectionColumn.Width = 50;
            // 
            // ModelNameColumn
            // 
            this.ModelNameColumn.DataPropertyName = "Name";
            this.ModelNameColumn.HeaderText = "Model Name";
            this.ModelNameColumn.Name = "ModelNameColumn";
            this.ModelNameColumn.ReadOnly = true;
            // 
            // TriangleCountColumn
            // 
            this.TriangleCountColumn.DataPropertyName = "TriangleCount";
            this.TriangleCountColumn.HeaderText = "Triangle Count";
            this.TriangleCountColumn.Name = "TriangleCountColumn";
            this.TriangleCountColumn.ReadOnly = true;
            // 
            // TexturedColumn
            // 
            this.TexturedColumn.DataPropertyName = "IsTextured";
            this.TexturedColumn.HeaderText = "Textured";
            this.TexturedColumn.Name = "TexturedColumn";
            this.TexturedColumn.ReadOnly = true;
            this.TexturedColumn.Width = 70;
            // 
            // FlexibleColumn
            // 
            this.FlexibleColumn.DataPropertyName = "IsFlexible";
            this.FlexibleColumn.HeaderText = "Flexible";
            this.FlexibleColumn.Name = "FlexibleColumn";
            this.FlexibleColumn.ReadOnly = true;
            this.FlexibleColumn.Width = 70;
            // 
            // SurfaceColumn
            // 
            this.SurfaceColumn.DataPropertyName = "SurfaceID";
            this.SurfaceColumn.HeaderText = "Destination Surface";
            this.SurfaceColumn.Name = "SurfaceColumn";
            this.SurfaceColumn.Width = 150;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "File to import";
            // 
            // ImportButton
            // 
            this.ImportButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ImportButton.Location = new System.Drawing.Point(415, 250);
            this.ImportButton.Name = "ImportButton";
            this.ImportButton.Size = new System.Drawing.Size(75, 23);
            this.ImportButton.TabIndex = 1;
            this.ImportButton.Text = "Import";
            this.ImportButton.UseVisualStyleBackColor = true;
            this.ImportButton.Click += new System.EventHandler(this.ImportButton_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.progressBar1, 2);
            this.progressBar1.Location = new System.Drawing.Point(3, 251);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(406, 21);
            this.progressBar1.TabIndex = 3;
            // 
            // ReturnButton
            // 
            this.ReturnButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ReturnButton.Location = new System.Drawing.Point(496, 250);
            this.ReturnButton.Name = "ReturnButton";
            this.ReturnButton.Size = new System.Drawing.Size(75, 23);
            this.ReturnButton.TabIndex = 0;
            this.ReturnButton.Text = "Cancel";
            this.ReturnButton.UseVisualStyleBackColor = true;
            // 
            // CheckUncheckButton
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.CheckUncheckButton, 2);
            this.CheckUncheckButton.Location = new System.Drawing.Point(3, 29);
            this.CheckUncheckButton.Name = "CheckUncheckButton";
            this.CheckUncheckButton.Size = new System.Drawing.Size(115, 23);
            this.CheckUncheckButton.TabIndex = 4;
            this.CheckUncheckButton.Text = "Check/Uncheck all";
            this.CheckUncheckButton.UseVisualStyleBackColor = true;
            this.CheckUncheckButton.Click += new System.EventHandler(this.CheckUncheckButton_Click);
            // 
            // ImportModelsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ReturnButton;
            this.ClientSize = new System.Drawing.Size(574, 276);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ImportModelsDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Import models";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ModelsGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button ReturnButton;
        private System.Windows.Forms.DataGridView ModelsGridView;
        private System.Windows.Forms.DataGridViewCheckBoxColumn SelectionColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ModelNameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn TriangleCountColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn TexturedColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn FlexibleColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn SurfaceColumn;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label1;
        private Controls.BrowseTextBox browseTextBox1;
        private System.Windows.Forms.Button ImportButton;
        private System.Windows.Forms.Button CheckUncheckButton;
    }
}