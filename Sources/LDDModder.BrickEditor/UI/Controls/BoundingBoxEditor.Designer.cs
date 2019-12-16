namespace LDDModder.BrickEditor.UI.Controls
{
    partial class BoundingBoxEditor
    {
        /// <summary> 
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.MinX_Box = new LDDModder.BrickEditor.UI.Controls.NumberTextBox();
            this.MinY_Box = new LDDModder.BrickEditor.UI.Controls.NumberTextBox();
            this.MinZ_Box = new LDDModder.BrickEditor.UI.Controls.NumberTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.MaxX_Box = new LDDModder.BrickEditor.UI.Controls.NumberTextBox();
            this.MaxY_Box = new LDDModder.BrickEditor.UI.Controls.NumberTextBox();
            this.MaxZ_Box = new LDDModder.BrickEditor.UI.Controls.NumberTextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.label3, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.MinX_Box, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.MinY_Box, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.MinZ_Box, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(180, 42);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(120, 3);
            this.label3.Margin = new System.Windows.Forms.Padding(0, 3, 3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Min Z";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(60, 3);
            this.label2.Margin = new System.Windows.Forms.Padding(0, 3, 3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Min Y";
            // 
            // MinX_Box
            // 
            this.MinX_Box.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MinX_Box.Location = new System.Drawing.Point(0, 19);
            this.MinX_Box.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.MinX_Box.MaximumValue = 200D;
            this.MinX_Box.MinimumValue = -200D;
            this.MinX_Box.Name = "MinX_Box";
            this.MinX_Box.Size = new System.Drawing.Size(57, 20);
            this.MinX_Box.TabIndex = 0;
            this.MinX_Box.ValueChanged += new System.EventHandler(this.BoundingValueChanged);
            // 
            // MinY_Box
            // 
            this.MinY_Box.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MinY_Box.Location = new System.Drawing.Point(60, 19);
            this.MinY_Box.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.MinY_Box.MaximumValue = 200D;
            this.MinY_Box.MinimumValue = -200D;
            this.MinY_Box.Name = "MinY_Box";
            this.MinY_Box.Size = new System.Drawing.Size(57, 20);
            this.MinY_Box.TabIndex = 1;
            this.MinY_Box.ValueChanged += new System.EventHandler(this.BoundingValueChanged);
            // 
            // MinZ_Box
            // 
            this.MinZ_Box.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MinZ_Box.Location = new System.Drawing.Point(120, 19);
            this.MinZ_Box.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.MinZ_Box.MaximumValue = 200D;
            this.MinZ_Box.MinimumValue = -200D;
            this.MinZ_Box.Name = "MinZ_Box";
            this.MinZ_Box.Size = new System.Drawing.Size(57, 20);
            this.MinZ_Box.TabIndex = 2;
            this.MinZ_Box.ValueChanged += new System.EventHandler(this.BoundingValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 3);
            this.label1.Margin = new System.Windows.Forms.Padding(0, 3, 3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Min X";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(120, 3);
            this.label6.Margin = new System.Windows.Forms.Padding(0, 3, 3, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(37, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Max Z";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(60, 3);
            this.label5.Margin = new System.Windows.Forms.Padding(0, 3, 3, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(37, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Max Y";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(0, 3);
            this.label4.Margin = new System.Windows.Forms.Padding(0, 3, 3, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Max X";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.Controls.Add(this.label6, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.MaxX_Box, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.label5, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.MaxY_Box, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.MaxZ_Box, 2, 1);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(179, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(180, 42);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // MaxX_Box
            // 
            this.MaxX_Box.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MaxX_Box.Location = new System.Drawing.Point(0, 19);
            this.MaxX_Box.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.MaxX_Box.MaximumValue = 200D;
            this.MaxX_Box.MinimumValue = -200D;
            this.MaxX_Box.Name = "MaxX_Box";
            this.MaxX_Box.Size = new System.Drawing.Size(57, 20);
            this.MaxX_Box.TabIndex = 3;
            this.MaxX_Box.ValueChanged += new System.EventHandler(this.BoundingValueChanged);
            // 
            // MaxY_Box
            // 
            this.MaxY_Box.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MaxY_Box.Location = new System.Drawing.Point(60, 19);
            this.MaxY_Box.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.MaxY_Box.MaximumValue = 200D;
            this.MaxY_Box.MinimumValue = -200D;
            this.MaxY_Box.Name = "MaxY_Box";
            this.MaxY_Box.Size = new System.Drawing.Size(57, 20);
            this.MaxY_Box.TabIndex = 4;
            this.MaxY_Box.ValueChanged += new System.EventHandler(this.BoundingValueChanged);
            // 
            // MaxZ_Box
            // 
            this.MaxZ_Box.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MaxZ_Box.Location = new System.Drawing.Point(120, 19);
            this.MaxZ_Box.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.MaxZ_Box.MaximumValue = 200D;
            this.MaxZ_Box.MinimumValue = -200D;
            this.MaxZ_Box.Name = "MaxZ_Box";
            this.MaxZ_Box.Size = new System.Drawing.Size(57, 20);
            this.MaxZ_Box.TabIndex = 5;
            this.MaxZ_Box.ValueChanged += new System.EventHandler(this.BoundingValueChanged);
            // 
            // BoundingBoxEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "BoundingBoxEditor";
            this.Size = new System.Drawing.Size(376, 58);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private NumberTextBox MinX_Box;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private NumberTextBox MinY_Box;
        private NumberTextBox MinZ_Box;
        private NumberTextBox MaxX_Box;
        private NumberTextBox MaxY_Box;
        private NumberTextBox MaxZ_Box;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
    }
}
