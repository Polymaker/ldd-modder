using LDDModder.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace LDDModder
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //if (args.Length > 0 && args[0].Equals("set", StringComparison.InvariantCultureIgnoreCase))
            //{
            //    for (int i = 1; i < args.Length; i++)
            //    {
            //        var prefEntry = PreferenceEntry.Deserialize(args[i]);
            //        LDDModder.LDD.LDDManager.SetSetting(prefEntry.Key, prefEntry.Value, prefEntry.Location);
            //    }
                
            //    return;
            //}
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ModelViewer());
        }
    }
}
