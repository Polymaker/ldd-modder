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
using System.Runtime.InteropServices;
using LDDModder.Simple3D;

namespace LDDModder.BrickEditor.UI.Controls
{
    public partial class BoundingBoxEditor : UserControl, INotifyPropertyChanged
    {
        private BoundingBox _Value;
        private bool InternalAssign = false;
        private bool IsOnTwoLines;
        private int MinimumBoxWidth;
        const int BOX_MARGIN = 3;

        [Browsable(false), Bindable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public BoundingBox Value
        {
            get => _Value;
            set => SetCurrentValue(value);
        }

        public event EventHandler ValueChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        public BoundingBoxEditor()
        {
            InitializeComponent();
            Height = tableLayoutPanel1.Height;
            MinimumBoxWidth = 50;
            IsOnTwoLines = false;
            _Value = new BoundingBox();
            AdjustTableLayoutPositions();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            AdjustTableLayoutPositions();
            Height = tableLayoutPanel2.Bottom;
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            //var curSize = new Size(Width, Height);
            //bool raiseSizeChanged = false;
            if (width > 0 || height > 0)
            {
                var prefSize = CalculateSize(new Size(width, height), out IsOnTwoLines);
                width = prefSize.Width;
                height = prefSize.Height;
                //if (!specified.HasFlag(BoundsSpecified.Width) || !specified.HasFlag(BoundsSpecified.Height))
                //    raiseSizeChanged = true;
                specified |= BoundsSpecified.Size;
            }

            base.SetBoundsCore(x, y, width, height, specified);

            //if (curSize.Width != width || curSize.Height != height)
            //    AdjustTableLayoutPositions();

            //if (raiseSizeChanged)
            //    UpdateBounds(Left, Top, width, height);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            AdjustTableLayoutPositions();
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct WINDOWPOS
        {
            public IntPtr hwnd;
            public IntPtr hwndInsertAfter;
            public int x;
            public int y;
            public int cx;
            public int cy;
            public uint flags;
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x0046:
                    {
                        var posInfo = Marshal.PtrToStructure<WINDOWPOS>(m.LParam);
                        var adjustedSize = CalculateSize(new Size(posInfo.cx, posInfo.cy), out IsOnTwoLines);
                        posInfo.cx = adjustedSize.Width;
                        posInfo.cy = adjustedSize.Height;
                        Marshal.StructureToPtr(posInfo, m.LParam, true);
                        break;
                    }
            }
            base.WndProc(ref m);

        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            var minHeight = tableLayoutPanel1.GetPreferredSize(new Size(999, 999)).Height;
            if (proposedSize.Width < 6)
                proposedSize.Width = Width;

            if (IsOnTwoLines/*   proposedSize.Width >= (MinimumBoxWidth * 6) + (BOX_MARGIN * 5)*/)
            {
                int boxWidth = proposedSize.Width - (BOX_MARGIN * 5);
                boxWidth = (int)Math.Floor(boxWidth / 6f);
                boxWidth = Math.Max(boxWidth, MinimumBoxWidth);
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

        private Size CalculateSize(Size proposedSize, out bool onTwoLines)
        {
            var minHeight = tableLayoutPanel1.GetPreferredSize(new Size(999, 999)).Height;
            if (proposedSize.Width < 6)
                proposedSize.Width = Width;
            if (proposedSize.Width >= (MinimumBoxWidth * 6) + (BOX_MARGIN * 5))
            {
                int boxWidth = proposedSize.Width - (BOX_MARGIN * 5);
                boxWidth = (int)Math.Floor(boxWidth / 6f);
                onTwoLines = false;
                return new Size((boxWidth * 6) + (BOX_MARGIN * 5), minHeight);
            }
            else
            {
                int boxWidth = proposedSize.Width - (BOX_MARGIN * 2);
                boxWidth = (int)Math.Floor(boxWidth / 3f);
                boxWidth = Math.Max(boxWidth, MinimumBoxWidth);
                onTwoLines = true;
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

            //Height = tableLayoutPanel2.Bottom;
        }

        private void OnValueChanged()
        {
            ValueChanged?.Invoke(this, EventArgs.Empty);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Value"));
        }

        private void SetCurrentValue(BoundingBox value)
        {
            var notNullValue = value ?? new BoundingBox();
            if (_Value != notNullValue)
            {
                _Value = notNullValue;
                LoadBoundingBox();
                OnValueChanged();
            }
        }

        private void SetValueFromControls()
        {
            var min = new Vector3()
            {
                X = (float)MinX_Box.Value,
                Y = (float)MinY_Box.Value,
                Z = (float)MinZ_Box.Value
            };

            var max = new Vector3()
            {
                X = (float)MaxX_Box.Value,
                Y = (float)MaxY_Box.Value,
                Z = (float)MaxZ_Box.Value
            };

            _Value = new BoundingBox(min, max);

            OnValueChanged();
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

            SetValueFromControls();
        }
    }
}
