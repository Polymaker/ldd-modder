using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LDDModder.Rebrickable
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    class ApiParameterAttribute : Attribute
    {
        public string ParamName { get; set; }
        public bool Optional { get; set; }

        public ApiParameterAttribute(string paramName)
        {
            ParamName = paramName;
            Optional = false;
        }

        public ApiParameterAttribute(string paramName, bool optional)
        {
            ParamName = paramName;
            Optional = optional;
        }
    }
}
