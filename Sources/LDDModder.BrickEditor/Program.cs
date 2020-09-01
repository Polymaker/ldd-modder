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
        static void Main()
        {
            //LDD.LDDEnvironment.Initialize();

            //Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-CA");
            //CheckMultiInstance();
            

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new BrickEditorWindow());
        }

        //static void CheckMultiInstance()
        //{
        //    int currInstanceID = 0;
        //    while (true)
        //    {
        //        var instanceMutex = new Mutex(true, $"9A3C8D23-BCA6-4485-965A-INSTANCE-{currInstanceID}");
        //        if (instanceMutex.WaitOne(500))
        //        {
        //            SettingsManager.AppInstanceID = currInstanceID;
        //            break;
        //        }
        //        else
        //        {
        //            currInstanceID++;
        //            instanceMutex.Close();
        //            instanceMutex.Dispose();
        //        }
        //    }
        //}
    }
}
