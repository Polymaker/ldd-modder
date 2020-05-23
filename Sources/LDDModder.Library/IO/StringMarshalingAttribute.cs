using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.IO
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class StringMarshalingAttribute : Attribute
    {
        public StringMarshalingMode MarshalingMode { get; set; }

        public StringMarshalingAttribute(StringMarshalingMode marshalingMode)
        {
            MarshalingMode = marshalingMode;
        }
    }

    public enum StringMarshalingMode
    {
        FixedLength,
        NullTerminated
    }
}
