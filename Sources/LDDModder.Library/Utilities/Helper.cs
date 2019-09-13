using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.Utilities
{
    public static class Helper
    {
        public static bool AnyNotNull(params object[] items)
        {
            for (int i = 0; i < items.Length; i++)
                if (items[i] != null)
                    return true;
            return false;
        }
    }
}
