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
            this.BrickListView = new LDDModder.BrickEditor.UI.Controls.ListViewDB();
            this.PartIDColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DescriptionColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DecorationColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.FlexibleColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.CancelDialogButton = new System.Windows.Forms.Button();
            this.OpenButton = new System.Windows.Forms.Button();
            this.LoadingProgressBar = new System.Windows.Forms.ProgressBar();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // BrickListView
            // 
            this.BrickListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.PartIDColumn,
            this.DescriptionColumn,
            this.DecorationColumn,
            this.FlexibleColumn});
            this.tableLayoutPanel1.SetColumnSpan(this.BrickListView, 3);
            this.BrickListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BrickListView.FullRowSelect = true;
            this.BrickListView.HideSelection = false;
            this.BrickListView.Location = new System.Drawing.Point(3, 3);
            this.BrickListView.Name = "BrickListView";
            this.BrickListView.Size = new System.Drawing.Size(463, 305);
            this.BrickListView.TabIndex = 0;
            this.BrickListView.UseCompatibleStateImageBehavior = false;
            this.BrickListView.View = System.Windows.Forms.View.Details;
            // 
            // PartIDColumn
            // 
            this.PartIDColumn.Text = "Part ID";
            this.PartIDColumn.Width = 70;
            // 
            // DescriptionColumn
            // 
            this.DescriptionColumn.Text = "Description";
            this.DescriptionColumn.Width = 223;
            // 
            // DecorationColumn
            // 
            this.DecorationColumn.Text = "Decorated";
            this.DecorationColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.DecorationColumn.Width = 70;
            // 
            // FlexibleColumn
            // 
            this.FlexibleColumn.Text = "Flexible";
            this.FlexibleColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.FlexibleColumn.Width = 70;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.CancelDialogButton, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.OpenButton, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.BrickListView, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.LoadingProgressBar, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(469, 340);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // CancelDialogButton
            // 
            this.CancelDialogButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelDialogButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelDialogButton.Location = new System.Drawing.Point(391, 314);
            this.CancelDialogButton.Name = "CancelDialogButton";
            this.CancelDialogButton.Size = new System.Drawing.Size(75, 23);
            this.CancelDialogButton.TabIndex = 1;
            this.CancelDialogButton.Text = "Cancel";
            this.CancelDialogButton.UseVisualStyleBackColor = true;
            // 
            // OpenButton
            // 
            this.OpenButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OpenButton.Location = new System.Drawing.Point(310, 314);
            this.OpenButton.Name = "OpenButton";
            this.OpenButton.Size = new System.Drawing.Size(75, 23);
            this.OpenButton.TabIndex = 2;
            this.OpenButton.Text = "Open";
            this.OpenButton.UseVisualStyleBackColor = true;
            // 
            // LoadingProgressBar
            // 
            this.LoadingProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LoadingProgressBar.Location = new System.Drawing.Point(3, 314);
            this.LoadingProgressBar.Name = "LoadingProgressBar";
            this.LoadingProgressBar.Size = new System.Drawing.Size(301, 23);
            this.LoadingProgressBar.TabIndex = 3;
            // 
            // SelectBrickDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(475, 346);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectBrickDialog";
            this.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.Text = "Select Brick";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private LDDModder.BrickEditor.UI.Controls.ListViewDB BrickListView;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button CancelDialogButton;
        private System.Windows.Forms.Button OpenButton;
        private System.Windows.Forms.ColumnHeader PartIDColumn;
        private System.Windows.Forms.ColumnHeader DescriptionColumn;
        private System.Windows.Forms.ColumnHeader DecorationColumn;
        private System.Windows.Forms.ColumnHeader FlexibleColumn;
        private System.Windows.Forms.ProgressBar LoadingProgressBar;
    }
}