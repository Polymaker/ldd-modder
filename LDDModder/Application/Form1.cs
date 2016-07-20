using LDDModder.LDD;
using LDDModder.LDD.Primitives;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace LDDModder
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        private void buttonEdit1_ButtonClicked(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.InitialDirectory = LDDManager.GetDirectory(LDDManager.DbDirectories.Primitives);
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    buttonEdit1.Text = Path.GetFileName(dlg.FileName);
                    var primitiveInfo = Primitive.LoadFrom<Primitive>(dlg.FileName);

                    if (primitiveInfo.Connections.OfType<ConnectivityCustom2DField>().Any())
                    {
                        custom2dFieldEditor1.EditValue = primitiveInfo.Connections.OfType<ConnectivityCustom2DField>().First();
                    }
                    
                }
            }
        }

        //[DllImport("user32.dll", CharSet = CharSet.Auto)]
        //static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
    }
}
