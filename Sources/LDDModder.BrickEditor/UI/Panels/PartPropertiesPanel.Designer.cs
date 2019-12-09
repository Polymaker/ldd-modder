namespace LDDModder.BrickEditor.UI.Panels
{
    partial class PartPropertiesPanel
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
            LDDModder.LDD.Primitives.BoundingBox boundingBox2 = new LDDModder.LDD.Primitives.BoundingBox();
            LDDModder.LDD.Primitives.BoundingBox boundingBox1 = new LDDModder.LDD.Primitives.BoundingBox();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PartPropertiesPanel));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.LabelAliases = new System.Windows.Forms.Label();
            this.LabelPlatform = new System.Windows.Forms.Label();
            this.DescriptionTextBox = new System.Windows.Forms.TextBox();
            this.LabelPartID = new System.Windows.Forms.Label();
            this.CategoryComboBox = new System.Windows.Forms.ComboBox();
            this.PartIDTextBox = new LDDModder.BrickEditor.UI.Controls.NumberTextBox();
            this.LabelDescription = new System.Windows.Forms.Label();
            this.LabelCategory = new System.Windows.Forms.Label();
            this.PlatformComboBox = new System.Windows.Forms.ComboBox();
            this.AliasesButtonBox = new LDDModder.BrickEditor.UI.Controls.BrowseTextBox();
            this.LabelGeomBounding = new System.Windows.Forms.Label();
            this.LabelBounding = new System.Windows.Forms.Label();
            this.GeomBoundingEditor = new LDDModder.BrickEditor.UI.Controls.BoundingBoxEditor();
            this.CalcBoundingButton = new System.Windows.Forms.Button();
            this.BoundingEditor = new LDDModder.BrickEditor.UI.Controls.BoundingBoxEditor();
            this.CalcGeomBoundingButton = new System.Windows.Forms.Button();
            this.collapsiblePanel1 = new LDDModder.BrickEditor.UI.Controls.CollapsiblePanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.collapsiblePanel2 = new LDDModder.BrickEditor.UI.Controls.CollapsiblePanel();
            this.collapsiblePanel3 = new LDDModder.BrickEditor.UI.Controls.CollapsiblePanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.LabelInertiaTensor = new System.Windows.Forms.Label();
            this.LabelCenterOfMass = new System.Windows.Forms.Label();
            this.InertiaTensorTextBox = new System.Windows.Forms.TextBox();
            this.LabelMass = new System.Windows.Forms.Label();
            this.MassNumberBox = new LDDModder.BrickEditor.UI.Controls.NumberTextBox();
            this.LabelFriction = new System.Windows.Forms.Label();
            this.FrictionCheckBox = new System.Windows.Forms.CheckBox();
            this.collapsiblePanel4 = new LDDModder.BrickEditor.UI.Controls.CollapsiblePanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.collapsiblePanel1.ContentPanel.SuspendLayout();
            this.collapsiblePanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.collapsiblePanel2.ContentPanel.SuspendLayout();
            this.collapsiblePanel2.SuspendLayout();
            this.collapsiblePanel3.ContentPanel.SuspendLayout();
            this.collapsiblePanel3.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.collapsiblePanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 85F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 85F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.LabelAliases, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.LabelPlatform, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.DescriptionTextBox, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.LabelPartID, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.CategoryComboBox, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.PartIDTextBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.LabelDescription, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.LabelCategory, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.PlatformComboBox, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.AliasesButtonBox, 3, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 3);
            this.tableLayoutPanel1.MaximumSize = new System.Drawing.Size(400, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(400, 163);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // LabelAliases
            // 
            this.LabelAliases.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.LabelAliases.AutoSize = true;
            this.LabelAliases.Location = new System.Drawing.Point(222, 6);
            this.LabelAliases.Name = "LabelAliases";
            this.LabelAliases.Size = new System.Drawing.Size(40, 13);
            this.LabelAliases.TabIndex = 3;
            this.LabelAliases.Text = "Aliases";
            // 
            // LabelPlatform
            // 
            this.LabelPlatform.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.LabelPlatform.AutoSize = true;
            this.LabelPlatform.Location = new System.Drawing.Point(3, 74);
            this.LabelPlatform.Margin = new System.Windows.Forms.Padding(3);
            this.LabelPlatform.Name = "LabelPlatform";
            this.LabelPlatform.Size = new System.Drawing.Size(45, 13);
            this.LabelPlatform.TabIndex = 6;
            this.LabelPlatform.Text = "Platform";
            // 
            // DescriptionTextBox
            // 
            this.DescriptionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.DescriptionTextBox, 4);
            this.DescriptionTextBox.Location = new System.Drawing.Point(3, 48);
            this.DescriptionTextBox.Name = "DescriptionTextBox";
            this.DescriptionTextBox.Size = new System.Drawing.Size(394, 20);
            this.DescriptionTextBox.TabIndex = 1;
            this.DescriptionTextBox.Validated += new System.EventHandler(this.DescriptionTextBox_Validated);
            // 
            // LabelPartID
            // 
            this.LabelPartID.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.LabelPartID.AutoSize = true;
            this.LabelPartID.Location = new System.Drawing.Point(3, 6);
            this.LabelPartID.Name = "LabelPartID";
            this.LabelPartID.Size = new System.Drawing.Size(40, 13);
            this.LabelPartID.TabIndex = 2;
            this.LabelPartID.Text = "Part ID";
            // 
            // CategoryComboBox
            // 
            this.CategoryComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.CategoryComboBox, 4);
            this.CategoryComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CategoryComboBox.FormattingEnabled = true;
            this.CategoryComboBox.Location = new System.Drawing.Point(3, 139);
            this.CategoryComboBox.Name = "CategoryComboBox";
            this.CategoryComboBox.Size = new System.Drawing.Size(394, 21);
            this.CategoryComboBox.TabIndex = 5;
            this.CategoryComboBox.SelectedValueChanged += new System.EventHandler(this.CategoryComboBox_SelectedValueChanged);
            // 
            // PartIDTextBox
            // 
            this.PartIDTextBox.AllowDecimals = false;
            this.PartIDTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PartIDTextBox.Location = new System.Drawing.Point(88, 3);
            this.PartIDTextBox.MaximumValue = 9999999D;
            this.PartIDTextBox.Name = "PartIDTextBox";
            this.PartIDTextBox.Size = new System.Drawing.Size(109, 20);
            this.PartIDTextBox.TabIndex = 8;
            this.PartIDTextBox.ValueChanged += new System.EventHandler(this.PartIDTextBox_ValueChanged);
            // 
            // LabelDescription
            // 
            this.LabelDescription.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.LabelDescription.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.LabelDescription, 2);
            this.LabelDescription.Location = new System.Drawing.Point(3, 29);
            this.LabelDescription.Margin = new System.Windows.Forms.Padding(3);
            this.LabelDescription.Name = "LabelDescription";
            this.LabelDescription.Size = new System.Drawing.Size(60, 13);
            this.LabelDescription.TabIndex = 3;
            this.LabelDescription.Text = "Description";
            // 
            // LabelCategory
            // 
            this.LabelCategory.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.LabelCategory.AutoSize = true;
            this.LabelCategory.Location = new System.Drawing.Point(3, 120);
            this.LabelCategory.Margin = new System.Windows.Forms.Padding(3);
            this.LabelCategory.Name = "LabelCategory";
            this.LabelCategory.Size = new System.Drawing.Size(49, 13);
            this.LabelCategory.TabIndex = 7;
            this.LabelCategory.Text = "Category";
            // 
            // PlatformComboBox
            // 
            this.PlatformComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.PlatformComboBox, 4);
            this.PlatformComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PlatformComboBox.FormattingEnabled = true;
            this.PlatformComboBox.Location = new System.Drawing.Point(3, 93);
            this.PlatformComboBox.Name = "PlatformComboBox";
            this.PlatformComboBox.Size = new System.Drawing.Size(394, 21);
            this.PlatformComboBox.TabIndex = 4;
            this.PlatformComboBox.SelectedValueChanged += new System.EventHandler(this.PlatformComboBox_SelectedValueChanged);
            // 
            // AliasesButtonBox
            // 
            this.AliasesButtonBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AliasesButtonBox.AutoSizeButton = true;
            this.AliasesButtonBox.ButtonText = "...";
            this.AliasesButtonBox.ButtonWidth = 26;
            this.AliasesButtonBox.Enabled = false;
            this.AliasesButtonBox.Location = new System.Drawing.Point(288, 3);
            this.AliasesButtonBox.Name = "AliasesButtonBox";
            this.AliasesButtonBox.ReadOnly = true;
            this.AliasesButtonBox.Size = new System.Drawing.Size(109, 20);
            this.AliasesButtonBox.TabIndex = 9;
            this.AliasesButtonBox.Value = "";
            // 
            // LabelGeomBounding
            // 
            this.LabelGeomBounding.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.LabelGeomBounding.AutoSize = true;
            this.LabelGeomBounding.Location = new System.Drawing.Point(3, 73);
            this.LabelGeomBounding.Name = "LabelGeomBounding";
            this.LabelGeomBounding.Size = new System.Drawing.Size(100, 13);
            this.LabelGeomBounding.TabIndex = 12;
            this.LabelGeomBounding.Text = "Geometry Bounding";
            // 
            // LabelBounding
            // 
            this.LabelBounding.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.LabelBounding.AutoSize = true;
            this.LabelBounding.Location = new System.Drawing.Point(3, 5);
            this.LabelBounding.Name = "LabelBounding";
            this.LabelBounding.Size = new System.Drawing.Size(52, 13);
            this.LabelBounding.TabIndex = 10;
            this.LabelBounding.Text = "Bounding";
            // 
            // GeomBoundingEditor
            // 
            this.GeomBoundingEditor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.SetColumnSpan(this.GeomBoundingEditor, 2);
            this.GeomBoundingEditor.Location = new System.Drawing.Point(3, 94);
            this.GeomBoundingEditor.Name = "GeomBoundingEditor";
            this.GeomBoundingEditor.Size = new System.Drawing.Size(394, 39);
            this.GeomBoundingEditor.TabIndex = 13;
            boundingBox2.Max = ((LDDModder.Simple3D.Vector3)(resources.GetObject("boundingBox2.Max")));
            boundingBox2.MaxX = 0F;
            boundingBox2.MaxY = 0F;
            boundingBox2.MaxZ = 0F;
            boundingBox2.Min = ((LDDModder.Simple3D.Vector3)(resources.GetObject("boundingBox2.Min")));
            boundingBox2.MinX = 0F;
            boundingBox2.MinY = 0F;
            boundingBox2.MinZ = 0F;
            this.GeomBoundingEditor.Value = boundingBox2;
            // 
            // CalcBoundingButton
            // 
            this.CalcBoundingButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.CalcBoundingButton.Location = new System.Drawing.Point(323, 0);
            this.CalcBoundingButton.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.CalcBoundingButton.Name = "CalcBoundingButton";
            this.CalcBoundingButton.Size = new System.Drawing.Size(74, 23);
            this.CalcBoundingButton.TabIndex = 11;
            this.CalcBoundingButton.Text = "Calculate";
            this.CalcBoundingButton.UseVisualStyleBackColor = true;
            this.CalcBoundingButton.Click += new System.EventHandler(this.CalcBoundingButton_Click);
            // 
            // BoundingEditor
            // 
            this.BoundingEditor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.SetColumnSpan(this.BoundingEditor, 2);
            this.BoundingEditor.Location = new System.Drawing.Point(3, 26);
            this.BoundingEditor.Name = "BoundingEditor";
            this.BoundingEditor.Size = new System.Drawing.Size(394, 39);
            this.BoundingEditor.TabIndex = 9;
            boundingBox1.Max = ((LDDModder.Simple3D.Vector3)(resources.GetObject("boundingBox1.Max")));
            boundingBox1.MaxX = 0F;
            boundingBox1.MaxY = 0F;
            boundingBox1.MaxZ = 0F;
            boundingBox1.Min = ((LDDModder.Simple3D.Vector3)(resources.GetObject("boundingBox1.Min")));
            boundingBox1.MinX = 0F;
            boundingBox1.MinY = 0F;
            boundingBox1.MinZ = 0F;
            this.BoundingEditor.Value = boundingBox1;
            // 
            // CalcGeomBoundingButton
            // 
            this.CalcGeomBoundingButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.CalcGeomBoundingButton.Location = new System.Drawing.Point(323, 68);
            this.CalcGeomBoundingButton.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.CalcGeomBoundingButton.Name = "CalcGeomBoundingButton";
            this.CalcGeomBoundingButton.Size = new System.Drawing.Size(74, 23);
            this.CalcGeomBoundingButton.TabIndex = 14;
            this.CalcGeomBoundingButton.Text = "Calculate";
            this.CalcGeomBoundingButton.UseVisualStyleBackColor = true;
            this.CalcGeomBoundingButton.Click += new System.EventHandler(this.CalcGeomBoundingButton_Click);
            // 
            // collapsiblePanel1
            // 
            this.collapsiblePanel1.AutoSizeHeight = true;
            // 
            // collapsiblePanel1.ContentPanel
            // 
            this.collapsiblePanel1.ContentPanel.BackColor = System.Drawing.SystemColors.Control;
            this.collapsiblePanel1.ContentPanel.Controls.Add(this.tableLayoutPanel2);
            this.collapsiblePanel1.ContentPanel.Location = new System.Drawing.Point(0, 23);
            this.collapsiblePanel1.ContentPanel.Name = "ContentPanel";
            this.collapsiblePanel1.ContentPanel.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.collapsiblePanel1.ContentPanel.Size = new System.Drawing.Size(793, 142);
            this.collapsiblePanel1.ContentPanel.TabIndex = 0;
            this.collapsiblePanel1.DisplayStyle = LDDModder.BrickEditor.UI.Controls.CollapsiblePanel.HeaderDisplayStyle.Button;
            this.collapsiblePanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.collapsiblePanel1.Location = new System.Drawing.Point(0, 192);
            this.collapsiblePanel1.Name = "collapsiblePanel1";
            this.collapsiblePanel1.PanelHeight = 142;
            this.collapsiblePanel1.Size = new System.Drawing.Size(793, 165);
            this.collapsiblePanel1.TabIndex = 1;
            this.collapsiblePanel1.Text = "Part Boundings";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.LabelBounding, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.CalcBoundingButton, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.BoundingEditor, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.LabelGeomBounding, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.CalcGeomBoundingButton, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.GeomBoundingEditor, 0, 3);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 3);
            this.tableLayoutPanel2.MaximumSize = new System.Drawing.Size(400, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(400, 136);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // collapsiblePanel2
            // 
            this.collapsiblePanel2.AutoSizeHeight = true;
            // 
            // collapsiblePanel2.ContentPanel
            // 
            this.collapsiblePanel2.ContentPanel.BackColor = System.Drawing.SystemColors.Control;
            this.collapsiblePanel2.ContentPanel.Controls.Add(this.tableLayoutPanel1);
            this.collapsiblePanel2.ContentPanel.Location = new System.Drawing.Point(0, 23);
            this.collapsiblePanel2.ContentPanel.Name = "ContentPanel";
            this.collapsiblePanel2.ContentPanel.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.collapsiblePanel2.ContentPanel.Size = new System.Drawing.Size(793, 169);
            this.collapsiblePanel2.ContentPanel.TabIndex = 0;
            this.collapsiblePanel2.DisplayStyle = LDDModder.BrickEditor.UI.Controls.CollapsiblePanel.HeaderDisplayStyle.Button;
            this.collapsiblePanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.collapsiblePanel2.Location = new System.Drawing.Point(0, 0);
            this.collapsiblePanel2.Name = "collapsiblePanel2";
            this.collapsiblePanel2.PanelHeight = 169;
            this.collapsiblePanel2.Size = new System.Drawing.Size(793, 192);
            this.collapsiblePanel2.TabIndex = 2;
            this.collapsiblePanel2.Text = "Part Description";
            // 
            // collapsiblePanel3
            // 
            this.collapsiblePanel3.AutoSizeHeight = true;
            // 
            // collapsiblePanel3.ContentPanel
            // 
            this.collapsiblePanel3.ContentPanel.Controls.Add(this.tableLayoutPanel3);
            this.collapsiblePanel3.ContentPanel.Location = new System.Drawing.Point(0, 23);
            this.collapsiblePanel3.ContentPanel.Name = "ContentPanel";
            this.collapsiblePanel3.ContentPanel.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.collapsiblePanel3.ContentPanel.Size = new System.Drawing.Size(793, 91);
            this.collapsiblePanel3.ContentPanel.TabIndex = 0;
            this.collapsiblePanel3.DisplayStyle = LDDModder.BrickEditor.UI.Controls.CollapsiblePanel.HeaderDisplayStyle.Button;
            this.collapsiblePanel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.collapsiblePanel3.Location = new System.Drawing.Point(0, 357);
            this.collapsiblePanel3.Name = "collapsiblePanel3";
            this.collapsiblePanel3.PanelHeight = 91;
            this.collapsiblePanel3.Size = new System.Drawing.Size(793, 114);
            this.collapsiblePanel3.TabIndex = 3;
            this.collapsiblePanel3.Text = "Physical Properties";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.AutoSize = true;
            this.tableLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.LabelInertiaTensor, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.LabelCenterOfMass, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.InertiaTensorTextBox, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.LabelMass, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.MassNumberBox, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.LabelFriction, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.FrictionCheckBox, 1, 3);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 3);
            this.tableLayoutPanel3.MaximumSize = new System.Drawing.Size(400, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 4;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Size = new System.Drawing.Size(400, 85);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // LabelInertiaTensor
            // 
            this.LabelInertiaTensor.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.LabelInertiaTensor.AutoSize = true;
            this.LabelInertiaTensor.Location = new System.Drawing.Point(3, 6);
            this.LabelInertiaTensor.Name = "LabelInertiaTensor";
            this.LabelInertiaTensor.Size = new System.Drawing.Size(72, 13);
            this.LabelInertiaTensor.TabIndex = 0;
            this.LabelInertiaTensor.Text = "Inertia Tensor";
            // 
            // LabelCenterOfMass
            // 
            this.LabelCenterOfMass.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.LabelCenterOfMass.AutoSize = true;
            this.LabelCenterOfMass.Location = new System.Drawing.Point(3, 26);
            this.LabelCenterOfMass.Name = "LabelCenterOfMass";
            this.LabelCenterOfMass.Size = new System.Drawing.Size(77, 13);
            this.LabelCenterOfMass.TabIndex = 1;
            this.LabelCenterOfMass.Text = "Center of mass";
            // 
            // InertiaTensorTextBox
            // 
            this.InertiaTensorTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InertiaTensorTextBox.Location = new System.Drawing.Point(103, 3);
            this.InertiaTensorTextBox.Name = "InertiaTensorTextBox";
            this.InertiaTensorTextBox.Size = new System.Drawing.Size(294, 20);
            this.InertiaTensorTextBox.TabIndex = 2;
            // 
            // LabelMass
            // 
            this.LabelMass.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.LabelMass.AutoSize = true;
            this.LabelMass.Location = new System.Drawing.Point(3, 45);
            this.LabelMass.Name = "LabelMass";
            this.LabelMass.Size = new System.Drawing.Size(32, 13);
            this.LabelMass.TabIndex = 3;
            this.LabelMass.Text = "Mass";
            // 
            // MassNumberBox
            // 
            this.MassNumberBox.Location = new System.Drawing.Point(103, 42);
            this.MassNumberBox.Name = "MassNumberBox";
            this.MassNumberBox.Size = new System.Drawing.Size(100, 20);
            this.MassNumberBox.TabIndex = 4;
            // 
            // LabelFriction
            // 
            this.LabelFriction.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.LabelFriction.AutoSize = true;
            this.LabelFriction.Location = new System.Drawing.Point(3, 68);
            this.LabelFriction.Name = "LabelFriction";
            this.LabelFriction.Size = new System.Drawing.Size(68, 13);
            this.LabelFriction.TabIndex = 5;
            this.LabelFriction.Text = "Friction Type";
            // 
            // FrictionCheckBox
            // 
            this.FrictionCheckBox.AutoSize = true;
            this.FrictionCheckBox.Location = new System.Drawing.Point(103, 68);
            this.FrictionCheckBox.Name = "FrictionCheckBox";
            this.FrictionCheckBox.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.FrictionCheckBox.Size = new System.Drawing.Size(25, 14);
            this.FrictionCheckBox.TabIndex = 6;
            this.FrictionCheckBox.UseVisualStyleBackColor = true;
            // 
            // collapsiblePanel4
            // 
            // 
            // collapsiblePanel4.ContentPanel
            // 
            this.collapsiblePanel4.ContentPanel.Location = new System.Drawing.Point(0, 23);
            this.collapsiblePanel4.ContentPanel.Name = "ContentPanel";
            this.collapsiblePanel4.ContentPanel.Size = new System.Drawing.Size(793, 83);
            this.collapsiblePanel4.ContentPanel.TabIndex = 0;
            this.collapsiblePanel4.DisplayStyle = LDDModder.BrickEditor.UI.Controls.CollapsiblePanel.HeaderDisplayStyle.Button;
            this.collapsiblePanel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.collapsiblePanel4.Location = new System.Drawing.Point(0, 471);
            this.collapsiblePanel4.Name = "collapsiblePanel4";
            this.collapsiblePanel4.PanelHeight = 83;
            this.collapsiblePanel4.Size = new System.Drawing.Size(793, 106);
            this.collapsiblePanel4.TabIndex = 4;
            this.collapsiblePanel4.Text = "Extra Properties";
            // 
            // PartPropertiesPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(810, 449);
            this.Controls.Add(this.collapsiblePanel4);
            this.Controls.Add(this.collapsiblePanel3);
            this.Controls.Add(this.collapsiblePanel1);
            this.Controls.Add(this.collapsiblePanel2);
            this.Name = "PartPropertiesPanel";
            this.Text = "Part Properties";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.collapsiblePanel1.ContentPanel.ResumeLayout(false);
            this.collapsiblePanel1.ContentPanel.PerformLayout();
            this.collapsiblePanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.collapsiblePanel2.ContentPanel.ResumeLayout(false);
            this.collapsiblePanel2.ContentPanel.PerformLayout();
            this.collapsiblePanel2.ResumeLayout(false);
            this.collapsiblePanel3.ContentPanel.ResumeLayout(false);
            this.collapsiblePanel3.ContentPanel.PerformLayout();
            this.collapsiblePanel3.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.collapsiblePanel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox DescriptionTextBox;
        private System.Windows.Forms.Label LabelPartID;
        private System.Windows.Forms.Label LabelDescription;
        private System.Windows.Forms.Label LabelCategory;
        private System.Windows.Forms.Label LabelPlatform;
        private System.Windows.Forms.ComboBox PlatformComboBox;
        private System.Windows.Forms.ComboBox CategoryComboBox;
        private Controls.NumberTextBox PartIDTextBox;
        private System.Windows.Forms.Label LabelBounding;
        private Controls.BoundingBoxEditor BoundingEditor;
        private System.Windows.Forms.Button CalcBoundingButton;
        private System.Windows.Forms.Label LabelGeomBounding;
        private Controls.BoundingBoxEditor GeomBoundingEditor;
        private System.Windows.Forms.Button CalcGeomBoundingButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private Controls.CollapsiblePanel collapsiblePanel1;
        private Controls.CollapsiblePanel collapsiblePanel2;
        private Controls.CollapsiblePanel collapsiblePanel3;
        private Controls.CollapsiblePanel collapsiblePanel4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label LabelInertiaTensor;
        private System.Windows.Forms.Label LabelCenterOfMass;
        private System.Windows.Forms.TextBox InertiaTensorTextBox;
        private System.Windows.Forms.Label LabelMass;
        private Controls.NumberTextBox MassNumberBox;
        private System.Windows.Forms.Label LabelFriction;
        private System.Windows.Forms.CheckBox FrictionCheckBox;
        private System.Windows.Forms.Label LabelAliases;
        private Controls.BrowseTextBox AliasesButtonBox;
    }
}