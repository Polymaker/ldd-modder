using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LDDModder.Rebrickable.Data
{
    public class GetSetPartsParameters : ApiFunctionParameters
    {
        /// <summary>
        /// The Set ID to look up.
        /// </summary>
        [ApiParameter("set")]
        public string SetId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setId">The Set ID to look up</param>
        public GetSetPartsParameters(string setId)
        {
            SetId = setId;
        }

        public static implicit operator GetSetPartsParameters(string setId)
        {
            return new GetSetPartsParameters(setId);
        }
    }
}
