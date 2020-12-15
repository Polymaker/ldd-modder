using LDDModder.PaletteMaker.Rebrickable.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.PaletteMaker.Rebrickable.Services
{
    public class SetService : RebrickableService
    {
        public SetService(string apiKey) : base(apiKey)
        {
        }

        public IEnumerable<Set> GetSets(
            int? themeID = null,
            int? minYear = null,
            int? maxYear = null,
            string search = null,
            int? page = null,
            int? minParts = null,
            int? maxParts = null,
            string ordering = null)
        {
            var request = new RestRequest("lego/sets/", Method.GET, DataFormat.Json);
            if (themeID.HasValue)
                request.AddParameter("theme_id", themeID.Value);
            if (minYear.HasValue)
                request.AddParameter("min_year", minYear.Value);
            if (maxYear.HasValue)
                request.AddParameter("max_year", maxYear.Value);
            if (!string.IsNullOrEmpty(search))
                request.AddParameter("search", search);

            if (PageSize != DefaultPageSize)
                request.AddParameter("page_size", PageSize);

            if (page.HasValue)
            {
                request.AddParameter("page", page.Value);
                var requestResult = Client.Execute<PagedResult<Set>>(request);
                if (requestResult.IsSuccessful)
                    return requestResult.Data.Results;

                throw new Exception(requestResult.ErrorMessage);
            }

            return GetRequestAllPages<Set>(request);
        }

        public Set GetSet(string setNum)
        {
            var request = new RestRequest("lego/sets/{set_num}/", Method.GET, DataFormat.Json);
            request.AddUrlSegment("set_num", setNum);
            var requestResult = Client.Execute<Set>(request);
            if (!requestResult.IsSuccessful)
                throw new Exception(requestResult.ErrorMessage);
            return requestResult.Data;
        }

        public IEnumerable<SetPart> GetSetParts(string setID, bool includeMinifigs = false, int? page = null)
        {
            var request = new RestRequest("lego/sets/{set_num}/parts/", Method.GET, DataFormat.Json);

            if (includeMinifigs)
                request.AddParameter("inc_minifig_parts", "1");

            request.AddUrlSegment("set_num", setID);

            if (PageSize != DefaultPageSize)
                request.AddParameter("page_size", PageSize);

            if (page.HasValue)
            {
                request.AddParameter("page", page.Value);
                var requestResult = Client.Execute<PagedResult<SetPart>>(request);
                if (requestResult.IsSuccessful)
                    return requestResult.Data.Results;

                throw new Exception(requestResult.ErrorMessage);
            }

            return GetRequestAllPages<SetPart>(request);
        }
    }
}
