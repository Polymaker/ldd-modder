using LDDModder.LDD;
using LDDModder.PaletteMaker.DB;
using LDDModder.PaletteMaker.Generation;
using LDDModder.PaletteMaker.Models.Rebrickable;
using LDDModder.PaletteMaker.Rebrickable;
using LDDModder.PaletteMaker.Rebrickable.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
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

            LDDEnvironment.Initialize();
            RebrickableAPI.ApiKey = "aU49o5xulf";
            RebrickableAPI.InitializeClient();

            var currentFolder = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            DBFilePath = Path.Combine(currentFolder, "BrickDatabase.db");
            if (!File.Exists(DBFilePath))
                File.Copy(currentFolder + "\\Resources\\EmptyDatabase.db", DBFilePath);

            ReloadRebrickableBaseData();
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
            var cts = new CancellationTokenSource();
            Task.Factory.StartNew(() =>
            {
                DatabaseInitializer.ImportBaseData(DBFilePath, cts.Token);
                //DatabaseInitializer.ImportRebrickableData(DBFilePath, cts.Token);
                DatabaseInitializer.InitializeDefaultMappings(DBFilePath);
            });
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                var setInfo = RebrickableAPI.GetSet("75252-1");
                var setParts = RebrickableAPI.GetSetParts(setInfo.SetNum).ToList();
                BeginInvoke((Action)(() => LoadSetParts(setParts)));
                //PalatteGenerator.CreatePaletteFromSet(DBFilePath, setInfo);

            });
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
                    PalatteGenerator.GeneratePalette(db, setParts);
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
                    switch (setPart.MatchingFlags)
                    {
                        case PartMatchingFlags.NotMatched:
                            e.Value = "Not Matched";
                            break;
                        case PartMatchingFlags.Matched:
                            e.Value = "Matched";
                            break;
                        case PartMatchingFlags.NonLegoPart:
                            e.Value = "Non Lego";
                            break;
                        case PartMatchingFlags.InvalidRbColor:
                            e.Value = "Invalid Color";
                            break;
                        case PartMatchingFlags.InvalidLddColor:
                            e.Value = "LDD Color not found";
                            break;
                        case PartMatchingFlags.InvalidRbPart:
                            e.Value = "Invalid Part";
                            break;
                        case PartMatchingFlags.LddPartNotFound:
                            e.Value = "LDD Part not found";
                            break;
                        case PartMatchingFlags.DecorationNotFound:
                            break;
                    }

                }
            }

        }
    }
}
