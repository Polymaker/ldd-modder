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
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.chkExtendedTooltip = new System.Windows.Forms.CheckBox();
            this.lblDeveloperMode = new System.Windows.Forms.Label();
            this.chkDeveloperMode = new System.Windows.Forms.CheckBox();
            this.lblExtendedTooltip = new System.Windows.Forms.Label();
            this.lblUserModelDir = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.lblShowTooltip = new System.Windows.Forms.Label();
            this.chkShowTooltip = new System.Windows.Forms.CheckBox();
            this.lblDoServerCall = new System.Windows.Forms.Label();
            this.chkDoServerCall = new System.Windows.Forms.CheckBox();
            this.lblVerbose = new System.Windows.Forms.Label();
            this.chkVerbose = new System.Windows.Forms.CheckBox();
            this.toolTipController = new System.Windows.Forms.ToolTip(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.btnTxtUserModelDir = new LDDModder.ButtonEdit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.lblVerbose, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.lblDoServerCall, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.chkShowTooltip, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.chkExtendedTooltip, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblDeveloperMode, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.chkDeveloperMode, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblExtendedTooltip, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblUserModelDir, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnTxtUserModelDir, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnSave, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.lblShowTooltip, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.chkDoServerCall, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.chkVerbose, 2, 5);
            this.tableLayoutPanel1.Controls.Add(this.pictureBox1, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.pictureBox2, 1, 5);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 8;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(401, 257);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // chkExtendedTooltip
            // 
            this.chkExtendedTooltip.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.chkExtendedTooltip, 2);
            this.chkExtendedTooltip.Location = new System.Drawing.Point(123, 43);
            this.chkExtendedTooltip.Name = "chkExtendedTooltip";
            this.chkExtendedTooltip.Padding = new System.Windows.Forms.Padding(4, 0, 30, 0);
            this.chkExtendedTooltip.Size = new System.Drawing.Size(49, 14);
            this.chkExtendedTooltip.TabIndex = 3;
            this.chkExtendedTooltip.UseVisualStyleBackColor = true;
            // 
            // lblDeveloperMode
            // 
            this.lblDeveloperMode.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblDeveloperMode.AutoSize = true;
            this.lblDeveloperMode.Location = new System.Drawing.Point(32, 3);
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
            this.tableLayoutPanel1.SetColumnSpan(this.chkDeveloperMode, 2);
            this.chkDeveloperMode.Location = new System.Drawing.Point(123, 3);
            this.chkDeveloperMode.Name = "chkDeveloperMode";
            this.chkDeveloperMode.Padding = new System.Windows.Forms.Padding(4, 0, 30, 0);
            this.chkDeveloperMode.Size = new System.Drawing.Size(49, 14);
            this.chkDeveloperMode.TabIndex = 1;
            this.chkDeveloperMode.UseVisualStyleBackColor = true;
            // 
            // lblExtendedTooltip
            // 
            this.lblExtendedTooltip.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblExtendedTooltip.AutoSize = true;
            this.lblExtendedTooltip.Location = new System.Drawing.Point(14, 43);
            this.lblExtendedTooltip.Name = "lblExtendedTooltip";
            this.lblExtendedTooltip.Size = new System.Drawing.Size(103, 13);
            this.lblExtendedTooltip.TabIndex = 2;
            this.lblExtendedTooltip.Text = "Extended tooltip info";
            this.lblExtendedTooltip.Click += new System.EventHandler(this.ExtendedTooltipLabel_ClickRedirect);
            // 
            // lblUserModelDir
            // 
            this.lblUserModelDir.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblUserModelDir.AutoSize = true;
            this.lblUserModelDir.Location = new System.Drawing.Point(12, 67);
            this.lblUserModelDir.Name = "lblUserModelDir";
            this.lblUserModelDir.Size = new System.Drawing.Size(105, 13);
            this.lblUserModelDir.TabIndex = 4;
            this.lblUserModelDir.Text = "User model Directory";
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.tableLayoutPanel1.SetColumnSpan(this.btnSave, 3);
            this.btnSave.Location = new System.Drawing.Point(163, 136);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblShowTooltip
            // 
            this.lblShowTooltip.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblShowTooltip.AutoSize = true;
            this.lblShowTooltip.Location = new System.Drawing.Point(6, 23);
            this.lblShowTooltip.Name = "lblShowTooltip";
            this.lblShowTooltip.Size = new System.Drawing.Size(111, 13);
            this.lblShowTooltip.TabIndex = 7;
            this.lblShowTooltip.Text = "Show tooltip on bricks";
            this.lblShowTooltip.Click += new System.EventHandler(this.ShowTooltipLabel_ClickRedirect);
            // 
            // chkShowTooltip
            // 
            this.chkShowTooltip.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.chkShowTooltip, 2);
            this.chkShowTooltip.Location = new System.Drawing.Point(123, 23);
            this.chkShowTooltip.Name = "chkShowTooltip";
            this.chkShowTooltip.Padding = new System.Windows.Forms.Padding(4, 0, 30, 0);
            this.chkShowTooltip.Size = new System.Drawing.Size(49, 14);
            this.chkShowTooltip.TabIndex = 8;
            this.chkShowTooltip.UseVisualStyleBackColor = true;
            this.chkShowTooltip.CheckedChanged += new System.EventHandler(this.chkShowTooltip_CheckedChanged);
            // 
            // lblDoServerCall
            // 
            this.lblDoServerCall.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblDoServerCall.AutoSize = true;
            this.lblDoServerCall.Location = new System.Drawing.Point(9, 92);
            this.lblDoServerCall.Name = "lblDoServerCall";
            this.lblDoServerCall.Size = new System.Drawing.Size(108, 13);
            this.lblDoServerCall.TabIndex = 9;
            this.lblDoServerCall.Text = "Enable LDD modding";
            // 
            // chkDoServerCall
            // 
            this.chkDoServerCall.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkDoServerCall.AutoSize = true;
            this.chkDoServerCall.Location = new System.Drawing.Point(140, 90);
            this.chkDoServerCall.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.chkDoServerCall.Name = "chkDoServerCall";
            this.chkDoServerCall.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.chkDoServerCall.Size = new System.Drawing.Size(130, 17);
            this.chkDoServerCall.TabIndex = 10;
            this.chkDoServerCall.Text = "(Disables server calls)";
            this.toolTipController.SetToolTip(this.chkDoServerCall, "May prevent LDD from detecting updates.\r\nUntested (due to the rarity of updates.." +
        ".)");
            this.chkDoServerCall.UseVisualStyleBackColor = true;
            // 
            // lblVerbose
            // 
            this.lblVerbose.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblVerbose.AutoSize = true;
            this.lblVerbose.Location = new System.Drawing.Point(3, 115);
            this.lblVerbose.Name = "lblVerbose";
            this.lblVerbose.Size = new System.Drawing.Size(114, 13);
            this.lblVerbose.TabIndex = 11;
            this.lblVerbose.Text = "Enable verbose output";
            // 
            // chkVerbose
            // 
            this.chkVerbose.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkVerbose.AutoSize = true;
            this.chkVerbose.Location = new System.Drawing.Point(140, 114);
            this.chkVerbose.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.chkVerbose.Name = "chkVerbose";
            this.chkVerbose.Padding = new System.Windows.Forms.Padding(2, 0, 30, 0);
            this.chkVerbose.Size = new System.Drawing.Size(47, 14);
            this.chkVerbose.TabIndex = 12;
            this.chkVerbose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.chkVerbose.UseVisualStyleBackColor = true;
            // 
            // toolTipController
            // 
            this.toolTipController.BackColor = System.Drawing.SystemColors.Window;
            this.toolTipController.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(120, 90);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(20, 20);
            this.pictureBox1.TabIndex = 13;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(120, 113);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(20, 20);
            this.pictureBox2.TabIndex = 14;
            this.pictureBox2.TabStop = false;
            // 
            // btnTxtUserModelDir
            // 
            this.btnTxtUserModelDir.ButtonText = "…";
            this.tableLayoutPanel1.SetColumnSpan(this.btnTxtUserModelDir, 2);
            this.btnTxtUserModelDir.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnTxtUserModelDir.Location = new System.Drawing.Point(123, 63);
            this.btnTxtUserModelDir.Name = "btnTxtUserModelDir";
            this.btnTxtUserModelDir.ReadOnly = true;
            this.btnTxtUserModelDir.Size = new System.Drawing.Size(275, 21);
            this.btnTxtUserModelDir.TabIndex = 5;
            this.btnTxtUserModelDir.UseReadOnlyAppearance = false;
            this.btnTxtUserModelDir.ButtonClicked += new System.EventHandler(this.btnTxtUserModelDir_ButtonClicked);
            // 
            // FrmLDDAdvancedSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(401, 260);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FrmLDDAdvancedSettings";
            this.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrmLDDAdvancedSettings";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblDeveloperMode;
        private System.Windows.Forms.CheckBox chkDeveloperMode;
        private System.Windows.Forms.CheckBox chkExtendedTooltip;
        private System.Windows.Forms.Label lblExtendedTooltip;
        private System.Windows.Forms.Label lblUserModelDir;
        private ButtonEdit btnTxtUserModelDir;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblShowTooltip;
        private System.Windows.Forms.CheckBox chkShowTooltip;
        private System.Windows.Forms.Label lblDoServerCall;
        private System.Windows.Forms.CheckBox chkDoServerCall;
        private System.Windows.Forms.CheckBox chkVerbose;
        private System.Windows.Forms.Label lblVerbose;
        private System.Windows.Forms.ToolTip toolTipController;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
    }
}