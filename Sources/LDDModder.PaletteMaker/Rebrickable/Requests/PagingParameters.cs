using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.PaletteMaker.Rebrickable.Requests
{
    public class PagingParameters : IRequestParameters
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
