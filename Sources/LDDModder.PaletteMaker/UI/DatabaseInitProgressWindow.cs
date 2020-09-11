using LDDModder.LDD;
using LDDModder.PaletteMaker.DB;
using LDDModder.PaletteMaker.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LDDModder.PaletteMaker.UI
{
    public partial class DatabaseInitProgressWindow : Form, IDbInitProgressHandler
    {
        private Task InitUpdateTask;
        private CancellationTokenSource CTS;
        private int TotalSteps;
        private int CurrentStep;

        public DatabaseInitProgressWindow()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            StartInitialization();
        }

        private void StartInitialization()
        {
            CTS = new CancellationTokenSource();
            InitUpdateTask = Task.Factory.StartNew(InitOrUptateDatabase);
            ProgressReportTimer.Start();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (InitUpdateTask != null && InitUpdateTask.Status == TaskStatus.Running)
            {
                e.Cancel = true;
            }
        }

        private void InitOrUptateDatabase()
        {
            UpdateCurrentStep("Database initialisation");
            UpdateCurrentProgress(0, 0);

            UpdateCurrentStatus("Verifying database...");
            if (!SettingsManager.HasInitialized)
                SettingsManager.Initialize();

            string databasePath = SettingsManager.GetFilePath(SettingsManager.DATABASE_FILENAME);
            if (!SettingsManager.DatabaseExists())
            {
                UpdateCurrentStatus("Creating database...");
                string templateDbPath = Path.GetFullPath("Resources\\EmptyDatabase.db");
                File.Copy(templateDbPath, databasePath);
            }

            UpdateCurrentStatus("Verifying LDD installation...");
            if (!LDDEnvironment.HasInitialized)
                LDDEnvironment.Initialize();

            if (LDDEnvironment.Current == null)
            {
                MessageBox.Show("Could not locate LDD installation.");
                CTS.Cancel();
                OnDbInitializationComplete();
                return;
            }

            try
            {
                DatabaseInitializer.InitializeOrUpdateDatabase(databasePath,
                    DatabaseInitializer.InitializationStep.RebrickableLddMappings, CTS.Token, this);
            }
            catch (Exception ex)
            {
                CTS.Cancel();
            }

            OnDbInitializationComplete();
        }

        private void UpdateCurrentStep(string stepName)
        {
            if (InvokeRequired)
                Invoke((Action)(() => UpdateCurrentStep(stepName)));
            else
                InitializationStepLabel.Text = $"{stepName} ({CurrentStep}/{TotalSteps})";
        }

        private void UpdateCurrentStatus(string statusText)
        {
            if (InvokeRequired)
                Invoke((Action)(() => UpdateCurrentStatus(statusText)));
            else
                ProgressStatusLabel.Text = statusText;
        }

        private void UpdateCurrentProgress(int currentRecord, int totalRecords)
        {
            if (InvokeRequired)
                Invoke((Action)(() => UpdateCurrentProgress(currentRecord, totalRecords)));
            else
            {
                if (totalRecords <= 0)
                {
                    progressBar1.Style = ProgressBarStyle.Marquee;
                    label2.Visible = false;
                }
                else
                {
                    if (progressBar1.Style != ProgressBarStyle.Continuous)
                        progressBar1.Style = ProgressBarStyle.Continuous;

                    if (totalRecords != progressBar1.Maximum)
                    {
                        progressBar1.Value = 0;
                        progressBar1.Maximum = totalRecords;
                        label2.Visible = true;
                    }

                    progressBar1.Value = Math.Min(currentRecord, totalRecords);

                    float progressPercent = currentRecord / (float)totalRecords;
                    label2.Text = $"Progress: {progressPercent:0.00%}";
                }
            }
        }

        private void OnDbInitializationComplete()
        {
            if (InvokeRequired)
                BeginInvoke(new MethodInvoker(OnDbInitializationComplete));
            else
            {
                ProgressReportTimer.Stop();
                UpdateCurrentProgress(1, 1);
                ProgressStatusLabel.Visible = false;
                label2.Visible = false;
                if (CTS.IsCancellationRequested)
                    InitializationStepLabel.Text = "Intialization cancelled";
                else
                    InitializationStepLabel.Text = "Intialization complete";
            }
        }

        public void OnInitImportTask(int totalSteps)
        {
            TotalSteps = totalSteps;
            CurrentStep = 0;
        }

        public void OnBeginStep(string stepName)
        {
            CurrentStep++;
            if (CurrentProgressTotal > 0)
                UpdateCurrentProgress(CurrentProgress, CurrentProgressTotal);
            UpdateCurrentStep(stepName);
            UpdateCurrentStatus(string.Empty);
            CurrentProgress = 0;
            CurrentProgressTotal = 0;
        }

        public void OnReportIndefiniteProgress()
        {
            if (CurrentProgressTotal > 0)
                UpdateCurrentProgress(CurrentProgress, CurrentProgressTotal);
            UpdateCurrentProgress(0, 0);
            CurrentProgress = 0;
            CurrentProgressTotal = 0;
        }

        private int CurrentProgress;
        private int CurrentProgressTotal;
        private object ProgressLock = new object();

        public void OnReportProgress(int currentRecord, int totalRecords)
        {
            lock (ProgressLock)
            {
                CurrentProgress = currentRecord;
                CurrentProgressTotal = totalRecords;
            }

            if (CurrentProgress == CurrentProgressTotal)//force refresh when finished
                UpdateCurrentProgress(CurrentProgress, CurrentProgressTotal);
        }

        public void OnReportProgressStatus(string statusText)
        {
            UpdateCurrentStatus(statusText);
        }

        private void ProgressReportTimer_Tick(object sender, EventArgs e)
        {
            lock (ProgressLock)
            {
                if (CurrentProgressTotal > 0)
                    UpdateCurrentProgress(CurrentProgress, CurrentProgressTotal);
            }
        }

        private void CancelCloseButton_Click(object sender, EventArgs e)
        {
            if (InitUpdateTask != null && InitUpdateTask.Status == TaskStatus.Running)
            {
                CTS.Cancel();
            }
            else
            {
                Close();
            }
        }
    }
}
