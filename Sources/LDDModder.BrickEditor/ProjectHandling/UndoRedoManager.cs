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

        //public ProjectDocument Document { get; private set; }

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
            //ProjectManager.ProjectChanged += ProjectManager_ProjectChanged;
            MaxHistory = 15;

            UndoHistory = new List<ChangeAction>();
            RedoHistory = new List<ChangeAction>();
            BatchChanges = new List<ChangeAction>();
        }

        //public UndoRedoManager(ProjectDocument document)
        //{
        //    Document = document;
        //    MaxHistory = 15;

        //    UndoHistory = new List<ChangeAction>();
        //    RedoHistory = new List<ChangeAction>();
        //    BatchChanges = new List<ChangeAction>();
        //}

        //private void ProjectManager_ProjectChanged(object sender, EventArgs e)
        //{
        //    ClearHistory();
        //}

        public void ClearHistory()
        {
            UndoHistory.Clear();
            RedoHistory.Clear();
            HistoryLimitExceeded = false;
            CurrentChangeID = 0;
        }

        internal void ProcessProjectElementsChanged(Modding.Editing.ElementCollectionChangedEventArgs e)
        {
            if (ExecutingUndoRedo)
                return;

            var action = new CollectionChangeAction(e);

            if (IsInBatch)
                BatchChanges.Add(action);
            else
                AddAction(action);
        }

        internal void ProcessElementPropertyChanged(Modding.Editing.ElementValueChangedEventArgs e)
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

        public bool EndBatchChanges()
        {
            BatchNesting--;
            if (BatchNesting == 0)
            {
                IsInBatch = false;
                if (BatchChanges.Any())
                    AddAction(CombineBatchChanges(BatchChanges));

                BatchChanges.Clear();
                return true;
            }
            return false;
        }

        private BatchChangeAction CombineBatchChanges(IEnumerable<ChangeAction> actions)
        {
            var combinedChanges = new List<ChangeAction>();
            CollectionChangeAction prevColChange = null;

            foreach (var action in actions)
            {
                if (action is CollectionChangeAction colChange)
                {
                    if (prevColChange != null)
                    {
                        if (colChange.Data.Collection == prevColChange.Data.Collection &&
                            colChange.Data.Action == prevColChange.Data.Action)
                        {
                            //var prevElements = prevColChange.Data.AddedElements.Concat(prevColChange.Data.RemovedElements);
                            //var currElements = colChange.Data.AddedElements.Concat(colChange.Data.RemovedElements);
                            
                            prevColChange = new CollectionChangeAction(
                                new Modding.Editing.ElementCollectionChangedEventArgs(
                                    colChange.Data.Collection, colChange.Data.Action,
                                    prevColChange.Data.ChangedItems.Concat(colChange.Data.ChangedItems)
                                    ));
                        }
                        else
                        {
                            combinedChanges.Add(prevColChange);
                            prevColChange = colChange;
                        }
                    }
                    else
                    {
                        prevColChange = colChange;
                    }
                }
                else
                {
                    if (prevColChange != null)
                    {
                        combinedChanges.Add(prevColChange);
                        prevColChange = null;
                    }

                    combinedChanges.Add(action);
                }
            }

            if (prevColChange != null)
                combinedChanges.Add(prevColChange);

            return new BatchChangeAction(combinedChanges);
        }

        public void AddEditorAction(EditorAction action)
        {
            if (IsInBatch)
                BatchChanges.Add(action);
            else
                AddAction(action);
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
