using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace LDDModder
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            //EM_SETMARGINS = 211;
            //EC_RIGHTMARGIN = 2;
            InitializeComponent();

        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            SendMessage(textBox1.Handle, 211, (IntPtr)2, (IntPtr)(20<<16));
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
    }
}
