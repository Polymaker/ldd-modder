using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LDDModder.LDD.Primitives;

namespace LDDModder.BrickEditor.UI.Controls
{
    public partial class BoundingBoxEditor : UserControl, INotifyPropertyChanged
    {
        private BoundingBox _Value;
        private bool InternalAssign = false;
        private int MinimumBoxWidth;
        const int BOX_MARGIN = 3;

        [Browsable(false), Bindable(true)]
        public BoundingBox Value
        {
            get => _Value;
            set
            {
                var notNullValue = value ?? new BoundingBox();
                if (!notNullValue.Equals(Value))
                {

                    _Value = notNullValue.Clone();
                    LoadBoundingBox();
                    OnValueChanged();
                }
            }
        }

        public event EventHandler ValueChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        public BoundingBoxEditor()
        {
            InitializeComponent();
            Height = tableLayoutPanel1.Height;
            MinimumBoxWidth = 50;
            _Value = new BoundingBox();
            AdjustTableLayoutPositions();
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            bool adjustLayout = false;

            if (specified.HasFlag(BoundsSpecified.Width) || specified.HasFlag(BoundsSpecified.Height))
            {
                var prefSize = GetPreferredSize(new Size(width, height));
                width = prefSize.Width;
                height = prefSize.Height;
                adjustLayout = true;
                specified |= BoundsSpecified.Width;
                specified |= BoundsSpecified.Height;
            }

            base.SetBoundsCore(x, y, width, height, specified);

            if (adjustLayout)
                AdjustTableLayoutPositions();
        }

        protected override void OnLayout(LayoutEventArgs e)
        {
            //if (e.AffectedControl == tableLayoutPanel1 || e.AffectedControl == tableLayoutPanel2)
            //{

            //}
            base.OnLayout(e);
            AdjustTableLayoutPositions();
        }

        private void TableLayouts_SizeChanged(object sender, EventArgs e)
        {

        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            var minHeight = tableLayoutPanel1.GetPreferredSize(new Size(999, 999)).Height;

            if (proposedSize.Width >= (MinimumBoxWidth * 6) + (BOX_MARGIN * 5))
            {
                int boxWidth = proposedSize.Width - (BOX_MARGIN * 5);
                boxWidth = (int)Math.Floor(boxWidth / 6f);
                return new Size((boxWidth * 6) + (BOX_MARGIN * 5), minHeight);
            }
            else
            {
                int boxWidth = proposedSize.Width - (BOX_MARGIN * 2);
                boxWidth = (int)Math.Floor(boxWidth / 3f);
                boxWidth = Math.Max(boxWidth, MinimumBoxWidth);
                return new Size((boxWidth * 3) + (BOX_MARGIN * 2), minHeight * 2);
            }
        }

        private void AdjustTableLayoutPositions()
        {
            var minHeight = tableLayoutPanel1.GetPreferredSize(new Size(999, 999)).Height;
            tableLayoutPanel1.Height = minHeight;
            tableLayoutPanel2.Height = minHeight;

            if (Width >= (MinimumBoxWidth * 6) + (BOX_MARGIN * 5))
            {
                int boxWidth = Width - (BOX_MARGIN * 5);
                boxWidth = (int)Math.Floor(boxWidth / 6f);
                tableLayoutPanel1.Width = boxWidth * 3 + (BOX_MARGIN * 3);
                
                tableLayoutPanel2.Left = tableLayoutPanel1.Width;
                tableLayoutPanel2.Top = 0;
                tableLayoutPanel2.Width = boxWidth * 3 + (BOX_MARGIN * 2);

                MinX_Box.Margin = new Padding(0, 3, 3, 0);
                MinY_Box.Margin = new Padding(0, 3, 3, 0);
                MinZ_Box.Margin = new Padding(0, 3, 3, 0);
                MaxX_Box.Margin = new Padding(0, 3, 2, 0);
                MaxY_Box.Margin = new Padding(1, 3, 1, 0);
                MaxZ_Box.Margin = new Padding(2, 3, 0, 0);
            }
            else
            {
                tableLayoutPanel1.Width = Width;
                tableLayoutPanel2.Left = 0;
                tableLayoutPanel2.Top = minHeight;
                tableLayoutPanel2.Width = Width;

                MinX_Box.Margin = new Padding(0, 3, 2, 0);
                MinY_Box.Margin = new Padding(1, 3, 1, 0);
                MinZ_Box.Margin = new Padding(2, 3, 0, 0);
                MaxX_Box.Margin = new Padding(0, 3, 2, 0);
                MaxY_Box.Margin = new Padding(1, 3, 1, 0);
                MaxZ_Box.Margin = new Padding(2, 3, 0, 0);
            }

            Height = tableLayoutPanel2.Bottom;
        }

        private void OnValueChanged()
        {
            ValueChanged?.Invoke(this, EventArgs.Empty);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Value"));
        }

        private void LoadBoundingBox()
        {
            InternalAssign = true;

            if (Value != null)
            {
                MinX_Box.Value = Value.MinX;
                MinY_Box.Value = Value.MinY;
                MinZ_Box.Value = Value.MinZ;
                MaxX_Box.Value = Value.MaxX;
                MaxY_Box.Value = Value.MaxY;
                MaxZ_Box.Value = Value.MaxZ;
            }

            InternalAssign = false;
        }

        private void BoundingValueChanged(object sender, EventArgs e)
        {
            if (InternalAssign)
                return;

            if (Value != null)
            {
                _Value.MinX = (float)MinX_Box.Value;
                _Value.MinY = (float)MinY_Box.Value;
                _Value.MinZ = (float)MinZ_Box.Value;
                _Value.MaxX = (float)MaxX_Box.Value;
                _Value.MaxY = (float)MaxY_Box.Value;
                _Value.MaxZ = (float)MaxZ_Box.Value;

                OnValueChanged();
            }
        }

        
    }
}
