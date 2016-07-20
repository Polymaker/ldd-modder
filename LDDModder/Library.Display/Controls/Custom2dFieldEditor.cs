using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LDDModder.LDD.Primitives;

namespace LDDModder.Display.Controls
{
    public partial class Custom2dFieldEditor : UserControl
    {
        // Fields...
        private ConnectivityCustom2DField _EditValue;

        public ConnectivityCustom2DField EditValue
        {
            get { return _EditValue; }
            set
            {
                if (_EditValue == value)
                    return;
                _EditValue = value;
                if (IsHandleCreated)
                    SetupGrid();
            }
        }

        public Custom2dFieldEditor()
        {
            InitializeComponent();
        }

        private void SetupGrid()
        {
            if (EditValue == null)
                return;
            tableLayoutPanel1.RowStyles.Clear();
            tableLayoutPanel1.ColumnStyles.Clear();
            for (int i = 0; i <= EditValue.Height; i++)
            {
                tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            }
            tableLayoutPanel1.RowCount = tableLayoutPanel1.RowStyles.Count;
            for (int i = 0; i <= EditValue.Width; i++)
            {

                tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            }
            tableLayoutPanel1.ColumnCount = tableLayoutPanel1.ColumnStyles.Count;
        }
    }
}
