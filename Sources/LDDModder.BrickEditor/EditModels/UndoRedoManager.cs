using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.EditModels
{
    public class UndoRedoManager
    {
        public ProjectManager ProjectManager { get; }

        public int MaxHistory { get; set; }

        public bool ExecutingUndoRedo { get; private set; }

        private List<ChangeAction> UndoHistory { get; }

        private List<ChangeAction> RedoHistory { get; }

        public bool CanUndo => UndoHistory.Any();

        public bool CanRedo => RedoHistory.Any();

        private List<ChangeAction> BatchChanges;

        private bool IsInBatch;

        public event EventHandler UndoHistoryChanged;

        public UndoRedoManager(ProjectManager projectManager)
        {
            ProjectManager = projectManager;
            ProjectManager.ProjectChanged += ProjectManager_ProjectChanged;
            ProjectManager.ElementPropertyChanged += ProjectManager_ElementPropertyChanged;
            ProjectManager.ProjectElementsChanged += ProjectManager_ProjectElementsChanged;
            MaxHistory = 15;

            UndoHistory = new List<ChangeAction>();
            RedoHistory = new List<ChangeAction>();
            BatchChanges = new List<ChangeAction>();
        }

        private void ProjectManager_ProjectChanged(object sender, EventArgs e)
        {
            UndoHistory.Clear();
            RedoHistory.Clear();
        }

        private void ProjectManager_ProjectElementsChanged(object sender, Modding.Editing.CollectionChangedEventArgs e)
        {
            if (ExecutingUndoRedo)
                return;

            var action = new CollectionChangeAction(e);

            if (IsInBatch)
                BatchChanges.Add(action);
            else
                AddAction(action);
        }

        private void ProjectManager_ElementPropertyChanged(object sender, Modding.Editing.PropertyChangedEventArgs e)
        {
            if (ExecutingUndoRedo)
                return;

            var action = new PropertyChangeAction(e);
            if (IsInBatch)
                BatchChanges.Add(action);
            else
                AddAction(action);
        }

        public void StartBatchChanges()
        {
            IsInBatch = true;
        }

        public void EndBatchChanges()
        {
            IsInBatch = false;
            if (BatchChanges.Any())
                AddAction(new BatchChangeAction(BatchChanges));
            BatchChanges.Clear();
        }
        
        private void AddAction(ChangeAction action)
        {
            RedoHistory.Clear();
            UndoHistory.Add(action);
            if (UndoHistory.Count > MaxHistory)
                UndoHistory.RemoveAt(0);

            UndoHistoryChanged?.Invoke(this, EventArgs.Empty);
        }

        public void Undo()
        {
            if (UndoHistory.Any())
            {
                var lastAction = UndoHistory.Last();
                UndoHistory.Remove(lastAction);
                RedoHistory.Add(lastAction);
                ExecutingUndoRedo = true;
                lastAction.Undo();
                ExecutingUndoRedo = false;
                UndoHistoryChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Redo()
        {
            if (RedoHistory.Any())
            {
                var lastAction = RedoHistory.Last();
                RedoHistory.Remove(lastAction);
                UndoHistory.Add(lastAction);
                ExecutingUndoRedo = true;
                lastAction.Redo();
                ExecutingUndoRedo = false;
                UndoHistoryChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
