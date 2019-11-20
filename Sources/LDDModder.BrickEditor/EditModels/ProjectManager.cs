using LDDModder.BrickEditor.Resources;
using LDDModder.Modding.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.EditModels
{
    public class ProjectManager
    {
        private List<PartElement> _SelectedElements;

        public PartProject CurrentProject { get; private set; }

        public UndoRedoManager UndoRedoManager { get; }

        public bool IsProjectOpen => CurrentProject != null;

        public bool IsNewProject => IsProjectOpen && string.IsNullOrEmpty(CurrentProject.ProjectPath);

        public bool IsExecutingUndoRedo => UndoRedoManager.ExecutingUndoRedo;

        public bool CanUndo => UndoRedoManager.CanUndo;

        public bool CanRedo => UndoRedoManager.CanRedo;

        public bool IsModified => UndoRedoManager.CanUndo || UndoRedoManager.HistoryLimitExceeded;

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

        #region Events

        public event EventHandler SelectionChanged;

        public event EventHandler ProjectClosed;

        public event EventHandler ProjectChanged;

        public event EventHandler<CollectionChangedEventArgs> ProjectElementsChanged;

        public event EventHandler<PropertyChangedEventArgs> ElementPropertyChanged;

        #endregion


        private bool PreventProjectChange;

        public ProjectManager()
        {
            _SelectedElements = new List<PartElement>();
            UndoRedoManager = new UndoRedoManager(this);
        }

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

                if (!PreventProjectChange)
                    ProjectChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private void AttachPartProject(PartProject project)
        {
            project.ElementCollectionChanged += Project_ElementCollectionChanged;
            project.ElementPropertyChanged += Project_ElementPropertyChanged;
        }

        private void DettachPartProject(PartProject project)
        {
            project.ElementCollectionChanged -= Project_ElementCollectionChanged;
            project.ElementPropertyChanged -= Project_ElementPropertyChanged;
        }
        
        private void Project_ElementCollectionChanged(object sender, CollectionChangedEventArgs e)
        {
            if (e.Action == System.ComponentModel.CollectionChangeAction.Remove)
            {
                int count = _SelectedElements.RemoveAll(x => e.RemovedElements.Contains(x));
                if (count > 0)
                    SelectionChanged?.Invoke(this, EventArgs.Empty);
            }
            ProjectElementsChanged?.Invoke(this, e);
        }

        private void Project_ElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ElementPropertyChanged?.Invoke(this, e);
        }

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

            return string.Empty;
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

        public void EndBatchChanges()
        {
            UndoRedoManager.EndBatchChanges();
        }
    }
}
