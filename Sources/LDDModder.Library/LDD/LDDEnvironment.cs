using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.LDD
{
    public class LDDEnvironment
    {
        public string ProgramFilesPath { get; set; }
        public string ApplicationDataPath { get; set; }

        public static LDDEnvironment Current { get; private set; }

        //static LDDEnvironment()
        //{
        //    var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        //    var progFilesPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
        //    if (string.IsNullOrEmpty(progFilesPath))
        //        progFilesPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);


        //}
    }
}
