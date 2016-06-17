namespace LDDModder.PaletteMaker
{
    partial class FrmCreateSetPalette
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
            this.txtSearchSetID = new System.Windows.Forms.TextBox();
            this.btnSearchSet = new System.Windows.Forms.Button();
            this.pbxSetPicture = new System.Windows.Forms.PictureBox();
            this.lblSetName = new System.Windows.Forms.Label();
            this.tlpSetDetails = new System.Windows.Forms.TableLayoutPanel();
            this.txtSetName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtSetID = new System.Windows.Forms.TextBox();
            this.txtSetTheme = new System.Windows.Forms.TextBox();
            this.txtSetYear = new System.Windows.Forms.TextBox();
            this.txtSetPieces = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.gridColPartID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridColPartType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridColPartName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridColColor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridColElementID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridColQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridColLDD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.listView1 = new System.Windows.Forms.ListView();
            this.lvColPaletteName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            ((System.ComponentModel.ISupportInitialize)(this.pbxSetPicture)).BeginInit();
            this.tlpSetDetails.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtSearchSetID
            // 
            this.txtSearchSetID.Location = new System.Drawing.Point(6, 18);
            this.txtSearchSetID.Name = "txtSearchSetID";
            this.txtSearchSetID.Size = new System.Drawing.Size(132, 20);
            this.txtSearchSetID.TabIndex = 0;
            this.txtSearchSetID.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtSearchSetID_KeyUp);
            // 
            // btnSearchSet
            // 
            this.btnSearchSet.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSearchSet.Location = new System.Drawing.Point(140, 17);
            this.btnSearchSet.Name = "btnSearchSet";
            this.btnSearchSet.Size = new System.Drawing.Size(52, 22);
            this.btnSearchSet.TabIndex = 1;
            this.btnSearchSet.Text = "Search";
            this.btnSearchSet.UseVisualStyleBackColor = true;
            this.btnSearchSet.Click += new System.EventHandler(this.btnSearchSet_Click);
            // 
            // pbxSetPicture
            // 
            this.pbxSetPicture.BackColor = System.Drawing.SystemColors.Window;
            this.pbxSetPicture.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbxSetPicture.Location = new System.Drawing.Point(194, 19);
            this.pbxSetPicture.Name = "pbxSetPicture";
            this.pbxSetPicture.Size = new System.Drawing.Size(128, 128);
            this.pbxSetPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbxSetPicture.TabIndex = 3;
            this.pbxSetPicture.TabStop = false;
            // 
            // lblSetName
            // 
            this.lblSetName.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblSetName.AutoSize = true;
            this.lblSetName.Location = new System.Drawing.Point(9, 24);
            this.lblSetName.Margin = new System.Windows.Forms.Padding(3);
            this.lblSetName.Name = "lblSetName";
            this.lblSetName.Size = new System.Drawing.Size(35, 13);
            this.lblSetName.TabIndex = 4;
            this.lblSetName.Text = "Name";
            // 
            // tlpSetDetails
            // 
            this.tlpSetDetails.AutoSize = true;
            this.tlpSetDetails.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpSetDetails.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tlpSetDetails.ColumnCount = 2;
            this.tlpSetDetails.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpSetDetails.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
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
            this.tlpSetDetails.Location = new System.Drawing.Point(6, 42);
            this.tlpSetDetails.Name = "tlpSetDetails";
            this.tlpSetDetails.RowCount = 5;
            this.tlpSetDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 19F));
            this.tlpSetDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 19F));
            this.tlpSetDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 19F));
            this.tlpSetDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 19F));
            this.tlpSetDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 19F));
            this.tlpSetDetails.Size = new System.Drawing.Size(185, 101);
            this.tlpSetDetails.TabIndex = 5;
            // 
            // txtSetName
            // 
            this.txtSetName.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtSetName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSetName.Location = new System.Drawing.Point(51, 24);
            this.txtSetName.Name = "txtSetName";
            this.txtSetName.ReadOnly = true;
            this.txtSetName.Size = new System.Drawing.Size(130, 13);
            this.txtSetName.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 44);
            this.label2.Margin = new System.Windows.Forms.Padding(3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Theme";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 64);
            this.label3.Margin = new System.Windows.Forms.Padding(3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Year";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 84);
            this.label4.Margin = new System.Windows.Forms.Padding(3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Pieces";
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 4);
            this.label5.Margin = new System.Windows.Forms.Padding(3);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(37, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Set ID";
            // 
            // txtSetID
            // 
            this.txtSetID.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSetID.Location = new System.Drawing.Point(51, 4);
            this.txtSetID.Name = "txtSetID";
            this.txtSetID.ReadOnly = true;
            this.txtSetID.Size = new System.Drawing.Size(130, 13);
            this.txtSetID.TabIndex = 10;
            // 
            // txtSetTheme
            // 
            this.txtSetTheme.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtSetTheme.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSetTheme.Location = new System.Drawing.Point(51, 44);
            this.txtSetTheme.Name = "txtSetTheme";
            this.txtSetTheme.ReadOnly = true;
            this.txtSetTheme.Size = new System.Drawing.Size(130, 13);
            this.txtSetTheme.TabIndex = 12;
            // 
            // txtSetYear
            // 
            this.txtSetYear.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtSetYear.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSetYear.Location = new System.Drawing.Point(51, 64);
            this.txtSetYear.Name = "txtSetYear";
            this.txtSetYear.ReadOnly = true;
            this.txtSetYear.Size = new System.Drawing.Size(130, 13);
            this.txtSetYear.TabIndex = 13;
            // 
            // txtSetPieces
            // 
            this.txtSetPieces.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtSetPieces.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSetPieces.Location = new System.Drawing.Point(51, 84);
            this.txtSetPieces.Name = "txtSetPieces";
            this.txtSetPieces.ReadOnly = true;
            this.txtSetPieces.Size = new System.Drawing.Size(130, 13);
            this.txtSetPieces.TabIndex = 14;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtSearchSetID);
            this.groupBox1.Controls.Add(this.pbxSetPicture);
            this.groupBox1.Controls.Add(this.tlpSetDetails);
            this.groupBox1.Controls.Add(this.btnSearchSet);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 3, 0, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(328, 154);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Search LEGO set";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeight = 25;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.gridColPartID,
            this.gridColPartType,
            this.gridColPartName,
            this.gridColColor,
            this.gridColElementID,
            this.gridColQty,
            this.gridColLDD});
            this.tableLayoutPanel1.SetColumnSpan(this.dataGridView1, 2);
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 162);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.Size = new System.Drawing.Size(621, 304);
            this.dataGridView1.TabIndex = 7;
            this.dataGridView1.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_ColumnHeaderMouseClick);
            // 
            // gridColPartID
            // 
            this.gridColPartID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.gridColPartID.DataPropertyName = "PartID";
            this.gridColPartID.HeaderText = "Part ID";
            this.gridColPartID.Name = "gridColPartID";
            this.gridColPartID.ReadOnly = true;
            this.gridColPartID.Width = 65;
            // 
            // gridColPartType
            // 
            this.gridColPartType.DataPropertyName = "PartType";
            this.gridColPartType.FillWeight = 25F;
            this.gridColPartType.HeaderText = "Type";
            this.gridColPartType.Name = "gridColPartType";
            this.gridColPartType.ReadOnly = true;
            // 
            // gridColPartName
            // 
            this.gridColPartName.DataPropertyName = "Name";
            this.gridColPartName.FillWeight = 40F;
            this.gridColPartName.HeaderText = "Part name";
            this.gridColPartName.Name = "gridColPartName";
            this.gridColPartName.ReadOnly = true;
            // 
            // gridColColor
            // 
            this.gridColColor.DataPropertyName = "Color";
            this.gridColColor.FillWeight = 20F;
            this.gridColColor.HeaderText = "Color";
            this.gridColColor.Name = "gridColColor";
            this.gridColColor.ReadOnly = true;
            // 
            // gridColElementID
            // 
            this.gridColElementID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.gridColElementID.DataPropertyName = "ElementID";
            this.gridColElementID.HeaderText = "Element ID";
            this.gridColElementID.Name = "gridColElementID";
            this.gridColElementID.ReadOnly = true;
            this.gridColElementID.Width = 84;
            // 
            // gridColQty
            // 
            this.gridColQty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.gridColQty.DataPropertyName = "Quantity";
            this.gridColQty.HeaderText = "Qty";
            this.gridColQty.Name = "gridColQty";
            this.gridColQty.ReadOnly = true;
            this.gridColQty.Width = 48;
            // 
            // gridColLDD
            // 
            this.gridColLDD.DataPropertyName = "LDD";
            this.gridColLDD.FillWeight = 15F;
            this.gridColLDD.HeaderText = "LDD ID";
            this.gridColLDD.Name = "gridColLDD";
            this.gridColLDD.ReadOnly = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.dataGridView1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.listView1, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(627, 469);
            this.tableLayoutPanel1.TabIndex = 8;
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.lvColPaletteName});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.Location = new System.Drawing.Point(334, 3);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(290, 153);
            this.listView1.TabIndex = 8;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // lvColPaletteName
            // 
            this.lvColPaletteName.Text = "Palette name";
            this.lvColPaletteName.Width = 163;
            // 
            // FrmCreateSetPalette
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(633, 475);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FrmCreateSetPalette";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Text = "FrmCreateSetPalette";
            ((System.ComponentModel.ISupportInitialize)(this.pbxSetPicture)).EndInit();
            this.tlpSetDetails.ResumeLayout(false);
            this.tlpSetDetails.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtSearchSetID;
        private System.Windows.Forms.Button btnSearchSet;
        private System.Windows.Forms.PictureBox pbxSetPicture;
        private System.Windows.Forms.Label lblSetName;
        private System.Windows.Forms.TableLayoutPanel tlpSetDetails;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtSetID;
        private System.Windows.Forms.TextBox txtSetName;
        private System.Windows.Forms.TextBox txtSetTheme;
        private System.Windows.Forms.TextBox txtSetYear;
        private System.Windows.Forms.TextBox txtSetPieces;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridColPartID;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridColPartType;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridColPartName;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridColColor;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridColElementID;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridColQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridColLDD;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader lvColPaletteName;
    }
}