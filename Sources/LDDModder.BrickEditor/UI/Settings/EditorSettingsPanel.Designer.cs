namespace LDDModder.BrickEditor.UI.Settings
{
    partial class EditorSettingsPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditorSettingsPanel));
            this.BuildConfigsGroupBox = new System.Windows.Forms.GroupBox();
            this.BuildCfgSplitContainer = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.DelBuildCfgBtn = new System.Windows.Forms.Button();
            this.AddBuildCfgBtn = new System.Windows.Forms.Button();
            this.BuildConfigListView = new System.Windows.Forms.ListView();
            this.BuildCfgNameColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.SaveBuildCfgBtn = new System.Windows.Forms.Button();
            this.CancelBuildCfgBtn = new System.Windows.Forms.Button();
            this.BuildCfg_NameBox = new System.Windows.Forms.TextBox();
            this.BuildCfg_OverwriteChk = new System.Windows.Forms.CheckBox();
            this.BuildCfg_PathBox = new LDDModder.BrickEditor.UI.Controls.BrowseTextBox();
            this.BuildCfgNameLabel = new System.Windows.Forms.Label();
            this.BuildCfg_Lod0Chk = new System.Windows.Forms.CheckBox();
            this.BuildCfgPathLabel = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.UsernameLabel = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.WorkspaceBrowseBox = new LDDModder.BrickEditor.UI.Controls.BrowseTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.BuildConfigsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BuildCfgSplitContainer)).BeginInit();
            this.BuildCfgSplitContainer.Panel1.SuspendLayout();
            this.BuildCfgSplitContainer.Panel2.SuspendLayout();
            this.BuildCfgSplitContainer.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // BuildConfigsGroupBox
            // 
            resources.ApplyResources(this.BuildConfigsGroupBox, "BuildConfigsGroupBox");
            this.BuildConfigsGroupBox.Controls.Add(this.BuildCfgSplitContainer);
            this.BuildConfigsGroupBox.Name = "BuildConfigsGroupBox";
            this.BuildConfigsGroupBox.TabStop = false;
            // 
            // BuildCfgSplitContainer
            // 
            resources.ApplyResources(this.BuildCfgSplitContainer, "BuildCfgSplitContainer");
            this.BuildCfgSplitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.BuildCfgSplitContainer.Name = "BuildCfgSplitContainer";
            // 
            // BuildCfgSplitContainer.Panel1
            // 
            this.BuildCfgSplitContainer.Panel1.Controls.Add(this.tableLayoutPanel1);
            // 
            // BuildCfgSplitContainer.Panel2
            // 
            this.BuildCfgSplitContainer.Panel2.Controls.Add(this.tableLayoutPanel2);
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.DelBuildCfgBtn, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.AddBuildCfgBtn, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.BuildConfigListView, 0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // DelBuildCfgBtn
            // 
            resources.ApplyResources(this.DelBuildCfgBtn, "DelBuildCfgBtn");
            this.DelBuildCfgBtn.Name = "DelBuildCfgBtn";
            this.DelBuildCfgBtn.UseVisualStyleBackColor = true;
            this.DelBuildCfgBtn.Click += new System.EventHandler(this.DelBuildCfgBtn_Click);
            // 
            // AddBuildCfgBtn
            // 
            resources.ApplyResources(this.AddBuildCfgBtn, "AddBuildCfgBtn");
            this.AddBuildCfgBtn.Name = "AddBuildCfgBtn";
            this.AddBuildCfgBtn.UseVisualStyleBackColor = true;
            this.AddBuildCfgBtn.Click += new System.EventHandler(this.AddBuildCfgBtn_Click);
            // 
            // BuildConfigListView
            // 
            resources.ApplyResources(this.BuildConfigListView, "BuildConfigListView");
            this.BuildConfigListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.BuildCfgNameColumn});
            this.tableLayoutPanel1.SetColumnSpan(this.BuildConfigListView, 2);
            this.BuildConfigListView.FullRowSelect = true;
            this.BuildConfigListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.BuildConfigListView.HideSelection = false;
            this.BuildConfigListView.MultiSelect = false;
            this.BuildConfigListView.Name = "BuildConfigListView";
            this.BuildConfigListView.UseCompatibleStateImageBehavior = false;
            this.BuildConfigListView.View = System.Windows.Forms.View.Details;
            this.BuildConfigListView.SelectedIndexChanged += new System.EventHandler(this.BuildConfigListView_SelectedIndexChanged);
            // 
            // BuildCfgNameColumn
            // 
            resources.ApplyResources(this.BuildCfgNameColumn, "BuildCfgNameColumn");
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.SaveBuildCfgBtn, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.CancelBuildCfgBtn, 2, 4);
            this.tableLayoutPanel2.Controls.Add(this.BuildCfg_NameBox, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.BuildCfg_OverwriteChk, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.BuildCfg_PathBox, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.BuildCfgNameLabel, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.BuildCfg_Lod0Chk, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.BuildCfgPathLabel, 0, 1);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // SaveBuildCfgBtn
            // 
            resources.ApplyResources(this.SaveBuildCfgBtn, "SaveBuildCfgBtn");
            this.tableLayoutPanel2.SetColumnSpan(this.SaveBuildCfgBtn, 2);
            this.SaveBuildCfgBtn.Name = "SaveBuildCfgBtn";
            this.SaveBuildCfgBtn.UseVisualStyleBackColor = true;
            this.SaveBuildCfgBtn.Click += new System.EventHandler(this.SaveBuildCfgBtn_Click);
            // 
            // CancelBuildCfgBtn
            // 
            resources.ApplyResources(this.CancelBuildCfgBtn, "CancelBuildCfgBtn");
            this.CancelBuildCfgBtn.Name = "CancelBuildCfgBtn";
            this.CancelBuildCfgBtn.UseVisualStyleBackColor = true;
            this.CancelBuildCfgBtn.Click += new System.EventHandler(this.CancelBuildCfgBtn_Click);
            // 
            // BuildCfg_NameBox
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.BuildCfg_NameBox, 2);
            resources.ApplyResources(this.BuildCfg_NameBox, "BuildCfg_NameBox");
            this.BuildCfg_NameBox.Name = "BuildCfg_NameBox";
            this.BuildCfg_NameBox.TextChanged += new System.EventHandler(this.BuildCfgProperty_ValueChanged);
            // 
            // BuildCfg_OverwriteChk
            // 
            this.BuildCfg_OverwriteChk.AutoEllipsis = true;
            resources.ApplyResources(this.BuildCfg_OverwriteChk, "BuildCfg_OverwriteChk");
            this.tableLayoutPanel2.SetColumnSpan(this.BuildCfg_OverwriteChk, 3);
            this.BuildCfg_OverwriteChk.Name = "BuildCfg_OverwriteChk";
            this.BuildCfg_OverwriteChk.UseVisualStyleBackColor = true;
            this.BuildCfg_OverwriteChk.CheckStateChanged += new System.EventHandler(this.BuildCfgProperty_ValueChanged);
            // 
            // BuildCfg_PathBox
            // 
            resources.ApplyResources(this.BuildCfg_PathBox, "BuildCfg_PathBox");
            this.BuildCfg_PathBox.AutoSizeButton = true;
            this.BuildCfg_PathBox.ButtonWidth = 52;
            this.tableLayoutPanel2.SetColumnSpan(this.BuildCfg_PathBox, 2);
            this.BuildCfg_PathBox.Name = "BuildCfg_PathBox";
            this.BuildCfg_PathBox.ReadOnly = true;
            this.BuildCfg_PathBox.Value = "";
            this.BuildCfg_PathBox.BrowseButtonClicked += new System.EventHandler(this.BuildCfg_PathBox_BrowseButtonClicked);
            this.BuildCfg_PathBox.ValueChanged += new System.EventHandler(this.BuildCfgProperty_ValueChanged);
            // 
            // BuildCfgNameLabel
            // 
            resources.ApplyResources(this.BuildCfgNameLabel, "BuildCfgNameLabel");
            this.BuildCfgNameLabel.Name = "BuildCfgNameLabel";
            // 
            // BuildCfg_Lod0Chk
            // 
            this.BuildCfg_Lod0Chk.AutoEllipsis = true;
            resources.ApplyResources(this.BuildCfg_Lod0Chk, "BuildCfg_Lod0Chk");
            this.tableLayoutPanel2.SetColumnSpan(this.BuildCfg_Lod0Chk, 3);
            this.BuildCfg_Lod0Chk.Name = "BuildCfg_Lod0Chk";
            this.BuildCfg_Lod0Chk.UseVisualStyleBackColor = true;
            this.BuildCfg_Lod0Chk.CheckedChanged += new System.EventHandler(this.BuildCfgProperty_ValueChanged);
            // 
            // BuildCfgPathLabel
            // 
            resources.ApplyResources(this.BuildCfgPathLabel, "BuildCfgPathLabel");
            this.BuildCfgPathLabel.Name = "BuildCfgPathLabel";
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.numericUpDown1);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.UsernameLabel);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.WorkspaceBrowseBox);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // numericUpDown1
            // 
            resources.ApplyResources(this.numericUpDown1, "numericUpDown1");
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // UsernameLabel
            // 
            resources.ApplyResources(this.UsernameLabel, "UsernameLabel");
            this.UsernameLabel.Name = "UsernameLabel";
            // 
            // textBox1
            // 
            resources.ApplyResources(this.textBox1, "textBox1");
            this.textBox1.Name = "textBox1";
            // 
            // WorkspaceBrowseBox
            // 
            resources.ApplyResources(this.WorkspaceBrowseBox, "WorkspaceBrowseBox");
            this.WorkspaceBrowseBox.Name = "WorkspaceBrowseBox";
            this.WorkspaceBrowseBox.Value = "";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // EditorSettingsPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.BuildConfigsGroupBox);
            this.Controls.Add(this.groupBox1);
            this.Name = "EditorSettingsPanel";
            this.BuildConfigsGroupBox.ResumeLayout(false);
            this.BuildCfgSplitContainer.Panel1.ResumeLayout(false);
            this.BuildCfgSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.BuildCfgSplitContainer)).EndInit();
            this.BuildCfgSplitContainer.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox BuildConfigsGroupBox;
        private System.Windows.Forms.SplitContainer BuildCfgSplitContainer;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button DelBuildCfgBtn;
        private System.Windows.Forms.Button AddBuildCfgBtn;
        private System.Windows.Forms.ListView BuildConfigListView;
        private System.Windows.Forms.ColumnHeader BuildCfgNameColumn;
        private System.Windows.Forms.Button CancelBuildCfgBtn;
        private System.Windows.Forms.CheckBox BuildCfg_OverwriteChk;
        private System.Windows.Forms.Button SaveBuildCfgBtn;
        private Controls.BrowseTextBox BuildCfg_PathBox;
        private System.Windows.Forms.TextBox BuildCfg_NameBox;
        private System.Windows.Forms.CheckBox BuildCfg_Lod0Chk;
        private System.Windows.Forms.Label BuildCfgNameLabel;
        private System.Windows.Forms.Label BuildCfgPathLabel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label UsernameLabel;
        private System.Windows.Forms.TextBox textBox1;
        private Controls.BrowseTextBox WorkspaceBrowseBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
    }
}
