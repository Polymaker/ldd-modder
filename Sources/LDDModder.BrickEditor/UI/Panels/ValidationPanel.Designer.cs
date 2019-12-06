namespace LDDModder.BrickEditor.UI.Panels
{
    partial class ValidationPanel
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
            this.button1 = new System.Windows.Forms.Button();
            this.collapsiblePanel1 = new LDDModder.BrickEditor.UI.Controls.CollapsiblePanel();
            this.label1 = new System.Windows.Forms.Label();
            this.collapsiblePanel2 = new LDDModder.BrickEditor.UI.Controls.CollapsiblePanel();
            this.label2 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.collapsiblePanel1.ContentPanel.SuspendLayout();
            this.collapsiblePanel1.SuspendLayout();
            this.collapsiblePanel2.ContentPanel.SuspendLayout();
            this.collapsiblePanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(3, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // collapsiblePanel1
            // 
            this.collapsiblePanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            // 
            // collapsiblePanel1.ContentPanel
            // 
            this.collapsiblePanel1.ContentPanel.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.collapsiblePanel1.ContentPanel.Controls.Add(this.label1);
            this.collapsiblePanel1.ContentPanel.Controls.Add(this.button1);
            this.collapsiblePanel1.ContentPanel.Location = new System.Drawing.Point(3, 19);
            this.collapsiblePanel1.ContentPanel.Name = "ContentPanel";
            this.collapsiblePanel1.ContentPanel.Size = new System.Drawing.Size(453, 93);
            this.collapsiblePanel1.ContentPanel.TabIndex = 0;
            this.collapsiblePanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.collapsiblePanel1.Location = new System.Drawing.Point(0, 0);
            this.collapsiblePanel1.Name = "collapsiblePanel1";
            this.collapsiblePanel1.Padding = new System.Windows.Forms.Padding(3, 19, 3, 3);
            this.collapsiblePanel1.PanelHeight = 95;
            this.collapsiblePanel1.Size = new System.Drawing.Size(461, 117);
            this.collapsiblePanel1.TabIndex = 1;
            this.collapsiblePanel1.Text = "collapsiblePanel1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "label1";
            // 
            // collapsiblePanel2
            // 
            this.collapsiblePanel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            // 
            // collapsiblePanel2.ContentPanel
            // 
            this.collapsiblePanel2.ContentPanel.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.collapsiblePanel2.ContentPanel.Controls.Add(this.label2);
            this.collapsiblePanel2.ContentPanel.Controls.Add(this.button2);
            this.collapsiblePanel2.ContentPanel.Location = new System.Drawing.Point(3, 19);
            this.collapsiblePanel2.ContentPanel.Name = "ContentPanel";
            this.collapsiblePanel2.ContentPanel.Size = new System.Drawing.Size(453, 90);
            this.collapsiblePanel2.ContentPanel.TabIndex = 0;
            this.collapsiblePanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.collapsiblePanel2.Location = new System.Drawing.Point(0, 117);
            this.collapsiblePanel2.Name = "collapsiblePanel2";
            this.collapsiblePanel2.Padding = new System.Windows.Forms.Padding(3, 19, 3, 3);
            this.collapsiblePanel2.PanelHeight = 92;
            this.collapsiblePanel2.Size = new System.Drawing.Size(461, 114);
            this.collapsiblePanel2.TabIndex = 2;
            this.collapsiblePanel2.Text = "collapsiblePanel2";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "label2";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(3, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 0;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // ValidationPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(461, 269);
            this.Controls.Add(this.collapsiblePanel2);
            this.Controls.Add(this.collapsiblePanel1);
            this.Name = "ValidationPanel";
            this.Text = "Validation Results";
            this.collapsiblePanel1.ContentPanel.ResumeLayout(false);
            this.collapsiblePanel1.ContentPanel.PerformLayout();
            this.collapsiblePanel1.ResumeLayout(false);
            this.collapsiblePanel2.ContentPanel.ResumeLayout(false);
            this.collapsiblePanel2.ContentPanel.PerformLayout();
            this.collapsiblePanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private Controls.CollapsiblePanel collapsiblePanel1;
        private System.Windows.Forms.Label label1;
        private Controls.CollapsiblePanel collapsiblePanel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button2;
    }
}