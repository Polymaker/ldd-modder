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
            this.treeListView1 = new BrightIdeasSoftware.TreeListView();
            this.olvColumn1 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addElementToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.surfaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.studToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.ViewModeComboBox = new System.Windows.Forms.ComboBox();
            this.LocalizedStrings = new LDDModder.BrickEditor.Localization.LocalizableStringList(this.components);
            this.ViewModeSurfaces = new LDDModder.BrickEditor.Localization.LocalizableString(this.components);
            this.ViewModeCollisions = new LDDModder.BrickEditor.Localization.LocalizableString(this.components);
            this.ViewModeConnections = new LDDModder.BrickEditor.Localization.LocalizableString(this.components);
            this.ViewModeBones = new LDDModder.BrickEditor.Localization.LocalizableString(this.components);
            this.ViewModeAll = new LDDModder.BrickEditor.Localization.LocalizableString(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.treeListView1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeListView1
            // 
            this.treeListView1.AllColumns.Add(this.olvColumn1);
            this.treeListView1.CellEditUseWholeCell = false;
            this.treeListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumn1});
            this.treeListView1.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.treeListView1, "treeListView1");
            this.treeListView1.HideSelection = false;
            this.treeListView1.Name = "treeListView1";
            this.treeListView1.ShowGroups = false;
            this.treeListView1.UseCompatibleStateImageBehavior = false;
            this.treeListView1.View = System.Windows.Forms.View.Details;
            this.treeListView1.VirtualMode = true;
            // 
            // olvColumn1
            // 
            this.olvColumn1.AspectName = "Text";
            this.olvColumn1.FillsFreeSpace = true;
            this.olvColumn1.IsEditable = false;
            this.olvColumn1.Sortable = false;
            resources.ApplyResources(this.olvColumn1, "olvColumn1");
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addElementToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            resources.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
            // 
            // addElementToolStripMenuItem
            // 
            this.addElementToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.surfaceToolStripMenuItem,
            this.studToolStripMenuItem});
            this.addElementToolStripMenuItem.Name = "addElementToolStripMenuItem";
            resources.ApplyResources(this.addElementToolStripMenuItem, "addElementToolStripMenuItem");
            // 
            // surfaceToolStripMenuItem
            // 
            this.surfaceToolStripMenuItem.Name = "surfaceToolStripMenuItem";
            resources.ApplyResources(this.surfaceToolStripMenuItem, "surfaceToolStripMenuItem");
            // 
            // studToolStripMenuItem
            // 
            this.studToolStripMenuItem.Name = "studToolStripMenuItem";
            resources.ApplyResources(this.studToolStripMenuItem, "studToolStripMenuItem");
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.treeListView1, 0, 2);
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
            ((System.ComponentModel.ISupportInitialize)(this.treeListView1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private BrightIdeasSoftware.TreeListView treeListView1;
        private BrightIdeasSoftware.OLVColumn olvColumn1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem addElementToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem surfaceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem studToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox ViewModeComboBox;
        private Localization.LocalizableStringList LocalizedStrings;
        private Localization.LocalizableString ViewModeSurfaces;
        private Localization.LocalizableString ViewModeCollisions;
        private Localization.LocalizableString ViewModeConnections;
        private Localization.LocalizableString ViewModeBones;
        private Localization.LocalizableString ViewModeAll;
    }
}