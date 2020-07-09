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
    public partial class WaitPopup : Form
    {
        public string Message
        {
            get => MessageLabel.Text;
            set => MessageLabel.Text = value;
        }

        public WaitPopup()
        {
            InitializeComponent();
            MessageLabel.Text = string.Empty;
        }
    }
}
