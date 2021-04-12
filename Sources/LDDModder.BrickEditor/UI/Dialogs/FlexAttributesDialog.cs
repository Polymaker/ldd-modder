using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LDDModder.BrickEditor.UI
{
    public partial class FlexAttributesDialog : Form
    {
        public double[] FlexAttributes { get; set; }

        public FlexAttributesDialog()
        {
            InitializeComponent();
            FlexAttributes = new double[5];
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (FlexAttributes != null)
            {
                ValueBox1.Value = FlexAttributes[0];
                ValueBox2.Value = FlexAttributes[1];
                ValueBox3.Value = FlexAttributes[2];
                ValueBox4.Value = FlexAttributes[3];
                ValueBox5.Value = FlexAttributes[4];
            }
        }

        private void ActionButton_Click(object sender, EventArgs e)
        {
            FlexAttributes[0] = ValueBox1.Value;
            FlexAttributes[1] = ValueBox2.Value;
            FlexAttributes[2] = ValueBox3.Value;
            FlexAttributes[3] = ValueBox4.Value;
            FlexAttributes[4] = ValueBox5.Value;
            this.DialogResult = DialogResult.OK;
        }
    }
}
