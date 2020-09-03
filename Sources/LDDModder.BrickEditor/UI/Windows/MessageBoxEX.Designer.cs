namespace LDDModder.BrickEditor.UI.Windows
{
    partial class MessageBoxEX
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.Option1Button = new System.Windows.Forms.Button();
            this.Option2Button = new System.Windows.Forms.Button();
            this.Option3Button = new System.Windows.Forms.Button();
            this.ErrorDetailTextBox = new System.Windows.Forms.TextBox();
            this.MessageTextLabel = new System.Windows.Forms.Label();
            this.MessageIconBox = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MessageIconBox)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.ErrorDetailTextBox, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.MessageTextLabel, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.MessageIconBox, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(328, 170);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.SetColumnSpan(this.flowLayoutPanel1, 2);
            this.flowLayoutPanel1.Controls.Add(this.Option1Button);
            this.flowLayoutPanel1.Controls.Add(this.Option2Button);
            this.flowLayoutPanel1.Controls.Add(this.Option3Button);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(85, 141);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(243, 29);
            this.flowLayoutPanel1.TabIndex = 4;
            // 
            // Option1Button
            // 
            this.Option1Button.Location = new System.Drawing.Point(3, 3);
            this.Option1Button.Name = "Option1Button";
            this.Option1Button.Size = new System.Drawing.Size(75, 23);
            this.Option1Button.TabIndex = 1;
            this.Option1Button.Text = "1";
            this.Option1Button.UseVisualStyleBackColor = true;
            // 
            // Option2Button
            // 
            this.Option2Button.Location = new System.Drawing.Point(84, 3);
            this.Option2Button.Name = "Option2Button";
            this.Option2Button.Size = new System.Drawing.Size(75, 23);
            this.Option2Button.TabIndex = 2;
            this.Option2Button.Text = "2";
            this.Option2Button.UseVisualStyleBackColor = true;
            // 
            // Option3Button
            // 
            this.Option3Button.Location = new System.Drawing.Point(165, 3);
            this.Option3Button.Name = "Option3Button";
            this.Option3Button.Size = new System.Drawing.Size(75, 23);
            this.Option3Button.TabIndex = 3;
            this.Option3Button.Text = "3";
            this.Option3Button.UseVisualStyleBackColor = true;
            // 
            // ErrorDetailTextBox
            // 
            this.ErrorDetailTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.ErrorDetailTextBox, 2);
            this.ErrorDetailTextBox.Location = new System.Drawing.Point(3, 41);
            this.ErrorDetailTextBox.Multiline = true;
            this.ErrorDetailTextBox.Name = "ErrorDetailTextBox";
            this.ErrorDetailTextBox.ReadOnly = true;
            this.ErrorDetailTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ErrorDetailTextBox.Size = new System.Drawing.Size(322, 97);
            this.ErrorDetailTextBox.TabIndex = 5;
            // 
            // MessageTextLabel
            // 
            this.MessageTextLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.MessageTextLabel.AutoSize = true;
            this.MessageTextLabel.Location = new System.Drawing.Point(41, 12);
            this.MessageTextLabel.Margin = new System.Windows.Forms.Padding(3);
            this.MessageTextLabel.Name = "MessageTextLabel";
            this.MessageTextLabel.Size = new System.Drawing.Size(35, 13);
            this.MessageTextLabel.TabIndex = 0;
            this.MessageTextLabel.Text = "label1";
            // 
            // MessageIconBox
            // 
            this.MessageIconBox.Location = new System.Drawing.Point(3, 3);
            this.MessageIconBox.Name = "MessageIconBox";
            this.MessageIconBox.Size = new System.Drawing.Size(32, 32);
            this.MessageIconBox.TabIndex = 6;
            this.MessageIconBox.TabStop = false;
            // 
            // MessageBoxEX
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 176);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MessageBoxEX";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "ErrorMessageBox";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MessageIconBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button Option1Button;
        private System.Windows.Forms.Button Option2Button;
        private System.Windows.Forms.Button Option3Button;
        private System.Windows.Forms.TextBox ErrorDetailTextBox;
        private System.Windows.Forms.Label MessageTextLabel;
        private System.Windows.Forms.PictureBox MessageIconBox;
    }
}