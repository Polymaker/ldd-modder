using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LDDModder.BrickInstaller
{
    internal interface IProgressLogOutput
    {
        void LogProgress(string text, ProgressLogger.LogType type);
        void UpdateStatus(string text);
        void SetProgress(int minValue, int maxValue);
        void UpdateProgress(int value);
    }
}
