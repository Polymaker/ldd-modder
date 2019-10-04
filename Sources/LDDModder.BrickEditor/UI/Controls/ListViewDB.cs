using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LDDModder.BrickEditor.UI.Controls
{
    public partial class ListViewDB : System.Windows.Forms.ListView
    {
        public ListViewDB()
        {
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer/* | ControlStyles.AllPaintingInWmPaint*/, true);

            //Enable the OnNotifyMessage event so we get a chance to filter out 
            // Windows messages before they get to the form's WndProc
            //SetStyle(ControlStyles.EnableNotifyMessage, true);
        }

        protected override void OnNotifyMessage(Message m)
        {
            //Filter out the WM_ERASEBKGND message
            if (m.Msg != 0x14)
                base.OnNotifyMessage(m);
        }
    }
}
