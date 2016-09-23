using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace LDDModder.Configuration
{
    public class SettingEntry
    {
        // Fields...
        private bool _IsValueChanged;
        private object _Value;
        private object _DefaultValue;
        private string _Key;
        private SettingType _Type;
        private SettingSource _Source;

        public SettingSource Source
        {
            get { return _Source; }
        }
        
        public SettingType Type
        {
            get { return _Type; }
        }
        
        public string Key
        {
            get { return _Key; }
        }

        public object Value
        {
            get { return _Value; }
            set { SetValue(value); }
        }

        public object DefaultValue
        {
            get { return _DefaultValue; }
        }

        public virtual Type ValueType
        {
            get { return Value != null ? Value.GetType() : null; }
        }

        public bool IsValueChanged
        {
            get { return _IsValueChanged; }
        }

        public SettingEntry(SettingSource source, SettingType type, string key, object defaultValue = null)
        {
            if (SettingsManager.GetSettings(source, type).Any(s => s.Key == key))
                throw new Exception();
            _Key = key;
            _Type = type;
            _Source = source;
            _Value = defaultValue;
            _DefaultValue = defaultValue;
            _IsValueChanged = false;
            SettingsManager._Settings.Add(this);
        }

        private void SetValue(object value)
        {
            if (CheckValueChanged(_Value, value))
                _IsValueChanged = true;
            _Value = value;
        }

        protected virtual bool CheckValueChanged(object oldValue, object newValue)
        {
            if ((oldValue == null || newValue == null) && !ReferenceEquals(oldValue, newValue))
                return true;

            if (ReferenceEquals(oldValue, newValue))
                return false;

            return true;
        }

        public void DeserializeValue(string value)
        {
            OnDeserializeValue(value);
            _IsValueChanged = false;
        }

        protected virtual void OnDeserializeValue(string value)
        {
            _Value = value;
        }
    }

    public class SettingEntry<T> : SettingEntry
    {
        public new T Value
        {
            get { return (T)base.Value; }
            set { base.Value = value; }
        }

        public override Type ValueType
        {
            get { return typeof(T); }
        }

        public SettingEntry(SettingSource source, SettingType type, string key) : base(source, type, key) { }

        public SettingEntry(SettingSource source, SettingType type, string key, T defaultValue) : base(source, type, key, defaultValue) { }

        public static implicit operator T(SettingEntry<T> entry)
        {
            return entry.Value;
        }

        protected override void OnDeserializeValue(string value)
        {
            if (ValueType == typeof(string))
            {
                base.OnDeserializeValue(value);
                return;
            }

            if (ValueType.Name.Contains("Int"))
            {
                long lValue;
                if(long.TryParse(value, out lValue))
                    Value = (T)Convert.ChangeType(lValue, ValueType);
                return;
            }

            if (ValueType.Name == "Decimal")
            {
                decimal dValue = 0;
                if (decimal.TryParse(value, out dValue))
                    Value = (T)Convert.ChangeType(dValue, ValueType);
                return;
            }

            if (ValueType.Name == "Single")
            {
                float fValue = 0;
                if (float.TryParse(value, out fValue))
                    Value = (T)Convert.ChangeType(fValue, ValueType);
                return;
            }

            if (ValueType == typeof(bool))
            {
                var cleanVal = value.ToLower().Trim();
                Value = (T)(object)(new string[] { "1", "yes", "true" }.Contains(cleanVal));
                return;
            }

            if (DefaultValue != null)
                Value = (T)DefaultValue;
        }

        protected override bool CheckValueChanged(object oldValue, object newValue)
        {
            if (oldValue == null || newValue == null)
                return !ReferenceEquals(oldValue, newValue);

            if (oldValue is IEqualityComparer<T>)
            {
                return !((IEqualityComparer<T>)oldValue).Equals(newValue);
            }
            else if (oldValue is IEquatable<T>)
            {
                return !((IEquatable<T>)oldValue).Equals(newValue);
            }

            return base.CheckValueChanged(oldValue, newValue);
        }

    }
}
