using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.Windows.Forms
{
    public static class WinformExtensions
    {
        public static void InvokeWithDelay(this Control control, int delay, Action action)
        {
            //Task.Delay(delay).ContinueWith(ac)
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(delay);
                control.BeginInvoke(action);
            });
        }
    }
}
