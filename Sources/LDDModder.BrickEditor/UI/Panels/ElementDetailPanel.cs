using LDDModder.BrickEditor.ProjectHandling;
using LDDModder.BrickEditor.Resources;
using LDDModder.LDD.Primitives.Connectors;
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
    public partial class ElementDetailPanel : ProjectDocumentPanel
    {
        public ElementDetailPanel()
        {
            InitializeComponent();
        }

        public ElementDetailPanel(ProjectManager projectManager) : base(projectManager)
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            FillSelectionDetails(null);
        }

        protected override void OnElementSelectionChanged()
        {
            base.OnElementSelectionChanged();
 
            ExecuteOnThread(() =>
            {
                FillSelectionDetails(ProjectManager.SelectedElement);
            });
        }

        private void FillSelectionDetails(PartElement selection)
        {
            CommentTextBox.DataBindings.Clear();

            ToggleControlsEnabled(selection != null,
                NameTextBox, CommentTextBox);
            SelectionTransformEdit.Enabled = false;

            NameTextBox.Text = selection?.Name ?? string.Empty;
            CommentTextBox.Text = selection?.Comments ?? string.Empty;

            if (selection != null)
            {
                SelectionTypeLabel.Text = GetElementTypeName(selection);
                CommentTextBox.DataBindings.Add(new Binding("Text", selection, "Comments"));
            }
            else
                SelectionTypeLabel.Text = string.Empty;

            if (SelectionTransformEdit.Tag != null)
            {
                SelectionTransformEdit.BindPhysicalElement(null);
            }
            
            if (selection is IPhysicalElement physicalElement)
            {
                SelectionTransformEdit.BindPhysicalElement(physicalElement);
                SelectionTransformEdit.Tag = physicalElement;
                SelectionTransformEdit.Enabled = true;
            }

        }

        private string GetElementTypeName(PartElement element)
        {
            if (element is PartSurface)
                return ModelLocalizations.Label_Surface;

            switch (element)
            {
                case PartSurface _:
                    return ModelLocalizations.Label_Surface;
                case PartBone _:
                    return ModelLocalizations.Label_Bone;
                case MaleStudModel _:
                    return ModelLocalizations.ModelComponentType_MaleStud;
                case FemaleStudModel _:
                    return ModelLocalizations.ModelComponentType_FemaleStud;
                case BrickTubeModel _:
                    return ModelLocalizations.ModelComponentType_BrickTube;
                case PartModel _:
                    return ModelLocalizations.ModelComponentType_Part;

                case ModelMeshReference _:
                    return ModelLocalizations.Label_Mesh;
                case PartConnection conn:
                    return $"{ModelLocalizations.Label_Connection} <{conn.ConnectorType.ToString()}>";
                    //return ModelLocalizations.ResourceManager.GetString($"ConnectorType_{conn.ConnectorType}");
                case PartCollision coll:
                    string collType = ModelLocalizations.ResourceManager.GetString($"CollisionType_{coll.CollisionType}");
                    return $"{ModelLocalizations.Label_Collision} ({collType})";
            }

            return element.GetType().Name;
        }

        protected override void OnProjectElementsChanged()
        {
            base.OnProjectElementsChanged();
        }

        private void NameTextBox_Validating(object sender, CancelEventArgs e)
        {
            //ProjectManager.CurrentProject.RenameElement()
            //ProjectManager.SelectedElement.
        }
    }
}
