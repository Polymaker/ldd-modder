using LDDModder.LDD.Primitives.Connectors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.Modding.Editing
{
    class ConnectorProxy : DynamicObject, INotifyPropertyValueChanged
    {
        public Connector Connector { get; }

        private Dictionary<string, PropertyInfo> Properties;

        public event PropertyValueChangedEventHandler PropertyValueChanged;

        public ConnectorProxy(Connector connector)
        {
            Connector = connector;
            Properties = new Dictionary<string, PropertyInfo>();
        }

        private PropertyInfo GetProperty(string propertyName)
        {
            if (Properties.TryGetValue(propertyName, out PropertyInfo propInfo))
                return propInfo;

            propInfo = Connector.GetType().GetProperty(propertyName);
            if (propInfo != null)
                Properties.Add(propertyName, propInfo);

            return propInfo;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var propInfo = GetProperty(binder.Name);
            if (propInfo != null)
            {
                result = propInfo.GetValue(Connector);
                return true;
            }

            return base.TryGetMember(binder, out result);
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            var propInfo = GetProperty(binder.Name);

            if (propInfo != null)
            {
                var oldValue = propInfo.GetValue(Connector);

                if (!Equals(oldValue, value))
                {
                    propInfo.SetValue(Connector, value);
                    RaisePropertyChanged(binder.Name, oldValue, value);
                }
                
                return true;
            }

            return base.TrySetMember(binder, value);
        }

        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            if (binder.Type == typeof(INotifyPropertyChanged))
            {
                result = this;
                return true;
            }
            else if (binder.Type.IsAssignableFrom(Connector.GetType()))
            {
                result = Connector;
                return true;
            }
            else
                return base.TryConvert(binder, out result);
        }

        private void RaisePropertyChanged(string propertyName, object oldValue, object newValue)
        {
            PropertyValueChanged?.Invoke(Connector, 
                new PropertyValueChangedEventArgs(propertyName, oldValue, newValue));
        }
    }
}
