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
            ProgressHandler?.ReportProgress(-1, -1);
            ProgressHandler?.ReportProgressStep(statusText);
        }

        protected void NotifyIndefiniteProgress()
        {
            ProgressHandler?.ReportProgress(-1,-1);
        }

        protected void NotifyProgressStatus(string statusText)
        {
            ProgressHandler?.ReportProgressDetails(statusText);
        }

        protected void NotifyBeginStep(string stepName)
        {
            ProgressHandler?.ReportProgressStep(stepName);
        }

        protected void NotifyTaskStart(string statusText, int totalRecords)
        {
            ProgressHandler?.ReportProgressStep(statusText);
            ProgressHandler?.ReportProgress(0, totalRecords);
        }

        protected void ReportProgress(int currentRecord, int totalRecords)
        {
            ProgressHandler?.ReportProgress(currentRecord, totalRecords);
        }
    }
}
