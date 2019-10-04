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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Assemblies");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Decorations");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("MainGroupDividers");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("MaterialNames");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("LOD0");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Primitives", new System.Windows.Forms.TreeNode[] {
            treeNode5});
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("DB", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3,
            treeNode4,
            treeNode6});
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "CurrentMaterials.xml",
            "2 Kb"}, -1);
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem(new string[] {
            "DecorationMapping.xml",
            "132 Kb"}, -1);
            this.StatusToolStrip = new System.Windows.Forms.StatusStrip();
            this.CurrentFileStripLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.LifTreeView = new System.Windows.Forms.TreeView();
            this.FolderListView = new System.Windows.Forms.ListView();
            this.NameColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SizeColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ModifiedColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.CreatedColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.AccessedColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.NavigationToolStrip = new System.Windows.Forms.ToolStrip();
            this.ToolBarFolderCombo = new System.Windows.Forms.ToolStripComboBox();
            this.IconImageList = new System.Windows.Forms.ImageList(this.components);
            this.BackToolbarButton = new System.Windows.Forms.ToolStripButton();
            this.NextToolbarButton = new System.Windows.Forms.ToolStripSplitButton();
            this.UpToolbarButton = new System.Windows.Forms.ToolStripButton();
            this.StatusToolStrip.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.NavigationToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // StatusToolStrip
            // 
            this.StatusToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CurrentFileStripLabel});
            this.StatusToolStrip.Location = new System.Drawing.Point(0, 334);
            this.StatusToolStrip.Name = "StatusToolStrip";
            this.StatusToolStrip.Size = new System.Drawing.Size(607, 22);
            this.StatusToolStrip.TabIndex = 0;
            this.StatusToolStrip.Text = "statusStrip1";
            // 
            // CurrentFileStripLabel
            // 
            this.CurrentFileStripLabel.Name = "CurrentFileStripLabel";
            this.CurrentFileStripLabel.Size = new System.Drawing.Size(168, 17);
            this.CurrentFileStripLabel.Text = "...\\LEGO Digital Designer\\db.lif";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(607, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(48, 20);
            this.toolStripMenuItem1.Text = "Open";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.ToolStripMenuItem1_Click);
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
            this.splitContainer1.Size = new System.Drawing.Size(607, 285);
            this.splitContainer1.SplitterDistance = 202;
            this.splitContainer1.TabIndex = 2;
            // 
            // LifTreeView
            // 
            this.LifTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LifTreeView.FullRowSelect = true;
            this.LifTreeView.ImageIndex = 0;
            this.LifTreeView.ImageList = this.IconImageList;
            this.LifTreeView.ItemHeight = 20;
            this.LifTreeView.Location = new System.Drawing.Point(0, 0);
            this.LifTreeView.Name = "LifTreeView";
            treeNode1.Name = "Nœud1";
            treeNode1.Text = "Assemblies";
            treeNode2.Name = "Nœud2";
            treeNode2.Text = "Decorations";
            treeNode3.Name = "Nœud3";
            treeNode3.Text = "MainGroupDividers";
            treeNode4.Name = "Nœud4";
            treeNode4.Text = "MaterialNames";
            treeNode5.Name = "Nœud6";
            treeNode5.Text = "LOD0";
            treeNode6.Name = "Nœud5";
            treeNode6.Text = "Primitives";
            treeNode7.Name = "Nœud0";
            treeNode7.Text = "DB";
            this.LifTreeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode7});
            this.LifTreeView.SelectedImageIndex = 0;
            this.LifTreeView.Size = new System.Drawing.Size(202, 285);
            this.LifTreeView.TabIndex = 0;
            this.LifTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.LifTreeView_AfterSelect);
            // 
            // FolderListView
            // 
            this.FolderListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.NameColumn,
            this.SizeColumn,
            this.ModifiedColumn,
            this.CreatedColumn,
            this.AccessedColumn});
            this.FolderListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FolderListView.HideSelection = false;
            this.FolderListView.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2});
            this.FolderListView.Location = new System.Drawing.Point(0, 0);
            this.FolderListView.Name = "FolderListView";
            this.FolderListView.Size = new System.Drawing.Size(401, 285);
            this.FolderListView.TabIndex = 0;
            this.FolderListView.UseCompatibleStateImageBehavior = false;
            this.FolderListView.View = System.Windows.Forms.View.Details;
            // 
            // NameColumn
            // 
            this.NameColumn.Text = "Name";
            this.NameColumn.Width = 150;
            // 
            // SizeColumn
            // 
            this.SizeColumn.Text = "Size";
            // 
            // ModifiedColumn
            // 
            this.ModifiedColumn.Text = "Modified";
            // 
            // CreatedColumn
            // 
            this.CreatedColumn.Text = "Created";
            // 
            // AccessedColumn
            // 
            this.AccessedColumn.Text = "Accessed";
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
            this.NavigationToolStrip.Size = new System.Drawing.Size(607, 25);
            this.NavigationToolStrip.TabIndex = 3;
            this.NavigationToolStrip.Text = "toolStrip1";
            // 
            // ToolBarFolderCombo
            // 
            this.ToolBarFolderCombo.AutoSize = false;
            this.ToolBarFolderCombo.Name = "ToolBarFolderCombo";
            this.ToolBarFolderCombo.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.ToolBarFolderCombo.Size = new System.Drawing.Size(150, 23);
            this.ToolBarFolderCombo.Text = "DB";
            // 
            // IconImageList
            // 
            this.IconImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.IconImageList.ImageSize = new System.Drawing.Size(16, 16);
            this.IconImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // BackToolbarButton
            // 
            this.BackToolbarButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BackToolbarButton.Image = global::LDDModder.LifExtractor.Properties.Resources.Arrow_Left_16x16_Black;
            this.BackToolbarButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BackToolbarButton.Name = "BackToolbarButton";
            this.BackToolbarButton.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.BackToolbarButton.Size = new System.Drawing.Size(23, 22);
            this.BackToolbarButton.Text = "toolStripButton1";
            // 
            // NextToolbarButton
            // 
            this.NextToolbarButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.NextToolbarButton.Image = global::LDDModder.LifExtractor.Properties.Resources.Arrow_Right_16x16_Black;
            this.NextToolbarButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.NextToolbarButton.Name = "NextToolbarButton";
            this.NextToolbarButton.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.NextToolbarButton.Size = new System.Drawing.Size(32, 22);
            this.NextToolbarButton.Text = "toolStripSplitButton1";
            // 
            // UpToolbarButton
            // 
            this.UpToolbarButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.UpToolbarButton.Image = global::LDDModder.LifExtractor.Properties.Resources.Arrow_Up_16x16_Black;
            this.UpToolbarButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.UpToolbarButton.Name = "UpToolbarButton";
            this.UpToolbarButton.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.UpToolbarButton.Size = new System.Drawing.Size(23, 22);
            this.UpToolbarButton.Text = "toolStripButton2";
            // 
            // LifViewerWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(607, 356);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.StatusToolStrip);
            this.Controls.Add(this.NavigationToolStrip);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "LifViewerWindow";
            this.Text = "Lif Viewer";
            this.StatusToolStrip.ResumeLayout(false);
            this.StatusToolStrip.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.NavigationToolStrip.ResumeLayout(false);
            this.NavigationToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip StatusToolStrip;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView LifTreeView;
        private System.Windows.Forms.ListView FolderListView;
        private System.Windows.Forms.ColumnHeader NameColumn;
        private System.Windows.Forms.ColumnHeader SizeColumn;
        private System.Windows.Forms.ColumnHeader ModifiedColumn;
        private System.Windows.Forms.ColumnHeader CreatedColumn;
        private System.Windows.Forms.ColumnHeader AccessedColumn;
        private System.Windows.Forms.ToolStripStatusLabel CurrentFileStripLabel;
        private System.Windows.Forms.ToolStrip NavigationToolStrip;
        private System.Windows.Forms.ToolStripButton BackToolbarButton;
        private System.Windows.Forms.ToolStripSplitButton NextToolbarButton;
        private System.Windows.Forms.ToolStripButton UpToolbarButton;
        private System.Windows.Forms.ToolStripComboBox ToolBarFolderCombo;
        private System.Windows.Forms.ImageList IconImageList;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
    }
}