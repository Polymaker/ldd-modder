using System;
using System.Collections.Generic;
using System.Linq;
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
        static void Main()
        {
            LDDModder.LDD.LDDEnvironment.Initialize();
            LDDModder.LDD.Parts.PartWrapper.Read(LDDModder.LDD.LDDEnvironment.Current, 3004);
            //var test = new GLTestWindow();
            //test.Run();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new BrickCreatorWindow());
        }
    }
}
