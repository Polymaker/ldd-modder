namespace LDDModder.BrickInstaller
{
    partial class FrmPackageInstaller
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPackageInstaller));
            this.btnShowDetails = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.rtbInstallLog = new System.Windows.Forms.RichTextBox();
            this.lblOperationInfo = new System.Windows.Forms.Label();
            this.progBar = new System.Windows.Forms.ProgressBar();
            this.btnAction = new System.Windows.Forms.Button();
            this.localizableStrings1 = new LDDModder.Display.Utilities.LocalizableStrings(this.components);
            this.LOC_ValidateLDD = new LDDModder.Display.Utilities.StringEntry(this.components);
            this.LOC_ShowDetails = new LDDModder.Display.Utilities.StringEntry(this.components);
            this.LOC_HideDetails = new LDDModder.Display.Utilities.StringEntry(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnShowDetails
            // 
            resources.ApplyResources(this.btnShowDetails, "btnShowDetails");
            this.btnShowDetails.Name = "btnShowDetails";
            this.btnShowDetails.UseVisualStyleBackColor = true;
            this.btnShowDetails.Click += new System.EventHandler(this.button1_Click);
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.rtbInstallLog, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnShowDetails, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblOperationInfo, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.progBar, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnAction, 1, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // rtbInstallLog
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.rtbInstallLog, 2);
            resources.ApplyResources(this.rtbInstallLog, "rtbInstallLog");
            this.rtbInstallLog.Name = "rtbInstallLog";
            // 
            // lblOperationInfo
            // 
            resources.ApplyResources(this.lblOperationInfo, "lblOperationInfo");
            this.tableLayoutPanel1.SetColumnSpan(this.lblOperationInfo, 2);
            this.lblOperationInfo.Name = "lblOperationInfo";
            // 
            // progBar
            // 
            resources.ApplyResources(this.progBar, "progBar");
            this.tableLayoutPanel1.SetColumnSpan(this.progBar, 2);
            this.progBar.MarqueeAnimationSpeed = 10;
            this.progBar.Name = "progBar";
            // 
            // btnAction
            // 
            resources.ApplyResources(this.btnAction, "btnAction");
            this.btnAction.Name = "btnAction";
            this.btnAction.UseVisualStyleBackColor = true;
            // 
            // localizableStrings1
            // 
            this.localizableStrings1.Entries.Add(this.LOC_ValidateLDD);
            this.localizableStrings1.Entries.Add(this.LOC_ShowDetails);
            this.localizableStrings1.Entries.Add(this.LOC_HideDetails);
            // 
            // LOC_ValidateLDD
            // 
            resources.ApplyResources(this.LOC_ValidateLDD, "LOC_ValidateLDD");
            // 
            // LOC_ShowDetails
            // 
            resources.ApplyResources(this.LOC_ShowDetails, "LOC_ShowDetails");
            // 
            // LOC_HideDetails
            // 
            resources.ApplyResources(this.LOC_HideDetails, "LOC_HideDetails");
            // 
            // FrmPackageInstaller
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximizeBox = false;
            this.Name = "FrmPackageInstaller";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnShowDetails;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblOperationInfo;
        private System.Windows.Forms.ProgressBar progBar;
        private Display.Utilities.LocalizableStrings localizableStrings1;
        private Display.Utilities.StringEntry LOC_ValidateLDD;
        private System.Windows.Forms.RichTextBox rtbInstallLog;
        private System.Windows.Forms.Button btnAction;
        private Display.Utilities.StringEntry LOC_ShowDetails;
        private Display.Utilities.StringEntry LOC_HideDetails;
    }
}

