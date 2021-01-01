using LDDModder.BrickEditor.ProjectHandling;
using LDDModder.Modding;
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
    public partial class BoneDataCopyDialog : Form
    {
        public List<PartBone> Bones { get; set; }

        //private PartBone _SelectedBone;

        private PartBone SelectedBone => SourceCombo.SelectedItem as PartBone;

        public ProjectManager ProjectManager { get; set; }

        public BoneDataCopyDialog()
        {
            InitializeComponent();
        }

        public BoneDataCopyDialog(ProjectManager projectManager)
        {
            Bones = projectManager.CurrentProject.Bones.ToList();
            ProjectManager = projectManager;

            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            SourceCombo.DataSource = Bones;
            SourceCombo.DisplayMember = "Name";
            //SourceCombo.BindingContext = new BindingContext();

            TargetListBox.DataSource = Bones;
            //TargetListBox.Items.AddRange(Bones.ToArray());
            TargetListBox.DisplayMember = "Name";
            TargetListBox.BindingContext = new BindingContext();
        }

        private void ApplyButton_Click(object sender, EventArgs e)
        {
            var sourceBone = SelectedBone;

            foreach (PartBone targetBone in TargetListBox.SelectedItems)
            {
                if (targetBone == SelectedBone)
                    continue;

                if (CollisionsCheckBox.Checked)
                {
                    if (ClearOldDataCheckBox.Checked)
                        targetBone.Collisions.Clear();

                    foreach (var col in sourceBone.Collisions)
                        targetBone.Collisions.Add(col.Clone());
                }

                if (ConnectionsCheckBox.Checked)
                {
                    if (ClearOldDataCheckBox.Checked)
                        targetBone.Connections.RemoveAll(x => x.SubType < 999000);

                    foreach (var conn in sourceBone.Connections.Where(x => x.SubType < 999000))
                        targetBone.Connections.Add(conn.Clone());
                }

                if (PhysicAttrCheckBox.Checked)
                {
                    targetBone.PhysicsAttributes = sourceBone.PhysicsAttributes.Clone();
                }

                if (BoundingCheckBox.Checked)
                {
                    targetBone.Bounding = sourceBone.Bounding.Clone();
                }
            }

            ProjectManager.ViewportWindow.RebuildModels();
            ProjectManager.ViewportWindow.ForceRender();
        }

        private void SourceCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            //var oldBone = _SelectedBone;
            //_SelectedBone = SourceCombo.SelectedItem as PartBone;
            TargetListBox.Invalidate();
        }

        private void TargetListBox_DrawItem(object sender, DrawItemEventArgs e)
        {
           
            var listItem = TargetListBox.Items[e.Index];

            SolidBrush textBrush;

            if (listItem == SelectedBone)
            {
                e.Graphics.FillRectangle(SystemBrushes.Control, e.Bounds);
                textBrush = new SolidBrush(Color.DarkGray);
                //textBrush = new SolidBrush(e.ForeColor);
            }
            else
            {
                e.DrawBackground();
                textBrush = new SolidBrush(e.ForeColor);
            }

            e.Graphics.DrawString(TargetListBox.GetItemText(listItem), 
                e.Font, textBrush, e.Bounds);

            textBrush.Dispose();
        }

        private void DataCheckBoxes_CheckedChanged(object sender, EventArgs e)
        {
            var boxes = new CheckBox[]
            {
                CollisionsCheckBox,
                BoundingCheckBox,
                CollisionsCheckBox,
                ConnectionsCheckBox
            };

            ApplyButton.Enabled = boxes.Any(x => x.Checked);
        }
    }
}
