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
    public partial class MessageBoxEX : Form
    {
        public MessageBoxEX()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!ErrorDetailTextBox.Visible)
            {
                tableLayoutPanel1.RowStyles[0].Height = 100;
                tableLayoutPanel1.RowStyles[0].SizeType = SizeType.Percent;
                ErrorDetailTextBox.Visible = false;
                tableLayoutPanel1.RowStyles[1].Height = 0;
                tableLayoutPanel1.RowStyles[1].SizeType = SizeType.Absolute;

                var prefSize = MessageTextLabel.GetPreferredSize(new Size(Width, 99999));

                int MinHeight = Math.Max(prefSize.Height, MessageIconBox.Height + MessageIconBox.Margin.Vertical);
                MinHeight = Math.Max(MinHeight, 100);
                MinHeight += flowLayoutPanel1.Height;
                Height = MinHeight + 6;
                MessageTextLabel.Anchor = AnchorStyles.Top;
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            int borderHeight = Height - tableLayoutPanel1.Height;
            int currentHeight = tableLayoutPanel1.Height;

            int messageWidth = tableLayoutPanel1.Width - MessageTextLabel.Margin.Horizontal;
            if (MessageIconBox.Visible)
                messageWidth -= MessageIconBox.Width + MessageIconBox.Margin.Horizontal;

            var minHeight = MessageTextLabel.GetPreferredSize(new Size(messageWidth ,9999)).Height + MessageTextLabel.Margin.Vertical;

            if (MessageIconBox.Visible)
                minHeight = Math.Max(minHeight, MessageIconBox.Height + MessageIconBox.Margin.Vertical);

            if (ErrorDetailTextBox.Visible)
                minHeight += 90 + ErrorDetailTextBox.Margin.Vertical;

            minHeight += flowLayoutPanel1.Height + flowLayoutPanel1.Margin.Vertical;

            //if (currentHeight < minHeight)
                Height = minHeight + borderHeight;
        }

        public void SetDialogButtons(MessageBoxButtons buttons, bool centered = true)
        {
            Option1Button.Visible = false;
            Option2Button.Visible = false;
            Option3Button.Visible = false;
            switch (buttons)
            {
                case MessageBoxButtons.OK:
                    Option1Button.Visible = true;
                    Option1Button.Text = Label_OK;
                    Option1Button.DialogResult = DialogResult.OK;
                    ControlBox = false;
                    break;
                case MessageBoxButtons.OKCancel:
                    Option1Button.Visible = true;
                    Option1Button.Text = Label_OK;
                    Option1Button.DialogResult = DialogResult.OK;

                    Option2Button.Visible = true;
                    Option2Button.Text = Label_Cancel;
                    Option2Button.DialogResult = DialogResult.Cancel;
                    break;
                case MessageBoxButtons.AbortRetryIgnore:
                    Option1Button.Visible = true;
                    Option1Button.Text = Label_Abort;
                    Option1Button.DialogResult = DialogResult.Abort;

                    Option2Button.Visible = true;
                    Option2Button.Text = Label_Retry;
                    Option2Button.DialogResult = DialogResult.Retry;

                    Option3Button.Visible = true;
                    Option3Button.Text = Label_Ignore;
                    Option3Button.DialogResult = DialogResult.Ignore;
                    break;
                case MessageBoxButtons.YesNoCancel:
                    Option1Button.Visible = true;
                    Option1Button.Text = Label_Yes;
                    Option1Button.DialogResult = DialogResult.Yes;

                    Option2Button.Visible = true;
                    Option2Button.Text = Label_No;
                    Option2Button.DialogResult = DialogResult.No;

                    Option3Button.Visible = true;
                    Option3Button.Text = Label_Cancel;
                    Option3Button.DialogResult = DialogResult.Cancel;
                    break;
                case MessageBoxButtons.YesNo:
                    Option1Button.Visible = true;
                    Option1Button.Text = Label_Yes;
                    Option1Button.DialogResult = DialogResult.Yes;

                    Option2Button.Visible = true;
                    Option2Button.Text = Label_No;
                    Option2Button.DialogResult = DialogResult.No;
                    ControlBox = false;
                    break;
                case MessageBoxButtons.RetryCancel:
                    Option1Button.Visible = true;
                    Option1Button.Text = Label_Retry;
                    Option1Button.DialogResult = DialogResult.Retry;

                    Option2Button.Visible = true;
                    Option2Button.Text = Label_Cancel;
                    Option2Button.DialogResult = DialogResult.Cancel;
                    break;
            }

            if (centered)
                flowLayoutPanel1.Anchor = AnchorStyles.Top;
            else
                flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        }

        public void SelectDefaultButton(MessageBoxDefaultButton defaultButton)
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

        public void SetMessageDetails(string detailText)
        {
            if (!string.IsNullOrEmpty(detailText))
            {
                ErrorDetailTextBox.Text = detailText;
                ErrorDetailTextBox.Visible = true;
            }
            else
            {
                ErrorDetailTextBox.Text = string.Empty;
                ErrorDetailTextBox.Visible = false;
            }
        }

        public void SetMessageIcon(MessageBoxIcon icon)
        {
            bool hasIcon = (icon != MessageBoxIcon.None);
            MessageIconBox.Visible = hasIcon;
            if (!hasIcon)
            {
                tableLayoutPanel1.ColumnStyles[0].SizeType = SizeType.Absolute;
                tableLayoutPanel1.ColumnStyles[0].Width = 0;
            }
            else
            {
                tableLayoutPanel1.ColumnStyles[0].SizeType = SizeType.AutoSize;
                tableLayoutPanel1.ColumnStyles[0].Width = 0;
            }

            switch (icon)
            {
                case MessageBoxIcon.Error:
                    MessageIconBox.Image = SystemIcons.Error.ToBitmap();
                    break;
                case MessageBoxIcon.Warning:
                    MessageIconBox.Image = SystemIcons.Warning.ToBitmap();
                    break;
                case MessageBoxIcon.Information:
                    MessageIconBox.Image = SystemIcons.Information.ToBitmap();
                    break;
            }
        }

        public static DialogResult Show(IWin32Window owner, 
            string text, string caption, string errorDetails,
            MessageBoxButtons buttons, MessageBoxIcon icon, 
            MessageBoxDefaultButton defaultButton, FormStartPosition startPosition)
        {
            using (var dlg = new MessageBoxEX())
            {
                dlg.Text = caption;
                dlg.MessageTextLabel.Text = text;
                dlg.StartPosition = startPosition;
                dlg.SetDialogButtons(buttons);
                dlg.SelectDefaultButton(defaultButton);
                dlg.SetMessageIcon(icon);
                dlg.SetMessageDetails(errorDetails);
                return dlg.ShowDialog(owner);
            }
        }

        public static DialogResult Show(string text, string caption, string errorDetails,
            MessageBoxButtons buttons, MessageBoxIcon icon,
            MessageBoxDefaultButton defaultButton, FormStartPosition startPosition)
        {
            return Show(null, text, caption, errorDetails, buttons, icon, defaultButton, startPosition);
        }

        #region Show Overloads

        public static DialogResult Show(IWin32Window owner,
            string text, string caption,
            MessageBoxButtons buttons, MessageBoxIcon icon,
            MessageBoxDefaultButton defaultButton, FormStartPosition startPosition)
        {
            return Show(owner, text, caption, null, buttons, icon, defaultButton, startPosition);
        }

        public static DialogResult Show(
            string text, string caption,
            MessageBoxButtons buttons, MessageBoxIcon icon,
            MessageBoxDefaultButton defaultButton, FormStartPosition startPosition)
        {
            return Show(null, text, caption, null, buttons, icon, defaultButton, startPosition);
        }

        public static DialogResult Show(IWin32Window owner,
            string text, string caption,
            MessageBoxButtons buttons, MessageBoxIcon icon = MessageBoxIcon.None)
        {
            return Show(owner, text, caption, null, buttons,
                icon, MessageBoxDefaultButton.Button1, FormStartPosition.CenterParent);
        }

        public static DialogResult Show(string text, string caption,
            MessageBoxButtons buttons, MessageBoxIcon icon = MessageBoxIcon.None)
        {
            return Show(null, text, caption, null, buttons,
                icon, MessageBoxDefaultButton.Button1, FormStartPosition.CenterParent);
        }

        #endregion

        #region ShowDetails Overloads

        public static DialogResult ShowDetails(IWin32Window owner,
            string text, string caption, string errorDetails,
            MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return Show(owner, text, caption, errorDetails, buttons,
                icon, MessageBoxDefaultButton.Button1, FormStartPosition.CenterParent);
        }

        public static DialogResult ShowDetails(IWin32Window owner,
            string text, string caption, string errorDetails,
            MessageBoxButtons buttons)
        {
            return Show(owner, text, caption, errorDetails, buttons,
                MessageBoxIcon.None, MessageBoxDefaultButton.Button1, FormStartPosition.CenterParent);
        }

        public static DialogResult ShowDetails(IWin32Window owner,
            string text, string caption, string errorDetails)
        {
            return Show(owner, text, caption, errorDetails, MessageBoxButtons.OK,
                MessageBoxIcon.None, MessageBoxDefaultButton.Button1, FormStartPosition.CenterParent);
        }

        #endregion

        #region ShowException Overloads

        public static DialogResult ShowException(IWin32Window owner,
            string text, string caption, Exception ex,
            MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return Show(owner, text, caption, ex.ToString(), buttons,
                icon, MessageBoxDefaultButton.Button1, FormStartPosition.CenterParent);
        }

        public static DialogResult ShowException(IWin32Window owner,
            string text, string caption, Exception ex,
            MessageBoxButtons buttons)
        {
            return Show(owner, text, caption, ex.ToString(), buttons,
                MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, FormStartPosition.CenterParent);
        }

        public static DialogResult ShowException(IWin32Window owner,
            string text, string caption, Exception ex)
        {
            return Show(owner, text, caption, ex.ToString(), MessageBoxButtons.OK,
                MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, FormStartPosition.CenterParent);
        }

        #endregion
    }
}
