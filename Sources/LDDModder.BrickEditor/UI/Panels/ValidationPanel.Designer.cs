namespace LDDModder.BrickEditor.UI.Panels
{
    partial class ValidationPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ValidationPanel));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.ValidatePartButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ToggleErrorsButton = new System.Windows.Forms.ToolStripButton();
            this.ToggleWarningsButton = new System.Windows.Forms.ToolStripButton();
            this.ToggleMessagesButton = new System.Windows.Forms.ToolStripButton();
            this.ValidationMessageList = new BrightIdeasSoftware.ObjectListView();
            this.ColumnMessageType = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.ColumnMessageCode = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.ColumnMessageDescription = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.ColumnMessageSource = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.localizableStringList1 = new LDDModder.BrickEditor.Localization.LocalizableStringList(this.components);
            this.ErrorCountText = new LDDModder.BrickEditor.Localization.LocalizableString();
            this.WarningCountText = new LDDModder.BrickEditor.Localization.LocalizableString();
            this.MessageCountText = new LDDModder.BrickEditor.Localization.LocalizableString();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ValidationMessageList)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ValidatePartButton,
            this.toolStripSeparator1,
            this.ToggleErrorsButton,
            this.ToggleWarningsButton,
            this.ToggleMessagesButton});
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            // 
            // ValidatePartButton
            // 
            this.ValidatePartButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(this.ValidatePartButton, "ValidatePartButton");
            this.ValidatePartButton.Name = "ValidatePartButton";
            this.ValidatePartButton.Click += new System.EventHandler(this.ValidatePartButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // ToggleErrorsButton
            // 
            this.ToggleErrorsButton.Checked = true;
            this.ToggleErrorsButton.CheckOnClick = true;
            this.ToggleErrorsButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ToggleErrorsButton.Image = global::LDDModder.BrickEditor.Properties.Resources.StatusError;
            resources.ApplyResources(this.ToggleErrorsButton, "ToggleErrorsButton");
            this.ToggleErrorsButton.Margin = new System.Windows.Forms.Padding(0, 1, 3, 2);
            this.ToggleErrorsButton.Name = "ToggleErrorsButton";
            this.ToggleErrorsButton.CheckedChanged += new System.EventHandler(this.ToggleStatusButtons_CheckedChanged);
            // 
            // ToggleWarningsButton
            // 
            this.ToggleWarningsButton.Checked = true;
            this.ToggleWarningsButton.CheckOnClick = true;
            this.ToggleWarningsButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ToggleWarningsButton.Image = global::LDDModder.BrickEditor.Properties.Resources.StatusWarning;
            resources.ApplyResources(this.ToggleWarningsButton, "ToggleWarningsButton");
            this.ToggleWarningsButton.Margin = new System.Windows.Forms.Padding(0, 1, 3, 2);
            this.ToggleWarningsButton.Name = "ToggleWarningsButton";
            this.ToggleWarningsButton.CheckedChanged += new System.EventHandler(this.ToggleStatusButtons_CheckedChanged);
            // 
            // ToggleMessagesButton
            // 
            this.ToggleMessagesButton.Checked = true;
            this.ToggleMessagesButton.CheckOnClick = true;
            this.ToggleMessagesButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ToggleMessagesButton.Image = global::LDDModder.BrickEditor.Properties.Resources.StatusInfo;
            resources.ApplyResources(this.ToggleMessagesButton, "ToggleMessagesButton");
            this.ToggleMessagesButton.Name = "ToggleMessagesButton";
            this.ToggleMessagesButton.CheckedChanged += new System.EventHandler(this.ToggleStatusButtons_CheckedChanged);
            // 
            // ValidationMessageList
            // 
            this.ValidationMessageList.AllColumns.Add(this.ColumnMessageType);
            this.ValidationMessageList.AllColumns.Add(this.ColumnMessageCode);
            this.ValidationMessageList.AllColumns.Add(this.ColumnMessageDescription);
            this.ValidationMessageList.AllColumns.Add(this.ColumnMessageSource);
            this.ValidationMessageList.CellEditUseWholeCell = false;
            this.ValidationMessageList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColumnMessageType,
            this.ColumnMessageDescription,
            this.ColumnMessageSource});
            this.ValidationMessageList.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.ValidationMessageList, "ValidationMessageList");
            this.ValidationMessageList.FullRowSelect = true;
            this.ValidationMessageList.HideSelection = false;
            this.ValidationMessageList.Name = "ValidationMessageList";
            this.ValidationMessageList.ShowGroups = false;
            this.ValidationMessageList.UseCompatibleStateImageBehavior = false;
            this.ValidationMessageList.UseFiltering = true;
            this.ValidationMessageList.View = System.Windows.Forms.View.Details;
            this.ValidationMessageList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ValidationMessageList_MouseDoubleClick);
            // 
            // ColumnMessageType
            // 
            this.ColumnMessageType.AspectName = "";
            this.ColumnMessageType.MaximumWidth = 24;
            this.ColumnMessageType.MinimumWidth = 20;
            this.ColumnMessageType.ShowTextInHeader = false;
            this.ColumnMessageType.Sortable = false;
            resources.ApplyResources(this.ColumnMessageType, "ColumnMessageType");
            // 
            // ColumnMessageCode
            // 
            this.ColumnMessageCode.AspectName = "Code";
            resources.ApplyResources(this.ColumnMessageCode, "ColumnMessageCode");
            this.ColumnMessageCode.IsVisible = false;
            // 
            // ColumnMessageDescription
            // 
            this.ColumnMessageDescription.AspectName = "Code";
            this.ColumnMessageDescription.FillsFreeSpace = true;
            resources.ApplyResources(this.ColumnMessageDescription, "ColumnMessageDescription");
            // 
            // ColumnMessageSource
            // 
            this.ColumnMessageSource.AspectName = "SourceKey";
            this.ColumnMessageSource.MinimumWidth = 50;
            resources.ApplyResources(this.ColumnMessageSource, "ColumnMessageSource");
            // 
            // localizableStringList1
            // 
            this.localizableStringList1.Items.AddRange(new LDDModder.BrickEditor.Localization.LocalizableString[] {
            this.ErrorCountText,
            this.WarningCountText,
            this.MessageCountText});
            // 
            // ErrorCountText
            // 
            resources.ApplyResources(this.ErrorCountText, "ErrorCountText");
            // 
            // WarningCountText
            // 
            resources.ApplyResources(this.WarningCountText, "WarningCountText");
            // 
            // MessageCountText
            // 
            resources.ApplyResources(this.MessageCountText, "MessageCountText");
            // 
            // ValidationPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ValidationMessageList);
            this.Controls.Add(this.toolStrip1);
            this.Name = "ValidationPanel";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ValidationMessageList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton ValidatePartButton;
        private System.Windows.Forms.ToolStripButton ToggleErrorsButton;
        private System.Windows.Forms.ToolStripButton ToggleWarningsButton;
        private System.Windows.Forms.ToolStripButton ToggleMessagesButton;
        private BrightIdeasSoftware.ObjectListView ValidationMessageList;
        private BrightIdeasSoftware.OLVColumn ColumnMessageType;
        private BrightIdeasSoftware.OLVColumn ColumnMessageCode;
        private BrightIdeasSoftware.OLVColumn ColumnMessageDescription;
        private BrightIdeasSoftware.OLVColumn ColumnMessageSource;
        private Localization.LocalizableStringList localizableStringList1;
        private Localization.LocalizableString ErrorCountText;
        private Localization.LocalizableString WarningCountText;
        private Localization.LocalizableString MessageCountText;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    }
}