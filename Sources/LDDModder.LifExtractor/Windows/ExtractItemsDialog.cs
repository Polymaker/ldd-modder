using LDDModder.LDD.Files;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LDDModder.LifExtractor.Windows
{
    public partial class ExtractItemsDialog : Form
    {
        private Task ExtractionTask;
        private CancellationTokenSource CancellationSource;
        private DateTime ExtractionStart;

        public string SelectedDirectory { get; set; }

        private bool ExtractFolderContent { get; set; }

        public string TargetPath
        {
            get
            {
                if (CreateSubDirectoryCheckBox.IsHandleCreated && CreateSubDirectoryCheckBox.Checked)
                    return Path.Combine(SelectedDirectory, SubDirectoryTextBox.Text);
                return SelectedDirectory;
            }
        }

        public List<LifFile.LifEntry> ItemsToExtract { get; }

        public ExtractItemsDialog()
        {
            InitializeComponent();
            ItemsToExtract = new List<LifFile.LifEntry>();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            UpdateExtractionProgress(LifFile.ExtractionProgress.Default);

            if (ItemsToExtract.Count() == 1 && 
                ItemsToExtract[0] is LifFile.FolderEntry folderEntry)
            {

                SubDirectoryTextBox.Text = folderEntry.IsRootDirectory ? (folderEntry.Lif.Name ?? "LIF") : folderEntry.Name;
                CreateSubDirectoryCheckBox.Checked = true;
                ExtractFolderContent = true;
            }
            else
            {
                CreateSubDirectoryCheckBox.Enabled = false;
                SubDirectoryTextBox.Enabled = false;
            }

            SetWindowTitle();


            if (string.IsNullOrEmpty(SelectedDirectory))
                SelectedDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            SetDestinationPath(SelectedDirectory);
        }

        private void SetWindowTitle()
        {
            if (ItemsToExtract.Count() == 1 &&
                ItemsToExtract[0] is LifFile.FolderEntry folderEntry)
            {
                if (folderEntry.IsRootDirectory)
                    Text = $"Extract Lif";
                else
                    Text = $"Extract folder - {folderEntry.Name}";
            }
            else if (ItemsToExtract.Count() == 1 &&
                ItemsToExtract[0] is LifFile.FileEntry fileEntry)
            {
                Text = $"Extract file - {fileEntry.Name}";
            }
            else
            {
                Text = $"Extract files";
            }
        }

        private void SelectFolderButton_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                fbd.SelectedPath = DestinationTextBox.Text;
                fbd.ShowNewFolderButton = true;

                if (fbd.ShowDialog(this) == DialogResult.OK)
                    SetDestinationPath(fbd.SelectedPath);
            }
        }

        private void SetDestinationTextBoxValue(string value)
        {
            DestinationTextBox.TextChanged -= DestinationTextBox_TextChanged;
            DestinationTextBox.Text = value;
            DestinationTextBox.TextChanged += DestinationTextBox_TextChanged;
        }

        private void SetDestinationPath(string destination)
        {
            if (DestinationTextBox.Text != destination)
                SetDestinationTextBoxValue(destination);

            SelectedDirectory = destination ?? string.Empty;
        }

        private void DestinationTextBox_TextChanged(object sender, EventArgs e)
        {
            SelectedDirectory = DestinationTextBox.Text;
        }

        private void CreateSubDirectoryCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            SubDirectoryTextBox.Enabled = CreateSubDirectoryCheckBox.Checked;
        }

        private void CancelExtractButton_Click(object sender, EventArgs e)
        {
            if (!IsExtracting())
            {
                DialogResult = DialogResult.Cancel;
            }
            else
            {
                CancellationSource.Cancel();
                ExtractionTask.Wait();
            }
        }



        private void ExtractButton_Click(object sender, EventArgs e)
        {
            if (ExtractionTask == null)
            {
                if (ValidateTargetPath())
                    StartExtraction();
            }
            else
            {
                CancelExtraction();
            }
        }

        private bool ValidateTargetPath()
        {
            if (!Path.IsPathRooted(TargetPath))
                return false;

            if (!LDDModder.Utilities.FileHelper.IsValidDirectory(TargetPath))
                return false;

            return true;
        }

        private bool IsExtracting()
        {
            return ExtractionTask?.Status == TaskStatus.Running;
        }

        private void StartExtraction()
        {
            if (!Directory.Exists(TargetPath))
                Directory.CreateDirectory(TargetPath);

            string tmpExtractDir = GetTmpExtractionDirectory();
            
            CancellationSource = new CancellationTokenSource();
            ExtractionStart = DateTime.Now;
            LastestProgress = LifFile.ExtractionProgress.Default;
            ExtractionProgressTimer.Start();

            var entriesArray = ItemsToExtract.ToArray();
            if (ExtractFolderContent)
                entriesArray = (ItemsToExtract[0] as LifFile.FolderEntry).Entries.ToArray();

            ExtractButton.Enabled = false;
            DestinationGroupBox.Enabled = false;

            ExtractionTask = Task.Factory.StartNew(() =>
            {
                try
                {
                    LifFile.ExtractEntries(entriesArray, tmpExtractDir, 
                        CancellationSource.Token, OnExtractionProgress);
                    ExtractionFinished(tmpExtractDir, true);
                }
                catch(Exception ex)
                {
                    ExtractionFinished(tmpExtractDir, false);
                }
            });
        }

        private void ExtractionFinished(string extractionDir, bool success)
        {
            if (ExtractionTask != null && ExtractionTask.Status == TaskStatus.Running)
                ExtractionTask.Wait(2000);

            UpdateExtractionProgress(LastestProgress);

            ExtractionProgressTimer.Stop();

            try
            {
                if (success)
                {
                    var files = Directory.GetFileSystemEntries(extractionDir);
                    var result = NativeMethods.CopyFiles(files, TargetPath);
                }
            }
            finally
            {
                NativeMethods.DeleteFileOrFolder(extractionDir);
            }
        }

        private void CancelExtraction()
        {
            if (ExtractionTask != null && ExtractionTask.Status == TaskStatus.Running)
            {
                CancellationSource.Cancel();
                ExtractionTask.Wait(2000);
            }
        }

        private LifFile.ExtractionProgress LastestProgress;
        private readonly object UpdateLock = new object();

        private void OnExtractionProgress(LifFile.ExtractionProgress progress)
        {
            lock (UpdateLock)
                LastestProgress = progress;
            //BeginInvoke((Action<LifFile.ExtractionProgress>)UpdateExtractionProgress, progress);
        }

        private void ExtractionProgressTimer_Tick(object sender, EventArgs e)
        {
            LifFile.ExtractionProgress currentProgress = null;
            lock (UpdateLock)
                currentProgress = LastestProgress;
            if (currentProgress != null)
                UpdateExtractionProgress(currentProgress);
        }

        private void UpdateExtractionProgress(LifFile.ExtractionProgress progress)
        {
            float percentage = progress.TotalFiles > 0 ? 
                (progress.ExtractedFiles / (float)progress.TotalFiles) * 100f : 0f;

            ExtractionProgressBar.Value = (int)percentage;
            FileProgressValueLabel.Text = $"{progress.ExtractedFiles} / {progress.TotalFiles}";
            ProgressPercentValueLabel.Text = $"{percentage:0}%";


            var timeElapsed = TimeSpan.Zero; // (DateTime.Now - ExtractionStart);
            if (ExtractionStart != default(DateTime))
                timeElapsed = (DateTime.Now - ExtractionStart);

            ElapsedTimeValueLabel.Text = timeElapsed.ToString("hh\\:mm\\:ss");

            var remainingTime = TimeSpan.Zero;

            if (progress.BytesExtracted > 0)
            {
                var avgTime = timeElapsed.TotalMilliseconds / (double)progress.BytesExtracted;
                var remainingBytes = (double)(progress.TotalBytes - progress.BytesExtracted);
                remainingTime = TimeSpan.FromMilliseconds(avgTime * remainingBytes);
            }

            RemainingTimeValueLabel.Text = remainingTime.ToString("hh\\:mm\\:ss");

            CurrentFileLabel.Text = progress.TargetPath;
        }

        
        private string GetTmpExtractionDirectory()
        {
            string tmpFolderName = "LIF" + LDDModder.Utilities.StringUtils.GenerateUID(8);
            return Path.Combine(Path.GetTempPath(), tmpFolderName);
        }

        
    }
}
