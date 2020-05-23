using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LDDModder.LDD.Files;
using System.Globalization;

namespace LDDModder.LifExtractor.Controls
{
    public partial class ExtractProgressPanel : UserControl
    {
        public DateTime ExtractionStart { get; private set; }

        public ExtractProgressPanel()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            ResetEstimates();
        }

        private void ResetEstimates()
        {
            string timeSep = CultureInfo.CurrentUICulture.DateTimeFormat.TimeSeparator;
            ElapsedTimeValueLabel.Text = $"--{timeSep}--{timeSep}--";
            RemainingTimeValueLabel.Text = $"--{timeSep}--{timeSep}--";
            FileProgressValueLabel.Text = "0 / 0";
            ProgressPercentValueLabel.Text = "0%";
            ExtractionProgressBar.Value = 0;
            CurrentFileLabel.Text = string.Empty;
            ExtractingLabel.Visible = false;
        }

        public void BeginExtraction()
        {
            ExtractionStart = DateTime.Now;
            ExtractingLabel.Visible = true;
            ResetEstimates();
        }

        public void FinishExtraction()
        {
            CurrentFileLabel.Text = string.Empty;
            string timeSep = CultureInfo.CurrentUICulture.DateTimeFormat.TimeSeparator;
            RemainingTimeValueLabel.Text = $"--{timeSep}--{timeSep}--";

            ExtractingLabel.Visible = false;
        }

        public void UpdateProgress(LifFile.ExtractionProgress progress)
        {
            float percentage = progress.TotalBytes > 0 ?
                (progress.BytesExtracted / (float)progress.TotalBytes) * 100f : 0f;

            ExtractionProgressBar.Value = (int)percentage;
            FileProgressValueLabel.Text = $"{progress.ExtractedFiles} / {progress.TotalFiles}";
            ProgressPercentValueLabel.Text = $"{percentage:0}%";

            var timeElapsed = TimeSpan.Zero; // (DateTime.Now - ExtractionStart);
            if (ExtractionStart != default(DateTime))
                timeElapsed = (DateTime.Now - ExtractionStart);

            ElapsedTimeValueLabel.Text = timeElapsed.ToString("hh\\:mm\\:ss");

            var remainingTime = TimeSpan.Zero;

            if (progress.BytesExtracted > 0)
            {
                var avgTime = timeElapsed.TotalMilliseconds / (double)progress.BytesExtracted;
                var remainingBytes = (double)(progress.TotalBytes - progress.BytesExtracted);
                remainingTime = TimeSpan.FromMilliseconds(avgTime * remainingBytes);
            }

            RemainingTimeValueLabel.Text = remainingTime.ToString("hh\\:mm\\:ss");

            CurrentFileLabel.Text = progress.TargetPath;
        }

        public void SetCurrentStatusText(string text)
        {
            CurrentFileLabel.Text = text;
        }
    }
}
