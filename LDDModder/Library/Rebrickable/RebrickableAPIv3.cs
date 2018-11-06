using LDDModder.Rebrickable.Models;
using LDDModder.Rebrickable.Modules;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace LDDModder.Rebrickable
{
    public static class RebrickableAPIv3
    {
        internal const string API_KEY = "aU49o5xulf";
        internal const string API_URL = "https://rebrickable.com/api/v3/";
        internal static WebClient WC;

        public static Sets Sets { get; } = new Sets();
        public static PartCategories PartCategories { get; } = new PartCategories();
        public static Themes Themes { get; } = new Themes();

        static RebrickableAPIv3()
        {
            WC = new WebClient() { Proxy = null };
        }

        public static HttpWebRequest CreateRequest(string url)
        {
            if (!url.StartsWith(API_URL))
                url = API_URL + url.TrimStart('/');

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Headers.Add(HttpRequestHeader.Authorization, $"key {API_KEY}");
            return request;
        }

        public static ListResult<T> GetAllResults<T>(string url, int pageSize = 100)
        {
            if (url.Contains("?"))
            {
                url += $"&page_size={pageSize}";
            }
            else
                url = url.TrimEnd('/') + $"/?page_size={pageSize}";

            var request = CreateRequest(url);
            try
            {
                string jsonContent = GetRequestData(request);
                var result = JsonConvert.DeserializeObject<ListResult<T>>(jsonContent);
                var finalResult = result;

                while (!string.IsNullOrEmpty(result.Next))
                {
                    Thread.Sleep(500);
                    request = CreateRequest(result.Next);
                    jsonContent = GetRequestData(request);
                    result = JsonConvert.DeserializeObject<ListResult<T>>(jsonContent);
                    if(result != null)
                        finalResult.Results.AddRange(result.Results);
                }

                finalResult.Next = null;
                return finalResult;
            }
            catch(WebException ex)
            {

            }
            return null;
        }

        public static string GetRequestData(HttpWebRequest request)
        {
            try
            {
                var response = request.GetResponse();

                using (var responseStream = response.GetResponseStream())
                using (var reader = new StreamReader(responseStream, System.Text.Encoding.UTF8))
                    return reader.ReadToEnd();
            }
            catch
            {

            }

            return null;
        }
    }
}
