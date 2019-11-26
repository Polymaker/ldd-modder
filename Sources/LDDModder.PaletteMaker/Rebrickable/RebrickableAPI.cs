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
    public static class RebrickableAPI
    {
        public static string ApiKey { get; set; }

        public static RestClient Client { get; private set; }

        public static void InitializeClient()
        {
            Client = new RestClient("https://rebrickable.com/api/v3");
            Client.UseSerializer(() => new JsonNetSerializer());
            Client.AddDefaultHeader("Authorization", $"key {ApiKey}");
        }

        public static IEnumerable<Color> GetAllColors()
        {
            var request = new RestRequest("lego/colors/", Method.GET, DataFormat.Json);
            return GetRequestAllPages<Color>(request);
        }

        //public RestRequest GetPagedRequest(string url, int page, int pageSize = 100)
        //{
        //    var request = new RestRequest(url, Method.GET, DataFormat.Json);
        //    request.AddParameter("page", page);
        //    request.AddParameter("page_size", pageSize);
        //    return request;
        //}

        //public IEnumerable<T> ExecutePagedRequest<T>(RestRequest request)
        //{
        //    //IEnumerable<T> results = Enumerable.Empty<T>();
        //    var requestResult = Client.Execute<PagedResult<T>>(request);
        //    //if (requestResult.IsSuccessful)
        //    //    results = requestResult.Data.Results;
        //    //if (requestResult.IsSuccessful && !string.IsNullOrEmpty(requestResult.Data.Next))
        //    //{
        //    //    var newRequest = new RestRequest(requestResult.Data.Next, Method.GET, DataFormat.Json);
        //    //    requestResult = client.Execute<PagedResult<T>>(newRequest);
        //    //    if (requestResult.IsSuccessful)
        //    //        results = results.Concat(requestResult.Data.Results);
        //    //}
        //    return requestResult.Data.Results;
        //}
        
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
                requestResult = Client.Execute<PagedResult<T>>(newRequest);
            }

            return results;
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
