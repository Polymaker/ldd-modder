﻿namespace LDDModder.BrickEditor.UI.Panels
{
    partial class PartPropertiesPanel
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
            LDDModder.LDD.Primitives.BoundingBox boundingBox1 = new LDDModder.LDD.Primitives.BoundingBox();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PartPropertiesPanel));
            LDDModder.LDD.Primitives.BoundingBox boundingBox2 = new LDDModder.LDD.Primitives.BoundingBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.DescriptionTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.PlatformComboBox = new System.Windows.Forms.ComboBox();
            this.CategoryComboBox = new System.Windows.Forms.ComboBox();
            this.PartIDTextBox = new LDDModder.BrickEditor.UI.Controls.NumberTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.boundingBoxEditor2 = new LDDModder.BrickEditor.UI.Controls.BoundingBoxEditor();
            this.CalculateBoundingButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.boundingBoxEditor1 = new LDDModder.BrickEditor.UI.Controls.BoundingBoxEditor();
            this.button1 = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 85F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.DescriptionTextBox, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.PlatformComboBox, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.CategoryComboBox, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.PartIDTextBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.boundingBoxEditor2, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.CalculateBoundingButton, 2, 6);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.boundingBoxEditor1, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.button1, 2, 8);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.MaximumSize = new System.Drawing.Size(400, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 10;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 17F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(400, 272);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Platform";
            // 
            // DescriptionTextBox
            // 
            this.DescriptionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.DescriptionTextBox, 3);
            this.DescriptionTextBox.Location = new System.Drawing.Point(3, 46);
            this.DescriptionTextBox.Name = "DescriptionTextBox";
            this.DescriptionTextBox.Size = new System.Drawing.Size(394, 20);
            this.DescriptionTextBox.TabIndex = 1;
            this.DescriptionTextBox.Validated += new System.EventHandler(this.DescriptionTextBox_Validated);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Part ID";
            // 
            // PlatformComboBox
            // 
            this.PlatformComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.PlatformComboBox, 2);
            this.PlatformComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PlatformComboBox.FormattingEnabled = true;
            this.PlatformComboBox.Location = new System.Drawing.Point(88, 72);
            this.PlatformComboBox.Name = "PlatformComboBox";
            this.PlatformComboBox.Size = new System.Drawing.Size(309, 21);
            this.PlatformComboBox.TabIndex = 4;
            this.PlatformComboBox.SelectedValueChanged += new System.EventHandler(this.PlatformComboBox_SelectedValueChanged);
            // 
            // CategoryComboBox
            // 
            this.CategoryComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.CategoryComboBox, 3);
            this.CategoryComboBox.FormattingEnabled = true;
            this.CategoryComboBox.Location = new System.Drawing.Point(3, 112);
            this.CategoryComboBox.Name = "CategoryComboBox";
            this.CategoryComboBox.Size = new System.Drawing.Size(394, 21);
            this.CategoryComboBox.TabIndex = 5;
            this.CategoryComboBox.SelectedValueChanged += new System.EventHandler(this.CategoryComboBox_SelectedValueChanged);
            // 
            // PartIDTextBox
            // 
            this.PartIDTextBox.AllowDecimals = false;
            this.tableLayoutPanel1.SetColumnSpan(this.PartIDTextBox, 2);
            this.PartIDTextBox.Location = new System.Drawing.Point(88, 3);
            this.PartIDTextBox.MaximumValue = 9999999D;
            this.PartIDTextBox.Name = "PartIDTextBox";
            this.PartIDTextBox.Size = new System.Drawing.Size(100, 20);
            this.PartIDTextBox.TabIndex = 8;
            this.PartIDTextBox.ValueChanged += new System.EventHandler(this.PartIDTextBox_ValueChanged);
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label6.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label6, 2);
            this.label6.Location = new System.Drawing.Point(3, 209);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(100, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Geometry Bounding";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label2, 3);
            this.label2.Location = new System.Drawing.Point(3, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Description";
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label5, 2);
            this.label5.Location = new System.Drawing.Point(3, 141);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Bounding";
            // 
            // boundingBoxEditor2
            // 
            this.boundingBoxEditor2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.boundingBoxEditor2, 3);
            this.boundingBoxEditor2.Location = new System.Drawing.Point(3, 230);
            this.boundingBoxEditor2.Name = "boundingBoxEditor2";
            this.boundingBoxEditor2.Size = new System.Drawing.Size(394, 39);
            this.boundingBoxEditor2.TabIndex = 13;
            boundingBox1.Max = ((LDDModder.Simple3D.Vector3)(resources.GetObject("boundingBox1.Max")));
            boundingBox1.MaxX = 0F;
            boundingBox1.MaxY = 0F;
            boundingBox1.MaxZ = 0F;
            boundingBox1.Min = ((LDDModder.Simple3D.Vector3)(resources.GetObject("boundingBox1.Min")));
            boundingBox1.MinX = 0F;
            boundingBox1.MinY = 0F;
            boundingBox1.MinZ = 0F;
            this.boundingBoxEditor2.Value = boundingBox1;
            // 
            // CalculateBoundingButton
            // 
            this.CalculateBoundingButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.CalculateBoundingButton.Location = new System.Drawing.Point(323, 136);
            this.CalculateBoundingButton.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.CalculateBoundingButton.Name = "CalculateBoundingButton";
            this.CalculateBoundingButton.Size = new System.Drawing.Size(74, 23);
            this.CalculateBoundingButton.TabIndex = 11;
            this.CalculateBoundingButton.Text = "Calculate";
            this.CalculateBoundingButton.UseVisualStyleBackColor = true;
            this.CalculateBoundingButton.Click += new System.EventHandler(this.CalculateBoundingButton_Click);
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 96);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Category";
            // 
            // boundingBoxEditor1
            // 
            this.boundingBoxEditor1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.boundingBoxEditor1, 3);
            this.boundingBoxEditor1.Location = new System.Drawing.Point(3, 162);
            this.boundingBoxEditor1.Name = "boundingBoxEditor1";
            this.boundingBoxEditor1.Size = new System.Drawing.Size(394, 39);
            this.boundingBoxEditor1.TabIndex = 9;
            boundingBox2.Max = ((LDDModder.Simple3D.Vector3)(resources.GetObject("boundingBox2.Max")));
            boundingBox2.MaxX = 0F;
            boundingBox2.MaxY = 0F;
            boundingBox2.MaxZ = 0F;
            boundingBox2.Min = ((LDDModder.Simple3D.Vector3)(resources.GetObject("boundingBox2.Min")));
            boundingBox2.MinX = 0F;
            boundingBox2.MinY = 0F;
            boundingBox2.MinZ = 0F;
            this.boundingBoxEditor1.Value = boundingBox2;
            // 
            // button1
            // 
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.button1.Location = new System.Drawing.Point(323, 204);
            this.button1.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(74, 23);
            this.button1.TabIndex = 14;
            this.button1.Text = "Calculate";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // PartPropertiesPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(459, 362);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "PartPropertiesPanel";
            this.Text = "Part Properties";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox DescriptionTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox PlatformComboBox;
        private System.Windows.Forms.ComboBox CategoryComboBox;
        private Controls.NumberTextBox PartIDTextBox;
        private System.Windows.Forms.Label label5;
        private Controls.BoundingBoxEditor boundingBoxEditor1;
        private System.Windows.Forms.Button CalculateBoundingButton;
        private System.Windows.Forms.Label label6;
        private Controls.BoundingBoxEditor boundingBoxEditor2;
        private System.Windows.Forms.Button button1;
    }
}