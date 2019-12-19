using LDDModder.BrickEditor.Settings;
using LDDModder.LDD.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace LDDModder.BrickEditor.UI.Windows
{
    public partial class SelectBrickDialog : Form
    {
        private const string BRICK_LIST_CACHE_FILENAME = "BrickList.json";

        #region Classes

        public class BrickInfo
        {
            [JsonProperty]
            public int PartId { get; set; }
            [JsonProperty]
            public string Platform { get; set; }
            [JsonProperty]
            public string Category { get; set; }
            [JsonProperty]
            public string Description { get; set; }
            [JsonProperty]
            public string PrimitiveFilename { get; set; }
            [JsonProperty]
            public string[] MeshFilenames { get; set; }
            [JsonProperty]
            public bool Decorated { get; set; }
            [JsonProperty]
            public bool Flexible { get; set; }
            [JsonProperty]
            public DateTime LastUpdate { get; set; }

            public BrickInfo()
            {
            }

            public BrickInfo(Primitive primitive, string primitivePath, string[] meshPaths)
            {
                PartId = primitive.ID;
                Description = primitive.Name;
                PrimitiveFilename = primitivePath;
                Platform = primitive.Platform.Name;
                Category = primitive.MainGroup.Name;
                MeshFilenames = meshPaths;
                Flexible = primitive.FlexBones.Any();
                Decorated = meshPaths.Length > 1;
            }
        }

        private class BrickListCache
        {
            [JsonProperty]
            public string PrimitivesPath { get; set; }
            [JsonProperty]
            public string MeshesPath { get; set; }
            [JsonProperty]
            public List<BrickInfo> Bricks { get; set; }
            //[JsonProperty]
            //public DateTime LastUpdate { get; set; }

            public bool ContainsBrick(int id, out BrickInfo foundBrick)
            {
                foundBrick = Bricks.FirstOrDefault(x => x.PartId == id);
                return foundBrick != null;
            }
        }

        #endregion


        private BrickListCache CachedBrickList { get; set; }

        private SortableBindingList<BrickInfo> BrickList { get; }

        private SortableBindingList<BrickInfo> FilteredBrickList { get; set; }

        private bool IsListFiltered;

        private Task BrickLoadingTask;

        private CancellationTokenSource CTS;

        private ConcurrentQueue<BrickInfo> BrickLoadingQueue { get; }

        public BrickInfo SelectedBrick { get; private set; }

        public SelectBrickDialog()
        {
            InitializeComponent();
            BrickList = new SortableBindingList<BrickInfo>();
            //FilteredBrickList = new SortableBindingList<BrickInfo>();
            BrickLoadingQueue = new ConcurrentQueue<BrickInfo>();
            LoadingProgressBar.Visible = false;
            BrickGridView.AutoGenerateColumns = false;
            BrickGridView.DataSource = BrickList;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            GenerateColumnContextMenu();
            SendMessage(SearchTextBox.Handle, EM_SETCUEBANNER, 0, "Search a part");
            
            SettingsManager.Initialize();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            if (LDD.LDDEnvironment.Current == null)
            {
                MessageBox.Show("Cannot fetch LDD bricks. The environment is not configured.", "Error");
                DialogResult = DialogResult.Cancel;
                return;
            }
            else if (!LDD.LDDEnvironment.Current.DatabaseExtracted)
            {
                MessageBox.Show("Cannot fetch LDD bricks. The LDD content (db.lif) is not extracted.", "Error");
                DialogResult = DialogResult.Cancel;
                return;
            }

            StartLoadingBricks();
        }

        #region Brick Loading

        private int TotalBricksToLoad;

        private int TotalBricksLoaded;

        private void StartLoadingBricks()
        {
            LoadingProgressBar.Visible = true;
            LoadingProgressBar.Style = ProgressBarStyle.Marquee;
            TotalBricksLoaded = 0;

            LoadCachedBrickList();

            CTS = new CancellationTokenSource();
            BrickLoadingTask = Task.Factory.StartNew(() => LoadBricksFromDirectory());
            RefreshTimer.Start();
        }

        private void OnBrickLoadingFinished()
        {
            RefreshTimer.Stop();

            SaveLoadedBricks();

            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(1200);
                UpdateProgress(-1);
            });
        }

        private void LoadCachedBrickList()
        {
            string brickListCache = Path.Combine(SettingsManager.AppDataFolder, BRICK_LIST_CACHE_FILENAME);
            if (File.Exists(brickListCache))
            {
                try
                {
                    CachedBrickList = JsonConvert.DeserializeObject<BrickListCache>(File.ReadAllText(brickListCache));
                }
                catch { }
            }
        }

        private void LoadBricksFromDirectory()
        {
            var primitiveDir = LDD.LDDEnvironment.Current.GetAppDataSubDirInfo("db\\Primitives");
            var meshesDir = LDD.LDDEnvironment.Current.GetAppDataSubDirInfo("db\\Primitives\\LOD0");

            var primitiveFiles = primitiveDir.EnumerateFiles("*.xml").ToList();
            //int processed = 0;
            TotalBricksToLoad = primitiveFiles.Count;

            int GetPrimitiveFileID(FileInfo fi)
            {
                string filename = Path.GetFileNameWithoutExtension(fi.Name);
                return int.TryParse(filename, out int pid) ? pid : 9999999;
            }

            foreach (var primitiveFi in primitiveFiles.OrderBy(x => GetPrimitiveFileID(x)))
            {
                if (CTS.IsCancellationRequested)
                    return;
                
                if (int.TryParse(Path.GetFileNameWithoutExtension(primitiveFi.Name), out int partID))
                {
                    var meshFileInfos = meshesDir.EnumerateFiles($"{partID}.g*");

                    DateTime partLastUpdate = primitiveFi.LastWriteTime;

                    foreach(var meshFi in meshFileInfos)
                    {
                        if (meshFi.LastWriteTime > partLastUpdate)
                            partLastUpdate = meshFi.LastWriteTime;
                    }

                    if (CachedBrickList != null &&
                        CachedBrickList.ContainsBrick(partID, out BrickInfo foundBrick))
                    {
                        if (foundBrick.LastUpdate >= partLastUpdate/* &&
                            foundBrick.MeshFilenames.Length == meshFileInfos.Count()*/)
                        {
                            BrickLoadingQueue.Enqueue(foundBrick);
                            continue;
                        }
                    }
                    try
                    {
                        var primitive = Primitive.Load(primitiveFi.FullName);
                        var meshFilenames = meshFileInfos.Select(x => x.Name).ToArray();

                        var brick = new BrickInfo(primitive, primitiveFi.Name, meshFilenames)
                        {
                            LastUpdate = partLastUpdate
                        };

                        BrickLoadingQueue.Enqueue(brick);
                    }
                    catch { }
                    
                }
            }

            if (CachedBrickList != null)
            {
                CachedBrickList.Bricks.Clear();
                CachedBrickList = null;//free memory
            }
            
            GC.Collect();
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

        private void EndLoadingTask()
        {
            if (BrickLoadingTask != null && 
                BrickLoadingTask.Status == TaskStatus.Running)
            {
                CTS.Cancel();
                BrickLoadingTask.Wait(1000);
            }
            
            BrickLoadingTask = null;

            if (CTS != null)
                CTS.Dispose();

            RefreshTimer.Stop();
        }

        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            RefreshTimer.Stop();
            ProcessLoadingQueue();

            if (TotalBricksLoaded >= TotalBricksToLoad)
                OnBrickLoadingFinished();
            else
                RefreshTimer.Start();
        }

        private void ProcessLoadingQueue()
        {
            const int MaxToProcessPerCycle = 150;

            int processed = 0;

            while (BrickLoadingQueue.TryDequeue(out BrickInfo brick))
            {
                BrickList.Add(brick);

                if (IsListFiltered && IsBrickVisible(brick))
                    FilteredBrickList.Add(brick);

                UpdateProgress((int)((++TotalBricksLoaded / (float)TotalBricksToLoad) * 100f));

                if (++processed >= MaxToProcessPerCycle)
                {
                    Application.DoEvents();
                    processed = 0;
                }
            }
        }

        #endregion

        #region Grid Columns Show/Hide Menu

        private void GenerateColumnContextMenu()
        {
            foreach (DataGridViewColumn column in BrickGridView.Columns)
            {
                if (column.DataPropertyName == "PartId")
                    continue;

                var columnMenuItem = new ToolStripMenuItem(column.HeaderText)
                {
                    Tag = column,
                    CheckOnClick = true,
                    Checked = column.Visible
                };
                columnMenuItem.Click += ColumnShowHideMenuItem_Click;
                GridColumnsContextMenu.Items.Add(columnMenuItem);
            }
        }

        private void ColumnShowHideMenuItem_Click(object sender, EventArgs e)
        {
            var columnMenuItem = (ToolStripMenuItem)sender;
            var column = columnMenuItem.Tag as DataGridViewColumn;
            column.Visible = columnMenuItem.Checked;
        }

        private void BrickGridView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var cellRect = BrickGridView.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                var clickPos = new Point(cellRect.X + e.X, cellRect.Y + e.Y);
                GridColumnsContextMenu.Show(BrickGridView, clickPos);
            }
        }

        #endregion

        private void SaveLoadedBricks()
        {
            var primitivePath = Path.Combine(LDD.LDDEnvironment.Current.ApplicationDataPath, "db\\Primitives");
            var meshPath = Path.Combine(LDD.LDDEnvironment.Current.ApplicationDataPath, "db\\Primitives\\LOD0");

            var item = new BrickListCache() 
            { 
                PrimitivesPath = primitivePath,
                MeshesPath = meshPath,
                Bricks = BrickList.ToList(),
            };

            File.WriteAllText(Path.Combine(SettingsManager.AppDataFolder, "BrickList.json"), 
                JsonConvert.SerializeObject(item));
        }

        private void BrickGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (BrickGridView.SelectedRows.Count > 0 &&
                BrickGridView.SelectedRows[0].DataBoundItem is BrickInfo brick)
            {
                SelectedBrick = brick;
            }
            else
                SelectedBrick = null;

            OpenButton.Enabled = SelectedBrick != null;
        }

        private void BrickGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 &&
                BrickGridView.Rows[e.RowIndex].DataBoundItem is BrickInfo brick)
            {
                SelectedBrick = brick;
                DialogResult = DialogResult.OK;
            }
        }

        private const int EM_SETCUEBANNER = 0x1501;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hWnd, int msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)]string lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);


        private void SearchTextBox_TextChanged(object sender, EventArgs e)
        {
            ApplySearchFilter();
        }

        private void ApplySearchFilter()
        {
            if (string.IsNullOrWhiteSpace(SearchTextBox.Text) && IsListFiltered)
            {
                if (FilteredBrickList!= null && FilteredBrickList.IsSorted)
                    BrickList.ApplySort(FilteredBrickList.PropertyDescriptor, FilteredBrickList.SortDirection);

                BrickGridView.DataSource = BrickList;
                FilteredBrickList.Clear();
                IsListFiltered = false;
                return;
            }

            FilteredBrickList = new SortableBindingList<BrickInfo>(
                BrickList.Where(x => IsBrickVisible(x)).ToList());

            if (BrickList.IsSorted)
                FilteredBrickList.ApplySort(BrickList.PropertyDescriptor, BrickList.SortDirection);

            BrickGridView.DataSource = FilteredBrickList;
            IsListFiltered = true;
        }

        private bool IsBrickVisible(BrickInfo b)
        {
            string filterText = SearchTextBox.Text;
            if (string.IsNullOrWhiteSpace(filterText))
                return true;

            return
                b.Description.ToUpperInvariant().Contains(filterText.ToUpperInvariant()) ||
                b.Category.ToUpperInvariant().Contains(filterText.ToUpperInvariant()) ||
                b.PartId.ToString().Contains(filterText);
        }

        private void SelectBrickDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (BrickLoadingTask != null && 
                BrickLoadingTask.Status == TaskStatus.Running)
            {
                EndLoadingTask();
            }
        }
    }
}
