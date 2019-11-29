using LDDModder.LDD;
using LDDModder.PaletteMaker.DB;
using LDDModder.PaletteMaker.Generation;
using LDDModder.PaletteMaker.Rebrickable;
using LDDModder.PaletteMaker.Rebrickable.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        public MainWindow()
        {
            InitializeComponent();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            LDDEnvironment.Initialize();
            RebrickableAPI.ApiKey = "aU49o5xulf";
            RebrickableAPI.InitializeClient();

            var currentFolder = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            DBFilePath = Path.Combine(currentFolder, "BrickDatabase.db");
            if (!File.Exists(DBFilePath))
                File.Copy(currentFolder + "\\Resources\\EmptyDatabase.db", DBFilePath);
        }
        private void button1_Click(object sender, EventArgs e)
        {

           
            var cts = new CancellationTokenSource();
            Task.Factory.StartNew(() =>
            {
                DatabaseInitializer.ImportBaseData(DBFilePath, cts.Token);
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

        public class SetPartWrapper
        {
            public Rebrickable.Models.SetPart SetPart { get; set; }

            public string PartNumber => SetPart.Part.PartNum;

            public string PartName => SetPart.Part.Name;

            public string ElementId => SetPart.ElementId;

            public string Color => $"{SetPart.Color.Name} ({SetPart.Color.Id})";

            public int Quantity => SetPart.Quantity;

            public SetPartWrapper(SetPart setPart)
            {
                SetPart = setPart;
            }
        }

        private void LoadSetParts(List<Rebrickable.Models.SetPart> parts)
        {
            dataGridView1.DataSource = parts.Select(x=> new SetPartWrapper(x)).ToList();
        }
    }
}
