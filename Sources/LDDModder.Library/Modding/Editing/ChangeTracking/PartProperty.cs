using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.Modding.Editing
{
    public class PartProperty
    {
        private PropertyCollection Owner { get; }

        public string Name { get; }
        public Type ValueTye { get; }

        public PartProperty(PropertyCollection owner, string name, Type valueTye)
        {
            Owner = owner;
            Name = name;
            ValueTye = valueTye;
        }

        public PartProperty(string name, Type valueTye)
        {
            Name = name;
            ValueTye = valueTye;
        }

        public virtual object GetValue()
        {
            return null;
        }

        public virtual void SetValue(object value)
        {

        }
    }

    public class PartProperty<T> : PartProperty
    {
        private T _Value;

        public T Value
        {
            get => _Value;
            set => SetValue(value);
        }

        public PartProperty(PropertyCollection owner, string name) : base(owner, name, typeof(T))
        {

        }

        public PartProperty(string name) : base(name, typeof(T))
        {

        }


    }
}
