namespace LDDModder.PaletteMaker.UI
{
    partial class MainWindow
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SetPartsGridView = new System.Windows.Forms.DataGridView();
            this.PartIDColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PartNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CategoryColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ColorColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ElementIDColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.QuantityColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PartMatchColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MatchStatusColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EditMatchColumn = new System.Windows.Forms.DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)(this.SetPartsGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(93, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // SetPartsGridView
            // 
            this.SetPartsGridView.AllowUserToAddRows = false;
            this.SetPartsGridView.AllowUserToDeleteRows = false;
            this.SetPartsGridView.AllowUserToResizeRows = false;
            this.SetPartsGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SetPartsGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.SetPartsGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PartIDColumn,
            this.PartNameColumn,
            this.CategoryColumn,
            this.ColorColumn,
            this.ElementIDColumn,
            this.QuantityColumn,
            this.PartMatchColumn,
            this.MatchStatusColumn,
            this.EditMatchColumn});
            this.SetPartsGridView.Location = new System.Drawing.Point(12, 41);
            this.SetPartsGridView.Name = "SetPartsGridView";
            this.SetPartsGridView.RowHeadersWidth = 30;
            this.SetPartsGridView.Size = new System.Drawing.Size(841, 275);
            this.SetPartsGridView.TabIndex = 2;
            this.SetPartsGridView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.SetPartsGridView_CellFormatting);
            // 
            // PartIDColumn
            // 
            this.PartIDColumn.DataPropertyName = "PartID";
            this.PartIDColumn.HeaderText = "Part ID";
            this.PartIDColumn.Name = "PartIDColumn";
            this.PartIDColumn.ReadOnly = true;
            this.PartIDColumn.Width = 90;
            // 
            // PartNameColumn
            // 
            this.PartNameColumn.DataPropertyName = "PartName";
            this.PartNameColumn.HeaderText = "Part Name";
            this.PartNameColumn.Name = "PartNameColumn";
            this.PartNameColumn.ReadOnly = true;
            this.PartNameColumn.Width = 140;
            // 
            // CategoryColumn
            // 
            this.CategoryColumn.DataPropertyName = "CategoryID";
            this.CategoryColumn.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
            this.CategoryColumn.HeaderText = "Category";
            this.CategoryColumn.Name = "CategoryColumn";
            this.CategoryColumn.ReadOnly = true;
            this.CategoryColumn.Width = 90;
            // 
            // ColorColumn
            // 
            this.ColorColumn.DataPropertyName = "ColorID";
            this.ColorColumn.HeaderText = "Color";
            this.ColorColumn.Name = "ColorColumn";
            this.ColorColumn.ReadOnly = true;
            this.ColorColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColorColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ElementIDColumn
            // 
            this.ElementIDColumn.DataPropertyName = "ElementID";
            this.ElementIDColumn.HeaderText = "Element ID";
            this.ElementIDColumn.Name = "ElementIDColumn";
            this.ElementIDColumn.ReadOnly = true;
            this.ElementIDColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ElementIDColumn.Width = 80;
            // 
            // QuantityColumn
            // 
            this.QuantityColumn.DataPropertyName = "Quantity";
            this.QuantityColumn.HeaderText = "Qty.";
            this.QuantityColumn.Name = "QuantityColumn";
            this.QuantityColumn.ReadOnly = true;
            this.QuantityColumn.Width = 50;
            // 
            // PartMatchColumn
            // 
            this.PartMatchColumn.DataPropertyName = "LddPartID";
            this.PartMatchColumn.HeaderText = "LDD Part";
            this.PartMatchColumn.Name = "PartMatchColumn";
            this.PartMatchColumn.ReadOnly = true;
            this.PartMatchColumn.Width = 80;
            // 
            // MatchStatusColumn
            // 
            this.MatchStatusColumn.HeaderText = "Status";
            this.MatchStatusColumn.Name = "MatchStatusColumn";
            this.MatchStatusColumn.ReadOnly = true;
            // 
            // EditMatchColumn
            // 
            this.EditMatchColumn.HeaderText = "";
            this.EditMatchColumn.Name = "EditMatchColumn";
            this.EditMatchColumn.Width = 40;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(865, 328);
            this.Controls.Add(this.SetPartsGridView);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "MainWindow";
            this.Text = "MainWindow";
            ((System.ComponentModel.ISupportInitialize)(this.SetPartsGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataGridView SetPartsGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn PartIDColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn PartNameColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn CategoryColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColorColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ElementIDColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn QuantityColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn PartMatchColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn MatchStatusColumn;
        private System.Windows.Forms.DataGridViewButtonColumn EditMatchColumn;
    }
}