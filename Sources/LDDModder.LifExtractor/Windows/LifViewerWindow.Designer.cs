namespace LDDModder.LifExtractor.Windows
{
    partial class LifViewerWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LifViewerWindow));
            this.StatusToolStrip = new System.Windows.Forms.StatusStrip();
            this.CurrentFileStripLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.MainMenuToolStrip = new System.Windows.Forms.MenuStrip();
            this.MainMenu_FileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.FileMenu_OpenItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newLIFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.FileMenu_ExtractItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MainMenu_ViewMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewModeDetailsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewModeSmallMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewModeLargeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.LifTreeView = new System.Windows.Forms.TreeView();
            this.SmallIconImageList = new System.Windows.Forms.ImageList(this.components);
            this.FolderListView = new BrightIdeasSoftware.DataListView();
            this.FlvNameColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.FlvTypeColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.FlvSizeColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.FlvCreatedColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.FlvModifiedColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.FolderListContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ListMenu_ExtractItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ListMenu_CreateFolderItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ListMenu_AddFileItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ListMenu_RenameItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ListMenu_DeleteItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LargeIconImageList = new System.Windows.Forms.ImageList(this.components);
            this.NavigationToolStrip = new System.Windows.Forms.ToolStrip();
            this.BackToolbarButton = new System.Windows.Forms.ToolStripButton();
            this.NextToolbarButton = new System.Windows.Forms.ToolStripButton();
            this.UpToolbarButton = new System.Windows.Forms.ToolStripButton();
            this.ToolBarFolderCombo = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.ActionsMenuToolStrip = new System.Windows.Forms.ToolStrip();
            this.ActionsMenu_Extract = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.ActionsMenu_EnableEdit = new System.Windows.Forms.ToolStripButton();
            this.ActionsMenu_AddFile = new System.Windows.Forms.ToolStripButton();
            this.StatusToolStrip.SuspendLayout();
            this.MainMenuToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FolderListView)).BeginInit();
            this.FolderListContextMenu.SuspendLayout();
            this.NavigationToolStrip.SuspendLayout();
            this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.ActionsMenuToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // StatusToolStrip
            // 
            this.StatusToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.StatusToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CurrentFileStripLabel});
            this.StatusToolStrip.Location = new System.Drawing.Point(0, 0);
            this.StatusToolStrip.Name = "StatusToolStrip";
            this.StatusToolStrip.Size = new System.Drawing.Size(784, 22);
            this.StatusToolStrip.TabIndex = 0;
            this.StatusToolStrip.Text = "statusStrip1";
            // 
            // CurrentFileStripLabel
            // 
            this.CurrentFileStripLabel.Name = "CurrentFileStripLabel";
            this.CurrentFileStripLabel.Size = new System.Drawing.Size(168, 17);
            this.CurrentFileStripLabel.Text = "...\\LEGO Digital Designer\\db.lif";
            // 
            // MainMenuToolStrip
            // 
            this.MainMenuToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.MainMenuToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MainMenu_FileMenu,
            this.MainMenu_ViewMenu});
            this.MainMenuToolStrip.Location = new System.Drawing.Point(0, 0);
            this.MainMenuToolStrip.Name = "MainMenuToolStrip";
            this.MainMenuToolStrip.Size = new System.Drawing.Size(784, 24);
            this.MainMenuToolStrip.TabIndex = 1;
            this.MainMenuToolStrip.Text = "menuStrip1";
            // 
            // MainMenu_FileMenu
            // 
            this.MainMenu_FileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileMenu_OpenItem,
            this.newLIFToolStripMenuItem,
            this.toolStripSeparator2,
            this.FileMenu_ExtractItem});
            this.MainMenu_FileMenu.Name = "MainMenu_FileMenu";
            this.MainMenu_FileMenu.Size = new System.Drawing.Size(37, 20);
            this.MainMenu_FileMenu.Text = "File";
            // 
            // FileMenu_OpenItem
            // 
            this.FileMenu_OpenItem.Name = "FileMenu_OpenItem";
            this.FileMenu_OpenItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.FileMenu_OpenItem.Size = new System.Drawing.Size(159, 22);
            this.FileMenu_OpenItem.Text = "Open";
            this.FileMenu_OpenItem.Click += new System.EventHandler(this.FileOpenMenuItem_Click);
            // 
            // newLIFToolStripMenuItem
            // 
            this.newLIFToolStripMenuItem.Name = "newLIFToolStripMenuItem";
            this.newLIFToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newLIFToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.newLIFToolStripMenuItem.Text = "New LIF";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(156, 6);
            // 
            // FileMenu_ExtractItem
            // 
            this.FileMenu_ExtractItem.Enabled = false;
            this.FileMenu_ExtractItem.Name = "FileMenu_ExtractItem";
            this.FileMenu_ExtractItem.Size = new System.Drawing.Size(159, 22);
            this.FileMenu_ExtractItem.Text = "Extract to...";
            this.FileMenu_ExtractItem.Click += new System.EventHandler(this.FileMenu_ExtractItem_Click);
            // 
            // MainMenu_ViewMenu
            // 
            this.MainMenu_ViewMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ViewModeDetailsMenuItem,
            this.ViewModeSmallMenuItem,
            this.ViewModeLargeMenuItem});
            this.MainMenu_ViewMenu.Name = "MainMenu_ViewMenu";
            this.MainMenu_ViewMenu.Size = new System.Drawing.Size(44, 20);
            this.MainMenu_ViewMenu.Text = "View";
            // 
            // ViewModeDetailsMenuItem
            // 
            this.ViewModeDetailsMenuItem.Checked = true;
            this.ViewModeDetailsMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ViewModeDetailsMenuItem.Name = "ViewModeDetailsMenuItem";
            this.ViewModeDetailsMenuItem.Size = new System.Drawing.Size(134, 22);
            this.ViewModeDetailsMenuItem.Text = "Details";
            this.ViewModeDetailsMenuItem.Click += new System.EventHandler(this.ViewModeMenuItems_Click);
            // 
            // ViewModeSmallMenuItem
            // 
            this.ViewModeSmallMenuItem.Name = "ViewModeSmallMenuItem";
            this.ViewModeSmallMenuItem.Size = new System.Drawing.Size(134, 22);
            this.ViewModeSmallMenuItem.Text = "Small Icons";
            this.ViewModeSmallMenuItem.Click += new System.EventHandler(this.ViewModeMenuItems_Click);
            // 
            // ViewModeLargeMenuItem
            // 
            this.ViewModeLargeMenuItem.Name = "ViewModeLargeMenuItem";
            this.ViewModeLargeMenuItem.Size = new System.Drawing.Size(134, 22);
            this.ViewModeLargeMenuItem.Text = "Large Icons";
            this.ViewModeLargeMenuItem.Click += new System.EventHandler(this.ViewModeMenuItems_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.LifTreeView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.FolderListView);
            this.splitContainer1.Size = new System.Drawing.Size(784, 436);
            this.splitContainer1.SplitterDistance = 200;
            this.splitContainer1.TabIndex = 2;
            // 
            // LifTreeView
            // 
            this.LifTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LifTreeView.FullRowSelect = true;
            this.LifTreeView.HideSelection = false;
            this.LifTreeView.HotTracking = true;
            this.LifTreeView.ImageIndex = 0;
            this.LifTreeView.ImageList = this.SmallIconImageList;
            this.LifTreeView.Indent = 12;
            this.LifTreeView.ItemHeight = 20;
            this.LifTreeView.Location = new System.Drawing.Point(0, 0);
            this.LifTreeView.Name = "LifTreeView";
            this.LifTreeView.SelectedImageIndex = 0;
            this.LifTreeView.ShowLines = false;
            this.LifTreeView.Size = new System.Drawing.Size(200, 436);
            this.LifTreeView.TabIndex = 0;
            this.LifTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.LifTreeView_AfterSelect);
            // 
            // SmallIconImageList
            // 
            this.SmallIconImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.SmallIconImageList.ImageSize = new System.Drawing.Size(16, 16);
            this.SmallIconImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // FolderListView
            // 
            this.FolderListView.AllColumns.Add(this.FlvNameColumn);
            this.FolderListView.AllColumns.Add(this.FlvTypeColumn);
            this.FolderListView.AllColumns.Add(this.FlvSizeColumn);
            this.FolderListView.AllColumns.Add(this.FlvCreatedColumn);
            this.FolderListView.AllColumns.Add(this.FlvModifiedColumn);
            this.FolderListView.AllowDrop = true;
            this.FolderListView.AutoGenerateColumns = false;
            this.FolderListView.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.F2Only;
            this.FolderListView.CellEditUseWholeCell = false;
            this.FolderListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.FlvNameColumn,
            this.FlvTypeColumn,
            this.FlvSizeColumn,
            this.FlvCreatedColumn,
            this.FlvModifiedColumn});
            this.FolderListView.ContextMenuStrip = this.FolderListContextMenu;
            this.FolderListView.Cursor = System.Windows.Forms.Cursors.Default;
            this.FolderListView.DataSource = null;
            this.FolderListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FolderListView.FullRowSelect = true;
            this.FolderListView.HideSelection = false;
            this.FolderListView.LargeImageList = this.LargeIconImageList;
            this.FolderListView.Location = new System.Drawing.Point(0, 0);
            this.FolderListView.Name = "FolderListView";
            this.FolderListView.ShowGroups = false;
            this.FolderListView.ShowHeaderInAllViews = false;
            this.FolderListView.ShowItemToolTips = true;
            this.FolderListView.Size = new System.Drawing.Size(580, 436);
            this.FolderListView.SmallImageList = this.SmallIconImageList;
            this.FolderListView.TabIndex = 1;
            this.FolderListView.UseCompatibleStateImageBehavior = false;
            this.FolderListView.UseExplorerTheme = true;
            this.FolderListView.View = System.Windows.Forms.View.Details;
            this.FolderListView.BeforeSorting += new System.EventHandler<BrightIdeasSoftware.BeforeSortingEventArgs>(this.FolderListView_BeforeSorting);
            this.FolderListView.CellEditFinished += new BrightIdeasSoftware.CellEditEventHandler(this.FolderListView_CellEditFinished);
            this.FolderListView.CellEditValidating += new BrightIdeasSoftware.CellEditEventHandler(this.FolderListView_CellEditValidating);
            this.FolderListView.ItemActivate += new System.EventHandler(this.FolderListView_ItemActivate);
            this.FolderListView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.FolderListView_ItemDrag);
            // 
            // FlvNameColumn
            // 
            this.FlvNameColumn.AspectName = "Name";
            this.FlvNameColumn.AutoCompleteEditor = false;
            this.FlvNameColumn.AutoCompleteEditorMode = System.Windows.Forms.AutoCompleteMode.None;
            this.FlvNameColumn.ImageAspectName = "ItemImageKey";
            this.FlvNameColumn.Text = "Name";
            this.FlvNameColumn.Width = 150;
            // 
            // FlvTypeColumn
            // 
            this.FlvTypeColumn.AspectName = "Description";
            this.FlvTypeColumn.IsEditable = false;
            this.FlvTypeColumn.Text = "Type";
            this.FlvTypeColumn.Width = 130;
            // 
            // FlvSizeColumn
            // 
            this.FlvSizeColumn.AspectName = "Size";
            this.FlvSizeColumn.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.FlvSizeColumn.IsEditable = false;
            this.FlvSizeColumn.Text = "Size";
            this.FlvSizeColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.FlvSizeColumn.Width = 85;
            // 
            // FlvCreatedColumn
            // 
            this.FlvCreatedColumn.AspectName = "CreatedDate";
            this.FlvCreatedColumn.IsEditable = false;
            this.FlvCreatedColumn.Text = "Date created";
            this.FlvCreatedColumn.Width = 130;
            // 
            // FlvModifiedColumn
            // 
            this.FlvModifiedColumn.AspectName = "ModifiedDate";
            this.FlvModifiedColumn.IsEditable = false;
            this.FlvModifiedColumn.Text = "Date modified";
            this.FlvModifiedColumn.Width = 130;
            // 
            // FolderListContextMenu
            // 
            this.FolderListContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ListMenu_ExtractItem,
            this.toolStripSeparator1,
            this.ListMenu_CreateFolderItem,
            this.ListMenu_AddFileItem,
            this.ListMenu_RenameItem,
            this.ListMenu_DeleteItem});
            this.FolderListContextMenu.Name = "FolderListContextMenu";
            this.FolderListContextMenu.Size = new System.Drawing.Size(154, 120);
            this.FolderListContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.FolderListContextMenu_Opening);
            // 
            // ListMenu_ExtractItem
            // 
            this.ListMenu_ExtractItem.Name = "ListMenu_ExtractItem";
            this.ListMenu_ExtractItem.Size = new System.Drawing.Size(153, 22);
            this.ListMenu_ExtractItem.Text = "Extract…";
            this.ListMenu_ExtractItem.Click += new System.EventHandler(this.ListMenu_ExtractItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(150, 6);
            // 
            // ListMenu_CreateFolderItem
            // 
            this.ListMenu_CreateFolderItem.Name = "ListMenu_CreateFolderItem";
            this.ListMenu_CreateFolderItem.Size = new System.Drawing.Size(153, 22);
            this.ListMenu_CreateFolderItem.Text = "Create Folder…";
            this.ListMenu_CreateFolderItem.Click += new System.EventHandler(this.ListMenu_CreateFolderItem_Click);
            // 
            // ListMenu_AddFileItem
            // 
            this.ListMenu_AddFileItem.Name = "ListMenu_AddFileItem";
            this.ListMenu_AddFileItem.Size = new System.Drawing.Size(153, 22);
            this.ListMenu_AddFileItem.Text = "Add File…";
            // 
            // ListMenu_RenameItem
            // 
            this.ListMenu_RenameItem.Name = "ListMenu_RenameItem";
            this.ListMenu_RenameItem.Size = new System.Drawing.Size(153, 22);
            this.ListMenu_RenameItem.Text = "Rename";
            this.ListMenu_RenameItem.Click += new System.EventHandler(this.ListMenu_RenameItem_Click);
            // 
            // ListMenu_DeleteItem
            // 
            this.ListMenu_DeleteItem.Name = "ListMenu_DeleteItem";
            this.ListMenu_DeleteItem.Size = new System.Drawing.Size(153, 22);
            this.ListMenu_DeleteItem.Text = "Delete";
            this.ListMenu_DeleteItem.Click += new System.EventHandler(this.ListMenu_DeleteItem_Click);
            // 
            // LargeIconImageList
            // 
            this.LargeIconImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.LargeIconImageList.ImageSize = new System.Drawing.Size(32, 32);
            this.LargeIconImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // NavigationToolStrip
            // 
            this.NavigationToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.NavigationToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.NavigationToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.BackToolbarButton,
            this.NextToolbarButton,
            this.UpToolbarButton,
            this.ToolBarFolderCombo});
            this.NavigationToolStrip.Location = new System.Drawing.Point(0, 78);
            this.NavigationToolStrip.Name = "NavigationToolStrip";
            this.NavigationToolStrip.Size = new System.Drawing.Size(784, 25);
            this.NavigationToolStrip.Stretch = true;
            this.NavigationToolStrip.TabIndex = 3;
            this.NavigationToolStrip.Text = "toolStrip1";
            // 
            // BackToolbarButton
            // 
            this.BackToolbarButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BackToolbarButton.Enabled = false;
            this.BackToolbarButton.Image = global::LDDModder.LifExtractor.Properties.Resources.Arrow_Left_16x16_Black;
            this.BackToolbarButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BackToolbarButton.Name = "BackToolbarButton";
            this.BackToolbarButton.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.BackToolbarButton.Size = new System.Drawing.Size(23, 22);
            this.BackToolbarButton.Text = "Back";
            this.BackToolbarButton.Click += new System.EventHandler(this.BackToolbarButton_Click);
            // 
            // NextToolbarButton
            // 
            this.NextToolbarButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.NextToolbarButton.Enabled = false;
            this.NextToolbarButton.Image = global::LDDModder.LifExtractor.Properties.Resources.Arrow_Right_16x16_Black;
            this.NextToolbarButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.NextToolbarButton.Name = "NextToolbarButton";
            this.NextToolbarButton.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.NextToolbarButton.Size = new System.Drawing.Size(23, 22);
            this.NextToolbarButton.Text = "Forward";
            this.NextToolbarButton.Click += new System.EventHandler(this.NextToolbarButton_Click);
            // 
            // UpToolbarButton
            // 
            this.UpToolbarButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.UpToolbarButton.Enabled = false;
            this.UpToolbarButton.Image = global::LDDModder.LifExtractor.Properties.Resources.Arrow_Up_16x16_Black;
            this.UpToolbarButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.UpToolbarButton.Name = "UpToolbarButton";
            this.UpToolbarButton.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.UpToolbarButton.Size = new System.Drawing.Size(23, 22);
            this.UpToolbarButton.Text = "Up";
            this.UpToolbarButton.Click += new System.EventHandler(this.UpToolbarButton_Click);
            // 
            // ToolBarFolderCombo
            // 
            this.ToolBarFolderCombo.AutoSize = false;
            this.ToolBarFolderCombo.MaxDropDownItems = 10;
            this.ToolBarFolderCombo.Name = "ToolBarFolderCombo";
            this.ToolBarFolderCombo.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.ToolBarFolderCombo.Size = new System.Drawing.Size(150, 23);
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.BottomToolStripPanel
            // 
            this.toolStripContainer1.BottomToolStripPanel.Controls.Add(this.StatusToolStrip);
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.splitContainer1);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(784, 436);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(784, 561);
            this.toolStripContainer1.TabIndex = 4;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.MainMenuToolStrip);
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.ActionsMenuToolStrip);
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.NavigationToolStrip);
            // 
            // ActionsMenuToolStrip
            // 
            this.ActionsMenuToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.ActionsMenuToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ActionsMenuToolStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.ActionsMenuToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ActionsMenu_Extract,
            this.toolStripSeparator3,
            this.ActionsMenu_EnableEdit,
            this.ActionsMenu_AddFile});
            this.ActionsMenuToolStrip.Location = new System.Drawing.Point(0, 24);
            this.ActionsMenuToolStrip.Name = "ActionsMenuToolStrip";
            this.ActionsMenuToolStrip.Size = new System.Drawing.Size(784, 54);
            this.ActionsMenuToolStrip.Stretch = true;
            this.ActionsMenuToolStrip.TabIndex = 4;
            // 
            // ActionsMenu_Extract
            // 
            this.ActionsMenu_Extract.Enabled = false;
            this.ActionsMenu_Extract.Image = global::LDDModder.LifExtractor.Properties.Resources.Folder_32x32;
            this.ActionsMenu_Extract.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ActionsMenu_Extract.Name = "ActionsMenu_Extract";
            this.ActionsMenu_Extract.Size = new System.Drawing.Size(46, 51);
            this.ActionsMenu_Extract.Text = "Extract";
            this.ActionsMenu_Extract.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.ActionsMenu_Extract.Click += new System.EventHandler(this.ActionsMenu_Extract_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 54);
            // 
            // ActionsMenu_EnableEdit
            // 
            this.ActionsMenu_EnableEdit.CheckOnClick = true;
            this.ActionsMenu_EnableEdit.Enabled = false;
            this.ActionsMenu_EnableEdit.Image = ((System.Drawing.Image)(resources.GetObject("ActionsMenu_EnableEdit.Image")));
            this.ActionsMenu_EnableEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ActionsMenu_EnableEdit.Name = "ActionsMenu_EnableEdit";
            this.ActionsMenu_EnableEdit.Size = new System.Drawing.Size(47, 51);
            this.ActionsMenu_EnableEdit.Text = "Edit Lif";
            this.ActionsMenu_EnableEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.ActionsMenu_EnableEdit.CheckedChanged += new System.EventHandler(this.ActionsMenu_EnableEdit_CheckedChanged);
            // 
            // ActionsMenu_AddFile
            // 
            this.ActionsMenu_AddFile.Enabled = false;
            this.ActionsMenu_AddFile.Image = ((System.Drawing.Image)(resources.GetObject("ActionsMenu_AddFile.Image")));
            this.ActionsMenu_AddFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ActionsMenu_AddFile.Name = "ActionsMenu_AddFile";
            this.ActionsMenu_AddFile.Size = new System.Drawing.Size(63, 51);
            this.ActionsMenu_AddFile.Text = "Add File…";
            this.ActionsMenu_AddFile.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // LifViewerWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.toolStripContainer1);
            this.Name = "LifViewerWindow";
            this.Text = "Lif Viewer";
            this.StatusToolStrip.ResumeLayout(false);
            this.StatusToolStrip.PerformLayout();
            this.MainMenuToolStrip.ResumeLayout(false);
            this.MainMenuToolStrip.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.FolderListView)).EndInit();
            this.FolderListContextMenu.ResumeLayout(false);
            this.NavigationToolStrip.ResumeLayout(false);
            this.NavigationToolStrip.PerformLayout();
            this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.ActionsMenuToolStrip.ResumeLayout(false);
            this.ActionsMenuToolStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.StatusStrip StatusToolStrip;
        private System.Windows.Forms.MenuStrip MainMenuToolStrip;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStripStatusLabel CurrentFileStripLabel;
        private System.Windows.Forms.ToolStrip NavigationToolStrip;
        private System.Windows.Forms.ToolStripButton BackToolbarButton;
        private System.Windows.Forms.ToolStripButton UpToolbarButton;
        private System.Windows.Forms.ToolStripComboBox ToolBarFolderCombo;
        private System.Windows.Forms.ImageList SmallIconImageList;
        private System.Windows.Forms.ToolStripMenuItem MainMenu_FileMenu;
        private System.Windows.Forms.ToolStripButton NextToolbarButton;
        private System.Windows.Forms.ImageList LargeIconImageList;
        private BrightIdeasSoftware.DataListView FolderListView;
        private BrightIdeasSoftware.OLVColumn FlvSizeColumn;
        private BrightIdeasSoftware.OLVColumn FlvNameColumn;
        private BrightIdeasSoftware.OLVColumn FlvTypeColumn;
        private BrightIdeasSoftware.OLVColumn FlvCreatedColumn;
        private BrightIdeasSoftware.OLVColumn FlvModifiedColumn;
        private System.Windows.Forms.ToolStripMenuItem MainMenu_ViewMenu;
        private System.Windows.Forms.ToolStripMenuItem ViewModeDetailsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ViewModeSmallMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ViewModeLargeMenuItem;
        private System.Windows.Forms.ToolStripMenuItem FileMenu_OpenItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem FileMenu_ExtractItem;
        private System.Windows.Forms.ContextMenuStrip FolderListContextMenu;
        private System.Windows.Forms.ToolStripMenuItem ListMenu_ExtractItem;
        private System.Windows.Forms.ToolStripMenuItem newLIFToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem ListMenu_CreateFolderItem;
        private System.Windows.Forms.ToolStripMenuItem ListMenu_AddFileItem;
        private System.Windows.Forms.ToolStripMenuItem ListMenu_RenameItem;
        private System.Windows.Forms.ToolStripMenuItem ListMenu_DeleteItem;
        private System.Windows.Forms.TreeView LifTreeView;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ToolStrip ActionsMenuToolStrip;
        private System.Windows.Forms.ToolStripButton ActionsMenu_Extract;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton ActionsMenu_AddFile;
        private System.Windows.Forms.ToolStripButton ActionsMenu_EnableEdit;
    }
}