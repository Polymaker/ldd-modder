namespace LDDModder.BrickEditor.UI.Controls
{
    partial class BrowseTextBox
    {
        /// <summary> 
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.ValueTextBox = new System.Windows.Forms.TextBox();
            this.BrowseButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ValueTextBox
            // 
            this.ValueTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ValueTextBox.Location = new System.Drawing.Point(0, 0);
            this.ValueTextBox.Margin = new System.Windows.Forms.Padding(0);
            this.ValueTextBox.Name = "ValueTextBox";
            this.ValueTextBox.Size = new System.Drawing.Size(155, 20);
            this.ValueTextBox.TabIndex = 0;
            this.ValueTextBox.FontChanged += new System.EventHandler(this.Controls_FontChanged);
            this.ValueTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.ValueTextBox_Validating);
            this.ValueTextBox.Validated += new System.EventHandler(this.ValueTextBox_Validated);
            // 
            // BrowseButton
            // 
            this.BrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BrowseButton.Location = new System.Drawing.Point(153, -1);
            this.BrowseButton.Margin = new System.Windows.Forms.Padding(0);
            this.BrowseButton.Name = "BrowseButton";
            this.BrowseButton.Size = new System.Drawing.Size(75, 22);
            this.BrowseButton.TabIndex = 1;
            this.BrowseButton.Text = "Browse";
            this.BrowseButton.UseVisualStyleBackColor = true;
            this.BrowseButton.FontChanged += new System.EventHandler(this.Controls_FontChanged);
            this.BrowseButton.Click += new System.EventHandler(this.BrowseButton_Click);
            // 
            // BrowseTextBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ValueTextBox);
            this.Controls.Add(this.BrowseButton);
            this.Name = "BrowseTextBox";
            this.Size = new System.Drawing.Size(227, 22);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ValueTextBox;
        private System.Windows.Forms.Button BrowseButton;
    }
}
