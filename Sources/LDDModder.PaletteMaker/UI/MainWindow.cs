using LDDModder.LDD;
using LDDModder.LDD.Files;
using LDDModder.PaletteMaker.DB;
using LDDModder.PaletteMaker.Generation;
using LDDModder.PaletteMaker.Models.Rebrickable;
using LDDModder.PaletteMaker.Rebrickable;
using LDDModder.PaletteMaker.Rebrickable.Models;
using LDDModder.PaletteMaker.Settings;
using LDDModder.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LDDModder.PaletteMaker.UI
{
    public partial class MainWindow : Form
    {
        private string DBFilePath;

        private List<RbColor> Colors;
        private List<RbTheme> Themes;
        private List<RbCategory> Categories;
        private Set SetInfo;

        public MainWindow()
        {
            InitializeComponent();
            Colors = new List<RbColor>();
            Themes = new List<RbTheme>();
            Categories = new List<RbCategory>();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            SetPartsGridView.AutoGenerateColumns = false;

            if (!SettingsManager.HasInitialized)
                SettingsManager.Initialize();

            LDDEnvironment.Initialize();
            RebrickableAPI.ApiKey = "aU49o5xulf";
            RebrickableAPI.InitializeClient();
            

            DBFilePath = SettingsManager.GetFilePath(SettingsManager.DATABASE_FILENAME);
            var currentFolder = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            //DBFilePath = Path.Combine(currentFolder, "BrickDatabase.db");
            //if (!File.Exists(DBFilePath))
            //    File.Copy(currentFolder + "\\Resources\\EmptyDatabase.db", DBFilePath);
            if (SettingsManager.DatabaseExists())
                ReloadRebrickableBaseData();
            else
                InitDatabase();
        }

        private void ReloadRebrickableBaseData()
        {
            using (var db = GetDbContext())
            {
                Colors = db.Colors.ToList();
                Themes = db.Themes.ToList();
                Categories = db.Categories.ToList();

                //ColorColumn.DataSource = Categories;
                //ColorColumn.DisplayMember = "Name";
                //ColorColumn.ValueMember = "ID";

                CategoryColumn.DataSource = Categories;
                CategoryColumn.DisplayMember = "Name";
                CategoryColumn.ValueMember = "ID";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            InitDatabase();
        }

        private void InitDatabase()
        {
            using (var win = new DatabaseInitProgressWindow())
            {
                win.StartPosition = FormStartPosition.CenterParent;
                win.ShowDialog();
                ReloadRebrickableBaseData();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    SetInfo = RebrickableAPI.GetSet(textBox1.Text);
                    if (SetInfo != null)
                    {
                        var setParts = RebrickableAPI.GetSetParts(SetInfo.SetNum).ToList();
                        BeginInvoke((Action)(() => LoadSetParts(setParts)));
                        //PalatteGenerator.CreatePaletteFromSet(DBFilePath, setInfo);
                    }

                }
                catch { }
            });
        }

        private void FindSet(string searchTxt)
        {
            using (var db = GetDbContext())
            {
                RbSet foundSet = null;
                if (Regex.IsMatch(searchTxt.Trim(), "^\\d+-\\d{1,2}$"))
                    foundSet = db.RbSets.FirstOrDefault(x => x.SetID == searchTxt.Trim());

                
            }
            
        }

        private PaletteDbContext GetDbContext()
        {
            return new PaletteDbContext($"Data Source={DBFilePath}");
        }

        private void LoadSetParts(List<Rebrickable.Models.SetPart> parts)
        {
            var setParts = parts.Select(x => new SetPartWrapper(x)).ToList();
            SetPartsGridView.DataSource = setParts;

            Task.Factory.StartNew(() =>
            {
                using (var db = GetDbContext())
                {
                    PalatteGenerator.FindLddPartsForSet(db, setParts);
                    var palette = PalatteGenerator.GeneratePalette(db, setParts);

                    var paletteFile = new PaletteFile(new LDD.Palettes.Bag()
                    {
                        Name = $"{SetInfo.SetNum} {SetInfo.Name}",
                        Countable = true,
                        ParentBrand = LDD.Data.Brand.LDD
                    });

                    paletteFile.Palettes.Add(palette);

                    var userPaletteDir = LDD.LDDEnvironment.Current.GetAppDataSubDir("UserPalettes");
                    string paletteFileName = FileHelper.GetSafeFileName(SetInfo.Name.Replace(" ", ""));

                    paletteFile.SaveToDirectory(Path.Combine(userPaletteDir, paletteFileName), false);

                    //paletteFile.SaveAsLif(Path.Combine(userPaletteDir, paletteFileName) + ".lif");
                }
            });
        }

        private void SetPartsGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            var row = SetPartsGridView.Rows[e.RowIndex];

            if (row.DataBoundItem is SetPartWrapper setPart)
            {
                if (SetPartsGridView.Columns[e.ColumnIndex] == ColorColumn)
                {
                    var color = Colors.FirstOrDefault(x => x.ID == setPart.ColorID);
                    e.Value = color?.Name ?? "Invalid Color";

                }
                else if (SetPartsGridView.Columns[e.ColumnIndex] == MatchStatusColumn)
                {
                    var tooltipLines = new List<string>();

                    if (setPart.MatchingFlags.HasFlag(PartMatchingFlags.InvalidRbPart))
                        tooltipLines.Add("Invalid Rebrickable part");

                    if (setPart.MatchingFlags.HasFlag(PartMatchingFlags.InvalidRbColor))
                        tooltipLines.Add("Invalid Rebrickable color");

                    if (setPart.MatchingFlags.HasFlag(PartMatchingFlags.InvalidLddColor))
                        tooltipLines.Add("Could not find matching LDD color");

                    row.Cells[e.ColumnIndex].ToolTipText = string.Join(Environment.NewLine, tooltipLines);

                    if (setPart.IsValid)
                    {
                        e.Value = "Matched";
                    }
                    else if (setPart.LddPart != null)
                    {
                        e.Value = "Invalid LDD Color";
                    }
                    else
                    {
                        e.Value = "Invalid";
                    }
                }
            }

        }

        private void SetPartsGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            Debug.WriteLine(SetPartsGridView.Rows[e.RowIndex].DataBoundItem.ToString());
            e.Cancel = true;
        }
    }
}
