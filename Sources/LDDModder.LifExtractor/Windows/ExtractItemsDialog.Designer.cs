namespace LDDModder.LifExtractor.Windows
{
    partial class ExtractItemsDialog
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
            this.SelectFolderButton = new System.Windows.Forms.Button();
            this.DestinationTextBox = new System.Windows.Forms.TextBox();
            this.CancelExtractButton = new System.Windows.Forms.Button();
            this.ExtractButton = new System.Windows.Forms.Button();
            this.ExtractionProgressBar = new System.Windows.Forms.ProgressBar();
            this.DestinationGroupBox = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.CreateSubDirectoryCheckBox = new System.Windows.Forms.CheckBox();
            this.SubDirectoryTextBox = new System.Windows.Forms.TextBox();
            this.ProgressGroupBox = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.RemainingTimeValueLabel = new System.Windows.Forms.Label();
            this.ElapsedTimeValueLabel = new System.Windows.Forms.Label();
            this.FileProgressValueLabel = new System.Windows.Forms.Label();
            this.ExtractingLabel = new System.Windows.Forms.Label();
            this.CurrentFileLabel = new System.Windows.Forms.Label();
            this.FileProgressLabel = new System.Windows.Forms.Label();
            this.ElapsedTimeLabel = new System.Windows.Forms.Label();
            this.RemainingTimeLabel = new System.Windows.Forms.Label();
            this.ProgressPercentLabel = new System.Windows.Forms.Label();
            this.ProgressPercentValueLabel = new System.Windows.Forms.Label();
            this.ExtractionProgressTimer = new System.Windows.Forms.Timer(this.components);
            this.DestinationGroupBox.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.ProgressGroupBox.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // SelectFolderButton
            // 
            this.SelectFolderButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.SelectFolderButton.Location = new System.Drawing.Point(369, 3);
            this.SelectFolderButton.Name = "SelectFolderButton";
            this.SelectFolderButton.Size = new System.Drawing.Size(50, 23);
            this.SelectFolderButton.TabIndex = 0;
            this.SelectFolderButton.Text = "...";
            this.SelectFolderButton.UseVisualStyleBackColor = true;
            this.SelectFolderButton.Click += new System.EventHandler(this.SelectFolderButton_Click);
            // 
            // DestinationTextBox
            // 
            this.DestinationTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.SetColumnSpan(this.DestinationTextBox, 2);
            this.DestinationTextBox.Location = new System.Drawing.Point(3, 4);
            this.DestinationTextBox.Name = "DestinationTextBox";
            this.DestinationTextBox.Size = new System.Drawing.Size(360, 20);
            this.DestinationTextBox.TabIndex = 1;
            this.DestinationTextBox.TextChanged += new System.EventHandler(this.DestinationTextBox_TextChanged);
            // 
            // CancelExtractButton
            // 
            this.CancelExtractButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelExtractButton.Location = new System.Drawing.Point(356, 201);
            this.CancelExtractButton.Name = "CancelExtractButton";
            this.CancelExtractButton.Size = new System.Drawing.Size(75, 23);
            this.CancelExtractButton.TabIndex = 3;
            this.CancelExtractButton.Text = "Cancel";
            this.CancelExtractButton.UseVisualStyleBackColor = true;
            this.CancelExtractButton.Click += new System.EventHandler(this.CancelExtractButton_Click);
            // 
            // ExtractButton
            // 
            this.ExtractButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ExtractButton.Location = new System.Drawing.Point(277, 201);
            this.ExtractButton.Name = "ExtractButton";
            this.ExtractButton.Size = new System.Drawing.Size(75, 23);
            this.ExtractButton.TabIndex = 4;
            this.ExtractButton.Text = "Extract";
            this.ExtractButton.UseVisualStyleBackColor = true;
            this.ExtractButton.Click += new System.EventHandler(this.ExtractButton_Click);
            // 
            // ExtractionProgressBar
            // 
            this.ExtractionProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel3.SetColumnSpan(this.ExtractionProgressBar, 4);
            this.ExtractionProgressBar.Location = new System.Drawing.Point(3, 41);
            this.ExtractionProgressBar.Name = "ExtractionProgressBar";
            this.ExtractionProgressBar.Size = new System.Drawing.Size(416, 23);
            this.ExtractionProgressBar.TabIndex = 5;
            // 
            // DestinationGroupBox
            // 
            this.DestinationGroupBox.AutoSize = true;
            this.DestinationGroupBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.DestinationGroupBox.Controls.Add(this.tableLayoutPanel2);
            this.DestinationGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.DestinationGroupBox.Location = new System.Drawing.Point(3, 3);
            this.DestinationGroupBox.Name = "DestinationGroupBox";
            this.DestinationGroupBox.Size = new System.Drawing.Size(428, 74);
            this.DestinationGroupBox.TabIndex = 1;
            this.DestinationGroupBox.TabStop = false;
            this.DestinationGroupBox.Text = "Extract to…";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.SelectFolderButton, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.DestinationTextBox, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.CreateSubDirectoryCheckBox, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.SubDirectoryTextBox, 1, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(422, 55);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // CreateSubDirectoryCheckBox
            // 
            this.CreateSubDirectoryCheckBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.CreateSubDirectoryCheckBox.AutoSize = true;
            this.CreateSubDirectoryCheckBox.Location = new System.Drawing.Point(3, 33);
            this.CreateSubDirectoryCheckBox.Name = "CreateSubDirectoryCheckBox";
            this.CreateSubDirectoryCheckBox.Size = new System.Drawing.Size(131, 17);
            this.CreateSubDirectoryCheckBox.TabIndex = 2;
            this.CreateSubDirectoryCheckBox.Text = "Create in subdirectory:";
            this.CreateSubDirectoryCheckBox.UseVisualStyleBackColor = true;
            this.CreateSubDirectoryCheckBox.CheckedChanged += new System.EventHandler(this.CreateSubDirectoryCheckBox_CheckedChanged);
            // 
            // SubDirectoryTextBox
            // 
            this.SubDirectoryTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.SubDirectoryTextBox.Location = new System.Drawing.Point(140, 32);
            this.SubDirectoryTextBox.Name = "SubDirectoryTextBox";
            this.SubDirectoryTextBox.Size = new System.Drawing.Size(139, 20);
            this.SubDirectoryTextBox.TabIndex = 3;
            // 
            // ProgressGroupBox
            // 
            this.ProgressGroupBox.Controls.Add(this.tableLayoutPanel3);
            this.ProgressGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.ProgressGroupBox.Location = new System.Drawing.Point(3, 77);
            this.ProgressGroupBox.Name = "ProgressGroupBox";
            this.ProgressGroupBox.Size = new System.Drawing.Size(428, 124);
            this.ProgressGroupBox.TabIndex = 1;
            this.ProgressGroupBox.TabStop = false;
            this.ProgressGroupBox.Text = "Progress";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 4;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.Controls.Add(this.RemainingTimeValueLabel, 3, 1);
            this.tableLayoutPanel3.Controls.Add(this.ElapsedTimeValueLabel, 3, 0);
            this.tableLayoutPanel3.Controls.Add(this.FileProgressValueLabel, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.ExtractingLabel, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.CurrentFileLabel, 0, 4);
            this.tableLayoutPanel3.Controls.Add(this.FileProgressLabel, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.ExtractionProgressBar, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.ElapsedTimeLabel, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.RemainingTimeLabel, 2, 1);
            this.tableLayoutPanel3.Controls.Add(this.ProgressPercentLabel, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.ProgressPercentValueLabel, 1, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 5;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(422, 105);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // RemainingTimeValueLabel
            // 
            this.RemainingTimeValueLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.RemainingTimeValueLabel.AutoSize = true;
            this.RemainingTimeValueLabel.Location = new System.Drawing.Point(364, 19);
            this.RemainingTimeValueLabel.Name = "RemainingTimeValueLabel";
            this.RemainingTimeValueLabel.Padding = new System.Windows.Forms.Padding(3);
            this.RemainingTimeValueLabel.Size = new System.Drawing.Size(55, 19);
            this.RemainingTimeValueLabel.TabIndex = 11;
            this.RemainingTimeValueLabel.Text = "00:15:00";
            // 
            // ElapsedTimeValueLabel
            // 
            this.ElapsedTimeValueLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ElapsedTimeValueLabel.AutoSize = true;
            this.ElapsedTimeValueLabel.Location = new System.Drawing.Point(364, 0);
            this.ElapsedTimeValueLabel.Name = "ElapsedTimeValueLabel";
            this.ElapsedTimeValueLabel.Padding = new System.Windows.Forms.Padding(3);
            this.ElapsedTimeValueLabel.Size = new System.Drawing.Size(55, 19);
            this.ElapsedTimeValueLabel.TabIndex = 10;
            this.ElapsedTimeValueLabel.Text = "00:02:01";
            // 
            // FileProgressValueLabel
            // 
            this.FileProgressValueLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FileProgressValueLabel.AutoSize = true;
            this.FileProgressValueLabel.Location = new System.Drawing.Point(165, 0);
            this.FileProgressValueLabel.Name = "FileProgressValueLabel";
            this.FileProgressValueLabel.Padding = new System.Windows.Forms.Padding(3);
            this.FileProgressValueLabel.Size = new System.Drawing.Size(42, 19);
            this.FileProgressValueLabel.TabIndex = 8;
            this.FileProgressValueLabel.Text = "1 / 10";
            // 
            // ExtractingLabel
            // 
            this.ExtractingLabel.AutoSize = true;
            this.tableLayoutPanel3.SetColumnSpan(this.ExtractingLabel, 2);
            this.ExtractingLabel.Location = new System.Drawing.Point(3, 67);
            this.ExtractingLabel.Name = "ExtractingLabel";
            this.ExtractingLabel.Size = new System.Drawing.Size(57, 13);
            this.ExtractingLabel.TabIndex = 7;
            this.ExtractingLabel.Text = "Extracting:";
            // 
            // CurrentFileLabel
            // 
            this.CurrentFileLabel.AutoSize = true;
            this.tableLayoutPanel3.SetColumnSpan(this.CurrentFileLabel, 4);
            this.CurrentFileLabel.Location = new System.Drawing.Point(3, 80);
            this.CurrentFileLabel.Name = "CurrentFileLabel";
            this.CurrentFileLabel.Size = new System.Drawing.Size(118, 13);
            this.CurrentFileLabel.TabIndex = 1;
            this.CurrentFileLabel.Text = "Lif\\Assemblies\\1234.txt";
            // 
            // FileProgressLabel
            // 
            this.FileProgressLabel.AutoSize = true;
            this.FileProgressLabel.Location = new System.Drawing.Point(3, 0);
            this.FileProgressLabel.Name = "FileProgressLabel";
            this.FileProgressLabel.Padding = new System.Windows.Forms.Padding(3);
            this.FileProgressLabel.Size = new System.Drawing.Size(37, 19);
            this.FileProgressLabel.TabIndex = 3;
            this.FileProgressLabel.Text = "Files:";
            // 
            // ElapsedTimeLabel
            // 
            this.ElapsedTimeLabel.AutoSize = true;
            this.ElapsedTimeLabel.Location = new System.Drawing.Point(213, 0);
            this.ElapsedTimeLabel.Name = "ElapsedTimeLabel";
            this.ElapsedTimeLabel.Padding = new System.Windows.Forms.Padding(3);
            this.ElapsedTimeLabel.Size = new System.Drawing.Size(76, 19);
            this.ElapsedTimeLabel.TabIndex = 1;
            this.ElapsedTimeLabel.Text = "Elapsed time:";
            // 
            // RemainingTimeLabel
            // 
            this.RemainingTimeLabel.AutoSize = true;
            this.RemainingTimeLabel.Location = new System.Drawing.Point(213, 19);
            this.RemainingTimeLabel.Name = "RemainingTimeLabel";
            this.RemainingTimeLabel.Padding = new System.Windows.Forms.Padding(3);
            this.RemainingTimeLabel.Size = new System.Drawing.Size(94, 19);
            this.RemainingTimeLabel.TabIndex = 2;
            this.RemainingTimeLabel.Text = "Remainging time:";
            // 
            // ProgressPercentLabel
            // 
            this.ProgressPercentLabel.AutoSize = true;
            this.ProgressPercentLabel.Location = new System.Drawing.Point(3, 19);
            this.ProgressPercentLabel.Name = "ProgressPercentLabel";
            this.ProgressPercentLabel.Padding = new System.Windows.Forms.Padding(3);
            this.ProgressPercentLabel.Size = new System.Drawing.Size(57, 19);
            this.ProgressPercentLabel.TabIndex = 6;
            this.ProgressPercentLabel.Text = "Progress:";
            // 
            // ProgressPercentValueLabel
            // 
            this.ProgressPercentValueLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ProgressPercentValueLabel.AutoSize = true;
            this.ProgressPercentValueLabel.Location = new System.Drawing.Point(174, 19);
            this.ProgressPercentValueLabel.Name = "ProgressPercentValueLabel";
            this.ProgressPercentValueLabel.Padding = new System.Windows.Forms.Padding(3);
            this.ProgressPercentValueLabel.Size = new System.Drawing.Size(33, 19);
            this.ProgressPercentValueLabel.TabIndex = 9;
            this.ProgressPercentValueLabel.Text = "10%";
            // 
            // ExtractionProgressTimer
            // 
            this.ExtractionProgressTimer.Tick += new System.EventHandler(this.ExtractionProgressTimer_Tick);
            // 
            // ExtractItemsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelExtractButton;
            this.ClientSize = new System.Drawing.Size(434, 228);
            this.Controls.Add(this.CancelExtractButton);
            this.Controls.Add(this.ExtractButton);
            this.Controls.Add(this.ProgressGroupBox);
            this.Controls.Add(this.DestinationGroupBox);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExtractItemsDialog";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Extract";
            this.DestinationGroupBox.ResumeLayout(false);
            this.DestinationGroupBox.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ProgressGroupBox.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button SelectFolderButton;
        private System.Windows.Forms.TextBox DestinationTextBox;
        private System.Windows.Forms.Button CancelExtractButton;
        private System.Windows.Forms.Button ExtractButton;
        private System.Windows.Forms.ProgressBar ExtractionProgressBar;
        private System.Windows.Forms.GroupBox DestinationGroupBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label RemainingTimeValueLabel;
        private System.Windows.Forms.Label ElapsedTimeValueLabel;
        private System.Windows.Forms.Label FileProgressValueLabel;
        private System.Windows.Forms.Label ExtractingLabel;
        private System.Windows.Forms.Label CurrentFileLabel;
        private System.Windows.Forms.Label FileProgressLabel;
        private System.Windows.Forms.Label ElapsedTimeLabel;
        private System.Windows.Forms.Label RemainingTimeLabel;
        private System.Windows.Forms.Label ProgressPercentLabel;
        private System.Windows.Forms.Label ProgressPercentValueLabel;
        private System.Windows.Forms.GroupBox ProgressGroupBox;
        private System.Windows.Forms.Timer ExtractionProgressTimer;
        private System.Windows.Forms.CheckBox CreateSubDirectoryCheckBox;
        private System.Windows.Forms.TextBox SubDirectoryTextBox;
    }
}