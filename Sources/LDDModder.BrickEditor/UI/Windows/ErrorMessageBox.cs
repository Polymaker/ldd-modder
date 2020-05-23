using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LDDModder.BrickEditor.UI.Windows
{
    public partial class ErrorMessageBox : Form
    {
        public ErrorMessageBox()
        {
            InitializeComponent();
        }

        private void SetDialogButtons(MessageBoxButtons buttons)
        {
            Option1Button.Visible = false;
            Option2Button.Visible = false;
            Option3Button.Visible = false;
            switch (buttons)
            {
                case MessageBoxButtons.OK:
                    Option1Button.Visible = true;
                    Option1Button.Text = "OK";
                    Option1Button.DialogResult = DialogResult.OK;
                    flowLayoutPanel1.Anchor = AnchorStyles.Top;
                    break;
                case MessageBoxButtons.OKCancel:
                    Option1Button.Visible = true;
                    Option1Button.Text = "OK";
                    Option1Button.DialogResult = DialogResult.OK;

                    Option2Button.Visible = true;
                    Option2Button.Text = "Cancel";
                    Option2Button.DialogResult = DialogResult.Cancel;

                    flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                    break;
                case MessageBoxButtons.AbortRetryIgnore:
                    Option1Button.Visible = true;
                    Option1Button.Text = "Abort";
                    Option1Button.DialogResult = DialogResult.Abort;

                    Option2Button.Visible = true;
                    Option2Button.Text = "Retry";
                    Option2Button.DialogResult = DialogResult.Retry;

                    Option3Button.Visible = true;
                    Option3Button.Text = "Ignore";
                    Option3Button.DialogResult = DialogResult.Ignore;

                    flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                    break;
                case MessageBoxButtons.YesNoCancel:
                    Option1Button.Visible = true;
                    Option1Button.Text = "Yes";
                    Option1Button.DialogResult = DialogResult.Yes;

                    Option2Button.Visible = true;
                    Option2Button.Text = "No";
                    Option2Button.DialogResult = DialogResult.No;

                    Option3Button.Visible = true;
                    Option3Button.Text = "Cancel";
                    Option3Button.DialogResult = DialogResult.Cancel;

                    flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                    break;
                case MessageBoxButtons.YesNo:
                    Option1Button.Visible = true;
                    Option1Button.Text = "Yes";
                    Option1Button.DialogResult = DialogResult.Yes;

                    Option2Button.Visible = true;
                    Option2Button.Text = "No";
                    Option2Button.DialogResult = DialogResult.No;

                    flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                    break;
                case MessageBoxButtons.RetryCancel:
                    Option1Button.Visible = true;
                    Option1Button.Text = "Retry";
                    Option1Button.DialogResult = DialogResult.Retry;

                    Option2Button.Visible = true;
                    Option2Button.Text = "Cancel";
                    Option2Button.DialogResult = DialogResult.Cancel;

                    flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                    break;
            }
        }

        private void SelectDefaultButton(MessageBoxDefaultButton defaultButton)
        {
            switch (defaultButton)
            {
                default:
                case MessageBoxDefaultButton.Button1:
                    Option1Button.Select();
                    break;
                case MessageBoxDefaultButton.Button2:
                    Option2Button.Select();
                    break;
                case MessageBoxDefaultButton.Button3:
                    Option3Button.Select();
                    break;
            }
        }

        public static DialogResult Show(IWin32Window owner, 
            string text, string caption, string errorDetails,
            MessageBoxButtons buttons, MessageBoxIcon icon, 
            MessageBoxDefaultButton defaultButton, FormStartPosition startPosition)
        {
            using (var dlg = new ErrorMessageBox())
            {
                dlg.Text = caption;
                dlg.MessageTextLabel.Text = text;
                dlg.ErrorDetailTextBox.Text = errorDetails;
                dlg.MessageIconBox.Visible = (icon != MessageBoxIcon.None);
                dlg.StartPosition = startPosition;
                dlg.SetDialogButtons(buttons);
                dlg.SelectDefaultButton(defaultButton);
                return dlg.ShowDialog(owner);
            }
        }

        public static DialogResult Show(IWin32Window owner,
            string text, string caption, string errorDetails,
            MessageBoxButtons buttons)
        {
            return Show(owner, text, caption, errorDetails, buttons, 
                MessageBoxIcon.None, MessageBoxDefaultButton.Button1, FormStartPosition.CenterParent);
        }

        public static DialogResult Show(IWin32Window owner,
            string text, string caption, string errorDetails)
        {
            return Show(owner, text, caption, errorDetails, MessageBoxButtons.OK,
                MessageBoxIcon.None, MessageBoxDefaultButton.Button1, FormStartPosition.CenterParent);
        }
    }
}
