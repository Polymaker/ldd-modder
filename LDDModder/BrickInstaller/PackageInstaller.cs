using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LDDModder.LDD;
using LDDModder.Modding;
using System.Threading;
using System.IO;
using LDDModder.LDD.Files;
using System.Diagnostics;
namespace LDDModder.BrickInstaller
{
    internal static class PackageInstaller
    {

        public static bool ValidateLddInstall()
        {
            ProgressLogger.UpdateStatus(LocalizedStrings.StepValidateLDD);
            
            if (!LDDManager.InstallChecked)
            {
                ProgressLogger.SetProgress(0, 0);
                ProgressLogger.LogProgress(LocalizedStrings.StepFindLDD);
                LDDManager.InitializeDirectories();
            }

            if (!LDDManager.IsInstalled)
            {
                ProgressLogger.LogProgress(LocalizedStrings.LogInstallNotFound, ProgressLogger.LogType.Error);

                if (ProgressLogger.ShowMessageBox(LocalizedStrings.LogInstallNotFound + Environment.NewLine + LocalizedStrings.DiagFindFolder, "", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                    return false;

                using (var dlg = new FolderBrowserDialog())
                {
                    if (ProgressLogger.ShowDialog(dlg) == DialogResult.Cancel)
                        goto ValidationFailed;
                    LDDManager.SetApplicationPath(dlg.SelectedPath);
                }

                if (!LDDManager.IsInstalled)
                    goto ValidationFailed;
            }
            
            if(LDDManager.IsInstalled)
                ProgressLogger.LogProgress(string.Format(LocalizedStrings.LogInstallFound, LDDManager.ApplicationPath), ProgressLogger.LogType.Info);

            if (string.IsNullOrEmpty(LDDManager.ApplicationDataPath))
            {
                ProgressLogger.LogProgress(LocalizedStrings.LogAppDataNotFound, ProgressLogger.LogType.Error);
                goto ValidationFailed;
            }

            ProgressLogger.UpdateStatus(LocalizedStrings.StepCheckLif);

            if (!LDDManager.IsLifExtracted(LifInstance.Database))
            {
                ProgressLogger.LogProgress(string.Format(LocalizedStrings.LogLifNotExtracted, "db.lif"), ProgressLogger.LogType.Warning);
                if (ProgressLogger.ShowMessageBox(LocalizedStrings.DiagExtractDbLifText, LocalizedStrings.DiagExtractDbLifTitle, MessageBoxButtons.YesNo) != DialogResult.Yes)
                    goto ValidationFailed;

                LogLifExtract(LifInstance.Database);
            }
            
            if(LDDManager.IsLifExtracted(LifInstance.Database))
                ProgressLogger.LogProgress(string.Format(LocalizedStrings.LogLifExtracted, "db.lif"), ProgressLogger.LogType.Info);

            if (!LDDManager.IsLifExtracted(LifInstance.Assets))
                ProgressLogger.LogProgress(string.Format(LocalizedStrings.LogLifNotExtracted, "Assets.lif"), ProgressLogger.LogType.Info);
            else
                ProgressLogger.LogProgress(string.Format(LocalizedStrings.LogLifExtracted, "Assets.lif"), ProgressLogger.LogType.Info);

        
            return true;

        ValidationFailed:
            ProgressLogger.SetProgress(-1, -1);
            return false;
        }


        internal static void LogLifExtract(LifInstance lif)
        {
            if (LDDManager.IsLifExtracted(lif))
                return;

            string lifDir = LDDManager.GetLifDirectory(lif);

            Directory.CreateDirectory(lifDir);
            
            using (var lifFile = LDDManager.OpenLif(lif))
            {
                int totalFiles = lifFile.Entries.Count(x => !x.IsDirectory);
                ProgressLogger.SetProgress(0, totalFiles);
                int ctr = 0;
                var timer = Stopwatch.StartNew();
                foreach (var fileEntry in lifFile.Entries.Where(x => !x.IsDirectory))
                {
                    var targetPath = Path.Combine(lifDir, fileEntry.FullPath);
                    if (!Directory.Exists(Path.GetDirectoryName(targetPath)))
                        Directory.CreateDirectory(Path.GetDirectoryName(targetPath));

                    if (timer.ElapsedMilliseconds > 70)
                    {
                        ProgressLogger.LogProgress("Extracting " + targetPath);
                        timer.Restart();
                    }
                    ((LifFile.FileEntry)fileEntry).Extract(targetPath);
                    ProgressLogger.UpdateProgress(ctr++);
                }
            }
            LDDManager.DiscardOfLif(lif);
        }
    }
}
