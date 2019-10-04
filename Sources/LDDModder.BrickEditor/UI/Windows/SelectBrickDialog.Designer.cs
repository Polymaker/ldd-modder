namespace LDDModder.BrickEditor.UI.Windows
{
    partial class SelectBrickDialog
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.CancelDialogButton = new System.Windows.Forms.Button();
            this.OpenButton = new System.Windows.Forms.Button();
            this.LoadingProgressBar = new System.Windows.Forms.ProgressBar();
            this.BrickGridView = new System.Windows.Forms.DataGridView();
            this.SearchTextBox = new System.Windows.Forms.TextBox();
            this.RefreshTimer = new System.Windows.Forms.Timer(this.components);
            this.GridColumnsContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.PartIdColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DescriptionColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PlatformColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CategoryColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DecoratedColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.FlexibleColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BrickGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.CancelDialogButton, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.OpenButton, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.LoadingProgressBar, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.BrickGridView, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.SearchTextBox, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(628, 405);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // CancelDialogButton
            // 
            this.CancelDialogButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelDialogButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelDialogButton.Location = new System.Drawing.Point(550, 379);
            this.CancelDialogButton.Name = "CancelDialogButton";
            this.CancelDialogButton.Size = new System.Drawing.Size(75, 23);
            this.CancelDialogButton.TabIndex = 2;
            this.CancelDialogButton.Text = "Cancel";
            this.CancelDialogButton.UseVisualStyleBackColor = true;
            // 
            // OpenButton
            // 
            this.OpenButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OpenButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OpenButton.Enabled = false;
            this.OpenButton.Location = new System.Drawing.Point(469, 379);
            this.OpenButton.Name = "OpenButton";
            this.OpenButton.Size = new System.Drawing.Size(75, 23);
            this.OpenButton.TabIndex = 1;
            this.OpenButton.Text = "Open";
            this.OpenButton.UseVisualStyleBackColor = true;
            // 
            // LoadingProgressBar
            // 
            this.LoadingProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LoadingProgressBar.Location = new System.Drawing.Point(3, 379);
            this.LoadingProgressBar.Name = "LoadingProgressBar";
            this.LoadingProgressBar.Size = new System.Drawing.Size(460, 23);
            this.LoadingProgressBar.TabIndex = 3;
            // 
            // BrickGridView
            // 
            this.BrickGridView.AllowUserToAddRows = false;
            this.BrickGridView.AllowUserToDeleteRows = false;
            this.BrickGridView.AllowUserToResizeRows = false;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            this.BrickGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
            this.BrickGridView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.BrickGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.BrickGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PartIdColumn,
            this.DescriptionColumn,
            this.PlatformColumn,
            this.CategoryColumn,
            this.DecoratedColumn,
            this.FlexibleColumn});
            this.tableLayoutPanel1.SetColumnSpan(this.BrickGridView, 3);
            this.BrickGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BrickGridView.Location = new System.Drawing.Point(3, 31);
            this.BrickGridView.Name = "BrickGridView";
            this.BrickGridView.ReadOnly = true;
            this.BrickGridView.RowHeadersVisible = false;
            this.BrickGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.BrickGridView.Size = new System.Drawing.Size(622, 342);
            this.BrickGridView.TabIndex = 4;
            this.BrickGridView.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.BrickGridView_CellMouseDoubleClick);
            this.BrickGridView.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.BrickGridView_ColumnHeaderMouseClick);
            this.BrickGridView.SelectionChanged += new System.EventHandler(this.BrickGridView_SelectionChanged);
            // 
            // SearchTextBox
            // 
            this.SearchTextBox.Location = new System.Drawing.Point(3, 3);
            this.SearchTextBox.Name = "SearchTextBox";
            this.SearchTextBox.Size = new System.Drawing.Size(200, 22);
            this.SearchTextBox.TabIndex = 5;
            this.SearchTextBox.TextChanged += new System.EventHandler(this.SearchTextBox_TextChanged);
            // 
            // RefreshTimer
            // 
            this.RefreshTimer.Tick += new System.EventHandler(this.RefreshTimer_Tick);
            // 
            // GridColumnsContextMenu
            // 
            this.GridColumnsContextMenu.Name = "GridColumnsContextMenu";
            this.GridColumnsContextMenu.Size = new System.Drawing.Size(61, 4);
            // 
            // PartIdColumn
            // 
            this.PartIdColumn.DataPropertyName = "PartId";
            this.PartIdColumn.HeaderText = "Part ID";
            this.PartIdColumn.Name = "PartIdColumn";
            this.PartIdColumn.ReadOnly = true;
            this.PartIdColumn.Width = 80;
            // 
            // DescriptionColumn
            // 
            this.DescriptionColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.DescriptionColumn.DataPropertyName = "Description";
            this.DescriptionColumn.HeaderText = "Description";
            this.DescriptionColumn.MinimumWidth = 100;
            this.DescriptionColumn.Name = "DescriptionColumn";
            this.DescriptionColumn.ReadOnly = true;
            // 
            // PlatformColumn
            // 
            this.PlatformColumn.DataPropertyName = "Platform";
            this.PlatformColumn.HeaderText = "Platform";
            this.PlatformColumn.Name = "PlatformColumn";
            this.PlatformColumn.ReadOnly = true;
            this.PlatformColumn.Width = 80;
            // 
            // CategoryColumn
            // 
            this.CategoryColumn.DataPropertyName = "Category";
            this.CategoryColumn.HeaderText = "Category";
            this.CategoryColumn.Name = "CategoryColumn";
            this.CategoryColumn.ReadOnly = true;
            // 
            // DecoratedColumn
            // 
            this.DecoratedColumn.DataPropertyName = "Decorated";
            this.DecoratedColumn.HeaderText = "Decorated";
            this.DecoratedColumn.Name = "DecoratedColumn";
            this.DecoratedColumn.ReadOnly = true;
            this.DecoratedColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.DecoratedColumn.Width = 80;
            // 
            // FlexibleColumn
            // 
            this.FlexibleColumn.DataPropertyName = "Flexible";
            this.FlexibleColumn.HeaderText = "Flexible";
            this.FlexibleColumn.MinimumWidth = 80;
            this.FlexibleColumn.Name = "FlexibleColumn";
            this.FlexibleColumn.ReadOnly = true;
            this.FlexibleColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.FlexibleColumn.Width = 80;
            // 
            // SelectBrickDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 411);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(500, 400);
            this.Name = "SelectBrickDialog";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Text = "Select LDD Brick";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SelectBrickDialog_FormClosing);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BrickGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button CancelDialogButton;
        private System.Windows.Forms.Button OpenButton;
        private System.Windows.Forms.ProgressBar LoadingProgressBar;
        private System.Windows.Forms.DataGridView BrickGridView;
        private System.Windows.Forms.Timer RefreshTimer;
        private System.Windows.Forms.TextBox SearchTextBox;
        private System.Windows.Forms.ContextMenuStrip GridColumnsContextMenu;
        private System.Windows.Forms.DataGridViewTextBoxColumn PartIdColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn DescriptionColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn PlatformColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn CategoryColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn DecoratedColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn FlexibleColumn;
    }
}