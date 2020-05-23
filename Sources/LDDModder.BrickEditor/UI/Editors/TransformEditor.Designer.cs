namespace LDDModder.BrickEditor.UI.Controls
{
    partial class TransformEditor
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TransformEditor));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.PositionLabel = new System.Windows.Forms.Label();
            this.RotationLabel = new System.Windows.Forms.Label();
            this.PosX_TextBox = new LDDModder.BrickEditor.UI.Controls.NumberTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.PosY_TextBox = new LDDModder.BrickEditor.UI.Controls.NumberTextBox();
            this.PosZ_TextBox = new LDDModder.BrickEditor.UI.Controls.NumberTextBox();
            this.RotX_TextBox = new LDDModder.BrickEditor.UI.Controls.NumberTextBox();
            this.RotY_TextBox = new LDDModder.BrickEditor.UI.Controls.NumberTextBox();
            this.RotZ_TextBox = new LDDModder.BrickEditor.UI.Controls.NumberTextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.PositionLabel, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.RotationLabel, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.PosX_TextBox, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label4, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.label5, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.PosY_TextBox, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.PosZ_TextBox, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.RotX_TextBox, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.RotY_TextBox, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.RotZ_TextBox, 3, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // PositionLabel
            // 
            resources.ApplyResources(this.PositionLabel, "PositionLabel");
            this.PositionLabel.Name = "PositionLabel";
            // 
            // RotationLabel
            // 
            resources.ApplyResources(this.RotationLabel, "RotationLabel");
            this.RotationLabel.Name = "RotationLabel";
            // 
            // PosX_TextBox
            // 
            resources.ApplyResources(this.PosX_TextBox, "PosX_TextBox");
            this.PosX_TextBox.MaximumValue = 5000D;
            this.PosX_TextBox.MinimumValue = -5000D;
            this.PosX_TextBox.Name = "PosX_TextBox";
            this.PosX_TextBox.ValueChanged += new System.EventHandler(this.PositionValues_ValueChanged);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // PosY_TextBox
            // 
            resources.ApplyResources(this.PosY_TextBox, "PosY_TextBox");
            this.PosY_TextBox.MaximumValue = 5000D;
            this.PosY_TextBox.MinimumValue = -5000D;
            this.PosY_TextBox.Name = "PosY_TextBox";
            this.PosY_TextBox.ValueChanged += new System.EventHandler(this.PositionValues_ValueChanged);
            // 
            // PosZ_TextBox
            // 
            resources.ApplyResources(this.PosZ_TextBox, "PosZ_TextBox");
            this.PosZ_TextBox.MaximumValue = 5000D;
            this.PosZ_TextBox.MinimumValue = -5000D;
            this.PosZ_TextBox.Name = "PosZ_TextBox";
            this.PosZ_TextBox.ValueChanged += new System.EventHandler(this.PositionValues_ValueChanged);
            // 
            // RotX_TextBox
            // 
            resources.ApplyResources(this.RotX_TextBox, "RotX_TextBox");
            this.RotX_TextBox.MaximumValue = 360D;
            this.RotX_TextBox.MinimumValue = -360D;
            this.RotX_TextBox.Name = "RotX_TextBox";
            this.RotX_TextBox.ValueChanged += new System.EventHandler(this.RotationValues_ValueChanged);
            // 
            // RotY_TextBox
            // 
            resources.ApplyResources(this.RotY_TextBox, "RotY_TextBox");
            this.RotY_TextBox.MaximumValue = 360D;
            this.RotY_TextBox.MinimumValue = -360D;
            this.RotY_TextBox.Name = "RotY_TextBox";
            this.RotY_TextBox.ValueChanged += new System.EventHandler(this.RotationValues_ValueChanged);
            // 
            // RotZ_TextBox
            // 
            resources.ApplyResources(this.RotZ_TextBox, "RotZ_TextBox");
            this.RotZ_TextBox.MaximumValue = 360D;
            this.RotZ_TextBox.MinimumValue = -360D;
            this.RotZ_TextBox.Name = "RotZ_TextBox";
            this.RotZ_TextBox.ValueChanged += new System.EventHandler(this.RotationValues_ValueChanged);
            // 
            // TransformEditor
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "TransformEditor";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label PositionLabel;
        private System.Windows.Forms.Label RotationLabel;
        private NumberTextBox PosX_TextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private NumberTextBox PosY_TextBox;
        private NumberTextBox PosZ_TextBox;
        private NumberTextBox RotX_TextBox;
        private NumberTextBox RotY_TextBox;
        private NumberTextBox RotZ_TextBox;
    }
}
