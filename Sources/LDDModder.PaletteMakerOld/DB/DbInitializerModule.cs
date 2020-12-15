using LDDModder.LDD;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LDDModder.PaletteMaker.DB
{
    public class DbInitializerModule
    {
        public SQLiteConnection Connection { get; set; }

        public LDDEnvironment Environment { get; set; }

        public CancellationToken CancellationToken { get; set; }

        public bool IsCancellationRequested => CancellationToken.IsCancellationRequested;

        public IDbInitProgressHandler ProgressHandler { get; set; }


        public DbInitializerModule(SQLiteConnection connection, LDDEnvironment environment, CancellationToken cancellationToken)
        {
            Connection = connection;
            Environment = environment;
            CancellationToken = cancellationToken;
        }

        protected void NotifyIndefiniteProgress(string statusText)
        {
            ProgressHandler?.OnReportIndefiniteProgress();
            ProgressHandler?.OnReportProgressStatus(statusText);
        }

        protected void NotifyIndefiniteProgress()
        {
            ProgressHandler?.OnReportIndefiniteProgress();
        }

        protected void NotifyProgressStatus(string statusText)
        {
            ProgressHandler?.OnReportProgressStatus(statusText);
        }

        protected void NotifyBeginStep(string stepName)
        {
            ProgressHandler?.OnBeginStep(stepName);
        }

        protected void NotifyTaskStart(string statusText, int totalRecords)
        {
            ProgressHandler?.OnReportProgressStatus(statusText);
            ProgressHandler?.OnReportProgress(0, totalRecords);
        }

        protected void ReportProgress(int currentRecord, int totalRecords)
        {
            ProgressHandler?.OnReportProgress(currentRecord, totalRecords);
        }
    }
}
