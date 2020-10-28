using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Resources;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using WeifenLuo.WinFormsUI.ThemeVS2012;

namespace LDDModder.BrickEditor.UI
{
    class VS2015DockPaneCaption : DockPaneCaptionBase
    {
        private const int TextGapTop = 3;

        private const int TextGapBottom = 2;

        private const int TextGapLeft = 2;

        private const int TextGapRight = 3;

        private const int ButtonGapTop = 4;

        private const int ButtonGapBottom = 3;

        private const int ButtonGapBetween = 1;

        private const int ButtonGapLeft = 1;

        private const int ButtonGapRight = 5;

        private InertButtonBase m_buttonClose;

        private InertButtonBase m_buttonAutoHide;

        private InertButtonBase m_buttonOptions;

        private IContainer m_components;

        private ToolTip m_toolTip;

        private static string _toolTipClose;

        private static string _toolTipOptions;

        private static string _toolTipAutoHide;

        private static TextFormatFlags _textFormat = TextFormatFlags.EndEllipsis | TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter;

        private InertButtonBase ButtonClose
        {
            get
            {
                if (m_buttonClose == null)
                {
                    m_buttonClose = new VS2012DockPaneCaptionInertButton(this, base.DockPane.DockPanel.Theme.ImageService.DockPaneHover_Close, base.DockPane.DockPanel.Theme.ImageService.DockPane_Close, base.DockPane.DockPanel.Theme.ImageService.DockPanePress_Close, base.DockPane.DockPanel.Theme.ImageService.DockPaneActiveHover_Close, base.DockPane.DockPanel.Theme.ImageService.DockPaneActive_Close);
                    m_toolTip.SetToolTip(m_buttonClose, ToolTipClose);
                    m_buttonClose.Click += Close_Click;
                    base.Controls.Add(m_buttonClose);
                }
                return m_buttonClose;
            }
        }

        private InertButtonBase ButtonAutoHide
        {
            get
            {
                if (m_buttonAutoHide == null)
                {
                    m_buttonAutoHide = new VS2012DockPaneCaptionInertButton(this, base.DockPane.DockPanel.Theme.ImageService.DockPaneHover_Dock, base.DockPane.DockPanel.Theme.ImageService.DockPane_Dock, base.DockPane.DockPanel.Theme.ImageService.DockPanePress_Dock, base.DockPane.DockPanel.Theme.ImageService.DockPaneActiveHover_Dock, base.DockPane.DockPanel.Theme.ImageService.DockPaneActive_Dock, base.DockPane.DockPanel.Theme.ImageService.DockPaneActiveHover_AutoHide, base.DockPane.DockPanel.Theme.ImageService.DockPaneActive_AutoHide, base.DockPane.DockPanel.Theme.ImageService.DockPanePress_AutoHide);
                    m_toolTip.SetToolTip(m_buttonAutoHide, ToolTipAutoHide);
                    m_buttonAutoHide.Click += AutoHide_Click;
                    base.Controls.Add(m_buttonAutoHide);
                }
                return m_buttonAutoHide;
            }
        }

        private InertButtonBase ButtonOptions
        {
            get
            {
                if (m_buttonOptions == null)
                {
                    m_buttonOptions = new VS2012DockPaneCaptionInertButton(this, base.DockPane.DockPanel.Theme.ImageService.DockPaneHover_Option, base.DockPane.DockPanel.Theme.ImageService.DockPane_Option, base.DockPane.DockPanel.Theme.ImageService.DockPanePress_Option, base.DockPane.DockPanel.Theme.ImageService.DockPaneActiveHover_Option, base.DockPane.DockPanel.Theme.ImageService.DockPaneActive_Option);
                    m_toolTip.SetToolTip(m_buttonOptions, ToolTipOptions);
                    m_buttonOptions.Click += Options_Click;
                    base.Controls.Add(m_buttonOptions);
                }
                return m_buttonOptions;
            }
        }

        private IContainer Components => m_components;

        public Font TextFont => base.DockPane.DockPanel.Theme.Skin.DockPaneStripSkin.TextFont;

        private static string ToolTipClose
        {
            get
            {
                if (_toolTipClose == null)
                {
                    _toolTipClose = GetResourceString("DockPaneCaption_ToolTipClose");
                }
                return _toolTipClose;
            }
        }

        private static string ToolTipOptions
        {
            get
            {
                if (_toolTipOptions == null)
                {
                    _toolTipOptions = GetResourceString("DockPaneCaption_ToolTipOptions");
                }
                return _toolTipOptions;
            }
        }

        private static string ToolTipAutoHide
        {
            get
            {
                if (_toolTipAutoHide == null)
                {
                    _toolTipAutoHide = GetResourceString("DockPaneCaption_ToolTipAutoHide");
                }
                return _toolTipAutoHide;
            }
        }

        private Color TextColor
        {
            get
            {
                if (base.DockPane.IsActivePane)
                {
                    return base.DockPane.DockPanel.Theme.ColorPalette.ToolWindowCaptionActive.Text;
                }
                return base.DockPane.DockPanel.Theme.ColorPalette.ToolWindowCaptionInactive.Text;
            }
        }

        private TextFormatFlags TextFormat
        {
            get
            {
                if (RightToLeft == RightToLeft.No)
                {
                    return _textFormat;
                }
                return _textFormat | TextFormatFlags.RightToLeft | TextFormatFlags.Right;
            }
        }

        private bool CloseButtonEnabled
        {
            get
            {
                if (base.DockPane.ActiveContent == null)
                {
                    return false;
                }
                return base.DockPane.ActiveContent.DockHandler.CloseButton;
            }
        }

        private bool CloseButtonVisible
        {
            get
            {
                if (base.DockPane.ActiveContent == null)
                {
                    return false;
                }
                return base.DockPane.ActiveContent.DockHandler.CloseButtonVisible;
            }
        }

        private bool ShouldShowAutoHideButton => !base.DockPane.IsFloat;

        protected override bool CanDragAutoHide => true;

        public VS2015DockPaneCaption(DockPane pane)
            : base(pane)
        {
            SuspendLayout();
            m_components = new Container();
            m_toolTip = new ToolTip(Components);
            ResumeLayout();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Components.Dispose();
            }
            base.Dispose(disposing);
        }

        protected override int MeasureHeight()
        {
            int num = TextFont.Height + 3 + 2 + 8;
            if (num < ButtonClose.Image.Height + 4 + 3)
            {
                num = ButtonClose.Image.Height + 4 + 3;
            }
            return num;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawCaption(e.Graphics);
        }

        private void DrawCaption(Graphics g)
        {
            if (base.ClientRectangle.Width != 0 && base.ClientRectangle.Height != 0)
            {
                Rectangle clientRectangle = base.ClientRectangle;
                Color toolWindowBorder = base.DockPane.DockPanel.Theme.ColorPalette.ToolWindowBorder;
                ToolWindowCaptionPalette toolWindowCaptionPalette = (!base.DockPane.IsActivePane) ? base.DockPane.DockPanel.Theme.ColorPalette.ToolWindowCaptionInactive : base.DockPane.DockPanel.Theme.ColorPalette.ToolWindowCaptionActive;
                SolidBrush brush = base.DockPane.DockPanel.Theme.PaintingService.GetBrush(toolWindowCaptionPalette.Background);
                g.FillRectangle(brush, clientRectangle);
                g.DrawLine(base.DockPane.DockPanel.Theme.PaintingService.GetPen(toolWindowBorder), clientRectangle.Left, clientRectangle.Top, clientRectangle.Left, clientRectangle.Bottom);
                g.DrawLine(base.DockPane.DockPanel.Theme.PaintingService.GetPen(toolWindowBorder), clientRectangle.Left, clientRectangle.Top, clientRectangle.Right, clientRectangle.Top);
                g.DrawLine(base.DockPane.DockPanel.Theme.PaintingService.GetPen(toolWindowBorder), clientRectangle.Right - 1, clientRectangle.Top, clientRectangle.Right - 1, clientRectangle.Bottom);
                Rectangle rectangle = clientRectangle;
                rectangle.X += 4;
                rectangle.Width -= 7;
                rectangle.Width -= 1 + ButtonClose.Width + 5;
                if (ShouldShowAutoHideButton)
                {
                    rectangle.Width -= ButtonAutoHide.Width + 1;
                }
                if (base.HasTabPageContextMenu)
                {
                    rectangle.Width -= ButtonOptions.Width + 1;
                }
                rectangle.Y += 3;
                rectangle.Height -= 5;
                TextRenderer.DrawText(g, base.DockPane.CaptionText, TextFont, DrawHelper.RtlTransform(this, rectangle), toolWindowCaptionPalette.Text, TextFormat);
                Rectangle rectStrip = rectangle;
                int num = (int)g.MeasureString(base.DockPane.CaptionText, TextFont).Width + 2;
                rectStrip.X += num;
                rectStrip.Width -= num;
                rectStrip.Height = base.ClientRectangle.Height;
                //DrawDotsStrip(g, rectStrip, toolWindowCaptionPalette.Grip);
            }
        }

        protected void DrawDotsStrip(Graphics g, Rectangle rectStrip, Color colorDots)
        {
            if (rectStrip.Width > 0 && rectStrip.Height > 0)
            {
                Pen pen = base.DockPane.DockPanel.Theme.PaintingService.GetPen(colorDots);
                pen.DashStyle = DashStyle.Custom;
                pen.DashPattern = new float[2]
                {
                1f,
                3f
                };
                int num = rectStrip.Height / 2;
                g.DrawLine(pen, rectStrip.X + 2, num, rectStrip.X + rectStrip.Width - 2, num);
                g.DrawLine(pen, rectStrip.X, num - 2, rectStrip.X + rectStrip.Width, num - 2);
                g.DrawLine(pen, rectStrip.X, num + 2, rectStrip.X + rectStrip.Width, num + 2);
            }
        }

        protected override void OnLayout(LayoutEventArgs levent)
        {
            SetButtonsPosition();
            base.OnLayout(levent);
        }

        protected override void OnRefreshChanges()
        {
            SetButtons();
            Invalidate();
        }

        private void SetButtons()
        {
            ButtonClose.Enabled = CloseButtonEnabled;
            ButtonClose.Visible = CloseButtonVisible;
            ButtonAutoHide.Visible = ShouldShowAutoHideButton;
            ButtonOptions.Visible = base.HasTabPageContextMenu;
            ButtonClose.RefreshChanges();
            ButtonAutoHide.RefreshChanges();
            ButtonOptions.RefreshChanges();
            SetButtonsPosition();
        }

        private void SetButtonsPosition()
        {
            Rectangle clientRectangle = base.ClientRectangle;
            int width = ButtonClose.Image.Width;
            int height = ButtonClose.Image.Height;
            Size size = new Size(width, height);
            int x = clientRectangle.X + clientRectangle.Width - 5 - m_buttonClose.Width;
            int y = clientRectangle.Y + (clientRectangle.Height - size.Height) / 2;
            Point location = new Point(x, y);
            ButtonClose.Bounds = DrawHelper.RtlTransform(this, new Rectangle(location, size));
            if (CloseButtonVisible)
            {
                location.Offset(-(width + 1), 0);
            }
            ButtonAutoHide.Bounds = DrawHelper.RtlTransform(this, new Rectangle(location, size));
            if (ShouldShowAutoHideButton)
            {
                location.Offset(-(width + 1), 0);
            }
            ButtonOptions.Bounds = DrawHelper.RtlTransform(this, new Rectangle(location, size));
        }

        private void Close_Click(object sender, EventArgs e)
        {
            base.DockPane.CloseActiveContent();
        }

        private void AutoHide_Click(object sender, EventArgs e)
        {
            base.DockPane.DockState = DockHelper.ToggleAutoHideState(base.DockPane.DockState);
            if (DockHelper.IsDockStateAutoHide(base.DockPane.DockState))
            {
                base.DockPane.DockPanel.ActiveAutoHideContent = null;
                base.DockPane.NestedDockingStatus.NestedPanes.SwitchPaneWithFirstChild(base.DockPane);
            }
        }

        private void Options_Click(object sender, EventArgs e)
        {
            ShowTabPageContextMenu(PointToClient(Control.MousePosition));
        }

        protected override void OnRightToLeftChanged(EventArgs e)
        {
            base.OnRightToLeftChanged(e);
            PerformLayout();
        }

        private static ResourceManager ResMan;
        private static string GetResourceString(string key)
        {
            if (ResMan == null)
            {
                ResMan = new ResourceManager("WeifenLuo.WinFormsUI.ThemeVS2012.Strings", typeof(WeifenLuo.WinFormsUI.ThemeVS2015.VS2015ThemeBase).Assembly);
            }
            return ResMan.GetString(key);
        }
    }

    class VS2015DockPaneCaptionFactory : DockPanelExtender.IDockPaneCaptionFactory
    {
        public DockPaneCaptionBase CreateDockPaneCaption(DockPane pane)
        {
            return new VS2015DockPaneCaption(pane);
        }
    }
}
