using LDDModder.BrickEditor.Settings;
using LDDModder.BrickEditor.UI;
using LDDModder.BrickEditor.UI.Windows;
using LDDModder.BrickEditor.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LDDModder.BrickEditor
{
    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("fr-CA");
     
            if (args != null && args.Length > 0)
            {
                
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new BrickEditorWindow());
        }
    }
}
