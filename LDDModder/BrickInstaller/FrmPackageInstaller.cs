using LDDModder.LDD;
using LDDModder.Modding;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LDDModder.BrickInstaller
{
    public partial class FrmPackageInstaller : Form
    {
        public FrmPackageInstaller()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            InitializeLayout();
            progBar.Style = ProgressBarStyle.Marquee;
            lblOperationInfo.Text = LOC_ValidateLDD;
            Common.ValidateLddInstall(this);
            
        }

        private void InitializeLayout()
        {
            rtbInstallLog.Visible = false;
            btnAction.Visible = btnShowDetails.Visible = false;

            tableLayoutPanel1.RowStyles[tableLayoutPanel1.RowCount - 1].SizeType = SizeType.AutoSize;
            tableLayoutPanel1.Dock = DockStyle.Top;
            tableLayoutPanel1.AutoSize = true;
            tableLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            int vPadding = Height - ClientSize.Height;
            Height = tableLayoutPanel1.Height + vPadding;
        }

        public void SetPackages(string[] filepaths)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //var brickPack = BrickPackage.OpenOrCreate("Wheel Hub.cbp");
            //brickPack.AddBrick("32496.xml");
            //brickPack.AddModel("32496.g");
            //brickPack.Close();
            //PackageInstaller.InstallPackage(brickPack);
        }
    }
}
