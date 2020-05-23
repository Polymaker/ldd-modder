using LDDModder.BrickEditor.ProjectHandling;
using LDDModder.BrickEditor.Resources;
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
using WeifenLuo.WinFormsUI.Docking;

namespace LDDModder.BrickEditor.UI.Panels
{
    public partial class ValidationPanel : ProjectDocumentPanel
    {
        internal ValidationPanel()
        {
            InitializeComponent();
        }

        public ValidationPanel(ProjectManager projectManager) : base(projectManager)
        {
            InitializeComponent();

            InitializeImageList();
            InitializeListView();

            projectManager.ValidationStarted += ProjectManager_ValidationStarted;
            projectManager.ValidationFinished += ProjectManager_ValidationFinished;

            UpdateStatusButtons();
        }

        private void InitializeImageList()
        {
            var imgList = new ImageList()
            {
                ColorDepth = ColorDepth.Depth32Bit,
                ImageSize = new Size(16,16)
            };
            imgList.Images.Add("Error", Properties.Resources.StatusError);
            imgList.Images.Add("Warning", Properties.Resources.StatusWarning);
            imgList.Images.Add("Info", Properties.Resources.StatusInfo);
            ValidationMessageList.SmallImageList = imgList;

        }

        private void InitializeListView()
        {
            ColumnMessageType.ImageGetter = (o) =>
            {
                if (o is ValidationMessage message)
                    return message.Level.ToString();
                return "";
            };

            ValidationMessageList.ModelFilter = new BrightIdeasSoftware.ModelFilter(x =>
            {
                if (x is ValidationMessage message)
                    return IsMessageVisible(message);
                return true;
            });

            ColumnMessageDescription.AspectGetter = (x) =>
            {
                if (x is ValidationMessage message)
                    return FormatMessageDescription(message);

                return string.Empty;
            };

            ColumnMessageSource.AspectGetter = (x) =>
            {
                if (x is ValidationMessage message)
                    return FormatMessageSource(message);

                return string.Empty;
            };
        }

        private void ValidatePartButton_Click(object sender, EventArgs e)
        {
            ProjectManager.ValidateProject();
        }

        protected override void OnProjectChanged()
        {
            base.OnProjectChanged();
            ValidatePartButton.Enabled = ProjectManager.IsProjectOpen;
            UpdateStatusButtons();
            ValidationMessageList.ClearObjects();
        }

        private void ProjectManager_ValidationFinished(object sender, EventArgs e)
        {
            ValidatePartButton.Enabled = true;
            FillValidationMessages();
        }

        private void ProjectManager_ValidationStarted(object sender, EventArgs e)
        {
            ValidatePartButton.Enabled = false;
            ValidationMessageList.ClearObjects();
        }

        private void FillValidationMessages()
        {
            ValidationMessageList.ClearObjects();

            if (CurrentProject != null)
            {
                ValidationMessageList.AddObjects(ProjectManager.ValidationMessages.ToList());
            }

            UpdateStatusButtons();
        }

        private void UpdateStatusButtons()
        {
            int errorCount = 0;
            int warningCount = 0;
            int messageCount = 0;

            if (ProjectManager.IsProjectOpen)
            {
                errorCount = ProjectManager.ValidationMessages.Count(x => x.Level == ValidationLevel.Error);
                warningCount = ProjectManager.ValidationMessages.Count(x => x.Level == ValidationLevel.Warning);
                messageCount = ProjectManager.ValidationMessages.Count(x => x.Level == ValidationLevel.Info);
            }

            ToggleErrorsButton.Text = string.Format(ErrorCountText.Text, errorCount);
            ToggleWarningsButton.Text = string.Format(WarningCountText.Text, warningCount);
            ToggleMessagesButton.Text = string.Format(MessageCountText.Text, messageCount);
        }

        private void ToggleStatusButtons_CheckedChanged(object sender, EventArgs e)
        {
            //trigger filtering
            ValidationMessageList.ModelFilter = ValidationMessageList.ModelFilter;
        }

        private bool IsMessageVisible(ValidationMessage message)
        {
            switch (message.Level)
            {
                default:
                    return false;
                case ValidationLevel.Info:
                    return ToggleMessagesButton.Checked;
                case ValidationLevel.Warning:
                    return ToggleWarningsButton.Checked;
                case ValidationLevel.Error:
                    return ToggleErrorsButton.Checked;
            }
        }

        #region Listview Handling

        private string FormatMessageDescription(ValidationMessage message)
        {
            string localizedText = ModelLocalizations.ResourceManager.GetString(message.Code);
            if (!string.IsNullOrEmpty(localizedText))
            {
                if (message.SourceElement != null)
                    localizedText = localizedText.Replace("{name}", message.SourceElement.Name);
                if (localizedText.Contains("{0}"))
                    return string.Format(localizedText, message.MessageArguments);
                return localizedText;
            }
            return message.Code;
        }

        private string FormatMessageSource(ValidationMessage message)
        {
            if (message.SourceElement != null)
            {
                if (message.SourceElement is PartProperties)
                    return ModelLocalizations.Label_Part;
                return message.SourceElement.Name;
            }

            if (message.SourceKey == "PROJECT")
                return ModelLocalizations.Label_Project;

            return message.SourceKey;
        }

        #endregion

        private void ValidationMessageList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (ValidationMessageList.FocusedItem != null)
            {
                var message = ValidationMessageList.FocusedObject as ValidationMessage;
                if (message?.SourceElement != null)
                {
                    ProjectManager.SelectElement(message.SourceElement);
                }
            }
        }
    }
}
