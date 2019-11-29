using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.PaletteMaker.Generation
{
    public enum PartMatchingFlags
    {
        NotMatched,
        Matched = 1,
        NonLegoPart = 2,
        InvalidRbColor = 4,
        InvalidLddColor = 8,
        InvalidRbPart = 16,
        LddPartNotFound = 32,
        DecorationNotFound = 64
    }
}
