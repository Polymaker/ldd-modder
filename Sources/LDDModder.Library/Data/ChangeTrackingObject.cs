using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace System.ComponentModel
{
    public abstract class ChangeTrackingObject : 
        INotifyPropertyValueChanged, 
        INotifyPropertyChanged,
        INotifyPropertyChanging
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyValueChangedEventHandler PropertyValueChanged;

        public event PropertyChangingEventHandler PropertyChanging;
        public event PropertyValueChangingEventHandler PropertyValueChanging;

        public event ForwardedEventHandler ChildEventForwarded;

        public event CollectionChangedEventHandler CollectionChanged;

        private List<IChangeTrackingCollection> _Collections;

        private HashSet<ChildEventHook> HookedObjects;

        protected delegate void ChildEventHandler(ChildEventHook hook, string eventName, EventArgs eventArgs);

        protected class ChildEventHook : IDisposable
        {
            public string PropertyName { get; }
            public ChangeTrackingObject Value { get; }
            public ChildEventHandler ChildEventHandler { get; private set; }

            public ChildEventHook(string propertyName, ChangeTrackingObject value)
            {
                PropertyName = propertyName;
                Value = value;
            }

            public override bool Equals(object obj)
            {
                return obj is ChildEventHook hook &&
                       PropertyName == hook.PropertyName &&
                       Value == hook.Value;
            }

            public override int GetHashCode()
            {
                var hashCode = -295901811;
                hashCode = hashCode * -1521134295 + PropertyName.GetHashCode();
                hashCode = hashCode * -1521134295 + Value.GetHashCode();
                return hashCode;
            }

            public void Attach(ChildEventHandler eventHandler)
            {
                ChildEventHandler = eventHandler;
                Value.CollectionChanged += Value_CollectionChanged;
                Value.PropertyValueChanging += Value_PropertyValueChanging;
                Value.PropertyValueChanged += Value_PropertyValueChanged;
                Value.ChildEventForwarded += Value_ChildEventForwarded;
            }

            private void Value_ChildEventForwarded(object sender, ForwardedEventArgs e)
            {
                ChildEventHandler(this, nameof(ChildEventForwarded), e);
            }

            private void Value_PropertyValueChanging(object sender, PropertyValueChangingEventArgs e)
            {
                ChildEventHandler(this, nameof(PropertyValueChanging), e);
            }

            private void Value_PropertyValueChanged(object sender, PropertyValueChangedEventArgs e)
            {
                ChildEventHandler(this, nameof(PropertyChanged), e);
            }

            private void Value_CollectionChanged(object sender, CollectionChangedEventArgs ccea)
            {
                ChildEventHandler(this, nameof(CollectionChanged), ccea);
            }

            public void Dettach()
            {
                if (ChildEventHandler != null)
                {
                    Value.CollectionChanged -= Value_CollectionChanged;
                    Value.PropertyValueChanging -= Value_PropertyValueChanging;
                    Value.PropertyValueChanged -= Value_PropertyValueChanged;
                    Value.ChildEventForwarded -= Value_ChildEventForwarded;
                    ChildEventHandler = null;
                }
            }

            public void Dispose()
            {
                Dettach();
            }
        }

        public ChangeTrackingObject()
        {
            HookedObjects = new HashSet<ChildEventHook>();
            _Collections = new List<IChangeTrackingCollection>();
        }

        protected bool SetPropertyValue<T>(ref T property, T value, [CallerMemberName] string propertyName = null)
        {
            if (!AreEquals(property, value))
            {
                object oldValue = property;

                if (oldValue is ChangeTrackingObject oldTracking)
                    DettachChildEvents(oldTracking, propertyName);

                OnPropertyValueChanging(new PropertyValueChangingEventArgs(propertyName, oldValue, value));

                property = value;

                OnPropertyValueChanged(new PropertyValueChangedEventArgs(propertyName, oldValue, value));

                if (value is ChangeTrackingObject newTracking)
                    AttachChildEvents(newTracking, propertyName);

                return true;
            }
            return false;
        }

        protected bool SetPropertyValue<T>(ref T[] property, int index, T value, [CallerMemberName] string propertyName = null)
        {
            if (!AreEquals(property[index], value))
            {
                object oldValue = property[index];

                if (oldValue is ChangeTrackingObject oldTracking)
                    DettachChildEvents(oldTracking, propertyName);

                OnPropertyValueChanging(new PropertyValueChangingEventArgs(propertyName, new int[] { index }, oldValue, value));

                property[index] = value;

                OnPropertyValueChanged(new PropertyValueChangedEventArgs(propertyName, new int[] { index }, oldValue, value));

                if (value is ChangeTrackingObject newTracking)
                    AttachChildEvents(newTracking, propertyName);

                return true;
            }
            return false;
        }

        //protected bool SetIndexedPropertyValue<T>(object[] index, T value, [CallerMemberName] string propertyName = null)
        //{
        //    var propInfo = GetType().GetProperty(propertyName);
        //    var oldValue = propInfo.GetValue(this, index);
            
        //    if (!AreEquals(oldValue, value))
        //    {
        //        if (oldValue is ChangeTrackingObject oldTracking)
        //            DettachChildEvents(oldTracking);

        //        RaisePropertyValueChanging(new PropertyValueChangingEventArgs(propertyName, index, oldValue, value));

        //        propInfo.SetValue(this, value, index);

        //        RaisePropertyValueChanged(new PropertyValueChangedEventArgs(propertyName, index, oldValue, value));

        //        if (value is ChangeTrackingObject newTracking)
        //            AttachChildEvents(newTracking);

        //        return true;
        //    }

        //    return false;
        //}

        protected bool SetIndexedPropertyValue<T>(ref T[] property, int index, T value, [CallerMemberName] string propertyName = null)
        {
            var oldValue = property[index];

            if (!AreEquals(oldValue, value))
            {
                if (oldValue is ChangeTrackingObject oldTracking)
                    DettachChildEvents(oldTracking, propertyName);

                OnPropertyValueChanging(new PropertyValueChangingEventArgs(propertyName, new int[] { index }, oldValue, value));

                property[index] = value;

                OnPropertyValueChanged(new PropertyValueChangedEventArgs(propertyName, new int[] { index }, oldValue, value));

                if (value is ChangeTrackingObject newTracking)
                    AttachChildEvents(newTracking, propertyName);

                return true;
            }

            return false;
        }

        protected bool SetIndexedPropertyValue<T>(ref T[,] property, int[] index, T value, [CallerMemberName] string propertyName = null)
        {
            var oldValue = property[index[0], index[1]];

            if (!AreEquals(oldValue, value))
            {
                if (oldValue is ChangeTrackingObject oldTracking)
                    DettachChildEvents(oldTracking, propertyName);

                OnPropertyValueChanging(new PropertyValueChangingEventArgs(propertyName, index, oldValue, value));

                property[index[0], index[1]] = value;

                OnPropertyValueChanged(new PropertyValueChangedEventArgs(propertyName, index, oldValue, value));

                if (value is ChangeTrackingObject newTracking)
                    AttachChildEvents(newTracking, propertyName);

                return true;
            }

            return false;
        }

        public static bool AreEquals<T>(T value1, T value2)
        {
            if (typeof(T).IsArray)
            {
                if (value1 is Array array1 && value2 is Array array2)
                {
                    if (array1.Length != array2.Length)
                        return false;

                    for (int i = 0; i < array1.Length; i++)
                    {
                        if (!Equals(array1.GetValue(i), array2.GetValue(i)))
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }

            return EqualityComparer<T>.Default.Equals(value1, value2);
        }


        #region Child Object Tracking

        //private bool IsContainedInHookChain(ChangeTrackingObject trackingObject)
        //{
        //    if (HookedObjects.Count == 0)
        //        return false;
        //    if (HookedObjects.Any(x => x.Value == trackingObject))
        //        return true;

        //    foreach (var childObj in HookedObjects)
        //    {
        //        if (childObj.Value.IsContainedInHookChain(trackingObject))
        //            return true;
        //    }
        //    return false;
        //}

        private void AttachChildEvents(ChangeTrackingObject trackingObject, string propertyName = null)
        {
            var childHook = new ChildEventHook(propertyName, trackingObject);
            
            if (!HookedObjects.Contains(childHook))
            {
                HookedObjects.Add(childHook);
                childHook.Attach(OnChildEventRaised);
            }

            OnChildObjectAssigned(trackingObject);
        }

        protected virtual void OnChildObjectAssigned(ChangeTrackingObject childObject)
        {

        }

        private void OnChildEventRaised(ChildEventHook hook, string eventName, EventArgs args)
        {
            if (args is ForwardedEventArgs fea && fea.Depth > 10)
                return;

            if (args is PropertyValueChangedEventArgs ev1)
                OnChildPropertyValueChanged(hook.PropertyName, hook.Value, ev1);
            else if (args is PropertyValueChangingEventArgs ev2)
                OnChildPropertyValueChanging(hook.PropertyName, hook.Value, ev2);

            OnChildObjectEventRaised(new ForwardedEventArgs(hook.PropertyName, hook.Value, "PropertyValueChanging", args)
            {
                Depth = (args is ForwardedEventArgs fea2) ? fea2.Depth + 1 : 0
            });
        }

        private void DettachChildEvents(ChangeTrackingObject trackingObject, string propertyName = null)
        {
            var childHook = HookedObjects.FirstOrDefault(x => x.Value == trackingObject && x.PropertyName == propertyName);
            if (childHook != null)
            {
                childHook.Dettach();
                HookedObjects.Remove(childHook);
            }

            OnChildObjectRemoved(trackingObject);
        }

        protected virtual void OnChildObjectRemoved(ChangeTrackingObject childObject)
        {

        }

        protected virtual void OnChildPropertyValueChanging(string propertyName, object childObject, PropertyValueChangingEventArgs args)
        {
           
        }

        protected virtual void OnChildPropertyValueChanged(string propertyName, object childObject, PropertyValueChangedEventArgs args)
        {
            
        }

        #endregion

        protected void TrackCollectionChanges(IChangeTrackingCollection collection)
        {
            if (!_Collections.Contains(collection))
            {
                _Collections.Add(collection);
                collection.CollectionChanged += OnCollectionChanged;
            }
        }
        protected void UntrackCollectionChanges(IChangeTrackingCollection collection)
        {
            if (_Collections.Contains(collection))
            {
                _Collections.Remove(collection);
                collection.CollectionChanged -= OnCollectionChanged;
            }
        }
        internal void AttachCollection(IChangeTrackingCollection collection)
        {
            TrackCollectionChanges(collection);
        }

        protected virtual void OnCollectionChanged(object sender, CollectionChangedEventArgs ccea)
        {
            CollectionChanged?.Invoke(this, ccea);
        }

        protected virtual void OnPropertyValueChanging(PropertyValueChangingEventArgs args)
        {
            PropertyChanging?.Invoke(this, args);
            PropertyValueChanging?.Invoke(this, args);
        }

        protected virtual void OnPropertyValueChanged(PropertyValueChangedEventArgs args)
        {
            PropertyChanged?.Invoke(this, args);
            PropertyValueChanged?.Invoke(this, args);
        }

        protected virtual void OnChildObjectEventRaised(ForwardedEventArgs args)
        {
            ChildEventForwarded?.Invoke(this, args);
        }
    }
}
