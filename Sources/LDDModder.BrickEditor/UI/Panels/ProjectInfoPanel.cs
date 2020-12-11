using LDDModder.BrickEditor.ProjectHandling;
using LDDModder.Modding.Editing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LDDModder.BrickEditor.UI.Panels
{
    public partial class ProjectInfoPanel : ProjectDocumentPanel
    {
        public ProjectInfoPanel()
        {
            InitializeComponent();
        }

        public ProjectInfoPanel(ProjectManager projectManager) : base(projectManager)
        {
            InitializeComponent();
            CloseButtonVisible = false;
            CloseButton = false;

            DockAreas ^= WeifenLuo.WinFormsUI.Docking.DockAreas.Document;

            SetControlDoubleBuffered(tableLayoutPanel1);
        }

        protected override void OnProjectChanged()
        {
            base.OnProjectChanged();
            UpdateControlBindings();
        }


        private void UpdateControlBindings()
        {
            ToggleControlsEnabled(CurrentProject != null,
                CreatedByBox,
                OriginatedFromBox,
                CommentBox);

            CreatedByBox.DataBindings.Clear();
            OriginatedFromBox.DataBindings.Clear();
            CommentBox.DataBindings.Clear();

            if (CurrentProject != null)
            {
                CreatedByBox.DataBindings.Add(new Binding("Text",
                    CurrentProject.Properties, nameof(PartProperties.Authors),
                    true, DataSourceUpdateMode.OnValidation));

                OriginatedFromBox.DataBindings.Add(new Binding("Text",
                    CurrentProject.Properties, nameof(PartProperties.OriginalPart),
                    true, DataSourceUpdateMode.OnValidation));

                CommentBox.DataBindings.Add(new Binding("Text",
                    CurrentProject.Properties, nameof(PartProperties.ChangeLog),
                    true, DataSourceUpdateMode.OnValidation));

            }
        }
    }
}
