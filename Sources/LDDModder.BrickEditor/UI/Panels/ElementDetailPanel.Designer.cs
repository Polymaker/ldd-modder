namespace LDDModder.BrickEditor.UI.Panels
{
    partial class ElementDetailPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ElementDetailPanel));
            this.NameTextBox = new System.Windows.Forms.TextBox();
            this.PropertiesTableLayout = new System.Windows.Forms.TableLayoutPanel();
            this.CollisionSizeLabel = new System.Windows.Forms.Label();
            this.CollisionRadiusLabel = new System.Windows.Forms.Label();
            this.SelectionTransformEdit = new LDDModder.BrickEditor.UI.Controls.TransformEditor();
            this.NameLabel = new System.Windows.Forms.Label();
            this.SubMaterialIndexLabel = new System.Windows.Forms.Label();
            this.SubMaterialIndexBox = new LDDModder.BrickEditor.UI.Controls.NumberTextBox();
            this.CollisionSizeEditor = new LDDModder.BrickEditor.UI.Editors.VectorEditor();
            this.CollisionRadiusBox = new LDDModder.BrickEditor.UI.Controls.NumberTextBox();
            this.SelectionInfoLabel = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.MainInfoTab = new System.Windows.Forms.TabPage();
            this.StudRefTab = new System.Windows.Forms.TabPage();
            this.StudRefGridView = new System.Windows.Forms.DataGridView();
            this.ConnectionColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.FieldIndexColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FieldPositionColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StudRefValue1Colunm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StudRefValue2Colunm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AdjStudColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ConnectionInfoTab = new System.Windows.Forms.TabPage();
            this.connectorEditor1 = new LDDModder.BrickEditor.UI.Controls.ConnectorEditor();
            this.localizableStringList1 = new LDDModder.BrickEditor.Localization.LocalizableStringList(this.components);
            this.MultiSelectionMsg = new LDDModder.BrickEditor.Localization.LocalizableString();
            this.NoSelectionMsg = new LDDModder.BrickEditor.Localization.LocalizableString();
            this.TopStudsLabel = new LDDModder.BrickEditor.Localization.LocalizableString();
            this.BottomStudsLabel = new LDDModder.BrickEditor.Localization.LocalizableString();
            this.NoConnectorRefLabel = new LDDModder.BrickEditor.Localization.LocalizableString();
            this.PropertiesTableLayout.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.MainInfoTab.SuspendLayout();
            this.StudRefTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.StudRefGridView)).BeginInit();
            this.ConnectionInfoTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // NameTextBox
            // 
            resources.ApplyResources(this.NameTextBox, "NameTextBox");
            this.NameTextBox.Name = "NameTextBox";
            this.NameTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.NameTextBox_Validating);
            this.NameTextBox.Validated += new System.EventHandler(this.NameTextBox_Validated);
            // 
            // PropertiesTableLayout
            // 
            resources.ApplyResources(this.PropertiesTableLayout, "PropertiesTableLayout");
            this.PropertiesTableLayout.Controls.Add(this.CollisionSizeLabel, 0, 4);
            this.PropertiesTableLayout.Controls.Add(this.CollisionRadiusLabel, 0, 3);
            this.PropertiesTableLayout.Controls.Add(this.SelectionTransformEdit, 0, 5);
            this.PropertiesTableLayout.Controls.Add(this.NameTextBox, 1, 1);
            this.PropertiesTableLayout.Controls.Add(this.NameLabel, 0, 1);
            this.PropertiesTableLayout.Controls.Add(this.SubMaterialIndexLabel, 0, 2);
            this.PropertiesTableLayout.Controls.Add(this.SubMaterialIndexBox, 1, 2);
            this.PropertiesTableLayout.Controls.Add(this.CollisionSizeEditor, 1, 4);
            this.PropertiesTableLayout.Controls.Add(this.CollisionRadiusBox, 1, 3);
            this.PropertiesTableLayout.Controls.Add(this.SelectionInfoLabel, 1, 0);
            this.PropertiesTableLayout.Name = "PropertiesTableLayout";
            // 
            // CollisionSizeLabel
            // 
            resources.ApplyResources(this.CollisionSizeLabel, "CollisionSizeLabel");
            this.CollisionSizeLabel.Name = "CollisionSizeLabel";
            // 
            // CollisionRadiusLabel
            // 
            resources.ApplyResources(this.CollisionRadiusLabel, "CollisionRadiusLabel");
            this.CollisionRadiusLabel.Name = "CollisionRadiusLabel";
            // 
            // SelectionTransformEdit
            // 
            resources.ApplyResources(this.SelectionTransformEdit, "SelectionTransformEdit");
            this.PropertiesTableLayout.SetColumnSpan(this.SelectionTransformEdit, 2);
            this.SelectionTransformEdit.Name = "SelectionTransformEdit";
            this.SelectionTransformEdit.ViewLayout = LDDModder.BrickEditor.UI.Controls.TransformEditor.EditLayout.Vertical;
            // 
            // NameLabel
            // 
            resources.ApplyResources(this.NameLabel, "NameLabel");
            this.NameLabel.Name = "NameLabel";
            // 
            // SubMaterialIndexLabel
            // 
            resources.ApplyResources(this.SubMaterialIndexLabel, "SubMaterialIndexLabel");
            this.SubMaterialIndexLabel.Name = "SubMaterialIndexLabel";
            // 
            // SubMaterialIndexBox
            // 
            resources.ApplyResources(this.SubMaterialIndexBox, "SubMaterialIndexBox");
            this.SubMaterialIndexBox.Name = "SubMaterialIndexBox";
            // 
            // CollisionSizeEditor
            // 
            resources.ApplyResources(this.CollisionSizeEditor, "CollisionSizeEditor");
            this.CollisionSizeEditor.Name = "CollisionSizeEditor";
            this.CollisionSizeEditor.ValueChanged += new System.EventHandler(this.CollisionSizeEditor_ValueChanged);
            // 
            // CollisionRadiusBox
            // 
            resources.ApplyResources(this.CollisionRadiusBox, "CollisionRadiusBox");
            this.CollisionRadiusBox.Name = "CollisionRadiusBox";
            this.CollisionRadiusBox.ValueChanged += new System.EventHandler(this.CollisionRadiusBox_ValueChanged);
            // 
            // SelectionInfoLabel
            // 
            resources.ApplyResources(this.SelectionInfoLabel, "SelectionInfoLabel");
            this.SelectionInfoLabel.Name = "SelectionInfoLabel";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.MainInfoTab);
            this.tabControl1.Controls.Add(this.StudRefTab);
            this.tabControl1.Controls.Add(this.ConnectionInfoTab);
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            // 
            // MainInfoTab
            // 
            resources.ApplyResources(this.MainInfoTab, "MainInfoTab");
            this.MainInfoTab.Controls.Add(this.PropertiesTableLayout);
            this.MainInfoTab.Name = "MainInfoTab";
            this.MainInfoTab.UseVisualStyleBackColor = true;
            // 
            // StudRefTab
            // 
            this.StudRefTab.Controls.Add(this.StudRefGridView);
            resources.ApplyResources(this.StudRefTab, "StudRefTab");
            this.StudRefTab.Name = "StudRefTab";
            this.StudRefTab.UseVisualStyleBackColor = true;
            // 
            // StudRefGridView
            // 
            this.StudRefGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.StudRefGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ConnectionColumn,
            this.FieldIndexColumn,
            this.FieldPositionColumn,
            this.StudRefValue1Colunm,
            this.StudRefValue2Colunm,
            this.AdjStudColumn});
            resources.ApplyResources(this.StudRefGridView, "StudRefGridView");
            this.StudRefGridView.Name = "StudRefGridView";
            this.StudRefGridView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.StudRefGridView_CellFormatting);
            this.StudRefGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.StudRefGridView_DataError);
            // 
            // ConnectionColumn
            // 
            this.ConnectionColumn.DataPropertyName = "ConnectionID";
            resources.ApplyResources(this.ConnectionColumn, "ConnectionColumn");
            this.ConnectionColumn.Name = "ConnectionColumn";
            // 
            // FieldIndexColumn
            // 
            this.FieldIndexColumn.DataPropertyName = "FieldIndex";
            resources.ApplyResources(this.FieldIndexColumn, "FieldIndexColumn");
            this.FieldIndexColumn.Name = "FieldIndexColumn";
            // 
            // FieldPositionColumn
            // 
            resources.ApplyResources(this.FieldPositionColumn, "FieldPositionColumn");
            this.FieldPositionColumn.Name = "FieldPositionColumn";
            // 
            // StudRefValue1Colunm
            // 
            this.StudRefValue1Colunm.DataPropertyName = "Value1";
            resources.ApplyResources(this.StudRefValue1Colunm, "StudRefValue1Colunm");
            this.StudRefValue1Colunm.Name = "StudRefValue1Colunm";
            this.StudRefValue1Colunm.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // StudRefValue2Colunm
            // 
            this.StudRefValue2Colunm.DataPropertyName = "Value2";
            resources.ApplyResources(this.StudRefValue2Colunm, "StudRefValue2Colunm");
            this.StudRefValue2Colunm.Name = "StudRefValue2Colunm";
            this.StudRefValue2Colunm.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // AdjStudColumn
            // 
            resources.ApplyResources(this.AdjStudColumn, "AdjStudColumn");
            this.AdjStudColumn.Name = "AdjStudColumn";
            // 
            // ConnectionInfoTab
            // 
            this.ConnectionInfoTab.Controls.Add(this.connectorEditor1);
            resources.ApplyResources(this.ConnectionInfoTab, "ConnectionInfoTab");
            this.ConnectionInfoTab.Name = "ConnectionInfoTab";
            this.ConnectionInfoTab.UseVisualStyleBackColor = true;
            // 
            // connectorEditor1
            // 
            resources.ApplyResources(this.connectorEditor1, "connectorEditor1");
            this.connectorEditor1.Name = "connectorEditor1";
            // 
            // localizableStringList1
            // 
            this.localizableStringList1.Items.AddRange(new LDDModder.BrickEditor.Localization.LocalizableString[] {
            this.MultiSelectionMsg,
            this.NoSelectionMsg,
            this.TopStudsLabel,
            this.BottomStudsLabel,
            this.NoConnectorRefLabel});
            // 
            // MultiSelectionMsg
            // 
            resources.ApplyResources(this.MultiSelectionMsg, "MultiSelectionMsg");
            // 
            // NoSelectionMsg
            // 
            resources.ApplyResources(this.NoSelectionMsg, "NoSelectionMsg");
            // 
            // TopStudsLabel
            // 
            resources.ApplyResources(this.TopStudsLabel, "TopStudsLabel");
            // 
            // BottomStudsLabel
            // 
            resources.ApplyResources(this.BottomStudsLabel, "BottomStudsLabel");
            // 
            // NoConnectorRefLabel
            // 
            resources.ApplyResources(this.NoConnectorRefLabel, "NoConnectorRefLabel");
            // 
            // ElementDetailPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "ElementDetailPanel";
            this.PropertiesTableLayout.ResumeLayout(false);
            this.PropertiesTableLayout.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.MainInfoTab.ResumeLayout(false);
            this.MainInfoTab.PerformLayout();
            this.StudRefTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.StudRefGridView)).EndInit();
            this.ConnectionInfoTab.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Controls.TransformEditor SelectionTransformEdit;
        private System.Windows.Forms.TextBox NameTextBox;
        private System.Windows.Forms.TableLayoutPanel PropertiesTableLayout;
        private System.Windows.Forms.Label NameLabel;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage MainInfoTab;
        private System.Windows.Forms.TabPage StudRefTab;
        private System.Windows.Forms.TabPage ConnectionInfoTab;
        private System.Windows.Forms.Label CollisionSizeLabel;
        private System.Windows.Forms.Label CollisionRadiusLabel;
        private System.Windows.Forms.Label SubMaterialIndexLabel;
        private Controls.NumberTextBox SubMaterialIndexBox;
        private Editors.VectorEditor CollisionSizeEditor;
        private Controls.NumberTextBox CollisionRadiusBox;
        private System.Windows.Forms.Label SelectionInfoLabel;
        private Localization.LocalizableStringList localizableStringList1;
        private Localization.LocalizableString MultiSelectionMsg;
        private Localization.LocalizableString NoSelectionMsg;
        private Localization.LocalizableString TopStudsLabel;
        private Localization.LocalizableString BottomStudsLabel;
        private Localization.LocalizableString NoConnectorRefLabel;
        private System.Windows.Forms.DataGridView StudRefGridView;
        private System.Windows.Forms.DataGridViewComboBoxColumn ConnectionColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn FieldIndexColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn FieldPositionColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn StudRefValue1Colunm;
        private System.Windows.Forms.DataGridViewTextBoxColumn StudRefValue2Colunm;
        private System.Windows.Forms.DataGridViewCheckBoxColumn AdjStudColumn;
        private Controls.ConnectorEditor connectorEditor1;
    }
}