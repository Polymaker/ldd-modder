namespace LDDModder.BrickEditor.UI.Settings
{
    partial class LddSettingsPanel
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LddSettingsPanel));
            this.LddPathsGroupBox = new System.Windows.Forms.GroupBox();
            this.LddPathsLayout = new System.Windows.Forms.TableLayoutPanel();
            this.PrgmFilePathLabel = new System.Windows.Forms.Label();
            this.AppDataPathLabel = new System.Windows.Forms.Label();
            this.PrgmFilePathTextBox = new LDDModder.BrickEditor.UI.Controls.BrowseTextBox();
            this.AppDataPathTextBox = new LDDModder.BrickEditor.UI.Controls.BrowseTextBox();
            this.FindEnvironmentButton = new System.Windows.Forms.Button();
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
            this.localizableStringList1 = new LDDModder.BrickEditor.Localization.LocalizableStringList(this.components);
            this.LifExtractedMessage = new LDDModder.BrickEditor.Localization.LocalizableString();
            this.LifNotExtractedMessage = new LDDModder.BrickEditor.Localization.LocalizableString();
            this.LifNotFoundMessage = new LDDModder.BrickEditor.Localization.LocalizableString();
            this.LddExeNotFound = new LDDModder.BrickEditor.Localization.LocalizableString();
            this.AppDataDbNotFound = new LDDModder.BrickEditor.Localization.LocalizableString();
            this.LddPathsGroupBox.SuspendLayout();
            this.LddPathsLayout.SuspendLayout();
            this.LddDataGroupBox.SuspendLayout();
            this.LddLifsLayout.SuspendLayout();
            this.SuspendLayout();
            // 
            // LddPathsGroupBox
            // 
            resources.ApplyResources(this.LddPathsGroupBox, "LddPathsGroupBox");
            this.LddPathsGroupBox.Controls.Add(this.LddPathsLayout);
            this.LddPathsGroupBox.Name = "LddPathsGroupBox";
            this.LddPathsGroupBox.TabStop = false;
            // 
            // LddPathsLayout
            // 
            resources.ApplyResources(this.LddPathsLayout, "LddPathsLayout");
            this.LddPathsLayout.Controls.Add(this.PrgmFilePathLabel, 0, 0);
            this.LddPathsLayout.Controls.Add(this.AppDataPathLabel, 0, 1);
            this.LddPathsLayout.Controls.Add(this.PrgmFilePathTextBox, 1, 0);
            this.LddPathsLayout.Controls.Add(this.AppDataPathTextBox, 1, 1);
            this.LddPathsLayout.Controls.Add(this.FindEnvironmentButton, 1, 2);
            this.LddPathsLayout.Name = "LddPathsLayout";
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
            // FindEnvironmentButton
            // 
            resources.ApplyResources(this.FindEnvironmentButton, "FindEnvironmentButton");
            this.FindEnvironmentButton.Name = "FindEnvironmentButton";
            this.FindEnvironmentButton.UseVisualStyleBackColor = true;
            this.FindEnvironmentButton.Click += new System.EventHandler(this.FindEnvironmentButton_Click);
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
            // localizableStringList1
            // 
            this.localizableStringList1.Items.AddRange(new LDDModder.BrickEditor.Localization.LocalizableString[] {
            this.LifExtractedMessage,
            this.LifNotExtractedMessage,
            this.LifNotFoundMessage,
            this.LddExeNotFound,
            this.AppDataDbNotFound});
            // 
            // LifExtractedMessage
            // 
            resources.ApplyResources(this.LifExtractedMessage, "LifExtractedMessage");
            // 
            // LifNotExtractedMessage
            // 
            resources.ApplyResources(this.LifNotExtractedMessage, "LifNotExtractedMessage");
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
            // LddSettingsPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.LddPathsGroupBox);
            this.Controls.Add(this.LddDataGroupBox);
            this.Name = "LddSettingsPanel";
            this.LddPathsGroupBox.ResumeLayout(false);
            this.LddPathsLayout.ResumeLayout(false);
            this.LddPathsLayout.PerformLayout();
            this.LddDataGroupBox.ResumeLayout(false);
            this.LddLifsLayout.ResumeLayout(false);
            this.LddLifsLayout.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox LddPathsGroupBox;
        private System.Windows.Forms.TableLayoutPanel LddPathsLayout;
        private System.Windows.Forms.Label PrgmFilePathLabel;
        private System.Windows.Forms.Label AppDataPathLabel;
        private Controls.BrowseTextBox PrgmFilePathTextBox;
        private Controls.BrowseTextBox AppDataPathTextBox;
        private System.Windows.Forms.Button FindEnvironmentButton;
        private System.Windows.Forms.GroupBox LddDataGroupBox;
        private System.Windows.Forms.TableLayoutPanel LddLifsLayout;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label AssetsStatusLabel;
        private System.Windows.Forms.Label DbStatusLabel;
        private System.Windows.Forms.Button ExtractAssetsButton;
        private System.Windows.Forms.Button ExtractDBButton;
        private System.Windows.Forms.Label DbInfoLabel;
        private System.Windows.Forms.Label AssetsInfoLabel;
        private Localization.LocalizableStringList localizableStringList1;
        private Localization.LocalizableString LifExtractedMessage;
        private Localization.LocalizableString LifNotExtractedMessage;
        private Localization.LocalizableString LifNotFoundMessage;
        private Localization.LocalizableString LddExeNotFound;
        private Localization.LocalizableString AppDataDbNotFound;
    }
}
