namespace LDDModder.BrickEditor.UI.Windows
{
    partial class ConnectionUsageWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConnectionUsageWindow));
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.button1 = new System.Windows.Forms.Button();
            this.ConnTypeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ConnSubTypeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RefCountColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PartListColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            resources.ApplyResources(this.dataGridView1, "dataGridView1");
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ConnTypeColumn,
            this.ConnSubTypeColumn,
            this.RefCountColumn,
            this.PartListColumn});
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ConnTypeColumn
            // 
            this.ConnTypeColumn.DataPropertyName = "ConnectionType";
            resources.ApplyResources(this.ConnTypeColumn, "ConnTypeColumn");
            this.ConnTypeColumn.Name = "ConnTypeColumn";
            this.ConnTypeColumn.ReadOnly = true;
            // 
            // ConnSubTypeColumn
            // 
            this.ConnSubTypeColumn.DataPropertyName = "SubType";
            resources.ApplyResources(this.ConnSubTypeColumn, "ConnSubTypeColumn");
            this.ConnSubTypeColumn.Name = "ConnSubTypeColumn";
            this.ConnSubTypeColumn.ReadOnly = true;
            // 
            // RefCountColumn
            // 
            this.RefCountColumn.DataPropertyName = "RefCount";
            resources.ApplyResources(this.RefCountColumn, "RefCountColumn");
            this.RefCountColumn.Name = "RefCountColumn";
            this.RefCountColumn.ReadOnly = true;
            // 
            // PartListColumn
            // 
            this.PartListColumn.DataPropertyName = "Parts";
            resources.ApplyResources(this.PartListColumn, "PartListColumn");
            this.PartListColumn.Name = "PartListColumn";
            this.PartListColumn.ReadOnly = true;
            // 
            // ConnectionUsageWindow
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dataGridView1);
            this.Name = "ConnectionUsageWindow";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ConnTypeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ConnSubTypeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn RefCountColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn PartListColumn;
    }
}