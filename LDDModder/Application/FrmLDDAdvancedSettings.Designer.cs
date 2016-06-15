namespace LDDModder
{
    partial class FrmLDDAdvancedSettings
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
            this.chkExtendedTooltips = new System.Windows.Forms.CheckBox();
            this.lblDeveloperMode = new System.Windows.Forms.Label();
            this.chkDeveloperMode = new System.Windows.Forms.CheckBox();
            this.lblExtendedTooltips = new System.Windows.Forms.Label();
            this.lblUserModelDir = new System.Windows.Forms.Label();
            this.btnTxtUserModelDir = new LDDModder.ButtonEdit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.Controls.Add(this.chkExtendedTooltips, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblDeveloperMode, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.chkDeveloperMode, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblExtendedTooltips, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblUserModelDir, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnTxtUserModelDir, 1, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(401, 129);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // chkExtendedTooltips
            // 
            this.chkExtendedTooltips.AutoSize = true;
            this.chkExtendedTooltips.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkExtendedTooltips.Location = new System.Drawing.Point(123, 26);
            this.chkExtendedTooltips.Name = "chkExtendedTooltips";
            this.chkExtendedTooltips.Size = new System.Drawing.Size(29, 17);
            this.chkExtendedTooltips.TabIndex = 3;
            this.chkExtendedTooltips.Text = "      ";
            this.chkExtendedTooltips.UseVisualStyleBackColor = true;
            // 
            // lblDeveloperMode
            // 
            this.lblDeveloperMode.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblDeveloperMode.AutoSize = true;
            this.lblDeveloperMode.Location = new System.Drawing.Point(32, 5);
            this.lblDeveloperMode.Name = "lblDeveloperMode";
            this.lblDeveloperMode.Size = new System.Drawing.Size(85, 13);
            this.lblDeveloperMode.TabIndex = 0;
            this.lblDeveloperMode.Text = "Developer mode";
            this.lblDeveloperMode.Click += new System.EventHandler(this.DeveloperModeLabel_ClickRedirect);
            this.lblDeveloperMode.DoubleClick += new System.EventHandler(this.DeveloperModeLabel_ClickRedirect);
            // 
            // chkDeveloperMode
            // 
            this.chkDeveloperMode.AutoSize = true;
            this.chkDeveloperMode.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkDeveloperMode.Location = new System.Drawing.Point(123, 3);
            this.chkDeveloperMode.Name = "chkDeveloperMode";
            this.chkDeveloperMode.Size = new System.Drawing.Size(29, 17);
            this.chkDeveloperMode.TabIndex = 1;
            this.chkDeveloperMode.Text = "      ";
            this.chkDeveloperMode.UseVisualStyleBackColor = true;
            // 
            // lblExtendedTooltips
            // 
            this.lblExtendedTooltips.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblExtendedTooltips.AutoSize = true;
            this.lblExtendedTooltips.Location = new System.Drawing.Point(3, 28);
            this.lblExtendedTooltips.Name = "lblExtendedTooltips";
            this.lblExtendedTooltips.Size = new System.Drawing.Size(114, 13);
            this.lblExtendedTooltips.TabIndex = 2;
            this.lblExtendedTooltips.Text = "Extended brick tooltips";
            this.lblExtendedTooltips.Click += new System.EventHandler(this.ExtendedTooltipLabel_ClickRedirect);
            // 
            // lblUserModelDir
            // 
            this.lblUserModelDir.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblUserModelDir.AutoSize = true;
            this.lblUserModelDir.Location = new System.Drawing.Point(12, 53);
            this.lblUserModelDir.Name = "lblUserModelDir";
            this.lblUserModelDir.Size = new System.Drawing.Size(105, 13);
            this.lblUserModelDir.TabIndex = 4;
            this.lblUserModelDir.Text = "User model Directory";
            // 
            // btnTxtUserModelDir
            // 
            this.btnTxtUserModelDir.ButtonText = "...";
            this.btnTxtUserModelDir.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnTxtUserModelDir.Location = new System.Drawing.Point(123, 49);
            this.btnTxtUserModelDir.Name = "btnTxtUserModelDir";
            this.btnTxtUserModelDir.Size = new System.Drawing.Size(275, 21);
            this.btnTxtUserModelDir.TabIndex = 5;
            this.btnTxtUserModelDir.ButtonClicked += new System.EventHandler(this.btnTxtUserModelDir_ButtonClicked);
            // 
            // FrmLDDAdvancedSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(401, 132);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FrmLDDAdvancedSettings";
            this.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.Text = "FrmLDDAdvancedSettings";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblDeveloperMode;
        private System.Windows.Forms.CheckBox chkDeveloperMode;
        private System.Windows.Forms.CheckBox chkExtendedTooltips;
        private System.Windows.Forms.Label lblExtendedTooltips;
        private System.Windows.Forms.Label lblUserModelDir;
        private ButtonEdit btnTxtUserModelDir;
    }
}