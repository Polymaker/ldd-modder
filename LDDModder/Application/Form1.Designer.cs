namespace LDDModder
{
    partial class Form1
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
            this.custom2dFieldEditor1 = new LDDModder.Display.Controls.Custom2dFieldEditor();
            this.buttonEdit1 = new LDDModder.Display.Controls.ButtonEdit();
            this.SuspendLayout();
            // 
            // custom2dFieldEditor1
            // 
            this.custom2dFieldEditor1.EditValue = null;
            this.custom2dFieldEditor1.Location = new System.Drawing.Point(12, 83);
            this.custom2dFieldEditor1.Name = "custom2dFieldEditor1";
            this.custom2dFieldEditor1.Size = new System.Drawing.Size(221, 140);
            this.custom2dFieldEditor1.TabIndex = 2;
            // 
            // buttonEdit1
            // 
            this.buttonEdit1.Location = new System.Drawing.Point(12, 24);
            this.buttonEdit1.Name = "buttonEdit1";
            this.buttonEdit1.Size = new System.Drawing.Size(181, 20);
            this.buttonEdit1.TabIndex = 3;
            this.buttonEdit1.ButtonClicked += new System.EventHandler(this.buttonEdit1_ButtonClicked);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.buttonEdit1);
            this.Controls.Add(this.custom2dFieldEditor1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion
        private Display.Controls.Custom2dFieldEditor custom2dFieldEditor1;
        private LDDModder.Display.Controls.ButtonEdit buttonEdit1;
    }
}

