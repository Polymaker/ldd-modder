using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.Utilities
{
    public class FormattedMessage
    {
        public string MessageFormat { get; set; }

        public object[] FormatValues { get; set; }

        public FormattedMessage(string messageFormat)
        {
            MessageFormat = messageFormat;
        }

        public FormattedMessage(string messageFormat, object[] formatValues)
        {
            MessageFormat = messageFormat;
            FormatValues = formatValues;
        }

        public override string ToString()
        {
            if (FormatValues != null && FormatValues.Length > 0)
                return string.Format(MessageFormat, FormatValues);
            return MessageFormat;
        }
    }
}
