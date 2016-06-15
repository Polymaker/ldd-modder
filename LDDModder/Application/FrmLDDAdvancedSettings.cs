using LDDModder.LDD;
using LDDModder.LDD.General;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LDDModder
{
    public partial class FrmLDDAdvancedSettings : Form
    {

        public FrmLDDAdvancedSettings()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            LoadSettings();
        }

        private void LoadSettings()
        {
            chkDeveloperMode.Checked = LDDManager.GetSettingValue(PreferencesSettings.DeveloperMode, LDDLocation.AppData) == "1";
            chkExtendedTooltips.Checked = LDDManager.GetSettingValue(PreferencesSettings.ShowExtendedBrickToolTip, LDDLocation.AppData) == "1";
            var userModelDirValue = LDDManager.GetSettingValue(PreferencesSettings.UserModelDirectory, LDDLocation.AppData);
            btnTxtUserModelDir.Text = DecodeSettingPath(userModelDirValue);
        }

        private void DeveloperModeLabel_ClickRedirect(object sender, EventArgs e)
        {
            chkDeveloperMode.Checked = !chkDeveloperMode.Checked;
        }

        private void ExtendedTooltipLabel_ClickRedirect(object sender, EventArgs e)
        {
            chkExtendedTooltips.Checked = !chkExtendedTooltips.Checked;
        }

        private static string DecodeSettingPath(string pathValue)
        {
            if (pathValue.StartsWith("file"))
                pathValue = pathValue.Substring(5);
            pathValue = pathValue.Replace('/', '\\');
            pathValue = pathValue.Insert(1, ":");
            return pathValue;
        }

        private void btnTxtUserModelDir_ButtonClicked(object sender, EventArgs e)
        {
            using (var dlg = new FolderBrowserDialog())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    btnTxtUserModelDir.Text = dlg.SelectedPath;
                }
            }
        }
    }
}
