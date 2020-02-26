using LDDModder.BrickEditor.Meshes;
using LDDModder.LDD;
using LDDModder.LDD.Parts;
using LDDModder.LDD.Primitives;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LDDModder.BrickEditor.UI.Windows
{
    public partial class ExportPartModelWindow : Form
    {
        private Assimp.AssimpContext AssimpContext;

        public int PartIDToExport { get; set; }

        public ExportPartModelWindow()
        {
            InitializeComponent();
            PartNameLabel.Visible = false;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!LDDEnvironment.HasInitialized)
                LDDEnvironment.Initialize();

            AssimpContext = new Assimp.AssimpContext();

            if (PartIDToExport > 0)
            {
                TxtPartID.Text = PartIDToExport.ToString();
                ValidatePartID();
            }
        }

        private void TxtPartID_Validated(object sender, EventArgs e)
        {
            ValidatePartID();
        }

        private void ValidatePartID()
        {
            if (string.IsNullOrEmpty(TxtPartID.Text))
            {
                PartNameLabel.Visible = false;
                ExportButton.Enabled = false;
            }
            else
            {
                if (!int.TryParse(TxtPartID.Text, out int partID))
                {
                    PartNameLabel.Text = "Invalid part ID";
                    PartNameLabel.ForeColor = Color.Red;
                    PartNameLabel.Visible = true;
                    ExportButton.Enabled = false;
                }
                else
                {
                    var partInfo = FindPart(partID);
                    if (partInfo != null)
                    {
                        PartNameLabel.Text = partInfo.Name;
                        PartNameLabel.ForeColor = SystemColors.ControlText;
                        PartNameLabel.Visible = true;
                        ExportButton.Enabled = true;
                    }
                    else
                    {
                        PartNameLabel.Text = "Part not found";
                        PartNameLabel.ForeColor = Color.Red;
                        PartNameLabel.Visible = true;
                        ExportButton.Enabled = false;
                    }
                }
            }
        }

        private Primitive FindPart(int partID)
        {
            try
            {
                return PartWrapper.GetPrimitiveInfo(LDDEnvironment.Current, partID);
            }
            catch { }
            return null;
        }

        private void SearchPartButton_Click(object sender, EventArgs e)
        {
            using (var dlg = new SelectBrickDialog())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    TxtPartID.Text = dlg.SelectedBrick.PartId.ToString();
                    PartNameLabel.Text = dlg.SelectedBrick.Description;
                    PartNameLabel.Visible = true;
                    ExportButton.Enabled = true;
                }
            }
        }

        private void RbAdvancedExport_CheckedChanged(object sender, EventArgs e)
        {
            UpdateExportOptionStates();
        }

        private void RbCollada_CheckedChanged(object sender, EventArgs e)
        {
            UpdateExportOptionStates();
        }

        private void UpdateExportOptionStates()
        {
            ChkBones.Enabled =
               ChkConnections.Enabled = RbAdvancedExport.Checked && RbCollada.Checked;

            ChkCollisions.Enabled =
               ChkAltMeshes.Enabled = RbAdvancedExport.Checked;
        }

        private void GetSelectedFormatInfo(out string formatID, out string fileExt)
        {
            formatID = null;
            fileExt = null;

            if (RbCollada.Checked)
            {
                formatID = "collada";
                fileExt = "dae";
            }
            else if (RbWavefront.Checked)
            {
                formatID = "obj";
                fileExt = "obj";
            }
        }

        private void ExportButton_Click(object sender, EventArgs e)
        {
            int.TryParse(TxtPartID.Text, out int partID);

            PartWrapper partInfo = null;

            try
            {
                partInfo = PartWrapper.LoadPart(LDDEnvironment.Current, partID, true);
            }
            catch (Exception ex) 
            {
                MessageBox.Show($"An error occured while loading LDD part:\r\n{ex.ToString()}", "Error");
                return;
            }

            if (partInfo != null)
            {
                var exportOptions = new MeshExportOptions()
                {
                    IndividualComponents = RbAdvancedExport.Checked,
                    IncludeBones = RbAdvancedExport.Checked && ChkBones.Checked,
                    IncludeAltMeshes = RbAdvancedExport.Checked && ChkBones.Checked,
                    IncludeCollisions = RbAdvancedExport.Checked && ChkCollisions.Checked,
                    IncludeConnections = RbAdvancedExport.Checked && ChkConnections.Checked
                };

                GetSelectedFormatInfo(out string formatID, out string formatExt);

                using (var sfd = new SaveFileDialog())
                {
                    sfd.FileName = $"{partInfo.PartID}.{formatExt}";

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            var assimpScene = MeshConverter.LddPartToAssimp(partInfo, exportOptions);
                            AssimpContext.ExportFile(assimpScene, sfd.FileName, formatID);
                            MessageBox.Show("Part exported");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"An error occured while exporting model:\r\n{ex.ToString()}", "Error");
                        }
                    }
                }
            }
        }

        
    }
}
