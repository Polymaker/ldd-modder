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
    public partial class BoundingBoxEditor : UserControl
    {
        private BoundingBox _Value;
        private bool InternalAssign = false;

        [Browsable(false)]
        public BoundingBox Value
        {
            get => _Value;
            set
            {
                if (_Value != value)
                {
                    _Value = value;
                    LoadBoundingBox();
                    OnValueChanged();
                }
            }
        }

        public event EventHandler ValueChanged;

        public BoundingBoxEditor()
        {
            InitializeComponent();
            Height = tableLayoutPanel1.Height;
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
            base.OnLayout(e);
            AdjustTableLayoutPositions();
            if (tableLayoutPanel2.Bottom > Height)
            {
                Height = tableLayoutPanel2.Bottom;
            }
            else if(tableLayoutPanel2.Bottom < Height)
            {
                Height = tableLayoutPanel2.Bottom;
            }
        }

        const int BOX_MARGIN = 3;

        public override Size GetPreferredSize(Size proposedSize)
        {
            var minHeight = tableLayoutPanel1.GetPreferredSize(new Size(999, 999)).Height;

            if (proposedSize.Width >= 180 * 2)
            {
                int boxWidth = proposedSize.Width - (BOX_MARGIN * 5);
                boxWidth = (int)Math.Floor(boxWidth / 6f);
                return new Size((boxWidth * 6) + (BOX_MARGIN * 5), minHeight);
            }
            else
            {
                int boxWidth = proposedSize.Width - (BOX_MARGIN * 2);
                boxWidth = (int)Math.Floor(boxWidth / 3f);
                boxWidth = Math.Max(boxWidth, 60);
                return new Size((boxWidth * 3) + (BOX_MARGIN * 2), minHeight * 2);
            }
        }

        private void AdjustTableLayoutPositions()
        {
            var minHeight = tableLayoutPanel1.GetPreferredSize(new Size(999, 999)).Height;
            tableLayoutPanel1.Height = minHeight;
            tableLayoutPanel2.Height = minHeight;


            if (Width >= 180 * 2)
            {
                int boxWidth = Width - (3 * 5);
                boxWidth = (int)Math.Floor(boxWidth / 6f);
                tableLayoutPanel1.Width = boxWidth * 3 + (3 * 3);
                
                tableLayoutPanel2.Left = tableLayoutPanel1.Width;
                tableLayoutPanel2.Top = 0;
                tableLayoutPanel2.Width = boxWidth * 3 + (3 * 2);

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
        }

        private void OnValueChanged()
        {
            ValueChanged?.Invoke(this, EventArgs.Empty);
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
