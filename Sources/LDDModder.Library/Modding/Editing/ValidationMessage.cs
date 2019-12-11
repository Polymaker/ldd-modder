using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.Modding.Editing
{
    public class ValidationMessage
    {
        public string Code { get; set; }

        public string Source { get; set; }

        public ValidationLevel Level { get; set; }

        public object[] Arguments { get; set; }

        public ValidationMessage(string source, string code, ValidationLevel level, params object[] arguments)
        {
            Source = source;
            Code = code;
            Level = level;
            Arguments = arguments;
        }
    }

    public enum ValidationLevel : int
    {
        Info = 0,
        Warning = 1,
        Error = 2
    }
}
