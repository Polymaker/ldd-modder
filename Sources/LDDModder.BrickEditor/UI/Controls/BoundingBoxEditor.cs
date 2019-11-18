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
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            if ((specified & BoundsSpecified.Height) == BoundsSpecified.Height && tableLayoutPanel1.IsHandleCreated)
                height = tableLayoutPanel1.Height;
            if ((specified & BoundsSpecified.Width) == BoundsSpecified.Width)
                width = Math.Max(width, 300);
            base.SetBoundsCore(x, y, width, height, specified);
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
