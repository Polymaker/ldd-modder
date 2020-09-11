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
            this.BuildConfigsGroupBox = new System.Windows.Forms.GroupBox();
            this.BuildCfgSplitContainer = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.DelBuildCfgBtn = new System.Windows.Forms.Button();
            this.AddBuildCfgBtn = new System.Windows.Forms.Button();
            this.BuildConfigListView = new System.Windows.Forms.ListView();
            this.BuildCfgNameColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.BuildCfgEditLayout = new System.Windows.Forms.TableLayoutPanel();
            this.CancelBuildCfgBtn = new System.Windows.Forms.Button();
            this.BuildCfg_OverwriteChk = new System.Windows.Forms.CheckBox();
            this.SaveBuildCfgBtn = new System.Windows.Forms.Button();
            this.BuildCfg_NameBox = new System.Windows.Forms.TextBox();
            this.BuildCfg_Lod0Chk = new System.Windows.Forms.CheckBox();
            this.BuildCfgNameLabel = new System.Windows.Forms.Label();
            this.BuildCfgPathLabel = new System.Windows.Forms.Label();
            this.PrgmFilePathTextBox = new LDDModder.BrickEditor.UI.Controls.BrowseTextBox();
            this.AppDataPathTextBox = new LDDModder.BrickEditor.UI.Controls.BrowseTextBox();
            this.UserCreationPathTextBox = new LDDModder.BrickEditor.UI.Controls.BrowseTextBox();
            this.BuildCfg_PathBox = new LDDModder.BrickEditor.UI.Controls.BrowseTextBox();
            this.LifNotExtractedMessage = new LDDModder.BrickEditor.Localization.LocalizableString();
            this.LifExtractedMessage = new LDDModder.BrickEditor.Localization.LocalizableString();
            this.localizableStringList1 = new LDDModder.BrickEditor.Localization.LocalizableStringList(this.components);
            this.LifNotFoundMessage = new LDDModder.BrickEditor.Localization.LocalizableString();
            this.LddExeNotFound = new LDDModder.BrickEditor.Localization.LocalizableString();
            this.AppDataDbNotFound = new LDDModder.BrickEditor.Localization.LocalizableString();
            this.LddPathsLayout.SuspendLayout();
            this.LddPathsGroupBox.SuspendLayout();
            this.LddDataGroupBox.SuspendLayout();
            this.LddLifsLayout.SuspendLayout();
            this.SettingsTabControl.SuspendLayout();
            this.LDDEnvPanel.SuspendLayout();
            this.EditorSettingsTabPage.SuspendLayout();
            this.BuildConfigsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BuildCfgSplitContainer)).BeginInit();
            this.BuildCfgSplitContainer.Panel1.SuspendLayout();
            this.BuildCfgSplitContainer.Panel2.SuspendLayout();
            this.BuildCfgSplitContainer.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.BuildCfgEditLayout.SuspendLayout();
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
            resources.ApplyResources(this.EditorSettingsTabPage, "EditorSettingsTabPage");
            this.EditorSettingsTabPage.Name = "EditorSettingsTabPage";
            this.EditorSettingsTabPage.UseVisualStyleBackColor = true;
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
            this.BuildCfgSplitContainer.Panel2.Controls.Add(this.BuildCfgEditLayout);
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
            // BuildCfgEditLayout
            // 
            resources.ApplyResources(this.BuildCfgEditLayout, "BuildCfgEditLayout");
            this.BuildCfgEditLayout.Controls.Add(this.CancelBuildCfgBtn, 2, 4);
            this.BuildCfgEditLayout.Controls.Add(this.BuildCfg_OverwriteChk, 0, 3);
            this.BuildCfgEditLayout.Controls.Add(this.SaveBuildCfgBtn, 1, 4);
            this.BuildCfgEditLayout.Controls.Add(this.BuildCfg_PathBox, 1, 1);
            this.BuildCfgEditLayout.Controls.Add(this.BuildCfg_NameBox, 1, 0);
            this.BuildCfgEditLayout.Controls.Add(this.BuildCfg_Lod0Chk, 0, 2);
            this.BuildCfgEditLayout.Controls.Add(this.BuildCfgNameLabel, 0, 0);
            this.BuildCfgEditLayout.Controls.Add(this.BuildCfgPathLabel, 0, 1);
            this.BuildCfgEditLayout.Name = "BuildCfgEditLayout";
            // 
            // CancelBuildCfgBtn
            // 
            resources.ApplyResources(this.CancelBuildCfgBtn, "CancelBuildCfgBtn");
            this.CancelBuildCfgBtn.Name = "CancelBuildCfgBtn";
            this.CancelBuildCfgBtn.UseVisualStyleBackColor = true;
            this.CancelBuildCfgBtn.Click += new System.EventHandler(this.CancelBuildCfgBtn_Click);
            // 
            // BuildCfg_OverwriteChk
            // 
            resources.ApplyResources(this.BuildCfg_OverwriteChk, "BuildCfg_OverwriteChk");
            this.BuildCfgEditLayout.SetColumnSpan(this.BuildCfg_OverwriteChk, 3);
            this.BuildCfg_OverwriteChk.Name = "BuildCfg_OverwriteChk";
            this.BuildCfg_OverwriteChk.UseVisualStyleBackColor = true;
            this.BuildCfg_OverwriteChk.CheckedChanged += new System.EventHandler(this.BuildCfgProperty_ValueChanged);
            // 
            // SaveBuildCfgBtn
            // 
            resources.ApplyResources(this.SaveBuildCfgBtn, "SaveBuildCfgBtn");
            this.SaveBuildCfgBtn.Name = "SaveBuildCfgBtn";
            this.SaveBuildCfgBtn.UseVisualStyleBackColor = true;
            this.SaveBuildCfgBtn.Click += new System.EventHandler(this.SaveBuildCfgBtn_Click);
            // 
            // BuildCfg_NameBox
            // 
            this.BuildCfgEditLayout.SetColumnSpan(this.BuildCfg_NameBox, 2);
            resources.ApplyResources(this.BuildCfg_NameBox, "BuildCfg_NameBox");
            this.BuildCfg_NameBox.Name = "BuildCfg_NameBox";
            this.BuildCfg_NameBox.TextChanged += new System.EventHandler(this.BuildCfgProperty_ValueChanged);
            // 
            // BuildCfg_Lod0Chk
            // 
            resources.ApplyResources(this.BuildCfg_Lod0Chk, "BuildCfg_Lod0Chk");
            this.BuildCfgEditLayout.SetColumnSpan(this.BuildCfg_Lod0Chk, 3);
            this.BuildCfg_Lod0Chk.Name = "BuildCfg_Lod0Chk";
            this.BuildCfg_Lod0Chk.UseVisualStyleBackColor = true;
            this.BuildCfg_Lod0Chk.CheckedChanged += new System.EventHandler(this.BuildCfgProperty_ValueChanged);
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
            // BuildCfg_PathBox
            // 
            resources.ApplyResources(this.BuildCfg_PathBox, "BuildCfg_PathBox");
            this.BuildCfg_PathBox.AutoSizeButton = true;
            this.BuildCfg_PathBox.ButtonWidth = 54;
            this.BuildCfgEditLayout.SetColumnSpan(this.BuildCfg_PathBox, 2);
            this.BuildCfg_PathBox.Name = "BuildCfg_PathBox";
            this.BuildCfg_PathBox.ReadOnly = true;
            this.BuildCfg_PathBox.Value = "";
            this.BuildCfg_PathBox.BrowseButtonClicked += new System.EventHandler(this.BuildCfg_PathBox_BrowseButtonClicked);
            this.BuildCfg_PathBox.ValueChanged += new System.EventHandler(this.BuildCfgProperty_ValueChanged);
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
            this.BuildConfigsGroupBox.ResumeLayout(false);
            this.BuildCfgSplitContainer.Panel1.ResumeLayout(false);
            this.BuildCfgSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.BuildCfgSplitContainer)).EndInit();
            this.BuildCfgSplitContainer.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.BuildCfgEditLayout.ResumeLayout(false);
            this.BuildCfgEditLayout.PerformLayout();
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
        private System.Windows.Forms.Label BuildCfgPathLabel;
        private System.Windows.Forms.Label BuildCfgNameLabel;
        private System.Windows.Forms.CheckBox BuildCfg_OverwriteChk;
        private System.Windows.Forms.CheckBox BuildCfg_Lod0Chk;
        private Controls.BrowseTextBox BuildCfg_PathBox;
        private System.Windows.Forms.TextBox BuildCfg_NameBox;
        private System.Windows.Forms.GroupBox BuildConfigsGroupBox;
        private System.Windows.Forms.SplitContainer BuildCfgSplitContainer;
        private System.Windows.Forms.ListView BuildConfigListView;
        private System.Windows.Forms.Button DelBuildCfgBtn;
        private System.Windows.Forms.Button AddBuildCfgBtn;
        private System.Windows.Forms.Button CancelBuildCfgBtn;
        private System.Windows.Forms.Button SaveBuildCfgBtn;
        private System.Windows.Forms.TableLayoutPanel BuildCfgEditLayout;
        private System.Windows.Forms.ColumnHeader BuildCfgNameColumn;
        private Localization.LocalizableString AppDataDbNotFound;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}