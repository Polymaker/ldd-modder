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
            LDDModder.LDD.Primitives.BoundingBox boundingBox1 = new LDDModder.LDD.Primitives.BoundingBox();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TransformEditor));
            LDDModder.LDD.Primitives.BoundingBox boundingBox2 = new LDDModder.LDD.Primitives.BoundingBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.boundingBoxEditor2 = new LDDModder.BrickEditor.UI.Controls.BoundingBoxEditor();
            this.boundingBoxEditor1 = new LDDModder.BrickEditor.UI.Controls.BoundingBoxEditor();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.boundingBoxEditor2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.boundingBoxEditor1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(340, 90);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // boundingBoxEditor2
            // 
            this.boundingBoxEditor2.Dock = System.Windows.Forms.DockStyle.Top;
            this.boundingBoxEditor2.Location = new System.Drawing.Point(3, 48);
            this.boundingBoxEditor2.Name = "boundingBoxEditor2";
            this.boundingBoxEditor2.Size = new System.Drawing.Size(333, 39);
            this.boundingBoxEditor2.TabIndex = 1;
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
            // boundingBoxEditor1
            // 
            this.boundingBoxEditor1.Dock = System.Windows.Forms.DockStyle.Top;
            this.boundingBoxEditor1.Location = new System.Drawing.Point(3, 3);
            this.boundingBoxEditor1.Name = "boundingBoxEditor1";
            this.boundingBoxEditor1.Size = new System.Drawing.Size(333, 39);
            this.boundingBoxEditor1.TabIndex = 0;
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
            // TransformEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "TransformEditor";
            this.Size = new System.Drawing.Size(340, 246);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private BoundingBoxEditor boundingBoxEditor1;
        private BoundingBoxEditor boundingBoxEditor2;
    }
}
