using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows.Forms.VisualStyles;

namespace LDDModder.BrickEditor.UI.Controls
{
    [Designer(typeof(CollapsiblePanelDesigner))]
    public partial class CollapsiblePanel : Panel
    {
        private bool _Collapsed;
        private int _PanelHeight;
        private int _HeaderHeight;
        private bool _AutoSizeHeight;
        private HeaderDisplayStyle _DisplayStyle;

        private bool InternalSetHeight;
        private bool HasInitialized;
        private int ComputedHeaderHeight;

        private static Bitmap ArrowGlyph;

        [DefaultValue(false)]
        public bool Collapsed
        {
            get => _Collapsed;
            set => SetCollapsed(value);
        }

        [DefaultValue(false)]
        public bool AutoSizeHeight
        {
            get => _AutoSizeHeight;
            set
            {
                if (_AutoSizeHeight != value)
                {
                    _AutoSizeHeight = value;
                    AdjustContainerPanelDock();
                }
            }
        }

        [Browsable(true), RefreshProperties(RefreshProperties.Repaint)]
        [Localizable(true)]
        public override string Text { get => base.Text; set => base.Text = value; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public CollapsiblePanelContainer ContentPanel { get; private set; }

        public int PanelHeight
        {
            get => _PanelHeight;
            set
            {
                if (_PanelHeight != value && value > 6)
                {
                    _PanelHeight = value;
                    if (!AutoSizeHeight)
                        AdjustPanelSize();
                }
            }
        }

        [DefaultValue(-1)]
        public int HeaderHeight
        {
            get => _HeaderHeight;
            set
            {
                if (_HeaderHeight != value && (value == -1 || value >= 6))
                {
                    _HeaderHeight = value;
                    AdjustPanelSize();
                }
            }
        }

        [DefaultValue(HeaderDisplayStyle.Label)]
        public HeaderDisplayStyle DisplayStyle
        {
            get => _DisplayStyle;
            set
            {
                if (_DisplayStyle != value)
                {
                    _DisplayStyle = value;
                    CalculateHeaderHeight(true);
                    Invalidate();
                }
            }
        }

        public override Rectangle DisplayRectangle
        {
            get
            {
                return GetContainerRectangle();
            }
        }

        #region Events

        public event EventHandler CollapsedChanged;

        public event EventHandler AfterCollapse;

        public event EventHandler AfterExpand;

        #endregion

        #region State variables

        private bool IsOverHeader;

        #endregion


        public CollapsiblePanel()
        {
            InitializeComponent();
            _DisplayStyle = HeaderDisplayStyle.Label;
            ContentPanel = new CollapsiblePanelContainer();
            Controls.Add(ContentPanel);
            ContentPanel.Dock = DockStyle.Fill;
            _HeaderHeight = -1;
            _PanelHeight = ContentPanel.Height;
            SetStyle(ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, true);
        }

        #region Base Control Events

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            CalculateHeaderHeight();
            HasInitialized = true;
            if (ArrowGlyph == null && VisualStyleRenderer.IsSupported)
            {
                ArrowGlyph = new Bitmap(20, 20);
                using (var g = Graphics.FromImage(ArrowGlyph))
                {
                    var vsr = new VisualStyleRenderer(VisualStyleElement.ToolBar.SplitButtonDropDown.Normal);
                    vsr.DrawBackground(g, new Rectangle(0, 0, 20, 20));
                }
            }
        }

        //private bool AutoSizeInitialized;

        //protected override void OnResize(EventArgs eventargs)
        //{
        //    base.OnResize(eventargs);

        //    if (!Collapsed && AutoSizeHeight && !AutoSizeInitialized)
        //    {
                
        //        ContentPanel.PerformLayout();
        //        AdjustPanelToContent();
        //        AutoSizeInitialized = true;
        //    }
        //}

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            CalculateHeaderHeight();
        }

        protected override void OnPaddingChanged(EventArgs e)
        {
            base.OnPaddingChanged(e);
            PerformLayout();
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            Invalidate();
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (!DesignMode)
            {
                var headerRect = GetHeaderRectangle();
                if (headerRect.Contains(e.Location) && e.Button == MouseButtons.Left)
                    ToggleCollapsed();
            }
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            if (DesignMode)
            {
                var headerRect = GetHeaderRectangle();
                if (headerRect.Contains(e.Location) && e.Button == MouseButtons.Left)
                    ToggleCollapsed();
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            var headerRect = GetHeaderRectangle();
            if (headerRect.Contains(e.Location))
            {
                if (!IsOverHeader)
                {
                    IsOverHeader = true;
                    Invalidate();
                }
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (IsOverHeader)
            {
                IsOverHeader = false;
                Invalidate();
            }
        }


        #endregion

        private int GetHeaderHeight()
        {
            if (HeaderHeight == -1)
            {
                if (ComputedHeaderHeight == 0)
                    CalculateHeaderHeight();
                return ComputedHeaderHeight;
            }
            return HeaderHeight;
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public Rectangle GetContainerRectangle()
        {
            var rect = new Rectangle(0, 0, Width, Height);

            if (!Collapsed)
            {
                int headerHeight = GetHeaderHeight();
                if (!HasInitialized)
                    headerHeight = Math.Max(headerHeight, Font.Height);
                rect.X += Padding.Left;
                rect.Y += Padding.Top + headerHeight;
                rect.Width -= Padding.Horizontal;
                rect.Height -= Padding.Vertical + headerHeight;
            }

            return rect;
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public Rectangle GetHeaderRectangle()
        {
            return new Rectangle(0, 0, Width, GetHeaderHeight());
        }

        #region Size & layout functions

        internal void AdjustPanelSize()
        {
            InternalSetHeight = true;
            if (!Collapsed)
            {
                Height = PanelHeight + GetHeaderHeight() + Padding.Vertical;
            }
            else
            {
                Height = GetHeaderHeight();
            }
            InternalSetHeight = false;
        }

        public void ForceAdjustHeight()
        {
            AdjustPanelSize();
        }

        internal void AdjustPanelToContent()
        {
            if (!Collapsed && AutoSizeHeight)
            {
                InternalSetHeight = true;
                Height = CalculateHeight();
                InternalSetHeight = false;
            }
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            if (specified.HasFlag(BoundsSpecified.Height))
            {
                if (height == 0)
                    height = Height;

                if (AutoSizeHeight)
                    height = CalculateHeight();
                if (!Collapsed && !InternalSetHeight)
                    _PanelHeight = Math.Max(height - GetHeaderHeight() - Padding.Vertical, 6);
                else if (Collapsed)
                    height = GetHeaderHeight();
            }

            base.SetBoundsCore(x, y, width, height, specified);
        }

        protected override void OnLayout(LayoutEventArgs levent)
        {
            base.OnLayout(levent);

            if (AutoSizeHeight && !Collapsed)
            {
                int newHeight = CalculateHeight();
                if (newHeight != Height)
                    UpdateBounds(Left, Top, Width, newHeight);
            }
        }

        public int CalculateHeight()
        {
            var prefSize = ContentPanel.GetPreferredSize(DisplayRectangle.Size);
            return GetHeaderHeight() + Padding.Vertical + prefSize.Height;
        }

        private void CalculateHeaderHeight(bool performLayout = false)
        {
            if (IsHandleCreated)
            {
                using (var g = CreateGraphics())
                    ComputedHeaderHeight = (int)g.MeasureString("Wasdf123", Font).Height + 6;
                if (DisplayStyle == HeaderDisplayStyle.Button)
                    ComputedHeaderHeight += 4;
            }
            else
                ComputedHeaderHeight = Font.Height + 6;

            if (HeaderHeight == -1 && performLayout)
                PerformLayout();
        }


        #endregion

        #region Collapse/Expand functions

        public void Collapse()
        {
            if (!Collapsed)
                SetCollapsed(true);
        }

        public void ToggleCollapsed()
        {
            SetCollapsed(!Collapsed);
        }

        public void Expand()
        {
            if (Collapsed)
                SetCollapsed(false);
        }

        public void SetCollapsed(bool collapsed)
        {
            if (_Collapsed != collapsed)
            {

                _Collapsed = collapsed;

                SuspendLayout();
                if (collapsed)
                {
                    var curSize = ContentPanel.Size;
                    var curPos = ContentPanel.Location;
                    ContentPanel.Visible = false;
                    ContentPanel.AutoSize = false;
                    ContentPanel.AutoSizeMode = AutoSizeMode.GrowOnly;
                    ContentPanel.Dock = DockStyle.None;
                    ContentPanel.Size = curSize;
                    ContentPanel.Location = curPos;
                }
                else
                {
                    ContentPanel.Dock = AutoSizeHeight ? DockStyle.Top : DockStyle.Fill;
                    ContentPanel.AutoSize = AutoSizeHeight;
                    ContentPanel.AutoSizeMode = AutoSizeHeight ? AutoSizeMode.GrowAndShrink : AutoSizeMode.GrowOnly;
                    ContentPanel.Visible = true;
                }

                ResumeLayout();

                AdjustPanelSize();

                //if (!collapsed)
                //    ContentPanel.PerformLayout();

                CollapsedChanged?.Invoke(this, EventArgs.Empty);

                if (_Collapsed)
                    AfterCollapse?.Invoke(this, EventArgs.Empty);
                else
                    AfterExpand?.Invoke(this, EventArgs.Empty);

                if (!Collapsed && Parent is ScrollableControl scrollableControl)
                {
                    scrollableControl.ScrollControlIntoView(this);
                }
            }
        }

        private void AdjustContainerPanelDock()
        {
            if (!Collapsed && ContentPanel != null)
            { 
                ContentPanel.Dock = AutoSizeHeight ? DockStyle.Top : DockStyle.Fill;
                ContentPanel.AutoSize = AutoSizeHeight;
                ContentPanel.AutoSizeMode = AutoSizeHeight ? AutoSizeMode.GrowAndShrink : AutoSizeMode.GrowOnly;
                if (AutoSizeHeight)
                {
                    ContentPanel.PerformLayout();
                    Height = CalculateHeight();
                }
            }
        }

        #endregion


        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            var headerRect = GetHeaderRectangle();

            if (VisualStyleRenderer.IsSupported)
            {
                if (DisplayStyle == HeaderDisplayStyle.Button)
                {
                    var elemStyle = VisualStyleElement.Button.PushButton.Normal;
                    if (IsOverHeader)
                        elemStyle = VisualStyleElement.Button.PushButton.Hot;
                    var vsr = new VisualStyleRenderer(elemStyle);
                    vsr.DrawBackground(pe.Graphics, headerRect);
                }
                else if (DisplayStyle == HeaderDisplayStyle.Toolstrip)
                {
                    using(var test = new ToolStrip())
                    {
                        test.Width = Width;

                        test.Renderer.DrawToolStripBackground(
                            new ToolStripRenderEventArgs(pe.Graphics, test, headerRect, BackColor));
                    }
                    //var elemStyle = VisualStyleElement.ToolBar.Button.Normal;
                    //var vsr = new VisualStyleRenderer(elemStyle);
                    //vsr.DrawBackground(pe.Graphics, headerRect);
                }
            }

            

            if (ArrowGlyph != null)
            {
                pe.Graphics.TranslateTransform(10, headerRect.Height / 2);
                if (Collapsed)
                    pe.Graphics.RotateTransform(-90);
                pe.Graphics.DrawImage(ArrowGlyph, new Rectangle(-10, -10, 20, 20));
                pe.Graphics.ResetTransform();
            }

            using (var brush = new SolidBrush(ForeColor))
            {
                pe.Graphics.DrawString(Text, Font, brush,
                    new RectangleF(20, 0, Width - 6, headerRect.Height),
                    new StringFormat() { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Near });
            }
        }

        #region MyRegion

        public enum HeaderDisplayStyle
        {
            Label,
            Button,
            Toolstrip,
            OwnerDraw
        }

        [Designer(typeof(CollapsiblePanelContainerDesigner))]
        public class CollapsiblePanelContainer : Panel
        {
            public CollapsiblePanel ParentPanel => Parent as CollapsiblePanel;

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
            public new Size Size
            {
                get => base.Size;
                set => base.Size = value;
            }

            public CollapsiblePanelContainer()
            {
                SetStyle(ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, true);
            }

            protected override void OnLayout(LayoutEventArgs levent)
            {
                base.OnLayout(levent);
                if (Visible && ParentPanel.AutoSizeHeight)
                {
                    int newHeight = ParentPanel.CalculateHeight();
                    ParentPanel.Height = newHeight;
                }
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                if (DesignMode)
                {
                    ControlPaint.DrawFocusRectangle(e.Graphics, ClientRectangle);
                }
            }
        }

        #endregion
    }

    #region Designer Classes

    internal class CollapsiblePanelDesigner : ParentControlDesigner
    {
        private DesignerActionListCollection _ActionList;
        private ISelectionService SelectionService;
        private bool PassThrough;

        protected override bool AllowControlLasso => false;

        private CollapsiblePanel Panel => Control as CollapsiblePanel;

        public override SelectionRules SelectionRules
        {
            get
            {
                var rules = base.SelectionRules;
                if (Panel.Collapsed)
                {
                    rules &= ~(SelectionRules.TopSizeable | SelectionRules.BottomSizeable);
                }
                if (Panel.AutoSizeHeight)
                {
                    rules &= ~(SelectionRules.BottomSizeable);
                }
                return rules;
            }
        }

        public override DesignerActionListCollection ActionLists
        {
            get
            {
                if (_ActionList == null)
                {
                    _ActionList = new DesignerActionListCollection();
                    _ActionList.Add(new CollapsiblePanelActionList(this));
                }

                return _ActionList;
            }
        }

        public override void Initialize(IComponent component)
        {
            base.Initialize(component);
            EnableDesignMode((Control as CollapsiblePanel).ContentPanel, "ContentPanel");
            SelectionService = (ISelectionService)GetService(typeof(ISelectionService));
            //Panel.MouseClick += Panel_MouseClick;
        }

        private void Panel_MouseClick(object sender, MouseEventArgs e)
        {
            var headerRect = Panel.GetHeaderRectangle();
            if (headerRect.Contains(e.Location))
            {
                PassThrough = true;
            }
        }
        

        public override bool CanParent(Control control)
        {
            if (control is CollapsiblePanel.CollapsiblePanelContainer)
                return true;
            return false;
        }

        protected override bool GetHitTest(Point point)
        {
            if (PassThrough)
                return base.GetHitTest(point);

            var headerRect = Panel.GetHeaderRectangle();
            Point pt = Control.PointToClient(point);
            return headerRect.Contains(pt) || headerRect.Contains(point);
            //return base.GetHitTest(point);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0201 || m.Msg == 0x0204)
            {
                PassThrough = true;
                var tmpMsg = new Message()
                {
                    Msg = 0x084,
                    HWnd = m.HWnd,
                    LParam = m.LParam,
                    WParam = m.WParam
                };
                base.WndProc(ref tmpMsg);
                PassThrough = false;
            }

            base.WndProc(ref m);
        }

        //protected override void OnMouseDragBegin(int x, int y)
        //{
        //    base.OnMouseDragBegin(x, y);
        //    Control.Invalidate();
        //}

        private class CollapsiblePanelActionList : DesignerActionList
        {
            public bool Collapsed
            {
                get => (Component as CollapsiblePanel).Collapsed;
                set => (Component as CollapsiblePanel).Collapsed = value;
            }

            public CollapsiblePanelActionList(CollapsiblePanelDesigner designer) : base(designer.Component)
            {
            }
        }

    }

    internal class CollapsiblePanelContainerDesigner : ScrollableControlDesigner
    {
        public override SelectionRules SelectionRules => SelectionRules.Locked;

        protected override void PreFilterProperties(IDictionary properties)
        {
            properties.Remove("AutoSize");
            properties.Remove("AutoSizeMode");
            properties.Remove("Dock");
            properties.Remove("Margin");
            properties.Remove("Visible");
            //properties.Remove("Size");
            properties.Remove("Anchor");
            properties.Remove("MaximumSize");
            properties.Remove("BorderStyle");
            base.PreFilterProperties(properties);
        }
    }

    #endregion

}
