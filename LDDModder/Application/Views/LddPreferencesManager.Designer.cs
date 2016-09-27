namespace LDDModder.Views
{
    partial class LddPreferencesManager
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
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                if (UacShieldBmp != null)
                {
                    UacShieldBmp.Dispose();
                    UacShieldBmp = null;
                }
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LddPreferencesManager));
            this.tlpLayout = new System.Windows.Forms.TableLayoutPanel();
            this.chkShowTooltip = new System.Windows.Forms.CheckBox();
            this.chkExtendedTooltip = new System.Windows.Forms.CheckBox();
            this.lblDeveloperMode = new System.Windows.Forms.Label();
            this.chkDeveloperMode = new System.Windows.Forms.CheckBox();
            this.lblExtendedTooltip = new System.Windows.Forms.Label();
            this.lblUserModelDir = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.lblShowTooltip = new System.Windows.Forms.Label();
            this.chkDoServerCall = new System.Windows.Forms.CheckBox();
            this.chkVerbose = new System.Windows.Forms.CheckBox();
            this.lblVerbose = new LDDModder.Display.Controls.LabelEx();
            this.lblDoServerCall = new LDDModder.Display.Controls.LabelEx();
            this.btnTxtUserModelDir = new LDDModder.Display.Controls.ButtonEdit();
            this.tlpLayout.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpLayout
            // 
            resources.ApplyResources(this.tlpLayout, "tlpLayout");
            this.tlpLayout.Controls.Add(this.lblVerbose, 0, 6);
            this.tlpLayout.Controls.Add(this.lblDoServerCall, 0, 5);
            this.tlpLayout.Controls.Add(this.chkShowTooltip, 1, 1);
            this.tlpLayout.Controls.Add(this.chkExtendedTooltip, 1, 2);
            this.tlpLayout.Controls.Add(this.lblDeveloperMode, 0, 0);
            this.tlpLayout.Controls.Add(this.chkDeveloperMode, 1, 0);
            this.tlpLayout.Controls.Add(this.lblExtendedTooltip, 0, 2);
            this.tlpLayout.Controls.Add(this.lblUserModelDir, 0, 3);
            this.tlpLayout.Controls.Add(this.btnTxtUserModelDir, 1, 3);
            this.tlpLayout.Controls.Add(this.btnSave, 0, 7);
            this.tlpLayout.Controls.Add(this.lblShowTooltip, 0, 1);
            this.tlpLayout.Controls.Add(this.chkDoServerCall, 1, 5);
            this.tlpLayout.Controls.Add(this.chkVerbose, 1, 6);
            this.tlpLayout.Name = "tlpLayout";
            // 
            // chkShowTooltip
            // 
            resources.ApplyResources(this.chkShowTooltip, "chkShowTooltip");
            this.chkShowTooltip.Name = "chkShowTooltip";
            this.chkShowTooltip.UseVisualStyleBackColor = true;
            this.chkShowTooltip.CheckedChanged += new System.EventHandler(this.chkShowTooltip_CheckedChanged);
            // 
            // chkExtendedTooltip
            // 
            resources.ApplyResources(this.chkExtendedTooltip, "chkExtendedTooltip");
            this.chkExtendedTooltip.Name = "chkExtendedTooltip";
            this.chkExtendedTooltip.UseVisualStyleBackColor = true;
            // 
            // lblDeveloperMode
            // 
            resources.ApplyResources(this.lblDeveloperMode, "lblDeveloperMode");
            this.lblDeveloperMode.Name = "lblDeveloperMode";
            this.lblDeveloperMode.Click += new System.EventHandler(this.CheckboxLabel_Click);
            this.lblDeveloperMode.DoubleClick += new System.EventHandler(this.CheckboxLabel_Click);
            // 
            // chkDeveloperMode
            // 
            resources.ApplyResources(this.chkDeveloperMode, "chkDeveloperMode");
            this.chkDeveloperMode.Name = "chkDeveloperMode";
            this.chkDeveloperMode.UseVisualStyleBackColor = true;
            // 
            // lblExtendedTooltip
            // 
            resources.ApplyResources(this.lblExtendedTooltip, "lblExtendedTooltip");
            this.lblExtendedTooltip.Name = "lblExtendedTooltip";
            this.lblExtendedTooltip.Click += new System.EventHandler(this.CheckboxLabel_Click);
            this.lblExtendedTooltip.DoubleClick += new System.EventHandler(this.CheckboxLabel_Click);
            // 
            // lblUserModelDir
            // 
            resources.ApplyResources(this.lblUserModelDir, "lblUserModelDir");
            this.lblUserModelDir.Name = "lblUserModelDir";
            // 
            // btnSave
            // 
            resources.ApplyResources(this.btnSave, "btnSave");
            this.tlpLayout.SetColumnSpan(this.btnSave, 2);
            this.btnSave.Name = "btnSave";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblShowTooltip
            // 
            resources.ApplyResources(this.lblShowTooltip, "lblShowTooltip");
            this.lblShowTooltip.Name = "lblShowTooltip";
            this.lblShowTooltip.Click += new System.EventHandler(this.CheckboxLabel_Click);
            this.lblShowTooltip.DoubleClick += new System.EventHandler(this.CheckboxLabel_Click);
            // 
            // chkDoServerCall
            // 
            resources.ApplyResources(this.chkDoServerCall, "chkDoServerCall");
            this.chkDoServerCall.Name = "chkDoServerCall";
            this.chkDoServerCall.UseVisualStyleBackColor = true;
            // 
            // chkVerbose
            // 
            resources.ApplyResources(this.chkVerbose, "chkVerbose");
            this.chkVerbose.Name = "chkVerbose";
            this.chkVerbose.UseVisualStyleBackColor = true;
            // 
            // lblVerbose
            // 
            resources.ApplyResources(this.lblVerbose, "lblVerbose");
            this.lblVerbose.Name = "lblVerbose";
            this.lblVerbose.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.lblVerbose.Click += new System.EventHandler(this.CheckboxLabel_Click);
            this.lblVerbose.DoubleClick += new System.EventHandler(this.CheckboxLabel_Click);
            // 
            // lblDoServerCall
            // 
            resources.ApplyResources(this.lblDoServerCall, "lblDoServerCall");
            this.lblDoServerCall.Name = "lblDoServerCall";
            this.lblDoServerCall.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.lblDoServerCall.Click += new System.EventHandler(this.CheckboxLabel_Click);
            this.lblDoServerCall.DoubleClick += new System.EventHandler(this.CheckboxLabel_Click);
            // 
            // btnTxtUserModelDir
            // 
            resources.ApplyResources(this.btnTxtUserModelDir, "btnTxtUserModelDir");
            this.btnTxtUserModelDir.Name = "btnTxtUserModelDir";
            this.btnTxtUserModelDir.ReadOnly = true;
            this.btnTxtUserModelDir.UseReadOnlyAppearance = false;
            this.btnTxtUserModelDir.ButtonClicked += new System.EventHandler(this.btnTxtUserModelDir_ButtonClicked);
            // 
            // LDDSettingsView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpLayout);
            this.Name = "LDDSettingsView";
            this.tlpLayout.ResumeLayout(false);
            this.tlpLayout.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpLayout;
        private LDDModder.Display.Controls.LabelEx lblVerbose;
        private LDDModder.Display.Controls.LabelEx lblDoServerCall;
        private System.Windows.Forms.CheckBox chkShowTooltip;
        private System.Windows.Forms.CheckBox chkExtendedTooltip;
        private System.Windows.Forms.Label lblDeveloperMode;
        private System.Windows.Forms.CheckBox chkDeveloperMode;
        private System.Windows.Forms.Label lblExtendedTooltip;
        private System.Windows.Forms.Label lblUserModelDir;
        private LDDModder.Display.Controls.ButtonEdit btnTxtUserModelDir;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblShowTooltip;
        private System.Windows.Forms.CheckBox chkDoServerCall;
        private System.Windows.Forms.CheckBox chkVerbose;
    }
}
