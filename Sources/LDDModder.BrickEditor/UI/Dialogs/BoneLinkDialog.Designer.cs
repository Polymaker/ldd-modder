namespace LDDModder.BrickEditor.UI.Windows
{
    partial class BoneLinkDialog
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.HierarchyTreeView = new BrightIdeasSoftware.TreeListView();
            this.olvColumn1 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.UnassignedTreeView = new BrightIdeasSoftware.TreeListView();
            this.olvColumn2 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.ApplyButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.HierarchyTreeView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UnassignedTreeView)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tableLayoutPanel1.Controls.Add(this.HierarchyTreeView, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.UnassignedTreeView, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.ApplyButton, 1, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.67742F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 90.32258F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(446, 340);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // HierarchyTreeView
            // 
            this.HierarchyTreeView.AllColumns.Add(this.olvColumn1);
            this.HierarchyTreeView.CellEditUseWholeCell = false;
            this.HierarchyTreeView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumn1});
            this.HierarchyTreeView.Cursor = System.Windows.Forms.Cursors.Default;
            this.HierarchyTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HierarchyTreeView.HideSelection = false;
            this.HierarchyTreeView.IsSimpleDragSource = true;
            this.HierarchyTreeView.Location = new System.Drawing.Point(3, 33);
            this.HierarchyTreeView.Name = "HierarchyTreeView";
            this.HierarchyTreeView.ShowGroups = false;
            this.HierarchyTreeView.Size = new System.Drawing.Size(283, 274);
            this.HierarchyTreeView.TabIndex = 0;
            this.HierarchyTreeView.UseCompatibleStateImageBehavior = false;
            this.HierarchyTreeView.View = System.Windows.Forms.View.Details;
            this.HierarchyTreeView.VirtualMode = true;
            this.HierarchyTreeView.Expanded += new System.EventHandler<BrightIdeasSoftware.TreeBranchExpandedEventArgs>(this.HierarchyTreeView_Expanded);
            this.HierarchyTreeView.Collapsed += new System.EventHandler<BrightIdeasSoftware.TreeBranchCollapsedEventArgs>(this.HierarchyTreeView_Collapsed);
            this.HierarchyTreeView.ModelCanDrop += new System.EventHandler<BrightIdeasSoftware.ModelDropEventArgs>(this.BonesTreeViews_ModelCanDrop);
            this.HierarchyTreeView.ModelDropped += new System.EventHandler<BrightIdeasSoftware.ModelDropEventArgs>(this.BonesTreeViews_ModelDropped);
            // 
            // olvColumn1
            // 
            this.olvColumn1.AspectName = "Name";
            this.olvColumn1.Text = "Bone Hierarchy";
            this.olvColumn1.Width = 120;
            // 
            // UnassignedTreeView
            // 
            this.UnassignedTreeView.AllColumns.Add(this.olvColumn2);
            this.UnassignedTreeView.CellEditUseWholeCell = false;
            this.UnassignedTreeView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumn2});
            this.UnassignedTreeView.Cursor = System.Windows.Forms.Cursors.Default;
            this.UnassignedTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UnassignedTreeView.HideSelection = false;
            this.UnassignedTreeView.IsSimpleDragSource = true;
            this.UnassignedTreeView.Location = new System.Drawing.Point(292, 33);
            this.UnassignedTreeView.Name = "UnassignedTreeView";
            this.UnassignedTreeView.ShowGroups = false;
            this.UnassignedTreeView.Size = new System.Drawing.Size(151, 274);
            this.UnassignedTreeView.TabIndex = 1;
            this.UnassignedTreeView.UseCompatibleStateImageBehavior = false;
            this.UnassignedTreeView.View = System.Windows.Forms.View.Details;
            this.UnassignedTreeView.VirtualMode = true;
            this.UnassignedTreeView.ModelCanDrop += new System.EventHandler<BrightIdeasSoftware.ModelDropEventArgs>(this.BonesTreeViews_ModelCanDrop);
            this.UnassignedTreeView.ModelDropped += new System.EventHandler<BrightIdeasSoftware.ModelDropEventArgs>(this.BonesTreeViews_ModelDropped);
            // 
            // olvColumn2
            // 
            this.olvColumn2.AspectName = "Name";
            this.olvColumn2.FillsFreeSpace = true;
            this.olvColumn2.Text = "Unassigned Bones";
            // 
            // ApplyButton
            // 
            this.ApplyButton.Location = new System.Drawing.Point(292, 313);
            this.ApplyButton.Name = "ApplyButton";
            this.ApplyButton.Size = new System.Drawing.Size(75, 23);
            this.ApplyButton.TabIndex = 2;
            this.ApplyButton.Text = "Apply";
            this.ApplyButton.UseVisualStyleBackColor = true;
            this.ApplyButton.Click += new System.EventHandler(this.ApplyButton_Click);
            // 
            // BoneLinkDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(446, 340);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "BoneLinkDialog";
            this.Text = "BoneLinkDialog";
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.HierarchyTreeView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UnassignedTreeView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private BrightIdeasSoftware.TreeListView HierarchyTreeView;
        private BrightIdeasSoftware.TreeListView UnassignedTreeView;
        private BrightIdeasSoftware.OLVColumn olvColumn1;
        private BrightIdeasSoftware.OLVColumn olvColumn2;
        private System.Windows.Forms.Button ApplyButton;
    }
}