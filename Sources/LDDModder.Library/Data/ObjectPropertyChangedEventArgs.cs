namespace System.ComponentModel
{
    public class ObjectPropertyChangedEventArgs : PropertyValueChangedEventArgs
    {
        public object Object { get; }

        public ObjectPropertyChangedEventArgs(object @object, string propertyName, object oldValue, object newValue) : base(propertyName, oldValue, newValue)
        {
            Object = @object;
        }

        public ObjectPropertyChangedEventArgs(object @object, string propertyName, int[] index, object oldValue, object newValue) : base(propertyName, index, oldValue, newValue)
        {
            Object = @object;
        }

        public ObjectPropertyChangedEventArgs(object @object, PropertyValueChangedEventArgs pvcea)
            : base(pvcea.PropertyName, pvcea.Index, pvcea.OldValue, pvcea.NewValue)
        {
            Object = @object;
        }

    }
}
