using LDDModder.PaletteMaker.Rebrickable.Models;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.PaletteMaker.Rebrickable
{
    public class RebrickableAPI
    {
        public string ApiKey { get; set; }

        public RebrickableAPI(string apiKey)
        {
            ApiKey = apiKey;
        }

        public RestClient GetClient()
        {
            var client = new RestClient("https://rebrickable.com/api/v3");
            client.UseSerializer(() => new JsonNetSerializer());
            client.AddDefaultHeader("Authorization", ApiKey);
            return client;
        }

        public void GetSetsRequest()
        {

        }

        public RestRequest GetPagedRequest(string url, int page, int pageSize = 100)
        {
            var request = new RestRequest(url, Method.GET, DataFormat.Json);
            request.AddParameter("page", page);
            request.AddParameter("page_size", pageSize);
            return request;
        }

        public IEnumerable<T> ExecutePagedRequest<T>(RestRequest request)
        {
            var client = GetClient();
            //IEnumerable<T> results = Enumerable.Empty<T>();
            var requestResult = client.Execute<PagedResult<T>>(request);
            //if (requestResult.IsSuccessful)
            //    results = requestResult.Data.Results;
            //if (requestResult.IsSuccessful && !string.IsNullOrEmpty(requestResult.Data.Next))
            //{
            //    var newRequest = new RestRequest(requestResult.Data.Next, Method.GET, DataFormat.Json);
            //    requestResult = client.Execute<PagedResult<T>>(newRequest);
            //    if (requestResult.IsSuccessful)
            //        results = results.Concat(requestResult.Data.Results);
            //}
            return requestResult.Data.Results;
        }

        class JsonNetSerializer : IRestSerializer
        {
            public string Serialize(object obj) =>
            JsonConvert.SerializeObject(obj);

            public string Serialize(Parameter parameter) =>
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
