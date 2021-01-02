namespace LDDModder.BrickEditor.UI.Windows
{
    partial class InputDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InputDialog));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.Option1Button = new System.Windows.Forms.Button();
            this.Option2Button = new System.Windows.Forms.Button();
            this.MessageTextLabel = new System.Windows.Forms.Label();
            this.MessageIconBox = new System.Windows.Forms.PictureBox();
            this.InputTextBox = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MessageIconBox)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.MessageTextLabel, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.MessageIconBox, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.InputTextBox, 0, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // flowLayoutPanel1
            // 
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.tableLayoutPanel1.SetColumnSpan(this.flowLayoutPanel1, 2);
            this.flowLayoutPanel1.Controls.Add(this.Option1Button);
            this.flowLayoutPanel1.Controls.Add(this.Option2Button);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // Option1Button
            // 
            resources.ApplyResources(this.Option1Button, "Option1Button");
            this.Option1Button.Name = "Option1Button";
            this.Option1Button.UseVisualStyleBackColor = true;
            this.Option1Button.Click += new System.EventHandler(this.Option1Button_Click);
            // 
            // Option2Button
            // 
            this.Option2Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.Option2Button, "Option2Button");
            this.Option2Button.Name = "Option2Button";
            this.Option2Button.UseVisualStyleBackColor = true;
            // 
            // MessageTextLabel
            // 
            resources.ApplyResources(this.MessageTextLabel, "MessageTextLabel");
            this.MessageTextLabel.Name = "MessageTextLabel";
            // 
            // MessageIconBox
            // 
            resources.ApplyResources(this.MessageIconBox, "MessageIconBox");
            this.MessageIconBox.Name = "MessageIconBox";
            this.MessageIconBox.TabStop = false;
            // 
            // InputTextBox
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.InputTextBox, 2);
            resources.ApplyResources(this.InputTextBox, "InputTextBox");
            this.InputTextBox.Name = "InputTextBox";
            // 
            // InputDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InputDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
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
        private System.Windows.Forms.Label MessageTextLabel;
        private System.Windows.Forms.PictureBox MessageIconBox;
        private System.Windows.Forms.TextBox InputTextBox;
    }
}