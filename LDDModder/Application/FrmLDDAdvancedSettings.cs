using LDDModder.LDD;
using LDDModder.LDD.General;
using LDDModder.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Windows.Forms;

namespace LDDModder
{
    public partial class FrmLDDAdvancedSettings : Form
    {
        private bool AdminRightsNeeded = false;
        private Bitmap UacShieldBmp;

        public FrmLDDAdvancedSettings()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            CheckProgramFilesRights();
            LoadSettings();

            var layoutMinSize = tableLayoutPanel1.GetPreferredSize(Size).Height;
            var topFrameHeight =Height - ClientSize.Height;
            MinimumSize = new Size(DefaultMinimumSize.Width, layoutMinSize + topFrameHeight + Padding.Vertical);
            MaximumSize = new Size(900, MinimumSize.Height);
            
        }


        private void LoadSettings()
        {
            chkDeveloperMode.Checked = GetSettingBoolean(PreferencesSettings.DeveloperMode, LDDLocation.AppData);
            chkShowTooltip.Checked = GetSettingBoolean(PreferencesSettings.ShowToolTip, LDDLocation.AppData);
            chkExtendedTooltip.Checked = GetSettingBoolean(PreferencesSettings.ShowExtendedBrickToolTip, LDDLocation.AppData);
            chkDoServerCall.Checked = !GetSettingBoolean(PreferencesSettings.DoServerCall, LDDLocation.ProgramFiles);
            chkVerbose.Checked = GetSettingBoolean(PreferencesSettings.Verbose, LDDLocation.ProgramFiles);

            var userModelDirValue = LDDManager.GetSettingValue(PreferencesSettings.UserModelDirectory, LDDLocation.AppData);
            btnTxtUserModelDir.Text = DecodeSettingPath(userModelDirValue);
        }

        private void CheckProgramFilesRights()
        {
            if (SecurityHelper.IsUserAdministrator)
                AdminRightsNeeded = false;
            else
                AdminRightsNeeded = !SecurityHelper.HasWritePermission(LDDManager.GetSettingsFilePath(LDDLocation.ProgramFiles));
            
            if (AdminRightsNeeded)
            {
                UacShieldBmp = NativeHelper.GetUacShieldIcon();
                lblDoServerCall.Image = UacShieldBmp;
                lblVerbose.Image = UacShieldBmp;
            }
        }

        private void DeveloperModeLabel_ClickRedirect(object sender, EventArgs e)
        {
            chkDeveloperMode.Checked = !chkDeveloperMode.Checked;
        }

        private void ShowTooltipLabel_ClickRedirect(object sender, EventArgs e)
        {
            chkShowTooltip.Checked = !chkShowTooltip.Checked;
        }

        private void ExtendedTooltipLabel_ClickRedirect(object sender, EventArgs e)
        {
            if(chkExtendedTooltip.Enabled)
                chkExtendedTooltip.Checked = !chkExtendedTooltip.Checked;
        }

        private static string DecodeSettingPath(string pathValue)
        {
            if (string.IsNullOrEmpty(pathValue) || !pathValue.StartsWith("file"))
                return pathValue;

            pathValue = pathValue.Substring(5).Replace('/', '\\');
            pathValue = pathValue.Insert(1, ":");
            return pathValue;
        }

        private void btnTxtUserModelDir_ButtonClicked(object sender, EventArgs e)
        {
            using (var dlg = new FolderBrowserDialog())
            {
                dlg.SelectedPath = btnTxtUserModelDir.Text;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    btnTxtUserModelDir.Text = dlg.SelectedPath;
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            LDDManager.SetSetting(PreferencesSettings.ShowToolTip, chkShowTooltip.Checked ? "yes" : "no", LDDLocation.AppData);
            LDDManager.SetSetting(PreferencesSettings.ShowExtendedBrickToolTip, chkExtendedTooltip.Checked && chkShowTooltip.Checked ? "yes" : "no", LDDLocation.AppData);

            LDDManager.SetSetting(PreferencesSettings.DeveloperMode, chkDeveloperMode.Checked ? "yes" : "no", LDDLocation.AppData);

            if (!AdminRightsNeeded)
            {
                LDDManager.SetSetting(PreferencesSettings.DoServerCall, !chkDoServerCall.Checked ? "yes" : "no", LDDLocation.ProgramFiles);
                LDDManager.SetSetting(PreferencesSettings.Verbose, chkVerbose.Checked ? "yes" : "no", LDDLocation.ProgramFiles);
            }
            else
            {
                var changedSettings = new List<PreferenceEntry>();
                if (!GetSettingBoolean(PreferencesSettings.DoServerCall, LDDLocation.ProgramFiles) != chkDoServerCall.Checked)
                    changedSettings.Add(new PreferenceEntry() { Key = PreferencesSettings.DoServerCall, Value = (!chkDoServerCall.Checked ? "yes" : "no"), Location = LDDLocation.ProgramFiles });

                if (GetSettingBoolean(PreferencesSettings.Verbose, LDDLocation.ProgramFiles) != chkVerbose.Checked)
                    changedSettings.Add(new PreferenceEntry() { Key = PreferencesSettings.Verbose, Value = (chkVerbose.Checked ? "yes" : "no"), Location = LDDLocation.ProgramFiles });

                if (changedSettings.Count > 0)
                {
                    var processInfo = new ProcessStartInfo()
                    {
                        Verb = "runas",
                        Arguments = "set " + changedSettings.Select(x => x.Serialize()).Aggregate((a, b) => a + " " + b),
                        FileName = Application.ExecutablePath
                    };
                    Process.Start(processInfo);
                }
            }
        }

        private void chkShowTooltip_CheckedChanged(object sender, EventArgs e)
        {
            chkExtendedTooltip.Enabled = chkShowTooltip.Checked;
        }

        private static bool GetSettingBoolean(string key, LDDLocation loc)
        {
            string strVal = LDDManager.GetSettingValue(key, loc).Trim();
            if (string.IsNullOrEmpty(strVal))
                return true; 
            return strVal == "1" || strVal.Equals("yes", StringComparison.InvariantCultureIgnoreCase);
        }

    }
}
