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

        private Size LabelMinSize;
        private Size ControlMinSize;

        private Size TotalMinimumSize => new Size(
                Padding.Horizontal + LabelBounds.Width + ChildControlBounds.Width,
                Padding.Vertical + LabelBounds.Height + ChildControlBounds.Height
            );

        private Rectangle LabelBounds;
        private Rectangle ChildControlBounds;
        private bool BoundsInitialized;

        private int _LabelWidth;
        private bool _AutoSizeHeight;
        private bool _AutoSizeWidth;

        private ContentAlignment _LabelAlignment;

        [DefaultValue(-1), RefreshProperties(RefreshProperties.Repaint)]
        public int LabelWidth
        {
            get => _LabelWidth;
            set
            {
                if ((value > 0 || value == -1) && _LabelWidth != value)
                {
                    AdjustSize(value, false, false, AdjustSpecified.LabelWidth);
                }
            }
        }

        [Browsable(false)]
        public bool AutoSizeLabel => LabelWidth == -1;

        [DefaultValue(ContentAlignment.MiddleLeft), RefreshProperties(RefreshProperties.Repaint)]
        public ContentAlignment LabelAlignment
        {
            get => _LabelAlignment;
            set
            {
                if (_LabelAlignment != value)
                {
                    _LabelAlignment = value;
                    AdjustSize(0, false, false, AdjustSpecified.LabelDisplay);
                    //CalculateLabelSize();
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
                    AdjustSize(0, false, value, AdjustSpecified.AutoHeight);
                    //_AutoSizeHeight = value;
                    //AdjustControlSize();
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
                    AdjustSize(0, value, false, AdjustSpecified.AutoWidth);
                    //_AutoSizeWidth = value;
                    //AdjustControlSize();
                }
            }
        }

        [Browsable(false)]
        public bool AutoSizeAll => AutoSizeWidth && AutoSizeHeight;


        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Control Control => Controls.Count > 0 ? Controls[0] : null;

        public override Rectangle DisplayRectangle => 
            ChildControlBounds.IsEmpty ? base.DisplayRectangle : ChildControlBounds;


        public ControlLabel()
        {
            InitializeComponent();
            _LabelWidth = -1;
            _AutoSizeHeight = true;
            _LabelAlignment = ContentAlignment.MiddleLeft;
            SetStyle(ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, true);
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            CalculateChildControlMinSize(); 
            CalculateLabelSize();
            UpdateLabelBounds();
            UpdateChildControlBounds();
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            AdjustSize(0, false, false, AdjustSpecified.LabelDisplay);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            AdjustSize(0, false, false, AdjustSpecified.LabelDisplay);
        }

        protected override void OnPaddingChanged(EventArgs e)
        {
            base.OnPaddingChanged(e);
            if (AutoSizeWidth || AutoSizeHeight)
            {
                AdjustSize(0, false, false, AdjustSpecified.LabelDisplay);
            }
        }

        private void CalculateLabelSize()
        {
            string textToMeasure = string.IsNullOrEmpty(Text) ? DefaultText : Text;
            var testSize = new Size(LabelWidth > 0 ? LabelWidth : 999999, Height);
            testSize.Width -= Padding.Left;

            LabelMinSize = TextRenderer.MeasureText(textToMeasure, Font, testSize, GetLabelFormatFlags());
            if (LabelWidth > 0)
                LabelMinSize.Width = LabelWidth;
        }

        public int CalculateLabelWidth()
        {
            string textToMeasure = string.IsNullOrEmpty(Text) ? DefaultText : Text;
            var testSize = new Size(999999, Height);
            testSize.Width -= Padding.Left;

            var labelSize = TextRenderer.MeasureText(textToMeasure, Font, testSize, GetLabelFormatFlags());
            return labelSize.Width;
        }

        private void UpdateLabelBounds()
        {
            LabelBounds = new Rectangle(
                Padding.Left,
                Padding.Top,
                LabelMinSize.Width,
                Height - Padding.Vertical);
        }

        private void CalculateChildControlMinSize()
        {
            if (Control != null)
            {
                if (AutoSizeWidth && AutoSizeHeight)
                {
                    ControlMinSize = Control.Size;
                    ControlMinSize.Width += Control.Margin.Horizontal;
                    ControlMinSize.Height += Control.Margin.Vertical;
                }
                else
                {
                    ControlMinSize = new Size(
                        AutoSizeWidth ? (Control.Width + Control.Margin.Horizontal) : Font.Height,
                        AutoSizeHeight ? (Control.Height + Control.Margin.Vertical) : Font.Height);
                    //int remainingWidth = Width - Minimum_Size.Width;
                    //if (remainingWidth < 0)
                    //    remainingWidth = 0;
                    //var ctrlMinSize = Control.GetPreferredSize(new Size(remainingWidth, Height));
                    //if (AutoSizeWidth)
                    //    ctrlMinSize.Width = Control.Width;
                    //if (AutoSizeHeight)
                    //    ctrlMinSize.Height = Control.Height;
                    //ctrlMinSize.Width += Control.Margin.Horizontal;
                    //ctrlMinSize.Height += Control.Margin.Vertical;
                    //ControlMinSize = ctrlMinSize;
                }
            }
            else
            {
                ControlMinSize = new Size(Font.Height, Font.Height);
            }

            UpdateChildControlBounds();
        }

        private void UpdateChildControlBounds()
        {
            var oldPos = ChildControlBounds.Location;
            ChildControlBounds = new Rectangle(
                LabelMinSize.Width + Padding.Left,
                Padding.Top, Width - LabelMinSize.Width - Padding.Horizontal,
                Height - Padding.Vertical);

            if (oldPos != ChildControlBounds.Location)
                PositionChildControl();
        }


        private Rectangle GetChildControlBounds()
        {
            return new Rectangle(
                LabelMinSize.Width + Padding.Left,
                Padding.Top, Width - LabelMinSize.Width - Padding.Horizontal,
                Height - Padding.Vertical);
        }

        private Size GetMinimumSize()
        {
            return new Size(
                   LabelMinSize.Width + ControlMinSize.Width + Padding.Horizontal,
                   Math.Max(LabelMinSize.Height, ControlMinSize.Height) + Padding.Vertical
               );
        }

        private TextFormatFlags GetLabelFormatFlags()
        {
            var flags = TextFormatFlags.Default;

            var alignStr = LabelAlignment.ToString();
            if (alignStr.Contains("Top"))
                flags |= TextFormatFlags.Top;
            else if (alignStr.Contains("Bottom"))
                flags |= TextFormatFlags.Bottom;
            else if (alignStr.Contains("Middle"))
                flags |= TextFormatFlags.VerticalCenter;

            if (alignStr.Contains("Left"))
                flags |= TextFormatFlags.Left;
            else if (alignStr.Contains("Right"))
                flags |= TextFormatFlags.Right;
            else if (alignStr.Contains("Center"))
                flags |= TextFormatFlags.HorizontalCenter;

            return flags | TextFormatFlags.WordBreak | TextFormatFlags.EndEllipsis;
        }

        

        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);
            OnControlAssigned();

            e.Control.SizeChanged += Control_SizeChanged;
            e.Control.MarginChanged += Control_MarginChanged;
        }

        protected override void OnControlRemoved(ControlEventArgs e)
        {
            base.OnControlRemoved(e);
            e.Control.SizeChanged -= Control_SizeChanged;
            e.Control.MarginChanged -= Control_MarginChanged;

            AdjustSize(0, false, false, AdjustSpecified.ControlSize);
            //if (AutoSizeHeight || AutoSizeWidth)
            //    AdjustControlSize();
        }

        private void Control_SizeChanged(object sender, EventArgs e)
        {
            AdjustSize(0, false, false, AdjustSpecified.ControlSize);
        }

        private void Control_MarginChanged(object sender, EventArgs e)
        {
            AdjustSize(0, false, false, AdjustSpecified.ControlSize);
        }


        private enum AdjustSpecified
        {
            None = 0,
            LabelWidth = 1,
            LabelDisplay = 2,
            AutoWidth = 4,
            AutoHeight = 8,
            AutoSize = AutoWidth | AutoHeight,
            ControlSize = 16
        }

        private void AdjustSize(int labelW, bool autoW, bool autoH, AdjustSpecified specified)
        {
            bool autoWChanged = /*autoW != _AutoSizeWidth && */specified.HasFlag(AdjustSpecified.AutoWidth);
            bool autoHChanged = /*autoH != _AutoSizeHeight && */specified.HasFlag(AdjustSpecified.AutoHeight);
            bool labelWChanged = /*labelW != _LabelWidth && */specified.HasFlag(AdjustSpecified.LabelWidth);


            if (labelWChanged || specified.HasFlag(AdjustSpecified.LabelDisplay))
            {
                var oldSize = LabelMinSize;
                if (labelWChanged)
                    _LabelWidth = labelW;

                CalculateLabelSize();
                UpdateLabelBounds();
                UpdateChildControlBounds();

                if (AutoSizeAll)
                {
                    SetBounds(0, 0, 0, 0, BoundsSpecified.Size);
                }
                else
                {
                    ResizeIfNeeded();
                    //var sizeDiff = LabelMinSize - oldSize;
                    //int newW = Width + sizeDiff.Width;
                    //int newH = Height + sizeDiff.Height;
                    //SetBounds(0, 0, newW, newH, BoundsSpecified.Size);
                }
                Invalidate();
            }

            if (autoWChanged)
                _AutoSizeWidth = autoW;

            if (autoHChanged)
                _AutoSizeHeight = autoH;

            if (autoWChanged || autoHChanged || specified.HasFlag(AdjustSpecified.ControlSize))
            {
                CalculateChildControlMinSize();
            }

        }

        protected void OnControlAssigned()
        {
            CalculateChildControlMinSize();

            if (AutoSizeAll)
                SetBounds(0, 0, 0, 0, BoundsSpecified.Size); // Force resize
            else
                ResizeIfNeeded();
        }

        private void ResizeIfNeeded()
        {
            var minSize = GetMinimumSize();

            if (Width < minSize.Width || Height < minSize.Height)
            {
                SetBounds(0, 0,
                    Math.Max(minSize.Width, Width),
                    Math.Max(minSize.Height, Height),
                    BoundsSpecified.Size);
            }
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
            if (Control == null)
                return;


            Control.Location = new Point(
                ChildControlBounds.Left + Control.Margin.Left, 
                ChildControlBounds.Top + Control.Margin.Top);
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            var minSize = new Size(
                    LabelMinSize.Width + ControlMinSize.Width + Padding.Horizontal,
                    Math.Max(LabelMinSize.Height, ControlMinSize.Height) + Padding.Vertical
                );
            return minSize;
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            bool sizeAssigned = false;

            if (specified.HasFlag(BoundsSpecified.Width) ||
                specified.HasFlag(BoundsSpecified.Height))
            {
                sizeAssigned = true;

                var minSize = GetMinimumSize();

                if (width < minSize.Width || AutoSizeWidth)
                    width = minSize.Width;
                if (height < minSize.Height || AutoSizeHeight)
                    height = minSize.Height;
            }

            base.SetBoundsCore(x, y, width, height, specified);

            if (sizeAssigned)
            {
                UpdateLabelBounds();
                UpdateChildControlBounds();
            }
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            string textToDraw = string.IsNullOrEmpty(Text) ? DefaultText : Text;
            TextRenderer.DrawText(pe.Graphics, textToDraw, Font, 
                LabelBounds, ForeColor,
                GetLabelFormatFlags());

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

        private ControlLabel LabelControl => Control as ControlLabel;

        public override SelectionRules SelectionRules
        {
            get
            {
                var rules = base.SelectionRules;
  
                if (LabelControl.AutoSizeHeight)
                    rules &= ~(SelectionRules.TopSizeable | SelectionRules.BottomSizeable);
                if (LabelControl.AutoSizeWidth)
                    rules &= ~(SelectionRules.LeftSizeable | SelectionRules.RightSizeable);

                return rules;
            }
        }

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
