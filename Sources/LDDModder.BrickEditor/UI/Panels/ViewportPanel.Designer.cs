namespace LDDModder.BrickEditor.UI.Panels
{
    partial class ViewportPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewportPanel));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.CameraMenuDropDown = new System.Windows.Forms.ToolStripDropDownButton();
            this.CameraMenu_ResetCamera = new System.Windows.Forms.ToolStripMenuItem();
            this.CameraMenu_AlignTo = new System.Windows.Forms.ToolStripMenuItem();
            this.AlignToMenu_Top = new System.Windows.Forms.ToolStripMenuItem();
            this.AlignToMenu_Bottom = new System.Windows.Forms.ToolStripMenuItem();
            this.AlignToMenu_Front = new System.Windows.Forms.ToolStripMenuItem();
            this.AlignToMenu_Back = new System.Windows.Forms.ToolStripMenuItem();
            this.AlignToMenu_Right = new System.Windows.Forms.ToolStripMenuItem();
            this.AlignToMenu_Left = new System.Windows.Forms.ToolStripMenuItem();
            this.CameraMenu_LookAt = new System.Windows.Forms.ToolStripMenuItem();
            this.CameraMenu_Orthographic = new System.Windows.Forms.ToolStripMenuItem();
            this.DisplayMenuDropDown = new System.Windows.Forms.ToolStripDropDownButton();
            this.DisplayMenu_Collisions = new System.Windows.Forms.ToolStripMenuItem();
            this.DisplayMenu_Connections = new System.Windows.Forms.ToolStripMenuItem();
            this.DisplayMenu_Meshes = new System.Windows.Forms.ToolStripMenuItem();
            this.GizmoOrientationMenu = new System.Windows.Forms.ToolStripDropDownButton();
            this.globalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.localToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.GizmoPivotModeMenu = new System.Windows.Forms.ToolStripDropDownButton();
            this.boundingBoxCenterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.medianOriginsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.medianBoundingBoxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.activeElementToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cursorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.visualStudioToolStripExtender1 = new WeifenLuo.WinFormsUI.Docking.VisualStudioToolStripExtender(this.components);
            this.SelectionInfoPanel = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.PosZNumBox = new LDDModder.BrickEditor.UI.Controls.NumberTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.PosXNumBox = new LDDModder.BrickEditor.UI.Controls.NumberTextBox();
            this.PosYNumBox = new LDDModder.BrickEditor.UI.Controls.NumberTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.RotXNumBox = new LDDModder.BrickEditor.UI.Controls.NumberTextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.RotYNumBox = new LDDModder.BrickEditor.UI.Controls.NumberTextBox();
            this.RotZNumBox = new LDDModder.BrickEditor.UI.Controls.NumberTextBox();
            this.toolStrip1.SuspendLayout();
            this.SelectionInfoPanel.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CameraMenuDropDown,
            this.DisplayMenuDropDown,
            this.GizmoOrientationMenu,
            this.GizmoPivotModeMenu});
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Name = "toolStrip1";
            // 
            // CameraMenuDropDown
            // 
            this.CameraMenuDropDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.CameraMenuDropDown.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CameraMenu_ResetCamera,
            this.CameraMenu_AlignTo,
            this.CameraMenu_LookAt,
            this.CameraMenu_Orthographic});
            resources.ApplyResources(this.CameraMenuDropDown, "CameraMenuDropDown");
            this.CameraMenuDropDown.Margin = new System.Windows.Forms.Padding(1, 1, 0, 2);
            this.CameraMenuDropDown.Name = "CameraMenuDropDown";
            // 
            // CameraMenu_ResetCamera
            // 
            this.CameraMenu_ResetCamera.Name = "CameraMenu_ResetCamera";
            resources.ApplyResources(this.CameraMenu_ResetCamera, "CameraMenu_ResetCamera");
            this.CameraMenu_ResetCamera.Tag = "";
            this.CameraMenu_ResetCamera.Click += new System.EventHandler(this.CameraMenu_ResetCamera_Click);
            // 
            // CameraMenu_AlignTo
            // 
            this.CameraMenu_AlignTo.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AlignToMenu_Top,
            this.AlignToMenu_Bottom,
            this.AlignToMenu_Front,
            this.AlignToMenu_Back,
            this.AlignToMenu_Right,
            this.AlignToMenu_Left});
            this.CameraMenu_AlignTo.Name = "CameraMenu_AlignTo";
            resources.ApplyResources(this.CameraMenu_AlignTo, "CameraMenu_AlignTo");
            this.CameraMenu_AlignTo.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.CameraMenu_AlignTo_DropDownItemClicked);
            // 
            // AlignToMenu_Top
            // 
            this.AlignToMenu_Top.Name = "AlignToMenu_Top";
            resources.ApplyResources(this.AlignToMenu_Top, "AlignToMenu_Top");
            this.AlignToMenu_Top.Tag = "Top";
            // 
            // AlignToMenu_Bottom
            // 
            this.AlignToMenu_Bottom.Name = "AlignToMenu_Bottom";
            resources.ApplyResources(this.AlignToMenu_Bottom, "AlignToMenu_Bottom");
            this.AlignToMenu_Bottom.Tag = "Bottom";
            // 
            // AlignToMenu_Front
            // 
            this.AlignToMenu_Front.Name = "AlignToMenu_Front";
            resources.ApplyResources(this.AlignToMenu_Front, "AlignToMenu_Front");
            this.AlignToMenu_Front.Tag = "Front";
            // 
            // AlignToMenu_Back
            // 
            this.AlignToMenu_Back.Name = "AlignToMenu_Back";
            resources.ApplyResources(this.AlignToMenu_Back, "AlignToMenu_Back");
            this.AlignToMenu_Back.Tag = "Back";
            // 
            // AlignToMenu_Right
            // 
            this.AlignToMenu_Right.Name = "AlignToMenu_Right";
            resources.ApplyResources(this.AlignToMenu_Right, "AlignToMenu_Right");
            this.AlignToMenu_Right.Tag = "Right";
            // 
            // AlignToMenu_Left
            // 
            this.AlignToMenu_Left.Name = "AlignToMenu_Left";
            resources.ApplyResources(this.AlignToMenu_Left, "AlignToMenu_Left");
            this.AlignToMenu_Left.Tag = "Left";
            // 
            // CameraMenu_LookAt
            // 
            this.CameraMenu_LookAt.Name = "CameraMenu_LookAt";
            resources.ApplyResources(this.CameraMenu_LookAt, "CameraMenu_LookAt");
            this.CameraMenu_LookAt.Click += new System.EventHandler(this.CameraMenu_LookAt_Click);
            // 
            // CameraMenu_Orthographic
            // 
            this.CameraMenu_Orthographic.CheckOnClick = true;
            this.CameraMenu_Orthographic.Name = "CameraMenu_Orthographic";
            resources.ApplyResources(this.CameraMenu_Orthographic, "CameraMenu_Orthographic");
            this.CameraMenu_Orthographic.CheckedChanged += new System.EventHandler(this.CameraMenu_Orthographic_CheckedChanged);
            // 
            // DisplayMenuDropDown
            // 
            this.DisplayMenuDropDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.DisplayMenuDropDown.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DisplayMenu_Collisions,
            this.DisplayMenu_Connections,
            this.DisplayMenu_Meshes});
            resources.ApplyResources(this.DisplayMenuDropDown, "DisplayMenuDropDown");
            this.DisplayMenuDropDown.Name = "DisplayMenuDropDown";
            // 
            // DisplayMenu_Collisions
            // 
            this.DisplayMenu_Collisions.CheckOnClick = true;
            this.DisplayMenu_Collisions.Name = "DisplayMenu_Collisions";
            resources.ApplyResources(this.DisplayMenu_Collisions, "DisplayMenu_Collisions");
            this.DisplayMenu_Collisions.CheckedChanged += new System.EventHandler(this.DisplayMenu_Collisions_CheckedChanged);
            // 
            // DisplayMenu_Connections
            // 
            this.DisplayMenu_Connections.CheckOnClick = true;
            this.DisplayMenu_Connections.Name = "DisplayMenu_Connections";
            resources.ApplyResources(this.DisplayMenu_Connections, "DisplayMenu_Connections");
            this.DisplayMenu_Connections.CheckedChanged += new System.EventHandler(this.DisplayMenu_Connections_CheckedChanged);
            // 
            // DisplayMenu_Meshes
            // 
            this.DisplayMenu_Meshes.Checked = true;
            this.DisplayMenu_Meshes.CheckOnClick = true;
            this.DisplayMenu_Meshes.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DisplayMenu_Meshes.Name = "DisplayMenu_Meshes";
            resources.ApplyResources(this.DisplayMenu_Meshes, "DisplayMenu_Meshes");
            this.DisplayMenu_Meshes.CheckedChanged += new System.EventHandler(this.DisplayMenu_Meshes_CheckedChanged);
            // 
            // GizmoOrientationMenu
            // 
            this.GizmoOrientationMenu.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.GizmoOrientationMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.globalToolStripMenuItem,
            this.localToolStripMenuItem});
            resources.ApplyResources(this.GizmoOrientationMenu, "GizmoOrientationMenu");
            this.GizmoOrientationMenu.Margin = new System.Windows.Forms.Padding(50, 1, 0, 2);
            this.GizmoOrientationMenu.Name = "GizmoOrientationMenu";
            this.GizmoOrientationMenu.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.GizmoOrientationMenu_DropDownItemClicked);
            // 
            // globalToolStripMenuItem
            // 
            this.globalToolStripMenuItem.Checked = true;
            this.globalToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.globalToolStripMenuItem.Name = "globalToolStripMenuItem";
            resources.ApplyResources(this.globalToolStripMenuItem, "globalToolStripMenuItem");
            // 
            // localToolStripMenuItem
            // 
            this.localToolStripMenuItem.Name = "localToolStripMenuItem";
            resources.ApplyResources(this.localToolStripMenuItem, "localToolStripMenuItem");
            // 
            // GizmoPivotModeMenu
            // 
            this.GizmoPivotModeMenu.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.GizmoPivotModeMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.boundingBoxCenterToolStripMenuItem,
            this.medianOriginsToolStripMenuItem,
            this.medianBoundingBoxToolStripMenuItem,
            this.activeElementToolStripMenuItem,
            this.cursorToolStripMenuItem});
            resources.ApplyResources(this.GizmoPivotModeMenu, "GizmoPivotModeMenu");
            this.GizmoPivotModeMenu.Name = "GizmoPivotModeMenu";
            this.GizmoPivotModeMenu.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.GizmoPivotModeMenu_DropDownItemClicked);
            // 
            // boundingBoxCenterToolStripMenuItem
            // 
            this.boundingBoxCenterToolStripMenuItem.Name = "boundingBoxCenterToolStripMenuItem";
            resources.ApplyResources(this.boundingBoxCenterToolStripMenuItem, "boundingBoxCenterToolStripMenuItem");
            // 
            // medianOriginsToolStripMenuItem
            // 
            this.medianOriginsToolStripMenuItem.Name = "medianOriginsToolStripMenuItem";
            resources.ApplyResources(this.medianOriginsToolStripMenuItem, "medianOriginsToolStripMenuItem");
            // 
            // medianBoundingBoxToolStripMenuItem
            // 
            this.medianBoundingBoxToolStripMenuItem.Checked = true;
            this.medianBoundingBoxToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.medianBoundingBoxToolStripMenuItem.Name = "medianBoundingBoxToolStripMenuItem";
            resources.ApplyResources(this.medianBoundingBoxToolStripMenuItem, "medianBoundingBoxToolStripMenuItem");
            // 
            // activeElementToolStripMenuItem
            // 
            this.activeElementToolStripMenuItem.Name = "activeElementToolStripMenuItem";
            resources.ApplyResources(this.activeElementToolStripMenuItem, "activeElementToolStripMenuItem");
            // 
            // cursorToolStripMenuItem
            // 
            this.cursorToolStripMenuItem.Name = "cursorToolStripMenuItem";
            resources.ApplyResources(this.cursorToolStripMenuItem, "cursorToolStripMenuItem");
            // 
            // visualStudioToolStripExtender1
            // 
            this.visualStudioToolStripExtender1.DefaultRenderer = null;
            // 
            // SelectionInfoPanel
            // 
            resources.ApplyResources(this.SelectionInfoPanel, "SelectionInfoPanel");
            this.SelectionInfoPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.SelectionInfoPanel.Controls.Add(this.tableLayoutPanel1);
            this.SelectionInfoPanel.Name = "SelectionInfoPanel";
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.PosZNumBox, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.PosXNumBox, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.PosYNumBox, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.RotXNumBox, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.label7, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.label8, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.RotYNumBox, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.RotZNumBox, 1, 7);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // PosZNumBox
            // 
            resources.ApplyResources(this.PosZNumBox, "PosZNumBox");
            this.PosZNumBox.MaximumValue = 5000D;
            this.PosZNumBox.MinDisplayedDecimalPlaces = 2;
            this.PosZNumBox.MinimumValue = -5000D;
            this.PosZNumBox.Name = "PosZNumBox";
            this.PosZNumBox.ValueChanged += new System.EventHandler(this.PositionNumBoxes_ValueChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Name = "label3";
            // 
            // PosXNumBox
            // 
            resources.ApplyResources(this.PosXNumBox, "PosXNumBox");
            this.PosXNumBox.MaximumValue = 5000D;
            this.PosXNumBox.MinDisplayedDecimalPlaces = 2;
            this.PosXNumBox.MinimumValue = -5000D;
            this.PosXNumBox.Name = "PosXNumBox";
            this.PosXNumBox.ValueChanged += new System.EventHandler(this.PositionNumBoxes_ValueChanged);
            // 
            // PosYNumBox
            // 
            resources.ApplyResources(this.PosYNumBox, "PosYNumBox");
            this.PosYNumBox.MaximumValue = 5000D;
            this.PosYNumBox.MinDisplayedDecimalPlaces = 2;
            this.PosYNumBox.MinimumValue = -5000D;
            this.PosYNumBox.Name = "PosYNumBox";
            this.PosYNumBox.ValueChanged += new System.EventHandler(this.PositionNumBoxes_ValueChanged);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.tableLayoutPanel1.SetColumnSpan(this.label4, 2);
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Name = "label4";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.tableLayoutPanel1.SetColumnSpan(this.label5, 2);
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Name = "label5";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Name = "label6";
            // 
            // RotXNumBox
            // 
            resources.ApplyResources(this.RotXNumBox, "RotXNumBox");
            this.RotXNumBox.MaximumValue = 360D;
            this.RotXNumBox.MinDisplayedDecimalPlaces = 2;
            this.RotXNumBox.MinimumValue = -360D;
            this.RotXNumBox.Name = "RotXNumBox";
            this.RotXNumBox.ValueChanged += new System.EventHandler(this.RotationNumBoxes_ValueChanged);
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Name = "label7";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Name = "label8";
            // 
            // RotYNumBox
            // 
            resources.ApplyResources(this.RotYNumBox, "RotYNumBox");
            this.RotYNumBox.MaximumValue = 360D;
            this.RotYNumBox.MinDisplayedDecimalPlaces = 2;
            this.RotYNumBox.MinimumValue = -360D;
            this.RotYNumBox.Name = "RotYNumBox";
            this.RotYNumBox.ValueChanged += new System.EventHandler(this.RotationNumBoxes_ValueChanged);
            // 
            // RotZNumBox
            // 
            resources.ApplyResources(this.RotZNumBox, "RotZNumBox");
            this.RotZNumBox.MaximumValue = 360D;
            this.RotZNumBox.MinDisplayedDecimalPlaces = 2;
            this.RotZNumBox.MinimumValue = -360D;
            this.RotZNumBox.Name = "RotZNumBox";
            this.RotZNumBox.ValueChanged += new System.EventHandler(this.RotationNumBoxes_ValueChanged);
            // 
            // ViewportPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.SelectionInfoPanel);
            this.Controls.Add(this.toolStrip1);
            this.Name = "ViewportPanel";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.SelectionInfoPanel.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton CameraMenuDropDown;
        private System.Windows.Forms.ToolStripMenuItem CameraMenu_ResetCamera;
        private WeifenLuo.WinFormsUI.Docking.VisualStudioToolStripExtender visualStudioToolStripExtender1;
        private System.Windows.Forms.ToolStripMenuItem CameraMenu_AlignTo;
        private System.Windows.Forms.ToolStripMenuItem AlignToMenu_Front;
        private System.Windows.Forms.ToolStripMenuItem AlignToMenu_Back;
        private System.Windows.Forms.ToolStripMenuItem AlignToMenu_Top;
        private System.Windows.Forms.ToolStripMenuItem AlignToMenu_Bottom;
        private System.Windows.Forms.ToolStripMenuItem AlignToMenu_Left;
        private System.Windows.Forms.ToolStripMenuItem AlignToMenu_Right;
        private System.Windows.Forms.ToolStripMenuItem CameraMenu_Orthographic;
        private System.Windows.Forms.ToolStripDropDownButton DisplayMenuDropDown;
        private System.Windows.Forms.ToolStripMenuItem DisplayMenu_Collisions;
        private System.Windows.Forms.ToolStripMenuItem DisplayMenu_Connections;
        private System.Windows.Forms.ToolStripMenuItem DisplayMenu_Meshes;
        private System.Windows.Forms.ToolStripDropDownButton GizmoOrientationMenu;
        private System.Windows.Forms.ToolStripMenuItem globalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem localToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton GizmoPivotModeMenu;
        private System.Windows.Forms.ToolStripMenuItem boundingBoxCenterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem medianOriginsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem medianBoundingBoxToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem activeElementToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cursorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CameraMenu_LookAt;
        private System.Windows.Forms.Panel SelectionInfoPanel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private Controls.NumberTextBox PosXNumBox;
        private Controls.NumberTextBox PosZNumBox;
        private Controls.NumberTextBox PosYNumBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private Controls.NumberTextBox RotXNumBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private Controls.NumberTextBox RotYNumBox;
        private Controls.NumberTextBox RotZNumBox;
    }
}