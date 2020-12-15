using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.PaletteMaker.DB
{
    public interface IDbInitProgressHandler
    {
        //void OnInitImportTask(int totalSteps);

        //void OnBeginStep(string stepName);

        //void OnReportIndefiniteProgress();

        void ReportProgressStep(string description);
        void ReportProgressDetails(string description);
        void ReportProgress(int currentRecord, int totalRecords);
        void LogMessage(string message);
        //void ReportIndefiniteProgress() => ReportProgress(-1, -1);

    }
}
