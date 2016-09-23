using LDDModder.LDD;
using LDDModder.Modding;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LDDModder.BrickInstaller
{
    public partial class FrmPackageInstaller : Form, IProgressLogOutput
    {
        private int PreviousLogHeight = 0;

        public FrmPackageInstaller()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            InitializeLayout();
            
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            ProgressLogger.CurrentOutput = this;
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(250);
                if (!InstallHelper.ValidateLddInstall())
                    Invoke(new MethodInvoker(ValidationFailed));
            });
            //FormExtensions.DelayInvoke(this, 250, () => PackageInstaller.ValidateLddInstall());
            
        }

        private void ValidationFailed()
        {
            ToggleInstallLog(true);
            btnShowDetails.Visible = false;
            btnAction.Text = LOC_Exit;
            btnAction.Visible = true;
            btnAction.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void InitializeLayout()
        {
            //btnAction.Visible = btnShowDetails.Visible = false;
            btnAction.Visible = false;
            //ToggleInstallLog(false);
        }

        private void AdjustFormSize()
        {
            int verticalFormPadding = Height - ClientSize.Height;
            MaximumSize = Size.Empty;
            MinimumSize = Size.Empty;

            if (tlpLayout.AutoSize && tlpLayout.AutoSizeMode == System.Windows.Forms.AutoSizeMode.GrowAndShrink)
            {
                var calcHeight = tlpLayout.GetPreferredSize(new Size(500, 500));
                int newHeight = calcHeight.Height + verticalFormPadding;
                MinimumSize = new Size(0, newHeight);
                Height = newHeight;
                MaximumSize = new Size(500, newHeight);
            }
            else
            {
                int minHeight = rtbInstallLog.Top + 40 + rtbInstallLog.Margin.Bottom;
                MinimumSize = new Size(0, minHeight + verticalFormPadding);
            }
        }

        public void SetPackages(string[] filepaths)
        {

        }

        #region IProgressLogOutput

        void IProgressLogOutput.LogProgress(string text, ProgressLogger.LogType logType)
        {
            LogProgress(text, logType);
        }

        void IProgressLogOutput.UpdateStatus(string text)
        {
            UpdateStatus(text);
        }

        void IProgressLogOutput.SetProgress(int min, int max)
        {
            SetProgress(min, max);
        }

        void IProgressLogOutput.UpdateProgress(int value)
        {
            UpdateProgress(value);
        }

        private void LogProgress(string text, ProgressLogger.LogType logType)
        {
            if (rtbInstallLog.InvokeRequired)
            {
                Invoke((Action)(() => LogProgress(text, logType)));
                return;
            }

            var currentColor = rtbInstallLog.SelectionColor;

            switch (logType)
            {
                case ProgressLogger.LogType.Error:
                    {
                        rtbInstallLog.SelectionColor = Color.Red;
                        rtbInstallLog.AppendText(LocalizedStrings.LogError);
                        rtbInstallLog.SelectionColor = currentColor;
                        rtbInstallLog.AppendText(" ");
                        break;
                    }
                case ProgressLogger.LogType.Warning:
                    {
                        rtbInstallLog.SelectionColor = Color.Orange;
                        rtbInstallLog.AppendText(LocalizedStrings.LogWarning);
                        rtbInstallLog.SelectionColor = currentColor;
                        rtbInstallLog.AppendText(" ");
                        break;
                    }
                case ProgressLogger.LogType.Info:
                    {
                        rtbInstallLog.SelectionColor = Color.DodgerBlue;
                        rtbInstallLog.AppendText(LocalizedStrings.LogInfo);
                        rtbInstallLog.SelectionColor = currentColor;
                        rtbInstallLog.AppendText(" ");
                        break;
                    }
            }
            rtbInstallLog.AppendText(text + Environment.NewLine);
            rtbInstallLog.SelectionStart = rtbInstallLog.TextLength;
            //BeginInvoke((Action)(() => rtbInstallLog.ScrollToCaret()));
            rtbInstallLog.ScrollToCaret();
        }
        
        private void UpdateStatus(string text)
        {
            if (lblOperationInfo.InvokeRequired)
            {
                BeginInvoke((Action)(() => UpdateStatus(text)));
                return;
            }
            lblOperationInfo.Text = text;
        }

        private void SetProgress(int min, int max)
        {
            if (progBar.InvokeRequired)
            {
                BeginInvoke((Action)(() => SetProgress(min, max)));
                return;
            }
            if (max == 0)
            {
                progBar.Style = ProgressBarStyle.Marquee;
            }
            else if (max == -1)
            {
                progBar.Visible = false;
                AdjustFormSize();
                return;
            }
            else
            {
                progBar.Style = ProgressBarStyle.Continuous;
                progBar.Maximum = max;
                progBar.Minimum = min;
            }
            if (!progBar.Visible)
            {
                progBar.Visible = true;
                AdjustFormSize();
            }
        }

        private void UpdateProgress(int value)
        {
            if (progBar.InvokeRequired)
            {
                BeginInvoke((Action)(() => UpdateProgress(value)));
                return;
            }
            progBar.Value = Math.Max(progBar.Minimum, Math.Min(value, progBar.Maximum));
        }

        #endregion

        private void btnShowDetails_Click(object sender, EventArgs e)
        {
            ToggleInstallLog(!rtbInstallLog.Visible);
            //var brickPack = BrickPackage.OpenOrCreate("Wheel Hub.cbp");
            //brickPack.AddBrick("32496.xml");
            //brickPack.AddModel("32496.g");
            //brickPack.Close();
            //PackageInstaller.InstallPackage(brickPack);
        }


        private void ToggleInstallLog(bool value)
        {
            if (value == rtbInstallLog.Visible)
                return;

            if (value)
            {
                tlpLayout.RowStyles[tlpLayout.RowCount - 1].SizeType = SizeType.Percent;
                tlpLayout.Dock = DockStyle.Fill;
                tlpLayout.AutoSize = false;
                tlpLayout.AutoSizeMode = AutoSizeMode.GrowOnly;
                rtbInstallLog.Visible = true;
                btnShowDetails.Text = LOC_HideDetails;
            }
            else
            {
                PreviousLogHeight = rtbInstallLog.Height;
                tlpLayout.RowStyles[tlpLayout.RowCount - 1].SizeType = SizeType.AutoSize;
                tlpLayout.Dock = DockStyle.Top;
                tlpLayout.AutoSize = true;
                tlpLayout.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                rtbInstallLog.Visible = false;
                btnShowDetails.Text = LOC_ShowDetails;
            }
            AdjustFormSize();
            if (value && rtbInstallLog.Height != PreviousLogHeight)
            {
                Height += PreviousLogHeight - rtbInstallLog.Height;
            }
        }

        private void AdjustOutputElements(object sender, EventArgs e)
        {
            //foreach (Control ctrl in rtbInstallLog.Controls)
            //{
            //    if (ctrl.Tag != null)
            //        ctrl.Location = rtbInstallLog.GetPositionFromCharIndex((int)ctrl.Tag);
            //}
        }

        private void rtbInstallLog_TextChanged(object sender, EventArgs e)
        {
            //DetectLogPaths();
        }


        private static readonly Regex PathParsingRegex = new Regex("(?:[a-zA-Z]\\:(\\\\|\\/)|file\\:\\/\\/|\\\\\\\\|\\.(\\/|\\\\))([^\\\\\\/\\:\\*\\?\\<\\>\\\"\\|\\r\\n]+(\\\\|\\/){0,1})+", RegexOptions.Compiled | RegexOptions.CultureInvariant);

        private void DetectLogPaths()
        {
            var matches = PathParsingRegex.Matches(rtbInstallLog.Text);

            var currentLinks = rtbInstallLog.Controls.OfType<LinkLabel>().ToList();
            int currentSelectionPos = rtbInstallLog.SelectionStart;
            int currentSelectionLen  = rtbInstallLog.SelectionLength;

            if (matches.Count > 0)
            {
                foreach (Match match in matches)
                {
                    var linkCtrl = currentLinks.FirstOrDefault(l => match.Index.Equals(l.Tag));

                    if (linkCtrl != null)
                        currentLinks.Remove(linkCtrl);
                    else
                    {
                        linkCtrl = new LinkLabel()
                        {
                            Tag = match.Index,
                            Location = rtbInstallLog.GetPositionFromCharIndex(match.Index),
                            AutoSize = true
                        };
                        linkCtrl.LinkClicked += OutputLog_LinkClicked;
                    }
                    rtbInstallLog.SelectionStart = match.Index;
                    rtbInstallLog.SelectionLength = 1;
                    linkCtrl.Font = rtbInstallLog.SelectionFont;
                    linkCtrl.Text = match.Value;
                    linkCtrl.Links.Clear();
                    linkCtrl.Links.Add(new LinkLabel.Link() { LinkData = match.Value, Name = "PATH" });
                    rtbInstallLog.Controls.Add(linkCtrl);
                }
            }

            foreach (var oldLink in currentLinks)
            {
                rtbInstallLog.Controls.Remove(oldLink);
                oldLink.Dispose();
            }

            rtbInstallLog.SelectionStart = currentSelectionPos;
            rtbInstallLog.SelectionLength = currentSelectionLen;
        }

        private void OutputLog_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (e.Link.Name == "PATH")
            {
                string linkPath = (string)e.Link.LinkData;
                if(Directory.Exists(linkPath))
                    Process.Start("explorer.exe", linkPath);
            }
        }

        private void AppendLogText(string text)
        {
            var matches = PathParsingRegex.Matches(text);
            int currentCharIdx = rtbInstallLog.TextLength;
            rtbInstallLog.AppendText(text + Environment.NewLine);
            if (matches.Count > 0)
            {

                foreach (Match match in matches)
                {
                    var pathLink = new LinkLabel()
                    {
                        Text = match.Value,
                        Tag = currentCharIdx + match.Index,
                        Location = rtbInstallLog.GetPositionFromCharIndex(currentCharIdx + match.Index),
                        AutoSize = true
                    };
                    pathLink.Links.Add(new LinkLabel.Link() { LinkData = match.Value });

                    rtbInstallLog.Controls.Add(pathLink);
                }
            }

        }
    }
}
