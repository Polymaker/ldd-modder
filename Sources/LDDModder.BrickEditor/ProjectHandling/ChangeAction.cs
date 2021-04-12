using LDDModder.Modding;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.ProjectHandling
{
    public class ChangeAction
    {
        internal long ChangeID;

        public virtual void Undo()
        {

        }

        public virtual void Redo()
        {

        }
    }

    public class CollectionChangeAction : ChangeAction
    {
        public CollectionChangedEventArgs Data { get; }

        public CollectionChangeAction(CollectionChangedEventArgs data)
        {
            Data = data;
        }

        public override void Redo()
        {
            var collection = Data.Collection as System.Collections.IList;
            foreach (var change in Data.ChangedItems)
            {
                switch (change.Action)
                {
                    case CollectionChangeActions.Add:
                        {
                            collection.Insert(change.NewIndex, change.Item);
                            break;
                        }
                    case CollectionChangeActions.Remove:
                        {
                            collection.Remove(change.Item);
                            break;
                        }
                    case CollectionChangeActions.Move:
                        {
                            collection.RemoveAt(change.OldIndex);
                            collection.Insert(change.NewIndex, change.Item);
                            break;
                        }
                }
            }
        }

        public override void Undo()
        {
            var collection = Data.Collection as System.Collections.IList;
            foreach (var change in Data.ChangedItems.Reverse())
            {
                switch (change.Action)
                {
                    case CollectionChangeActions.Add:
                        {
                            collection.Remove(change.Item);
                            
                            break;
                        }
                    case CollectionChangeActions.Remove:
                        {
                            collection.Insert(change.OldIndex, change.Item);
                            break;
                        }
                    case CollectionChangeActions.Move:
                        {
                            collection.RemoveAt(change.NewIndex);
                            collection.Insert(change.OldIndex, change.Item);
                            break;
                        }
                }
            }
        }
    }
    public class PropertyChangeAction : ChangeAction
    {
        public ObjectPropertyChangedEventArgs Data { get; }

        public PropertyChangeAction(ObjectPropertyChangedEventArgs data)
        {
            Data = data;
        }

        private void AssignPropertyValue(object value)
        {
            object objToAssign = Data.Object;
            var propInfo = objToAssign.GetType().GetProperty(Data.PropertyName);

            if (propInfo != null)
            {
                if (Data.Index != null)
                {
                    //TODO: find a way to remove this hard-coded case
                    if (Data.Index.Length == 2 &&
                        objToAssign is LDD.Primitives.Connectors.Custom2DFieldConnector connector)
                    {
                        connector.SetValue(Data.Index[0], Data.Index[1], (LDD.Primitives.Connectors.Custom2DFieldValue)value);
                        return;
                    }
                    var arrayObj = propInfo.GetValue(objToAssign) as Array;
                    if (arrayObj != null)
                        arrayObj.SetValue(value, Data.Index);
                }
                else
                    propInfo.SetValue(objToAssign, value);
            }
        }

        public override void Undo()
        {
            AssignPropertyValue(Data.OldValue);
        }

        public override void Redo()
        {
            AssignPropertyValue(Data.NewValue);
        }
    }

    public class BatchChangeAction : ChangeAction
    {
        public List<ChangeAction> Actions { get; }

        public BatchChangeAction(IEnumerable<ChangeAction> actions)
        {
            Actions = actions.ToList();
        }

        public override void Undo()
        {
            foreach (var action in Actions.Reverse<ChangeAction>())
                action.Undo();
        }

        public override void Redo()
        {
            foreach (var action in Actions)
                action.Redo();
        }
    }

    public abstract class EditorAction : ChangeAction
    {
        public string ActionName { get; set; }

        public EditorAction(string actionName)
        {
            ActionName = actionName;
        }
    }

    public class HideElementAction : EditorAction
    {
        public PartElement[] AffectedElements { get; private set; }
        //public IElementCollection[] AffectedCollections { get; private set; }
        public bool HideState { get; private set; }

        public HideElementAction(string actionName, IEnumerable<PartElement> elements, bool hideStatus) : base(actionName)
        {
            AffectedElements = elements.ToArray();
            //AffectedCollections = new IElementCollection[0];
            HideState = hideStatus;
        }

        //public HideElementAction(string actionName, IElementCollection[] affectedCollections, bool hideState) : base(actionName)
        //{
        //    AffectedCollections = affectedCollections;
        //    AffectedElements = new PartElement[0];
        //    HideState = hideState;
        //}

        public override void Undo()
        {
            foreach (var elem in AffectedElements)
            {
                var elementExt = elem.GetExtension<ModelElementExtension>();
                if (elementExt != null)
                {
                    elementExt.IsHidden = !HideState;
                    elementExt.CalculateVisibility();
                }
            }

            //foreach (var collection in AffectedCollections)
            //{
            //    var elementExt = collection.Owner?.GetExtension<ModelElementExtension>();
            //    if (elementExt != null)
            //    {
            //        elementExt.IsHidden = !HideState;
            //        elementExt.CalculateVisibility();
            //    }
            //}
        }

        public override void Redo()
        {
            foreach (var elem in AffectedElements)
            {
                var elementExt = elem.GetExtension<ModelElementExtension>();
                if (elementExt != null)
                {
                    elementExt.IsHidden = HideState;
                    elementExt.CalculateVisibility();
                }
            }
        }
    }
}
