namespace LDDModder.LifExtractor.Windows
{
    partial class ExtractItemsDialog
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
            this.SelectFolderButton = new System.Windows.Forms.Button();
            this.DestinationTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.CancelExtractButton = new System.Windows.Forms.Button();
            this.ExtractButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 63.5514F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.SelectFolderButton, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.DestinationTextBox, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.CancelExtractButton, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.ExtractButton, 1, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(352, 117);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // SelectFolderButton
            // 
            this.SelectFolderButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.SelectFolderButton.Location = new System.Drawing.Point(299, 16);
            this.SelectFolderButton.Name = "SelectFolderButton";
            this.SelectFolderButton.Size = new System.Drawing.Size(50, 23);
            this.SelectFolderButton.TabIndex = 0;
            this.SelectFolderButton.Text = "...";
            this.SelectFolderButton.UseVisualStyleBackColor = true;
            this.SelectFolderButton.Click += new System.EventHandler(this.SelectFolderButton_Click);
            // 
            // DestinationTextBox
            // 
            this.DestinationTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.DestinationTextBox, 3);
            this.DestinationTextBox.Location = new System.Drawing.Point(3, 17);
            this.DestinationTextBox.Name = "DestinationTextBox";
            this.DestinationTextBox.Size = new System.Drawing.Size(290, 20);
            this.DestinationTextBox.TabIndex = 1;
            this.DestinationTextBox.TextChanged += new System.EventHandler(this.DestinationTextBox_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Extract to…";
            // 
            // CancelExtractButton
            // 
            this.CancelExtractButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.CancelExtractButton, 2);
            this.CancelExtractButton.Location = new System.Drawing.Point(274, 91);
            this.CancelExtractButton.Name = "CancelExtractButton";
            this.CancelExtractButton.Size = new System.Drawing.Size(75, 23);
            this.CancelExtractButton.TabIndex = 3;
            this.CancelExtractButton.Text = "Cancel";
            this.CancelExtractButton.UseVisualStyleBackColor = true;
            this.CancelExtractButton.Click += new System.EventHandler(this.CancelExtractButton_Click);
            // 
            // ExtractButton
            // 
            this.ExtractButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ExtractButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ExtractButton.Location = new System.Drawing.Point(193, 91);
            this.ExtractButton.Name = "ExtractButton";
            this.ExtractButton.Size = new System.Drawing.Size(75, 23);
            this.ExtractButton.TabIndex = 4;
            this.ExtractButton.Text = "Extract";
            this.ExtractButton.UseVisualStyleBackColor = true;
            this.ExtractButton.Click += new System.EventHandler(this.ExtractButton_Click);
            // 
            // ExtractItemsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ExtractButton;
            this.ClientSize = new System.Drawing.Size(358, 123);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExtractItemsDialog";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Text = "Extract";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button SelectFolderButton;
        private System.Windows.Forms.TextBox DestinationTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button CancelExtractButton;
        private System.Windows.Forms.Button ExtractButton;
    }
}