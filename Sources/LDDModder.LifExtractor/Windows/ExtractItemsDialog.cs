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

namespace LDDModder.LifExtractor.Windows
{
    public partial class ExtractItemsDialog : Form
    {
        //private string _TargetDirectory;

        public string TargetDirectory { get; set; }

        public List<LifFile.LifEntry> ItemsToExtract { get; }

        public ExtractItemsDialog()
        {
            InitializeComponent();
            ItemsToExtract = new List<LifFile.LifEntry>();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!string.IsNullOrEmpty(TargetDirectory))
                DestinationTextBox.Text = TargetDirectory;
        }

        private void SelectFolderButton_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                fbd.SelectedPath = DestinationTextBox.Text;
                fbd.ShowNewFolderButton = true;

                if (fbd.ShowDialog(this) == DialogResult.OK)
                {
                    TargetDirectory = DestinationTextBox.Text = fbd.SelectedPath;
                }
            }
        }

        private void CancelExtractButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void ExtractButton_Click(object sender, EventArgs e)
        {
            string tmpDir = Path.Combine(Path.GetTempPath(),
                    Path.GetFileNameWithoutExtension(Path.GetRandomFileName()));

            //var topMostParent = ItemsToExtract
            //    .Select(x => x.Parent ?? x as LifFile.FolderEntry)
            //    .OrderBy(x => x.GetLevel())
            //    .FirstOrDefault();

            //string parentName = topMostParent.IsRootDirectory ? topMostParent.Lif.Name : topMostParent.Name;
            //string tmpSubDir = Path.Combine(tmpDir, parentName);

            try
            {

                foreach (var entry in ItemsToExtract)
                {
                    entry.ExtractToDirectory(tmpDir);
                }

                var files = Directory.GetFileSystemEntries(tmpDir);
                var result = NativeMethods.CopyFiles(files, TargetDirectory);
            }
            finally
            {
                NativeMethods.DeleteFileOrFolder(tmpDir);
            }
        }

        private void DestinationTextBox_TextChanged(object sender, EventArgs e)
        {
            TargetDirectory = DestinationTextBox.Text;
        }
    }
}
