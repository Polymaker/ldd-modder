using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LDDModder.BrickEditor.UI.Windows
{
    public partial class ProgressPopupWindow : Form
    {

        public string Message
        {
            get => MessageLabel.Text;
            set => MessageLabel.Text = value;
        }

        public ProgressPopupWindow()
        {
            InitializeComponent();
            MessageLabel.Text = string.Empty;
        }

        public void ShowCenter(Form parent)
        {
            
            StartPosition = FormStartPosition.Manual;

            parent.Move += Parent_Move;
            Show(parent);

            CenterToParent();
        }

        private void Parent_Move(object sender, EventArgs e)
        {
            CenterToParent();
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (!Visible)
            {
                Owner.Move -= Parent_Move;
                progressBar1.Style = ProgressBarStyle.Blocks;
            }
            else
            {
                progressBar1.Style = ProgressBarStyle.Marquee;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.DrawRectangle(Pens.Black, 0, 0, Width - 1, Height - 1);
        }
    }
}
