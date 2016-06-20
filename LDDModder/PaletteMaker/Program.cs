using LDDModder.LDD.General;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LDDModder.PaletteMaker
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //var test = new VersionInfoConverter();
            //var ver = new VersionInfo(3, 4);
            //var res = test.ConvertTo(ver, typeof(InstanceDescriptor));
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmCreateSetPalette());
        }
    }
}
