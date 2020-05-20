namespace LDDModder.LifExtractor.Windows
{
    partial class CreateFolderDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreateFolderDialog));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.FolderNameTextBox = new System.Windows.Forms.TextBox();
            this.ReturnButton = new System.Windows.Forms.Button();
            this.ValidateButton = new System.Windows.Forms.Button();
            this.FolderNameLabel = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.FolderNameTextBox, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.ReturnButton, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.ValidateButton, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.FolderNameLabel, 0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // FolderNameTextBox
            // 
            resources.ApplyResources(this.FolderNameTextBox, "FolderNameTextBox");
            this.tableLayoutPanel1.SetColumnSpan(this.FolderNameTextBox, 2);
            this.FolderNameTextBox.Name = "FolderNameTextBox";
            // 
            // ReturnButton
            // 
            resources.ApplyResources(this.ReturnButton, "ReturnButton");
            this.ReturnButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ReturnButton.Name = "ReturnButton";
            this.ReturnButton.UseVisualStyleBackColor = true;
            // 
            // ValidateButton
            // 
            resources.ApplyResources(this.ValidateButton, "ValidateButton");
            this.ValidateButton.Name = "ValidateButton";
            this.ValidateButton.UseVisualStyleBackColor = true;
            this.ValidateButton.Click += new System.EventHandler(this.ValidateButton_Click);
            // 
            // FolderNameLabel
            // 
            resources.ApplyResources(this.FolderNameLabel, "FolderNameLabel");
            this.tableLayoutPanel1.SetColumnSpan(this.FolderNameLabel, 2);
            this.FolderNameLabel.Name = "FolderNameLabel";
            // 
            // CreateFolderDialog
            // 
            this.AcceptButton = this.ValidateButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ReturnButton;
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CreateFolderDialog";
            this.ShowInTaskbar = false;
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox FolderNameTextBox;
        private System.Windows.Forms.Button ReturnButton;
        private System.Windows.Forms.Button ValidateButton;
        private System.Windows.Forms.Label FolderNameLabel;
    }
}