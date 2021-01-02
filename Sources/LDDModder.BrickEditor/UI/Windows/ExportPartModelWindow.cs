using LDDModder.BrickEditor.Meshes;
using LDDModder.BrickEditor.Models;
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

        public Modding.PartProject CurrentProject { get; set; }

        private Models.BrickInfo PartToExport;

        public ExportPartModelWindow()
        {
            InitializeComponent();
            PartNameLabel.Text = string.Empty;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!LDDEnvironment.HasInitialized)
                LDDEnvironment.Initialize();

            AssimpContext = new Assimp.AssimpContext();

            bool isProjectOpen = (CurrentProject != null);

            CurrentProjectRb.Enabled = isProjectOpen;
            (isProjectOpen ? CurrentProjectRb : SelectPartRb).Checked = true;
            PartBrowseTextBox.Enabled = SelectPartRb.Checked;

            if (SelectPartRb.Checked)
                ValidateSelectedPartID();
        }

        private void PartBrowseTextBox_ValueChanged(object sender, EventArgs e)
        {
            if (PartToExport != null)
            {
                if (PartToExport.PartId.ToString() != PartBrowseTextBox.Value.Trim())
                    PartToExport = null;
                else
                    return;
            }

            ValidateSelectedPartID();
        }

        private void PartBrowseTextBox_BrowseButtonClicked(object sender, EventArgs e)
        {
            SelectPartToExport();
        }

        private void UpdateCanExport()
        {
            bool canExport;
            if (CurrentProjectRb.Checked)
                canExport = CurrentProject != null;
            else
                canExport = PartToExport != null;

            ExportButton.Enabled = canExport;
        }

        private void UpdateSelectedPartDescription()
        {
            PartNameLabel.ForeColor = ForeColor;

            if (CurrentProjectRb.Checked)
                PartNameLabel.Text = CurrentProject.PartDescription;
            else if (PartToExport != null)
                PartNameLabel.Text = PartToExport.Description;
            else if(!string.IsNullOrEmpty(PartBrowseTextBox.Value))
            {
                PartNameLabel.ForeColor = Color.Red;
                PartNameLabel.Text = "Part not found!";
            }
            else
                PartNameLabel.Text = string.Empty;
        }

        private void ValidateSelectedPartID()
        {
            if (!int.TryParse(PartBrowseTextBox.Value, out int partID) &&
                !string.IsNullOrEmpty(PartBrowseTextBox.Value))
            {
                PartNameLabel.Text = "Invalid part ID";
                PartNameLabel.ForeColor = Color.Red;
            }
            else
            {
                if (!string.IsNullOrEmpty(PartBrowseTextBox.Value))
                    PartToExport = FindPartInfo(partID);
                else
                    PartToExport = null;
                UpdateSelectedPartDescription();
            }

            UpdateCanExport();
        }

        private void SelectPartToExport()
        {
            using (var dlg = new SelectBrickDialog())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    PartToExport = dlg.SelectedBrick;
                    PartBrowseTextBox.Value = dlg.SelectedBrick.PartId.ToString();
                    PartNameLabel.Text = dlg.SelectedBrick.Description;

                    UpdateCanExport();
                }
            }
        }

        private Models.BrickInfo FindPartInfo(int partID)
        {
            try
            {
                var brickInfo = BrickListCache.GetBrick(partID);
                if (brickInfo != null)
                    return brickInfo;

                var part = PartWrapper.LoadPart(LDDEnvironment.Current, partID, false);
                if (part != null)
                    return new BrickInfo(part);
            }
            catch { }

            return null;
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
                ChkRoundEdge.Enabled =
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
            //int.TryParse(PartBrowseTextBox.Text, out int partID);

            //PartWrapper partInfo = null;

            //try
            //{
            //    partInfo = PartWrapper.LoadPart(LDDEnvironment.Current, partID, true);
            //}
            //catch (Exception ex) 
            //{
            //    MessageBox.Show($"An error occured while loading LDD part:\r\n{ex.ToString()}", "Error");
            //    return;
            //}

            //if (partInfo != null)
            //{
            //    var exportOptions = new MeshExportOptions()
            //    {
            //        IndividualComponents = RbAdvancedExport.Checked,
            //        IncludeBones = ChkBones.Enabled && ChkBones.Checked,
            //        IncludeAltMeshes = ChkAltMeshes.Enabled && ChkAltMeshes.Checked,
            //        IncludeCollisions = ChkCollisions.Enabled && ChkCollisions.Checked,
            //        IncludeConnections = ChkConnections.Enabled && ChkConnections.Checked,
            //        IncludeRoundEdgeData = ChkRoundEdge.Enabled && ChkRoundEdge.Checked
            //    };

            //    GetSelectedFormatInfo(out string formatID, out string formatExt);

            //    using (var sfd = new SaveFileDialog())
            //    {
            //        sfd.FileName = $"{partInfo.PartID}.{formatExt}";

            //        if (sfd.ShowDialog() == DialogResult.OK)
            //        {
            //            try
            //            {
            //                var assimpScene = MeshConverter.LddPartToAssimp(partInfo, exportOptions);
            //                AssimpContext.ExportFile(assimpScene, sfd.FileName, formatID, 
            //                    Assimp.PostProcessSteps.FlipUVs);
            //                MessageBox.Show("Part exported");
            //            }
            //            catch (Exception ex)
            //            {
            //                MessageBox.Show($"An error occured while exporting model:\r\n{ex.ToString()}", "Error");
            //            }
            //        }
            //    }
            //}
            ExportPartModel();
        }

        private void ExportPartModel()
        {
            var exportOptions = new MeshExportOptions()
            {
                IndividualComponents = RbAdvancedExport.Checked,
                IncludeBones = ChkBones.Enabled && ChkBones.Checked,
                IncludeAltMeshes = ChkAltMeshes.Enabled && ChkAltMeshes.Checked,
                IncludeCollisions = ChkCollisions.Enabled && ChkCollisions.Checked,
                IncludeConnections = ChkConnections.Enabled && ChkConnections.Checked,
                IncludeRoundEdgeData = ChkRoundEdge.Enabled && ChkRoundEdge.Checked
            };

            GetSelectedFormatInfo(out string formatID, out string formatExt);
            exportOptions.FileFormatID = formatID;

            int currentPartID;
            if (CurrentProjectRb.Checked)
                currentPartID = CurrentProject.PartID;
            else
                currentPartID = PartToExport.PartId;

            using (var sfd = new SaveFileDialog())
            {
                sfd.FileName = $"{currentPartID}.{formatExt}";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        if (CurrentProjectRb.Checked)
                            ExportCurrentProject(exportOptions, sfd.FileName);
                        else
                            ExportSelectedPart(exportOptions, sfd.FileName);

                        MessageBox.Show("Model exported");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occured while exporting model:\r\n{ex.ToString()}", "Error");
                    }
                }
            }
        }

        private void ExportCurrentProject(MeshExportOptions exportOptions, string filepath)
        {
            var assimpScene = MeshConverter.PartProjectToAssimp(CurrentProject, exportOptions);
            AssimpContext.ExportFile(assimpScene, filepath,
                exportOptions.FileFormatID,
                Assimp.PostProcessSteps.FlipUVs);
            assimpScene.Clear();
        }

        private void ExportSelectedPart(MeshExportOptions exportOptions, string filepath)
        {
            var partInfo = PartWrapper.LoadPart(LDDEnvironment.Current, PartToExport.PartId, true);
            var assimpScene = MeshConverter.LddPartToAssimp(partInfo, exportOptions);
            AssimpContext.ExportFile(assimpScene, filepath, 
                exportOptions.FileFormatID,
                Assimp.PostProcessSteps.FlipUVs);
            assimpScene.Clear();
        }

        private void PartToExportRb_CheckedChanged(object sender, EventArgs e)
        {
            PartBrowseTextBox.Enabled = SelectPartRb.Checked;
            UpdateSelectedPartDescription();
            UpdateCanExport();
        }
    }
}
