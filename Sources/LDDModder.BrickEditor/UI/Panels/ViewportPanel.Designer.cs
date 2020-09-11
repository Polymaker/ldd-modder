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
            this.DisplayMenu_Meshes = new System.Windows.Forms.ToolStripMenuItem();
            this.DisplayMenu_Collisions = new System.Windows.Forms.ToolStripMenuItem();
            this.DisplayMenu_Connections = new System.Windows.Forms.ToolStripMenuItem();
            this.MeshesDropDownMenu = new System.Windows.Forms.ToolStripDropDownButton();
            this.MeshesMenu_CalculateOutlines = new System.Windows.Forms.ToolStripMenuItem();
            this.MeshesMenu_RemoveOutlines = new System.Windows.Forms.ToolStripMenuItem();
            this.MeshesMenu_Separate = new System.Windows.Forms.ToolStripMenuItem();
            this.MeshesMenu_Merge = new System.Windows.Forms.ToolStripMenuItem();
            this.BonesDropDownMenu = new System.Windows.Forms.ToolStripDropDownButton();
            this.Bones_RebuildConnections = new System.Windows.Forms.ToolStripMenuItem();
            this.Bones_CalcBounding = new System.Windows.Forms.ToolStripMenuItem();
            this.Bones_CopyData = new System.Windows.Forms.ToolStripMenuItem();
            this.GizmoOrientationMenu = new System.Windows.Forms.ToolStripDropDownButton();
            this.globalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.localToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.GizmoPivotModeMenu = new System.Windows.Forms.ToolStripDropDownButton();
            this.boundingBoxCenterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.medianOriginsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.medianBoundingBoxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.activeElementToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cursorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ModelRenderMode1Button = new System.Windows.Forms.ToolStripButton();
            this.ModelRenderMode2Button = new System.Windows.Forms.ToolStripButton();
            this.ModelRenderMode3Button = new System.Windows.Forms.ToolStripButton();
            this.visualStudioToolStripExtender1 = new WeifenLuo.WinFormsUI.Docking.VisualStudioToolStripExtender(this.components);
            this.SelectionInfoPanel = new System.Windows.Forms.Panel();
            this.transformEditor1 = new LDDModder.BrickEditor.UI.Controls.TransformEditor();
            this.MeshesMenu_Separator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStrip1.SuspendLayout();
            this.SelectionInfoPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CameraMenuDropDown,
            this.DisplayMenuDropDown,
            this.MeshesDropDownMenu,
            this.BonesDropDownMenu,
            this.GizmoOrientationMenu,
            this.GizmoPivotModeMenu,
            this.ModelRenderMode1Button,
            this.ModelRenderMode2Button,
            this.ModelRenderMode3Button});
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
            this.DisplayMenu_Meshes,
            this.DisplayMenu_Collisions,
            this.DisplayMenu_Connections});
            resources.ApplyResources(this.DisplayMenuDropDown, "DisplayMenuDropDown");
            this.DisplayMenuDropDown.Name = "DisplayMenuDropDown";
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
            // MeshesDropDownMenu
            // 
            this.MeshesDropDownMenu.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.MeshesDropDownMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MeshesMenu_CalculateOutlines,
            this.MeshesMenu_RemoveOutlines,
            this.MeshesMenu_Separator1,
            this.MeshesMenu_Separate,
            this.MeshesMenu_Merge});
            resources.ApplyResources(this.MeshesDropDownMenu, "MeshesDropDownMenu");
            this.MeshesDropDownMenu.Name = "MeshesDropDownMenu";
            this.MeshesDropDownMenu.DropDownOpening += new System.EventHandler(this.MeshesDropDownMenu_DropDownOpening);
            // 
            // MeshesMenu_CalculateOutlines
            // 
            this.MeshesMenu_CalculateOutlines.Name = "MeshesMenu_CalculateOutlines";
            resources.ApplyResources(this.MeshesMenu_CalculateOutlines, "MeshesMenu_CalculateOutlines");
            this.MeshesMenu_CalculateOutlines.Click += new System.EventHandler(this.MeshesMenu_CalculateOutlines_Click);
            // 
            // MeshesMenu_RemoveOutlines
            // 
            this.MeshesMenu_RemoveOutlines.Name = "MeshesMenu_RemoveOutlines";
            resources.ApplyResources(this.MeshesMenu_RemoveOutlines, "MeshesMenu_RemoveOutlines");
            this.MeshesMenu_RemoveOutlines.Click += new System.EventHandler(this.MeshesMenu_RemoveOutlines_Click);
            // 
            // MeshesMenu_Separate
            // 
            this.MeshesMenu_Separate.Name = "MeshesMenu_Separate";
            resources.ApplyResources(this.MeshesMenu_Separate, "MeshesMenu_Separate");
            this.MeshesMenu_Separate.Click += new System.EventHandler(this.MeshesMenu_Separate_Click);
            // 
            // MeshesMenu_Merge
            // 
            this.MeshesMenu_Merge.Name = "MeshesMenu_Merge";
            resources.ApplyResources(this.MeshesMenu_Merge, "MeshesMenu_Merge");
            this.MeshesMenu_Merge.Click += new System.EventHandler(this.MeshesMenu_Merge_Click);
            // 
            // BonesDropDownMenu
            // 
            this.BonesDropDownMenu.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.BonesDropDownMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Bones_RebuildConnections,
            this.Bones_CalcBounding,
            this.Bones_CopyData});
            resources.ApplyResources(this.BonesDropDownMenu, "BonesDropDownMenu");
            this.BonesDropDownMenu.Name = "BonesDropDownMenu";
            // 
            // Bones_RebuildConnections
            // 
            this.Bones_RebuildConnections.Name = "Bones_RebuildConnections";
            resources.ApplyResources(this.Bones_RebuildConnections, "Bones_RebuildConnections");
            this.Bones_RebuildConnections.Click += new System.EventHandler(this.Bones_RebuildConnections_Click);
            // 
            // Bones_CalcBounding
            // 
            this.Bones_CalcBounding.Name = "Bones_CalcBounding";
            resources.ApplyResources(this.Bones_CalcBounding, "Bones_CalcBounding");
            this.Bones_CalcBounding.Click += new System.EventHandler(this.Bones_CalcBounding_Click);
            // 
            // Bones_CopyData
            // 
            this.Bones_CopyData.Name = "Bones_CopyData";
            resources.ApplyResources(this.Bones_CopyData, "Bones_CopyData");
            this.Bones_CopyData.Click += new System.EventHandler(this.Bones_CopyData_Click);
            // 
            // GizmoOrientationMenu
            // 
            this.GizmoOrientationMenu.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.GizmoOrientationMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.globalToolStripMenuItem,
            this.localToolStripMenuItem});
            this.GizmoOrientationMenu.Image = global::LDDModder.BrickEditor.Properties.Resources.GizmoOrientationIcon;
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
            this.GizmoPivotModeMenu.Image = global::LDDModder.BrickEditor.Properties.Resources.GizmoPivotPointIcon;
            resources.ApplyResources(this.GizmoPivotModeMenu, "GizmoPivotModeMenu");
            this.GizmoPivotModeMenu.Margin = new System.Windows.Forms.Padding(0, 1, 20, 2);
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
            // ModelRenderMode1Button
            // 
            this.ModelRenderMode1Button.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ModelRenderMode1Button.Image = global::LDDModder.BrickEditor.Properties.Resources.WireframeIcon;
            resources.ApplyResources(this.ModelRenderMode1Button, "ModelRenderMode1Button");
            this.ModelRenderMode1Button.Name = "ModelRenderMode1Button";
            this.ModelRenderMode1Button.Click += new System.EventHandler(this.ModelRenderModeButton_Click);
            // 
            // ModelRenderMode2Button
            // 
            this.ModelRenderMode2Button.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ModelRenderMode2Button.Image = global::LDDModder.BrickEditor.Properties.Resources.MainSurfaceIcon;
            resources.ApplyResources(this.ModelRenderMode2Button, "ModelRenderMode2Button");
            this.ModelRenderMode2Button.Name = "ModelRenderMode2Button";
            this.ModelRenderMode2Button.Click += new System.EventHandler(this.ModelRenderModeButton_Click);
            // 
            // ModelRenderMode3Button
            // 
            this.ModelRenderMode3Button.Checked = true;
            this.ModelRenderMode3Button.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ModelRenderMode3Button.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ModelRenderMode3Button.Image = global::LDDModder.BrickEditor.Properties.Resources.SolidWireframeIcon;
            resources.ApplyResources(this.ModelRenderMode3Button, "ModelRenderMode3Button");
            this.ModelRenderMode3Button.Name = "ModelRenderMode3Button";
            this.ModelRenderMode3Button.Click += new System.EventHandler(this.ModelRenderModeButton_Click);
            // 
            // visualStudioToolStripExtender1
            // 
            this.visualStudioToolStripExtender1.DefaultRenderer = null;
            // 
            // SelectionInfoPanel
            // 
            resources.ApplyResources(this.SelectionInfoPanel, "SelectionInfoPanel");
            this.SelectionInfoPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.SelectionInfoPanel.Controls.Add(this.transformEditor1);
            this.SelectionInfoPanel.Name = "SelectionInfoPanel";
            // 
            // transformEditor1
            // 
            resources.ApplyResources(this.transformEditor1, "transformEditor1");
            this.transformEditor1.ForeColor = System.Drawing.Color.White;
            this.transformEditor1.Name = "transformEditor1";
            this.transformEditor1.ViewLayout = LDDModder.BrickEditor.UI.Controls.TransformEditor.EditLayout.Vertical;
            // 
            // MeshesMenu_Separator1
            // 
            this.MeshesMenu_Separator1.Name = "MeshesMenu_Separator1";
            resources.ApplyResources(this.MeshesMenu_Separator1, "MeshesMenu_Separator1");
            // 
            // ViewportPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.SelectionInfoPanel);
            this.Name = "ViewportPanel";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.SelectionInfoPanel.ResumeLayout(false);
            this.SelectionInfoPanel.PerformLayout();
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
        private System.Windows.Forms.ToolStripButton ModelRenderMode1Button;
        private System.Windows.Forms.ToolStripButton ModelRenderMode2Button;
        private System.Windows.Forms.ToolStripButton ModelRenderMode3Button;
        private System.Windows.Forms.ToolStripMenuItem MeshesMenu_Separate;
        private System.Windows.Forms.ToolStripMenuItem MeshesMenu_Merge;
        private System.Windows.Forms.ToolStripDropDownButton MeshesDropDownMenu;
        private System.Windows.Forms.ToolStripDropDownButton BonesDropDownMenu;
        private System.Windows.Forms.ToolStripMenuItem Bones_RebuildConnections;
        private System.Windows.Forms.ToolStripMenuItem Bones_CalcBounding;
        private System.Windows.Forms.ToolStripMenuItem Bones_CopyData;
        private Controls.TransformEditor transformEditor1;
        private System.Windows.Forms.ToolStripMenuItem MeshesMenu_CalculateOutlines;
        private System.Windows.Forms.ToolStripMenuItem MeshesMenu_RemoveOutlines;
        private System.Windows.Forms.ToolStripSeparator MeshesMenu_Separator1;
    }
}