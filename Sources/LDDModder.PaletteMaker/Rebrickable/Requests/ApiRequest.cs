using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.PaletteMaker.Rebrickable.Requests
{
    public class ApiRequest
    {
        public string Url { get; set; }
        public Method Method { get; set; }


    }
}
