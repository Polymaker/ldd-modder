using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LDDModder.Simple3D;

namespace LDDModder.BrickEditor.UI.Editors
{
    [DefaultEvent("ValueChanged")]
    public partial class VectorEditor : UserControl
    {
        private Vector3d _Value;
        private bool InternalAssign;

        [Browsable(false), Bindable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Vector3d Value
        {
            get => _Value;
            set => SetValue(value);
        }

        public event EventHandler ValueChanged;

        public VectorEditor()
        {
            InitializeComponent();
            _Value = Vector3d.Zero;
            UpdateValueControls();
        }

        private void SetValue(Vector3d value)
        {
            if (value != _Value)
            {
                _Value = value;
                UpdateValueControls();
                ValueChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private void UpdateValueControls()
        {
            InternalAssign = true;
            ValueX.Value = Value.X;
            ValueY.Value = Value.Y;
            ValueZ.Value = Value.Z;
            InternalAssign = false;
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            height = tableLayoutPanel1.Height;
            base.SetBoundsCore(x, y, width, height, specified);
        }

        private void ValueBoxes_ValueChanged(object sender, EventArgs e)
        {
            if (InternalAssign)
                return;
            _Value.X = ValueX.Value;
            _Value.Y = ValueY.Value;
            _Value.Z = ValueZ.Value;
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
