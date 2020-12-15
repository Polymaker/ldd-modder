using LDDModder.PaletteMaker.Rebrickable.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LDDModder.PaletteMaker.Rebrickable.Services
{
    public class RebrickableService
    {
        public string ApiKey { get; private set; }

        public RestClient Client { get; private set; }

        public int PageSize { get; set; }

        public const int DefaultPageSize = 100;

        public RebrickableService(string apiKey)
        {
            ApiKey = apiKey;
            Client = new RestClient("https://rebrickable.com/api/v3");
            Client.UseSerializer(() => new JsonNetSerializer());
            Client.AddDefaultHeader("Authorization", $"key {ApiKey}");
            PageSize = DefaultPageSize;
        }

        public IEnumerable<T> GetRequestAllPages<T>(RestRequest request)
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

        public async Task<IEnumerable<T>> GetRequestAllPagesAsync<T>(RestRequest request)
        {
            IEnumerable<T> results = Enumerable.Empty<T>();

            var requestResult = await Client.ExecuteAsync<PagedResult<T>>(request);

            while (requestResult.IsSuccessful)
            {
                results = results.Concat(requestResult.Data.Results);

                if (string.IsNullOrEmpty(requestResult.Data.Next))
                    break;

                var newRequest = new RestRequest(requestResult.Data.Next, Method.GET, DataFormat.Json);
                Thread.Sleep(100);
                requestResult = await Client.ExecuteAsync<PagedResult<T>>(newRequest);
            }

            return results;
        }
    }
}
