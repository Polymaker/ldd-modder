using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LDDModder.BrickEditor.Settings;
using System.IO;
using System.Diagnostics;

namespace LDDModder.BrickEditor.UI.Settings
{
    public partial class DisplaySettingsPanel : SettingsPanelBase
    {
        public DisplaySettingsPanel()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string layoutDir = Path.Combine(SettingsManager.AppDataFolder, "layouts");
            if (!Directory.Exists(layoutDir))
                Directory.CreateDirectory(layoutDir);
            Process.Start("explorer.exe", $"\"{layoutDir}\"");
        }
    }
}
