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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BrickEditorWindow));
            this.DockPanelControl = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.vS2015LightTheme1 = new WeifenLuo.WinFormsUI.Docking.VS2015LightTheme();
            this.vS2015DarkTheme1 = new WeifenLuo.WinFormsUI.Docking.VS2015DarkTheme();
            this.visualStudioToolStripExtender1 = new WeifenLuo.WinFormsUI.Docking.VisualStudioToolStripExtender(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.File_NewProjectMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.File_CreateFromBrickMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.File_OpenProjectMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.File_OpenRecentMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.File_SaveMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.File_SaveAsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.File_CloseProjectMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EditMenu_Undo = new System.Windows.Forms.ToolStripMenuItem();
            this.EditMenu_Redo = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.Edit_ImportMeshMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.Edit_ValidatePartMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.Edit_GenerateFilesMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lDDEnvironmentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExportBrickMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.StartLddMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // DockPanelControl
            // 
            resources.ApplyResources(this.DockPanelControl, "DockPanelControl");
            this.DockPanelControl.DockBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(242)))));
            this.DockPanelControl.DocumentStyle = WeifenLuo.WinFormsUI.Docking.DocumentStyle.DockingSdi;
            this.DockPanelControl.Name = "DockPanelControl";
            this.DockPanelControl.ShowAutoHideContentOnHover = false;
            this.DockPanelControl.Theme = this.vS2015LightTheme1;
            // 
            // visualStudioToolStripExtender1
            // 
            this.visualStudioToolStripExtender1.DefaultRenderer = null;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.ToolsMenuItem});
            resources.ApplyResources(this.menuStrip1, "menuStrip1");
            this.menuStrip1.Name = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.File_NewProjectMenu,
            this.File_CreateFromBrickMenu,
            this.toolStripSeparator2,
            this.File_OpenProjectMenu,
            this.File_OpenRecentMenu,
            this.toolStripSeparator1,
            this.File_SaveMenu,
            this.File_SaveAsMenu,
            this.toolStripSeparator5,
            this.File_CloseProjectMenu});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            resources.ApplyResources(this.fileToolStripMenuItem, "fileToolStripMenuItem");
            // 
            // File_NewProjectMenu
            // 
            this.File_NewProjectMenu.Name = "File_NewProjectMenu";
            resources.ApplyResources(this.File_NewProjectMenu, "File_NewProjectMenu");
            this.File_NewProjectMenu.Click += new System.EventHandler(this.File_NewProjectMenu_Click);
            // 
            // File_CreateFromBrickMenu
            // 
            this.File_CreateFromBrickMenu.Name = "File_CreateFromBrickMenu";
            resources.ApplyResources(this.File_CreateFromBrickMenu, "File_CreateFromBrickMenu");
            this.File_CreateFromBrickMenu.Click += new System.EventHandler(this.File_CreateFromBrickMenu_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // File_OpenProjectMenu
            // 
            this.File_OpenProjectMenu.Name = "File_OpenProjectMenu";
            resources.ApplyResources(this.File_OpenProjectMenu, "File_OpenProjectMenu");
            this.File_OpenProjectMenu.Click += new System.EventHandler(this.File_OpenProjectMenu_Click);
            // 
            // File_OpenRecentMenu
            // 
            resources.ApplyResources(this.File_OpenRecentMenu, "File_OpenRecentMenu");
            this.File_OpenRecentMenu.Name = "File_OpenRecentMenu";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // File_SaveMenu
            // 
            resources.ApplyResources(this.File_SaveMenu, "File_SaveMenu");
            this.File_SaveMenu.Name = "File_SaveMenu";
            this.File_SaveMenu.Click += new System.EventHandler(this.File_SaveMenu_Click);
            // 
            // File_SaveAsMenu
            // 
            resources.ApplyResources(this.File_SaveAsMenu, "File_SaveAsMenu");
            this.File_SaveAsMenu.Name = "File_SaveAsMenu";
            this.File_SaveAsMenu.Click += new System.EventHandler(this.File_SaveAsMenu_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            resources.ApplyResources(this.toolStripSeparator5, "toolStripSeparator5");
            // 
            // File_CloseProjectMenu
            // 
            this.File_CloseProjectMenu.Name = "File_CloseProjectMenu";
            resources.ApplyResources(this.File_CloseProjectMenu, "File_CloseProjectMenu");
            this.File_CloseProjectMenu.Click += new System.EventHandler(this.File_CloseProjectMenu_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.EditMenu_Undo,
            this.EditMenu_Redo,
            this.toolStripSeparator6,
            this.Edit_ImportMeshMenu,
            this.toolStripSeparator4,
            this.Edit_ValidatePartMenu,
            this.Edit_GenerateFilesMenu});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            resources.ApplyResources(this.editToolStripMenuItem, "editToolStripMenuItem");
            // 
            // EditMenu_Undo
            // 
            this.EditMenu_Undo.Name = "EditMenu_Undo";
            resources.ApplyResources(this.EditMenu_Undo, "EditMenu_Undo");
            this.EditMenu_Undo.Click += new System.EventHandler(this.EditMenu_Undo_Click);
            // 
            // EditMenu_Redo
            // 
            this.EditMenu_Redo.Name = "EditMenu_Redo";
            resources.ApplyResources(this.EditMenu_Redo, "EditMenu_Redo");
            this.EditMenu_Redo.Click += new System.EventHandler(this.EditMenu_Redo_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            resources.ApplyResources(this.toolStripSeparator6, "toolStripSeparator6");
            // 
            // Edit_ImportMeshMenu
            // 
            this.Edit_ImportMeshMenu.Name = "Edit_ImportMeshMenu";
            resources.ApplyResources(this.Edit_ImportMeshMenu, "Edit_ImportMeshMenu");
            this.Edit_ImportMeshMenu.Click += new System.EventHandler(this.Edit_ImportMeshMenu_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
            // 
            // Edit_ValidatePartMenu
            // 
            this.Edit_ValidatePartMenu.Name = "Edit_ValidatePartMenu";
            resources.ApplyResources(this.Edit_ValidatePartMenu, "Edit_ValidatePartMenu");
            // 
            // Edit_GenerateFilesMenu
            // 
            this.Edit_GenerateFilesMenu.Name = "Edit_GenerateFilesMenu";
            resources.ApplyResources(this.Edit_GenerateFilesMenu, "Edit_GenerateFilesMenu");
            this.Edit_GenerateFilesMenu.Click += new System.EventHandler(this.Edit_GenerateFilesMenu_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lDDEnvironmentToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            resources.ApplyResources(this.settingsToolStripMenuItem, "settingsToolStripMenuItem");
            // 
            // lDDEnvironmentToolStripMenuItem
            // 
            this.lDDEnvironmentToolStripMenuItem.Name = "lDDEnvironmentToolStripMenuItem";
            resources.ApplyResources(this.lDDEnvironmentToolStripMenuItem, "lDDEnvironmentToolStripMenuItem");
            this.lDDEnvironmentToolStripMenuItem.Click += new System.EventHandler(this.LDDEnvironmentToolStripMenuItem_Click);
            // 
            // ToolsMenuItem
            // 
            this.ToolsMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ExportBrickMenuItem,
            this.toolStripSeparator3,
            this.StartLddMenuItem});
            this.ToolsMenuItem.Name = "ToolsMenuItem";
            resources.ApplyResources(this.ToolsMenuItem, "ToolsMenuItem");
            // 
            // ExportBrickMenuItem
            // 
            this.ExportBrickMenuItem.Name = "ExportBrickMenuItem";
            resources.ApplyResources(this.ExportBrickMenuItem, "ExportBrickMenuItem");
            this.ExportBrickMenuItem.Click += new System.EventHandler(this.ExportBrickMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // StartLddMenuItem
            // 
            this.StartLddMenuItem.Name = "StartLddMenuItem";
            resources.ApplyResources(this.StartLddMenuItem, "StartLddMenuItem");
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1});
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Name = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            resources.ApplyResources(this.toolStripLabel1, "toolStripLabel1");
            // 
            // BrickEditorWindow
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.DockPanelControl);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "BrickEditorWindow";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BrickEditorWindow_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
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
        private System.Windows.Forms.ToolStripMenuItem File_NewProjectMenu;
        private System.Windows.Forms.ToolStripMenuItem File_OpenProjectMenu;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lDDEnvironmentToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem File_CreateFromBrickMenu;
        private System.Windows.Forms.ToolStripMenuItem ToolsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ExportBrickMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem StartLddMenuItem;
        private System.Windows.Forms.ToolStripMenuItem File_OpenRecentMenu;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem File_SaveMenu;
        private System.Windows.Forms.ToolStripMenuItem File_SaveAsMenu;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem Edit_ImportMeshMenu;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem Edit_ValidatePartMenu;
        private System.Windows.Forms.ToolStripMenuItem Edit_GenerateFilesMenu;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem File_CloseProjectMenu;
        private System.Windows.Forms.ToolStripMenuItem EditMenu_Undo;
        private System.Windows.Forms.ToolStripMenuItem EditMenu_Redo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
    }
}