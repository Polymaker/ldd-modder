using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace LDDModder.Rebrickable.Data
{
    public class SearchParameters : ApiFunctionParameters
    {
        public SearchType Type { get; set; }

        [ApiParameter("query")]
        public string Query { get; set; }

        [ApiParameter("type")]
        protected string TypeStr
        {
            get { return ((Char)Type).ToString(); }
        }

        public SearchParameters(SearchType type, string query)
        {
            Type = type;
            Query = query;
        }
    }
}
