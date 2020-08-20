namespace LDDModder.BrickEditor.UI.Editors
{
    partial class VectorEditor
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
            this.BoxesLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ValueZ = new LDDModder.BrickEditor.UI.Controls.NumberTextBox();
            this.ValueY = new LDDModder.BrickEditor.UI.Controls.NumberTextBox();
            this.ValueX = new LDDModder.BrickEditor.UI.Controls.NumberTextBox();
            this.BoxesLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // BoxesLayoutPanel
            // 
            this.BoxesLayoutPanel.AutoSize = true;
            this.BoxesLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BoxesLayoutPanel.ColumnCount = 3;
            this.BoxesLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.BoxesLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.BoxesLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.BoxesLayoutPanel.Controls.Add(this.ValueZ, 0, 0);
            this.BoxesLayoutPanel.Controls.Add(this.ValueY, 0, 0);
            this.BoxesLayoutPanel.Controls.Add(this.ValueX, 0, 0);
            this.BoxesLayoutPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.BoxesLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.BoxesLayoutPanel.Name = "BoxesLayoutPanel";
            this.BoxesLayoutPanel.RowCount = 1;
            this.BoxesLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.BoxesLayoutPanel.Size = new System.Drawing.Size(216, 20);
            this.BoxesLayoutPanel.TabIndex = 0;
            this.BoxesLayoutPanel.SizeChanged += new System.EventHandler(this.BoxesLayoutPanel_SizeChanged);
            // 
            // ValueZ
            // 
            this.ValueZ.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ValueZ.Location = new System.Drawing.Point(146, 0);
            this.ValueZ.Margin = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.ValueZ.Name = "ValueZ";
            this.ValueZ.Size = new System.Drawing.Size(70, 20);
            this.ValueZ.TabIndex = 2;
            this.ValueZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ValueZ.UseMinMax = false;
            this.ValueZ.ValueChanged += new System.EventHandler(this.ValueBoxes_ValueChanged);
            // 
            // ValueY
            // 
            this.ValueY.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ValueY.Location = new System.Drawing.Point(73, 0);
            this.ValueY.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.ValueY.Name = "ValueY";
            this.ValueY.Size = new System.Drawing.Size(70, 20);
            this.ValueY.TabIndex = 1;
            this.ValueY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ValueY.UseMinMax = false;
            this.ValueY.ValueChanged += new System.EventHandler(this.ValueBoxes_ValueChanged);
            // 
            // ValueX
            // 
            this.ValueX.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ValueX.Location = new System.Drawing.Point(0, 0);
            this.ValueX.Margin = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.ValueX.Name = "ValueX";
            this.ValueX.Size = new System.Drawing.Size(70, 20);
            this.ValueX.TabIndex = 0;
            this.ValueX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ValueX.UseMinMax = false;
            this.ValueX.ValueChanged += new System.EventHandler(this.ValueBoxes_ValueChanged);
            // 
            // VectorEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.BoxesLayoutPanel);
            this.Name = "VectorEditor";
            this.Size = new System.Drawing.Size(216, 42);
            this.BoxesLayoutPanel.ResumeLayout(false);
            this.BoxesLayoutPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel BoxesLayoutPanel;
        private Controls.NumberTextBox ValueX;
        private Controls.NumberTextBox ValueZ;
        private Controls.NumberTextBox ValueY;
    }
}
