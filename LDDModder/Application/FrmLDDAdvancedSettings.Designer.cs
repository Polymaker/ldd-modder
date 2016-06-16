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
            this.lblDeveloperMode = new System.Windows.Forms.Label();
            this.lblExtendedTooltip = new System.Windows.Forms.Label();
            this.lblUserModelDir = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.lblShowTooltip = new System.Windows.Forms.Label();
            this.toolTipController = new System.Windows.Forms.ToolTip(this.components);
            this.lblVerbose = new LDDModder.LabelEx();
            this.lblDoServerCall = new LDDModder.LabelEx();
            this.chkVerbose = new System.Windows.Forms.CheckBox();
            this.chkDoServerCall = new System.Windows.Forms.CheckBox();
            this.btnTxtUserModelDir = new LDDModder.ButtonEdit();
            this.chkDeveloperMode = new System.Windows.Forms.CheckBox();
            this.chkExtendedTooltip = new System.Windows.Forms.CheckBox();
            this.chkShowTooltip = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.lblVerbose, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.lblDoServerCall, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.chkShowTooltip, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.chkExtendedTooltip, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblDeveloperMode, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.chkDeveloperMode, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblExtendedTooltip, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblUserModelDir, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnTxtUserModelDir, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnSave, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.lblShowTooltip, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.chkDoServerCall, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.chkVerbose, 1, 6);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
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
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(401, 158);
            this.tableLayoutPanel1.TabIndex = 0;
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
            this.lblExtendedTooltip.DoubleClick += new System.EventHandler(this.ExtendedTooltipLabel_ClickRedirect);
            // 
            // lblUserModelDir
            // 
            this.lblUserModelDir.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblUserModelDir.AutoSize = true;
            this.lblUserModelDir.Location = new System.Drawing.Point(12, 66);
            this.lblUserModelDir.Name = "lblUserModelDir";
            this.lblUserModelDir.Size = new System.Drawing.Size(105, 13);
            this.lblUserModelDir.TabIndex = 4;
            this.lblUserModelDir.Text = "User model Directory";
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.tableLayoutPanel1.SetColumnSpan(this.btnSave, 2);
            this.btnSave.Location = new System.Drawing.Point(163, 132);
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
            this.lblShowTooltip.DoubleClick += new System.EventHandler(this.ShowTooltipLabel_ClickRedirect);
            // 
            // toolTipController
            // 
            this.toolTipController.BackColor = System.Drawing.SystemColors.Window;
            this.toolTipController.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            // 
            // lblVerbose
            // 
            this.lblVerbose.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblVerbose.AutoSize = true;
            this.lblVerbose.Location = new System.Drawing.Point(3, 112);
            this.lblVerbose.Name = "lblVerbose";
            this.lblVerbose.Size = new System.Drawing.Size(114, 13);
            this.lblVerbose.TabIndex = 11;
            this.lblVerbose.Text = "Enable verbose output";
            this.lblVerbose.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            // 
            // lblDoServerCall
            // 
            this.lblDoServerCall.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblDoServerCall.AutoSize = true;
            this.lblDoServerCall.Location = new System.Drawing.Point(9, 91);
            this.lblDoServerCall.Name = "lblDoServerCall";
            this.lblDoServerCall.Size = new System.Drawing.Size(108, 13);
            this.lblDoServerCall.TabIndex = 9;
            this.lblDoServerCall.Text = "Enable LDD modding";
            this.lblDoServerCall.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            // 
            // chkVerbose
            // 
            this.chkVerbose.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkVerbose.AutoSize = true;
            this.chkVerbose.Location = new System.Drawing.Point(123, 112);
            this.chkVerbose.Name = "chkVerbose";
            this.chkVerbose.Padding = new System.Windows.Forms.Padding(4, 0, 30, 0);
            this.chkVerbose.Size = new System.Drawing.Size(49, 14);
            this.chkVerbose.TabIndex = 12;
            this.chkVerbose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.chkVerbose.UseVisualStyleBackColor = true;
            // 
            // chkDoServerCall
            // 
            this.chkDoServerCall.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkDoServerCall.AutoSize = true;
            this.chkDoServerCall.Location = new System.Drawing.Point(123, 89);
            this.chkDoServerCall.Name = "chkDoServerCall";
            this.chkDoServerCall.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.chkDoServerCall.Size = new System.Drawing.Size(132, 17);
            this.chkDoServerCall.TabIndex = 10;
            this.chkDoServerCall.Text = "(Disables server calls)";
            this.toolTipController.SetToolTip(this.chkDoServerCall, "May prevent LDD from detecting updates.\r\nUntested (due to the rarity of updates.." +
        ".)");
            this.chkDoServerCall.UseVisualStyleBackColor = true;
            // 
            // btnTxtUserModelDir
            // 
            this.btnTxtUserModelDir.ButtonText = "…";
            this.btnTxtUserModelDir.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnTxtUserModelDir.Location = new System.Drawing.Point(123, 63);
            this.btnTxtUserModelDir.Name = "btnTxtUserModelDir";
            this.btnTxtUserModelDir.ReadOnly = true;
            this.btnTxtUserModelDir.Size = new System.Drawing.Size(275, 20);
            this.btnTxtUserModelDir.TabIndex = 5;
            this.btnTxtUserModelDir.UseReadOnlyAppearance = false;
            this.btnTxtUserModelDir.ButtonClicked += new System.EventHandler(this.btnTxtUserModelDir_ButtonClicked);
            // 
            // chkDeveloperMode
            // 
            this.chkDeveloperMode.AutoSize = true;
            this.chkDeveloperMode.Location = new System.Drawing.Point(123, 3);
            this.chkDeveloperMode.Name = "chkDeveloperMode";
            this.chkDeveloperMode.Padding = new System.Windows.Forms.Padding(4, 0, 30, 0);
            this.chkDeveloperMode.Size = new System.Drawing.Size(49, 14);
            this.chkDeveloperMode.TabIndex = 1;
            this.chkDeveloperMode.UseVisualStyleBackColor = true;
            // 
            // chkExtendedTooltip
            // 
            this.chkExtendedTooltip.AutoSize = true;
            this.chkExtendedTooltip.Location = new System.Drawing.Point(123, 43);
            this.chkExtendedTooltip.Name = "chkExtendedTooltip";
            this.chkExtendedTooltip.Padding = new System.Windows.Forms.Padding(4, 0, 30, 0);
            this.chkExtendedTooltip.Size = new System.Drawing.Size(49, 14);
            this.chkExtendedTooltip.TabIndex = 3;
            this.chkExtendedTooltip.UseVisualStyleBackColor = true;
            // 
            // chkShowTooltip
            // 
            this.chkShowTooltip.AutoSize = true;
            this.chkShowTooltip.Location = new System.Drawing.Point(123, 23);
            this.chkShowTooltip.Name = "chkShowTooltip";
            this.chkShowTooltip.Padding = new System.Windows.Forms.Padding(4, 0, 30, 0);
            this.chkShowTooltip.Size = new System.Drawing.Size(49, 14);
            this.chkShowTooltip.TabIndex = 8;
            this.chkShowTooltip.UseVisualStyleBackColor = true;
            this.chkShowTooltip.CheckedChanged += new System.EventHandler(this.chkShowTooltip_CheckedChanged);
            // 
            // FrmLDDAdvancedSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(401, 179);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FrmLDDAdvancedSettings";
            this.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrmLDDAdvancedSettings";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblDeveloperMode;
        private System.Windows.Forms.Label lblExtendedTooltip;
        private System.Windows.Forms.Label lblUserModelDir;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblShowTooltip;
        private LDDModder.LabelEx lblDoServerCall;
        private LDDModder.LabelEx lblVerbose;
        private System.Windows.Forms.ToolTip toolTipController;
        private System.Windows.Forms.CheckBox chkShowTooltip;
        private System.Windows.Forms.CheckBox chkExtendedTooltip;
        private System.Windows.Forms.CheckBox chkDeveloperMode;
        private ButtonEdit btnTxtUserModelDir;
        private System.Windows.Forms.CheckBox chkDoServerCall;
        private System.Windows.Forms.CheckBox chkVerbose;
    }
}