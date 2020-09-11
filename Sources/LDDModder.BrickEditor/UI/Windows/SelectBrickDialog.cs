﻿using LDDModder.BrickEditor.Models;
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
        private SortableBindingList<BrickInfo> BrickList { get; }

        private SortableBindingList<BrickInfo> FilteredBrickList { get; set; }

        private bool IsListFiltered;

        private Task BrickLoadingTask;

        private CancellationTokenSource CTS;

        public BrickInfo SelectedBrick { get; private set; }

        public SelectBrickDialog()
        {
            InitializeComponent();
            BrickList = new SortableBindingList<BrickInfo>();
            LoadingProgressBar.Visible = false;
            BrickGridView.AutoGenerateColumns = false;
            BrickGridView.DataSource = BrickList;
            BrickGridView.Sort(PartIdColumn, ListSortDirection.Ascending);
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

        private void StartLoadingBricks()
        {
            if (BrickListCache.IsCacheDirty ||
                BrickListCache.IsCacheEmpty)
            {
                LoadingProgressBar.Visible = true;
                LoadingProgressBar.Style = ProgressBarStyle.Marquee;
                //TotalBricksLoaded = 0;
                CTS = new CancellationTokenSource();
                BrickLoadingTask = Task.Factory.StartNew(() => 
                    BrickListCache.RebuildCache(CTS.Token)
                    
                );
                RefreshTimer.Start();
            }
            
            if (!BrickListCache.IsCacheEmpty)
            {
                BrickList.Clear();
                FillValidatedParts(BrickLoadingTask != null);
            }
        }

        private void FillValidatedParts(bool onlyOnce = false)
        {
            var validBricks = BrickListCache.GetValidatedBricks();
            var newBricks = validBricks.Except(BrickList)
                .OrderBy(x=>x.PartId).ToList();

            const int MAX_ADD = 200;

            //add items in batches to prevent freezing the window
            while (newBricks.Count > 0)
            {
                var range = newBricks.Take(MAX_ADD).ToList();
                var curScrollRow = BrickGridView.FirstDisplayedScrollingRowIndex;

                BrickList.AddRange(range);
                if (IsListFiltered)
                    FilteredBrickList.AddRange(range.Where(x => IsBrickVisible(x)));

                if (curScrollRow >= 0 && curScrollRow != BrickGridView.FirstDisplayedScrollingRowIndex)
                {
                    //fix to prevent scrolling back to first row
                    BrickGridView.FirstDisplayedScrollingRowIndex = curScrollRow;
                }

                newBricks.RemoveRange(0, range.Count);

                if (onlyOnce)
                    break;

                if (newBricks.Count > 0)
                    Application.DoEvents();
            }

        }

        private void OnBrickLoadingFinished()
        {
            RefreshTimer.Stop();
            FillValidatedParts(false);

            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(1200);
                UpdateProgress(-1);
            });
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

            if (BrickListCache.CurrentProgress != null)
            {
                UpdateProgress((int)(BrickListCache.CurrentProgress.Percent * 100));
                FillValidatedParts(true);

                if (BrickListCache.CurrentProgress.Percent >= 1)
                {
                    OnBrickLoadingFinished();
                    return;
                }
            }

            RefreshTimer.Start();
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
