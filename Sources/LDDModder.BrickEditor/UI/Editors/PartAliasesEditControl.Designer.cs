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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PartAliasesEditControl));
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
            resources.ApplyResources(this.AliasesListBox, "AliasesListBox");
            this.AliasesListBox.FormattingEnabled = true;
            this.AliasesListBox.Name = "AliasesListBox";
            this.AliasesListBox.SelectedIndexChanged += new System.EventHandler(this.AliasesListBox_SelectedIndexChanged);
            // 
            // RemoveButton
            // 
            resources.ApplyResources(this.RemoveButton, "RemoveButton");
            this.RemoveButton.Name = "RemoveButton";
            this.RemoveButton.UseVisualStyleBackColor = true;
            this.RemoveButton.Click += new System.EventHandler(this.RemoveButton_Click);
            // 
            // AddButton
            // 
            resources.ApplyResources(this.AddButton, "AddButton");
            this.AddButton.Name = "AddButton";
            this.AddButton.UseVisualStyleBackColor = true;
            this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // AliasIDTextBox
            // 
            resources.ApplyResources(this.AliasIDTextBox, "AliasIDTextBox");
            this.AliasIDTextBox.Name = "AliasIDTextBox";
            this.AliasIDTextBox.TextChanged += new System.EventHandler(this.AliasIDTextBox_TextChanged);
            // 
            // ApplyEditButton
            // 
            resources.ApplyResources(this.ApplyEditButton, "ApplyEditButton");
            this.ApplyEditButton.Name = "ApplyEditButton";
            this.ApplyEditButton.UseVisualStyleBackColor = true;
            this.ApplyEditButton.Click += new System.EventHandler(this.ApplyEditButton_Click);
            // 
            // CancelEditButton
            // 
            resources.ApplyResources(this.CancelEditButton, "CancelEditButton");
            this.CancelEditButton.Name = "CancelEditButton";
            this.CancelEditButton.UseVisualStyleBackColor = true;
            this.CancelEditButton.Click += new System.EventHandler(this.CancelEditButton_Click);
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.ApplyEditButton, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.CancelEditButton, 1, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // PartAliasesEditControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.AliasesListBox);
            this.Controls.Add(this.AliasIDTextBox);
            this.Controls.Add(this.AddButton);
            this.Controls.Add(this.RemoveButton);
            this.Name = "PartAliasesEditControl";
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
