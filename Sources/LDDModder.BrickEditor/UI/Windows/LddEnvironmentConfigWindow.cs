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
    public partial class LddEnvironmentConfigWindow : Form
    {
        public LddEnvironmentConfigWindow()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            FillSettings(LDD.LDDEnvironment.Current);
        }

        private void FillSettings(LDD.LDDEnvironment environment)
        {
            PrgmFilePathTextBox.Text = environment?.ProgramFilesPath;
            AppDataPathTextBox.Text = environment?.ApplicationDataPath;

            AssetsStatusLabel.ForeColor = environment.AssetsExtracted ? Color.Green : Color.Red;
            AssetsStatusLabel.Text = environment.AssetsExtracted ? "LIF extracted" : "LIF not extracted";
            ExtractAssetsButton.Enabled = !environment.AssetsExtracted;

            DBStatusLabel.ForeColor = environment.DatabaseExtracted ? Color.Green : Color.Red;
            DBStatusLabel.Text = environment.DatabaseExtracted ? "LIF extracted" : "LIF not extracted";
            ExtractDBButton.Enabled = !environment.DatabaseExtracted;


        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
