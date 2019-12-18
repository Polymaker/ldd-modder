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

        public ValidationLevel Level { get; set; }

        public string SourceKey { get; set; }

        public PartElement SourceElement { get; set; }

        public string Message { get; set; }

        public object[] MessageArguments { get; set; }

        public ValidationMessage()
        {
        }

        public ValidationMessage(string source, string code, ValidationLevel level, params object[] arguments)
        {
            SourceKey = source;
            Code = code;
            Level = level;
            MessageArguments = arguments;
        }

        public ValidationMessage(PartElement sourceElement, string messageCode, ValidationLevel level)
        {
            Code = messageCode;
            Level = level;
            SourceElement = sourceElement;
            if (sourceElement is PartSurface)
                SourceKey = "SURFACE";
            else if (sourceElement is SurfaceComponent)
                SourceKey = "COMPONENT";
            else if (sourceElement is PartProperties)
                SourceKey = "PART";
            else if (sourceElement is PartBone)
                SourceKey = "BONE";
            else
                SourceKey = "PROJECT";
        }
    }

    public enum ValidationLevel : int
    {
        Info = 0,
        Warning = 1,
        Error = 2
    }
}
