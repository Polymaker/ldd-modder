using LDDModder.BrickEditor.Models.Navigation;
using LDDModder.BrickEditor.ProjectHandling.ViewInterfaces;
using LDDModder.BrickEditor.Rendering;
using LDDModder.BrickEditor.Resources;
using LDDModder.BrickEditor.Settings;
using LDDModder.BrickEditor.UI.Windows;
using LDDModder.LDD;
using LDDModder.LDD.Parts;
using LDDModder.LDD.Primitives.Collisions;
using LDDModder.LDD.Primitives.Connectors;
using LDDModder.Modding.Editing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace LDDModder.BrickEditor.ProjectHandling
{
    public class ProjectManager : IProjectManager
    {
        private List<PartElement> _SelectedElements;
        private List<ValidationMessage> _ValidationMessages;
        private long LastValidation;
        private long LastSavedChange;

        public PartProject CurrentProject { get; private set; }

        public UndoRedoManager UndoRedoManager { get; }

        public bool IsProjectOpen => CurrentProject != null;

        public bool IsNewProject => IsProjectOpen && string.IsNullOrEmpty(CurrentProject.ProjectPath);

        public bool IsModified => LastSavedChange != UndoRedoManager.CurrentChangeID;

        public event EventHandler UndoHistoryChanged
        {
            add => UndoRedoManager.UndoHistoryChanged += value;
            remove => UndoRedoManager.UndoHistoryChanged -= value;
        }

        public PartElement SelectedElement
        {
            get => _SelectedElements.FirstOrDefault();
            set => SelectElement(value);
        }

        public IList<PartElement> SelectedElements => _SelectedElements.AsReadOnly();

        public ProjectTreeNodeCollection NavigationTreeNodes { get; private set; }

        #region Windows

        public IMainWindow MainWindow { get; set; }
        public IViewportWindow ViewportWindow { get; set; }
        public INavigationWindow NavigationWindow { get; set; }

        #endregion

        #region Events

        public event EventHandler SelectionChanged;

        public event EventHandler ProjectClosed;

        public event EventHandler ProjectChanged;

        public event EventHandler ProjectModified;

        public event EventHandler<ElementCollectionChangedEventArgs> ElementCollectionChanged;

        public event EventHandler<ElementValueChangedEventArgs> ElementPropertyChanged;

        public event EventHandler ProjectElementsChanged;

        #endregion

        private bool ElementsChanged;
        private bool PreventProjectChange;
        private bool IsProjectAttached;

        static ProjectManager()
        {
            ElementExtenderFactory.RegisterExtension(typeof(PartSurface), typeof(ModelElementExtension));
            ElementExtenderFactory.RegisterExtension(typeof(SurfaceComponent), typeof(ModelElementExtension));
            ElementExtenderFactory.RegisterExtension(typeof(FemaleStudModel), typeof(FemaleStudModelExtension));
            ElementExtenderFactory.RegisterExtension(typeof(ModelMeshReference), typeof(ModelElementExtension));
            
            ElementExtenderFactory.RegisterExtension(typeof(PartBone), typeof(BoneElementExtension));

            ElementExtenderFactory.RegisterExtension(typeof(PartCollision), typeof(ModelElementExtension));
            ElementExtenderFactory.RegisterExtension(typeof(PartConnection), typeof(ModelElementExtension));
        }

        public ProjectManager()
        {
            _SelectedElements = new List<PartElement>();
            _ValidationMessages = new List<ValidationMessage>();
            NavigationTreeNodes = new ProjectTreeNodeCollection(this);
            
            UndoRedoManager = new UndoRedoManager(this);
            UndoRedoManager.BeginUndoRedo += UndoRedoManager_BeginUndoRedo;
            UndoRedoManager.EndUndoRedo += UndoRedoManager_EndUndoRedo;
            UndoRedoManager.UndoHistoryChanged += UndoRedoManager_UndoHistoryChanged;

            _ShowPartModels = true;
            _PartRenderMode = MeshRenderMode.SolidWireframe;
        }

        #region Project Loading/Closing

        public void SetCurrentProject(PartProject project)
        {
            if (CurrentProject != project)
            {
                PreventProjectChange = true;
                CloseCurrentProject();
                PreventProjectChange = false;

                CurrentProject = project;
                
                if (project != null)
                    AttachPartProject(project);
                ProjectChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public void CloseCurrentProject()
        {
            if (CurrentProject != null)
            {
                DettachPartProject(CurrentProject);

                ProjectClosed?.Invoke(this, EventArgs.Empty);

                CurrentProject = null;
                UndoRedoManager.ClearHistory();

                if (!PreventProjectChange)
                    ProjectChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private void AttachPartProject(PartProject project)
        {
            project.ElementCollectionChanged += Project_ElementCollectionChanged;
            project.ElementPropertyChanged += Project_ElementPropertyChanged;
            project.UpdateModelStatistics();

            //LoadUserProperties();

            LastValidation = -1;
            IsProjectAttached = true;

            InitializeElementExtensions();
        }

        private void DettachPartProject(PartProject project)
        {
            project.ElementCollectionChanged -= Project_ElementCollectionChanged;
            project.ElementPropertyChanged -= Project_ElementPropertyChanged;

            NavigationTreeNodes.Clear();
            _SelectedElements.Clear();
            _ValidationMessages.Clear();
            LastSavedChange = 0;
            LastValidation = -1;
            IsProjectAttached = false;
        }

        #endregion

        public void SaveProject(string targetPath)
        {
            if (IsProjectOpen)
            {
                //SaveUserProperties();
                CurrentProject.Save(targetPath);
                CurrentProject.ProjectPath = targetPath;
                LastSavedChange = UndoRedoManager.CurrentChangeID;
                ProjectModified?.Invoke(this, EventArgs.Empty);
            }
        }

        public void SaveWorkingProject()
        {
            if (IsProjectOpen)
            {
                //SaveUserProperties();
                var projectXml = CurrentProject.GenerateProjectXml();
                projectXml.Save(Path.Combine(CurrentProject.ProjectWorkingDir, PartProject.ProjectFileName));
            }
        }

        #region Project User Properties


        private void LoadUserProperties()
        {
            if (CurrentProject.TryGetProperty("ShowModels", out bool showModels))
                ShowPartModels = showModels;

            if (CurrentProject.TryGetProperty("ShowCollisions", out showModels))
                ShowCollisions = showModels;

            if (CurrentProject.TryGetProperty("ShowConnections", out showModels))
                ShowConnections = showModels;

            if (CurrentProject.TryGetProperty("PartRenderMode", out MeshRenderMode renderMode))
                PartRenderMode = renderMode;

            foreach (var elem in CurrentProject.GetAllElements())
            {
                var elemExt = elem.GetExtension<ModelElementExtension>();
                if (elemExt == null)
                    continue;

                string elemCfg = string.Empty;
                string elemKey = GetElemKey(elem);

                if (!string.IsNullOrEmpty(elemKey) && 
                    CurrentProject.ProjectProperties.ContainsKey(elemKey))
                {
                    elemCfg = CurrentProject.ProjectProperties[elemKey];
                }

                if (elemCfg.EqualsIC("Hidden"))
                    elemExt.IsHidden = true;
            }
        }

        private void SaveUserProperties()
        {
            CurrentProject.ProjectProperties.Clear();
            CurrentProject.ProjectProperties["ShowModels"] = ShowPartModels.ToString();
            CurrentProject.ProjectProperties["ShowCollisions"] = ShowCollisions.ToString();
            CurrentProject.ProjectProperties["ShowConnections"] = ShowConnections.ToString();
            CurrentProject.ProjectProperties["PartRenderMode"] = PartRenderMode.ToString();

            foreach (var elem in CurrentProject.GetAllElements())
            {
                var elemExt = elem.GetExtension<ModelElementExtension>();

                if (elemExt != null && elemExt.IsHidden)
                {
                    string elemKey = GetElemKey(elem);
                    CurrentProject.ProjectProperties[elemKey] = "Hidden";
                }
            }
        }

        private string GetElemKey(PartElement element)
        {
            if (element is PartSurface)
                return element.Name;
            else
                return $"Elem_{element.ID}";
        }

        #endregion

        #region Project Events

        private void Project_ElementCollectionChanged(object sender, ElementCollectionChangedEventArgs e)
        {
            if (e.Action == System.ComponentModel.CollectionChangeAction.Remove)
            {
                int count = _SelectedElements.RemoveAll(x => e.RemovedElements.Contains(x));
                if (count > 0)
                    SelectionChanged?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                if (IsProjectAttached)
                    InitializeElementExtensions();
            }

            ElementCollectionChanged?.Invoke(this, e);

            UndoRedoManager.ProcessProjectElementsChanged(e);

            if (IsExecutingUndoRedo || IsExecutingBatchChanges)
                ElementsChanged = true;
            else
                ProjectElementsChanged?.Invoke(this, EventArgs.Empty);
        }

        private void Project_ElementPropertyChanged(object sender, ElementValueChangedEventArgs e)
        {
            ElementPropertyChanged?.Invoke(this, e);

            UndoRedoManager.ProcessElementPropertyChanged(e);
        }

        #endregion

        public string GetProjectDisplayName()
        {
            if (CurrentProject != null)
            {
                if (CurrentProject.PartID > 0 && !string.IsNullOrEmpty(CurrentProject.PartDescription))
                    return $"{CurrentProject.PartID} - {CurrentProject.PartDescription}";
                else if (CurrentProject.PartID > 0)
                    return $"{ModelLocalizations.Label_Part} {CurrentProject.PartID}";
                else if (!string.IsNullOrEmpty(CurrentProject.PartDescription))
                    return $"{CurrentProject.PartDescription}";
                else
                    return ModelLocalizations.Label_NewPartProject;
            }

            return ModelLocalizations.Label_NoActiveProject;
        }

        #region Viewport Display Handling

        private bool _ShowPartModels;
        private bool _ShowCollisions;
        private bool _ShowConnections;
        private MeshRenderMode _PartRenderMode;
        private bool BatchChangingVisibility;

        public bool ShowPartModels
        {
            get => _ShowPartModels;
            set => SetPartModelsVisibility(value);
        }

        public bool ShowCollisions
        {
            get => _ShowCollisions;
            set => SetCollisionsVisibility(value);
        }

        public bool ShowConnections
        {
            get => _ShowConnections;
            set => SetConnectionsVisibility(value);
        }

        public MeshRenderMode PartRenderMode
        {
            get => _PartRenderMode;
            set => SetPartRenderMode(value);
        }

        public event EventHandler PartModelsVisibilityChanged;

        public event EventHandler CollisionsVisibilityChanged;

        public event EventHandler ConnectionsVisibilityChanged;

        public event EventHandler PartRenderModeChanged;

        private void SetPartModelsVisibility(bool visible)
        {
            if (visible != ShowPartModels)
            {
                Trace.WriteLine($"{nameof(SetPartModelsVisibility)}( visible => {visible} )");

                _ShowPartModels = visible;

                if (IsProjectOpen && IsProjectAttached)
                {
                    //CurrentProject.ProjectProperties["ShowModels"] = visible.ToString();
                    InvalidateElementsVisibility(CurrentProject.Surfaces);
                    RefreshAllNavigation();
                }

                PartModelsVisibilityChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private void SetCollisionsVisibility(bool visible)
        {
            if (visible != ShowCollisions)
            {
                Trace.WriteLine($"{nameof(SetCollisionsVisibility)}( visible => {visible} )");

                _ShowCollisions = visible;

                if (IsProjectOpen && IsProjectAttached)
                {
                    InvalidateElementsVisibility(CurrentProject.GetAllElements<PartCollision>());
                    RefreshAllNavigation();
                }

                CollisionsVisibilityChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private void SetConnectionsVisibility(bool visible)
        {
            if (visible != ShowConnections)
            {
                Trace.WriteLine($"{nameof(SetConnectionsVisibility)}( visible => {visible} )");

                _ShowConnections = visible;

                if (IsProjectOpen && IsProjectAttached)
                {
                    InvalidateElementsVisibility(CurrentProject.GetAllElements<PartConnection>());
                    RefreshAllNavigation();
                }

                ConnectionsVisibilityChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private void SetPartRenderMode(MeshRenderMode renderMode)
        {
            if (renderMode != _PartRenderMode)
            {
                _PartRenderMode = renderMode;
                PartRenderModeChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private void InvalidateElementsVisibility(IEnumerable<PartElement> elements)
        {
            BatchChangingVisibility = true;
            var elemExtensions = elements.Select(x => x.GetExtension<ModelElementExtension>()).ToList();
            elemExtensions.RemoveAll(e => e == null);

            foreach (var elemExt in elemExtensions)
                elemExt.InvalidateVisibility();

            foreach (var elemExt in elemExtensions)
                elemExt.CalculateVisibility();

            var test = elemExtensions.Where(x => x.IsVisibilityDirty);
            BatchChangingVisibility = false;
        }

        #endregion

        public void InitializeElementExtensions()
        {
            if (IsProjectOpen)
            {
                foreach (var elem in CurrentProject.GetAllElements())
                {
                    var modelExt = elem.GetExtension<ModelElementExtension>();
                    if (modelExt != null && modelExt.Manager == null)
                        modelExt.AssignManager(this);
                }
            }
        }

        #region Selection Management

        public void ClearSelection()
        {
            if (_SelectedElements.Any())
            {
                _SelectedElements.Clear();
                SelectionChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public void SelectElement(PartElement element)
        {
            if (element == null)
            {
                ClearSelection();
            }
            else if (!(SelectedElement == element && _SelectedElements.Count == 1))
            {
                _SelectedElements.Clear();
                _SelectedElements.Add(element);
                SelectionChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public void SetSelected(PartElement element, bool selected)
        {
            bool isSelected = SelectedElements.Contains(element);
            if (selected != isSelected)
            {
                if (selected)
                    _SelectedElements.Add(element);
                else
                    _SelectedElements.Remove(element);
                SelectionChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public void SelectElements(IEnumerable<PartElement> elements)
        {
            _SelectedElements.Clear();
            _SelectedElements.AddRange(elements);
            SelectionChanged?.Invoke(this, EventArgs.Empty);
        }

        public IEnumerable<PartElement> GetSelectionHierarchy()
        {
            return SelectedElements.SelectMany(x => x.GetChildsHierarchy(true)).Distinct();
        }

        public bool IsSelected(PartElement element)
        {
            return SelectedElements.Contains(element);
        }

        public bool IsContainedInSelection(PartElement element)
        {
            var allChilds = SelectedElements.SelectMany(x => x.GetChildsHierarchy(true));
            return allChilds.Contains(element);
        }

        public int GetSelectionIndex(PartElement element)
        {
            for (int i = 0; i < _SelectedElements.Count; i++)
            {
                var allChilds = _SelectedElements[i].GetChildsHierarchy(true);
                if (allChilds.Contains(element))
                    return i;
            }
            return -1;
        }

        #endregion

        #region Undo/Redo

        public bool IsExecutingUndoRedo => UndoRedoManager.ExecutingUndoRedo;

        public bool IsExecutingBatchChanges => UndoRedoManager.IsInBatch;

        public bool CanUndo => UndoRedoManager.CanUndo;

        public bool CanRedo => UndoRedoManager.CanRedo;

        public void Undo()
        {
            UndoRedoManager.Undo();
        }

        public void Redo()
        {
            UndoRedoManager.Redo();
        }

        public void StartBatchChanges()
        {
            UndoRedoManager.StartBatchChanges();
        }

        public void StartBatchChanges(string description)
        {
            //TODO
            Trace.WriteLine($"Executing: {description}");
            UndoRedoManager.StartBatchChanges();
        }

        public void EndBatchChanges()
        {
            UndoRedoManager.EndBatchChanges();
            if (ElementsChanged)
            {
                ElementsChanged = false;
                ProjectElementsChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private void UndoRedoManager_UndoHistoryChanged(object sender, EventArgs e)
        {
            ProjectModified?.Invoke(this, EventArgs.Empty);
        }

        private void UndoRedoManager_BeginUndoRedo(object sender, EventArgs e)
        {

        }

        private void UndoRedoManager_EndUndoRedo(object sender, EventArgs e)
        {
            if (ElementsChanged)
            {
                ElementsChanged = false;
                ProjectElementsChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        #endregion

        #region Part Validation Handling

        public bool IsValidatingProject { get; private set; }

        public IList<ValidationMessage> ValidationMessages => _ValidationMessages.AsReadOnly();

        public bool IsPartValidated => IsProjectOpen && LastValidation == UndoRedoManager.CurrentChangeID;

        public bool IsPartValid => IsPartValidated &&
            !ValidationMessages.Any(x => x.Level == ValidationLevel.Error);

        public event EventHandler ValidationStarted;

        public event EventHandler ValidationFinished;

        public void ValidateProject()
        {
            if (IsProjectOpen)
            {
                IsValidatingProject = true;
                ValidationStarted?.Invoke(this, EventArgs.Empty);

                _ValidationMessages.Clear();

                try
                {
                    _ValidationMessages.AddRange(CurrentProject.ValidatePart());
                }
                catch (Exception ex)
                {
                    _ValidationMessages.Add(new ValidationMessage("PROJECT", "UNHANDLED_EXCEPTION", ValidationLevel.Error)
                    {
                        Message = ex.ToString()
                    });
                }

                IsValidatingProject = false;
                LastValidation = UndoRedoManager.CurrentChangeID;

                ValidationFinished?.Invoke(this, EventArgs.Empty);

            }
        }

        #endregion

        #region Editing Validation

        public bool ValidateResizeStud(Custom2DFieldConnector connector, int newWidth, int newHeight)
        {
            //var connection = CurrentProject.GetAllElements<PartConnection>(x => x.Connector == connector).FirstOrDefault();
            var studRefs = CurrentProject.GetAllElements<StudReference>(x => x.Connector == connector).ToList();
            
            if (studRefs.Any(x => x.FieldNode != null && (x.PositionX > newWidth || x.PositionY > newHeight)))
            {
                var dlgResult = MessageBox.Show(
                    Messages.Message_ConfirmResizeStuds, 
                    Messages.Caption_Confirmation, 
                    MessageBoxButtons.YesNo);
                return dlgResult == DialogResult.Yes;
            }

            return true;
        }



        #endregion

        #region Element handling

        

        #endregion

        #region LDD File Generation Handling

        public bool IsGeneratingFiles { get; private set; }

        //todo: move to GenerationFinished eventargs
        public bool GenerationSuccessful { get; private set; }

        public event EventHandler GenerationStarted;

        public event EventHandler<ProjectBuildEventArgs> GenerationFinished;

        public PartWrapper GenerateLddFiles(bool generateOutlines = true)
        {
            if (!IsProjectOpen)
                return null;

            IsGeneratingFiles = true;
            GenerationSuccessful = false;
            GenerationStarted?.Invoke(this, EventArgs.Empty);
            PartWrapper generatedPart = null;
            var messages = new List<ValidationMessage>();

            try
            {
                generatedPart = CurrentProject.GenerateLddPart();
                GenerationSuccessful = true;
            }
            catch (Exception ex)
            {
                messages.Add(new ValidationMessage("PROJECT", "UNHANDLED_EXCEPTION", ValidationLevel.Error)
                {
                    Message = ex.ToString()
                });
            }

            IsGeneratingFiles = false;

            GenerationFinished?.Invoke(this, 
                new ProjectBuildEventArgs(
                    generatedPart, 
                GenerationSuccessful,
                messages
                )
            );

            return generatedPart;
        }

        public string ExpandVariablePath(string path)
        {
            string result = path;

            if (result.Contains("$(LddAppData)"))
            {
                if (string.IsNullOrEmpty(LDDEnvironment.Current?.ApplicationDataPath) || 
                    !Directory.Exists(LDDEnvironment.Current.ApplicationDataPath))
                    throw new ArgumentException("Could not find LDD AppData directory");
                result = result.Replace("$(LddAppData)", LDDEnvironment.Current.ApplicationDataPath);
            }

            if (result.Contains("$(ProjectDir)"))
            {
                if (!IsProjectOpen || !CurrentProject.IsLoadedFromDisk)
                    throw new ArgumentException("Project is not saved to disk.");
                result = result.Replace("$(ProjectDir)", Path.GetDirectoryName(CurrentProject.ProjectPath));
            }

            if (result.Contains("$(WorkspaceDir)"))
            {
                if (!SettingsManager.IsWorkspaceDefined)
                    throw new ArgumentException("Workspace not defined!");
                result = result.Replace("$(WorkspaceDir)", SettingsManager.Current.ProjectWorkspace);
            }
            if (result.Contains("$(PartID)"))
            {
                if (CurrentProject.PartID == 0)
                    throw new ArgumentException("Part ID is not defined");
                result = result.Replace("$(PartID)", CurrentProject.PartID.ToString());
            }

            return result;
        }

        #endregion

        #region Navigation Tree

        public void RebuildNavigationTree()
        {
            foreach (var node in NavigationTreeNodes)
                node.FreeObjects();
            NavigationTreeNodes.Clear();

            NavigationTreeNodes.Add(new ProjectCollectionNode(
                CurrentProject.Surfaces,
                ModelLocalizations.Label_Surfaces));

            if (CurrentProject.Properties.Flexible)
            {
                NavigationTreeNodes.Add(new ProjectCollectionNode(
                    CurrentProject.Bones,
                    ModelLocalizations.Label_Bones));
            }
            else
            {
                NavigationTreeNodes.Add(new ProjectCollectionNode(
                    CurrentProject.Collisions,
                    ModelLocalizations.Label_Collisions));

                NavigationTreeNodes.Add(new ProjectCollectionNode(
                    CurrentProject.Connections,
                    ModelLocalizations.Label_Connections));
            }
        }

        public void RefreshNavigationNode(ProjectTreeNode node)
        {
            if (!BatchChangingVisibility)
                NavigationWindow?.RefreshNavigationNode(node);
        }

        public void RefreshAllNavigation()
        {
            NavigationWindow.RefreshAllNavigation();
        }

        #endregion

        #region Editor Actions

        #region Element Editing

        public PartSurface AddSurface()
        {
            StartBatchChanges($"{nameof(AddSurface)}");
            var newSurface = CurrentProject.AddSurface();
            EndBatchChanges();

            SelectElement(newSurface);

            return newSurface;
        }

        public PartConnection AddConnection(ConnectorType type, PartBone parent)
        {
            if (CurrentProject == null)
                return null;

            StartBatchChanges($"{nameof(AddConnection)}( type => {type}, parent => {parent?.Name ?? "null"})");

            var newConnection = PartConnection.Create(type);
            if (parent != null)
                parent.Connections.Add(newConnection);
            else
                CurrentProject.Connections.Add(newConnection);

            EndBatchChanges();

            if (!ShowConnections)
                ShowConnections = true;

            SelectElement(newConnection);

            return newConnection;
        }

        public PartCollision AddCollision(CollisionType type, PartBone parent)
        {
            if (CurrentProject == null)
                return null;

            StartBatchChanges($"{nameof(AddCollision)}( type => {type}, parent => {parent?.Name ?? "null"})");

            var newCollision = PartCollision.Create(type);
            if (parent != null)
                parent.Collisions.Add(newCollision);
            else
                CurrentProject.Collisions.Add(newCollision);

            EndBatchChanges();

            if (!ShowCollisions)
                ShowCollisions = true;

            SelectElement(newCollision);

            return newCollision;
        }

        public PartElement DuplicateElement(PartElement element)
        {
            PartElement newElement = null;

            StartBatchChanges($"{nameof(DuplicateElement)}( element => {element.Name} )");

            if (element is PartConnection connection)
            {
                newElement = connection.Clone();
                var collection = (element.Parent as PartBone)?.Connections ?? CurrentProject.Connections;
                collection.Add(newElement);
                CurrentProject.RenameElement(newElement, element.Name);
                if (!ShowConnections)
                    ShowConnections = true;
            }
            else if (element is PartCollision collision)
            {
                newElement = collision.Clone();
                var collection = (element.Parent as PartBone)?.Collisions ?? CurrentProject.Collisions;
                collection.Add(newElement);
                CurrentProject.RenameElement(newElement, element.Name);
                if (!ShowCollisions)
                    ShowCollisions = true;
            }

            EndBatchChanges();

            if (newElement != null)
                SelectElement(newElement);


            return newElement;
        }

        public void DeleteElements(IEnumerable<PartElement> elements)
        {
            var elemsToDelete = new List<PartElement>();

            foreach (var elem in elements)
            {
                var dlgResult = ConfirmCanDelete(elem);

                if (dlgResult == DialogResult.Cancel)
                {
                    elemsToDelete.Clear();
                    break;
                }

                if (dlgResult == DialogResult.Yes)
                    elemsToDelete.Add(elem);
            }

            if (elemsToDelete.Any())
            {
                StartBatchChanges(nameof(DeleteElements));
                ClearSelection();

                var removedElements = elemsToDelete.Where(x => x.TryRemove()).ToList();

                if (removedElements.OfType<ModelMeshReference>().Any())
                    CurrentProject.RemoveUnreferencedMeshes();

                EndBatchChanges();
            }
        }

        private DialogResult ConfirmCanDelete(PartElement element)
        {
            if (element is PartConnection conn)
            {
                if (conn.ConnectorType == ConnectorType.Custom2DField)
                {
                    var allStudRefs = CurrentProject.GetAllElements<StudReference>();
                    if (allStudRefs.Any(x => x.ConnectionID == conn.ID))
                    {
                        return MessageBox.Show(
                            string.Format(Messages.Message_ConfirmDeleteStudConnection, conn.Name), 
                            Messages.Caption_DeleteConfirmation, 
                            MessageBoxButtons.YesNoCancel);
                    }
                }
                else if (conn.ConnectorType == ConnectorType.Ball || 
                    conn.ConnectorType == ConnectorType.Fixed)
                {
                    if (CurrentProject.Bones.Any(x => 
                        x.SourceConnectionID == conn.ID ||
                        x.TargetConnectionID == conn.ID))
                    {
                        return MessageBox.Show(
                            string.Format(Messages.Message_ConfirmDeleteBoneConnection, conn.Name), 
                            Messages.Caption_DeleteConfirmation, 
                            MessageBoxButtons.YesNoCancel);
                    }
                }
            }
            if (element is PartBone bone)
            {



            }
            return DialogResult.Yes;
        }

        public void CopySelectedElementsToClipboard()
        {
            if (!IsProjectOpen)
                return;

            string clipboardText = string.Empty;
            foreach (var elem in GetSelectionHierarchy())
            {
                if (elem is PartCollision collision)
                {
                    var colXml = collision.GenerateLDD().SerializeToXml();
                    if (!string.IsNullOrEmpty(clipboardText))
                        clipboardText += Environment.NewLine;
                    clipboardText += colXml.ToString();
                }
                else if (elem is PartConnection connection)
                {

                    var colXml = connection.GenerateLDD().SerializeToXml();
                    if (!string.IsNullOrEmpty(clipboardText))
                        clipboardText += Environment.NewLine;
                    clipboardText += colXml.ToString();
                }
            }

            if (!string.IsNullOrEmpty(clipboardText))
            {
                Clipboard.SetText(clipboardText, TextDataFormat.Text);
            }
        }

        public void HandlePasteFromClipboard()
        {
            if (!IsProjectOpen)
                return;

            var clipboardText = Clipboard.GetText(TextDataFormat.Text);
            if (string.IsNullOrEmpty(clipboardText))
                return;

            clipboardText = $"<Root>{clipboardText}</Root>";
            try
            {
                var tmpXml = XElement.Parse(clipboardText);
                var pastedElements = new List<PartElement>();

                foreach (var elem in tmpXml.Elements())
                {
                    var conn = Connector.DeserializeConnector(elem);
                    if (conn != null)
                    {
                        var connElem = new PartConnection(conn);
                        pastedElements.Add(connElem);
                        continue;
                    }

                    var coll = Collision.DeserializeCollision(elem);
                    if (coll != null)
                    {
                        var collElem = PartCollision.FromLDD(coll);
                        pastedElements.Add(collElem);
                        continue;
                    }
                }

                if (pastedElements.Any())
                {
                    StartBatchChanges(nameof(HandlePasteFromClipboard));
                    var newCollisions = pastedElements.OfType<PartCollision>();
                    var newConnections = pastedElements.OfType<PartConnection>();

                    if (newCollisions.Any() && !ShowCollisions)
                        ShowCollisions = true;

                    if (newConnections.Any() && !ShowConnections)
                        ShowConnections = true;

                    if (SelectedElement is PartBone selectedBone)
                    {
                        if (newCollisions.Any())
                            selectedBone.Collisions.AddRange(newCollisions);
                        if (newConnections.Any())
                            selectedBone.Connections.AddRange(newConnections);
                    }
                    else
                    {
                        if (newCollisions.Any())
                            CurrentProject.Collisions.AddRange(newCollisions);
                        if (newConnections.Any())
                            CurrentProject.Connections.AddRange(newConnections);
                    }

                    EndBatchChanges();

                    SelectElements(pastedElements);
                }
                
            }
            catch { }
        }

        #endregion

        #region Visibility Handling

        public void SetElementHidden(PartElement element, bool hidden)
        {
            var elementExt = element.GetExtension<ModelElementExtension>();

            if (elementExt != null && elementExt.IsHidden != hidden)
            {
                elementExt.IsHidden = hidden;
                elementExt.CalculateVisibility();

                UndoRedoManager.AddEditorAction(new HideElementAction(
                    nameof(HideSelectedElements),
                    new PartElement[] { element },
                    hidden));
            }
        }

        public void HideSelectedElements()
        {
            var hideableElements = SelectedElements.Where(x => x.GetExtension<ModelElementExtension>() != null);
            var hiddenElems = new List<PartElement>();

            foreach (var elem in hideableElements)
            {
                var elementExt = elem.GetExtension<ModelElementExtension>();
                if (elementExt != null && !elementExt.IsHidden)
                {
                    hiddenElems.Add(elem);
                    elementExt.IsHidden = true;
                    elementExt.CalculateVisibility();
                }
            }

            if (hiddenElems.Any())
            {
                UndoRedoManager.AddEditorAction(new HideElementAction(nameof(HideSelectedElements), hiddenElems, true));
            }
        }

        public void HideUnselectedElements()
        {
            var selectedElements = SelectedElements.Select(x => x.GetExtension<ModelElementExtension>()).Where(x => x != null);
            if (!selectedElements.Any(x => x.IsVisible))
                return;

            var hideableElements = CurrentProject.GetAllElements(x => x.GetExtension<ModelElementExtension>() != null);
            var hiddenElems = new List<PartElement>();

            foreach (var elem in hideableElements)
            {
                if (IsContainedInSelection(elem))
                    continue;

                if (elem.GetChildsHierarchy().Any(x => IsSelected(x)))
                    continue;

                var elementExt = elem.GetExtension<ModelElementExtension>();

                if (elementExt != null && !elementExt.IsHidden)
                {
                    hiddenElems.Add(elem);
                    elementExt.IsHidden = true;
                    elementExt.CalculateVisibility();
                }
            }

            if (hiddenElems.Any())
            {
                UndoRedoManager.AddEditorAction(new HideElementAction(nameof(HideSelectedElements), hiddenElems, true));
            }
        }

        public void UnhideEverything()
        {
            var hideableElements = CurrentProject.GetAllElements(x => x.GetExtension<ModelElementExtension>() != null);
            var hiddenElems = new List<PartElement>();

            foreach (var elem in hideableElements)
            {
                var elementExt = elem.GetExtension<ModelElementExtension>();
                if (elementExt != null && elementExt.IsHidden)
                {
                    hiddenElems.Add(elem);
                    elementExt.IsHidden = false;
                    elementExt.CalculateVisibility();
                }
            }

            if (hiddenElems.Any())
            {
                UndoRedoManager.AddEditorAction(new HideElementAction(nameof(HideSelectedElements), hiddenElems, false));
            }
        }

        #endregion

        #region Dialogs

        public void ShowCopyBoneDataDialog()
        {
            if (CurrentProject == null)
                return;

            Trace.WriteLine($"Executing: {nameof(ShowCopyBoneDataDialog)}");

            using (var dlg = new BoneDataCopyDialog(this))
            {
                StartBatchChanges("BoneDataCopyDialog");
                dlg.ShowDialog();
                EndBatchChanges();
            }
        }

        #endregion


        #endregion
    }
}
