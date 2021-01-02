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
    public partial class InputDialog : Form
    {
        public InputDialog()
        {
            InitializeComponent();
        }

        public string Value
        {
            get => InputTextBox?.Text;
            set => InputTextBox.Text = value;
        }

        public bool ValueRequired { get; set; }

        public void SetMessageIcon(MessageBoxIcon icon)
        {
            MessageIconBox.Visible = (icon != MessageBoxIcon.None);
            if (!MessageIconBox.Visible)
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

        public static string Show(IWin32Window owner,
            string text, string caption, MessageBoxIcon icon, FormStartPosition startPosition)
        {
            using (var dlg = new InputDialog())
            {
                dlg.Text = caption;
                dlg.MessageTextLabel.Text = text;
                dlg.StartPosition = startPosition;
                dlg.SetMessageIcon(icon);
                if (dlg.ShowDialog(owner) == DialogResult.OK)
                    return dlg.Value;
            }

            return null;
        }

        public static string Show(string text, string caption, MessageBoxIcon icon, FormStartPosition startPosition)
        {
            return Show(null, text, caption, icon, startPosition);
        }

        public static string Show(IWin32Window owner, string text, string caption, MessageBoxIcon icon = MessageBoxIcon.None)
        {
            return Show(owner, text, caption, icon, FormStartPosition.CenterParent);
        }

        public static string Show(string text, string caption, MessageBoxIcon icon = MessageBoxIcon.None)
        {
            return Show(null, text, caption, icon, FormStartPosition.CenterParent);
        }

        private void Option1Button_Click(object sender, EventArgs e)
        {
            if (ValueRequired && string.IsNullOrWhiteSpace(Value))
            {
                InputTextBox.Focus();
                return;
            }

            DialogResult = DialogResult.OK;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            var normalKey = keyData & ~Keys.Control;
            normalKey &= ~Keys.Shift;
            normalKey &= ~Keys.Alt;
            if (normalKey == Keys.Enter)
            {
                if (InputTextBox.Focused)
                {
                    Option1Button_Click(null, EventArgs.Empty);
                    return true;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
