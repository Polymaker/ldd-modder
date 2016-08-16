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
            this.SuspendLayout();
            // 
            // polyEngineView
            // 
            this.polyEngineView.BackColor = System.Drawing.Color.Black;
            this.polyEngineView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.polyEngineView.Location = new System.Drawing.Point(0, 0);
            this.polyEngineView.Name = "polyEngineView";
            this.polyEngineView.Size = new System.Drawing.Size(284, 261);
            this.polyEngineView.TabIndex = 0;
            this.polyEngineView.VSync = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(0, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(69, 27);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ModelViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.polyEngineView);
            this.Name = "ModelViewer";
            this.Text = "ModelViewer";
            this.ResumeLayout(false);

        }

        #endregion

        private Poly3D.Platform.EngineControl polyEngineView;
        private System.Windows.Forms.Button button1;
    }
}