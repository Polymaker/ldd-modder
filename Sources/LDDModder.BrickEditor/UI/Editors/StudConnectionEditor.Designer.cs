namespace LDDModder.BrickEditor.UI.Editors
{
    partial class StudConnectionEditor
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ApplySizeButton = new System.Windows.Forms.Button();
            this.studGridControl1 = new LDDModder.BrickEditor.UI.Editors.StudGridControl();
            this.GridWidthBox = new LDDModder.BrickEditor.UI.Controls.NumberTextBox();
            this.GridHeightBox = new LDDModder.BrickEditor.UI.Controls.NumberTextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.studGridControl1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.GridWidthBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.GridHeightBox, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.ApplySizeButton, 4, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(432, 170);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 8);
            this.label1.Margin = new System.Windows.Forms.Padding(3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Width";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(110, 8);
            this.label2.Margin = new System.Windows.Forms.Padding(3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Height";
            // 
            // ApplySizeButton
            // 
            this.ApplySizeButton.Location = new System.Drawing.Point(220, 3);
            this.ApplySizeButton.Name = "ApplySizeButton";
            this.ApplySizeButton.Size = new System.Drawing.Size(75, 23);
            this.ApplySizeButton.TabIndex = 5;
            this.ApplySizeButton.Text = "Apply";
            this.ApplySizeButton.UseVisualStyleBackColor = true;
            this.ApplySizeButton.Click += new System.EventHandler(this.ApplySizeButton_Click);
            // 
            // studGridControl1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.studGridControl1, 5);
            this.studGridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.studGridControl1.Location = new System.Drawing.Point(3, 32);
            this.studGridControl1.MaxGridSize = new System.Drawing.Size(10, 10);
            this.studGridControl1.Name = "studGridControl1";
            this.studGridControl1.Size = new System.Drawing.Size(426, 135);
            this.studGridControl1.TabIndex = 0;
            this.studGridControl1.Text = "studGridControl1";
            this.studGridControl1.ConnectorChanged += new System.EventHandler(this.studGridControl1_ConnectorChanged);
            this.studGridControl1.ConnectorSizeChanged += new System.EventHandler(this.studGridControl1_ConnectorSizeChanged);
            // 
            // GridWidthBox
            // 
            this.GridWidthBox.AllowDecimals = false;
            this.GridWidthBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.GridWidthBox.Location = new System.Drawing.Point(44, 4);
            this.GridWidthBox.MaximumValue = 500D;
            this.GridWidthBox.MinimumValue = 1D;
            this.GridWidthBox.Name = "GridWidthBox";
            this.GridWidthBox.Size = new System.Drawing.Size(60, 20);
            this.GridWidthBox.TabIndex = 3;
            this.GridWidthBox.Value = 1D;
            // 
            // GridHeightBox
            // 
            this.GridHeightBox.AllowDecimals = false;
            this.GridHeightBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.GridHeightBox.Location = new System.Drawing.Point(154, 4);
            this.GridHeightBox.MaximumValue = 500D;
            this.GridHeightBox.MinimumValue = 1D;
            this.GridHeightBox.Name = "GridHeightBox";
            this.GridHeightBox.Size = new System.Drawing.Size(60, 20);
            this.GridHeightBox.TabIndex = 4;
            this.GridHeightBox.Value = 1D;
            // 
            // StudConnectionEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "StudConnectionEditor";
            this.Size = new System.Drawing.Size(432, 170);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private StudGridControl studGridControl1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private Controls.NumberTextBox GridWidthBox;
        private Controls.NumberTextBox GridHeightBox;
        private System.Windows.Forms.Button ApplySizeButton;
    }
}
