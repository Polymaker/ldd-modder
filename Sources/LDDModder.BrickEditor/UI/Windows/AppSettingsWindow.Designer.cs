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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.UserCreationPathLabel = new System.Windows.Forms.Label();
            this.PrgmFilePathLabel = new System.Windows.Forms.Label();
            this.AppDataPathLabel = new System.Windows.Forms.Label();
            this.PrgmFilePathTextBox = new LDDModder.BrickEditor.UI.Controls.BrowseTextBox();
            this.AppDataPathTextBox = new LDDModder.BrickEditor.UI.Controls.BrowseTextBox();
            this.UserCreationPathTextBox = new LDDModder.BrickEditor.UI.Controls.BrowseTextBox();
            this.FindEnvironmentButton = new System.Windows.Forms.Button();
            this.LddPathsGroupBox = new System.Windows.Forms.GroupBox();
            this.LddDataGroupBox = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.AssetsStatusLabel = new System.Windows.Forms.Label();
            this.DbStatusLabel = new System.Windows.Forms.Label();
            this.ExtractAssetsButton = new System.Windows.Forms.Button();
            this.ExtractDBButton = new System.Windows.Forms.Button();
            this.DbInfoLabel = new System.Windows.Forms.Label();
            this.AssetsInfoLabel = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.CloseButton = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this.LifNotExtractedMessage = new LDDModder.BrickEditor.Localization.LocalizableString();
            this.LifExtractedMessage = new LDDModder.BrickEditor.Localization.LocalizableString();
            this.localizableStringList1 = new LDDModder.BrickEditor.Localization.LocalizableStringList(this.components);
            this.LifNotFoundMessage = new LDDModder.BrickEditor.Localization.LocalizableString();
            this.LddExeNotFound = new LDDModder.BrickEditor.Localization.LocalizableString();
            this.tableLayoutPanel1.SuspendLayout();
            this.LddPathsGroupBox.SuspendLayout();
            this.LddDataGroupBox.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.UserCreationPathLabel, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.PrgmFilePathLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.AppDataPathLabel, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.PrgmFilePathTextBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.AppDataPathTextBox, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.UserCreationPathTextBox, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.FindEnvironmentButton, 1, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
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
            this.PrgmFilePathTextBox.ButtonText = "...";
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
            this.AppDataPathTextBox.ButtonText = "...";
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
            this.UserCreationPathTextBox.ButtonText = "...";
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
            this.tableLayoutPanel3.SetColumnSpan(this.LddPathsGroupBox, 2);
            this.LddPathsGroupBox.Controls.Add(this.tableLayoutPanel1);
            this.LddPathsGroupBox.Name = "LddPathsGroupBox";
            this.LddPathsGroupBox.TabStop = false;
            // 
            // LddDataGroupBox
            // 
            this.tableLayoutPanel3.SetColumnSpan(this.LddDataGroupBox, 2);
            this.LddDataGroupBox.Controls.Add(this.tableLayoutPanel2);
            resources.ApplyResources(this.LddDataGroupBox, "LddDataGroupBox");
            this.LddDataGroupBox.Name = "LddDataGroupBox";
            this.LddDataGroupBox.TabStop = false;
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label2, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.AssetsStatusLabel, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.DbStatusLabel, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.ExtractAssetsButton, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.ExtractDBButton, 2, 2);
            this.tableLayoutPanel2.Controls.Add(this.DbInfoLabel, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.AssetsInfoLabel, 0, 1);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
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
            this.tableLayoutPanel2.SetColumnSpan(this.DbInfoLabel, 3);
            this.DbInfoLabel.Name = "DbInfoLabel";
            // 
            // AssetsInfoLabel
            // 
            resources.ApplyResources(this.AssetsInfoLabel, "AssetsInfoLabel");
            this.tableLayoutPanel2.SetColumnSpan(this.AssetsInfoLabel, 2);
            this.AssetsInfoLabel.Name = "AssetsInfoLabel";
            // 
            // tableLayoutPanel3
            // 
            resources.ApplyResources(this.tableLayoutPanel3, "tableLayoutPanel3");
            this.tableLayoutPanel3.Controls.Add(this.LddDataGroupBox, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.LddPathsGroupBox, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.CloseButton, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.SaveButton, 0, 2);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
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
            this.LddExeNotFound});
            // 
            // LifNotFoundMessage
            // 
            resources.ApplyResources(this.LifNotFoundMessage, "LifNotFoundMessage");
            // 
            // LddExeNotFound
            // 
            resources.ApplyResources(this.LddExeNotFound, "LddExeNotFound");
            // 
            // AppSettingsWindow
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AppSettingsWindow";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.LddPathsGroupBox.ResumeLayout(false);
            this.LddPathsGroupBox.PerformLayout();
            this.LddDataGroupBox.ResumeLayout(false);
            this.LddDataGroupBox.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label PrgmFilePathLabel;
        private System.Windows.Forms.Label AppDataPathLabel;
        private System.Windows.Forms.GroupBox LddPathsGroupBox;
        private System.Windows.Forms.GroupBox LddDataGroupBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label AssetsStatusLabel;
        private System.Windows.Forms.Label DbStatusLabel;
        private System.Windows.Forms.Button ExtractAssetsButton;
        private System.Windows.Forms.Button ExtractDBButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
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
    }
}