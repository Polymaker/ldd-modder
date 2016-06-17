using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LDDModder.Rebrickable.Data
{
    public class GetPartParameters : ApiFunctionParameters
    {
        /// <summary>
        /// The Part ID to look up.
        /// </summary>
        [ApiParameter("part_id")]
        public string PartId { get; set; }
        /// <summary>
        /// Optional flag to include Part Relationships in return data (may be a lot of data for some parts).
        /// </summary>
        [ApiParameter("inc_rels", true)]
        public bool IncludeRelationships { get; set; }
        /// <summary>
        /// Optional flag to include external Part IDs (may be a lot of data for some parts due to LEGO element ids)
        /// </summary>
        [ApiParameter("inc_ext", true)]
        public bool IncludeExternalIDs { get; set; }
        /// <summary>
        /// Optional flag to include available part colors which can be slow for common parts (default 1).
        /// </summary>
        [ApiParameter("inc_colors", true)]
        public bool IncludePartColors { get; set; }

        public GetPartParameters(string partId)
        {
            PartId = partId;
            IncludeRelationships = false;
            IncludeExternalIDs = false;
            IncludePartColors = false;
        }

        public GetPartParameters(string partId, bool includeRelationships, bool includeExternalIDs, bool includePartColors)
        {
            PartId = partId;
            IncludeRelationships = includeRelationships;
            IncludeExternalIDs = includeExternalIDs;
            IncludePartColors = includePartColors;
        }

        public static implicit operator GetPartParameters(string partId)
        {
            return new GetPartParameters(partId);
        }
    }
}
