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
            this.Camera_ResetCameraMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.CameraMenu_AlignTo = new System.Windows.Forms.ToolStripMenuItem();
            this.AlignToMenu_Front = new System.Windows.Forms.ToolStripMenuItem();
            this.AlignToMenu_Back = new System.Windows.Forms.ToolStripMenuItem();
            this.AlignToMenu_Top = new System.Windows.Forms.ToolStripMenuItem();
            this.AlignToMenu_Bottom = new System.Windows.Forms.ToolStripMenuItem();
            this.AlignToMenu_Left = new System.Windows.Forms.ToolStripMenuItem();
            this.AlignToMenu_Right = new System.Windows.Forms.ToolStripMenuItem();
            this.CameraMenu_Orthographic = new System.Windows.Forms.ToolStripMenuItem();
            this.visualStudioToolStripExtender1 = new WeifenLuo.WinFormsUI.Docking.VisualStudioToolStripExtender(this.components);
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CameraMenuDropDown});
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Name = "toolStrip1";
            // 
            // CameraMenuDropDown
            // 
            this.CameraMenuDropDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.CameraMenuDropDown.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Camera_ResetCameraMenu,
            this.CameraMenu_AlignTo,
            this.CameraMenu_Orthographic});
            resources.ApplyResources(this.CameraMenuDropDown, "CameraMenuDropDown");
            this.CameraMenuDropDown.Name = "CameraMenuDropDown";
            // 
            // Camera_ResetCameraMenu
            // 
            this.Camera_ResetCameraMenu.Name = "Camera_ResetCameraMenu";
            resources.ApplyResources(this.Camera_ResetCameraMenu, "Camera_ResetCameraMenu");
            this.Camera_ResetCameraMenu.Click += new System.EventHandler(this.Camera_ResetCameraMenu_Click);
            // 
            // CameraMenu_AlignTo
            // 
            this.CameraMenu_AlignTo.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AlignToMenu_Top,
            this.AlignToMenu_Bottom,
            this.AlignToMenu_Front,
            this.AlignToMenu_Back,
            this.AlignToMenu_Left,
            this.AlignToMenu_Right});
            this.CameraMenu_AlignTo.Name = "CameraMenu_AlignTo";
            resources.ApplyResources(this.CameraMenu_AlignTo, "CameraMenu_AlignTo");
            this.CameraMenu_AlignTo.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.CameraMenu_AlignTo_DropDownItemClicked);
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
            // AlignToMenu_Left
            // 
            this.AlignToMenu_Left.Name = "AlignToMenu_Left";
            resources.ApplyResources(this.AlignToMenu_Left, "AlignToMenu_Left");
            this.AlignToMenu_Left.Tag = "Left";
            // 
            // AlignToMenu_Right
            // 
            this.AlignToMenu_Right.Name = "AlignToMenu_Right";
            resources.ApplyResources(this.AlignToMenu_Right, "AlignToMenu_Right");
            this.AlignToMenu_Right.Tag = "Right";
            // 
            // CameraMenu_Orthographic
            // 
            this.CameraMenu_Orthographic.CheckOnClick = true;
            this.CameraMenu_Orthographic.Name = "CameraMenu_Orthographic";
            resources.ApplyResources(this.CameraMenu_Orthographic, "CameraMenu_Orthographic");
            this.CameraMenu_Orthographic.CheckedChanged += new System.EventHandler(this.CameraMenu_Orthographic_CheckedChanged);
            // 
            // visualStudioToolStripExtender1
            // 
            this.visualStudioToolStripExtender1.DefaultRenderer = null;
            // 
            // ViewportPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolStrip1);
            this.Name = "ViewportPanel";
            this.SizeChanged += new System.EventHandler(this.ViewportPanel_SizeChanged);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton CameraMenuDropDown;
        private System.Windows.Forms.ToolStripMenuItem Camera_ResetCameraMenu;
        private WeifenLuo.WinFormsUI.Docking.VisualStudioToolStripExtender visualStudioToolStripExtender1;
        private System.Windows.Forms.ToolStripMenuItem CameraMenu_AlignTo;
        private System.Windows.Forms.ToolStripMenuItem AlignToMenu_Front;
        private System.Windows.Forms.ToolStripMenuItem AlignToMenu_Back;
        private System.Windows.Forms.ToolStripMenuItem AlignToMenu_Top;
        private System.Windows.Forms.ToolStripMenuItem AlignToMenu_Bottom;
        private System.Windows.Forms.ToolStripMenuItem AlignToMenu_Left;
        private System.Windows.Forms.ToolStripMenuItem AlignToMenu_Right;
        private System.Windows.Forms.ToolStripMenuItem CameraMenu_Orthographic;
    }
}