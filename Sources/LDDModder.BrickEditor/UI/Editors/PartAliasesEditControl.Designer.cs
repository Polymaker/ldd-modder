namespace LDDModder.BrickEditor.UI.Controls
{
    partial class PartAliasesEditControl
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
            this.AliasesListBox = new System.Windows.Forms.ListBox();
            this.RemoveButton = new System.Windows.Forms.Button();
            this.AddButton = new System.Windows.Forms.Button();
            this.AliasIDTextBox = new System.Windows.Forms.TextBox();
            this.ApplyEditButton = new System.Windows.Forms.Button();
            this.CancelEditButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // AliasesListBox
            // 
            this.AliasesListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AliasesListBox.FormattingEnabled = true;
            this.AliasesListBox.IntegralHeight = false;
            this.AliasesListBox.Location = new System.Drawing.Point(3, 27);
            this.AliasesListBox.Name = "AliasesListBox";
            this.AliasesListBox.Size = new System.Drawing.Size(142, 104);
            this.AliasesListBox.TabIndex = 0;
            this.AliasesListBox.SelectedIndexChanged += new System.EventHandler(this.AliasesListBox_SelectedIndexChanged);
            // 
            // RemoveButton
            // 
            this.RemoveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.RemoveButton.Enabled = false;
            this.RemoveButton.Location = new System.Drawing.Point(148, 26);
            this.RemoveButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 3);
            this.RemoveButton.Name = "RemoveButton";
            this.RemoveButton.Size = new System.Drawing.Size(29, 23);
            this.RemoveButton.TabIndex = 1;
            this.RemoveButton.Text = "-";
            this.RemoveButton.UseVisualStyleBackColor = true;
            this.RemoveButton.Click += new System.EventHandler(this.RemoveButton_Click);
            // 
            // AddButton
            // 
            this.AddButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AddButton.Enabled = false;
            this.AddButton.Location = new System.Drawing.Point(148, 2);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(29, 22);
            this.AddButton.TabIndex = 2;
            this.AddButton.Text = "+";
            this.AddButton.UseVisualStyleBackColor = true;
            this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // AliasIDTextBox
            // 
            this.AliasIDTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AliasIDTextBox.Location = new System.Drawing.Point(3, 3);
            this.AliasIDTextBox.Name = "AliasIDTextBox";
            this.AliasIDTextBox.Size = new System.Drawing.Size(142, 20);
            this.AliasIDTextBox.TabIndex = 3;
            this.AliasIDTextBox.TextChanged += new System.EventHandler(this.AliasIDTextBox_TextChanged);
            // 
            // ApplyEditButton
            // 
            this.ApplyEditButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ApplyEditButton.Location = new System.Drawing.Point(3, 3);
            this.ApplyEditButton.Name = "ApplyEditButton";
            this.ApplyEditButton.Size = new System.Drawing.Size(84, 23);
            this.ApplyEditButton.TabIndex = 4;
            this.ApplyEditButton.Text = "Save";
            this.ApplyEditButton.UseVisualStyleBackColor = true;
            this.ApplyEditButton.Click += new System.EventHandler(this.ApplyEditButton_Click);
            // 
            // CancelEditButton
            // 
            this.CancelEditButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelEditButton.Location = new System.Drawing.Point(93, 3);
            this.CancelEditButton.Name = "CancelEditButton";
            this.CancelEditButton.Size = new System.Drawing.Size(84, 23);
            this.CancelEditButton.TabIndex = 5;
            this.CancelEditButton.Text = "Cancel";
            this.CancelEditButton.UseVisualStyleBackColor = true;
            this.CancelEditButton.Click += new System.EventHandler(this.CancelEditButton_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.ApplyEditButton, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.CancelEditButton, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 131);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(180, 29);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // PartAliasesEditControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.AliasesListBox);
            this.Controls.Add(this.AliasIDTextBox);
            this.Controls.Add(this.AddButton);
            this.Controls.Add(this.RemoveButton);
            this.Name = "PartAliasesEditControl";
            this.Size = new System.Drawing.Size(180, 160);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ListBox AliasesListBox;
        private System.Windows.Forms.Button RemoveButton;
        private System.Windows.Forms.Button AddButton;
        private System.Windows.Forms.TextBox AliasIDTextBox;
        private System.Windows.Forms.Button CancelEditButton;
        private System.Windows.Forms.Button ApplyEditButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}
