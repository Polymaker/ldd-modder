namespace LDDModder.BrickEditor.UI.Windows
{
    partial class ImportModelsDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImportModelsDialog));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.BrowseModelBox = new LDDModder.BrickEditor.UI.Controls.BrowseTextBox();
            this.ModelsGridView = new System.Windows.Forms.DataGridView();
            this.SelectionColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ModelNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TriangleCountColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TexturedColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.FlexibleColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.SurfaceColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.ImportButton = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.ReturnButton = new System.Windows.Forms.Button();
            this.CheckUncheckButton = new System.Windows.Forms.Button();
            this.WarningMessageLabel = new System.Windows.Forms.Label();
            this.LocalizableMessages = new LDDModder.BrickEditor.Localization.LocalizableStringList(this.components);
            this.WarningNotAllFlexible = new LDDModder.BrickEditor.Localization.LocalizableString();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ModelsGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.BrowseModelBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.ModelsGridView, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.ImportButton, 3, 3);
            this.tableLayoutPanel1.Controls.Add(this.progressBar1, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.ReturnButton, 4, 3);
            this.tableLayoutPanel1.Controls.Add(this.CheckUncheckButton, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.WarningMessageLabel, 2, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // BrowseModelBox
            // 
            this.BrowseModelBox.AutoSizeButton = true;
            this.BrowseModelBox.ButtonText = "Select model...";
            this.BrowseModelBox.ButtonWidth = 87;
            this.tableLayoutPanel1.SetColumnSpan(this.BrowseModelBox, 4);
            resources.ApplyResources(this.BrowseModelBox, "BrowseModelBox");
            this.BrowseModelBox.Name = "BrowseModelBox";
            this.BrowseModelBox.ReadOnly = true;
            this.BrowseModelBox.Value = "";
            this.BrowseModelBox.BrowseButtonClicked += new System.EventHandler(this.BrowseModelBox_BrowseButtonClicked);
            // 
            // ModelsGridView
            // 
            this.ModelsGridView.AllowUserToAddRows = false;
            this.ModelsGridView.AllowUserToDeleteRows = false;
            this.ModelsGridView.AllowUserToResizeRows = false;
            this.ModelsGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ModelsGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SelectionColumn,
            this.ModelNameColumn,
            this.TriangleCountColumn,
            this.TexturedColumn,
            this.FlexibleColumn,
            this.SurfaceColumn});
            this.tableLayoutPanel1.SetColumnSpan(this.ModelsGridView, 5);
            resources.ApplyResources(this.ModelsGridView, "ModelsGridView");
            this.ModelsGridView.Name = "ModelsGridView";
            this.ModelsGridView.RowHeadersVisible = false;
            this.ModelsGridView.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.ModelsGridView_CellEnter);
            this.ModelsGridView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.ModelsGridView_CellValueChanged);
            // 
            // SelectionColumn
            // 
            this.SelectionColumn.DataPropertyName = "Selected";
            resources.ApplyResources(this.SelectionColumn, "SelectionColumn");
            this.SelectionColumn.Name = "SelectionColumn";
            // 
            // ModelNameColumn
            // 
            this.ModelNameColumn.DataPropertyName = "Name";
            resources.ApplyResources(this.ModelNameColumn, "ModelNameColumn");
            this.ModelNameColumn.Name = "ModelNameColumn";
            this.ModelNameColumn.ReadOnly = true;
            // 
            // TriangleCountColumn
            // 
            this.TriangleCountColumn.DataPropertyName = "TriangleCount";
            resources.ApplyResources(this.TriangleCountColumn, "TriangleCountColumn");
            this.TriangleCountColumn.Name = "TriangleCountColumn";
            this.TriangleCountColumn.ReadOnly = true;
            // 
            // TexturedColumn
            // 
            this.TexturedColumn.DataPropertyName = "IsTextured";
            resources.ApplyResources(this.TexturedColumn, "TexturedColumn");
            this.TexturedColumn.Name = "TexturedColumn";
            this.TexturedColumn.ReadOnly = true;
            // 
            // FlexibleColumn
            // 
            this.FlexibleColumn.DataPropertyName = "IsFlexible";
            resources.ApplyResources(this.FlexibleColumn, "FlexibleColumn");
            this.FlexibleColumn.Name = "FlexibleColumn";
            this.FlexibleColumn.ReadOnly = true;
            // 
            // SurfaceColumn
            // 
            this.SurfaceColumn.DataPropertyName = "SurfaceID";
            resources.ApplyResources(this.SurfaceColumn, "SurfaceColumn");
            this.SurfaceColumn.Name = "SurfaceColumn";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // ImportButton
            // 
            resources.ApplyResources(this.ImportButton, "ImportButton");
            this.ImportButton.Name = "ImportButton";
            this.ImportButton.UseVisualStyleBackColor = true;
            this.ImportButton.Click += new System.EventHandler(this.ImportButton_Click);
            // 
            // progressBar1
            // 
            resources.ApplyResources(this.progressBar1, "progressBar1");
            this.tableLayoutPanel1.SetColumnSpan(this.progressBar1, 3);
            this.progressBar1.Name = "progressBar1";
            // 
            // ReturnButton
            // 
            this.ReturnButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.ReturnButton, "ReturnButton");
            this.ReturnButton.Name = "ReturnButton";
            this.ReturnButton.UseVisualStyleBackColor = true;
            // 
            // CheckUncheckButton
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.CheckUncheckButton, 2);
            resources.ApplyResources(this.CheckUncheckButton, "CheckUncheckButton");
            this.CheckUncheckButton.Name = "CheckUncheckButton";
            this.CheckUncheckButton.UseVisualStyleBackColor = true;
            this.CheckUncheckButton.Click += new System.EventHandler(this.CheckUncheckButton_Click);
            // 
            // WarningMessageLabel
            // 
            resources.ApplyResources(this.WarningMessageLabel, "WarningMessageLabel");
            this.WarningMessageLabel.Name = "WarningMessageLabel";
            // 
            // LocalizableMessages
            // 
            this.LocalizableMessages.Items.AddRange(new LDDModder.BrickEditor.Localization.LocalizableString[] {
            this.WarningNotAllFlexible});
            // 
            // WarningNotAllFlexible
            // 
            resources.ApplyResources(this.WarningNotAllFlexible, "WarningNotAllFlexible");
            // 
            // ImportModelsDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ReturnButton;
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ImportModelsDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ModelsGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button ReturnButton;
        private System.Windows.Forms.DataGridView ModelsGridView;
        private System.Windows.Forms.DataGridViewCheckBoxColumn SelectionColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ModelNameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn TriangleCountColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn TexturedColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn FlexibleColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn SurfaceColumn;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label1;
        private Controls.BrowseTextBox BrowseModelBox;
        private System.Windows.Forms.Button ImportButton;
        private System.Windows.Forms.Button CheckUncheckButton;
        private System.Windows.Forms.Label WarningMessageLabel;
        private Localization.LocalizableStringList LocalizableMessages;
        private Localization.LocalizableString WarningNotAllFlexible;
    }
}