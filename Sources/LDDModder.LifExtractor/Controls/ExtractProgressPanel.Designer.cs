namespace LDDModder.LifExtractor.Controls
{
    partial class ExtractProgressPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExtractProgressPanel));
            this.ProgressTableLayout = new System.Windows.Forms.TableLayoutPanel();
            this.RemainingTimeValueLabel = new System.Windows.Forms.Label();
            this.ElapsedTimeValueLabel = new System.Windows.Forms.Label();
            this.FileProgressValueLabel = new System.Windows.Forms.Label();
            this.ExtractingLabel = new System.Windows.Forms.Label();
            this.CurrentFileLabel = new System.Windows.Forms.Label();
            this.FileProgressLabel = new System.Windows.Forms.Label();
            this.ExtractionProgressBar = new System.Windows.Forms.ProgressBar();
            this.ElapsedTimeLabel = new System.Windows.Forms.Label();
            this.RemainingTimeLabel = new System.Windows.Forms.Label();
            this.ProgressPercentLabel = new System.Windows.Forms.Label();
            this.ProgressPercentValueLabel = new System.Windows.Forms.Label();
            this.ProgressTableLayout.SuspendLayout();
            this.SuspendLayout();
            // 
            // ProgressTableLayout
            // 
            resources.ApplyResources(this.ProgressTableLayout, "ProgressTableLayout");
            this.ProgressTableLayout.Controls.Add(this.RemainingTimeValueLabel, 3, 1);
            this.ProgressTableLayout.Controls.Add(this.ElapsedTimeValueLabel, 3, 0);
            this.ProgressTableLayout.Controls.Add(this.FileProgressValueLabel, 1, 0);
            this.ProgressTableLayout.Controls.Add(this.ExtractingLabel, 0, 3);
            this.ProgressTableLayout.Controls.Add(this.CurrentFileLabel, 0, 4);
            this.ProgressTableLayout.Controls.Add(this.FileProgressLabel, 0, 0);
            this.ProgressTableLayout.Controls.Add(this.ExtractionProgressBar, 0, 2);
            this.ProgressTableLayout.Controls.Add(this.ElapsedTimeLabel, 2, 0);
            this.ProgressTableLayout.Controls.Add(this.RemainingTimeLabel, 2, 1);
            this.ProgressTableLayout.Controls.Add(this.ProgressPercentLabel, 0, 1);
            this.ProgressTableLayout.Controls.Add(this.ProgressPercentValueLabel, 1, 1);
            this.ProgressTableLayout.Name = "ProgressTableLayout";
            // 
            // RemainingTimeValueLabel
            // 
            resources.ApplyResources(this.RemainingTimeValueLabel, "RemainingTimeValueLabel");
            this.RemainingTimeValueLabel.Name = "RemainingTimeValueLabel";
            // 
            // ElapsedTimeValueLabel
            // 
            resources.ApplyResources(this.ElapsedTimeValueLabel, "ElapsedTimeValueLabel");
            this.ElapsedTimeValueLabel.Name = "ElapsedTimeValueLabel";
            // 
            // FileProgressValueLabel
            // 
            resources.ApplyResources(this.FileProgressValueLabel, "FileProgressValueLabel");
            this.FileProgressValueLabel.Name = "FileProgressValueLabel";
            // 
            // ExtractingLabel
            // 
            resources.ApplyResources(this.ExtractingLabel, "ExtractingLabel");
            this.ProgressTableLayout.SetColumnSpan(this.ExtractingLabel, 2);
            this.ExtractingLabel.Name = "ExtractingLabel";
            // 
            // CurrentFileLabel
            // 
            resources.ApplyResources(this.CurrentFileLabel, "CurrentFileLabel");
            this.ProgressTableLayout.SetColumnSpan(this.CurrentFileLabel, 4);
            this.CurrentFileLabel.Name = "CurrentFileLabel";
            // 
            // FileProgressLabel
            // 
            resources.ApplyResources(this.FileProgressLabel, "FileProgressLabel");
            this.FileProgressLabel.Name = "FileProgressLabel";
            // 
            // ExtractionProgressBar
            // 
            resources.ApplyResources(this.ExtractionProgressBar, "ExtractionProgressBar");
            this.ProgressTableLayout.SetColumnSpan(this.ExtractionProgressBar, 4);
            this.ExtractionProgressBar.Name = "ExtractionProgressBar";
            // 
            // ElapsedTimeLabel
            // 
            resources.ApplyResources(this.ElapsedTimeLabel, "ElapsedTimeLabel");
            this.ElapsedTimeLabel.Name = "ElapsedTimeLabel";
            // 
            // RemainingTimeLabel
            // 
            resources.ApplyResources(this.RemainingTimeLabel, "RemainingTimeLabel");
            this.RemainingTimeLabel.Name = "RemainingTimeLabel";
            // 
            // ProgressPercentLabel
            // 
            resources.ApplyResources(this.ProgressPercentLabel, "ProgressPercentLabel");
            this.ProgressPercentLabel.Name = "ProgressPercentLabel";
            // 
            // ProgressPercentValueLabel
            // 
            resources.ApplyResources(this.ProgressPercentValueLabel, "ProgressPercentValueLabel");
            this.ProgressPercentValueLabel.Name = "ProgressPercentValueLabel";
            // 
            // ExtractProgressPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ProgressTableLayout);
            this.Name = "ExtractProgressPanel";
            this.ProgressTableLayout.ResumeLayout(false);
            this.ProgressTableLayout.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel ProgressTableLayout;
        private System.Windows.Forms.Label RemainingTimeValueLabel;
        private System.Windows.Forms.Label ElapsedTimeValueLabel;
        private System.Windows.Forms.Label FileProgressValueLabel;
        private System.Windows.Forms.Label ExtractingLabel;
        private System.Windows.Forms.Label CurrentFileLabel;
        private System.Windows.Forms.Label FileProgressLabel;
        private System.Windows.Forms.ProgressBar ExtractionProgressBar;
        private System.Windows.Forms.Label ElapsedTimeLabel;
        private System.Windows.Forms.Label RemainingTimeLabel;
        private System.Windows.Forms.Label ProgressPercentLabel;
        private System.Windows.Forms.Label ProgressPercentValueLabel;
    }
}
