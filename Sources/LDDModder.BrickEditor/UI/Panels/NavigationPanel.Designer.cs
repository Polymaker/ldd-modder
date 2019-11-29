namespace LDDModder.BrickEditor.UI.Panels
{
    partial class NavigationPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NavigationPanel));
            this.ProjectTreeView = new BrightIdeasSoftware.TreeListView();
            this.olvColumn1 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.ElementsContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ContextMenu_AddSurface = new System.Windows.Forms.ToolStripMenuItem();
            this.ContextMenu_AddElement = new System.Windows.Forms.ToolStripMenuItem();
            this.AddElementMenu_Part = new System.Windows.Forms.ToolStripMenuItem();
            this.AddElementMenu_MaleStud = new System.Windows.Forms.ToolStripMenuItem();
            this.AddElementMenu_FemaleStud = new System.Windows.Forms.ToolStripMenuItem();
            this.AddElementMenu_BrickTube = new System.Windows.Forms.ToolStripMenuItem();
            this.ContextMenu_AddCollision = new System.Windows.Forms.ToolStripMenuItem();
            this.AddCollisionMenu_Box = new System.Windows.Forms.ToolStripMenuItem();
            this.AddCollisionMenu_Sphere = new System.Windows.Forms.ToolStripMenuItem();
            this.ContextMenu_AddConnection = new System.Windows.Forms.ToolStripMenuItem();
            this.studsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.technicToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hingeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ballToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.railToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sliderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fixedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ElementsMenu_Delete = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.ViewModeComboBox = new System.Windows.Forms.ComboBox();
            this.LocalizedStrings = new LDDModder.BrickEditor.Localization.LocalizableStringList(this.components);
            this.ViewModeSurfaces = new LDDModder.BrickEditor.Localization.LocalizableString(this.components);
            this.ViewModeCollisions = new LDDModder.BrickEditor.Localization.LocalizableString(this.components);
            this.ViewModeConnections = new LDDModder.BrickEditor.Localization.LocalizableString(this.components);
            this.ViewModeBones = new LDDModder.BrickEditor.Localization.LocalizableString(this.components);
            this.ViewModeAll = new LDDModder.BrickEditor.Localization.LocalizableString(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.ProjectTreeView)).BeginInit();
            this.ElementsContextMenu.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ProjectTreeView
            // 
            this.ProjectTreeView.AllColumns.Add(this.olvColumn1);
            this.ProjectTreeView.CellEditUseWholeCell = false;
            this.ProjectTreeView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumn1});
            this.ProjectTreeView.ContextMenuStrip = this.ElementsContextMenu;
            this.ProjectTreeView.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.ProjectTreeView, "ProjectTreeView");
            this.ProjectTreeView.HideSelection = false;
            this.ProjectTreeView.Name = "ProjectTreeView";
            this.ProjectTreeView.ShowGroups = false;
            this.ProjectTreeView.UseCompatibleStateImageBehavior = false;
            this.ProjectTreeView.View = System.Windows.Forms.View.Details;
            this.ProjectTreeView.VirtualMode = true;
            this.ProjectTreeView.SelectionChanged += new System.EventHandler(this.ProjectTreeView_SelectionChanged);
            // 
            // olvColumn1
            // 
            this.olvColumn1.AspectName = "Text";
            this.olvColumn1.FillsFreeSpace = true;
            this.olvColumn1.IsEditable = false;
            this.olvColumn1.Sortable = false;
            resources.ApplyResources(this.olvColumn1, "olvColumn1");
            // 
            // ElementsContextMenu
            // 
            this.ElementsContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ContextMenu_AddSurface,
            this.ContextMenu_AddElement,
            this.ContextMenu_AddCollision,
            this.ContextMenu_AddConnection,
            this.toolStripSeparator1,
            this.ElementsMenu_Delete});
            this.ElementsContextMenu.Name = "contextMenuStrip1";
            resources.ApplyResources(this.ElementsContextMenu, "ElementsContextMenu");
            this.ElementsContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.ElementsContextMenu_Opening);
            // 
            // ContextMenu_AddSurface
            // 
            this.ContextMenu_AddSurface.Name = "ContextMenu_AddSurface";
            resources.ApplyResources(this.ContextMenu_AddSurface, "ContextMenu_AddSurface");
            // 
            // ContextMenu_AddElement
            // 
            this.ContextMenu_AddElement.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddElementMenu_Part,
            this.AddElementMenu_MaleStud,
            this.AddElementMenu_FemaleStud,
            this.AddElementMenu_BrickTube});
            this.ContextMenu_AddElement.Name = "ContextMenu_AddElement";
            resources.ApplyResources(this.ContextMenu_AddElement, "ContextMenu_AddElement");
            // 
            // AddElementMenu_Part
            // 
            this.AddElementMenu_Part.Name = "AddElementMenu_Part";
            resources.ApplyResources(this.AddElementMenu_Part, "AddElementMenu_Part");
            // 
            // AddElementMenu_MaleStud
            // 
            this.AddElementMenu_MaleStud.Name = "AddElementMenu_MaleStud";
            resources.ApplyResources(this.AddElementMenu_MaleStud, "AddElementMenu_MaleStud");
            // 
            // AddElementMenu_FemaleStud
            // 
            this.AddElementMenu_FemaleStud.Name = "AddElementMenu_FemaleStud";
            resources.ApplyResources(this.AddElementMenu_FemaleStud, "AddElementMenu_FemaleStud");
            // 
            // AddElementMenu_BrickTube
            // 
            this.AddElementMenu_BrickTube.Name = "AddElementMenu_BrickTube";
            resources.ApplyResources(this.AddElementMenu_BrickTube, "AddElementMenu_BrickTube");
            // 
            // ContextMenu_AddCollision
            // 
            this.ContextMenu_AddCollision.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddCollisionMenu_Box,
            this.AddCollisionMenu_Sphere});
            this.ContextMenu_AddCollision.Name = "ContextMenu_AddCollision";
            resources.ApplyResources(this.ContextMenu_AddCollision, "ContextMenu_AddCollision");
            // 
            // AddCollisionMenu_Box
            // 
            this.AddCollisionMenu_Box.Name = "AddCollisionMenu_Box";
            resources.ApplyResources(this.AddCollisionMenu_Box, "AddCollisionMenu_Box");
            this.AddCollisionMenu_Box.Click += new System.EventHandler(this.AddCollisionMenu_Box_Click);
            // 
            // AddCollisionMenu_Sphere
            // 
            this.AddCollisionMenu_Sphere.Name = "AddCollisionMenu_Sphere";
            resources.ApplyResources(this.AddCollisionMenu_Sphere, "AddCollisionMenu_Sphere");
            this.AddCollisionMenu_Sphere.Click += new System.EventHandler(this.AddCollisionMenu_Sphere_Click);
            // 
            // ContextMenu_AddConnection
            // 
            this.ContextMenu_AddConnection.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.studsToolStripMenuItem,
            this.technicToolStripMenuItem,
            this.hingeToolStripMenuItem,
            this.ballToolStripMenuItem,
            this.railToolStripMenuItem,
            this.gearToolStripMenuItem,
            this.sliderToolStripMenuItem,
            this.fixedToolStripMenuItem});
            this.ContextMenu_AddConnection.Name = "ContextMenu_AddConnection";
            resources.ApplyResources(this.ContextMenu_AddConnection, "ContextMenu_AddConnection");
            // 
            // studsToolStripMenuItem
            // 
            this.studsToolStripMenuItem.Name = "studsToolStripMenuItem";
            resources.ApplyResources(this.studsToolStripMenuItem, "studsToolStripMenuItem");
            // 
            // technicToolStripMenuItem
            // 
            this.technicToolStripMenuItem.Name = "technicToolStripMenuItem";
            resources.ApplyResources(this.technicToolStripMenuItem, "technicToolStripMenuItem");
            // 
            // hingeToolStripMenuItem
            // 
            this.hingeToolStripMenuItem.Name = "hingeToolStripMenuItem";
            resources.ApplyResources(this.hingeToolStripMenuItem, "hingeToolStripMenuItem");
            // 
            // ballToolStripMenuItem
            // 
            this.ballToolStripMenuItem.Name = "ballToolStripMenuItem";
            resources.ApplyResources(this.ballToolStripMenuItem, "ballToolStripMenuItem");
            // 
            // railToolStripMenuItem
            // 
            this.railToolStripMenuItem.Name = "railToolStripMenuItem";
            resources.ApplyResources(this.railToolStripMenuItem, "railToolStripMenuItem");
            // 
            // gearToolStripMenuItem
            // 
            this.gearToolStripMenuItem.Name = "gearToolStripMenuItem";
            resources.ApplyResources(this.gearToolStripMenuItem, "gearToolStripMenuItem");
            // 
            // sliderToolStripMenuItem
            // 
            this.sliderToolStripMenuItem.Name = "sliderToolStripMenuItem";
            resources.ApplyResources(this.sliderToolStripMenuItem, "sliderToolStripMenuItem");
            // 
            // fixedToolStripMenuItem
            // 
            this.fixedToolStripMenuItem.Name = "fixedToolStripMenuItem";
            resources.ApplyResources(this.fixedToolStripMenuItem, "fixedToolStripMenuItem");
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // ElementsMenu_Delete
            // 
            this.ElementsMenu_Delete.Name = "ElementsMenu_Delete";
            resources.ApplyResources(this.ElementsMenu_Delete, "ElementsMenu_Delete");
            this.ElementsMenu_Delete.Click += new System.EventHandler(this.ElementsMenu_Delete_Click);
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.ProjectTreeView, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.ViewModeComboBox, 0, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // ViewModeComboBox
            // 
            resources.ApplyResources(this.ViewModeComboBox, "ViewModeComboBox");
            this.ViewModeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ViewModeComboBox.FormattingEnabled = true;
            this.ViewModeComboBox.Name = "ViewModeComboBox";
            this.ViewModeComboBox.SelectedIndexChanged += new System.EventHandler(this.ViewModeComboBox_SelectedIndexChanged);
            // 
            // LocalizedStrings
            // 
            this.LocalizedStrings.Items.Add(this.ViewModeSurfaces);
            this.LocalizedStrings.Items.Add(this.ViewModeCollisions);
            this.LocalizedStrings.Items.Add(this.ViewModeConnections);
            this.LocalizedStrings.Items.Add(this.ViewModeBones);
            this.LocalizedStrings.Items.Add(this.ViewModeAll);
            // 
            // ViewModeSurfaces
            // 
            resources.ApplyResources(this.ViewModeSurfaces, "ViewModeSurfaces");
            // 
            // ViewModeCollisions
            // 
            resources.ApplyResources(this.ViewModeCollisions, "ViewModeCollisions");
            // 
            // ViewModeConnections
            // 
            resources.ApplyResources(this.ViewModeConnections, "ViewModeConnections");
            // 
            // ViewModeBones
            // 
            resources.ApplyResources(this.ViewModeBones, "ViewModeBones");
            // 
            // ViewModeAll
            // 
            resources.ApplyResources(this.ViewModeAll, "ViewModeAll");
            // 
            // NavigationPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "NavigationPanel";
            ((System.ComponentModel.ISupportInitialize)(this.ProjectTreeView)).EndInit();
            this.ElementsContextMenu.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private BrightIdeasSoftware.TreeListView ProjectTreeView;
        private BrightIdeasSoftware.OLVColumn olvColumn1;
        private System.Windows.Forms.ContextMenuStrip ElementsContextMenu;
        private System.Windows.Forms.ToolStripMenuItem ContextMenu_AddElement;
        private System.Windows.Forms.ToolStripMenuItem AddElementMenu_Part;
        private System.Windows.Forms.ToolStripMenuItem AddElementMenu_MaleStud;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox ViewModeComboBox;
        private Localization.LocalizableStringList LocalizedStrings;
        private Localization.LocalizableString ViewModeSurfaces;
        private Localization.LocalizableString ViewModeCollisions;
        private Localization.LocalizableString ViewModeConnections;
        private Localization.LocalizableString ViewModeBones;
        private Localization.LocalizableString ViewModeAll;
        private System.Windows.Forms.ToolStripMenuItem ElementsMenu_Delete;
        private System.Windows.Forms.ToolStripMenuItem ContextMenu_AddSurface;
        private System.Windows.Forms.ToolStripMenuItem AddElementMenu_FemaleStud;
        private System.Windows.Forms.ToolStripMenuItem AddElementMenu_BrickTube;
        private System.Windows.Forms.ToolStripMenuItem ContextMenu_AddCollision;
        private System.Windows.Forms.ToolStripMenuItem AddCollisionMenu_Box;
        private System.Windows.Forms.ToolStripMenuItem AddCollisionMenu_Sphere;
        private System.Windows.Forms.ToolStripMenuItem ContextMenu_AddConnection;
        private System.Windows.Forms.ToolStripMenuItem studsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem technicToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hingeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gearToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem ballToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem railToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sliderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fixedToolStripMenuItem;
    }
}