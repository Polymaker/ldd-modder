namespace LDDModder.BrickEditor.UI.Windows
{
    partial class SelectOutputLocationWindow
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
            this.MessageLabel = new System.Windows.Forms.Label();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.browseTextBox1 = new LDDModder.BrickEditor.UI.Controls.BrowseTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // MessageLabel
            // 
            this.MessageLabel.AutoSize = true;
            this.MessageLabel.Location = new System.Drawing.Point(12, 9);
            this.MessageLabel.Name = "MessageLabel";
            this.MessageLabel.Size = new System.Drawing.Size(174, 13);
            this.MessageLabel.TabIndex = 0;
            this.MessageLabel.Text = "Choose where to generate the files:";
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(15, 27);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(125, 17);
            this.radioButton1.TabIndex = 1;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Directly in LDD folder";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(15, 50);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(99, 17);
            this.radioButton2.TabIndex = 2;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Selected folder:";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // browseTextBox1
            // 
            this.browseTextBox1.AutoSizeButton = true;
            this.browseTextBox1.ButtonText = "Select";
            this.browseTextBox1.ButtonWidth = 47;
            this.browseTextBox1.Location = new System.Drawing.Point(15, 73);
            this.browseTextBox1.Name = "browseTextBox1";
            this.browseTextBox1.Size = new System.Drawing.Size(260, 20);
            this.browseTextBox1.TabIndex = 3;
            this.browseTextBox1.Value = "";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(14, 98);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(93, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Generate files";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // SelectOutputLocationWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(287, 133);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.browseTextBox1);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.MessageLabel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectOutputLocationWindow";
            this.ShowIcon = false;
            this.Text = "Select destination";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label MessageLabel;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private Controls.BrowseTextBox browseTextBox1;
        private System.Windows.Forms.Button button1;
    }
}