using LDDModder.Modding.Editing;
using System;
using System.Collections.Generic;
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
        public ElementCollectionChangedEventArgs Data { get; }

        public CollectionChangeAction(ElementCollectionChangedEventArgs data)
        {
            Data = data;
        }

        public override void Redo()
        {
            if (Data.Action == System.ComponentModel.CollectionChangeAction.Remove)
            {
                foreach (var item in Data.RemovedElements)
                    Data.Collection.Remove(item);
            }
            else if (Data.Action == System.ComponentModel.CollectionChangeAction.Add)
            {
                foreach (var item in Data.ChangedItems)
                    Data.Collection.Insert(item.NewIndex, item.Element);
            }
        }

        public override void Undo()
        {
            if (Data.Action == System.ComponentModel.CollectionChangeAction.Remove)
            {
                foreach (var item in Data.ChangedItems.Reverse())
                    Data.Collection.Insert(item.OldIndex, item.Element);
            }
            else if (Data.Action == System.ComponentModel.CollectionChangeAction.Add)
            {
                foreach (var item in Data.AddedElements)
                    Data.Collection.Remove(item);
            }
        }
    }

    public class PropertyChangeAction : ChangeAction
    {
        public ElementValueChangedEventArgs Data { get; }

        public PropertyChangeAction(ElementValueChangedEventArgs data)
        {
            Data = data;
        }

        private void AssignPropertyValue(object value)
        {
            object objToAssign = Data.ChildProperty ?? Data.Element;
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
