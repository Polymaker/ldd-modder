namespace LDDModder.BrickEditor.UI.Windows
{
    partial class BrickEditorWindow
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
            this.DockPanelControl = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.vS2015LightTheme1 = new WeifenLuo.WinFormsUI.Docking.VS2015LightTheme();
            this.vS2015DarkTheme1 = new WeifenLuo.WinFormsUI.Docking.VS2015DarkTheme();
            this.visualStudioToolStripExtender1 = new WeifenLuo.WinFormsUI.Docking.VisualStudioToolStripExtender(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lDDEnvironmentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lDDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LddPreferencesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LddLocalizationsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.StartLddMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // DockPanelControl
            // 
            this.DockPanelControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DockPanelControl.DockBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(242)))));
            this.DockPanelControl.DocumentStyle = WeifenLuo.WinFormsUI.Docking.DocumentStyle.DockingSdi;
            this.DockPanelControl.Location = new System.Drawing.Point(0, 24);
            this.DockPanelControl.Name = "DockPanelControl";
            this.DockPanelControl.Padding = new System.Windows.Forms.Padding(6);
            this.DockPanelControl.ShowAutoHideContentOnHover = false;
            this.DockPanelControl.Size = new System.Drawing.Size(731, 471);
            this.DockPanelControl.TabIndex = 1;
            this.DockPanelControl.Theme = this.vS2015LightTheme1;
            this.DockPanelControl.ActiveDocumentChanged += new System.EventHandler(this.DockPanelControl_ActiveDocumentChanged);
            // 
            // visualStudioToolStripExtender1
            // 
            this.visualStudioToolStripExtender1.DefaultRenderer = null;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.lDDToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(731, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.newToolStripMenuItem.Text = "New";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.openToolStripMenuItem.Text = "Open";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lDDEnvironmentToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // lDDEnvironmentToolStripMenuItem
            // 
            this.lDDEnvironmentToolStripMenuItem.Name = "lDDEnvironmentToolStripMenuItem";
            this.lDDEnvironmentToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.lDDEnvironmentToolStripMenuItem.Text = "LDD Environment";
            this.lDDEnvironmentToolStripMenuItem.Click += new System.EventHandler(this.LDDEnvironmentToolStripMenuItem_Click);
            // 
            // lDDToolStripMenuItem
            // 
            this.lDDToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LddPreferencesMenuItem,
            this.LddLocalizationsMenuItem,
            this.toolStripSeparator1,
            this.StartLddMenuItem});
            this.lDDToolStripMenuItem.Name = "lDDToolStripMenuItem";
            this.lDDToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
            this.lDDToolStripMenuItem.Text = "LDD";
            // 
            // LddPreferencesMenuItem
            // 
            this.LddPreferencesMenuItem.Name = "LddPreferencesMenuItem";
            this.LddPreferencesMenuItem.Size = new System.Drawing.Size(180, 22);
            this.LddPreferencesMenuItem.Text = "Preferences";
            // 
            // LddLocalizationsMenuItem
            // 
            this.LddLocalizationsMenuItem.Name = "LddLocalizationsMenuItem";
            this.LddLocalizationsMenuItem.Size = new System.Drawing.Size(180, 22);
            this.LddLocalizationsMenuItem.Text = "Localizations";
            this.LddLocalizationsMenuItem.Click += new System.EventHandler(this.LddLocalizationsMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
            // 
            // StartLddMenuItem
            // 
            this.StartLddMenuItem.Name = "StartLddMenuItem";
            this.StartLddMenuItem.Size = new System.Drawing.Size(180, 22);
            this.StartLddMenuItem.Text = "Start LDD";
            // 
            // BrickEditorWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(731, 495);
            this.Controls.Add(this.DockPanelControl);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "BrickEditorWindow";
            this.Text = "BrickEditorWindow";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BrickEditorWindow_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private WeifenLuo.WinFormsUI.Docking.DockPanel DockPanelControl;
        private WeifenLuo.WinFormsUI.Docking.VS2015DarkTheme vS2015DarkTheme1;
        private WeifenLuo.WinFormsUI.Docking.VisualStudioToolStripExtender visualStudioToolStripExtender1;
        private WeifenLuo.WinFormsUI.Docking.VS2015LightTheme vS2015LightTheme1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lDDToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem LddPreferencesMenuItem;
        private System.Windows.Forms.ToolStripMenuItem LddLocalizationsMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem StartLddMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lDDEnvironmentToolStripMenuItem;
    }
}