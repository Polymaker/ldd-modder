using LDDModder.LDD.Primitives;
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
using System.Xml.Linq;

namespace LDDModder.BrickEditor.UI.Windows
{
    public partial class SelectBrickDialog : Form
    {
        public class BrickInfo
        {
            public int PartId { get; private set; }
            public string Description { get; private set; }
            public string PrimitivePath { get; private set; }
            public string[] MeshPaths { get; private set; }
            public bool Decorated { get; private set; }
            public bool Flexible { get; private set; }

            public BrickInfo(int partId, string description, string primitivePath, string[] meshPaths, bool flexible)
            {
                PartId = partId;
                Description = description;
                PrimitivePath = primitivePath;
                MeshPaths = meshPaths;
                Flexible = flexible;
                Decorated = meshPaths.Length > 1;
            }
        }

        private List<BrickInfo> Bricks;

        public SelectBrickDialog()
        {
            InitializeComponent();
            Bricks = new List<BrickInfo>();
            LoadingProgressBar.Visible = false;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            LoadingProgressBar.Visible = true;
            LoadingProgressBar.Style = ProgressBarStyle.Marquee;
            Task.Factory.StartNew(() => LoadBrickList());
        }

        private void LoadBrickList()
        {
            var primitivePath = Path.Combine(LDD.LDDEnvironment.Current.ApplicationDataPath, "db\\Primitives");
            var meshPath = Path.Combine(LDD.LDDEnvironment.Current.ApplicationDataPath, "db\\Primitives\\LOD0");

            var primitiveFiles = Directory.EnumerateFiles(primitivePath, "*.xml").ToList();
            int processed = 0;

            foreach (var xmlFilePath in primitiveFiles)
            {
                if (int.TryParse(Path.GetFileNameWithoutExtension(xmlFilePath), out int partID))
                {
                    var primitive = Primitive.Load(xmlFilePath);
                    var meshFiles = Directory.EnumerateFiles(meshPath, $"{partID}.g*").ToArray();
                    var brick = new BrickInfo(partID, primitive.Name, xmlFilePath, meshFiles, primitive.FlexBones.Any());
                    Bricks.Add(brick);

                    BeginInvoke((Action)(() => AddBrickToList(brick)));
                }
                UpdateProgress((int)((++processed / (float)primitiveFiles.Count) * 100f));
            }
            GC.Collect();
            Thread.Sleep(1000);
            UpdateProgress(-1);
        }


        private void UpdateProgress(int value)
        {
            if (InvokeRequired)
                BeginInvoke((Action)(() => UpdateProgress(value)));
            else
            {
                if (value >= 0)
                {
                    if (LoadingProgressBar.Style == ProgressBarStyle.Marquee)
                        LoadingProgressBar.Style = ProgressBarStyle.Continuous;
                    LoadingProgressBar.Value = value;
                }
                else
                {
                    LoadingProgressBar.Visible = false;
                }
            }
        }

        private void AddBrickToList(BrickInfo brick)
        {
            var lvi = new ListViewItem();
            lvi.Text = brick.PartId.ToString();
            lvi.SubItems.Add(brick.Description);
            lvi.SubItems.Add(brick.Decorated ? "Yes" : "No");
            lvi.SubItems.Add(brick.Flexible ? "Yes" : "No");
            BrickListView.Items.Add(lvi);
            //BrickListView.Invalidate();
        }
    }
}
