namespace LDDModder.PaletteMaker.Views
{
    partial class SetDetailView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetDetailView));
            this.tlpSetDetails = new System.Windows.Forms.TableLayoutPanel();
            this.txtSetName = new System.Windows.Forms.TextBox();
            this.lblSetName = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtSetID = new System.Windows.Forms.TextBox();
            this.txtSetTheme = new System.Windows.Forms.TextBox();
            this.txtSetYear = new System.Windows.Forms.TextBox();
            this.txtSetPieces = new System.Windows.Forms.TextBox();
            this.pbxSetPicture = new System.Windows.Forms.PictureBox();
            this.tlpSetDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxSetPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // tlpSetDetails
            // 
            resources.ApplyResources(this.tlpSetDetails, "tlpSetDetails");
            this.tlpSetDetails.Controls.Add(this.txtSetName, 1, 1);
            this.tlpSetDetails.Controls.Add(this.lblSetName, 0, 1);
            this.tlpSetDetails.Controls.Add(this.label2, 0, 2);
            this.tlpSetDetails.Controls.Add(this.label3, 0, 3);
            this.tlpSetDetails.Controls.Add(this.label4, 0, 4);
            this.tlpSetDetails.Controls.Add(this.label5, 0, 0);
            this.tlpSetDetails.Controls.Add(this.txtSetID, 1, 0);
            this.tlpSetDetails.Controls.Add(this.txtSetTheme, 1, 2);
            this.tlpSetDetails.Controls.Add(this.txtSetYear, 1, 3);
            this.tlpSetDetails.Controls.Add(this.txtSetPieces, 1, 4);
            this.tlpSetDetails.Name = "tlpSetDetails";
            // 
            // txtSetName
            // 
            resources.ApplyResources(this.txtSetName, "txtSetName");
            this.txtSetName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSetName.Name = "txtSetName";
            this.txtSetName.ReadOnly = true;
            // 
            // lblSetName
            // 
            resources.ApplyResources(this.lblSetName, "lblSetName");
            this.lblSetName.Name = "lblSetName";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // txtSetID
            // 
            this.txtSetID.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.txtSetID, "txtSetID");
            this.txtSetID.Name = "txtSetID";
            this.txtSetID.ReadOnly = true;
            // 
            // txtSetTheme
            // 
            resources.ApplyResources(this.txtSetTheme, "txtSetTheme");
            this.txtSetTheme.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSetTheme.Name = "txtSetTheme";
            this.txtSetTheme.ReadOnly = true;
            // 
            // txtSetYear
            // 
            resources.ApplyResources(this.txtSetYear, "txtSetYear");
            this.txtSetYear.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSetYear.Name = "txtSetYear";
            this.txtSetYear.ReadOnly = true;
            // 
            // txtSetPieces
            // 
            resources.ApplyResources(this.txtSetPieces, "txtSetPieces");
            this.txtSetPieces.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSetPieces.Name = "txtSetPieces";
            this.txtSetPieces.ReadOnly = true;
            // 
            // pbxSetPicture
            // 
            this.pbxSetPicture.BackColor = System.Drawing.SystemColors.Window;
            this.pbxSetPicture.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.pbxSetPicture, "pbxSetPicture");
            this.pbxSetPicture.Name = "pbxSetPicture";
            this.pbxSetPicture.TabStop = false;
            // 
            // SetDetailView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pbxSetPicture);
            this.Controls.Add(this.tlpSetDetails);
            this.Name = "SetDetailView";
            this.tlpSetDetails.ResumeLayout(false);
            this.tlpSetDetails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxSetPicture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpSetDetails;
        private System.Windows.Forms.TextBox txtSetName;
        private System.Windows.Forms.Label lblSetName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtSetID;
        private System.Windows.Forms.TextBox txtSetTheme;
        private System.Windows.Forms.TextBox txtSetYear;
        private System.Windows.Forms.TextBox txtSetPieces;
        private System.Windows.Forms.PictureBox pbxSetPicture;
    }
}
