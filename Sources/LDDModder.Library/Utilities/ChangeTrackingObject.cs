using System;
using System.Collections.Generic;
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

        protected bool SetPropertyValue<T>(ref T property, T value, [CallerMemberName] string propertyName = null)
        {
            if (!AreEquals(property, value))
            {
                object oldValue = property;

                if (oldValue is ChangeTrackingObject oldTracking)
                    DettachChildEvents(oldTracking);

                RaisePropertyValueChanging(new PropertyValueChangingEventArgs(propertyName, oldValue, value));

                property = value;

                RaisePropertyValueChanged(new PropertyValueChangedEventArgs(propertyName, oldValue, value));

                if (value is ChangeTrackingObject newTracking)
                    AttachChildEvents(newTracking);

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
                    DettachChildEvents(oldTracking);

                RaisePropertyValueChanging(new PropertyValueChangingEventArgs(propertyName, new int[] { index }, oldValue, value));

                property[index] = value;

                RaisePropertyValueChanged(new PropertyValueChangedEventArgs(propertyName, new int[] { index }, oldValue, value));

                if (value is ChangeTrackingObject newTracking)
                    AttachChildEvents(newTracking);

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
                    DettachChildEvents(oldTracking);

                RaisePropertyValueChanging(new PropertyValueChangingEventArgs(propertyName, new int[] { index }, oldValue, value));

                property[index] = value;

                RaisePropertyValueChanged(new PropertyValueChangedEventArgs(propertyName, new int[] { index }, oldValue, value));

                if (value is ChangeTrackingObject newTracking)
                    AttachChildEvents(newTracking);

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
                    DettachChildEvents(oldTracking);

                RaisePropertyValueChanging(new PropertyValueChangingEventArgs(propertyName, index, oldValue, value));

                property[index[0], index[1]] = value;

                RaisePropertyValueChanged(new PropertyValueChangedEventArgs(propertyName, index, oldValue, value));

                if (value is ChangeTrackingObject newTracking)
                    AttachChildEvents(newTracking);

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

        private void AttachChildEvents(ChangeTrackingObject trackingObject)
        {
            trackingObject.PropertyValueChanging += ChildObject_PropertyValueChanging;
            trackingObject.PropertyValueChanged += ChildObject_PropertyValueChanged;
            trackingObject.ChildEventForwarded += ChildObject_ChildEventForwarded;
        }

        private void DettachChildEvents(ChangeTrackingObject trackingObject)
        {
            trackingObject.PropertyValueChanging -= ChildObject_PropertyValueChanging;
            trackingObject.PropertyValueChanged -= ChildObject_PropertyValueChanged;
            trackingObject.ChildEventForwarded -= ChildObject_ChildEventForwarded;
        }

        private void ChildObject_PropertyValueChanging(object sender, PropertyValueChangingEventArgs e)
        {
            RaiseChildObjectEvent(new ForwardedEventArgs(sender, "PropertyValueChanging", e));
        }

        private void ChildObject_PropertyValueChanged(object sender, PropertyValueChangedEventArgs e)
        {
            RaiseChildObjectEvent(new ForwardedEventArgs(sender, "PropertyValueChanged", e));
        }

        private void ChildObject_ChildEventForwarded(object sender, ForwardedEventArgs e)
        {
            RaiseChildObjectEvent(new ForwardedEventArgs(sender, "ChildEventForwarded", e));
        }

        #endregion

        protected void RaisePropertyValueChanging(PropertyValueChangingEventArgs args)
        {
            PropertyChanging?.Invoke(this, args);
            PropertyValueChanging?.Invoke(this, args);
        }

        protected void RaisePropertyValueChanged(PropertyValueChangedEventArgs args)
        {
            PropertyChanged?.Invoke(this, args);
            PropertyValueChanged?.Invoke(this, args);
        }

        protected void RaiseChildObjectEvent(ForwardedEventArgs args)
        {
            ChildEventForwarded?.Invoke(this, args);
        }
    }
}
