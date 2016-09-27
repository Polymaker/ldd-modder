using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LDDModder.LDD;
using System.IO;
using LDDModder.LDD.Files;
using System.Diagnostics;

namespace LDDModder.Views
{
    public partial class LocalizationsManager : UserControl
    {
        private List<LocLanguageInfo> Languages;
        private LocLanguageInfo SelectedLanguage;

        public LocalizationsManager()
        {
            InitializeComponent();
            Languages = new List<LocLanguageInfo>();
            //lvwLanguages.group
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            //if (!DesignMode)
            //    LoadLanguageList();
        }

        class LocLanguageInfo
        {
            public string Key { get; set; }
            public string Name { get; set; }
            public bool IsActive { get; set; }
            public string AppStringFilename { get; set; }
            public bool AppStringExist { get; set; }
            public string MaterialsFilename { get; set; }
            public bool MaterialsExist { get; set; }
        }

        public void LoadLanguageList()
        {
            Languages.Clear();
            if (!LDDManager.IsLifExtracted(LifInstance.Assets) || !LDDManager.IsLifExtracted(LifInstance.Database))
                return;

            var languagesFilePath = LocalizationFile.GetLocFilename(LocalizationFileKind.AvailableLanguages, null);
            var availableLangs = ReadLocFile(languagesFilePath);

            foreach (var entry in availableLangs)
            {
                if (entry.Key == "Languages")
                    continue;
                Languages.Add(new LocLanguageInfo() { Key = entry.Key, Name = entry.Value });
            }

            if (availableLangs.ContainsKey("Languages"))//check for ultimate safety...
            {
                var activeLangs = availableLangs["Languages"].Split(',');
                foreach (var langKey in activeLangs)
                {
                    var langObj = Languages.FirstOrDefault(l => l.Key == langKey);
                    if (langObj == null)//again for safety...
                    {
                        Trace.WriteLine("Active language not found!?");
                        continue;
                    }
                    langObj.IsActive = true;
                }
            }

            foreach (var langInfo in Languages)
            {
                langInfo.AppStringFilename = LocalizationFile.GetLocFilename(LocalizationFileKind.ApplicationStrings, langInfo.Key);
                langInfo.AppStringExist = File.Exists(langInfo.AppStringFilename);

                langInfo.MaterialsFilename = LocalizationFile.GetLocFilename(LocalizationFileKind.MaterialNames, langInfo.Key);
                langInfo.MaterialsExist = File.Exists(langInfo.MaterialsFilename);
            }
            lvwLanguages.SetObjects(Languages);
        }

        private void lvwLanguages_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Languages.Count > 0)
            {
                var selectedItem = lvwLanguages.SelectedObject as LocLanguageInfo;
                if (selectedItem != SelectedLanguage)
                    LoadLanguage(selectedItem);
            }
        }

        private void LoadLanguage(LocLanguageInfo language)
        {
            SelectedLanguage = language;
            if (language == null)
                return;

            //chkActive.Checked = language.IsActive;
            //tabCtrlFiles.SelectedTab = tabPageApp;
            //lvwLocalizations.Enabled = language.AppStringExist;

            //if (language.AppStringExist)
            //{
            //    var locKeyVals = ReadLocFile(language.AppStringFilename);
            //    lvwLocalizations.SetObjects(locKeyVals);
            //}

        }

        private static Dictionary<string, string> ReadLocFile(string filename)
        {
            if (!File.Exists(filename))
                return new Dictionary<string, string>();
            using (var locFile = LocalizationFile.OpenRead(filename))
            {
                return locFile.Entries.ToDictionary(entry => entry.Key, entry => entry.Value);//clone
            }
        }

        private void lvwLocalizations_CellEditStarting(object sender, BrightIdeasSoftware.CellEditEventArgs e)
        {
            //txtLocValue.Text = (string)e.Value;
        }
    }
}
