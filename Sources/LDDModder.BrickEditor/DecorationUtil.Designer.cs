namespace LDDModder.BrickEditor
{
    partial class DecorationUtil
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
            this.glControl1 = new OpenTK.GLControl();
            this.SelectFileButton = new System.Windows.Forms.Button();
            this.TriangleListBox = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.CreateMeshButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // glControl1
            // 
            this.glControl1.BackColor = System.Drawing.Color.SkyBlue;
            this.glControl1.Location = new System.Drawing.Point(277, 12);
            this.glControl1.Name = "glControl1";
            this.glControl1.Size = new System.Drawing.Size(299, 254);
            this.glControl1.TabIndex = 0;
            this.glControl1.VSync = false;
            // 
            // SelectFileButton
            // 
            this.SelectFileButton.Location = new System.Drawing.Point(12, 12);
            this.SelectFileButton.Name = "SelectFileButton";
            this.SelectFileButton.Size = new System.Drawing.Size(130, 23);
            this.SelectFileButton.TabIndex = 1;
            this.SelectFileButton.Text = "Select LDD Mesh…";
            this.SelectFileButton.UseVisualStyleBackColor = true;
            this.SelectFileButton.Click += new System.EventHandler(this.SelectFileButton_Click);
            // 
            // TriangleListBox
            // 
            this.TriangleListBox.FormattingEnabled = true;
            this.TriangleListBox.Location = new System.Drawing.Point(12, 54);
            this.TriangleListBox.Name = "TriangleListBox";
            this.TriangleListBox.Size = new System.Drawing.Size(259, 214);
            this.TriangleListBox.TabIndex = 2;
            this.TriangleListBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TriangleListBox_MouseMove);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Select triangles:";
            // 
            // CreateMeshButton
            // 
            this.CreateMeshButton.Location = new System.Drawing.Point(12, 274);
            this.CreateMeshButton.Name = "CreateMeshButton";
            this.CreateMeshButton.Size = new System.Drawing.Size(151, 23);
            this.CreateMeshButton.TabIndex = 4;
            this.CreateMeshButton.Text = "Create Decoration Mesh";
            this.CreateMeshButton.UseVisualStyleBackColor = true;
            this.CreateMeshButton.Click += new System.EventHandler(this.CreateMeshButton_Click);
            // 
            // DecorationUtil
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(587, 311);
            this.Controls.Add(this.CreateMeshButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TriangleListBox);
            this.Controls.Add(this.SelectFileButton);
            this.Controls.Add(this.glControl1);
            this.Name = "DecorationUtil";
            this.Text = "DecorationUtil";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OpenTK.GLControl glControl1;
        private System.Windows.Forms.Button SelectFileButton;
        private System.Windows.Forms.CheckedListBox TriangleListBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button CreateMeshButton;
    }
}