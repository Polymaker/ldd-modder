using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LDDModder.BrickInstaller
{
    internal static class ProgressLogger
    {
        internal static IProgressLogOutput CurrentOutput;

        internal static IWin32Window OutputWindow
        {
            get
            {
                if (CurrentOutput == null)
                    return null;
                if (CurrentOutput is Control)
                    return (CurrentOutput as Control).FindForm();
                if (CurrentOutput is IWin32Window)
                    return (IWin32Window)CurrentOutput;
                return null;
            }
        }

        public enum LogType
        {
            Normal,
            Warning,
            Error,
            Info
        }

        internal static void LogProgress(string text, ProgressLogger.LogType logType = LogType.Normal)
        {
            if (CurrentOutput == null)
                return;
            CurrentOutput.LogProgress(text, logType);
        }

        internal static void UpdateStatus(string text, bool autoLog = true)
        {
            if (CurrentOutput == null)
                return;
            CurrentOutput.UpdateStatus(text);
            CurrentOutput.LogProgress(text, LogType.Normal);
        }

        internal static void SetProgress(int min, int max)
        {
            if (CurrentOutput == null)
                return;
            CurrentOutput.SetProgress(min, max);
        }

        internal static void UpdateProgress(int value)
        {
            if (CurrentOutput == null)
                return;
            CurrentOutput.UpdateProgress(value);
        }

        internal static DialogResult ShowMessageBox(string text, string caption, MessageBoxButtons buttons = MessageBoxButtons.OK)
        {
            if (OutputWindow != null && OutputWindow is Form)
            {
                var form = (Form)OutputWindow;

                return (DialogResult)form.Invoke((Func<DialogResult>)(() =>
                {
                    return MessageBox.Show(form, text, caption, buttons);
                }));
            }
            return MessageBox.Show(text, caption, buttons);
        }

        internal static DialogResult ShowDialog(CommonDialog dialog)
        {
            Form myform = OutputWindow as Form;
            if (myform == null && Application.OpenForms.Count > 0)
                myform = Application.OpenForms[0];

            if (myform != null)
            {
                return (DialogResult)myform.Invoke((Func<DialogResult>)(() =>
                {
                    return dialog.ShowDialog();
                }));
            }
            
            return dialog.ShowDialog();
        }
    }
}
