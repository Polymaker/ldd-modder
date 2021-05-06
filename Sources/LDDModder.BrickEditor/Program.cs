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
        private static NLog.Logger Logger;


        private const string LOG_FILENAME = "BrickStudioLog.txt";
        public static string LogFilePath { get; private set; }

        public static string[] StartupArgs { get; set; }

        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("fr-CA");
            ConfigureLogging();

            Logger.Info("Startup");
            StartupArgs = args;
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            Application.Run(new BrickEditorWindow());
        }

        private static void ConfigureLogging()
        {
            var logConfig = new NLog.Config.LoggingConfiguration();

            LogFilePath = System.IO.Path.Combine(SettingsManager.AppDataFolder, LOG_FILENAME);

            var fileConfig = new NLog.Targets.FileTarget
            {
                Name = "logfile",
                DeleteOldFileOnStartup = true,
                FileName = LogFilePath,
                AutoFlush = true,
                Layout = "[${time}] ${logger:padding=-15} | ${level} > ${message} ${exception:format=tostring}"
            };
            logConfig.AddTarget(fileConfig);
            logConfig.AddRule(NLog.LogLevel.Info, NLog.LogLevel.Off, fileConfig);

            var consoleConfig = new NLog.Targets.ConsoleTarget
            {
                Name = "logconsole"
            };
            logConfig.AddTarget(consoleConfig);
            logConfig.AddRule(NLog.LogLevel.Trace, NLog.LogLevel.Off, consoleConfig);


            NLog.LogManager.Configuration = logConfig;
            Logger = NLog.LogManager.GetLogger("Program");
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
            {
                if (Logger != null)
                    Logger.Error(ex, "UnhandledException");
                MessageBoxEX.ShowDetails(
                    Resources.Messages.Message_UnhandledException + Environment.NewLine + LogFilePath, 
                    Resources.Messages.Caption_UnexpectedError, ex.Message);
            }
            
        }
    }
}
