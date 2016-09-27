using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LDDModder.Utilities.Logging
{
    public interface IOperationLogger
    {
        void LogOperation(string info);
        void LogDetails(string text, LogType type);
        void SetProgressMinMax(int min, int max);
        void SetProgressValue(int value);
    }
}
