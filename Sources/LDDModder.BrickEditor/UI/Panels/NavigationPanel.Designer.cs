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
            this.olvColumnElement = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnVisible = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
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
            this.AddConnectionMenu_Axel = new System.Windows.Forms.ToolStripMenuItem();
            this.AddConnectionMenu_Ball = new System.Windows.Forms.ToolStripMenuItem();
            this.AddConnectionMenu_Custom2DField = new System.Windows.Forms.ToolStripMenuItem();
            this.AddConnectionMenu_Fixed = new System.Windows.Forms.ToolStripMenuItem();
            this.AddConnectionMenu_Gear = new System.Windows.Forms.ToolStripMenuItem();
            this.AddConnectionMenu_Hinge = new System.Windows.Forms.ToolStripMenuItem();
            this.AddConnectionMenu_Rail = new System.Windows.Forms.ToolStripMenuItem();
            this.AddConnectionMenu_Slider = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ContextMenu_Rename = new System.Windows.Forms.ToolStripMenuItem();
            this.ContextMenu_Delete = new System.Windows.Forms.ToolStripMenuItem();
            this.NavigationImageList = new System.Windows.Forms.ImageList(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.ViewModeComboBox = new System.Windows.Forms.ComboBox();
            this.LocalizedStrings = new LDDModder.BrickEditor.Localization.LocalizableStringList(this.components);
            this.ViewModeAll = new LDDModder.BrickEditor.Localization.LocalizableString();
            this.ViewModeBones = new LDDModder.BrickEditor.Localization.LocalizableString();
            this.ViewModeCollisions = new LDDModder.BrickEditor.Localization.LocalizableString();
            this.ViewModeConnections = new LDDModder.BrickEditor.Localization.LocalizableString();
            this.ViewModeSurfaces = new LDDModder.BrickEditor.Localization.LocalizableString();
            ((System.ComponentModel.ISupportInitialize)(this.ProjectTreeView)).BeginInit();
            this.ElementsContextMenu.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ProjectTreeView
            // 
            this.ProjectTreeView.AllColumns.Add(this.olvColumnElement);
            this.ProjectTreeView.AllColumns.Add(this.olvColumnVisible);
            this.ProjectTreeView.AlternateRowBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.ProjectTreeView.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.DoubleClick;
            this.ProjectTreeView.CellEditUseWholeCell = false;
            this.ProjectTreeView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumnElement,
            this.olvColumnVisible});
            this.ProjectTreeView.ContextMenuStrip = this.ElementsContextMenu;
            resources.ApplyResources(this.ProjectTreeView, "ProjectTreeView");
            this.ProjectTreeView.FullRowSelect = true;
            this.ProjectTreeView.HideSelection = false;
            this.ProjectTreeView.Name = "ProjectTreeView";
            this.ProjectTreeView.ShowGroups = false;
            this.ProjectTreeView.SmallImageList = this.NavigationImageList;
            this.ProjectTreeView.UseAlternatingBackColors = true;
            this.ProjectTreeView.UseCompatibleStateImageBehavior = false;
            this.ProjectTreeView.UseFiltering = true;
            this.ProjectTreeView.UseHotItem = true;
            this.ProjectTreeView.View = System.Windows.Forms.View.Details;
            this.ProjectTreeView.VirtualMode = true;
            this.ProjectTreeView.CanDrop += new System.EventHandler<BrightIdeasSoftware.OlvDropEventArgs>(this.ProjectTreeView_CanDrop);
            this.ProjectTreeView.CellEditFinishing += new BrightIdeasSoftware.CellEditEventHandler(this.ProjectTreeView_CellEditFinishing);
            this.ProjectTreeView.CellEditStarting += new BrightIdeasSoftware.CellEditEventHandler(this.ProjectTreeView_CellEditStarting);
            this.ProjectTreeView.CellClick += new System.EventHandler<BrightIdeasSoftware.CellClickEventArgs>(this.ProjectTreeView_CellClick);
            this.ProjectTreeView.Dropped += new System.EventHandler<BrightIdeasSoftware.OlvDropEventArgs>(this.ProjectTreeView_Dropped);
            this.ProjectTreeView.SelectionChanged += new System.EventHandler(this.ProjectTreeView_SelectionChanged);
            this.ProjectTreeView.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.ProjectTreeView_ItemSelectionChanged);
            this.ProjectTreeView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ProjectTreeView_MouseDown);
            this.ProjectTreeView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ProjectTreeView_MouseUp);
            // 
            // olvColumnElement
            // 
            this.olvColumnElement.AspectName = "Text";
            this.olvColumnElement.CellEditUseWholeCell = true;
            this.olvColumnElement.FillsFreeSpace = true;
            this.olvColumnElement.ImageAspectName = "ImageKey";
            this.olvColumnElement.Sortable = false;
            resources.ApplyResources(this.olvColumnElement, "olvColumnElement");
            // 
            // olvColumnVisible
            // 
            this.olvColumnVisible.MinimumWidth = 20;
            this.olvColumnVisible.ShowTextInHeader = false;
            this.olvColumnVisible.Sortable = false;
            resources.ApplyResources(this.olvColumnVisible, "olvColumnVisible");
            // 
            // ElementsContextMenu
            // 
            this.ElementsContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ContextMenu_AddSurface,
            this.ContextMenu_AddElement,
            this.ContextMenu_AddCollision,
            this.ContextMenu_AddConnection,
            this.toolStripSeparator1,
            this.ContextMenu_Rename,
            this.ContextMenu_Delete});
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
            this.AddCollisionMenu_Box.Tag = "Box";
            // 
            // AddCollisionMenu_Sphere
            // 
            this.AddCollisionMenu_Sphere.Name = "AddCollisionMenu_Sphere";
            resources.ApplyResources(this.AddCollisionMenu_Sphere, "AddCollisionMenu_Sphere");
            this.AddCollisionMenu_Sphere.Tag = "Sphere";
            // 
            // ContextMenu_AddConnection
            // 
            this.ContextMenu_AddConnection.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddConnectionMenu_Axel,
            this.AddConnectionMenu_Ball,
            this.AddConnectionMenu_Custom2DField,
            this.AddConnectionMenu_Fixed,
            this.AddConnectionMenu_Gear,
            this.AddConnectionMenu_Hinge,
            this.AddConnectionMenu_Rail,
            this.AddConnectionMenu_Slider});
            this.ContextMenu_AddConnection.Name = "ContextMenu_AddConnection";
            resources.ApplyResources(this.ContextMenu_AddConnection, "ContextMenu_AddConnection");
            // 
            // AddConnectionMenu_Axel
            // 
            this.AddConnectionMenu_Axel.Name = "AddConnectionMenu_Axel";
            resources.ApplyResources(this.AddConnectionMenu_Axel, "AddConnectionMenu_Axel");
            this.AddConnectionMenu_Axel.Tag = "Axel";
            // 
            // AddConnectionMenu_Ball
            // 
            this.AddConnectionMenu_Ball.Name = "AddConnectionMenu_Ball";
            resources.ApplyResources(this.AddConnectionMenu_Ball, "AddConnectionMenu_Ball");
            this.AddConnectionMenu_Ball.Tag = "Ball";
            // 
            // AddConnectionMenu_Custom2DField
            // 
            this.AddConnectionMenu_Custom2DField.Name = "AddConnectionMenu_Custom2DField";
            resources.ApplyResources(this.AddConnectionMenu_Custom2DField, "AddConnectionMenu_Custom2DField");
            this.AddConnectionMenu_Custom2DField.Tag = "Custom2DField";
            // 
            // AddConnectionMenu_Fixed
            // 
            this.AddConnectionMenu_Fixed.Name = "AddConnectionMenu_Fixed";
            resources.ApplyResources(this.AddConnectionMenu_Fixed, "AddConnectionMenu_Fixed");
            this.AddConnectionMenu_Fixed.Tag = "Fixed";
            // 
            // AddConnectionMenu_Gear
            // 
            this.AddConnectionMenu_Gear.Name = "AddConnectionMenu_Gear";
            resources.ApplyResources(this.AddConnectionMenu_Gear, "AddConnectionMenu_Gear");
            this.AddConnectionMenu_Gear.Tag = "Gear";
            // 
            // AddConnectionMenu_Hinge
            // 
            this.AddConnectionMenu_Hinge.Name = "AddConnectionMenu_Hinge";
            resources.ApplyResources(this.AddConnectionMenu_Hinge, "AddConnectionMenu_Hinge");
            this.AddConnectionMenu_Hinge.Tag = "Hinge";
            // 
            // AddConnectionMenu_Rail
            // 
            this.AddConnectionMenu_Rail.Name = "AddConnectionMenu_Rail";
            resources.ApplyResources(this.AddConnectionMenu_Rail, "AddConnectionMenu_Rail");
            this.AddConnectionMenu_Rail.Tag = "Rail";
            // 
            // AddConnectionMenu_Slider
            // 
            this.AddConnectionMenu_Slider.Name = "AddConnectionMenu_Slider";
            resources.ApplyResources(this.AddConnectionMenu_Slider, "AddConnectionMenu_Slider");
            this.AddConnectionMenu_Slider.Tag = "Slider";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // ContextMenu_Rename
            // 
            this.ContextMenu_Rename.Name = "ContextMenu_Rename";
            resources.ApplyResources(this.ContextMenu_Rename, "ContextMenu_Rename");
            this.ContextMenu_Rename.Click += new System.EventHandler(this.ContextMenu_Rename_Click);
            // 
            // ContextMenu_Delete
            // 
            this.ContextMenu_Delete.Name = "ContextMenu_Delete";
            resources.ApplyResources(this.ContextMenu_Delete, "ContextMenu_Delete");
            this.ContextMenu_Delete.Click += new System.EventHandler(this.ContextMenu_Delete_Click);
            // 
            // NavigationImageList
            // 
            this.NavigationImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            resources.ApplyResources(this.NavigationImageList, "NavigationImageList");
            this.NavigationImageList.TransparentColor = System.Drawing.Color.Transparent;
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
            this.LocalizedStrings.Items.AddRange(new LDDModder.BrickEditor.Localization.LocalizableString[] {
            this.ViewModeAll,
            this.ViewModeBones,
            this.ViewModeCollisions,
            this.ViewModeConnections,
            this.ViewModeSurfaces});
            // 
            // ViewModeAll
            // 
            resources.ApplyResources(this.ViewModeAll, "ViewModeAll");
            // 
            // ViewModeBones
            // 
            resources.ApplyResources(this.ViewModeBones, "ViewModeBones");
            // 
            // ViewModeCollisions
            // 
            resources.ApplyResources(this.ViewModeCollisions, "ViewModeCollisions");
            // 
            // ViewModeConnections
            // 
            resources.ApplyResources(this.ViewModeConnections, "ViewModeConnections");
            // 
            // ViewModeSurfaces
            // 
            resources.ApplyResources(this.ViewModeSurfaces, "ViewModeSurfaces");
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
        private BrightIdeasSoftware.OLVColumn olvColumnElement;
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
        private System.Windows.Forms.ToolStripMenuItem ContextMenu_Delete;
        private System.Windows.Forms.ToolStripMenuItem ContextMenu_AddSurface;
        private System.Windows.Forms.ToolStripMenuItem AddElementMenu_FemaleStud;
        private System.Windows.Forms.ToolStripMenuItem AddElementMenu_BrickTube;
        private System.Windows.Forms.ToolStripMenuItem ContextMenu_AddCollision;
        private System.Windows.Forms.ToolStripMenuItem AddCollisionMenu_Box;
        private System.Windows.Forms.ToolStripMenuItem AddCollisionMenu_Sphere;
        private System.Windows.Forms.ToolStripMenuItem ContextMenu_AddConnection;
        private System.Windows.Forms.ToolStripMenuItem AddConnectionMenu_Custom2DField;
        private System.Windows.Forms.ToolStripMenuItem AddConnectionMenu_Axel;
        private System.Windows.Forms.ToolStripMenuItem AddConnectionMenu_Hinge;
        private System.Windows.Forms.ToolStripMenuItem AddConnectionMenu_Gear;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem AddConnectionMenu_Ball;
        private System.Windows.Forms.ToolStripMenuItem AddConnectionMenu_Rail;
        private System.Windows.Forms.ToolStripMenuItem AddConnectionMenu_Slider;
        private System.Windows.Forms.ToolStripMenuItem AddConnectionMenu_Fixed;
        private System.Windows.Forms.ImageList NavigationImageList;
        private BrightIdeasSoftware.OLVColumn olvColumnVisible;
        private System.Windows.Forms.ToolStripMenuItem ContextMenu_Rename;
    }
}