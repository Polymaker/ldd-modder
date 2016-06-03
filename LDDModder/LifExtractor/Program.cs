using LDDModder.LDD.Files;
using LDDModder.LDD.Palettes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace LifExtractor
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        //[STAThread]
        //static void Main()
        //{
        //    Application.EnableVisualStyles();
        //    Application.SetCompatibleTextRenderingDefault(false);
        //    Application.Run(new Form1());
        //}
        
        static void Main(string[] args)
        {
            if (args == null || args.Length == 0)
                return;

            if (File.Exists(args[0]) && Path.GetExtension(args[0]).ToLower().Contains("lif"))
            {
                var dirPath = Path.GetDirectoryName(args[0]);
                dirPath = Path.Combine(dirPath, Path.GetFileNameWithoutExtension(args[0]));
                //Directory.CreateDirectory(dirPath);
                using (var lifFile = LifFile.Open(args[0]))
                    lifFile.Extract(dirPath);
            }
        }
    }
}
