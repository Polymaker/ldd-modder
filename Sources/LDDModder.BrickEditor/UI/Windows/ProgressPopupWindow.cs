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
            int offsetX = (parent.Width - Width) / 2;
            int offsetY = (parent.Height - Height) / 2;
            Location = new Point(parent.Left + offsetX, parent.Top + offsetY);

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

        public void UpdateProgress(int current, int max)
        {
            if (InvokeRequired)
            {
                BeginInvoke((Action)(() => UpdateProgress(current, max)));
                return;
            }

            if (max > 0)
            {
                if (progressBar1.Style != ProgressBarStyle.Continuous)
                    progressBar1.Style = ProgressBarStyle.Continuous;

                if (progressBar1.Maximum != max)
                {
                    if (progressBar1.Value > max)
                        progressBar1.Value = current;
                    progressBar1.Maximum = max;
                }

                if (progressBar1.Value != current)
                    progressBar1.Value = current;
            }
            else if (progressBar1.Style != ProgressBarStyle.Marquee)
                progressBar1.Style = ProgressBarStyle.Marquee;
        }
    }
}
