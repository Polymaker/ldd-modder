using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace LDDModder.BrickEditor.UI.Controls
{
    [Designer(typeof(ControlLabelDesigner))]
    public partial class ControlLabel : Control
    {
        const string DefaultText = "<Empty>";
        private Size LabelSize;
        private Size Minimum_Size;
        private Rectangle ChildControlBounds;

        private int _LabelWidth;
        private bool _AutoSizeHeight;
        private bool _AutoSizeWidth;

        [DefaultValue(-1), RefreshProperties(RefreshProperties.Repaint)]
        public int LabelWidth
        {
            get => _LabelWidth;
            set
            {
                if ((value > 0 || value == -1) && _LabelWidth != value)
                {
                    _LabelWidth = value;
                    RecalculateLabelSize();
                }
            }
        }

        [DefaultValue(true)]
        public bool AutoSizeHeight
        {
            get => _AutoSizeHeight;
            set
            {
                if (_AutoSizeHeight != value)
                {
                    _AutoSizeHeight = value;
                    AdjustControlSize();
                }
            }
        }

        [DefaultValue(false)]
        public bool AutoSizeWidth
        {
            get => _AutoSizeWidth;
            set
            {
                if (_AutoSizeWidth != value)
                {
                    _AutoSizeWidth = value;
                    AdjustControlSize();
                }
            }
        }


        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Control Control => Controls.Count > 0 ? Controls[0] : null;

        public override Rectangle DisplayRectangle => 
            ChildControlBounds.IsEmpty ? base.DisplayRectangle : ChildControlBounds;

        

        public ControlLabel()
        {
            InitializeComponent();
            _LabelWidth = -1;
            _AutoSizeHeight = true;
            SetStyle(ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, true);
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            RecalculateLabelSize(false);
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            RecalculateLabelSize();
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            RecalculateLabelSize();
        }

        protected override void OnPaddingChanged(EventArgs e)
        {
            base.OnPaddingChanged(e);
            AdjustControlSize();
        }

        private void RecalculateLabelSize(bool invalidate = true)
        {
            string textToMeasure = string.IsNullOrEmpty(Text) ? DefaultText : Text;
            var labelBounds = new Size(LabelWidth > 0 ? LabelWidth : 999999, Height);

            LabelSize = TextRenderer.MeasureText(textToMeasure, Font, labelBounds, 
                TextFormatFlags.WordBreak | TextFormatFlags.EndEllipsis);

            AdjustControlSize();

            if (invalidate)
                Invalidate();
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);
            AdjustControlSize();
            PositionChildControl();
            e.Control.SizeChanged += Control_SizeChanged;
            
        }

        protected override void OnControlRemoved(ControlEventArgs e)
        {
            base.OnControlRemoved(e);
            e.Control.SizeChanged -= Control_SizeChanged;
            AdjustControlSize();
        }

        private void Control_SizeChanged(object sender, EventArgs e)
        {

            //if (AutoSizeWidth || AutoSizeHeight)
            //    AdjustControlSize();

            //if (!AutoSizeWidth && Control != null && Control.Right + Control.Margin.Right > Width)
            //{
            //    SetBounds(0, 0, Control.Right + Control.Margin.Right, 0, BoundsSpecified.Width);
            //}
        }

        private void AdjustControlSize()
        {
            Minimum_Size = new Size(
                LabelSize.Width + Padding.Horizontal, 
                LabelSize.Height + Padding.Vertical);

            if (Control != null)
            {
                int remainingWidth = Width - Minimum_Size.Width;
                if (remainingWidth < 0)
                    remainingWidth = 0;
                var ctrlMinSize = Control.GetPreferredSize(new Size(remainingWidth, Height));
                if (AutoSizeWidth)
                    ctrlMinSize.Width = Control.Width;
                if (AutoSizeHeight)
                    ctrlMinSize.Height = Control.Height;
                ctrlMinSize.Width += Control.Margin.Horizontal;
                ctrlMinSize.Height += Control.Margin.Vertical;

                Minimum_Size.Width += ctrlMinSize.Width;
                Minimum_Size.Height = Math.Max(LabelSize.Height, ctrlMinSize.Height) + Padding.Vertical;

            }
            else
            {
                Minimum_Size.Width += Font.Height;
            }

            CalculateChildControlArea();
            if (Control != null)
                PositionChildControl();

            var boundsToSet = BoundsSpecified.None;
            if ((AutoSizeWidth && Width != MinimumSize.Width) || 
                (!AutoSizeWidth && Width < Minimum_Size.Width))
                boundsToSet = BoundsSpecified.Width;

            if ((AutoSizeHeight && Height != MinimumSize.Height) ||
                (!AutoSizeHeight && Height < Minimum_Size.Height))
                boundsToSet |= BoundsSpecified.Height;

            if (boundsToSet != BoundsSpecified.None)
                SetBounds(0, 0, Minimum_Size.Width, Minimum_Size.Height, boundsToSet);

            
        }

        protected override void OnLayout(LayoutEventArgs levent)
        {
            //if (IsPositionChildControl)
            //    return;
            if (levent.AffectedControl == Control)
                PositionChildControl();
            base.OnLayout(levent);
        }

        private void PositionChildControl()
        {
            Control.Location = new Point(
                ChildControlBounds.Left + Control.Margin.Left, 
                ChildControlBounds.Top + Control.Margin.Top);
        }

        //private bool IsPositionChildControl;
        //private void PositionChildControl()
        //{
        //    if (Control == null || Width < 10 || Height < 10)
        //        return;
        //    IsPositionChildControl = true;
        //    var controlPosX = 0;
        //    var controlPosY = 0;

        //    if (Control.Anchor.HasFlag(AnchorStyles.Right) && !Control.Anchor.HasFlag(AnchorStyles.Left))
        //        controlPosX = ChildControlBounds.Right - Control.Margin.Right - Control.Width;
        //    else if (Control.Anchor.HasFlag(AnchorStyles.Left) && !Control.Anchor.HasFlag(AnchorStyles.Right))
        //        controlPosX = ChildControlBounds.Left + Control.Margin.Left;
        //    else
        //        controlPosX = ChildControlBounds.Left + (ChildControlBounds.Width - Control.Width) / 2;

        //    if (Control.Anchor.HasFlag(AnchorStyles.Bottom) && !Control.Anchor.HasFlag(AnchorStyles.Top))
        //        controlPosY = ChildControlBounds.Bottom - Control.Margin.Bottom - Control.Height;
        //    else if (Control.Anchor.HasFlag(AnchorStyles.Top))
        //        controlPosY = ChildControlBounds.Top + Control.Margin.Top;
        //    else
        //        controlPosY = ChildControlBounds.Top + (ChildControlBounds.Height - Control.Height) / 2;

        //    Control.Location = new Point(controlPosX, controlPosY);
        //    IsPositionChildControl = false;
        //}

        private void CalculateChildControlArea()
        {
            ChildControlBounds = new Rectangle(
                LabelSize.Width + Padding.Left,
                Padding.Top, Width - LabelSize.Width - Padding.Horizontal, 
                Height - Padding.Vertical);
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            if (width < Minimum_Size.Width || AutoSizeWidth)
                width = Minimum_Size.Width;
            if (height < Minimum_Size.Height || AutoSizeHeight)
                height = Minimum_Size.Height;

            base.SetBoundsCore(x, y, width, height, specified);

            CalculateChildControlArea();
            //PositionChildControl();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            string textToDraw = string.IsNullOrEmpty(Text) ? DefaultText : Text;
            TextRenderer.DrawText(pe.Graphics, textToDraw, Font, 
                new Rectangle(0 ,0, LabelSize.Width, Height), ForeColor, 
                TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak | TextFormatFlags.EndEllipsis);

            if (DesignMode && !ChildControlBounds.IsEmpty)
            {
                ControlPaint.DrawFocusRectangle(pe.Graphics, ChildControlBounds);
            }
        }
    }

    internal class ControlLabelDesigner : ParentControlDesigner
    {
        //private DesignerActionListCollection _ActionList;
        //private ISelectionService SelectionService;
        //private bool PassThrough;

        protected override bool AllowControlLasso => false;

        public override void Initialize(IComponent component)
        {
            base.Initialize(component);

            //EnableDesignMode((Control as CollapsiblePanel).ContentPanel, "ContentPanel");
            //SelectionService = (ISelectionService)GetService(typeof(ISelectionService));
            //Panel.MouseClick += Panel_MouseClick;
        }

        public override bool CanParent(Control control)
        {
            if (Control.Controls.Count == 0)
                return true;
            else
                return false;
            //return base.CanParent(control);
        }
    }
}
