namespace LDDModder.BrickEditor.UI.Windows
{
    partial class MessageBoxEX
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MessageBoxEX));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.Option1Button = new System.Windows.Forms.Button();
            this.Option2Button = new System.Windows.Forms.Button();
            this.Option3Button = new System.Windows.Forms.Button();
            this.ErrorDetailTextBox = new System.Windows.Forms.TextBox();
            this.MessageTextLabel = new System.Windows.Forms.Label();
            this.MessageIconBox = new System.Windows.Forms.PictureBox();
            this.ButtonLabels = new LDDModder.BrickEditor.Localization.LocalizableStringList(this.components);
            this.Label_OK = new LDDModder.BrickEditor.Localization.LocalizableString();
            this.Label_Cancel = new LDDModder.BrickEditor.Localization.LocalizableString();
            this.Label_Yes = new LDDModder.BrickEditor.Localization.LocalizableString();
            this.Label_No = new LDDModder.BrickEditor.Localization.LocalizableString();
            this.Label_Abort = new LDDModder.BrickEditor.Localization.LocalizableString();
            this.Label_Retry = new LDDModder.BrickEditor.Localization.LocalizableString();
            this.Label_Ignore = new LDDModder.BrickEditor.Localization.LocalizableString();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MessageIconBox)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.ErrorDetailTextBox, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.MessageTextLabel, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.MessageIconBox, 0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // flowLayoutPanel1
            // 
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.tableLayoutPanel1.SetColumnSpan(this.flowLayoutPanel1, 2);
            this.flowLayoutPanel1.Controls.Add(this.Option1Button);
            this.flowLayoutPanel1.Controls.Add(this.Option2Button);
            this.flowLayoutPanel1.Controls.Add(this.Option3Button);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // Option1Button
            // 
            resources.ApplyResources(this.Option1Button, "Option1Button");
            this.Option1Button.Name = "Option1Button";
            this.Option1Button.UseVisualStyleBackColor = true;
            // 
            // Option2Button
            // 
            resources.ApplyResources(this.Option2Button, "Option2Button");
            this.Option2Button.Name = "Option2Button";
            this.Option2Button.UseVisualStyleBackColor = true;
            // 
            // Option3Button
            // 
            resources.ApplyResources(this.Option3Button, "Option3Button");
            this.Option3Button.Name = "Option3Button";
            this.Option3Button.UseVisualStyleBackColor = true;
            // 
            // ErrorDetailTextBox
            // 
            resources.ApplyResources(this.ErrorDetailTextBox, "ErrorDetailTextBox");
            this.tableLayoutPanel1.SetColumnSpan(this.ErrorDetailTextBox, 2);
            this.ErrorDetailTextBox.Name = "ErrorDetailTextBox";
            this.ErrorDetailTextBox.ReadOnly = true;
            // 
            // MessageTextLabel
            // 
            resources.ApplyResources(this.MessageTextLabel, "MessageTextLabel");
            this.MessageTextLabel.Name = "MessageTextLabel";
            // 
            // MessageIconBox
            // 
            resources.ApplyResources(this.MessageIconBox, "MessageIconBox");
            this.MessageIconBox.Name = "MessageIconBox";
            this.MessageIconBox.TabStop = false;
            // 
            // ButtonLabels
            // 
            this.ButtonLabels.Items.AddRange(new LDDModder.BrickEditor.Localization.LocalizableString[] {
            this.Label_OK,
            this.Label_Cancel,
            this.Label_Yes,
            this.Label_No,
            this.Label_Abort,
            this.Label_Retry,
            this.Label_Ignore});
            // 
            // Label_OK
            // 
            resources.ApplyResources(this.Label_OK, "Label_OK");
            // 
            // Label_Cancel
            // 
            resources.ApplyResources(this.Label_Cancel, "Label_Cancel");
            // 
            // Label_Yes
            // 
            resources.ApplyResources(this.Label_Yes, "Label_Yes");
            // 
            // Label_No
            // 
            resources.ApplyResources(this.Label_No, "Label_No");
            // 
            // Label_Abort
            // 
            resources.ApplyResources(this.Label_Abort, "Label_Abort");
            // 
            // Label_Retry
            // 
            resources.ApplyResources(this.Label_Retry, "Label_Retry");
            // 
            // Label_Ignore
            // 
            resources.ApplyResources(this.Label_Ignore, "Label_Ignore");
            // 
            // MessageBoxEX
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MessageBoxEX";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MessageIconBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button Option1Button;
        private System.Windows.Forms.Button Option2Button;
        private System.Windows.Forms.Button Option3Button;
        private System.Windows.Forms.TextBox ErrorDetailTextBox;
        private System.Windows.Forms.Label MessageTextLabel;
        private System.Windows.Forms.PictureBox MessageIconBox;
        private Localization.LocalizableStringList ButtonLabels;
        private Localization.LocalizableString Label_OK;
        private Localization.LocalizableString Label_Cancel;
        private Localization.LocalizableString Label_Yes;
        private Localization.LocalizableString Label_No;
        private Localization.LocalizableString Label_Abort;
        private Localization.LocalizableString Label_Retry;
        private Localization.LocalizableString Label_Ignore;
    }
}