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
            DerivedFromCombo.Items.Insert(0, string.Empty);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ToggleOriginalAuthor(false);
            UpdateControlBindings();


            int maxLabelWidth = OriginalAuthorLabel.CalculateLabelWidth();
            maxLabelWidth = Math.Max(maxLabelWidth, CreatedByLabel.CalculateLabelWidth());
            maxLabelWidth = Math.Max(maxLabelWidth, DerivedFromLabel.CalculateLabelWidth());
            OriginalAuthorLabel.LabelWidth = maxLabelWidth;
            CreatedByLabel.LabelWidth = maxLabelWidth;
            DerivedFromLabel.LabelWidth = maxLabelWidth;
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
                OriginalAuthorBox,
                CommentBox,
                DerivedFromCombo);

            CreatedByBox.DataBindings.Clear();
            DerivedFromCombo.DataBindings.Clear();
            OriginalAuthorBox.DataBindings.Clear();
            CommentBox.DataBindings.Clear();

            if (CurrentProject != null)
            {
                CreatedByBox.DataBindings.Add(new Binding("Text",
                    CurrentProject.ProjectInfo, nameof(ProjectInfo.Authors),
                    true, DataSourceUpdateMode.OnPropertyChanged));

                DerivedFromCombo.DataBindings.Add(new Binding("Text",
                    CurrentProject.ProjectInfo, nameof(ProjectInfo.DerivedFrom),
                    true, DataSourceUpdateMode.OnPropertyChanged));

                OriginalAuthorBox.DataBindings.Add(new Binding("Text",
                    CurrentProject.ProjectInfo, nameof(ProjectInfo.OriginalAuthor),
                    true, DataSourceUpdateMode.OnPropertyChanged));

                CommentBox.DataBindings.Add(new Binding("Text",
                    CurrentProject.ProjectInfo, nameof(ProjectInfo.Comments),
                    true, DataSourceUpdateMode.OnPropertyChanged));

            }
        }

        protected override void OnElementPropertyChanged(Modding.ElementValueChangedEventArgs e)
        {
            base.OnElementPropertyChanged(e);

            if (e.Element == CurrentProject?.ProjectInfo)
            {
                BeginInvokeOnce(UpdateControlBindings, nameof(UpdateControlBindings));
            }
        }

        private void DerivedFromCombo_TextChanged(object sender, EventArgs e)
        {
            ToggleOriginalAuthor(!string.IsNullOrEmpty(DerivedFromCombo.Text));
        }

        private void ToggleOriginalAuthor(bool visible)
        {
            OriginalAuthorLabel.Visible = visible;
            flowLayoutPanel1.SetFlowBreak(OriginalAuthorLabel, visible);
            flowLayoutPanel1.SetFlowBreak(DerivedFromLabel, !visible);
        }
    }
}
