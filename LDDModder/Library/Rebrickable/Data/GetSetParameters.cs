using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LDDModder.Rebrickable.Data
{
    public class GetSetParameters : ApiFunctionParameters
    {
        /// <summary>
        /// The Set ID to look up.
        /// </summary>
        [ApiParameter("set_id")]
        public string SetId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setId">The Set ID to look up.</param>
        public GetSetParameters(string setId)
        {
            SetId = setId;
        }

        public static implicit operator GetSetParameters(string setId)
        {
            return new GetSetParameters(setId);
        }
    }
}
