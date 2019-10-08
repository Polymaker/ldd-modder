using LDDModder.LDD.Files;
using LDDModder.LifExtractor.Models;
using LDDModder.LifExtractor.Utilities;
using LDDModder.LifExtractor.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LDDModder.LifExtractor.Windows
{
    public partial class LifViewerWindow : Form
    {
        private LifFile CurrentFile;

        private LifFile.FolderEntry CurrentFolder;

        private bool IsLoadingTreeView;

        private List<ILifItemInfo> CurrentFolderItems;

        private List<LifFolderInfo> CurrentLifFolders;

        public LifViewerWindow()
        {
            InitializeComponent();
            BackHistory = new List<LifFile.FolderEntry>();
            FwrdHistory = new List<LifFile.FolderEntry>();
            CurrentFolderItems = new List<ILifItemInfo>();
            CurrentLifFolders = new List<LifFolderInfo>();
            NativeMethods.SetWindowTheme(LifTreeView.Handle, "Explorer", null);

            ToolBarFolderCombo.ComboBox.DrawMode = DrawMode.OwnerDrawVariable;
            ToolBarFolderCombo.ComboBox.DrawItem += ComboBox_DrawItem;
            ToolBarFolderCombo.ComboBox.SelectedIndexChanged += ComboBox_SelectedIndexChanged;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            LoadFileTypesIcons();

            InitializeFolderListView();

            ToolBarFolderCombo.ComboBox.ItemHeight = LifTreeView.ItemHeight;
        }

        #region TreeView & ListView Icon Handling

        private List<string> UnknownFileExtensions = new List<string>();

        private void LoadFileTypesIcons()
        {
            SmallIconImageList.Images.Add("folder", Properties.Resources.Folder_16x16);
            LargeIconImageList.Images.Add("folder", Properties.Resources.Folder_32x32);

            var commonExtensions = new string[] { ".xml", ".png", ".lxfml", ".unknown" };

            foreach (var fileExtension in commonExtensions)
            {
                var smallIcon = FileTypeInfoHelper.GetFileTypeIconSmall($"text{fileExtension}");
                var largeIcon = FileTypeInfoHelper.GetFileTypeIconLarge($"text{fileExtension}");
                SmallIconImageList.Images.Add(fileExtension.ToUpper(), smallIcon.ToBitmap());
                LargeIconImageList.Images.Add(fileExtension.ToUpper(), largeIcon.ToBitmap());
                smallIcon.Dispose();
                largeIcon.Dispose();
            }
        }

        private void SetFileTypeIcon(LifFileInfo fileInfo)
        {
            if (SmallIconImageList.Images.ContainsKey(fileInfo.FileType))
                return;

            if (UnknownFileExtensions.Contains(fileInfo.FileType))
            {
                fileInfo.ItemImageKey = ".UNKNOWN";
                return;
            }

            var smallIcon = FileTypeInfoHelper.GetFileTypeIconSmall($"text{fileInfo.FileType}", out bool isKnownExt);

            if (!isKnownExt)
            {
                UnknownFileExtensions.Add(fileInfo.FileType);
                smallIcon.Dispose();
                fileInfo.ItemImageKey = ".UNKNOWN";
                return;
            }

            var largeIcon = FileTypeInfoHelper.GetFileTypeIconLarge($"text{fileInfo.FileType}");
            SmallIconImageList.Images.Add(fileInfo.FileType, smallIcon.ToBitmap());
            LargeIconImageList.Images.Add(fileInfo.FileType, largeIcon.ToBitmap());
            smallIcon.Dispose();
            largeIcon.Dispose();
        }

        #endregion

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            AdjustFolderComboWidth();
        }

        private void FileOpenMenuItem_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "LDD Lif files (*.lif)|*.lif|All files|*.*";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    if (Path.GetExtension(ofd.FileName).ToUpper() != ".LIF")
                    {
                        MessageBox.Show("Invalid Lif File.");
                        return;
                    }

                    try
                    {
                        var file = LifFile.Open(ofd.FileName);
                        OpenLifFile(file);
                    }
                    catch
                    {

                    }
                }
            }
        }

        private void OpenLifFile(LifFile file)
        {
            LifTreeView.Nodes.Clear();
            FolderListView.DataSource = null;
            ToolBarFolderCombo.ComboBox.DataSource = null;
            CurrentFolderItems.Clear();

            CurrentFile = file;
            CurrentFolder = null;
            FileMenu_ExtractItem.Enabled = !string.IsNullOrEmpty(file.FilePath);
            CurrentFileStripLabel.Text = file?.FilePath;
            
            FillTreeView();

            ToolBarFolderCombo.ComboBox.DisplayMember = "FullName";
            ToolBarFolderCombo.ComboBox.DataSource = CurrentLifFolders;

            NavigateToFolder(file.RootFolder);
        }

        #region TreeView Handling

        private void FillTreeView()
        {
            IsLoadingTreeView = true;
            LifTreeView.Nodes.Clear();
            CurrentLifFolders.Clear();

            string lifRootName = !string.IsNullOrEmpty(CurrentFile.FilePath) ? 
                Path.GetFileNameWithoutExtension(CurrentFile.FilePath) : "LIF";

            void AddFolderNodes(LifFile.FolderEntry folder, TreeNode parentNode)
            {
                var folderInfo = new LifFolderInfo(folder) { LifName = lifRootName };
                CurrentLifFolders.Add(folderInfo);

                var node = new TreeNode
                {
                    Name = folder.Fullname,
                    Text = folderInfo.Name,
                    Tag = folder
                };

                if (parentNode == null)
                    LifTreeView.Nodes.Add(node);
                else
                    parentNode.Nodes.Add(node);
                foreach (var subFolder in folder.Folders)
                    AddFolderNodes(subFolder, node);
            }

            AddFolderNodes(CurrentFile.RootFolder, null);

            LifTreeView.Nodes[0].Expand();

            IsLoadingTreeView = false;
        }

        private void LifTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (IsLoadingTreeView)
                return;

            if (e.Node.Tag is LifFile.FolderEntry selectedFolder && selectedFolder != CurrentFolder)
                NavigateToFolder(selectedFolder);
        }

        private TreeNode FindFolderNode(LifFile.FolderEntry folder)
        {
            var result = LifTreeView.Nodes.Find(folder.Fullname, true);
            return result?.Length > 0 ? result[0] : null;
        }

        private void SelectFolderNode(LifFile.FolderEntry folder)
        {
            LifTreeView.AfterSelect -= LifTreeView_AfterSelect;
            LifTreeView.SelectedNode = FindFolderNode(folder);
            LifTreeView.AfterSelect += LifTreeView_AfterSelect;
        }

        #endregion

        #region ListView Handling

        private void InitializeFolderListView()
        {
            FlvSizeColumn.AspectToStringConverter = (object cellValue) =>
            {
                if (cellValue is long fileSize)
                    return NativeMethods.FormatFileSize(fileSize);
                return string.Empty;
            };

            FolderListView.RowHeight = FolderListView.RowHeightEffective + 4;

            NativeMethods.SendMessage(FolderListView.Handle,
                NativeMethods.WM_CHANGEUISTATE,
                NativeMethods.MakeWord(NativeMethods.UIS_SET, NativeMethods.UISF_HIDEFOCUS), 0);

            FlvModifiedColumn.IsVisible = false;
            FolderListView.RebuildColumns();
        }

        private void FillFolderListView()
        {
            FolderListView.DataSource = null;
            CurrentFolderItems.Clear();
            
            if (CurrentFolder != null)
            {
                foreach (var entry in CurrentFolder.Entries
                    .OrderBy(x => x is LifFile.FileEntry))
                {
                    if (entry is LifFile.FileEntry file)
                    {
                        var fileInfo = new LifFileInfo(file)
                        {
                            Description = FileTypeInfoHelper.GetFileTypeDescription(file.Name)
                        };
                        SetFileTypeIcon(fileInfo);
                        CurrentFolderItems.Add(fileInfo);
                    }
                    else if (entry is LifFile.FolderEntry folder)
                    {
                        CurrentFolderItems.Add(new LifFolderInfo(folder));
                    }
                }

                FolderListView.DataSource = CurrentFolderItems;
                FolderListView.SelectedIndex = -1;
            }
        }

        private void FolderListView_BeforeSorting(object sender, BrightIdeasSoftware.BeforeSortingEventArgs e)
        {
            if (e.ColumnToSort != null && FolderListView.PrimarySortColumn == e.ColumnToSort)
            {
                if (e.SortOrder == SortOrder.Ascending)
                {
                    e.Canceled = true;
                    BeginInvoke((Action)(() =>
                    {
                        FolderListView.Sort(FlvTypeColumn, SortOrder.Ascending);
                        FolderListView.PrimarySortOrder = SortOrder.None;
                        FolderListView.PrimarySortColumn = null;
                    }));
                }
            }
        }

        private void FolderListView_ItemActivate(object sender, EventArgs e)
        {
            if (FolderListView.SelectedItem != null)
            {
                if (FolderListView.SelectedItem.RowObject is LifFolderInfo folderInfo)
                {
                    NavigateToFolder(folderInfo.Folder);
                }
            }
        }

        #endregion

        #region Folder ComboBox Handling

        private void AdjustFolderComboWidth()
        {
            var remainingWidth = NavigationToolStrip.Width -
                NavigationToolStrip.Padding.Horizontal - 3;

            if (NavigationToolStrip.OverflowButton.Visible)
            {
                remainingWidth -= NavigationToolStrip.OverflowButton.Width +
                    NavigationToolStrip.OverflowButton.Margin.Horizontal;
            }

            foreach (ToolStripItem item in NavigationToolStrip.Items)
            {
                if (item.IsOnOverflow || item == ToolBarFolderCombo)
                    continue;
                remainingWidth -= item.Width + item.Margin.Horizontal;
            }

            ToolBarFolderCombo.Width = remainingWidth - ToolBarFolderCombo.Margin.Horizontal;
        }

        private void SelectFolderCombo(LifFile.FolderEntry folder)
        {
            var foundFolder = CurrentLifFolders.FirstOrDefault(x => x.Folder == folder);
            ToolBarFolderCombo.ComboBox.SelectedIndexChanged -= ComboBox_SelectedIndexChanged;
            ToolBarFolderCombo.SelectedItem = foundFolder;
            ToolBarFolderCombo.ComboBox.SelectedIndexChanged += ComboBox_SelectedIndexChanged;
        }

        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CurrentFolder != null && ToolBarFolderCombo.SelectedItem is LifFolderInfo folderInfo)
                NavigateToFolder(folderInfo.Folder);
        }

        private void ComboBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
                return;
            e.DrawBackground();

            var folderInfo = ToolBarFolderCombo.Items[e.Index] as LifFolderInfo;
            var folderImage = SmallIconImageList.Images["folder"];

            var xOffset = folderInfo.Entry.GetLevel() * LifTreeView.Indent;
            var yOffset = (int)((e.Bounds.Height - folderImage.Height) / 2f);

            e.Graphics.DrawImage(folderImage, e.Bounds.X + xOffset, e.Bounds.Y + yOffset);

            var textBounds = new RectangleF(e.Bounds.X + xOffset + folderImage.Width + 3, e.Bounds.Y, 
                e.Bounds.Width, e.Bounds.Height);
            textBounds.Width -= (textBounds.X - e.Bounds.X);

            using (var brush = new SolidBrush(e.ForeColor))
            using (var sf = new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center })
            {
                e.Graphics.DrawString(folderInfo.Name, e.Font, brush, textBounds, sf);
            }
        }

        #endregion

        #region Navigation Handling

        private List<LifFile.FolderEntry> BackHistory;

        private List<LifFile.FolderEntry> FwrdHistory;

        private void NavigateToFolder(LifFile.FolderEntry folder, bool fromHistory = false)
        {
            var oldFolder = CurrentFolder;
            CurrentFolder = folder;

            SelectFolderNode(folder);
            SelectFolderCombo(folder);

            if (oldFolder != null && !fromHistory)
                BackHistory.Add(oldFolder);

            if (!fromHistory)
                FwrdHistory.Clear();

            UpdateNavigationButtons();

            FillFolderListView();

            if (folder != null && folder.Folders.Contains(oldFolder))
            {
                var foundItem = CurrentFolderItems.OfType<LifFolderInfo>().FirstOrDefault(x => x.Folder == oldFolder);
                FolderListView.SelectedObjects.Clear();
                FolderListView.SelectObject(foundItem);
                //FolderListView.SelectedItem = foundItem;
            }
        }

        private void UpdateNavigationButtons()
        {
            BackToolbarButton.Enabled = BackHistory.Count > 0;
            NextToolbarButton.Enabled = FwrdHistory.Count > 0;
            UpToolbarButton.Enabled = CurrentFolder?.Parent != null;
        }

        private void BackToolbarButton_Click(object sender, EventArgs e)
        {
            if (BackHistory.Count > 0)
            {
                var previousFolder = BackHistory.Last();
                BackHistory.RemoveAt(BackHistory.Count - 1);
                FwrdHistory.Add(CurrentFolder);
                NavigateToFolder(previousFolder, true);
            }
        }

        private void NextToolbarButton_Click(object sender, EventArgs e)
        {
            if (FwrdHistory.Count > 0)
            {
                var nextFolder = FwrdHistory.Last();
                FwrdHistory.RemoveAt(FwrdHistory.Count - 1);
                BackHistory.Add(CurrentFolder);
                NavigateToFolder(nextFolder, true);
            }
        }

        private void UpToolbarButton_Click(object sender, EventArgs e)
        {
            if (CurrentFolder?.Parent != null)
                NavigateToFolder(CurrentFolder.Parent);
        }

        

        #endregion

        private void ViewModeMenuItems_Click(object sender, EventArgs e)
        {
            if (sender == ViewModeDetailsMenuItem)
                SetListViewMode(View.Details);
            else if (sender == ViewModeSmallMenuItem)
                SetListViewMode(View.SmallIcon);
            else if (sender == ViewModeLargeMenuItem)
                SetListViewMode(View.LargeIcon);
        }

        private void SetListViewMode(View viewMode)
        {
            FolderListView.View = viewMode;
            ViewModeDetailsMenuItem.Checked = false;
            ViewModeSmallMenuItem.Checked = false;
            ViewModeLargeMenuItem.Checked = false;
            switch (viewMode)
            {
                case View.Details:
                    ViewModeDetailsMenuItem.Checked = true;
                    break;
                case View.SmallIcon:
                    ViewModeSmallMenuItem.Checked = true;
                    break;
                case View.LargeIcon:
                    ViewModeLargeMenuItem.Checked = true;
                    break;
            }
        }

        private void FolderListContextMenu_Opening(object sender, CancelEventArgs e)
        {
            if (FolderListView.SelectedObjects.Count == 0)
            {
                e.Cancel = true;
                return;
            }

            ListMenu_OpenItem.Enabled = FolderListView.SelectedObjects.Count == 1;
        }

        private void ListMenu_OpenItem_Click(object sender, EventArgs e)
        {
            if (FolderListView.SelectedObject is LifFileInfo fileInfo)
            {
                string tmpDir = GetTmpExtractionDirectory();
                string tmpPath = Path.Combine(tmpDir, fileInfo.Name);
                try
                {
                    fileInfo.File.ExtractToDirectory(tmpDir);
                    Process.Start(tmpPath);
                }
                catch
                {

                }
                
            }
        }

        private string GetTmpExtractionDirectory()
        {
            string tmpFolderName = "LIF" + LDDModder.Utilities.StringUtils.GenerateUID(8);
            return Path.Combine(Path.GetTempPath(), tmpFolderName);
        }

        private void ListMenu_ExtractItem_Click(object sender, EventArgs e)
        {
            var itemsToExtract = FolderListView.SelectedObjects
                .Cast<ILifItemInfo>().Select(x => x.Entry).ToList();

            if (itemsToExtract.Any())
            {
                using (var eid = new ExtractItemsDialog())
                {
                    //eid.TargetDirectory = Path.GetDirectoryName(CurrentFile.FilePath);
                    eid.ItemsToExtract.AddRange(itemsToExtract);
                    eid.ShowDialog();
                }
            }
        }

        private void FileMenu_ExtractItem_Click(object sender, EventArgs e)
        {
            using (var eid = new ExtractItemsDialog())
            {
                if (!string.IsNullOrEmpty(CurrentFile.FilePath))
                    eid.SelectedDirectory = Path.GetDirectoryName(CurrentFile.FilePath);
                eid.ItemsToExtract.Add(CurrentFile.RootFolder);
                eid.ShowDialog();
            }
        }


        private void FolderListView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            var selectedItems = FolderListView.SelectedObjects
                .Cast<ILifItemInfo>().Select(x => x.Entry).ToList();

            if (selectedItems.Any())
            {
                string tmpDirectory = GetTmpExtractionDirectory();

                var filenames = selectedItems.Select(x => Path.Combine(tmpDirectory, x.Name)).ToArray();

                using (var cancelSource = new CancellationTokenSource())
                {
                    var dropHelper = new FileDropHelper(filenames);
                    
                    Task extractionThread = null;

                    dropHelper.GetDataOverride = (frm, auto) =>
                    {
                        if (extractionThread == null)
                        {
                            extractionThread = Task.Factory.StartNew(() =>
                            {
                                try
                                {
                                    LifFile.ExtractEntries(selectedItems, tmpDirectory, cancelSource.Token, null);
                                }
                                catch { }
                            });

                        }
                        return filenames;
                    };

                    var result = FolderListView.DoDragDrop(dropHelper, DragDropEffects.Copy);

                    if (extractionThread != null && !extractionThread.IsCompleted)
                        cancelSource.Cancel();
                }

                try
                {
                    NativeMethods.DeleteFileOrFolder(tmpDirectory);
                }
                catch { }
            }
        }

        
    }
}
