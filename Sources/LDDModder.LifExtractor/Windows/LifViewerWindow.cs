using BrightIdeasSoftware;
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
        private bool IsNewLif;
        private LifFile.FolderEntry CurrentFolder;

        private bool IsLoadingTreeView;

        private SortableBindingList<ILifItemInfo> CurrentFolderItems;

        private SortableBindingList<LifFolderInfo> CurrentLifFolders;

        private bool LifEditingEnabled { get; set; }

        public int MAX_HISTORY = 25;

        private bool IsOpeningFile;

        public LifViewerWindow()
        {
            InitializeComponent();
            BackHistory = new List<LifFile.FolderEntry>();
            FwrdHistory = new List<LifFile.FolderEntry>();
            CurrentFolderItems = new SortableBindingList<ILifItemInfo>();
            CurrentFolderItems.ListChanged += CurrentFolderItems_ListChanged;
            CurrentLifFolders = new SortableBindingList<LifFolderInfo>();
            CurrentLifFolders.ApplySort("FullName", ListSortDirection.Ascending);

            NativeMethods.SetWindowTheme(LifTreeView.Handle, "Explorer", null);


            ToolBarFolderCombo.ComboBox.DisplayMember = "FullName";
            ToolBarFolderCombo.ComboBox.DrawMode = DrawMode.OwnerDrawVariable;
            ToolBarFolderCombo.ComboBox.DrawItem += ComboBox_DrawItem;
            ToolBarFolderCombo.ComboBox.SelectedIndexChanged += ComboBox_SelectedIndexChanged;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Icon = Properties.Resources.LIF_File_Icon;

            LoadFileTypesIcons();

            InitializeFolderListView();
            UpdateMenuItems();

            ToolBarFolderCombo.ComboBox.ItemHeight = LifTreeView.ItemHeight;
            ActionsMenu_SaveLif.DefaultItem = SaveMenu_Save;
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
            {
                fileInfo.ItemImageKey = fileInfo.FileType;
                return;
            }

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

            FolderListView.SmallImageList = SmallIconImageList;
            FolderListView.LargeImageList = LargeIconImageList;

            fileInfo.ItemImageKey = fileInfo.FileType;
        }

        #endregion

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            AdjustFolderComboWidth();
        }

        private void LoadLifFile(LifFile file)
        {
            
            LifTreeView.Nodes.Clear();
            FolderListView.DataSource = null;
            FolderListView.ClearObjects();
            ToolBarFolderCombo.ComboBox.DataSource = null;
            CurrentFolderItems.Clear();

            if (CurrentFile != null && CurrentFile != file)
            {
                CurrentFile.Dispose();
                CurrentFile = null;
            }

            CurrentFile = file;
            CurrentFolder = null;
            IsNewLif = string.IsNullOrEmpty(file?.FilePath);
 
            CurrentFileStripLabel.Text = file?.FilePath;

            if (file != null)
            {
                FillTreeView();
                NavigateToFolder(file.RootFolder);
                if (IsNewLif)
                    EnableLifEditing();
            }

            UpdateMenuItems();
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

                var node = CreateFolderTreeNode(folderInfo);

                if (parentNode == null)
                    LifTreeView.Nodes.Add(node);
                else
                    parentNode.Nodes.Add(node);
                foreach (var subFolder in folder.Folders)
                    AddFolderNodes(subFolder, node);
            }
            
            AddFolderNodes(CurrentFile.RootFolder, null);
            ToolBarFolderCombo.ComboBox.DataSource = CurrentLifFolders;

            LifTreeView.Nodes[0].Expand();

            IsLoadingTreeView = false;
        }

        private TreeNode CreateFolderTreeNode(LifFolderInfo folderInfo)
        {
            return new TreeNode
            {
                Name = folderInfo.Folder.FullName,
                Text = folderInfo.Name,
                Tag = folderInfo
            };
        }

        private void LifTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (IsLoadingTreeView)
                return;

            if (e.Node.Tag is LifFolderInfo selectedFolder && 
                selectedFolder.Folder != CurrentFolder)
                NavigateToFolder(selectedFolder.Folder);
        }

        private TreeNode FindFolderNode(LifFile.FolderEntry folder)
        {
            var result = LifTreeView.Nodes.Find(folder.FullName, true);
            return result?.Length > 0 ? result[0] : null;
        }

        private void SelectFolderNode(LifFile.FolderEntry folder)
        {
            LifTreeView.AfterSelect -= LifTreeView_AfterSelect;
            LifTreeView.SelectedNode = FindFolderNode(folder);
            LifTreeView.AfterSelect += LifTreeView_AfterSelect;
        }

        private void LifTreeView_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Node.Tag is LifFolderInfo selectedFolder)
            {
                if (selectedFolder.IsRootDirectory)
                    e.CancelEdit = true;
            }
            else
                e.CancelEdit = true;
        }

        private void LifTreeView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Node.Tag is LifFolderInfo selectedFolder)
            {

                if (!string.IsNullOrEmpty(e.Label) &&
                    selectedFolder.Folder.ValidateRename(e.Label, false))
                {
                    selectedFolder.Folder.Rename(e.Label);
                    e.Node.Name = selectedFolder.FullName;
                }
                else
                    e.CancelEdit = true;
            }
        }

        private void LifTreeView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (!LifEditingEnabled)
                return;

            if (e.Item is TreeNode node)
            {
                var draggedDir = node.Tag as LifFolderInfo;
                DoDragDrop(node, DragDropEffects.Move);
            }
        }

        private bool IsTreeNodeDrag(DragEventArgs e)
        {
            if (e.Data is DataObject dataObj)
                return dataObj.GetDataPresent(typeof(TreeNode));
            return false;
        }

        private LifFolderInfo GetDraggedTreeFolder(DragEventArgs e)
        {
            if (e.Data is DataObject dataObj)
            {
                var draggedNode = (TreeNode)dataObj.GetData(typeof(TreeNode));
                //draggedNode = LifTreeView.Nodes.Find(draggedNode.Name, true).FirstOrDefault();
                return draggedNode.Tag as LifFolderInfo;
            }
            return null;
        }

        private void LifTreeView_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data is OLVDataObject || IsTreeNodeDrag(e))
            {
                e.Effect = DragDropEffects.Move;
            }
        }

        private void LifTreeView_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data is OLVDataObject || IsTreeNodeDrag(e))
            {
                Point targetPoint = LifTreeView.PointToClient(new Point(e.X, e.Y));
                var targetNode = LifTreeView.GetNodeAt(targetPoint);
                //var targetDir = LifTreeView.SelectedNode.Tag as LifFolderInfo;

                //if (targetDir == null)
                //{
                //    e.Effect = DragDropEffects.None;
                //}
                //else
                //{

                //}
                //var draggedFolders = olvObj.ModelObjects.OfType<LifFolderInfo>();
                
                LifTreeView.AfterSelect -= LifTreeView_AfterSelect;
                LifTreeView.SelectedNode = LifTreeView.GetNodeAt(targetPoint);
                LifTreeView.AfterSelect += LifTreeView_AfterSelect;
            }
        }

        private void LifTreeView_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data is OLVDataObject olvObj && LifTreeView.SelectedNode != null)
            {
                var draggedItems = olvObj.ModelObjects.OfType<ILifItemInfo>();
                var entries = draggedItems.Select(x => x.Entry).ToList();
                var targetDir = LifTreeView.SelectedNode.Tag as LifFolderInfo;
                MoveLifEntries(entries, targetDir.Folder);
            }
            else if (IsTreeNodeDrag(e))
            {
                var draggedFolder = GetDraggedTreeFolder(e);
                var targetDir = LifTreeView.SelectedNode.Tag as LifFolderInfo;
                MoveLifEntries(new LifFile.LifEntry[] { draggedFolder.Folder }, targetDir.Folder);
            }
        }

        private void LifTreeView_DragLeave(object sender, EventArgs e)
        {
            if (CurrentFolder != null)
                SelectFolderNode(CurrentFolder);
        }

        private IEnumerable<ILifItemInfo> GetSelectedTreeViewItems()
        {
            if (LifTreeView.SelectedNode?.Tag is LifFolderInfo folderInfo)
                yield return folderInfo;

            yield break;
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

            FolderListView.SecondarySortOrder = SortOrder.Ascending;
            FolderListView.SecondarySortColumn = FlvNameColumn;
        }

        private void FillFolderListView()
        {
            FolderListView.DataSource = null;
            FolderListView.ClearObjects();

            CurrentFolderItems.Clear();
            Application.DoEvents();

            if (CurrentFolder != null)
            {
                toolStripProgressBar1.Visible = true;
                
                //int totalAdded = 0;
                //const int MAX_ITEMS = 500;
                //if (CurrentFolder.Entries.Count > MAX_ITEMS)
                //{
                //    CurrentFolderItems.RaiseListChangedEvents = false;
                //    FolderListView.DataSource = CurrentFolderItems;
                //}

                foreach (var entry in CurrentFolder.Entries
                    .OrderBy(x => x is LifFile.FileEntry))
                {
                    //bool triggerUpdate = false;
                    //totalAdded++;

                    //if ((totalAdded % MAX_ITEMS) == 0 || 
                    //    totalAdded == CurrentFolder.Entries.Count)
                    //{
                    //    CurrentFolderItems.RaiseListChangedEvents = true;
                    //    triggerUpdate = true;
                    //}

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

                    //if (triggerUpdate)
                    //{
                    //    CurrentFolderItems.RaiseListChangedEvents = false;
                    //    Application.DoEvents();
                    //}
                }

                //CurrentFolderItems.RaiseListChangedEvents = true;
                //if (CurrentFolder.Entries.Count <= MAX_ITEMS)
                    FolderListView.DataSource = CurrentFolderItems;
                
                FolderListView.SelectedIndex = -1;
                toolStripProgressBar1.Visible = false;
            }
        }

        private bool CurrentItemsChanged;

        private void CurrentFolderItems_ListChanged(object sender, ListChangedEventArgs e)
        {
            CurrentItemsChanged = true;
        }

        private void FolderListView_BeforeSorting(object sender, BrightIdeasSoftware.BeforeSortingEventArgs e)
        {
            if (CurrentItemsChanged)
            {
                CurrentItemsChanged = false;
                return;
            }

            if (e.ColumnToSort != null && FolderListView.PrimarySortColumn == e.ColumnToSort)
            {
                if (e.SortOrder == SortOrder.Ascending)
                {
                    e.Canceled = true;

                    BeginInvoke((Action)(() =>
                    {
                        FolderListView.PrimarySortOrder = SortOrder.None;
                        FolderListView.PrimarySortColumn = null;
                        
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

        private void FolderListView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            var selectedEntries = GetSelectedListViewItems();

            if (!LifEditingEnabled && selectedEntries.Any())
                ExtractDraggedEntries(selectedEntries.Select(x => x.Entry).ToList());
        }

        private void FolderListView_CanDrop(object sender, BrightIdeasSoftware.OlvDropEventArgs e)
        {
            if (!LifEditingEnabled)
            {
                e.Effect = DragDropEffects.None;
                e.Handled = true;
                return;
            }

            if (e.DataObject is BrightIdeasSoftware.OLVDataObject olvObj && olvObj.ListView == FolderListView)
            {
                if (e.DropTargetItem?.RowObject is LifFolderInfo targetDir)
                {
                    e.Effect = DragDropEffects.Move;
                    e.InfoMessage = $"Move into {targetDir.Name}";
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                }
            }
            else if (e.DataObject is DataObject dataObj && dataObj.ContainsFileDropList())
            {
                e.Effect = DragDropEffects.Copy;

                if (e.DropTargetItem?.RowObject is LifFolderInfo targetDir)
                    e.InfoMessage = $"Add file(s) into {targetDir.Name}";
                else if (!CurrentFolder.IsRootDirectory)
                    e.InfoMessage = $"Add file(s) into {CurrentFolder.Name}";
                else
                    e.InfoMessage = $"Add file(s)";
            }
        }

        private void FolderListView_Dropped(object sender, BrightIdeasSoftware.OlvDropEventArgs e)
        {
            if (e.DataObject is BrightIdeasSoftware.OLVDataObject olvObj && olvObj.ListView == FolderListView)
            {
                if (e.DropTargetItem?.RowObject is LifFolderInfo targetDir)
                {
                    var draggedItems = olvObj.ModelObjects.OfType<ILifItemInfo>();
                    var entries = draggedItems.Select(x => x.Entry).ToList();

                    MoveLifEntries(entries, targetDir.Folder);
                    //foreach (var entry in entries)
                    //    CurrentFolder.Entries.Remove(entry);
                    //targetDir.Folder.Entries.AddRange(entries);

                    //FillTreeView();
                    //FillFolderListView();
                }
            }
            else if (e.DataObject is DataObject dataObj && dataObj.ContainsFileDropList())
            {
                
            }
        }

        private void ExtractDraggedEntries(IEnumerable<LifFile.LifEntry> entries)
        {
            string tmpDirectory = GetTmpExtractionDirectory();

            var filenames = entries.Select(x => Path.Combine(tmpDirectory, x.Name)).ToArray();

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
                                LifFile.ExtractEntries(entries, tmpDirectory, cancelSource.Token, null);
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

        private void FolderListView_CellEditValidating(object sender, BrightIdeasSoftware.CellEditEventArgs e)
        {
            if (e.RowObject is ILifItemInfo itemInfo)
            {
                string newName = e.NewValue as string;
                if (!itemInfo.Entry.ValidateRename(newName))
                {
                    e.Cancel = true;
                    FolderListView.CancelCellEdit();
                }
                else if (e.RowObject is LifFileInfo fileInfo)
                {
                    var oldExt = Path.GetExtension(fileInfo.Name);
                    var newExt = Path.GetExtension(newName);
                    if (newExt != oldExt)
                    {
                        if (MessageBox.Show(this, "Are you sure you want to change the extension?", "Rename",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                            MessageBoxDefaultButton.Button2) == DialogResult.No)
                        {
                            e.Cancel = true;
                            FolderListView.CancelCellEdit();
                        }

                        SetFileTypeIcon(fileInfo);
                    }
                }
            }
        }

        private void FolderListView_CellEditFinished(object sender, BrightIdeasSoftware.CellEditEventArgs e)
        {
            if (e.RowObject is LifFolderInfo folderInfo)
            {
                var folderNode = FindFolderNode(folderInfo.Folder);

                folderInfo.Folder.Rename(e.NewValue as string);
                folderNode.Text = folderInfo.Name;
                folderNode.Name = folderInfo.Folder.FullName;
            }
            else if (e.RowObject is LifFileInfo fileInfo)
            {
                fileInfo.File.Rename(e.NewValue as string);
                fileInfo.FileType = Path.GetExtension(fileInfo.Name).ToUpper();
                SetFileTypeIcon(fileInfo);
                fileInfo.Description = FileTypeInfoHelper.GetFileTypeDescription(fileInfo.Name);
                e.ListViewItem.ImageKey = fileInfo.ItemImageKey;
            }
            FolderListView.RefreshItem(e.ListViewItem);
        }

        private IEnumerable<ILifItemInfo> GetSelectedListViewItems()
        {
            return FolderListView.SelectedObjects
                .Cast<ILifItemInfo>();
        }

        #endregion

        private string GetTmpExtractionDirectory()
        {
            string tmpFolderName = "LIF" + LDDModder.Utilities.StringUtils.GenerateUID(8);
            return Path.Combine(Path.GetTempPath(), tmpFolderName);
        }

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

        private void UpdateFolderList()
        {
            CurrentLifFolders.RaiseListChangedEvents = false;
            CurrentLifFolders.Clear();
            //var allFolders = CurrentFile.RootFolder.get
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
            if (!IsLoadingTreeView && CurrentFolder != null && ToolBarFolderCombo.SelectedItem is LifFolderInfo folderInfo)
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

            if (!fromHistory)
            {
                if (oldFolder != null)
                    BackHistory.Add(oldFolder);

                if (BackHistory.Count > MAX_HISTORY)
                    BackHistory.RemoveAt(0);

                FwrdHistory.Clear();
            }

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

        private void CleanNavigationHistory()
        {
            if (CurrentFile != null)
            {
                var allFolders = CurrentFile.RootFolder.GetFolderHierarchy();

                BackHistory.RemoveAll(x => !allFolders.Contains(x));
                FwrdHistory.RemoveAll(x => !allFolders.Contains(x));
                UpdateNavigationButtons();
            }
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

        #region File Loading / Saving

        private void OpenLifFile()
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = $"{Localizations.FileFilter_LifFiles}|*.lif|{Localizations.FileFilter_AllFiles}|*.*";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    if (Path.GetExtension(ofd.FileName).ToUpper() != ".LIF")
                    {
                        using (var fs = File.OpenRead(ofd.FileName))
                        {
                            if (!LifFile.CheckIsLif(fs))
                            {
                                MessageBox.Show("Invalid Lif File.");
                                return;
                            }
                        }
                    }

                    OpenLifFileAsync(ofd.FileName);
                }
            }
        }

        private void OpenLifFileAsync(string filepath)
        {
            BeginOpenFile();

            Task.Factory.StartNew(() =>
            {
                LifFile openedFile = null;
                Exception error = null;

                try
                {
                    openedFile = LifFile.Open(filepath);
                }
                catch (Exception ex) { error = ex; }

                BeginInvoke((Action)(() => FinishOpenFile(openedFile, error)));
            });
        }

        private void BeginOpenFile()
        {
            toolStripProgressBar1.Visible = true;
            toolStripProgressBar1.Style = ProgressBarStyle.Marquee;
            IsOpeningFile = true;
            toolStripContainer1.Enabled = false;
        }

        private void FinishOpenFile(LifFile result, Exception error)
        {

            IsOpeningFile = false;
            toolStripContainer1.Enabled = true;

            if (result != null)
                LoadLifFile(result);
            else
                MessageBox.Show("An error occured:\r\n" + error, "Error");

            toolStripProgressBar1.Visible = false;
        }

        private bool SaveLifFile(LifFile lifFile, bool selectPath)
        {
            string targetPath = lifFile.FilePath;

            if (string.IsNullOrEmpty(targetPath) || selectPath)
            {
                using (var sfd = new SaveFileDialog())
                {
                    sfd.Filter = $"{Localizations.FileFilter_LifFiles}|*.lif|{Localizations.FileFilter_AllFiles}|*.*";
                    sfd.DefaultExt = ".lif";

                    if (!string.IsNullOrEmpty(targetPath))
                        sfd.FileName = targetPath;

                    if (sfd.ShowDialog() == DialogResult.OK)
                        targetPath = sfd.FileName;
                    else
                        return false;
                }
            }

            try
            {
                lifFile.SaveAs(targetPath, false);
                DisableLifEditing();
                LoadLifFile(lifFile);
                return true;

            }
            catch (Exception ex)
            {

            }
            return false;
        }

        #endregion

        private void UpdateMenuItems()
        {
            FileMenu_Close.Enabled = CurrentFile != null && !LifEditingEnabled;
            FileMenu_OpenItem.Enabled = 
                ActionsMenu_Open.Enabled = !LifEditingEnabled;

            FileMenu_ExtractItem.Enabled =
                ActionsMenu_Extract.Enabled = !LifEditingEnabled && !string.IsNullOrEmpty(CurrentFile?.FilePath);

            ActionsMenu_EnableEdit.Enabled = CurrentFile != null;
            ActionsMenu_EnableEdit.Visible = !LifEditingEnabled;
            ActionsMenu_CancelEdit.Visible = LifEditingEnabled;
            ActionsMenu_SaveLif.Visible = LifEditingEnabled;
        }

        #region Main ToolStrip Menu

        private void FileOpenMenuItem_Click(object sender, EventArgs e)
        {
            OpenLifFile();
        }

        private void FileMenu_Close_Click(object sender, EventArgs e)
        {
            if (CurrentFile != null)
                LoadLifFile(null);
        }

        private void FileMenu_NewLif_Click(object sender, EventArgs e)
        {
            LoadLifFile(new LifFile());
        }

        private void FileMenu_ExtractItem_Click(object sender, EventArgs e)
        {
            ExtractLif();
        }

        private void ViewModeMenuItems_Click(object sender, EventArgs e)
        {
            if (sender == ViewModeDetailsMenuItem)
                SetListViewMode(View.Details);
            else if (sender == ViewModeSmallMenuItem)
                SetListViewMode(View.SmallIcon);
            else if (sender == ViewModeLargeMenuItem)
                SetListViewMode(View.LargeIcon);
        }

        #endregion

        #region Action Menu

        private void ActionsMenu_Open_Click(object sender, EventArgs e)
        {
            OpenLifFile();
        }

        private void ActionsMenu_Extract_Click(object sender, EventArgs e)
        {
            if (CurrentFile != null)
            {
                //if (FolderListView.SelectedObjects.Count > 0)
                //    ExtractSelectedItems();
                //else
                ExtractLif();
            }
        }

        private void ActionsMenu_EnableEdit_Click(object sender, EventArgs e)
        {
            if (!LifEditingEnabled && CurrentFile != null)
                EnableLifEditing();
        }

        private void SaveMenu_Save_Click(object sender, EventArgs e)
        {
            if (CurrentFile != null)
                SaveLifFile(CurrentFile, false);
        }

        private void SaveMenu_SaveAs_Click(object sender, EventArgs e)
        {
            if (CurrentFile != null)
                SaveLifFile(CurrentFile, true);
        }

        private void ActionsMenu_CancelEdit_Click(object sender, EventArgs e)
        {
            if (LifEditingEnabled)
            {
                DisableLifEditing();

                if (CurrentFile != null)
                {
                    if (IsNewLif)
                        LoadLifFile(null);
                    else
                        OpenLifFileAsync(CurrentFile.FilePath);
                }
                //ToggleLifEditing(false);
            }

        }

        #endregion

        #region File & Folders Context Menu

        private void FolderListContextMenu_Opening(object sender, CancelEventArgs e)
        {
            if (CurrentFile == null)
            {
                e.Cancel = true;
                return;
            }

            if (FolderListContextMenu.SourceControl == FolderListView)
            {
                ListMenu_RenameItem.Enabled = LifEditingEnabled && FolderListView.SelectedObjects.Count == 1;
                ListMenu_ExtractItem.Enabled = !LifEditingEnabled && FolderListView.SelectedObjects.Count >= 1;
                ListMenu_DeleteItem.Enabled = LifEditingEnabled && FolderListView.SelectedObjects.Count >= 1;
                ListMenu_AddFileItem.Enabled = LifEditingEnabled;
                ListMenu_CreateFolderItem.Enabled = LifEditingEnabled;
            }
            else if (FolderListContextMenu.SourceControl == LifTreeView)
            {
                ListMenu_RenameItem.Enabled = LifEditingEnabled && LifTreeView.SelectedNode != null;
                ListMenu_ExtractItem.Enabled = !LifEditingEnabled && LifTreeView.SelectedNode != null;
                ListMenu_DeleteItem.Enabled = LifEditingEnabled && LifTreeView.SelectedNode != null;
                ListMenu_AddFileItem.Enabled = LifEditingEnabled;
                ListMenu_CreateFolderItem.Enabled = LifEditingEnabled;
            }
        }

        private void ListMenu_CreateFolderItem_Click(object sender, EventArgs e)
        {
            if (CurrentFolder != null)
                AddNewFolder(CurrentFolder);
        }

        private void ListMenu_AddFileItem_Click(object sender, EventArgs e)
        {
            if (CurrentFolder != null)
                AddNewFile(CurrentFolder);
        }

        private void ListMenu_RenameItem_Click(object sender, EventArgs e)
        {
            if (FolderListContextMenu.SourceControl == FolderListView)
            {
                if (FolderListView.SelectedItem != null)
                    FolderListView.EditSubItem(FolderListView.SelectedItem, 0);
            }
            else if (FolderListContextMenu.SourceControl == LifTreeView)
            {
                if (LifTreeView.SelectedNode != null)
                    LifTreeView.SelectedNode.BeginEdit();
            }
        }

        private void ListMenu_DeleteItem_Click(object sender, EventArgs e)
        {
            var selectedItems = GetSelectedItemsForContextMenu();
            DeleteLifEntries(selectedItems.Select(x => x.Entry));
        }

        private void ListMenu_ExtractItem_Click(object sender, EventArgs e)
        {
            var selectedItems = GetSelectedItemsForContextMenu();

            ExtractLifEntries(selectedItems.Select(x => x.Entry));
        }

        private IEnumerable<ILifItemInfo> GetSelectedItemsForContextMenu()
        {
            IEnumerable<ILifItemInfo> selectedItems;

            if (FolderListContextMenu.SourceControl == FolderListView)
                selectedItems = GetSelectedListViewItems();
            else if (FolderListContextMenu.SourceControl == LifTreeView)
                selectedItems = GetSelectedTreeViewItems();
            else
                selectedItems = Enumerable.Empty<ILifItemInfo>();

            return selectedItems;
        }

        

        #endregion

        #region Extraction

        private void ExtractLif()
        {
            using (var eid = new ExtractItemsDialog())
            {
                if (!string.IsNullOrEmpty(CurrentFile.FilePath))
                    eid.SelectedDirectory = Path.GetDirectoryName(CurrentFile.FilePath);
                eid.ItemsToExtract.Add(CurrentFile.RootFolder);
                eid.ShowDialog();
            }
        }

        private void ExtractLifEntries(IEnumerable<LifFile.LifEntry> entries)
        {
            if (!entries.Any())
                return;

            using (var eid = new ExtractItemsDialog())
            {
                //eid.TargetDirectory = Path.GetDirectoryName(CurrentFile.FilePath);
                eid.ItemsToExtract.AddRange(entries);
                eid.ShowDialog();
            }
        }

        #endregion

        #region LIF Editing

        private void EnableLifEditing()
        {
            if (!LifEditingEnabled)
            {
                LifEditingEnabled = true;
                LifTreeView.LabelEdit = true;
                FolderListView.IsSimpleDragSource = true;
                FolderListView.IsSimpleDropSink = true;
                UpdateMenuItems();
            }
        }

        private void DisableLifEditing()
        {
            if (LifEditingEnabled)
            {
                LifEditingEnabled = false;
                LifTreeView.LabelEdit = false;
                FolderListView.IsSimpleDragSource = false;
                FolderListView.IsSimpleDropSink = false;
                UpdateMenuItems();
            }
        }

        /// <summary>
        /// Adds the item in the listview (if needed) and keeps the default order (Folders A-Z then Files A-Z)
        /// </summary>
        /// <param name="itemInfo"></param>
        private void AddLifItemInfo(ILifItemInfo itemInfo)
        {
            if (itemInfo.Entry.Parent == CurrentFolder)
            {
                if (itemInfo is LifFolderInfo folderInfo)
                {
                    var folderNames = CurrentFolderItems.OfType<LifFolderInfo>()
                            .Select(x => x.Name).ToList();
                    folderNames.Add(itemInfo.Name);
                    folderNames.Sort();
                    int folderIndex = folderNames.IndexOf(itemInfo.Name);
                    CurrentFolderItems.Insert(folderIndex, itemInfo);
                }
                else if (itemInfo is LifFileInfo fileInfo)
                {
                    var fileNames = CurrentFolderItems.OfType<LifFileInfo>()
                            .Select(x => x.Name).ToList();
                    fileNames.Add(itemInfo.Name);
                    fileNames.Sort();
                    int folderCount = CurrentFolderItems.OfType<LifFolderInfo>().Count();
                    int fileIndex = fileNames.IndexOf(itemInfo.Name);
                    CurrentFolderItems.Insert(fileIndex + folderCount, itemInfo);
                }
            }
        }

        private void AddNewFolder(LifFile.FolderEntry parentFolder)
        {
            using (var cfd = new CreateFolderDialog())
            {
                cfd.ParentFolder = parentFolder;

                if (cfd.ShowDialog() == DialogResult.OK)
                {
                    var newFolder = parentFolder.CreateFolder(cfd.FolderName);
                    var folderInfo = new LifFolderInfo(newFolder) { LifName = CurrentFile.Name };
                    var parentNode = FindFolderNode(parentFolder);
                    parentNode.Nodes.Add(CreateFolderTreeNode(folderInfo));

                    AddLifItemInfo(folderInfo);
                    CurrentLifFolders.Add(folderInfo);
                }
            }
        }

        private void AddNewFile(LifFile.FolderEntry parentFolder)
        {
            using (var ofd = new OpenFileDialog())
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    
                    var fs = File.OpenRead(ofd.FileName);
                    try
                    {
                        var createdFile = parentFolder.AddFile(fs);
                        var fileInfo = new LifFileInfo(createdFile)
                        {
                            Description = FileTypeInfoHelper.GetFileTypeDescription(ofd.FileName)
                        };
                        SetFileTypeIcon(fileInfo);
                        AddLifItemInfo(fileInfo);
                    }
                    catch { }
                }
            }
        }

        private void DeleteLifEntries(IEnumerable<LifFile.LifEntry> entries)
        {
            if (!entries.Any())
                return;

            if (MessageBox.Show(this, "Are you sure you want to delete the selected item(s)?", "Confirmation",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {

                var parentDir = entries.OrderBy(x => x.GetLevel()).First().Parent;

                foreach (var entry in entries.OrderByDescending(x=>x.GetLevel()))
                {
                    entry.Parent.Entries.Remove(entry);
                }

                var deletedFolders = entries.OfType<LifFile.FolderEntry>();

                if (deletedFolders.Any())
                {
                    CleanNavigationHistory();
                    FillTreeView();
                }

                if (parentDir != CurrentFolder)
                    NavigateToFolder(parentDir);
                else
                    FillFolderListView();
            }
        }

        private void MoveLifEntries(IEnumerable<LifFile.LifEntry> entries, LifFile.FolderEntry targetFolder)
        {
            if (!entries.Any())
                return;
            //TODO: confirmation message box

            var curFolder = CurrentFolder;

            bool isCurrentDirMoved = entries.OfType<LifFile.FolderEntry>().Any(x => x.GetEntryHierarchy().Contains(CurrentFolder));

            foreach (var entry in entries)
                entry.Parent.Entries.Remove(entry);

            targetFolder.Entries.AddRange(entries);

            FillTreeView();

            if (isCurrentDirMoved)
                NavigateToFolder(targetFolder.Parent);
            else
                FillFolderListView();

        }

        #endregion

        private void LifViewerWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsOpeningFile)
                e.Cancel = true;
        }

        
    }
}
