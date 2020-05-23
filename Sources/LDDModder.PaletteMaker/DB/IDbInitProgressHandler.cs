using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.PaletteMaker.DB
{
    public interface IDbInitProgressHandler
    {
        void OnBeginStep(string stepName);

        void OnReportIndefiniteProgress();

        void OnReportProgress(int currentRecord, int totalRecords);

        void OnReportProgressStatus(string statusText);
    }
}
