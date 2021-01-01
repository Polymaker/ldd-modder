namespace LDDModder.BrickEditor.UI.Windows
{
    partial class AboutWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutWindow));
            this.CopyrightLabel = new System.Windows.Forms.Label();
            this.ExternalInfoTextbox = new System.Windows.Forms.RichTextBox();
            this.AppVersionLabel = new System.Windows.Forms.Label();
            this.AppTitleLabel = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.ReturnButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // CopyrightLabel
            // 
            resources.ApplyResources(this.CopyrightLabel, "CopyrightLabel");
            this.CopyrightLabel.Name = "CopyrightLabel";
            // 
            // ExternalInfoTextbox
            // 
            resources.ApplyResources(this.ExternalInfoTextbox, "ExternalInfoTextbox");
            this.ExternalInfoTextbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ExternalInfoTextbox.Name = "ExternalInfoTextbox";
            this.ExternalInfoTextbox.ReadOnly = true;
            this.ExternalInfoTextbox.HScroll += new System.EventHandler(this.ExternalInfoTextbox_ScrollOrResized);
            this.ExternalInfoTextbox.VScroll += new System.EventHandler(this.ExternalInfoTextbox_ScrollOrResized);
            this.ExternalInfoTextbox.SizeChanged += new System.EventHandler(this.ExternalInfoTextbox_ScrollOrResized);
            // 
            // AppVersionLabel
            // 
            resources.ApplyResources(this.AppVersionLabel, "AppVersionLabel");
            this.AppVersionLabel.Name = "AppVersionLabel";
            // 
            // AppTitleLabel
            // 
            resources.ApplyResources(this.AppTitleLabel, "AppTitleLabel");
            this.AppTitleLabel.Name = "AppTitleLabel";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::LDDModder.BrickEditor.Properties.Resources.ldd_editor_x64;
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            // 
            // ReturnButton
            // 
            resources.ApplyResources(this.ReturnButton, "ReturnButton");
            this.ReturnButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ReturnButton.Name = "ReturnButton";
            this.ReturnButton.UseVisualStyleBackColor = true;
            // 
            // AboutWindow
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.CopyrightLabel);
            this.Controls.Add(this.ExternalInfoTextbox);
            this.Controls.Add(this.AppVersionLabel);
            this.Controls.Add(this.AppTitleLabel);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.ReturnButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutWindow";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label CopyrightLabel;
        private System.Windows.Forms.RichTextBox ExternalInfoTextbox;
        private System.Windows.Forms.Label AppVersionLabel;
        private System.Windows.Forms.Label AppTitleLabel;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button ReturnButton;
    }
}