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
            LDD.LDDEnvironment.Initialize();

            var test = Modding.Editing.PartProject.CreateFromLddPart(LDD.LDDEnvironment.Current, 3001);
            test.Save($"{test.PartID} Project");


            //var test = new GLTestWindow();
            //test.Run();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new BrickCreatorWindow());
        }
    }
}
