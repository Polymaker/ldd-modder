using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LDDModder.PaletteMaker.Rebrickable.Models;

namespace LDDModder.PaletteMaker.Rebrickable.Services
{
    public class PartService : RebrickableService
    {
        public PartService(string apiKey) : base(apiKey)
        {
        }

        public IEnumerable<Part> GetParts(
            string partNum = null, 
            IEnumerable<string> partNums = null,
            int? partCatId = null,
            int? colorId = null,
            string bricklinkId = null,
            string brickowlId = null,
            string legoId = null,
            string ldrawId = null,
            string ordering = null,
            bool includePartDetails = false,
            int? page = null
            )
        {
            var request = new RestRequest("lego/parts/", Method.GET, DataFormat.Json);

            
            if (partNum != null)
                request.AddParameter("part_num", partNum);
            if (partNums != null)
                request.AddParameter("part_nums", string.Join(",", partNums));
            if (partCatId.HasValue)
                request.AddParameter("part_cat_id", partCatId.Value);
            if (colorId.HasValue)
                request.AddParameter("color_id", colorId.Value);
            if (bricklinkId != null)
                request.AddParameter("bricklink_id", bricklinkId);
            if (brickowlId != null)
                request.AddParameter("brickowl_id", brickowlId);
            if (legoId != null)
                request.AddParameter("lego_id", legoId);
            if (ldrawId != null)
                request.AddParameter("ldraw_id", ldrawId);
            if (includePartDetails)
                request.AddParameter("inc_part_details", "1");

            if (PageSize != DefaultPageSize)
                request.AddParameter("page_size", PageSize);

            if (page.HasValue)
            {
                request.AddParameter("page", page.Value);
                var requestResult = Client.Execute<PagedResult<Part>>(request);
                if (requestResult.IsSuccessful)
                    return requestResult.Data.Results;

                throw new Exception(requestResult.ErrorMessage);
            }

            return GetRequestAllPages<Part>(request);
        }
    }
}
