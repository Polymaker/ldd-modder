namespace LDDModder.BrickEditor.UI.Panels
{
    partial class ElementDetailPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ElementDetailPanel));
            this.NameTextBox = new System.Windows.Forms.TextBox();
            this.CommentTextBox = new System.Windows.Forms.TextBox();
            this.SelectionTypeLabel = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.SelectionLabel = new System.Windows.Forms.Label();
            this.SelectionTransformEdit = new LDDModder.BrickEditor.UI.Controls.TransformEditor();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // NameTextBox
            // 
            resources.ApplyResources(this.NameTextBox, "NameTextBox");
            this.NameTextBox.Name = "NameTextBox";
            this.NameTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.NameTextBox_Validating);
            // 
            // CommentTextBox
            // 
            resources.ApplyResources(this.CommentTextBox, "CommentTextBox");
            this.CommentTextBox.Name = "CommentTextBox";
            // 
            // SelectionTypeLabel
            // 
            resources.ApplyResources(this.SelectionTypeLabel, "SelectionTypeLabel");
            this.SelectionTypeLabel.Name = "SelectionTypeLabel";
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.SelectionLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.SelectionTransformEdit, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.CommentTextBox, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.SelectionTypeLabel, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.NameTextBox, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // SelectionLabel
            // 
            resources.ApplyResources(this.SelectionLabel, "SelectionLabel");
            this.SelectionLabel.Name = "SelectionLabel";
            // 
            // SelectionTransformEdit
            // 
            resources.ApplyResources(this.SelectionTransformEdit, "SelectionTransformEdit");
            this.tableLayoutPanel1.SetColumnSpan(this.SelectionTransformEdit, 2);
            this.SelectionTransformEdit.Name = "SelectionTransformEdit";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // ElementDetailPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ElementDetailPanel";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private Controls.TransformEditor SelectionTransformEdit;
        private System.Windows.Forms.TextBox NameTextBox;
        private System.Windows.Forms.TextBox CommentTextBox;
        private System.Windows.Forms.Label SelectionTypeLabel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label SelectionLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}