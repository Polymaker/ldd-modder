namespace LDDModder.PaletteMaker.Views
{
    partial class PaletteDetailView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txtPaletteName = new System.Windows.Forms.TextBox();
            this.lblPaletteName = new System.Windows.Forms.Label();
            this.versionEdit1 = new LDDModder.PaletteMaker.Controls.VersionEdit();
            this.nudPaletteVersion = new System.Windows.Forms.NumericUpDown();
            this.lblVersion = new System.Windows.Forms.Label();
            this.chkAvdVersion = new System.Windows.Forms.CheckBox();
            this.lblCountable = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.lblBrand = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPaletteVersion)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel1.Controls.Add(this.versionEdit1, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblVersion, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtPaletteName, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblPaletteName, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.nudPaletteVersion, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.chkAvdVersion, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblCountable, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.checkBox1, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.comboBox1, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.lblBrand, 0, 4);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(381, 173);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // txtPaletteName
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.txtPaletteName, 2);
            this.txtPaletteName.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtPaletteName.Location = new System.Drawing.Point(126, 3);
            this.txtPaletteName.Name = "txtPaletteName";
            this.txtPaletteName.Size = new System.Drawing.Size(252, 20);
            this.txtPaletteName.TabIndex = 0;
            // 
            // lblPaletteName
            // 
            this.lblPaletteName.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblPaletteName.AutoSize = true;
            this.lblPaletteName.Location = new System.Drawing.Point(51, 6);
            this.lblPaletteName.Name = "lblPaletteName";
            this.lblPaletteName.Size = new System.Drawing.Size(69, 13);
            this.lblPaletteName.TabIndex = 1;
            this.lblPaletteName.Text = "Palette name";
            // 
            // versionEdit1
            // 
            this.versionEdit1.Location = new System.Drawing.Point(126, 55);
            this.versionEdit1.Name = "versionEdit1";
            this.versionEdit1.Size = new System.Drawing.Size(66, 20);
            this.versionEdit1.TabIndex = 1;
            this.versionEdit1.Value = new LDDModder.LDD.General.VersionInfo(1, 0);
            // 
            // nudPaletteVersion
            // 
            this.nudPaletteVersion.Location = new System.Drawing.Point(126, 29);
            this.nudPaletteVersion.Name = "nudPaletteVersion";
            this.nudPaletteVersion.Size = new System.Drawing.Size(66, 20);
            this.nudPaletteVersion.TabIndex = 2;
            this.nudPaletteVersion.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudPaletteVersion.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblVersion
            // 
            this.lblVersion.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new System.Drawing.Point(78, 32);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(42, 13);
            this.lblVersion.TabIndex = 3;
            this.lblVersion.Text = "Version";
            // 
            // chkAvdVersion
            // 
            this.chkAvdVersion.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkAvdVersion.AutoSize = true;
            this.chkAvdVersion.Location = new System.Drawing.Point(198, 30);
            this.chkAvdVersion.Name = "chkAvdVersion";
            this.chkAvdVersion.Size = new System.Drawing.Size(137, 17);
            this.chkAvdVersion.TabIndex = 4;
            this.chkAvdVersion.Text = "Show extended version";
            this.chkAvdVersion.UseVisualStyleBackColor = true;
            // 
            // lblCountable
            // 
            this.lblCountable.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblCountable.AutoSize = true;
            this.lblCountable.Location = new System.Drawing.Point(65, 83);
            this.lblCountable.Name = "lblCountable";
            this.lblCountable.Size = new System.Drawing.Size(55, 13);
            this.lblCountable.TabIndex = 5;
            this.lblCountable.Text = "Countable";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.checkBox1, 2);
            this.checkBox1.Location = new System.Drawing.Point(126, 81);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(118, 17);
            this.checkBox1.TabIndex = 6;
            this.checkBox1.Text = "(Enables quantities)";
            this.checkBox1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.comboBox1, 2);
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "LDD",
            "Mindstorm"});
            this.comboBox1.Location = new System.Drawing.Point(126, 104);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 7;
            // 
            // lblBrand
            // 
            this.lblBrand.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblBrand.AutoSize = true;
            this.lblBrand.Location = new System.Drawing.Point(85, 108);
            this.lblBrand.Name = "lblBrand";
            this.lblBrand.Size = new System.Drawing.Size(35, 13);
            this.lblBrand.TabIndex = 8;
            this.lblBrand.Text = "Brand";
            // 
            // PaletteDetailView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "PaletteDetailView";
            this.Size = new System.Drawing.Size(451, 379);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPaletteVersion)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox txtPaletteName;
        private System.Windows.Forms.Label lblPaletteName;
        private Controls.VersionEdit versionEdit1;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.NumericUpDown nudPaletteVersion;
        private System.Windows.Forms.CheckBox chkAvdVersion;
        private System.Windows.Forms.Label lblCountable;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label lblBrand;
    }
}
