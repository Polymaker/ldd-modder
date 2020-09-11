using LDDModder.BrickEditor.Models.Navigation;
using LDDModder.BrickEditor.Rendering;
using LDDModder.BrickEditor.Resources;
using LDDModder.Modding.Editing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.ProjectHandling
{
    public class ProjectDocument : IProjectDocument, IDisposable
    {

        private List<PartElement> _SelectedElements;
        private List<ValidationMessage> _ValidationMessages;
        private long LastValidation;
        private long LastSavedChange;

        public PartProject Project { get; private set; }

        public ProjectManager Manager { get; set; }

        public bool IsActiveDocument => Manager?.ActiveDocument == this;

        public ProjectTreeNodeCollection NavigationTreeNodes { get; private set; }

        public UndoRedoManager UndoRedoManager { get; private set; }

        public bool IsNewProject => string.IsNullOrEmpty(Project.ProjectPath);

        public bool IsModified => LastSavedChange != UndoRedoManager.CurrentChangeID;

        private bool PreventProjectChange;
        private bool IsProjectAttached;

        #region Events

        public event EventHandler SelectionChanged;

        public event EventHandler Modified;

        public event EventHandler<ElementCollectionChangedEventArgs> ElementCollectionChanged;

        public event EventHandler<ElementValueChangedEventArgs> ElementPropertyChanged;

        public event EventHandler ElementsChanged;

        #endregion

        public ProjectDocument(PartProject project)
        {
            Project = project;
            _SelectedElements = new List<PartElement>();
            _ValidationMessages = new List<ValidationMessage>();
            NavigationTreeNodes = new ProjectTreeNodeCollection(this);
            UndoRedoManager = new UndoRedoManager(this);
            AttachPartProject();
        }

        #region Project Events

        private void AttachPartProject()
        {
            Project.ElementCollectionChanged += Project_ElementCollectionChanged;
            Project.ElementPropertyChanged += Project_ElementPropertyChanged;
            Project.UpdateModelStatistics();

            LastValidation = -1;
            IsProjectAttached = true;
            InitializeElementExtensions();
        }

        private void DettachPartProject()
        {
            Project.ElementCollectionChanged -= Project_ElementCollectionChanged;
            Project.ElementPropertyChanged -= Project_ElementPropertyChanged;

            NavigationTreeNodes.Clear();
            _SelectedElements.Clear();
            _ValidationMessages.Clear();
            LastSavedChange = 0;
            LastValidation = -1;
            IsProjectAttached = false;
        }

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
                InitializeElementExtensions();
            }

            ElementCollectionChanged?.Invoke(this, e);

            UndoRedoManager.ProcessProjectElementsChanged(e);

            if (IsExecutingUndoRedo || IsExecutingBatchChanges)
                ElementsChangedWhilePerformingChanges = true;
            else
                ElementsChanged?.Invoke(this, EventArgs.Empty);
        }

        private void Project_ElementPropertyChanged(object sender, ElementValueChangedEventArgs e)
        {
            ElementPropertyChanged?.Invoke(this, e);
            UndoRedoManager.ProcessElementPropertyChanged(e);
        }

        #endregion

        #region Undo/Redo

        private bool ElementsChangedWhilePerformingChanges;

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
            if (ElementsChangedWhilePerformingChanges)
            {
                ElementsChangedWhilePerformingChanges = false;
                ElementsChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private void UndoRedoManager_UndoHistoryChanged(object sender, EventArgs e)
        {
            Modified?.Invoke(this, EventArgs.Empty);
        }

        private void UndoRedoManager_BeginUndoRedo(object sender, EventArgs e)
        {

        }

        private void UndoRedoManager_EndUndoRedo(object sender, EventArgs e)
        {
            if (ElementsChangedWhilePerformingChanges)
            {
                ElementsChangedWhilePerformingChanges = false;
                ElementsChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        #endregion

        #region Element Extensions

        public void InitializeElementExtensions()
        {
            foreach (var elem in Project.GetAllElements())
            {
                var modelExt = elem.GetExtension<ModelElementExtension>();
                if (modelExt != null && modelExt.Manager == null)
                    modelExt.AssignDocument(this);
            }
        }

        #endregion

        #region Selection Management

        public PartElement SelectedElement
        {
            get => _SelectedElements.FirstOrDefault();
            set => SelectElement(value);
        }

        public IList<PartElement> SelectedElements => _SelectedElements.AsReadOnly();

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

        #region Navigation Tree

        public void RebuildNavigationTree()
        {
            foreach (var node in NavigationTreeNodes)
                node.FreeObjects();
            NavigationTreeNodes.Clear();

            NavigationTreeNodes.Add(new ProjectCollectionNode(
                Project.Surfaces,
                ModelLocalizations.Label_Surfaces));

            if (Project.Properties.Flexible)
            {
                NavigationTreeNodes.Add(new ProjectCollectionNode(
                    Project.Bones,
                    ModelLocalizations.Label_Bones));
            }
            else
            {
                NavigationTreeNodes.Add(new ProjectCollectionNode(
                    Project.Collisions,
                    ModelLocalizations.Label_Collisions));

                NavigationTreeNodes.Add(new ProjectCollectionNode(
                    Project.Connections,
                    ModelLocalizations.Label_Connections));
            }
        }

        public void RefreshNavigationNode(ProjectTreeNode node)
        {
            if (IsActiveDocument)
                Manager.NavigationWindow?.RefreshNavigationNode(node);
        }

        #endregion

        #region Saving / Loading

        public void SaveProject(string targetPath)
        {
            Project.Save(targetPath);
            Project.ProjectPath = targetPath;
            //LastSavedChange = UndoRedoManager.CurrentChangeID;
            //ProjectModified?.Invoke(this, EventArgs.Empty);
        }

        public void SaveWorkingProject()
        {
            var projectXml = Project.GenerateProjectXml();
            projectXml.Save(Path.Combine(Project.ProjectWorkingDir, PartProject.ProjectFileName));
        }

        #endregion

        #region Viewport Display Handling

        private bool _ShowPartModels;
        private bool _ShowCollisions;
        private bool _ShowConnections;
        private MeshRenderMode _PartRenderMode;

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

                InvalidateElementsVisibility(Project.Surfaces);

                PartModelsVisibilityChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private void SetCollisionsVisibility(bool visible)
        {
            if (visible != ShowCollisions)
            {
                Trace.WriteLine($"{nameof(SetCollisionsVisibility)}( visible => {visible} )");

                _ShowCollisions = visible;

                InvalidateElementsVisibility(Project.Collisions);
                InvalidateElementsVisibility(Project.Bones);

                CollisionsVisibilityChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private void SetConnectionsVisibility(bool visible)
        {
            if (visible != ShowConnections)
            {
                Trace.WriteLine($"{nameof(SetConnectionsVisibility)}( visible => {visible} )");

                _ShowConnections = visible;

                InvalidateElementsVisibility(Project.Connections);
                InvalidateElementsVisibility(Project.Bones);

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
            foreach (var elem in elements)
                elem.GetExtension<ModelElementExtension>()?.InvalidateVisibility();
        }

        #endregion

        public string GetProjectDisplayName()
        {
            if (Project.PartID > 0 && !string.IsNullOrEmpty(Project.PartDescription))
                return $"{Project.PartID} - {Project.PartDescription}";
            else if (Project.PartID > 0)
                return $"{ModelLocalizations.Label_Part} {Project.PartID}";
            else if (!string.IsNullOrEmpty(Project.PartDescription))
                return $"{Project.PartDescription}";
            else
                return ModelLocalizations.Label_NewPartProject;
        }

        public void Dispose()
        {
            if (IsProjectAttached)
                DettachPartProject();
        }
    }
}
