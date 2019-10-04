using LDDModder.LDD.Files;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
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

        public LifViewerWindow()
        {
            InitializeComponent();
            IconImageList.Images.Add(Properties.Resources.Folder_16x16);
        }

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

        private void ToolStripMenuItem1_Click(object sender, EventArgs e)
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
            FolderListView.Items.Clear();

            CurrentFile = file;
            CurrentFolder = file.RootFolder;
            CurrentFileStripLabel.Text = file.FilePath;
            FillTreeView();
        }

        private void FillTreeView()
        {
            IsLoadingTreeView = true;
            LifTreeView.Nodes.Clear();
            string rootName = Path.GetFileNameWithoutExtension(CurrentFile.FilePath);

            void AddFolderNodes(LifFile.FolderEntry folder, TreeNode parentNode)
            {
                var node = new TreeNode(folder.IsRootDirectory ? rootName : folder.Name);
                node.Tag = folder;

                if (parentNode == null)
                    LifTreeView.Nodes.Add(node);
                else
                    parentNode.Nodes.Add(node);
                foreach (var subFolder in folder.Folders)
                    AddFolderNodes(subFolder, node);
            }

            AddFolderNodes(CurrentFile.RootFolder, null);
            LifTreeView.Nodes[0].Expand();
            LifTreeView.SelectedNode = LifTreeView.Nodes[0];
            IsLoadingTreeView = false;

            FillFolderListView();
        }

        private void FillFolderListView()
        {
            FolderListView.Items.Clear();
            if (CurrentFolder != null)
            {
                foreach (var entry in CurrentFolder.Entries.OrderBy(x => x is LifFile.FileEntry))
                {
                    var lvi = new ListViewItem(entry.Name);
                    if (entry is LifFile.FileEntry file)
                    {
                        lvi.SubItems.Add(file.FileSize.ToString());
                    }
                    FolderListView.Items.Add(lvi);
                }
            }
        }

        private void LifTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (IsLoadingTreeView)
                return;

            if (e.Node.Tag is LifFile.FolderEntry selectedFolder && selectedFolder != CurrentFolder)
            {
                CurrentFolder = selectedFolder;
                FillFolderListView();
            }
        }
    }
}
