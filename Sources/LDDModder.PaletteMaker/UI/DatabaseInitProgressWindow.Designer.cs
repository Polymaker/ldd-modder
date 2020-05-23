namespace LDDModder.PaletteMaker.UI
{
    partial class DatabaseInitProgressWindow
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.InitializationStepLabel = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label2 = new System.Windows.Forms.Label();
            this.CancelCloseButton = new System.Windows.Forms.Button();
            this.ProgressStatusLabel = new System.Windows.Forms.Label();
            this.ProgressReportTimer = new System.Windows.Forms.Timer(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 61.34454F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.InitializationStepLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.progressBar1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.CancelCloseButton, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.ProgressStatusLabel, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(295, 115);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // InitializationStepLabel
            // 
            this.InitializationStepLabel.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.InitializationStepLabel, 2);
            this.InitializationStepLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InitializationStepLabel.Location = new System.Drawing.Point(3, 3);
            this.InitializationStepLabel.Margin = new System.Windows.Forms.Padding(3);
            this.InitializationStepLabel.Name = "InitializationStepLabel";
            this.InitializationStepLabel.Size = new System.Drawing.Size(105, 16);
            this.InitializationStepLabel.TabIndex = 0;
            this.InitializationStepLabel.Text = "Initialization step";
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.progressBar1, 2);
            this.progressBar1.Location = new System.Drawing.Point(3, 25);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(289, 23);
            this.progressBar1.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Progress 0% (0/0)";
            // 
            // CancelCloseButton
            // 
            this.CancelCloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelCloseButton.Location = new System.Drawing.Point(217, 89);
            this.CancelCloseButton.Name = "CancelCloseButton";
            this.CancelCloseButton.Size = new System.Drawing.Size(75, 23);
            this.CancelCloseButton.TabIndex = 3;
            this.CancelCloseButton.Text = "Cancel";
            this.CancelCloseButton.UseVisualStyleBackColor = true;
            this.CancelCloseButton.Click += new System.EventHandler(this.CancelCloseButton_Click);
            // 
            // ProgressStatusLabel
            // 
            this.ProgressStatusLabel.AutoSize = true;
            this.ProgressStatusLabel.Location = new System.Drawing.Point(3, 54);
            this.ProgressStatusLabel.Margin = new System.Windows.Forms.Padding(3);
            this.ProgressStatusLabel.Name = "ProgressStatusLabel";
            this.ProgressStatusLabel.Size = new System.Drawing.Size(37, 13);
            this.ProgressStatusLabel.TabIndex = 4;
            this.ProgressStatusLabel.Text = "Status";
            // 
            // ProgressReportTimer
            // 
            this.ProgressReportTimer.Tick += new System.EventHandler(this.ProgressReportTimer_Tick);
            // 
            // DatabaseInitProgressWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(295, 115);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DatabaseInitProgressWindow";
            this.Text = "DatabaseInitProgressWindow";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label InitializationStepLabel;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button CancelCloseButton;
        private System.Windows.Forms.Label ProgressStatusLabel;
        private System.Windows.Forms.Timer ProgressReportTimer;
    }
}