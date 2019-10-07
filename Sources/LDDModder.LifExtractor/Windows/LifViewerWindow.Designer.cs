namespace LDDModder.LifExtractor
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
            this.StatusToolStrip = new System.Windows.Forms.StatusStrip();
            this.CurrentFileStripLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.MainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.FileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.FileMenu_OpenItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.FileMenu_ExtractItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewModeDetailsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewModeSmallMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewModeLargeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
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
            this.ListMenu_OpenItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ListMenu_ExtractItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LargeIconImageList = new System.Windows.Forms.ImageList(this.components);
            this.NavigationToolStrip = new System.Windows.Forms.ToolStrip();
            this.BackToolbarButton = new System.Windows.Forms.ToolStripButton();
            this.NextToolbarButton = new System.Windows.Forms.ToolStripButton();
            this.UpToolbarButton = new System.Windows.Forms.ToolStripButton();
            this.ToolBarFolderCombo = new System.Windows.Forms.ToolStripComboBox();
            this.StatusToolStrip.SuspendLayout();
            this.MainMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FolderListView)).BeginInit();
            this.FolderListContextMenu.SuspendLayout();
            this.NavigationToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // StatusToolStrip
            // 
            this.StatusToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CurrentFileStripLabel});
            this.StatusToolStrip.Location = new System.Drawing.Point(0, 539);
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
            // MainMenuStrip
            // 
            this.MainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileMenu,
            this.ViewMenu});
            this.MainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MainMenuStrip.Name = "MainMenuStrip";
            this.MainMenuStrip.Size = new System.Drawing.Size(784, 24);
            this.MainMenuStrip.TabIndex = 1;
            this.MainMenuStrip.Text = "menuStrip1";
            // 
            // FileMenu
            // 
            this.FileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileMenu_OpenItem,
            this.toolStripSeparator2,
            this.FileMenu_ExtractItem});
            this.FileMenu.Name = "FileMenu";
            this.FileMenu.Size = new System.Drawing.Size(37, 20);
            this.FileMenu.Text = "File";
            // 
            // FileMenu_OpenItem
            // 
            this.FileMenu_OpenItem.Name = "FileMenu_OpenItem";
            this.FileMenu_OpenItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.FileMenu_OpenItem.Size = new System.Drawing.Size(146, 22);
            this.FileMenu_OpenItem.Text = "Open";
            this.FileMenu_OpenItem.Click += new System.EventHandler(this.FileOpenMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(143, 6);
            // 
            // FileMenu_ExtractItem
            // 
            this.FileMenu_ExtractItem.Enabled = false;
            this.FileMenu_ExtractItem.Name = "FileMenu_ExtractItem";
            this.FileMenu_ExtractItem.Size = new System.Drawing.Size(146, 22);
            this.FileMenu_ExtractItem.Text = "Extract to...";
            // 
            // ViewMenu
            // 
            this.ViewMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ViewModeDetailsMenuItem,
            this.ViewModeSmallMenuItem,
            this.ViewModeLargeMenuItem,
            this.toolStripSeparator1});
            this.ViewMenu.Name = "ViewMenu";
            this.ViewMenu.Size = new System.Drawing.Size(44, 20);
            this.ViewMenu.Text = "View";
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
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(131, 6);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 49);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.LifTreeView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.FolderListView);
            this.splitContainer1.Size = new System.Drawing.Size(784, 490);
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
            this.LifTreeView.Size = new System.Drawing.Size(200, 490);
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
            this.FolderListView.AutoGenerateColumns = false;
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
            this.FolderListView.Size = new System.Drawing.Size(580, 490);
            this.FolderListView.SmallImageList = this.SmallIconImageList;
            this.FolderListView.TabIndex = 1;
            this.FolderListView.UseCompatibleStateImageBehavior = false;
            this.FolderListView.UseExplorerTheme = true;
            this.FolderListView.View = System.Windows.Forms.View.Details;
            this.FolderListView.BeforeSorting += new System.EventHandler<BrightIdeasSoftware.BeforeSortingEventArgs>(this.FolderListView_BeforeSorting);
            this.FolderListView.ItemActivate += new System.EventHandler(this.FolderListView_ItemActivate);
            this.FolderListView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.FolderListView_ItemDrag);
            // 
            // FlvNameColumn
            // 
            this.FlvNameColumn.AspectName = "Name";
            this.FlvNameColumn.ImageAspectName = "ItemImageKey";
            this.FlvNameColumn.Text = "Name";
            this.FlvNameColumn.Width = 150;
            // 
            // FlvTypeColumn
            // 
            this.FlvTypeColumn.AspectName = "Description";
            this.FlvTypeColumn.Text = "Type";
            this.FlvTypeColumn.Width = 130;
            // 
            // FlvSizeColumn
            // 
            this.FlvSizeColumn.AspectName = "Size";
            this.FlvSizeColumn.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.FlvSizeColumn.Text = "Size";
            this.FlvSizeColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.FlvSizeColumn.Width = 85;
            // 
            // FlvCreatedColumn
            // 
            this.FlvCreatedColumn.AspectName = "CreatedDate";
            this.FlvCreatedColumn.Text = "Date created";
            this.FlvCreatedColumn.Width = 130;
            // 
            // FlvModifiedColumn
            // 
            this.FlvModifiedColumn.AspectName = "ModifiedDate";
            this.FlvModifiedColumn.Text = "Date modified";
            this.FlvModifiedColumn.Width = 130;
            // 
            // FolderListContextMenu
            // 
            this.FolderListContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ListMenu_OpenItem,
            this.ListMenu_ExtractItem});
            this.FolderListContextMenu.Name = "FolderListContextMenu";
            this.FolderListContextMenu.Size = new System.Drawing.Size(110, 48);
            this.FolderListContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.FolderListContextMenu_Opening);
            // 
            // ListMenu_OpenItem
            // 
            this.ListMenu_OpenItem.Name = "ListMenu_OpenItem";
            this.ListMenu_OpenItem.Size = new System.Drawing.Size(109, 22);
            this.ListMenu_OpenItem.Text = "Open";
            this.ListMenu_OpenItem.Click += new System.EventHandler(this.ListMenu_OpenItem_Click);
            // 
            // ListMenu_ExtractItem
            // 
            this.ListMenu_ExtractItem.Name = "ListMenu_ExtractItem";
            this.ListMenu_ExtractItem.Size = new System.Drawing.Size(109, 22);
            this.ListMenu_ExtractItem.Text = "Extract";
            // 
            // LargeIconImageList
            // 
            this.LargeIconImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.LargeIconImageList.ImageSize = new System.Drawing.Size(32, 32);
            this.LargeIconImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // NavigationToolStrip
            // 
            this.NavigationToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.NavigationToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.BackToolbarButton,
            this.NextToolbarButton,
            this.UpToolbarButton,
            this.ToolBarFolderCombo});
            this.NavigationToolStrip.Location = new System.Drawing.Point(0, 24);
            this.NavigationToolStrip.Name = "NavigationToolStrip";
            this.NavigationToolStrip.Size = new System.Drawing.Size(784, 25);
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
            this.ToolBarFolderCombo.Name = "ToolBarFolderCombo";
            this.ToolBarFolderCombo.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.ToolBarFolderCombo.Size = new System.Drawing.Size(150, 23);
            this.ToolBarFolderCombo.Text = "DB";
            // 
            // LifViewerWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.StatusToolStrip);
            this.Controls.Add(this.NavigationToolStrip);
            this.Controls.Add(this.MainMenuStrip);
            this.Name = "LifViewerWindow";
            this.Text = "Lif Viewer";
            this.StatusToolStrip.ResumeLayout(false);
            this.StatusToolStrip.PerformLayout();
            this.MainMenuStrip.ResumeLayout(false);
            this.MainMenuStrip.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.FolderListView)).EndInit();
            this.FolderListContextMenu.ResumeLayout(false);
            this.NavigationToolStrip.ResumeLayout(false);
            this.NavigationToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip StatusToolStrip;
        private System.Windows.Forms.MenuStrip MainMenuStrip;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView LifTreeView;
        private System.Windows.Forms.ToolStripStatusLabel CurrentFileStripLabel;
        private System.Windows.Forms.ToolStrip NavigationToolStrip;
        private System.Windows.Forms.ToolStripButton BackToolbarButton;
        private System.Windows.Forms.ToolStripButton UpToolbarButton;
        private System.Windows.Forms.ToolStripComboBox ToolBarFolderCombo;
        private System.Windows.Forms.ImageList SmallIconImageList;
        private System.Windows.Forms.ToolStripMenuItem FileMenu;
        private System.Windows.Forms.ToolStripButton NextToolbarButton;
        private System.Windows.Forms.ImageList LargeIconImageList;
        private BrightIdeasSoftware.DataListView FolderListView;
        private BrightIdeasSoftware.OLVColumn FlvSizeColumn;
        private BrightIdeasSoftware.OLVColumn FlvNameColumn;
        private BrightIdeasSoftware.OLVColumn FlvTypeColumn;
        private BrightIdeasSoftware.OLVColumn FlvCreatedColumn;
        private BrightIdeasSoftware.OLVColumn FlvModifiedColumn;
        private System.Windows.Forms.ToolStripMenuItem ViewMenu;
        private System.Windows.Forms.ToolStripMenuItem ViewModeDetailsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ViewModeSmallMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ViewModeLargeMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem FileMenu_OpenItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem FileMenu_ExtractItem;
        private System.Windows.Forms.ContextMenuStrip FolderListContextMenu;
        private System.Windows.Forms.ToolStripMenuItem ListMenu_OpenItem;
        private System.Windows.Forms.ToolStripMenuItem ListMenu_ExtractItem;
    }
}