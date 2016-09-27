using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LDDModder.Forms
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            localizationsManager1.LoadLanguageList();
        }
    }
}
