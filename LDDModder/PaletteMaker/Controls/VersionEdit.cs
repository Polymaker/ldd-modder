using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LDDModder.LDD.General;

namespace LDDModder.PaletteMaker.Controls
{
    [ToolboxItem(true)]
    public partial class VersionEdit : UserControl
    {
        // Fields...
        private VersionInfo _Value;

        public VersionInfo Value
        {
            get { return _Value; }
            set
            {
                if (value == null)
                    value = new VersionInfo(1, 0);
                if (value != _Value)
                {
                    if(_Value != null)
                        _Value.PropertyChanged -= Value_PropertyChanged;
                    _Value = value;
                    _Value.PropertyChanged += Value_PropertyChanged;
                    SetTextboxValues();
                }
            }
        }

        public void ResetValue()
        {
            Value = new VersionInfo(1, 0);
        }

        public VersionEdit()
        {
            _Value = new VersionInfo(1, 0);
            InitializeComponent();
            _Value.PropertyChanged += Value_PropertyChanged;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            SetTextboxValues();
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            width = txtMinor.Right;
            height = txtMajor.Height;
            base.SetBoundsCore(x, y, width, height, specified);
        }

        private void SetTextboxValues()
        {
            txtMajor.TextChanged -= txtMajor_TextChanged;
            txtMinor.TextChanged -= txtMinor_TextChanged;

            txtMajor.Text = Value.Major.ToString();
            txtMinor.Text = Value.Minor.ToString();

            txtMajor.TextChanged += txtMajor_TextChanged;
            txtMinor.TextChanged += txtMinor_TextChanged;
        }

        private void Value_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            SetTextboxValues();
        }

        private void txtMajor_TextChanged(object sender, EventArgs e)
        {
            Value.Major = int.Parse(txtMajor.Text);
        }

        private void txtMinor_TextChanged(object sender, EventArgs e)
        {
            Value.Minor = int.Parse(txtMinor.Text);
        }

        private void NumberTextbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            int isNumber = 0;
            e.Handled = !int.TryParse(e.KeyChar.ToString(), out isNumber);
        }
    }
}