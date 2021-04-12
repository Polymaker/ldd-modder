namespace LDDModder.BrickEditor.UI
{
    partial class FlexAttributesDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FlexAttributesDialog));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.ValueBox1 = new LDDModder.BrickEditor.UI.Controls.NumberTextBox();
            this.ValueBox2 = new LDDModder.BrickEditor.UI.Controls.NumberTextBox();
            this.ValueBox3 = new LDDModder.BrickEditor.UI.Controls.NumberTextBox();
            this.ValueBox4 = new LDDModder.BrickEditor.UI.Controls.NumberTextBox();
            this.ValueBox5 = new LDDModder.BrickEditor.UI.Controls.NumberTextBox();
            this.FlexAmountLabel = new System.Windows.Forms.Label();
            this.ActionButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.ValueBox1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.ValueBox2, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.ValueBox3, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.ValueBox4, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.ValueBox5, 4, 1);
            this.tableLayoutPanel1.Controls.Add(this.FlexAmountLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.ActionButton, 3, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // ValueBox1
            // 
            resources.ApplyResources(this.ValueBox1, "ValueBox1");
            this.ValueBox1.MaximumValue = 500D;
            this.ValueBox1.MinimumValue = -500D;
            this.ValueBox1.Name = "ValueBox1";
            // 
            // ValueBox2
            // 
            resources.ApplyResources(this.ValueBox2, "ValueBox2");
            this.ValueBox2.MaximumValue = 500D;
            this.ValueBox2.MinimumValue = -500D;
            this.ValueBox2.Name = "ValueBox2";
            // 
            // ValueBox3
            // 
            resources.ApplyResources(this.ValueBox3, "ValueBox3");
            this.ValueBox3.MaximumValue = 500D;
            this.ValueBox3.MinimumValue = -500D;
            this.ValueBox3.Name = "ValueBox3";
            // 
            // ValueBox4
            // 
            resources.ApplyResources(this.ValueBox4, "ValueBox4");
            this.ValueBox4.MaximumValue = 500D;
            this.ValueBox4.MinimumValue = -500D;
            this.ValueBox4.Name = "ValueBox4";
            // 
            // ValueBox5
            // 
            resources.ApplyResources(this.ValueBox5, "ValueBox5");
            this.ValueBox5.MaximumValue = 500D;
            this.ValueBox5.MinimumValue = -500D;
            this.ValueBox5.Name = "ValueBox5";
            // 
            // FlexAmountLabel
            // 
            resources.ApplyResources(this.FlexAmountLabel, "FlexAmountLabel");
            this.tableLayoutPanel1.SetColumnSpan(this.FlexAmountLabel, 5);
            this.FlexAmountLabel.Name = "FlexAmountLabel";
            // 
            // ActionButton
            // 
            resources.ApplyResources(this.ActionButton, "ActionButton");
            this.tableLayoutPanel1.SetColumnSpan(this.ActionButton, 2);
            this.ActionButton.Name = "ActionButton";
            this.ActionButton.UseVisualStyleBackColor = true;
            this.ActionButton.Click += new System.EventHandler(this.ActionButton_Click);
            // 
            // FlexAttributesDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FlexAttributesDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Controls.NumberTextBox ValueBox1;
        private Controls.NumberTextBox ValueBox2;
        private Controls.NumberTextBox ValueBox3;
        private Controls.NumberTextBox ValueBox4;
        private Controls.NumberTextBox ValueBox5;
        private System.Windows.Forms.Label FlexAmountLabel;
        private System.Windows.Forms.Button ActionButton;
    }
}