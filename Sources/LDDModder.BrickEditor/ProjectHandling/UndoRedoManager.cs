using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.ProjectHandling
{
    public class UndoRedoManager
    {
        public ProjectManager ProjectManager { get; }

        public long CurrentChangeID { get; private set; }

        public int MaxHistory { get; set; }

        public bool ExecutingUndoRedo { get; private set; }

        public bool HistoryLimitExceeded { get; private set; }

        private List<ChangeAction> UndoHistory { get; }

        private List<ChangeAction> RedoHistory { get; }

        public bool CanUndo => UndoHistory.Any();

        public bool CanRedo => RedoHistory.Any();

        private List<ChangeAction> BatchChanges;

        private int BatchNesting;

        public bool IsInBatch { get; private set; }

        public event EventHandler UndoHistoryChanged;

        public event EventHandler BeginUndoRedo;

        public event EventHandler EndUndoRedo;

        public UndoRedoManager(ProjectManager projectManager)
        {
            ProjectManager = projectManager;
            ProjectManager.ProjectChanged += ProjectManager_ProjectChanged;
            ProjectManager.ElementPropertyChanged += ProjectManager_ElementPropertyChanged;
            ProjectManager.ElementCollectionChanged += ProjectManager_ProjectElementsChanged;
            MaxHistory = 15;

            UndoHistory = new List<ChangeAction>();
            RedoHistory = new List<ChangeAction>();
            BatchChanges = new List<ChangeAction>();
        }

        private void ProjectManager_ProjectChanged(object sender, EventArgs e)
        {
            UndoHistory.Clear();
            RedoHistory.Clear();
            HistoryLimitExceeded = false;
            CurrentChangeID = 0;
        }

        private void ProjectManager_ProjectElementsChanged(object sender, Modding.Editing.ElementCollectionChangedEventArgs e)
        {
            if (ExecutingUndoRedo)
                return;

            var action = new CollectionChangeAction(e);

            if (IsInBatch)
                BatchChanges.Add(action);
            else
                AddAction(action);
        }

        private void ProjectManager_ElementPropertyChanged(object sender, Modding.Editing.ElementValueChangedEventArgs e)
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
            BatchNesting++;
            IsInBatch = true;
        }

        public void EndBatchChanges()
        {
            BatchNesting--;
            if (BatchNesting == 0)
            {
                IsInBatch = false;
                if (BatchChanges.Any())
                    AddAction(new BatchChangeAction(BatchChanges));
                BatchChanges.Clear();
            }
        }

        private void AddAction(ChangeAction action)
        {
            RedoHistory.Clear();
            UndoHistory.Add(action);
            action.ChangeID = ++CurrentChangeID;

            if (UndoHistory.Count > MaxHistory)
            {
                HistoryLimitExceeded = true;
                UndoHistory.RemoveAt(0);
            }

            UndoHistoryChanged?.Invoke(this, EventArgs.Empty);
        }

        public void Undo()
        {
            if (UndoHistory.Any())
            {
                BeginUndoRedo?.Invoke(this, EventArgs.Empty);

                var lastAction = UndoHistory.Last();
                CurrentChangeID = lastAction.ChangeID - 1;

                UndoHistory.Remove(lastAction);
                RedoHistory.Add(lastAction);

                ExecutingUndoRedo = true;
                lastAction.Undo();
                ExecutingUndoRedo = false;

                EndUndoRedo?.Invoke(this, EventArgs.Empty);
                UndoHistoryChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Redo()
        {
            if (RedoHistory.Any())
            {
                BeginUndoRedo?.Invoke(this, EventArgs.Empty);

                var lastAction = RedoHistory.Last();
                CurrentChangeID = lastAction.ChangeID;

                RedoHistory.Remove(lastAction);
                UndoHistory.Add(lastAction);

                ExecutingUndoRedo = true;
                lastAction.Redo();
                ExecutingUndoRedo = false;

                EndUndoRedo?.Invoke(this, EventArgs.Empty);
                UndoHistoryChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
