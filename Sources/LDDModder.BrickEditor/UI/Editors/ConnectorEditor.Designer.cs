namespace LDDModder.BrickEditor.UI.Controls
{
    partial class ConnectorEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConnectorEditor));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.ConnectionTypeLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.LengthTextBox = new LDDModder.BrickEditor.UI.Controls.NumberTextBox();
            this.ConnectionSubTypeLabel = new System.Windows.Forms.Label();
            this.ConnectionSubTypeCombo = new System.Windows.Forms.ComboBox();
            this.ConnectionTypeValueLabel = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.EndCappedCheckBox = new System.Windows.Forms.CheckBox();
            this.StartCappedCheckBox = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.ConnectionTypeLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.LengthTextBox, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.ConnectionSubTypeLabel, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.ConnectionSubTypeCombo, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.ConnectionTypeValueLabel, 1, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // ConnectionTypeLabel
            // 
            resources.ApplyResources(this.ConnectionTypeLabel, "ConnectionTypeLabel");
            this.ConnectionTypeLabel.Name = "ConnectionTypeLabel";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // LengthTextBox
            // 
            resources.ApplyResources(this.LengthTextBox, "LengthTextBox");
            this.LengthTextBox.MaximumValue = 200D;
            this.LengthTextBox.Name = "LengthTextBox";
            // 
            // ConnectionSubTypeLabel
            // 
            resources.ApplyResources(this.ConnectionSubTypeLabel, "ConnectionSubTypeLabel");
            this.ConnectionSubTypeLabel.Name = "ConnectionSubTypeLabel";
            // 
            // ConnectionSubTypeCombo
            // 
            this.ConnectionSubTypeCombo.FormattingEnabled = true;
            resources.ApplyResources(this.ConnectionSubTypeCombo, "ConnectionSubTypeCombo");
            this.ConnectionSubTypeCombo.Name = "ConnectionSubTypeCombo";
            // 
            // ConnectionTypeValueLabel
            // 
            resources.ApplyResources(this.ConnectionTypeValueLabel, "ConnectionTypeValueLabel");
            this.ConnectionTypeValueLabel.Name = "ConnectionTypeValueLabel";
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.checkBox4, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.checkBox3, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.EndCappedCheckBox, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.StartCappedCheckBox, 0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // checkBox4
            // 
            resources.ApplyResources(this.checkBox4, "checkBox4");
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.UseVisualStyleBackColor = true;
            // 
            // checkBox3
            // 
            resources.ApplyResources(this.checkBox3, "checkBox3");
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // EndCappedCheckBox
            // 
            resources.ApplyResources(this.EndCappedCheckBox, "EndCappedCheckBox");
            this.EndCappedCheckBox.Name = "EndCappedCheckBox";
            this.EndCappedCheckBox.UseVisualStyleBackColor = true;
            // 
            // StartCappedCheckBox
            // 
            resources.ApplyResources(this.StartCappedCheckBox, "StartCappedCheckBox");
            this.StartCappedCheckBox.Name = "StartCappedCheckBox";
            this.StartCappedCheckBox.UseVisualStyleBackColor = true;
            // 
            // ConnectorEditor
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ConnectorEditor";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label ConnectionTypeLabel;
        private System.Windows.Forms.Label label2;
        private Controls.NumberTextBox LengthTextBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.CheckBox EndCappedCheckBox;
        private System.Windows.Forms.CheckBox StartCappedCheckBox;
        private System.Windows.Forms.Label ConnectionSubTypeLabel;
        private System.Windows.Forms.ComboBox ConnectionSubTypeCombo;
        private System.Windows.Forms.Label ConnectionTypeValueLabel;
    }
}
