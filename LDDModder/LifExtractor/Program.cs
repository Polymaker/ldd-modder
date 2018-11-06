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

            if (args.Any(a => a.ToLower().Contains("-help")))
            {
                Console.WriteLine("usage: LifExtractor [path] ([destination])");
                return;
            }

            if (File.Exists(args[0]) && Path.GetExtension(args[0]).ToLower().Contains("lif"))
            {
                var destDir = Path.GetDirectoryName(Application.ExecutablePath);

                if (args.Length > 1)
                    destDir = args[1];

                if (!Directory.Exists(destDir))
                    Directory.CreateDirectory(destDir);

                var destinationPath = Path.Combine(destDir, Path.GetFileNameWithoutExtension(args[0]));
                //Directory.CreateDirectory(dirPath);
                using (var lifFile = LifFile.Open(args[0]))
                    lifFile.Extract(destinationPath);
            }
        }
    }
}
