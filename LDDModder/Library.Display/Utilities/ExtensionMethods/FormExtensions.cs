using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.Windows.Forms
{
    public static class FormExtensions
    {


        public static void DelayInvoke(this Form form, int delayMs, Action action)
        {
            DelayInvoke(form, TimeSpan.FromMilliseconds(delayMs), action);
        }

        public static void DelayInvoke(this Form form, TimeSpan delay, Action action)
        {
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(delay);
                form.Invoke(action);
            });
        }
    }
}
