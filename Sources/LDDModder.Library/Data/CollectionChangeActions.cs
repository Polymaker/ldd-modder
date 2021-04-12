using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.ComponentModel
{
    [Flags]
    public enum CollectionChangeActions
    {
        None = 0,
        Add = 1,
        Remove = 2,
        Move = 3
    }
}
