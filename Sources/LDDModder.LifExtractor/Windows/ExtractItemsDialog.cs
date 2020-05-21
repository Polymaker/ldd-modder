using LDDModder.LDD.Files;
using LDDModder.Utilities;
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

        private bool CopyingFilesToDest;

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

            SetupWindowSize();

           

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

            ProgressGroupBox.Enabled = false;

            SetWindowTitle();

            if (string.IsNullOrEmpty(SelectedDirectory))
                SelectedDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            SetDestinationPath(SelectedDirectory);
            
        }

        private void SetupWindowSize()
        {
            var formBorderSize = Height - ClientRectangle.Height;
            var minimumHeight =
                DestinationGroupBox.Height +
                DestinationGroupBox.Margin.Vertical +
                ProgressGroupBox.Height +
                ProgressGroupBox.Margin.Vertical +
                Padding.Vertical +
                ExtractButton.Height +
                ExtractButton.Margin.Vertical +
                ButtonsTableLayout.Padding.Vertical + formBorderSize;

            MinimumSize = new Size(MinimumSize.Width, minimumHeight);
        }

        private void SetWindowTitle()
        {
            if (ItemsToExtract.Count() == 1 &&
                ItemsToExtract[0] is LifFile.FolderEntry folderEntry)
            {
                if (folderEntry.IsRootDirectory && !string.IsNullOrEmpty(folderEntry.Lif.Name))
                    Text = $"Extract - {Path.GetFileName(folderEntry.Lif.FilePath)}";
                else if (folderEntry.IsRootDirectory)
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
            if (IsExtracting())
            {
                CancelExtraction();
            }
            else if (!CopyingFilesToDest)
            {
                DialogResult = DialogResult.Cancel;
            }
        }

        private void ExtractButton_Click(object sender, EventArgs e)
        {
            if (!IsExtracting())
            {
                if (ValidateTargetPath())
                    StartExtraction();
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
            CreateDestinationFolder();

            string tmpExtractDir = GetTmpExtractionDirectory();
            
            CancellationSource = new CancellationTokenSource();
            ExtractionStart = DateTime.Now;
            LastestProgress = LifFile.ExtractionProgress.Default;
            ExtractionProgressTimer.Start();
            extractProgressPanel1.BeginExtraction();

            var entriesArray = ItemsToExtract.ToArray();
            if (ExtractFolderContent)
                entriesArray = (ItemsToExtract[0] as LifFile.FolderEntry).Entries.ToArray();

            ExtractButton.Enabled = false;
            DestinationGroupBox.Enabled = false;
            ProgressGroupBox.Enabled = true;

            ExtractionTask = Task.Factory.StartNew(() =>
            {
                bool extractionSucceded = false;

                try
                {
                    LifFile.ExtractEntries(entriesArray, tmpExtractDir, 
                        CancellationSource.Token, OnExtractionProgress);
                    extractionSucceded = true;
                }
                catch
                {

                }

                BeginInvoke((Action)(() => ExtractionFinished(tmpExtractDir, extractionSucceded)));
            });
        }

        private void CreateDestinationFolder()
        {
            if (!Directory.Exists(TargetPath))
            {
                bool adminRightsRequired = false;
                try
                {
                    Directory.CreateDirectory(TargetPath);
                }
                catch (UnauthorizedAccessException)
                {
                    adminRightsRequired = true;
                }

                if (adminRightsRequired)
                {
                    string tmpTargetDir = GetTmpExtractionDirectory();
                    Directory.CreateDirectory(tmpTargetDir);
                    bool success = FileHelper.MoveFile(tmpTargetDir, TargetPath, false);
                }
            }
        }

        private void ExtractionFinished(string extractionDir, bool success)
        {
            extractProgressPanel1.UpdateProgress(LastestProgress);
            
            ExtractionProgressTimer.Stop();
            CancelExtractButton.Enabled = false;

            Application.DoEvents();

            if (ExtractionTask != null && ExtractionTask.Status == TaskStatus.Running)
                ExtractionTask.Wait(2000);

            extractProgressPanel1.FinishExtraction();
            //ExtractingLabel.Visible = false;
            //CurrentFileLabel.Text = "Copying files to destination...";
            CancelExtractButton.Text = "Close";

            try
            {
                if (success)
                {
                    CopyingFilesToDest = true;
                    var extractedFiles = Directory.GetFileSystemEntries(extractionDir);
                    var result = FileHelper.MoveFiles(extractedFiles, TargetPath, false);
                    if (!result)
                    {
                        //show error message
                    }
                }
            }
            finally
            {
                CopyingFilesToDest = false;
                NativeMethods.DeleteFileOrFolder(extractionDir);
            }

            CancelExtractButton.Enabled = true;
        }

        private bool CancelExtraction()
        {
            
            if (ExtractionTask != null && ExtractionTask.Status == TaskStatus.Running)
            {
                var msgResult = MessageBox.Show(this, "Are you sure you want to cancel?", "", MessageBoxButtons.YesNo);

                if (msgResult == DialogResult.Yes)
                {
                    CancellationSource.Cancel();
                    ExtractionTask.Wait(2000);
                    return true;
                }
            }

            return false;
        }

        private LifFile.ExtractionProgress LastestProgress;
        private readonly object UpdateLock = new object();

        private void OnExtractionProgress(object sender, LifFile.ExtractionProgress progress)
        {
            lock (UpdateLock)
                LastestProgress = progress;
        }

        private void ExtractionProgressTimer_Tick(object sender, EventArgs e)
        {
            LifFile.ExtractionProgress currentProgress = null;
            lock (UpdateLock)
                currentProgress = LastestProgress;

            if (currentProgress != null)
                extractProgressPanel1.UpdateProgress(currentProgress);
        }

  
        private string GetTmpExtractionDirectory()
        {
            string tmpFolderName = "LIF" + StringUtils.GenerateUID(8);
            return Path.Combine(Path.GetTempPath(), tmpFolderName);
        }

        private void ExtractItemsDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CopyingFilesToDest || 
                (IsExtracting() && !CancelExtraction()))
            {
                e.Cancel = true;
            }
        }
    }
}
