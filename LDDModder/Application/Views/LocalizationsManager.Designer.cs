namespace LDDModder.Views
{
    partial class LocalizationsManager
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitPanelLayout = new System.Windows.Forms.SplitContainer();
            this.lvwLanguages = new BrightIdeasSoftware.ObjectListView();
            this.lvColLangName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lcColLangKey = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvColLangActive = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.splitPanelLocalizations = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tabCtrlFiles = new System.Windows.Forms.TabControl();
            this.tabPageApp = new System.Windows.Forms.TabPage();
            this.lvwLocalizations = new BrightIdeasSoftware.ObjectListView();
            this.lvColLocKey = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvColLocValue = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.tabPageMaterials = new System.Windows.Forms.TabPage();
            this.chkActive = new System.Windows.Forms.CheckBox();
            this.txtLocValue = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitPanelLayout)).BeginInit();
            this.splitPanelLayout.Panel1.SuspendLayout();
            this.splitPanelLayout.Panel2.SuspendLayout();
            this.splitPanelLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lvwLanguages)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitPanelLocalizations)).BeginInit();
            this.splitPanelLocalizations.Panel1.SuspendLayout();
            this.splitPanelLocalizations.Panel2.SuspendLayout();
            this.splitPanelLocalizations.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tabCtrlFiles.SuspendLayout();
            this.tabPageApp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lvwLocalizations)).BeginInit();
            this.SuspendLayout();
            // 
            // splitPanelLayout
            // 
            this.splitPanelLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitPanelLayout.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitPanelLayout.Location = new System.Drawing.Point(2, 2);
            this.splitPanelLayout.Name = "splitPanelLayout";
            // 
            // splitPanelLayout.Panel1
            // 
            this.splitPanelLayout.Panel1.Controls.Add(this.lvwLanguages);
            // 
            // splitPanelLayout.Panel2
            // 
            this.splitPanelLayout.Panel2.Controls.Add(this.splitPanelLocalizations);
            this.splitPanelLayout.Size = new System.Drawing.Size(581, 378);
            this.splitPanelLayout.SplitterDistance = 251;
            this.splitPanelLayout.TabIndex = 0;
            // 
            // lvwLanguages
            // 
            this.lvwLanguages.AllColumns.Add(this.lvColLangName);
            this.lvwLanguages.AllColumns.Add(this.lcColLangKey);
            this.lvwLanguages.AllColumns.Add(this.lvColLangActive);
            this.lvwLanguages.CellEditUseWholeCell = false;
            this.lvwLanguages.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.lvColLangName,
            this.lcColLangKey,
            this.lvColLangActive});
            this.lvwLanguages.Cursor = System.Windows.Forms.Cursors.Default;
            this.lvwLanguages.FullRowSelect = true;
            this.lvwLanguages.HideSelection = false;
            this.lvwLanguages.IsSearchOnSortColumn = false;
            this.lvwLanguages.Location = new System.Drawing.Point(0, 0);
            this.lvwLanguages.Name = "lvwLanguages";
            this.lvwLanguages.ShowGroups = false;
            this.lvwLanguages.Size = new System.Drawing.Size(251, 378);
            this.lvwLanguages.TabIndex = 0;
            this.lvwLanguages.UseCompatibleStateImageBehavior = false;
            this.lvwLanguages.View = System.Windows.Forms.View.Details;
            this.lvwLanguages.SelectedIndexChanged += new System.EventHandler(this.lvwLanguages_SelectedIndexChanged);
            // 
            // lvColLangName
            // 
            this.lvColLangName.AspectName = "Name";
            this.lvColLangName.FillsFreeSpace = true;
            this.lvColLangName.IsEditable = false;
            this.lvColLangName.Text = "Language";
            this.lvColLangName.Width = 117;
            // 
            // lcColLangKey
            // 
            this.lcColLangKey.AspectName = "Key";
            this.lcColLangKey.IsEditable = false;
            this.lcColLangKey.Text = "Key";
            this.lcColLangKey.Width = 73;
            // 
            // lvColLangActive
            // 
            this.lvColLangActive.AspectName = "IsActive";
            this.lvColLangActive.IsEditable = false;
            this.lvColLangActive.Text = "Active";
            // 
            // splitPanelLocalizations
            // 
            this.splitPanelLocalizations.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitPanelLocalizations.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitPanelLocalizations.Location = new System.Drawing.Point(0, 0);
            this.splitPanelLocalizations.Name = "splitPanelLocalizations";
            this.splitPanelLocalizations.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitPanelLocalizations.Panel1
            // 
            this.splitPanelLocalizations.Panel1.Controls.Add(this.tableLayoutPanel1);
            // 
            // splitPanelLocalizations.Panel2
            // 
            this.splitPanelLocalizations.Panel2.Controls.Add(this.txtLocValue);
            this.splitPanelLocalizations.Size = new System.Drawing.Size(326, 378);
            this.splitPanelLocalizations.SplitterDistance = 268;
            this.splitPanelLocalizations.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.tabCtrlFiles, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.chkActive, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(326, 268);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // tabCtrlFiles
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.tabCtrlFiles, 2);
            this.tabCtrlFiles.Controls.Add(this.tabPageApp);
            this.tabCtrlFiles.Controls.Add(this.tabPageMaterials);
            this.tabCtrlFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabCtrlFiles.Location = new System.Drawing.Point(1, 24);
            this.tabCtrlFiles.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.tabCtrlFiles.Name = "tabCtrlFiles";
            this.tabCtrlFiles.SelectedIndex = 0;
            this.tabCtrlFiles.Size = new System.Drawing.Size(325, 244);
            this.tabCtrlFiles.TabIndex = 1;
            // 
            // tabPageApp
            // 
            this.tabPageApp.Controls.Add(this.lvwLocalizations);
            this.tabPageApp.Location = new System.Drawing.Point(4, 22);
            this.tabPageApp.Name = "tabPageApp";
            this.tabPageApp.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageApp.Size = new System.Drawing.Size(317, 218);
            this.tabPageApp.TabIndex = 0;
            this.tabPageApp.Text = "Application strings";
            this.tabPageApp.UseVisualStyleBackColor = true;
            // 
            // lvwLocalizations
            // 
            this.lvwLocalizations.AllColumns.Add(this.lvColLocKey);
            this.lvwLocalizations.AllColumns.Add(this.lvColLocValue);
            this.lvwLocalizations.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.SingleClick;
            this.lvwLocalizations.CellEditUseWholeCell = false;
            this.lvwLocalizations.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.lvColLocKey,
            this.lvColLocValue});
            this.lvwLocalizations.Cursor = System.Windows.Forms.Cursors.Default;
            this.lvwLocalizations.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvwLocalizations.GridLines = true;
            this.lvwLocalizations.Location = new System.Drawing.Point(3, 3);
            this.lvwLocalizations.Name = "lvwLocalizations";
            this.lvwLocalizations.ShowGroups = false;
            this.lvwLocalizations.Size = new System.Drawing.Size(311, 212);
            this.lvwLocalizations.TabIndex = 0;
            this.lvwLocalizations.UseCompatibleStateImageBehavior = false;
            this.lvwLocalizations.View = System.Windows.Forms.View.Details;
            this.lvwLocalizations.CellEditStarting += new BrightIdeasSoftware.CellEditEventHandler(this.lvwLocalizations_CellEditStarting);
            // 
            // lvColLocKey
            // 
            this.lvColLocKey.AspectName = "Key";
            this.lvColLocKey.IsEditable = false;
            this.lvColLocKey.Text = "Key";
            this.lvColLocKey.Width = 68;
            // 
            // lvColLocValue
            // 
            this.lvColLocValue.AspectName = "Value";
            this.lvColLocValue.CellEditUseWholeCell = true;
            this.lvColLocValue.FillsFreeSpace = true;
            this.lvColLocValue.Text = "Value";
            // 
            // tabPageMaterials
            // 
            this.tabPageMaterials.Location = new System.Drawing.Point(4, 22);
            this.tabPageMaterials.Name = "tabPageMaterials";
            this.tabPageMaterials.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMaterials.Size = new System.Drawing.Size(317, 218);
            this.tabPageMaterials.TabIndex = 1;
            this.tabPageMaterials.Text = "Material names";
            this.tabPageMaterials.UseVisualStyleBackColor = true;
            // 
            // chkActive
            // 
            this.chkActive.AutoSize = true;
            this.chkActive.Location = new System.Drawing.Point(3, 3);
            this.chkActive.Name = "chkActive";
            this.chkActive.Size = new System.Drawing.Size(56, 17);
            this.chkActive.TabIndex = 2;
            this.chkActive.Text = "Active";
            this.chkActive.UseVisualStyleBackColor = true;
            // 
            // txtLocValue
            // 
            this.txtLocValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLocValue.Location = new System.Drawing.Point(0, 0);
            this.txtLocValue.Multiline = true;
            this.txtLocValue.Name = "txtLocValue";
            this.txtLocValue.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtLocValue.Size = new System.Drawing.Size(326, 106);
            this.txtLocValue.TabIndex = 0;
            this.txtLocValue.WordWrap = false;
            // 
            // LocalizationsManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitPanelLayout);
            this.Name = "LocalizationsManager";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.Size = new System.Drawing.Size(585, 382);
            this.splitPanelLayout.Panel1.ResumeLayout(false);
            this.splitPanelLayout.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitPanelLayout)).EndInit();
            this.splitPanelLayout.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lvwLanguages)).EndInit();
            this.splitPanelLocalizations.Panel1.ResumeLayout(false);
            this.splitPanelLocalizations.Panel2.ResumeLayout(false);
            this.splitPanelLocalizations.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitPanelLocalizations)).EndInit();
            this.splitPanelLocalizations.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tabCtrlFiles.ResumeLayout(false);
            this.tabPageApp.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lvwLocalizations)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitPanelLayout;
        private BrightIdeasSoftware.ObjectListView lvwLanguages;
        private BrightIdeasSoftware.OLVColumn lvColLangName;
        private BrightIdeasSoftware.OLVColumn lcColLangKey;
        private BrightIdeasSoftware.ObjectListView lvwLocalizations;
        private System.Windows.Forms.SplitContainer splitPanelLocalizations;
        private System.Windows.Forms.TextBox txtLocValue;
        private BrightIdeasSoftware.OLVColumn lvColLangActive;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TabControl tabCtrlFiles;
        private System.Windows.Forms.TabPage tabPageApp;
        private BrightIdeasSoftware.OLVColumn lvColLocKey;
        private BrightIdeasSoftware.OLVColumn lvColLocValue;
        private System.Windows.Forms.TabPage tabPageMaterials;
        private System.Windows.Forms.CheckBox chkActive;
    }
}
