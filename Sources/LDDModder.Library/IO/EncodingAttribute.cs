using System;
using System.Runtime.InteropServices;

namespace LDDModder.IO
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class EncodingAttribute : Attribute
    {
        public CharSet CharSet { get; set; }

        public EncodingAttribute(CharSet charSet)
        {
            CharSet = charSet;
        }
    }
}
