using LDDModder.LDD.Files;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace LDDModder.BrickEditor.UI.Panels
{
    public partial class LocalisationEditorPanel : DockContent
    {
        private static string LddAssetsPath;
        private static string LddMaterialsPath;

        #region Item Classes

        public enum LocFileType
        {
            Application,
            Materials
        }

        public class LanguageInfo
        {
            public string Key { get; set; }
            public string Name { get; set; }
            public bool Enabled { get; set; }
            public bool AppFileExist { get; set; }
            public bool MatFileExist { get; set; }

            public string OriginalName { get; set; }

            public bool WasEnabled { get; set; }

            public bool IsModified => OriginalName != Name || Enabled != WasEnabled;

            public LocalizationFile ApplicationLocalizations;

            public LocalizationFile MaterialsLocalizations;

            public LanguageInfo(string key, string name)
            {
                Key = key;
                Name = name;
                OriginalName = name;
            }

            public LanguageInfo(string key, string name, bool enabled)
            {
                Key = key;
                Name = name;
                Enabled = enabled;
                WasEnabled = enabled;
            }

            public string GetTargetPath(LocFileType fileType)
            {
                if (fileType == LocFileType.Application)
                    return Path.Combine(LddAssetsPath, Key, "localizedStrings.loc");
                return Path.Combine(LddMaterialsPath, Key.ToUpper(), "localizedStrings.loc");
            }

            public bool CheckFileExist(LocFileType fileType)
            {
                return File.Exists(GetTargetPath(fileType));
            }
        }

        #endregion

        private LocalizationFile LanguagesConfigFile;
        private BindingList<LanguageInfo> Languages;
        private Dictionary<string, LocalizationFile> AppLocalizations;
        private Dictionary<string, LocalizationFile> MatLocalizations;

        public LocalisationEditorPanel()
        {
            InitializeComponent();
            Languages = new BindingList<LanguageInfo>();
            AppLocalizations = new Dictionary<string, LocalizationFile>();
            MatLocalizations = new Dictionary<string, LocalizationFile>();

            AvailableLanguagesGrid.AutoGenerateColumns = false;
            dataGridView2.AutoGenerateColumns = false;
            dataGridView1.AutoGenerateColumns = false;

            DockAreas = DockAreas.Document | DockAreas.Float;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            UpdateLddEnvironment();

            LoadLocalizationFiles();

            LoadAvailableLanguages();

            GenerateLanguageColumns();

            LoadLocalizations(Languages.FirstOrDefault(x => x.AppFileExist));
        }

        public void UpdateLddEnvironment()
        {
            LddAssetsPath = Path.Combine(LDD.LDDEnvironment.Current.ProgramFilesPath, "Assets");
            LddMaterialsPath = Path.Combine(LDD.LDDEnvironment.Current.ApplicationDataPath, "db\\MaterialNames");
        }

        private void LoadLocalizationFiles()
        {
            var languagesFilePath = Path.Combine(LddAssetsPath, "Languages.loc");
            if (File.Exists(languagesFilePath))
                LanguagesConfigFile = LocalizationFile.Read(languagesFilePath);

            foreach (var locFilePath in Directory.GetFiles(LddAssetsPath, "localizedStrings.loc",
                SearchOption.AllDirectories))
            {
                var dirInfo = new DirectoryInfo(Path.GetDirectoryName(locFilePath));

                try
                {
                    var locFile = LocalizationFile.Read(locFilePath);
                    AppLocalizations.Add(dirInfo.Name.ToUpper(), locFile);
                }
                catch { }
            }

            foreach (var locFilePath in Directory.GetFiles(LddMaterialsPath, "localizedStrings.loc",
                SearchOption.AllDirectories))
            {
                var dirInfo = new DirectoryInfo(Path.GetDirectoryName(locFilePath));

                try
                {
                    var locFile = LocalizationFile.Read(locFilePath);
                    MatLocalizations.Add(dirInfo.Name.ToUpper(), locFile);
                }
                catch { }
            }
        }

        private void LoadAvailableLanguages()
        {
            Languages.Clear();
            var enabledLangs = LanguagesConfigFile["Languages"].Split(',');

            foreach (var item in LanguagesConfigFile)
            {
                if (item.Key == "Languages")
                    continue;

                var langInfo = new LanguageInfo(item.Key, item.Value, enabledLangs.Contains(item.Key));

                if (AppLocalizations.ContainsKey(item.Key.ToUpper()))
                {
                    langInfo.AppFileExist = true;
                    langInfo.ApplicationLocalizations = AppLocalizations[item.Key.ToUpper()];
                }

                if (MatLocalizations.ContainsKey(item.Key.ToUpper()))
                {
                    langInfo.MatFileExist = true;
                    langInfo.MaterialsLocalizations = MatLocalizations[item.Key.ToUpper()];
                }

                Languages.Add(langInfo);
            }

            AvailableLanguagesGrid.DataSource = Languages;
        }

        private void LoadLocalizations(LanguageInfo language)
        {
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();

            if (language != null)
            {
                var appFile = AppLocalizations.ContainsKey(language.Key.ToUpper()) ?
                AppLocalizations[language.Key.ToUpper()] : null;

                if (appFile != null)
                {
                    foreach (var locItem in appFile)
                    {
                        int rowIndex = dataGridView2.Rows.Add();
                        var newRow = dataGridView2.Rows[rowIndex];
                        newRow.Cells[0].Value = locItem.Key;
                        newRow.Cells[1].Value = locItem.Value;
                        //for (int i = 1; i < newRow.Cells.Count; i++)
                        //    newRow.Cells[i].Value = newRow.Cells[i].OwningColumn.Tag
                    }
                }

                var matFile = MatLocalizations.ContainsKey(language.Key.ToUpper()) ?
                    MatLocalizations[language.Key.ToUpper()] : null;

                if (matFile != null)
                {
                    foreach (var locItem in matFile)
                    {
                        int rowIndex = dataGridView1.Rows.Add();
                        var newRow = dataGridView1.Rows[rowIndex];
                        newRow.Cells[0].Value = locItem.Key;
                        newRow.Cells[1].Value = locItem.Value;
                    }
                }
            }
        }

        private void UpdateAvailableLanguagesButton_Click(object sender, EventArgs e)
        {
            UpdateAvailableLanguages();
        }

        private void UpdateAvailableLanguages()
        {
            LanguagesConfigFile.Clear();
            LanguagesConfigFile["Languages"] = string.Join(",", Languages.Where(x => x.Enabled).Select(x => x.Name));
            
            foreach (var lang in Languages)
                LanguagesConfigFile.Add(lang.Key, lang.Name);

            //TODO: Save (requires Admin rights to save in Program Files)
            //LanguagesConfigFile.Save(Path.Combine(LddAssetsPath, "Languages.loc"));
        }

        private void CreateLocalizationFiles(LanguageInfo language)
        {
            var appLocFilePath = language.GetTargetPath(LocFileType.Application);
            //TODO: Save (requires Admin rights to save in Program Files)

            var matLocFilePath = language.GetTargetPath(LocFileType.Materials);
            Directory.CreateDirectory(Path.GetDirectoryName(matLocFilePath));
            var tmpFile = new LocalizationFile();
            tmpFile.Save(matLocFilePath);
        }

        #region Available Languages Gird Handling

        private void AvailableLanguagesGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var clickedRow = AvailableLanguagesGrid.Rows[e.RowIndex];
                var clickedCell = clickedRow.Cells[e.ColumnIndex];
                var language = clickedRow.DataBoundItem as LanguageInfo;

                if (language != null &&
                    AvailableLanguagesGrid.Columns[e.ColumnIndex].DataPropertyName == "Enabled")
                {
                    if (!language.WasEnabled && (!language.AppFileExist || !language.MatFileExist))
                    {

                        var res = MessageBox.Show(
                            "The localization files do not exist for this language.\r\n" +
                            "Do you want to create them?"
                            , "", MessageBoxButtons.YesNo);

                        if (res == DialogResult.Yes)
                        {
                            CreateLocalizationFiles(language);
                            if (clickedCell.IsInEditMode)
                                AvailableLanguagesGrid.EndEdit();
                            GenerateLanguageColumns();
                        }
                        else
                        {
                            if (clickedCell.IsInEditMode)
                                AvailableLanguagesGrid.CancelEdit();
                        }
                    }
                }
            }

        }

        #endregion

        #region Localizations Grids Handling

        private void GenerateLanguageColumns()
        {
            for (int i = dataGridView1.Columns.Count - 1; i > 0; i--)
                dataGridView1.Columns.RemoveAt(i);
            for (int i = dataGridView2.Columns.Count - 1; i > 0; i--)
                dataGridView2.Columns.RemoveAt(i);

            foreach (var lang in Languages)
            {
                dataGridView2.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    Name = $"AppLang{lang.Key}Value",
                    Tag = lang.Key,
                    HeaderText = lang.Key,
                    Visible = lang.AppFileExist
                });

                dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    Name = $"MatLang{lang.Key}Value",
                    Tag = lang.Key,
                    HeaderText = lang.Key,
                    Visible = lang.MatFileExist
                });
            }
        }

        #endregion
    }
}
