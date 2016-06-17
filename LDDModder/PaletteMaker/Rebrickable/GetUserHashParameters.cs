using System;
using System.Collections.Generic;
using System.Linq;

namespace LDDModder.PaletteMaker.Rebrickable
{
    public class GetUserHashParameters : ApiFunctionParameters
    {
        [ApiParameter("email")]
        public string Email { get; set; }
        [ApiParameter("pass")]
        public string Password { get; set; }
    }
}
