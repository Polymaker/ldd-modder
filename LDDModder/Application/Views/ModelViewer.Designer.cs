namespace LDDModder.Views
{
    partial class ModelViewer
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
            this.polyEngineView = new Poly3D.Platform.EngineControl();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.ShowCollisionsCheckBox = new System.Windows.Forms.CheckBox();
            this.ShowConnectionsCheckBox = new System.Windows.Forms.CheckBox();
            this.HideMeshCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // polyEngineView
            // 
            this.polyEngineView.BackColor = System.Drawing.Color.SkyBlue;
            this.polyEngineView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.polyEngineView.Location = new System.Drawing.Point(0, 0);
            this.polyEngineView.Name = "polyEngineView";
            this.polyEngineView.Size = new System.Drawing.Size(672, 479);
            this.polyEngineView.TabIndex = 0;
            this.polyEngineView.VSync = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(0, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(69, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Previous";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(75, 2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "Next";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // ShowCollisionsCheckBox
            // 
            this.ShowCollisionsCheckBox.AutoSize = true;
            this.ShowCollisionsCheckBox.Location = new System.Drawing.Point(156, 6);
            this.ShowCollisionsCheckBox.Name = "ShowCollisionsCheckBox";
            this.ShowCollisionsCheckBox.Size = new System.Drawing.Size(99, 17);
            this.ShowCollisionsCheckBox.TabIndex = 3;
            this.ShowCollisionsCheckBox.Text = "Show Collisions";
            this.ShowCollisionsCheckBox.UseVisualStyleBackColor = true;
            this.ShowCollisionsCheckBox.CheckedChanged += new System.EventHandler(this.ShowCollisionsCheckBox_CheckedChanged);
            // 
            // ShowConnectionsCheckBox
            // 
            this.ShowConnectionsCheckBox.AutoSize = true;
            this.ShowConnectionsCheckBox.Location = new System.Drawing.Point(261, 6);
            this.ShowConnectionsCheckBox.Name = "ShowConnectionsCheckBox";
            this.ShowConnectionsCheckBox.Size = new System.Drawing.Size(115, 17);
            this.ShowConnectionsCheckBox.TabIndex = 4;
            this.ShowConnectionsCheckBox.Text = "Show Connections";
            this.ShowConnectionsCheckBox.UseVisualStyleBackColor = true;
            this.ShowConnectionsCheckBox.CheckedChanged += new System.EventHandler(this.ShowConnectionsCheckBox_CheckedChanged);
            // 
            // HideMeshCheckBox
            // 
            this.HideMeshCheckBox.AutoSize = true;
            this.HideMeshCheckBox.Location = new System.Drawing.Point(382, 6);
            this.HideMeshCheckBox.Name = "HideMeshCheckBox";
            this.HideMeshCheckBox.Size = new System.Drawing.Size(77, 17);
            this.HideMeshCheckBox.TabIndex = 5;
            this.HideMeshCheckBox.Text = "Hide Mesh";
            this.HideMeshCheckBox.UseVisualStyleBackColor = true;
            this.HideMeshCheckBox.CheckedChanged += new System.EventHandler(this.HideMeshCheckBox_CheckedChanged);
            // 
            // ModelViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SkyBlue;
            this.ClientSize = new System.Drawing.Size(672, 479);
            this.Controls.Add(this.HideMeshCheckBox);
            this.Controls.Add(this.ShowConnectionsCheckBox);
            this.Controls.Add(this.ShowCollisionsCheckBox);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.polyEngineView);
            this.Name = "ModelViewer";
            this.Text = "Brick Model Viewer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Poly3D.Platform.EngineControl polyEngineView;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox ShowCollisionsCheckBox;
        private System.Windows.Forms.CheckBox ShowConnectionsCheckBox;
        private System.Windows.Forms.CheckBox HideMeshCheckBox;
    }
}