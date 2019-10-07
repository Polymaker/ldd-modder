using LDDModder.LDD.Files;
using LDDModder.LifExtractor.Models;
using LDDModder.LifExtractor.Utilities;
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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LDDModder.LifExtractor
{
    public partial class LifViewerWindow : Form
    {
        private LifFile CurrentFile;

        private LifFile.FolderEntry CurrentFolder;

        private bool IsLoadingTreeView;

        private List<ILifItemInfo> CurrentFolderItems;

        public LifViewerWindow()
        {
            InitializeComponent();
            BackHistory = new List<LifFile.FolderEntry>();
            FwrdHistory = new List<LifFile.FolderEntry>();
            CurrentFolderItems = new List<ILifItemInfo>();
            NativeMethods.SetWindowTheme(LifTreeView.Handle, "Explorer", null);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            LoadFileTypesIcons();

            InitializeFolderListView();

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
            CurrentFolderItems.Clear();

            CurrentFile = file;
            FileMenu_ExtractItem.Enabled = true;
            CurrentFileStripLabel.Text = file.FilePath;

            FillTreeView();

            NavigateToFolder(file.RootFolder);
        }

        #region TreeView Handling

        private void FillTreeView()
        {
            IsLoadingTreeView = true;
            LifTreeView.Nodes.Clear();

            string rootName = Path.GetFileNameWithoutExtension(CurrentFile.FilePath);

            void AddFolderNodes(LifFile.FolderEntry folder, TreeNode parentNode)
            {
                var node = new TreeNode
                {
                    Name = folder.Fullname,
                    Text = folder.IsRootDirectory ? rootName : folder.Name,
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
                foreach (var entry in CurrentFolder.Entries.OrderBy(x => x is LifFile.FileEntry))
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

        #region Navigation Handling

        private List<LifFile.FolderEntry> BackHistory;

        private List<LifFile.FolderEntry> FwrdHistory;

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

        private void NavigateToFolder(LifFile.FolderEntry folder, bool fromHistory = false)
        {
            var oldFolder = CurrentFolder;
            CurrentFolder = folder;

            SelectFolderNode(folder);

            if (oldFolder != null && !fromHistory)
                BackHistory.Add(oldFolder);

            if (folder != null)
            {
                string rootName = Path.GetFileNameWithoutExtension(CurrentFile.FilePath);
                ToolBarFolderCombo.Text = Path.Combine(rootName, folder.Fullname);

                
            }

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
                string tmpPath = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(Path.GetRandomFileName()), fileInfo.Name);
                Directory.CreateDirectory(Path.GetDirectoryName(tmpPath));
                fileInfo.File.ExtractTo(tmpPath);
                Process.Start(tmpPath);
            }
        }

        private void FolderListView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (e.Item is BrightIdeasSoftware.OLVListItem listItem && listItem.RowObject is LifFileInfo fileInfo)
            {
                string tmpPath = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(Path.GetRandomFileName()), fileInfo.Name);
                Directory.CreateDirectory(Path.GetDirectoryName(tmpPath));
                fileInfo.File.ExtractTo(tmpPath);
                var files = new string[] { tmpPath };
                FolderListView.DoDragDrop(
                    new DataObject(DataFormats.FileDrop, files), 
                    DragDropEffects.Copy);
            }
            
        }
    }
}
