using LDDModder.LifExtractor.Windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LDDModder.LifExtractor
{
    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //TODO: Implement argument handling:
            // Args : 
            //  [Target Lif File]: self-explanatory
            //  [Target Directory]: optional, when not specified extract beside file
            // Params: 
            //  -nowindow: extract directly without dialog 
            //  -f: required to overwrite files when -nowindow is used

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new LifViewerWindow());
        }
    }
}
