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
            this.FileMenu_OpenPartFiles = new System.Windows.Forms.ToolStripMenuItem();
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
            this.Edit_BatchBuild = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExportBrickMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Tools_ConnectionsReportMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.StartLddMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.Tools_SettingsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.WindowMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.WindowMenu_SaveLayout = new System.Windows.Forms.ToolStripMenuItem();
            this.WindowMenu_ApplyLayout = new System.Windows.Forms.ToolStripMenuItem();
            this.WindowMenu_ManageLayouts = new System.Windows.Forms.ToolStripMenuItem();
            this.WindowMenu_ResetLayout = new System.Windows.Forms.ToolStripMenuItem();
            this.HelpMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.HelpMenu_About = new System.Windows.Forms.ToolStripMenuItem();
            this.SelectedBuildLabel = new System.Windows.Forms.ToolStripLabel();
            this.BuildConfigComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.AutoSaveTimer = new System.Windows.Forms.Timer(this.components);
            this.localizableStringList1 = new LDDModder.BrickEditor.Localization.LocalizableStringList(this.components);
            this.StartLddText = new LDDModder.BrickEditor.Localization.LocalizableString();
            this.RestartLddText = new LDDModder.BrickEditor.Localization.LocalizableString();
            this.WindowTitle = new LDDModder.BrickEditor.Localization.LocalizableString();
            this.Tools_OpenPartMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // DockPanelControl
            // 
            resources.ApplyResources(this.DockPanelControl, "DockPanelControl");
            this.DockPanelControl.DockBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(242)))));
            this.DockPanelControl.DocumentStyle = WeifenLuo.WinFormsUI.Docking.DocumentStyle.DockingWindow;
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
            this.menuStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.ToolsMenuItem,
            this.WindowMenuItem,
            this.HelpMenuItem,
            this.SelectedBuildLabel,
            this.BuildConfigComboBox});
            resources.ApplyResources(this.menuStrip1, "menuStrip1");
            this.menuStrip1.Name = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.File_NewProjectMenu,
            this.File_CreateFromBrickMenu,
            this.FileMenu_OpenPartFiles,
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
            // FileMenu_OpenPartFiles
            // 
            this.FileMenu_OpenPartFiles.Name = "FileMenu_OpenPartFiles";
            resources.ApplyResources(this.FileMenu_OpenPartFiles, "FileMenu_OpenPartFiles");
            this.FileMenu_OpenPartFiles.Click += new System.EventHandler(this.FileMenu_OpenPartFiles_Click);
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
            this.File_OpenRecentMenu.DropDownOpening += new System.EventHandler(this.File_OpenRecentMenu_DropDownOpening);
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
            this.Edit_GenerateFilesMenu,
            this.Edit_BatchBuild});
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
            this.Edit_ValidatePartMenu.Click += new System.EventHandler(this.Edit_ValidatePartMenu_Click);
            // 
            // Edit_GenerateFilesMenu
            // 
            this.Edit_GenerateFilesMenu.Name = "Edit_GenerateFilesMenu";
            resources.ApplyResources(this.Edit_GenerateFilesMenu, "Edit_GenerateFilesMenu");
            this.Edit_GenerateFilesMenu.Click += new System.EventHandler(this.Edit_GenerateFilesMenu_Click);
            // 
            // Edit_BatchBuild
            // 
            this.Edit_BatchBuild.Name = "Edit_BatchBuild";
            resources.ApplyResources(this.Edit_BatchBuild, "Edit_BatchBuild");
            // 
            // ToolsMenuItem
            // 
            this.ToolsMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ExportBrickMenuItem,
            this.Tools_ConnectionsReportMenu,
            this.toolStripSeparator3,
            this.StartLddMenuItem,
            this.Tools_OpenPartMenu,
            this.toolStripSeparator7,
            this.Tools_SettingsMenu});
            this.ToolsMenuItem.Name = "ToolsMenuItem";
            resources.ApplyResources(this.ToolsMenuItem, "ToolsMenuItem");
            this.ToolsMenuItem.DropDownOpening += new System.EventHandler(this.ToolsMenuItem_DropDownOpening);
            // 
            // ExportBrickMenuItem
            // 
            this.ExportBrickMenuItem.Name = "ExportBrickMenuItem";
            resources.ApplyResources(this.ExportBrickMenuItem, "ExportBrickMenuItem");
            this.ExportBrickMenuItem.Click += new System.EventHandler(this.ExportBrickMenuItem_Click);
            // 
            // Tools_ConnectionsReportMenu
            // 
            this.Tools_ConnectionsReportMenu.Name = "Tools_ConnectionsReportMenu";
            resources.ApplyResources(this.Tools_ConnectionsReportMenu, "Tools_ConnectionsReportMenu");
            this.Tools_ConnectionsReportMenu.Click += new System.EventHandler(this.Tools_ConnectionsReportMenu_Click);
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
            this.StartLddMenuItem.Click += new System.EventHandler(this.StartLddMenuItem_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            resources.ApplyResources(this.toolStripSeparator7, "toolStripSeparator7");
            // 
            // Tools_SettingsMenu
            // 
            this.Tools_SettingsMenu.Name = "Tools_SettingsMenu";
            resources.ApplyResources(this.Tools_SettingsMenu, "Tools_SettingsMenu");
            this.Tools_SettingsMenu.Click += new System.EventHandler(this.Settings_EnvironmentMenu_Click);
            // 
            // WindowMenuItem
            // 
            this.WindowMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.WindowMenu_SaveLayout,
            this.WindowMenu_ApplyLayout,
            this.WindowMenu_ManageLayouts,
            this.WindowMenu_ResetLayout});
            this.WindowMenuItem.Name = "WindowMenuItem";
            resources.ApplyResources(this.WindowMenuItem, "WindowMenuItem");
            // 
            // WindowMenu_SaveLayout
            // 
            this.WindowMenu_SaveLayout.Name = "WindowMenu_SaveLayout";
            resources.ApplyResources(this.WindowMenu_SaveLayout, "WindowMenu_SaveLayout");
            this.WindowMenu_SaveLayout.Click += new System.EventHandler(this.WindowMenu_SaveLayout_Click);
            // 
            // WindowMenu_ApplyLayout
            // 
            this.WindowMenu_ApplyLayout.Name = "WindowMenu_ApplyLayout";
            resources.ApplyResources(this.WindowMenu_ApplyLayout, "WindowMenu_ApplyLayout");
            // 
            // WindowMenu_ManageLayouts
            // 
            this.WindowMenu_ManageLayouts.Name = "WindowMenu_ManageLayouts";
            resources.ApplyResources(this.WindowMenu_ManageLayouts, "WindowMenu_ManageLayouts");
            this.WindowMenu_ManageLayouts.Click += new System.EventHandler(this.WindowMenu_ManageLayouts_Click);
            // 
            // WindowMenu_ResetLayout
            // 
            this.WindowMenu_ResetLayout.Name = "WindowMenu_ResetLayout";
            resources.ApplyResources(this.WindowMenu_ResetLayout, "WindowMenu_ResetLayout");
            this.WindowMenu_ResetLayout.Click += new System.EventHandler(this.WindowMenu_ResetLayout_Click);
            // 
            // HelpMenuItem
            // 
            this.HelpMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.HelpMenu_About});
            this.HelpMenuItem.Name = "HelpMenuItem";
            resources.ApplyResources(this.HelpMenuItem, "HelpMenuItem");
            // 
            // HelpMenu_About
            // 
            this.HelpMenu_About.Name = "HelpMenu_About";
            resources.ApplyResources(this.HelpMenu_About, "HelpMenu_About");
            this.HelpMenu_About.Click += new System.EventHandler(this.HelpMenu_About_Click);
            // 
            // SelectedBuildLabel
            // 
            this.SelectedBuildLabel.Margin = new System.Windows.Forms.Padding(40, 1, 0, 2);
            this.SelectedBuildLabel.Name = "SelectedBuildLabel";
            resources.ApplyResources(this.SelectedBuildLabel, "SelectedBuildLabel");
            // 
            // BuildConfigComboBox
            // 
            this.BuildConfigComboBox.DropDownWidth = 150;
            this.BuildConfigComboBox.Name = "BuildConfigComboBox";
            resources.ApplyResources(this.BuildConfigComboBox, "BuildConfigComboBox");
            this.BuildConfigComboBox.SelectedIndexChanged += new System.EventHandler(this.BuildConfigComboBox_SelectedIndexChanged);
            // 
            // AutoSaveTimer
            // 
            this.AutoSaveTimer.Interval = 15000;
            this.AutoSaveTimer.Tick += new System.EventHandler(this.AutoSaveTimer_Tick);
            // 
            // localizableStringList1
            // 
            this.localizableStringList1.Items.AddRange(new LDDModder.BrickEditor.Localization.LocalizableString[] {
            this.StartLddText,
            this.RestartLddText,
            this.WindowTitle});
            // 
            // StartLddText
            // 
            resources.ApplyResources(this.StartLddText, "StartLddText");
            // 
            // RestartLddText
            // 
            resources.ApplyResources(this.RestartLddText, "RestartLddText");
            // 
            // WindowTitle
            // 
            resources.ApplyResources(this.WindowTitle, "WindowTitle");
            // 
            // Tools_OpenPartMenu
            // 
            this.Tools_OpenPartMenu.Name = "Tools_OpenPartMenu";
            resources.ApplyResources(this.Tools_OpenPartMenu, "Tools_OpenPartMenu");
            this.Tools_OpenPartMenu.Click += new System.EventHandler(this.Tools_OpenPartMenu_Click);
            // 
            // BrickEditorWindow
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.DockPanelControl);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "BrickEditorWindow";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BrickEditorWindow_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.BrickEditorWindow_FormClosed);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private WeifenLuo.WinFormsUI.Docking.VS2015DarkTheme vS2015DarkTheme1;
        private WeifenLuo.WinFormsUI.Docking.VisualStudioToolStripExtender visualStudioToolStripExtender1;
        private WeifenLuo.WinFormsUI.Docking.VS2015LightTheme vS2015LightTheme1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem File_NewProjectMenu;
        private System.Windows.Forms.ToolStripMenuItem File_OpenProjectMenu;
        private System.Windows.Forms.ToolStripMenuItem Tools_SettingsMenu;
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
        private System.Windows.Forms.ToolStripLabel SelectedBuildLabel;
        private System.Windows.Forms.Timer AutoSaveTimer;
        private Localization.LocalizableStringList localizableStringList1;
        private Localization.LocalizableString StartLddText;
        private Localization.LocalizableString RestartLddText;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private Localization.LocalizableString WindowTitle;
        private System.Windows.Forms.ToolStripComboBox BuildConfigComboBox;
        private System.Windows.Forms.ToolStripMenuItem Edit_BatchBuild;
        public WeifenLuo.WinFormsUI.Docking.DockPanel DockPanelControl;
        private System.Windows.Forms.ToolStripMenuItem FileMenu_OpenPartFiles;
        private System.Windows.Forms.ToolStripMenuItem WindowMenuItem;
        private System.Windows.Forms.ToolStripMenuItem WindowMenu_SaveLayout;
        private System.Windows.Forms.ToolStripMenuItem WindowMenu_ResetLayout;
        private System.Windows.Forms.ToolStripMenuItem WindowMenu_ApplyLayout;
        private System.Windows.Forms.ToolStripMenuItem WindowMenu_ManageLayouts;
        private System.Windows.Forms.ToolStripMenuItem HelpMenuItem;
        private System.Windows.Forms.ToolStripMenuItem HelpMenu_About;
        private System.Windows.Forms.ToolStripMenuItem Tools_ConnectionsReportMenu;
        private System.Windows.Forms.ToolStripMenuItem Tools_OpenPartMenu;
    }
}