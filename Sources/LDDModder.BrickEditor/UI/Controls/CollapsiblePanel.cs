using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace LDDModder.BrickEditor.UI.Controls
{
    [Designer(typeof(CollapsiblePanelDesigner))]
    public partial class CollapsiblePanel : Panel
    {
        private bool _Collapsed;
        private int _PanelHeight;
        private int _HeaderHeight;
        private int HeaderHeightComputed;
        private bool InternalSetHeight;

        [DefaultValue(false)]
        public bool Collapsed
        {
            get => _Collapsed;
            set => SetCollapsed(value);
        }

        [Browsable(true)]
        public override string Text { get => base.Text; set => base.Text = value; }

        #region MyRegion

        [Designer(typeof(CollapsiblePanelContainerDesigner))]
        public class CollapsiblePanelContainer : Panel
        {
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

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public CollapsiblePanelContainer ContentPanel { get; private set; }

        [DefaultValue(-1)]
        public int PanelHeight
        {
            get => _PanelHeight;
            set
            {
                if (_PanelHeight != value && value > 6)
                {
                    _PanelHeight = value;
                    AdjustPanelSize();
                }
            }
        }

        public event EventHandler CollapsedChanged;

        public event EventHandler AfterCollapse;

        public event EventHandler AfterExpand;

        public CollapsiblePanel()
        {
            InitializeComponent();
            ContentPanel = new CollapsiblePanelContainer();
            Controls.Add(ContentPanel);
            ContentPanel.Dock = DockStyle.Fill;
            Padding = new Padding(3, 10, 3, 3);
            _HeaderHeight = -1;
            _PanelHeight = ContentPanel.Height;
            
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            CalculateHeaderHeight();
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            CalculateHeaderHeight();
        }

        private void AdjustPanelSize()
        {
            InternalSetHeight = true;
            if (!Collapsed)
            {
                Padding = new Padding(3, HeaderHeightComputed, 3, 3);
                Height = PanelHeight + Padding.Vertical;
            }
            else
            {
                Padding = new Padding(3, HeaderHeightComputed, 3, 3);
                Height = HeaderHeightComputed;
            }
            InternalSetHeight = false;
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            if (specified.HasFlag(BoundsSpecified.Height))
            {
                if (!Collapsed && !InternalSetHeight)
                    _PanelHeight = Math.Max(height - Padding.Vertical, 6);
                else if (Collapsed)
                    height = HeaderHeightComputed;
            }

            base.SetBoundsCore(x, y, width, height, specified);
        }

        private void CalculateHeaderHeight()
        {
            if (IsHandleCreated)
            {
                using (var g = CreateGraphics())
                    HeaderHeightComputed = (int)g.MeasureString("Wasdf123", Font).Height + 6;
                Padding = new Padding(3, HeaderHeightComputed, 3, 3);
            }
        }

        public void Collapse()
        {
            if (!Collapsed)
                SetCollapsed(true);
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

                if (collapsed)
                {
                    var curSize = ContentPanel.Size;
                    var curPos = ContentPanel.Location;
                    ContentPanel.Visible = false;
                    ContentPanel.Dock = DockStyle.None;
                    ContentPanel.Size = curSize;
                    ContentPanel.Location = curPos;
                }
                else
                {
                    ContentPanel.Dock = DockStyle.Fill;
                    ContentPanel.Visible = true;
                }

                AdjustPanelSize();
                CollapsedChanged?.Invoke(this, EventArgs.Empty);
                if (_Collapsed)
                    AfterCollapse?.Invoke(this, EventArgs.Empty);
                else
                    AfterExpand?.Invoke(this, EventArgs.Empty);
            }
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            SetCollapsed(!Collapsed);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);

            using (var brush = new SolidBrush(ForeColor))
            {
                pe.Graphics.DrawString(Text, Font, brush,
                    new RectangleF(3, 0, Width - 6, HeaderHeightComputed),
                    new StringFormat() { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Near });
            }
        }
    }

    internal class CollapsiblePanelDesigner : ParentControlDesigner
    {
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
                return rules;
            }
        } 

        public override void Initialize(IComponent component)
        {
            base.Initialize(component);
            EnableDesignMode((Control as CollapsiblePanel).ContentPanel, "ContentPanel");
        }

        public override bool CanParent(Control control)
        {
            if (control is CollapsiblePanel.CollapsiblePanelContainer)
                return true;
            return false;
        }
    }

    internal class CollapsiblePanelContainerDesigner : ScrollableControlDesigner
    {
        public override SelectionRules SelectionRules => SelectionRules.Locked;

        protected override void PreFilterProperties(IDictionary properties)
        {
            properties.Remove("Dock");
            properties.Remove("Margin");
            base.PreFilterProperties(properties);
        }
    }
}
