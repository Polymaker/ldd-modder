namespace LDDModder.BrickEditor.UI
{
    partial class ModelImportExportWindow
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
            this.label1 = new System.Windows.Forms.Label();
            this.LddPathTextBox = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ExportObjRadio = new System.Windows.Forms.RadioButton();
            this.ExportDaeRadio = new System.Windows.Forms.RadioButton();
            this.PartIdTextBox = new System.Windows.Forms.TextBox();
            this.ExportPartButton = new System.Windows.Forms.Button();
            this.IncludeBonesCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "LDD Directory";
            // 
            // LddPathTextBox
            // 
            this.LddPathTextBox.Location = new System.Drawing.Point(92, 6);
            this.LddPathTextBox.Name = "LddPathTextBox";
            this.LddPathTextBox.Size = new System.Drawing.Size(297, 20);
            this.LddPathTextBox.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(392, 5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(40, 22);
            this.button1.TabIndex = 2;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.IncludeBonesCheckBox);
            this.groupBox1.Controls.Add(this.ExportPartButton);
            this.groupBox1.Controls.Add(this.PartIdTextBox);
            this.groupBox1.Controls.Add(this.ExportDaeRadio);
            this.groupBox1.Controls.Add(this.ExportObjRadio);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(12, 32);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(194, 236);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Export";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Part ID";
            // 
            // ExportObjRadio
            // 
            this.ExportObjRadio.AutoSize = true;
            this.ExportObjRadio.Location = new System.Drawing.Point(9, 77);
            this.ExportObjRadio.Name = "ExportObjRadio";
            this.ExportObjRadio.Size = new System.Drawing.Size(98, 17);
            this.ExportObjRadio.TabIndex = 1;
            this.ExportObjRadio.Text = "Wavefront (obj)";
            this.ExportObjRadio.UseVisualStyleBackColor = true;
            // 
            // ExportDaeRadio
            // 
            this.ExportDaeRadio.AutoSize = true;
            this.ExportDaeRadio.Checked = true;
            this.ExportDaeRadio.Location = new System.Drawing.Point(9, 100);
            this.ExportDaeRadio.Name = "ExportDaeRadio";
            this.ExportDaeRadio.Size = new System.Drawing.Size(87, 17);
            this.ExportDaeRadio.TabIndex = 2;
            this.ExportDaeRadio.TabStop = true;
            this.ExportDaeRadio.Text = "Collada (dae)";
            this.ExportDaeRadio.UseVisualStyleBackColor = true;
            // 
            // PartIdTextBox
            // 
            this.PartIdTextBox.Location = new System.Drawing.Point(47, 24);
            this.PartIdTextBox.Name = "PartIdTextBox";
            this.PartIdTextBox.Size = new System.Drawing.Size(100, 20);
            this.PartIdTextBox.TabIndex = 3;
            // 
            // ExportPartButton
            // 
            this.ExportPartButton.Location = new System.Drawing.Point(47, 196);
            this.ExportPartButton.Name = "ExportPartButton";
            this.ExportPartButton.Size = new System.Drawing.Size(75, 23);
            this.ExportPartButton.TabIndex = 4;
            this.ExportPartButton.Text = "Export";
            this.ExportPartButton.UseVisualStyleBackColor = true;
            this.ExportPartButton.Click += new System.EventHandler(this.ExportPartButton_Click);
            // 
            // IncludeBonesCheckBox
            // 
            this.IncludeBonesCheckBox.AutoSize = true;
            this.IncludeBonesCheckBox.Checked = true;
            this.IncludeBonesCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.IncludeBonesCheckBox.Location = new System.Drawing.Point(27, 123);
            this.IncludeBonesCheckBox.Name = "IncludeBonesCheckBox";
            this.IncludeBonesCheckBox.Size = new System.Drawing.Size(94, 17);
            this.IncludeBonesCheckBox.TabIndex = 5;
            this.IncludeBonesCheckBox.Text = "Include Bones";
            this.IncludeBonesCheckBox.UseVisualStyleBackColor = true;
            // 
            // ModelImportExportWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(481, 327);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.LddPathTextBox);
            this.Controls.Add(this.label1);
            this.Name = "ModelImportExportWindow";
            this.Text = "ModelImportExportWindow";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox LddPathTextBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button ExportPartButton;
        private System.Windows.Forms.TextBox PartIdTextBox;
        private System.Windows.Forms.RadioButton ExportDaeRadio;
        private System.Windows.Forms.RadioButton ExportObjRadio;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox IncludeBonesCheckBox;
    }
}