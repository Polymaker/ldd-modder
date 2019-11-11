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
            this.button1 = new System.Windows.Forms.Button();
            this.ImportButton = new System.Windows.Forms.Button();
            this.ModelsGridView = new System.Windows.Forms.DataGridView();
            this.SelectionColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ModelNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TriangleCountColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TexturedColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.FlexibleColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.SurfaceColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ModelsGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.button1, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.ModelsGridView, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.ImportButton, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.progressBar1, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(557, 234);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Location = new System.Drawing.Point(479, 208);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // ImportButton
            // 
            this.ImportButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ImportButton.Location = new System.Drawing.Point(398, 208);
            this.ImportButton.Name = "ImportButton";
            this.ImportButton.Size = new System.Drawing.Size(75, 23);
            this.ImportButton.TabIndex = 1;
            this.ImportButton.Text = "Import";
            this.ImportButton.UseVisualStyleBackColor = true;
            this.ImportButton.Click += new System.EventHandler(this.ImportButton_Click);
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
            this.tableLayoutPanel1.SetColumnSpan(this.ModelsGridView, 3);
            this.ModelsGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ModelsGridView.Location = new System.Drawing.Point(3, 33);
            this.ModelsGridView.Name = "ModelsGridView";
            this.ModelsGridView.RowHeadersVisible = false;
            this.ModelsGridView.Size = new System.Drawing.Size(551, 169);
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
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(3, 209);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(389, 21);
            this.progressBar1.TabIndex = 3;
            // 
            // ImportModelsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(557, 234);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ImportModelsDialog";
            this.Text = "ImportModelsDialog";
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ModelsGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button ImportButton;
        private System.Windows.Forms.DataGridView ModelsGridView;
        private System.Windows.Forms.DataGridViewCheckBoxColumn SelectionColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ModelNameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn TriangleCountColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn TexturedColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn FlexibleColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn SurfaceColumn;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}