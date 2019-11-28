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

        public DbInitializerModule(SQLiteConnection connection, LDDEnvironment environment, CancellationToken cancellationToken)
        {
            Connection = connection;
            Environment = environment;
            CancellationToken = cancellationToken;
        }

        protected void NotifyIndefiniteProgress(string status)
        {
            
        }

        protected void NotifyTaskStart(long totalRecords, string status)
        {

        }
    }
}
