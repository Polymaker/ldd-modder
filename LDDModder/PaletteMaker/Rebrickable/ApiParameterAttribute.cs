using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.PaletteMaker.Rebrickable
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    class ApiParameterAttribute : Attribute
    {
        public string ParamName { get; set; }

        public ApiParameterAttribute(string paramName)
        {
            ParamName = paramName;
        }
    }
}
