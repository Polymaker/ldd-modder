using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.PaletteMaker.Models
{
    public enum MatchLevel
    {
        Exact,
        Alternate = 1,
        PossibleAlternate = 2,
        Equivalent = 3,
        ManualOverride = 4
    }
}
