namespace LDDModder.PaletteMaker.Controls
{
    partial class VersionEdit
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
            this.txtMajor = new System.Windows.Forms.TextBox();
            this.txtMinor = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtMajor
            // 
            this.txtMajor.Location = new System.Drawing.Point(0, 0);
            this.txtMajor.Name = "txtMajor";
            this.txtMajor.Size = new System.Drawing.Size(30, 20);
            this.txtMajor.TabIndex = 0;
            this.txtMajor.Text = "1";
            this.txtMajor.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtMajor.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.NumberTextbox_KeyPress);
            // 
            // txtMinor
            // 
            this.txtMinor.Location = new System.Drawing.Point(36, 0);
            this.txtMinor.Name = "txtMinor";
            this.txtMinor.Size = new System.Drawing.Size(30, 20);
            this.txtMinor.TabIndex = 1;
            this.txtMinor.Text = "0";
            this.txtMinor.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtMinor.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.NumberTextbox_KeyPress);
            // 
            // VersionEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtMinor);
            this.Controls.Add(this.txtMajor);
            this.Name = "VersionEdit";
            this.Size = new System.Drawing.Size(81, 29);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtMajor;
        private System.Windows.Forms.TextBox txtMinor;

    }
}
