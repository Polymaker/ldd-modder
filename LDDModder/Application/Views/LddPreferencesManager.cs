using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LDDModder.Utilities;
using LDDModder.LDD;
using LDDModder.LDD.General;
using System.IO;

namespace LDDModder.Views
{
    public partial class LddPreferencesManager : UserControl
    {
        #region Fields

        private bool AdminRightsNeeded = false;
        private Bitmap UacShieldBmp;

        #endregion

        public LddPreferencesManager()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (ValidateLDD())
            {
                CheckProgramFilesRights();
                LoadSettings();
            }
        }

        private bool ValidateLDD()
        {
            if (!LDDManager.IsInstalled)
            {
                foreach (var checkBox in tlpLayout.Controls.OfType<CheckBox>())
                {
                    checkBox.Checked = false;
                    checkBox.Enabled = false;
                }
                btnTxtUserModelDir.Text = string.Empty;
                btnTxtUserModelDir.Enabled = false;

            }
            return LDDManager.IsInstalled;
        }

        public void LoadSettings()
        {
            if (!LDDManager.IsInstalled)
                return;

            chkDeveloperMode.Checked = LDDManager.GetSettingBoolean(PreferencesSettings.DeveloperMode, LDDLocation.AppData);
            chkShowTooltip.Checked = LDDManager.GetSettingBoolean(PreferencesSettings.ShowToolTip, LDDLocation.AppData);
            chkExtendedTooltip.Checked = LDDManager.GetSettingBoolean(PreferencesSettings.ShowExtendedBrickToolTip, LDDLocation.AppData);
            chkDoServerCall.Checked = !LDDManager.GetSettingBoolean(PreferencesSettings.DoServerCall, LDDLocation.ProgramFiles);
            chkVerbose.Checked = LDDManager.GetSettingBoolean(PreferencesSettings.Verbose, LDDLocation.ProgramFiles);

            var userModelDirValue = LDDManager.GetSettingValue(PreferencesSettings.UserModelDirectory, LDDLocation.AppData);
            btnTxtUserModelDir.Text = PreferencesSettings.DecodePath(userModelDirValue);
        }
        
        private void CheckProgramFilesRights()
        {
            if (!LDDManager.IsInstalled)
                return;

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

        private void CheckboxLabel_Click(object sender, EventArgs e)
        {
            if (sender == lblDeveloperMode)
            {
                chkDeveloperMode.Checked = !chkDeveloperMode.Checked;
            }
            else if (sender == lblDoServerCall)
            {
                chkDoServerCall.Checked = !chkDoServerCall.Checked;
            }
            else if (sender == lblShowTooltip)
            {
                chkShowTooltip.Checked = !chkShowTooltip.Checked;
            }
            else if (sender == lblExtendedTooltip)
            {
                if (chkExtendedTooltip.Enabled)
                    chkExtendedTooltip.Checked = !chkExtendedTooltip.Checked;
            }
        }

        private void chkShowTooltip_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShowTooltip.Enabled)
                chkExtendedTooltip.Enabled = chkShowTooltip.Checked;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            LDDManager.SetSetting(PreferencesSettings.ShowToolTip, chkShowTooltip.Checked ? "yes" : "no", LDDLocation.AppData);
            LDDManager.SetSetting(PreferencesSettings.ShowExtendedBrickToolTip, chkExtendedTooltip.Checked && chkShowTooltip.Checked ? "yes" : "no", LDDLocation.AppData);

            LDDManager.SetSetting(PreferencesSettings.DeveloperMode, chkDeveloperMode.Checked ? "yes" : "no", LDDLocation.AppData);

            LDDManager.SetSetting(PreferencesSettings.UserModelDirectory, PreferencesSettings.EncodePath(btnTxtUserModelDir.Text), LDDLocation.AppData);

            if (!AdminRightsNeeded)
            {
                LDDManager.SetSetting(PreferencesSettings.DoServerCall, !chkDoServerCall.Checked ? "yes" : "no", LDDLocation.ProgramFiles);
                LDDManager.SetSetting(PreferencesSettings.Verbose, chkVerbose.Checked ? "yes" : "no", LDDLocation.ProgramFiles);
            }
            else
            {
                var changedSettings = new List<PreferenceEntry>();
                bool currentValue = LDDManager.GetSettingBoolean(PreferencesSettings.DoServerCall, LDDLocation.ProgramFiles);
                if (currentValue == chkDoServerCall.Checked)// == because chkDoServerCall is an inverted condition 
                {
                    changedSettings.Add(new PreferenceEntry() { Key = PreferencesSettings.DoServerCall, Value = (!chkDoServerCall.Checked ? "yes" : "no"), Location = LDDLocation.ProgramFiles });
                }
                currentValue = LDDManager.GetSettingBoolean(PreferencesSettings.Verbose, LDDLocation.ProgramFiles);

                if (currentValue != chkVerbose.Checked)
                {
                    changedSettings.Add(new PreferenceEntry() { Key = PreferencesSettings.Verbose, Value = (chkVerbose.Checked ? "yes" : "no"), Location = LDDLocation.ProgramFiles });
                }
            }
        }

        private void btnTxtUserModelDir_ButtonClicked(object sender, EventArgs e)
        {
            using (var dlg = new FolderBrowserDialog())
            {
                if(!string.IsNullOrEmpty(btnTxtUserModelDir.Text) && 
                    Directory.Exists(btnTxtUserModelDir.Text))
                    dlg.SelectedPath = btnTxtUserModelDir.Text;

                if (dlg.ShowDialog() == DialogResult.OK)
                    btnTxtUserModelDir.Text = dlg.SelectedPath;
            }
        }
    }
}
