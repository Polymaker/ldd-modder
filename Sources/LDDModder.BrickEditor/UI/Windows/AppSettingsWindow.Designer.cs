namespace LDDModder.BrickEditor.UI.Windows
{
    partial class AppSettingsWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AppSettingsWindow));
            this.LddPathsLayout = new System.Windows.Forms.TableLayoutPanel();
            this.UserCreationPathLabel = new System.Windows.Forms.Label();
            this.PrgmFilePathLabel = new System.Windows.Forms.Label();
            this.AppDataPathLabel = new System.Windows.Forms.Label();
            this.PrgmFilePathTextBox = new LDDModder.BrickEditor.UI.Controls.BrowseTextBox();
            this.AppDataPathTextBox = new LDDModder.BrickEditor.UI.Controls.BrowseTextBox();
            this.UserCreationPathTextBox = new LDDModder.BrickEditor.UI.Controls.BrowseTextBox();
            this.FindEnvironmentButton = new System.Windows.Forms.Button();
            this.LddPathsGroupBox = new System.Windows.Forms.GroupBox();
            this.LddDataGroupBox = new System.Windows.Forms.GroupBox();
            this.LddLifsLayout = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.AssetsStatusLabel = new System.Windows.Forms.Label();
            this.DbStatusLabel = new System.Windows.Forms.Label();
            this.ExtractAssetsButton = new System.Windows.Forms.Button();
            this.ExtractDBButton = new System.Windows.Forms.Button();
            this.DbInfoLabel = new System.Windows.Forms.Label();
            this.AssetsInfoLabel = new System.Windows.Forms.Label();
            this.CloseButton = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this.SettingsTabControl = new System.Windows.Forms.TabControl();
            this.LDDEnvPanel = new System.Windows.Forms.TabPage();
            this.EditorSettingsTabPage = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.BuildConfigsGroupBox = new System.Windows.Forms.GroupBox();
            this.BuildCfgSplitContainer = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.DelBuildCfgBtn = new System.Windows.Forms.Button();
            this.AddBuildCfgBtn = new System.Windows.Forms.Button();
            this.BuildConfigListView = new System.Windows.Forms.ListView();
            this.BuildCfgNameColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.LifNotExtractedMessage = new LDDModder.BrickEditor.Localization.LocalizableString();
            this.LifExtractedMessage = new LDDModder.BrickEditor.Localization.LocalizableString();
            this.localizableStringList1 = new LDDModder.BrickEditor.Localization.LocalizableStringList(this.components);
            this.LifNotFoundMessage = new LDDModder.BrickEditor.Localization.LocalizableString();
            this.LddExeNotFound = new LDDModder.BrickEditor.Localization.LocalizableString();
            this.AppDataDbNotFound = new LDDModder.BrickEditor.Localization.LocalizableString();
            this.UsernameLabel = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.WorkspaceBrowseBox = new LDDModder.BrickEditor.UI.Controls.BrowseTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.CancelBuildCfgBtn = new System.Windows.Forms.Button();
            this.BuildCfg_OverwriteChk = new System.Windows.Forms.CheckBox();
            this.SaveBuildCfgBtn = new System.Windows.Forms.Button();
            this.BuildCfg_PathBox = new LDDModder.BrickEditor.UI.Controls.BrowseTextBox();
            this.BuildCfg_NameBox = new System.Windows.Forms.TextBox();
            this.BuildCfg_Lod0Chk = new System.Windows.Forms.CheckBox();
            this.BuildCfgNameLabel = new System.Windows.Forms.Label();
            this.BuildCfgPathLabel = new System.Windows.Forms.Label();
            this.LddPathsLayout.SuspendLayout();
            this.LddPathsGroupBox.SuspendLayout();
            this.LddDataGroupBox.SuspendLayout();
            this.LddLifsLayout.SuspendLayout();
            this.SettingsTabControl.SuspendLayout();
            this.LDDEnvPanel.SuspendLayout();
            this.EditorSettingsTabPage.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.BuildConfigsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BuildCfgSplitContainer)).BeginInit();
            this.BuildCfgSplitContainer.Panel1.SuspendLayout();
            this.BuildCfgSplitContainer.Panel2.SuspendLayout();
            this.BuildCfgSplitContainer.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // LddPathsLayout
            // 
            resources.ApplyResources(this.LddPathsLayout, "LddPathsLayout");
            this.LddPathsLayout.Controls.Add(this.UserCreationPathLabel, 0, 2);
            this.LddPathsLayout.Controls.Add(this.PrgmFilePathLabel, 0, 0);
            this.LddPathsLayout.Controls.Add(this.AppDataPathLabel, 0, 1);
            this.LddPathsLayout.Controls.Add(this.PrgmFilePathTextBox, 1, 0);
            this.LddPathsLayout.Controls.Add(this.AppDataPathTextBox, 1, 1);
            this.LddPathsLayout.Controls.Add(this.UserCreationPathTextBox, 1, 2);
            this.LddPathsLayout.Controls.Add(this.FindEnvironmentButton, 1, 3);
            this.LddPathsLayout.Name = "LddPathsLayout";
            // 
            // UserCreationPathLabel
            // 
            resources.ApplyResources(this.UserCreationPathLabel, "UserCreationPathLabel");
            this.UserCreationPathLabel.Name = "UserCreationPathLabel";
            // 
            // PrgmFilePathLabel
            // 
            resources.ApplyResources(this.PrgmFilePathLabel, "PrgmFilePathLabel");
            this.PrgmFilePathLabel.Name = "PrgmFilePathLabel";
            // 
            // AppDataPathLabel
            // 
            resources.ApplyResources(this.AppDataPathLabel, "AppDataPathLabel");
            this.AppDataPathLabel.Name = "AppDataPathLabel";
            // 
            // PrgmFilePathTextBox
            // 
            resources.ApplyResources(this.PrgmFilePathTextBox, "PrgmFilePathTextBox");
            this.PrgmFilePathTextBox.AutoSizeButton = true;
            this.PrgmFilePathTextBox.ButtonWidth = 26;
            this.PrgmFilePathTextBox.Name = "PrgmFilePathTextBox";
            this.PrgmFilePathTextBox.Value = "";
            this.PrgmFilePathTextBox.BrowseButtonClicked += new System.EventHandler(this.PrgmFilePathTextBox_BrowseButtonClicked);
            this.PrgmFilePathTextBox.ValueChanged += new System.EventHandler(this.LddPathTextBoxes_ValueChanged);
            // 
            // AppDataPathTextBox
            // 
            resources.ApplyResources(this.AppDataPathTextBox, "AppDataPathTextBox");
            this.AppDataPathTextBox.AutoSizeButton = true;
            this.AppDataPathTextBox.ButtonWidth = 26;
            this.AppDataPathTextBox.Name = "AppDataPathTextBox";
            this.AppDataPathTextBox.Value = "";
            this.AppDataPathTextBox.BrowseButtonClicked += new System.EventHandler(this.PrgmFilePathTextBox_BrowseButtonClicked);
            this.AppDataPathTextBox.ValueChanged += new System.EventHandler(this.LddPathTextBoxes_ValueChanged);
            // 
            // UserCreationPathTextBox
            // 
            resources.ApplyResources(this.UserCreationPathTextBox, "UserCreationPathTextBox");
            this.UserCreationPathTextBox.AutoSizeButton = true;
            this.UserCreationPathTextBox.ButtonWidth = 26;
            this.UserCreationPathTextBox.Name = "UserCreationPathTextBox";
            this.UserCreationPathTextBox.Value = "";
            this.UserCreationPathTextBox.BrowseButtonClicked += new System.EventHandler(this.PrgmFilePathTextBox_BrowseButtonClicked);
            // 
            // FindEnvironmentButton
            // 
            resources.ApplyResources(this.FindEnvironmentButton, "FindEnvironmentButton");
            this.FindEnvironmentButton.Name = "FindEnvironmentButton";
            this.FindEnvironmentButton.UseVisualStyleBackColor = true;
            this.FindEnvironmentButton.Click += new System.EventHandler(this.FindEnvironmentButton_Click);
            // 
            // LddPathsGroupBox
            // 
            resources.ApplyResources(this.LddPathsGroupBox, "LddPathsGroupBox");
            this.LddPathsGroupBox.Controls.Add(this.LddPathsLayout);
            this.LddPathsGroupBox.Name = "LddPathsGroupBox";
            this.LddPathsGroupBox.TabStop = false;
            // 
            // LddDataGroupBox
            // 
            resources.ApplyResources(this.LddDataGroupBox, "LddDataGroupBox");
            this.LddDataGroupBox.Controls.Add(this.LddLifsLayout);
            this.LddDataGroupBox.Name = "LddDataGroupBox";
            this.LddDataGroupBox.TabStop = false;
            // 
            // LddLifsLayout
            // 
            resources.ApplyResources(this.LddLifsLayout, "LddLifsLayout");
            this.LddLifsLayout.Controls.Add(this.label1, 0, 0);
            this.LddLifsLayout.Controls.Add(this.label2, 0, 2);
            this.LddLifsLayout.Controls.Add(this.AssetsStatusLabel, 1, 0);
            this.LddLifsLayout.Controls.Add(this.DbStatusLabel, 1, 2);
            this.LddLifsLayout.Controls.Add(this.ExtractAssetsButton, 2, 0);
            this.LddLifsLayout.Controls.Add(this.ExtractDBButton, 2, 2);
            this.LddLifsLayout.Controls.Add(this.DbInfoLabel, 0, 3);
            this.LddLifsLayout.Controls.Add(this.AssetsInfoLabel, 0, 1);
            this.LddLifsLayout.Name = "LddLifsLayout";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // AssetsStatusLabel
            // 
            resources.ApplyResources(this.AssetsStatusLabel, "AssetsStatusLabel");
            this.AssetsStatusLabel.Name = "AssetsStatusLabel";
            // 
            // DbStatusLabel
            // 
            resources.ApplyResources(this.DbStatusLabel, "DbStatusLabel");
            this.DbStatusLabel.Name = "DbStatusLabel";
            // 
            // ExtractAssetsButton
            // 
            resources.ApplyResources(this.ExtractAssetsButton, "ExtractAssetsButton");
            this.ExtractAssetsButton.Name = "ExtractAssetsButton";
            this.ExtractAssetsButton.UseVisualStyleBackColor = true;
            this.ExtractAssetsButton.Click += new System.EventHandler(this.ExtractAssetsButton_Click);
            // 
            // ExtractDBButton
            // 
            resources.ApplyResources(this.ExtractDBButton, "ExtractDBButton");
            this.ExtractDBButton.Name = "ExtractDBButton";
            this.ExtractDBButton.UseVisualStyleBackColor = true;
            this.ExtractDBButton.Click += new System.EventHandler(this.ExtractDBButton_Click);
            // 
            // DbInfoLabel
            // 
            resources.ApplyResources(this.DbInfoLabel, "DbInfoLabel");
            this.LddLifsLayout.SetColumnSpan(this.DbInfoLabel, 3);
            this.DbInfoLabel.Name = "DbInfoLabel";
            // 
            // AssetsInfoLabel
            // 
            resources.ApplyResources(this.AssetsInfoLabel, "AssetsInfoLabel");
            this.LddLifsLayout.SetColumnSpan(this.AssetsInfoLabel, 2);
            this.AssetsInfoLabel.Name = "AssetsInfoLabel";
            // 
            // CloseButton
            // 
            resources.ApplyResources(this.CloseButton, "CloseButton");
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // SaveButton
            // 
            resources.ApplyResources(this.SaveButton, "SaveButton");
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // SettingsTabControl
            // 
            this.SettingsTabControl.Controls.Add(this.LDDEnvPanel);
            this.SettingsTabControl.Controls.Add(this.EditorSettingsTabPage);
            resources.ApplyResources(this.SettingsTabControl, "SettingsTabControl");
            this.SettingsTabControl.Name = "SettingsTabControl";
            this.SettingsTabControl.SelectedIndex = 0;
            this.SettingsTabControl.SelectedIndexChanged += new System.EventHandler(this.SettingsTabControl_SelectedIndexChanged);
            // 
            // LDDEnvPanel
            // 
            this.LDDEnvPanel.Controls.Add(this.LddPathsGroupBox);
            this.LDDEnvPanel.Controls.Add(this.LddDataGroupBox);
            this.LDDEnvPanel.Controls.Add(this.CloseButton);
            this.LDDEnvPanel.Controls.Add(this.SaveButton);
            resources.ApplyResources(this.LDDEnvPanel, "LDDEnvPanel");
            this.LDDEnvPanel.Name = "LDDEnvPanel";
            this.LDDEnvPanel.UseVisualStyleBackColor = true;
            // 
            // EditorSettingsTabPage
            // 
            this.EditorSettingsTabPage.Controls.Add(this.BuildConfigsGroupBox);
            this.EditorSettingsTabPage.Controls.Add(this.groupBox1);
            resources.ApplyResources(this.EditorSettingsTabPage, "EditorSettingsTabPage");
            this.EditorSettingsTabPage.Name = "EditorSettingsTabPage";
            this.EditorSettingsTabPage.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.numericUpDown1);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.UsernameLabel);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.WorkspaceBrowseBox);
            this.groupBox1.Controls.Add(this.label3);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // BuildConfigsGroupBox
            // 
            this.BuildConfigsGroupBox.Controls.Add(this.BuildCfgSplitContainer);
            resources.ApplyResources(this.BuildConfigsGroupBox, "BuildConfigsGroupBox");
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
            this.BuildCfgSplitContainer.Panel2.Controls.Add(this.CancelBuildCfgBtn);
            this.BuildCfgSplitContainer.Panel2.Controls.Add(this.BuildCfg_OverwriteChk);
            this.BuildCfgSplitContainer.Panel2.Controls.Add(this.SaveBuildCfgBtn);
            this.BuildCfgSplitContainer.Panel2.Controls.Add(this.BuildCfg_PathBox);
            this.BuildCfgSplitContainer.Panel2.Controls.Add(this.BuildCfg_NameBox);
            this.BuildCfgSplitContainer.Panel2.Controls.Add(this.BuildCfg_Lod0Chk);
            this.BuildCfgSplitContainer.Panel2.Controls.Add(this.BuildCfgNameLabel);
            this.BuildCfgSplitContainer.Panel2.Controls.Add(this.BuildCfgPathLabel);
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
            this.BuildConfigListView.SizeChanged += new System.EventHandler(this.BuildConfigListView_SizeChanged);
            // 
            // BuildCfgNameColumn
            // 
            resources.ApplyResources(this.BuildCfgNameColumn, "BuildCfgNameColumn");
            // 
            // LifNotExtractedMessage
            // 
            resources.ApplyResources(this.LifNotExtractedMessage, "LifNotExtractedMessage");
            // 
            // LifExtractedMessage
            // 
            resources.ApplyResources(this.LifExtractedMessage, "LifExtractedMessage");
            // 
            // localizableStringList1
            // 
            this.localizableStringList1.Items.AddRange(new LDDModder.BrickEditor.Localization.LocalizableString[] {
            this.LifExtractedMessage,
            this.LifNotExtractedMessage,
            this.LifNotFoundMessage,
            this.LddExeNotFound,
            this.AppDataDbNotFound});
            // 
            // LifNotFoundMessage
            // 
            resources.ApplyResources(this.LifNotFoundMessage, "LifNotFoundMessage");
            // 
            // LddExeNotFound
            // 
            resources.ApplyResources(this.LddExeNotFound, "LddExeNotFound");
            // 
            // AppDataDbNotFound
            // 
            resources.ApplyResources(this.AppDataDbNotFound, "AppDataDbNotFound");
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
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
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
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // CancelBuildCfgBtn
            // 
            resources.ApplyResources(this.CancelBuildCfgBtn, "CancelBuildCfgBtn");
            this.CancelBuildCfgBtn.Name = "CancelBuildCfgBtn";
            this.CancelBuildCfgBtn.UseVisualStyleBackColor = true;
            // 
            // BuildCfg_OverwriteChk
            // 
            resources.ApplyResources(this.BuildCfg_OverwriteChk, "BuildCfg_OverwriteChk");
            this.BuildCfg_OverwriteChk.Name = "BuildCfg_OverwriteChk";
            this.BuildCfg_OverwriteChk.UseVisualStyleBackColor = true;
            // 
            // SaveBuildCfgBtn
            // 
            resources.ApplyResources(this.SaveBuildCfgBtn, "SaveBuildCfgBtn");
            this.SaveBuildCfgBtn.Name = "SaveBuildCfgBtn";
            this.SaveBuildCfgBtn.UseVisualStyleBackColor = true;
            // 
            // BuildCfg_PathBox
            // 
            resources.ApplyResources(this.BuildCfg_PathBox, "BuildCfg_PathBox");
            this.BuildCfg_PathBox.AutoSizeButton = true;
            this.BuildCfg_PathBox.ButtonWidth = 54;
            this.BuildCfg_PathBox.Name = "BuildCfg_PathBox";
            this.BuildCfg_PathBox.ReadOnly = true;
            this.BuildCfg_PathBox.Value = "";
            // 
            // BuildCfg_NameBox
            // 
            resources.ApplyResources(this.BuildCfg_NameBox, "BuildCfg_NameBox");
            this.BuildCfg_NameBox.Name = "BuildCfg_NameBox";
            // 
            // BuildCfg_Lod0Chk
            // 
            resources.ApplyResources(this.BuildCfg_Lod0Chk, "BuildCfg_Lod0Chk");
            this.BuildCfg_Lod0Chk.Name = "BuildCfg_Lod0Chk";
            this.BuildCfg_Lod0Chk.UseVisualStyleBackColor = true;
            // 
            // BuildCfgNameLabel
            // 
            resources.ApplyResources(this.BuildCfgNameLabel, "BuildCfgNameLabel");
            this.BuildCfgNameLabel.Name = "BuildCfgNameLabel";
            // 
            // BuildCfgPathLabel
            // 
            resources.ApplyResources(this.BuildCfgPathLabel, "BuildCfgPathLabel");
            this.BuildCfgPathLabel.Name = "BuildCfgPathLabel";
            // 
            // AppSettingsWindow
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.SettingsTabControl);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AppSettingsWindow";
            this.LddPathsLayout.ResumeLayout(false);
            this.LddPathsLayout.PerformLayout();
            this.LddPathsGroupBox.ResumeLayout(false);
            this.LddDataGroupBox.ResumeLayout(false);
            this.LddLifsLayout.ResumeLayout(false);
            this.LddLifsLayout.PerformLayout();
            this.SettingsTabControl.ResumeLayout(false);
            this.LDDEnvPanel.ResumeLayout(false);
            this.EditorSettingsTabPage.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.BuildConfigsGroupBox.ResumeLayout(false);
            this.BuildCfgSplitContainer.Panel1.ResumeLayout(false);
            this.BuildCfgSplitContainer.Panel2.ResumeLayout(false);
            this.BuildCfgSplitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BuildCfgSplitContainer)).EndInit();
            this.BuildCfgSplitContainer.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel LddPathsLayout;
        private System.Windows.Forms.Label PrgmFilePathLabel;
        private System.Windows.Forms.Label AppDataPathLabel;
        private System.Windows.Forms.GroupBox LddPathsGroupBox;
        private System.Windows.Forms.GroupBox LddDataGroupBox;
        private System.Windows.Forms.TableLayoutPanel LddLifsLayout;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label AssetsStatusLabel;
        private System.Windows.Forms.Label DbStatusLabel;
        private System.Windows.Forms.Button ExtractAssetsButton;
        private System.Windows.Forms.Button ExtractDBButton;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.Label DbInfoLabel;
        private System.Windows.Forms.Label AssetsInfoLabel;
        private Controls.BrowseTextBox PrgmFilePathTextBox;
        private Controls.BrowseTextBox AppDataPathTextBox;
        private System.Windows.Forms.Label UserCreationPathLabel;
        private Controls.BrowseTextBox UserCreationPathTextBox;
        private Localization.LocalizableString LifNotExtractedMessage;
        private Localization.LocalizableString LifExtractedMessage;
        private Localization.LocalizableStringList localizableStringList1;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button FindEnvironmentButton;
        private Localization.LocalizableString LifNotFoundMessage;
        private Localization.LocalizableString LddExeNotFound;
        private System.Windows.Forms.TabControl SettingsTabControl;
        private System.Windows.Forms.TabPage LDDEnvPanel;
        private System.Windows.Forms.TabPage EditorSettingsTabPage;
        private System.Windows.Forms.GroupBox BuildConfigsGroupBox;
        private System.Windows.Forms.SplitContainer BuildCfgSplitContainer;
        private System.Windows.Forms.ListView BuildConfigListView;
        private System.Windows.Forms.Button DelBuildCfgBtn;
        private System.Windows.Forms.Button AddBuildCfgBtn;
        private System.Windows.Forms.ColumnHeader BuildCfgNameColumn;
        private Localization.LocalizableString AppDataDbNotFound;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label UsernameLabel;
        private System.Windows.Forms.TextBox textBox1;
        private Controls.BrowseTextBox WorkspaceBrowseBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button CancelBuildCfgBtn;
        private System.Windows.Forms.CheckBox BuildCfg_OverwriteChk;
        private System.Windows.Forms.Button SaveBuildCfgBtn;
        private Controls.BrowseTextBox BuildCfg_PathBox;
        private System.Windows.Forms.TextBox BuildCfg_NameBox;
        private System.Windows.Forms.CheckBox BuildCfg_Lod0Chk;
        private System.Windows.Forms.Label BuildCfgNameLabel;
        private System.Windows.Forms.Label BuildCfgPathLabel;
    }
}