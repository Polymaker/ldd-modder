using LDDModder.PaletteMaker.Rebrickable.Models;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LDDModder.PaletteMaker.Rebrickable
{
    public static class RebrickableAPI
    {
        public static string ApiKey { get; set; }

        public static RestClient Client { get; private set; }

        public static void InitializeClient()
        {
            Client = new RestClient("https://rebrickable.com/api/v3");
            //Client.UseSerializer(() => new JsonNetSerializer());
            Client.AddDefaultHeader("Authorization", $"key {ApiKey}");
        }

        public static IEnumerable<Color> GetAllColors()
        {
            var request = new RestRequest("lego/colors/", Method.GET, DataFormat.Json);
            return GetRequestAllPages<Color>(request);
        }

        public static IEnumerable<Category> GetAllCategories()
        {
            var request = new RestRequest("lego/part_categories/", Method.GET, DataFormat.Json);
            return GetRequestAllPages<Category>(request);
        }

        public static IEnumerable<Theme> GetAllThemes()
        {
            var request = new RestRequest("lego/themes/", Method.GET, DataFormat.Json);
            return GetRequestAllPages<Theme>(request);
        }

        public static IEnumerable<Set> GetAllSets()
        {
            var request = new RestRequest("lego/sets/", Method.GET, DataFormat.Json);
            return GetRequestAllPages<Set>(request);
        }

        public static IEnumerable<Set> GetSets(
            int? themeID = null,
            int? minYear = null,
            int? maxYear = null,
            string search = null)
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

            return GetRequestAllPages<Set>(request);
        }

        public static IEnumerable<SetPart> GetSetParts(string setID, bool includeMinifigs = false)
        {
            var request = new RestRequest("lego/sets/{set_num}/parts/", Method.GET, DataFormat.Json);
            if (includeMinifigs)
                request.AddParameter("inc_minifig_parts", "1");
            request.AddUrlSegment("set_num", setID);
            return GetRequestAllPages<SetPart>(request);
        }

        public static IEnumerable<Part> GetPartsDetails(IEnumerable<string> partIDs)
        {
            var request = new RestRequest("lego/parts/", Method.GET, DataFormat.Json);
            request.AddParameter("inc_part_details", "1");
            request.AddParameter("part_nums", string.Join(",", partIDs));
            return GetRequestAllPages<Part>(request);
        }

        public static IEnumerable<T> GetRequestAllPages<T>(RestRequest request)
        {
            IEnumerable<T> results = Enumerable.Empty<T>();

            var requestResult = Client.Execute<PagedResult<T>>(request);

            while (requestResult.IsSuccessful)
            {
                results = results.Concat(requestResult.Data.Results);

                if (string.IsNullOrEmpty(requestResult.Data.Next))
                    break;

                var newRequest = new RestRequest(requestResult.Data.Next, Method.GET, DataFormat.Json);
                Thread.Sleep(100);
                requestResult = Client.Execute<PagedResult<T>>(newRequest);
            }

            return results;
        }

        public static Set GetSet(string setID)
        {
            var request = new RestRequest("lego/sets/{set_num}/", Method.GET, DataFormat.Json);
            request.AddUrlSegment("set_num", setID);
            var requestResult = Client.Execute<Set>(request);
            return requestResult.IsSuccessful ? requestResult.Data : null;
        }

        class JsonNetSerializer : IRestSerializer
        {
            public string Serialize(object obj) =>
            JsonConvert.SerializeObject(obj);

#pragma warning disable CS0618 // Type or member is obsolete
            public string Serialize(Parameter parameter) =>
#pragma warning restore CS0618 // Type or member is obsolete
                JsonConvert.SerializeObject(parameter.Value);

            public T Deserialize<T>(IRestResponse response) =>
                JsonConvert.DeserializeObject<T>(response.Content);

            public string[] SupportedContentTypes { get; } =
            {
                "application/json", "text/json", "text/x-json", "text/javascript", "*+json"
            };

            public string ContentType { get; set; } = "application/json";

            public DataFormat DataFormat { get; } = DataFormat.Json;
        }
    }
}
