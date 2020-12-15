using LDDModder.LDD;
using LDDModder.PaletteMaker.DB;
using LDDModder.PaletteMaker.Settings;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LDDModder.PaletteMaker.Views
{
    /// <summary>
    /// Interaction logic for DbInitializaonWindow.xaml
    /// </summary>
    public partial class DbInitializaonWindow : Window, IDbInitProgressHandler
    {
        public bool IsDatabaseEmpty { get; set; }
        private CancellationTokenSource CTS;
        private Task InitUpdateTask;
        private Timer RefreshTimer;

        public DbInitializaonWindow()
        {
            InitializeComponent();
            RefreshTimer = new Timer(RefreshTimer_OnTick);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MaxWidth = ActualWidth;
            ProgressStepLabel.Visibility = Visibility.Collapsed;
            ProgressInfoLabel.Visibility = Visibility.Collapsed;
            InitProgressBar.Visibility = Visibility.Collapsed;
            ProgressPercentLabel.Visibility = Visibility.Collapsed;
    
            if (!AppSettings.HasInitialized)
                AppSettings.Initialize();

            if (!AppSettings.DatabaseExists())
                IsDatabaseEmpty = true;

            if (IsDatabaseEmpty)
            {
                ColorCheckBox.IsChecked = true;
                ColorCheckBox.IsEnabled = false;
                ThemesCheckBox.IsChecked = true;
                ThemesCheckBox.IsEnabled = false;
                CategoriesCheckBox.IsChecked = true;
                CategoriesCheckBox.IsEnabled = false;
                RbPartsCheckBox.IsChecked = true;
                RbPartsCheckBox.IsEnabled = false;
                PartRelationsCheckBox.IsChecked = true;
                PartRelationsCheckBox.IsEnabled = false;

                LddPartsCheckBox.IsChecked = true;
                LddElementsCheckBox.IsChecked = true;
                LddGroupBox.IsEnabled = false;

                PartMappingCheckBox.IsChecked = true;
                PartMappingCheckBox.IsEnabled = false;
                
            }
            //UpdateLayout();
            //MinHeight = ActualHeight;
        }


        #region Progress Reporting

        private void UpdateProgressStep(string stepName)
        {
            if (Dispatcher.Thread == Thread.CurrentThread)
            {
                ProgressStepLabel.Content = stepName;
            }
            else
            {
                Dispatcher.Invoke(() => UpdateProgressStep(stepName));
            }
        }

        private void UpdateProgressStatus(string statusText)
        {
            if (Dispatcher.Thread == Thread.CurrentThread)
            {
                ProgressInfoLabel.Content = statusText;
            }
            else
            {
                Dispatcher.Invoke(() => UpdateProgressStatus(statusText));
            }
        }

        private void UpdateCurrentProgress(int currentRecord, int totalRecords)
        {
            if (Dispatcher.Thread == Thread.CurrentThread)
            {
                if (totalRecords <= 0)
                {
                    InitProgressBar.IsIndeterminate = true;
                    ProgressPercentLabel.Visibility = Visibility.Hidden;
                }
                else
                {
                    InitProgressBar.IsIndeterminate = false;

                    if (totalRecords != InitProgressBar.Maximum)
                    {
                        InitProgressBar.Value = 0;
                        InitProgressBar.Maximum = totalRecords;
                        ProgressPercentLabel.Visibility = Visibility.Visible;
                    }

                    InitProgressBar.Value = Math.Min(currentRecord, totalRecords);

                    float progressPercent = currentRecord / (float)totalRecords;
                    ProgressPercentLabel.Content = $"{progressPercent:0.00%}";
                }
            }
            else
            {
                Dispatcher.Invoke(() => UpdateCurrentProgress(currentRecord, totalRecords));
            }
            
        }

        private void RefreshTimer_OnTick(object state)
        {
            lock (ProgressLock)
            {
                if (CurrentProgressTotal > 0)
                    UpdateCurrentProgress(CurrentProgress, CurrentProgressTotal);
            }
        }

        private int CurrentProgress;
        private int CurrentProgressTotal;
        private object ProgressLock = new object();

        //public void OnInitImportTask(int totalSteps)
        //{
        //    TotalSteps = totalSteps;
        //    CurrentStep = 0;
        //}

        //public void OnBeginStep(string stepName)
        //{
        //    CurrentStep++;
        //    if (CurrentProgressTotal > 0)
        //        UpdateCurrentProgress(CurrentProgress, CurrentProgressTotal);
        //    UpdateProgressStep(stepName);
        //    UpdateProgressStatus(string.Empty);
        //    CurrentProgress = 0;
        //    CurrentProgressTotal = 0;
        //}

        //public void OnReportIndefiniteProgress()
        //{
        //    if (CurrentProgressTotal > 0)
        //        UpdateCurrentProgress(CurrentProgress, CurrentProgressTotal);
        //    UpdateCurrentProgress(0, 0);
        //    CurrentProgress = 0;
        //    CurrentProgressTotal = 0;
        //}


        void IDbInitProgressHandler.ReportProgressStep(string description)
        {
            UpdateProgressStep(description);
        }

        void IDbInitProgressHandler.ReportProgressDetails(string description)
        {
            UpdateProgressStatus(description);
        }

        void IDbInitProgressHandler.LogMessage(string message)
        {
            
        }

        void IDbInitProgressHandler.ReportProgress(int currentRecord, int totalRecords)
        {
            if (currentRecord< 0)
            {
                if (CurrentProgressTotal > 0)
                    UpdateCurrentProgress(CurrentProgress, CurrentProgressTotal);
                UpdateCurrentProgress(0, 0);
                CurrentProgress = 0;
                CurrentProgressTotal = 0;
            }
            else
            {
                lock (ProgressLock)
                {
                    CurrentProgress = currentRecord;
                    CurrentProgressTotal = totalRecords;
                }

                if (CurrentProgress == CurrentProgressTotal)//force refresh when finished
                    UpdateCurrentProgress(CurrentProgress, CurrentProgressTotal);
            }
        }

        #endregion

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            

           

            BeginDatabaseInitialization();

            
            //InitUpdateTask = Task.Factory.StartNew(InitOrUptateDatabase);
        }


        private bool PerformPreValidation()
        {
            UpdateProgressStep("Database initialisation");
            UpdateProgressStatus("Verifying database...");

            if (!AppSettings.HasInitialized)
                AppSettings.Initialize();

            string databasePath = AppSettings.GetFilePath(AppSettings.DATABASE_FILENAME);

            if (!AppSettings.DatabaseExists())
            {
                UpdateProgressStatus("Creating database...");
                string templateDbPath = System.IO.Path.GetFullPath("Resources\\EmptyDatabase.db");
                try
                {
                    System.IO.File.Copy(templateDbPath, databasePath);
                }
                catch {
                    return false;
                }
            }

            UpdateProgressStatus("Verifying LDD installation...");
            if (!LDDEnvironment.HasInitialized)
                LDDEnvironment.Initialize();

            if (!LDDEnvironment.IsInstalled)
            {
                Dispatcher.Invoke(() =>
                {
                    MessageBox.Show("Could not locate LDD installation.");
                });
                return false;
            }

            UpdateProgressStatus("LDD installation found");

            return true;
        }

        private void OnPreValidationFailed()
        {
            CancelButton.IsEnabled = true;
        }

        private void PerformDatabaseInitialization()
        {
            UpdateProgressStep("Importing data");
            UpdateProgressStatus(string.Empty);

            string databasePath = AppSettings.GetFilePath(AppSettings.DATABASE_FILENAME);

            using (var conn = new SQLiteConnection($"Data Source={databasePath}"))
            {
                conn.Open();

                var rbImporter = new RebrickableDataImporter(conn, LDDEnvironment.Current, CTS.Token);
                rbImporter.DownloadDirectory = AppSettings.GetFilePath("Downloads");
                rbImporter.ProgressHandler = this;

                var lddImporter = new LddDataImporter(conn, LDDEnvironment.Current, CTS.Token);
                lddImporter.ProgressHandler = this;

                var importActions = new List<Action>();

                Dispatcher.Invoke(() =>
                {
                    if (ColorCheckBox.IsChecked ?? false)
                        importActions.Add(() => rbImporter.ImportColors());
                    if (CategoriesCheckBox.IsChecked ?? false)
                        importActions.Add(() => rbImporter.ImportCategories());
                    if (ThemesCheckBox.IsChecked ?? false)
                        importActions.Add(() => rbImporter.ImportThemes());

                    if (RbPartsCheckBox.IsChecked ?? false)
                        importActions.Add(() => rbImporter.ImportRebrickableParts());
                    if (PartRelationsCheckBox.IsChecked ?? false)
                        importActions.Add(() => rbImporter.ImportRebrickableRelationships());

                    if (LddPartsCheckBox.IsChecked ?? false)
                        importActions.Add(() => lddImporter.ImportLddParts());
                    if (LddElementsCheckBox.IsChecked ?? false)
                        importActions.Add(() => lddImporter.ImportLddElements());
                    if (PartMappingCheckBox.IsChecked ?? false)
                        importActions.Add(() => DatabaseInitializer.InitializeDefaultMappings(databasePath, this));
                });

                foreach (var importAction in importActions)
                {
                    try
                    {
                        UpdateProgressStatus(string.Empty);
                        importAction();
                    }
                    catch
                    {
                        CTS.Cancel();
                        break;
                    }
                }
            }
        }

        private void BeginDatabaseInitialization()
        {
            StartButton.Visibility = Visibility.Hidden;
            CancelButton.IsEnabled = false;

            RebrickableGroupBox.IsEnabled = false;
            LddGroupBox.IsEnabled = false;
            MiscGroupBox.IsEnabled = false;

            ProgressStepLabel.Visibility = Visibility.Visible;
            ProgressInfoLabel.Visibility = Visibility.Visible;
            InitProgressBar.Visibility = Visibility.Visible;
            //ProgressPercentLabel.Visibility = Visibility.Visible;

            InitProgressBar.IsIndeterminate = true;

            CTS = new CancellationTokenSource();
            RefreshTimer.Change(0, 100);
            InitUpdateTask = Task.Factory.StartNew(() =>
            {
                if (PerformPreValidation())
                {
                    PerformDatabaseInitialization();
                    Dispatcher.Invoke(OnInitializationFinished);
                }
                else
                    Dispatcher.Invoke(OnPreValidationFailed);

                RefreshTimer.Change(Timeout.Infinite, Timeout.Infinite);
            });

            //var importActions = new List<Action>();
            //
            //InitProgressBar.IsIndeterminate = false;

            //try
            //{
            //    using (var conn = new SQLiteConnection($"Data Source={databasePath}"))
            //    {
            //        conn.Open();

            //        var rbImporter = new RebrickableDataImporter(conn, LDDEnvironment.Current, CTS.Token);
            //        rbImporter.DownloadDirectory = AppSettings.GetFilePath("Downloads");
            //        rbImporter.ProgressHandler = this;

            //        var lddImporter = new LddDataImporter(conn, LDDEnvironment.Current, CTS.Token);
            //        lddImporter.ProgressHandler = this;

            //        if (ColorCheckBox.IsChecked ?? false)
            //            importActions.Add(() => rbImporter.ImportColors());
            //        if (CategoriesCheckBox.IsChecked ?? false)
            //            importActions.Add(() => rbImporter.ImportCategories());
            //        if (ThemesCheckBox.IsChecked ?? false)
            //            importActions.Add(() => rbImporter.ImportThemes());

            //        if (RbPartsCheckBox.IsChecked ?? false)
            //            importActions.Add(() => rbImporter.ImportRebrickableParts());
            //        if (PartRelationsCheckBox.IsChecked ?? false)
            //            importActions.Add(() => rbImporter.ImportRebrickableRelationships());

            //        if (LddPartsCheckBox.IsChecked ?? false)
            //            importActions.Add(() => lddImporter.ImportLddParts());
            //        if (LddElementsCheckBox.IsChecked ?? false)
            //            importActions.Add(() => lddImporter.ImportLddElements());
            //        if (PartMappingCheckBox.IsChecked ?? false)
            //            importActions.Add(() => DatabaseInitializer.InitializeDefaultMappings(databasePath, this));

            //        InitUpdateTask = Task.Factory.StartNew(() =>
            //        {
            //            foreach (var importAction in importActions)
            //            {
            //                try
            //                {
            //                    importAction();
            //                }
            //                catch {
            //                    CTS.Cancel();
            //                    break;
            //                }
            //            }
            //        });
            //    }
            //}
            //catch (Exception ex)
            //{
            //    //CTS.Cancel();
            //}

            //OnDbInitializationComplete();
        }

        private void OnInitializationFinished()
        {
            UpdateCurrentProgress(100, 100);
            UpdateProgressStatus(string.Empty);
            UpdateProgressStep("Importation finished");
            CancelButton.IsEnabled = true;
        }

        private bool AbortUpdatingDb()
        {
            if (InitUpdateTask?.Status == TaskStatus.Running)
            {
                //todo : ask confirmation

                CTS.Cancel();
                InitUpdateTask.Wait();

            }

            return true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (InitUpdateTask?.Status == TaskStatus.Running)
            {
                CTS.Cancel();
                InitUpdateTask.Wait();
                Close();
            }
            else
            {
                Close();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (InitUpdateTask?.Status == TaskStatus.Running)
            {
                if (!AbortUpdatingDb())
                    e.Cancel = true;
            }
        }
    }
}
