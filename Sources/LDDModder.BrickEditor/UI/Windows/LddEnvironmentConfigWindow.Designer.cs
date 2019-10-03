namespace LDDModder.BrickEditor.UI.Windows
{
    partial class LddEnvironmentConfigWindow
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
            this.PrgmFilePathLabel = new System.Windows.Forms.Label();
            this.AppDataPathLabel = new System.Windows.Forms.Label();
            this.PrgmFilePathTextBox = new System.Windows.Forms.TextBox();
            this.AppDataPathTextBox = new System.Windows.Forms.TextBox();
            this.LddPathsGroupBox = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.AssetsStatusLabel = new System.Windows.Forms.Label();
            this.DBStatusLabel = new System.Windows.Forms.Label();
            this.ExtractAssetsButton = new System.Windows.Forms.Button();
            this.ExtractDBButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.CloseButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.LddPathsGroupBox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.PrgmFilePathLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.AppDataPathLabel, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.PrgmFilePathTextBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.AppDataPathTextBox, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 18);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(443, 56);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // PrgmFilePathLabel
            // 
            this.PrgmFilePathLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.PrgmFilePathLabel.AutoSize = true;
            this.PrgmFilePathLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PrgmFilePathLabel.Location = new System.Drawing.Point(36, 7);
            this.PrgmFilePathLabel.Name = "PrgmFilePathLabel";
            this.PrgmFilePathLabel.Size = new System.Drawing.Size(81, 13);
            this.PrgmFilePathLabel.TabIndex = 0;
            this.PrgmFilePathLabel.Text = "Program Files:";
            // 
            // AppDataPathLabel
            // 
            this.AppDataPathLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.AppDataPathLabel.AutoSize = true;
            this.AppDataPathLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AppDataPathLabel.Location = new System.Drawing.Point(34, 35);
            this.AppDataPathLabel.Name = "AppDataPathLabel";
            this.AppDataPathLabel.Size = new System.Drawing.Size(83, 13);
            this.AppDataPathLabel.TabIndex = 1;
            this.AppDataPathLabel.Text = "AppData Path:";
            // 
            // PrgmFilePathTextBox
            // 
            this.PrgmFilePathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.PrgmFilePathTextBox.Location = new System.Drawing.Point(123, 3);
            this.PrgmFilePathTextBox.Name = "PrgmFilePathTextBox";
            this.PrgmFilePathTextBox.Size = new System.Drawing.Size(317, 22);
            this.PrgmFilePathTextBox.TabIndex = 2;
            // 
            // AppDataPathTextBox
            // 
            this.AppDataPathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.AppDataPathTextBox.Location = new System.Drawing.Point(123, 31);
            this.AppDataPathTextBox.Name = "AppDataPathTextBox";
            this.AppDataPathTextBox.Size = new System.Drawing.Size(317, 22);
            this.AppDataPathTextBox.TabIndex = 3;
            // 
            // LddPathsGroupBox
            // 
            this.LddPathsGroupBox.AutoSize = true;
            this.LddPathsGroupBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel3.SetColumnSpan(this.LddPathsGroupBox, 2);
            this.LddPathsGroupBox.Controls.Add(this.tableLayoutPanel1);
            this.LddPathsGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.LddPathsGroupBox.Location = new System.Drawing.Point(3, 3);
            this.LddPathsGroupBox.Name = "LddPathsGroupBox";
            this.LddPathsGroupBox.Size = new System.Drawing.Size(449, 77);
            this.LddPathsGroupBox.TabIndex = 1;
            this.LddPathsGroupBox.TabStop = false;
            this.LddPathsGroupBox.Text = "LDD Paths";
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSize = true;
            this.groupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel3.SetColumnSpan(this.groupBox1, 2);
            this.groupBox1.Controls.Add(this.tableLayoutPanel2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(3, 86);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(449, 77);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "LDD Data";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.AssetsStatusLabel, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.DBStatusLabel, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.ExtractAssetsButton, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.ExtractDBButton, 2, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 18);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(443, 56);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(28, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Program assets:";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(37, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Main content:";
            // 
            // AssetsStatusLabel
            // 
            this.AssetsStatusLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.AssetsStatusLabel.AutoSize = true;
            this.AssetsStatusLabel.Location = new System.Drawing.Point(123, 7);
            this.AssetsStatusLabel.Name = "AssetsStatusLabel";
            this.AssetsStatusLabel.Size = new System.Drawing.Size(38, 13);
            this.AssetsStatusLabel.TabIndex = 2;
            this.AssetsStatusLabel.Text = "label3";
            // 
            // DBStatusLabel
            // 
            this.DBStatusLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.DBStatusLabel.AutoSize = true;
            this.DBStatusLabel.Location = new System.Drawing.Point(123, 35);
            this.DBStatusLabel.Name = "DBStatusLabel";
            this.DBStatusLabel.Size = new System.Drawing.Size(38, 13);
            this.DBStatusLabel.TabIndex = 3;
            this.DBStatusLabel.Text = "label4";
            // 
            // ExtractAssetsButton
            // 
            this.ExtractAssetsButton.Location = new System.Drawing.Point(380, 3);
            this.ExtractAssetsButton.Name = "ExtractAssetsButton";
            this.ExtractAssetsButton.Size = new System.Drawing.Size(60, 22);
            this.ExtractAssetsButton.TabIndex = 4;
            this.ExtractAssetsButton.Text = "Extract";
            this.ExtractAssetsButton.UseVisualStyleBackColor = true;
            // 
            // ExtractDBButton
            // 
            this.ExtractDBButton.Location = new System.Drawing.Point(380, 31);
            this.ExtractDBButton.Name = "ExtractDBButton";
            this.ExtractDBButton.Size = new System.Drawing.Size(60, 22);
            this.ExtractDBButton.TabIndex = 5;
            this.ExtractDBButton.Text = "Extract";
            this.ExtractDBButton.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.groupBox1, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.LddPathsGroupBox, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.CloseButton, 1, 2);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(455, 272);
            this.tableLayoutPanel3.TabIndex = 3;
            // 
            // CloseButton
            // 
            this.CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseButton.Location = new System.Drawing.Point(377, 246);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(75, 23);
            this.CloseButton.TabIndex = 3;
            this.CloseButton.Text = "Close";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // LddEnvironmentConfigWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(461, 278);
            this.Controls.Add(this.tableLayoutPanel3);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "LddEnvironmentConfigWindow";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Text = "LDD Editor Settings";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.LddPathsGroupBox.ResumeLayout(false);
            this.LddPathsGroupBox.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label PrgmFilePathLabel;
        private System.Windows.Forms.Label AppDataPathLabel;
        private System.Windows.Forms.TextBox PrgmFilePathTextBox;
        private System.Windows.Forms.TextBox AppDataPathTextBox;
        private System.Windows.Forms.GroupBox LddPathsGroupBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label AssetsStatusLabel;
        private System.Windows.Forms.Label DBStatusLabel;
        private System.Windows.Forms.Button ExtractAssetsButton;
        private System.Windows.Forms.Button ExtractDBButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button CloseButton;
    }
}