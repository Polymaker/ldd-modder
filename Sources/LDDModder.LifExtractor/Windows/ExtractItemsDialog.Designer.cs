namespace LDDModder.LifExtractor.Windows
{
    partial class ExtractItemsDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExtractItemsDialog));
            this.SelectFolderButton = new System.Windows.Forms.Button();
            this.DestinationTextBox = new System.Windows.Forms.TextBox();
            this.CancelExtractButton = new System.Windows.Forms.Button();
            this.ExtractButton = new System.Windows.Forms.Button();
            this.DestinationGroupBox = new System.Windows.Forms.GroupBox();
            this.DestinationTableLayout = new System.Windows.Forms.TableLayoutPanel();
            this.CreateSubDirectoryCheckBox = new System.Windows.Forms.CheckBox();
            this.SubDirectoryTextBox = new System.Windows.Forms.TextBox();
            this.ProgressGroupBox = new System.Windows.Forms.GroupBox();
            this.ExtractionProgressTimer = new System.Windows.Forms.Timer(this.components);
            this.ButtonsTableLayout = new System.Windows.Forms.TableLayoutPanel();
            this.extractProgressPanel1 = new LDDModder.LifExtractor.Controls.ExtractProgressPanel();
            this.DestinationGroupBox.SuspendLayout();
            this.DestinationTableLayout.SuspendLayout();
            this.ProgressGroupBox.SuspendLayout();
            this.ButtonsTableLayout.SuspendLayout();
            this.SuspendLayout();
            // 
            // SelectFolderButton
            // 
            resources.ApplyResources(this.SelectFolderButton, "SelectFolderButton");
            this.SelectFolderButton.Name = "SelectFolderButton";
            this.SelectFolderButton.UseVisualStyleBackColor = true;
            this.SelectFolderButton.Click += new System.EventHandler(this.SelectFolderButton_Click);
            // 
            // DestinationTextBox
            // 
            resources.ApplyResources(this.DestinationTextBox, "DestinationTextBox");
            this.DestinationTableLayout.SetColumnSpan(this.DestinationTextBox, 2);
            this.DestinationTextBox.Name = "DestinationTextBox";
            this.DestinationTextBox.TextChanged += new System.EventHandler(this.DestinationTextBox_TextChanged);
            // 
            // CancelExtractButton
            // 
            resources.ApplyResources(this.CancelExtractButton, "CancelExtractButton");
            this.CancelExtractButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelExtractButton.Name = "CancelExtractButton";
            this.CancelExtractButton.UseVisualStyleBackColor = true;
            this.CancelExtractButton.Click += new System.EventHandler(this.CancelExtractButton_Click);
            // 
            // ExtractButton
            // 
            resources.ApplyResources(this.ExtractButton, "ExtractButton");
            this.ExtractButton.Name = "ExtractButton";
            this.ExtractButton.UseVisualStyleBackColor = true;
            this.ExtractButton.Click += new System.EventHandler(this.ExtractButton_Click);
            // 
            // DestinationGroupBox
            // 
            resources.ApplyResources(this.DestinationGroupBox, "DestinationGroupBox");
            this.ButtonsTableLayout.SetColumnSpan(this.DestinationGroupBox, 2);
            this.DestinationGroupBox.Controls.Add(this.DestinationTableLayout);
            this.DestinationGroupBox.Name = "DestinationGroupBox";
            this.DestinationGroupBox.TabStop = false;
            // 
            // DestinationTableLayout
            // 
            resources.ApplyResources(this.DestinationTableLayout, "DestinationTableLayout");
            this.DestinationTableLayout.Controls.Add(this.SelectFolderButton, 2, 0);
            this.DestinationTableLayout.Controls.Add(this.DestinationTextBox, 0, 0);
            this.DestinationTableLayout.Controls.Add(this.CreateSubDirectoryCheckBox, 0, 1);
            this.DestinationTableLayout.Controls.Add(this.SubDirectoryTextBox, 1, 1);
            this.DestinationTableLayout.Name = "DestinationTableLayout";
            // 
            // CreateSubDirectoryCheckBox
            // 
            resources.ApplyResources(this.CreateSubDirectoryCheckBox, "CreateSubDirectoryCheckBox");
            this.CreateSubDirectoryCheckBox.Name = "CreateSubDirectoryCheckBox";
            this.CreateSubDirectoryCheckBox.UseVisualStyleBackColor = true;
            this.CreateSubDirectoryCheckBox.CheckedChanged += new System.EventHandler(this.CreateSubDirectoryCheckBox_CheckedChanged);
            // 
            // SubDirectoryTextBox
            // 
            resources.ApplyResources(this.SubDirectoryTextBox, "SubDirectoryTextBox");
            this.SubDirectoryTextBox.Name = "SubDirectoryTextBox";
            // 
            // ProgressGroupBox
            // 
            this.ButtonsTableLayout.SetColumnSpan(this.ProgressGroupBox, 2);
            this.ProgressGroupBox.Controls.Add(this.extractProgressPanel1);
            resources.ApplyResources(this.ProgressGroupBox, "ProgressGroupBox");
            this.ProgressGroupBox.Name = "ProgressGroupBox";
            this.ProgressGroupBox.TabStop = false;
            // 
            // ExtractionProgressTimer
            // 
            this.ExtractionProgressTimer.Tick += new System.EventHandler(this.ExtractionProgressTimer_Tick);
            // 
            // ButtonsTableLayout
            // 
            resources.ApplyResources(this.ButtonsTableLayout, "ButtonsTableLayout");
            this.ButtonsTableLayout.Controls.Add(this.CancelExtractButton, 1, 2);
            this.ButtonsTableLayout.Controls.Add(this.DestinationGroupBox, 0, 0);
            this.ButtonsTableLayout.Controls.Add(this.ProgressGroupBox, 0, 1);
            this.ButtonsTableLayout.Controls.Add(this.ExtractButton, 0, 2);
            this.ButtonsTableLayout.Name = "ButtonsTableLayout";
            // 
            // extractProgressPanel1
            // 
            resources.ApplyResources(this.extractProgressPanel1, "extractProgressPanel1");
            this.extractProgressPanel1.Name = "extractProgressPanel1";
            // 
            // ExtractItemsDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelExtractButton;
            this.Controls.Add(this.ButtonsTableLayout);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExtractItemsDialog";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ExtractItemsDialog_FormClosing);
            this.DestinationGroupBox.ResumeLayout(false);
            this.DestinationGroupBox.PerformLayout();
            this.DestinationTableLayout.ResumeLayout(false);
            this.DestinationTableLayout.PerformLayout();
            this.ProgressGroupBox.ResumeLayout(false);
            this.ButtonsTableLayout.ResumeLayout(false);
            this.ButtonsTableLayout.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button SelectFolderButton;
        private System.Windows.Forms.TextBox DestinationTextBox;
        private System.Windows.Forms.Button CancelExtractButton;
        private System.Windows.Forms.Button ExtractButton;
        private System.Windows.Forms.GroupBox DestinationGroupBox;
        private System.Windows.Forms.TableLayoutPanel DestinationTableLayout;
        private System.Windows.Forms.GroupBox ProgressGroupBox;
        private System.Windows.Forms.Timer ExtractionProgressTimer;
        private System.Windows.Forms.CheckBox CreateSubDirectoryCheckBox;
        private System.Windows.Forms.TextBox SubDirectoryTextBox;
        private System.Windows.Forms.TableLayoutPanel ButtonsTableLayout;
        private Controls.ExtractProgressPanel extractProgressPanel1;
    }
}