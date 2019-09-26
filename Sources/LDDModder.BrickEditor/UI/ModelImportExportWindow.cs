using Assimp;
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
        public ModelImportExportWindow()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (LDD.LDDEnvironment.Current == null)
                LDD.LDDEnvironment.Initialize();

            if (LDD.LDDEnvironment.Current != null)
            {
                LddPathTextBox.Text = LDD.LDDEnvironment.Current.ApplicationDataPath;
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
                string formatID = ExportDaeRadio.Checked ? "collada" : "obj";
                string formatExt = ExportDaeRadio.Checked ? "dae" : "obj";

                using (var sfd = new SaveFileDialog())
                {
                    sfd.FileName = $"{part.PartID}.{formatExt}";
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        LddMeshExporter.ExportLddPart(part, sfd.FileName, formatID, IncludeBonesCheckBox.Checked);
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
