using Assimp;
using LDDModder.BrickEditor.Meshes;
using LDDModder.LDD.Parts;
using LDDModder.Utilities;
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

namespace LDDModder.BrickEditor.UI
{
    public partial class ModelImportExportWindow : Form
    {
        private AssimpContext AssimpContext { get; set; }

        public ModelImportExportWindow()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            AssimpContext = new AssimpContext();

            if (LDD.LDDEnvironment.Current == null)
                LDD.LDDEnvironment.Initialize();

            if (LDD.LDDEnvironment.Current != null)
            {
                LddPathTextBox.Text = LDD.LDDEnvironment.Current.ApplicationDataPath;
            }
        }

        private void GetFileFormatExtID(out string formatID, out string fileExt)
        {
            formatID = null;
            fileExt = null;

            if (ExportObjRadio.Checked)
            {
                formatID = "obj";
                fileExt = "obj";
            }
            else if (ExportDaeRadio.Checked)
            {
                formatID = "collada";
                fileExt = "dae";
            }
            else if (ExportFbxRadio.Checked)
            {
                formatID = "collada";
                fileExt = "dae";
            }
        }

        private void ExportPartButton_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(PartIdTextBox.Text, out int partID))
            {
                MessageBox.Show("The part ID must be numeric.", "Error");
                return;
            }
            try
            {
                var part = PartWrapper.LoadPart(LDD.LDDEnvironment.Current, partID);
                GetFileFormatExtID(out string formatID, out string formatExt);

                using (var sfd = new SaveFileDialog())
                {
                    sfd.FileName = $"{part.PartID}.{formatExt}";
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        var assimpScene = MeshConverter.LddPartToAssimp(part);
                        AssimpContext.ExportFile(assimpScene, sfd.FileName, formatID);
                        MessageBox.Show("Part exported");

                    }
                }
            }
            catch(Exception ex)
            {
                if (ex is FileNotFoundException)
                    MessageBox.Show("The part does not exist or could not be found.", "Error");
                else
                    MessageBox.Show("There was an error loading the part.", "Error");
            }
        }
    }
}
