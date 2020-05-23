using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Linq;

namespace LDDModder.BrickEditor.Localization
{
    [ToolboxItem(false), DesignTimeVisible(false)/*, Serializable*/]
    [TypeConverter(typeof(LocalizableStringConverter))]
    public class LocalizableString : IComponent, IDisposable
    {
        [NonSerialized]
        private ISite _Site;

        [Localizable(true), Category("Design")]
        public string Text { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ISite Site { get => _Site; set => _Site = value; }

        public LocalizableString()
        {
            
        }

        public LocalizableString(string text)
        {
            Text = text;
        }

        //public LocalizableString(IContainer container)
        //{

        //}

        public static implicit operator string(LocalizableString message)
        {
            return message.Text;
        }

        public event EventHandler Disposed;

        public void Dispose()
        {
            if (_Site != null && _Site.Container != null)
                _Site.Container.Remove(this);
            Disposed?.Invoke(this, EventArgs.Empty);
        }
    }

    internal class LocalizableStringConverter : ExpandableObjectConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(InstanceDescriptor))
            {
                return true;
            }
            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(InstanceDescriptor))
            {
                var defaultCtor = typeof(LocalizableString).GetConstructor(new Type[0]);
                if (defaultCtor == null)
                {
                    defaultCtor = typeof(LocalizableString).GetConstructors()
                        .OrderBy(x => x.GetParameters().Length).FirstOrDefault();
                }
                return new InstanceDescriptor(defaultCtor, new object[0], false);
            }
            

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
