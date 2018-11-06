using LDDModder.Rebrickable.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace LDDModder.Rebrickable.Modules
{
    public class Sets : ApiModule
    {
        public override string Url => "lego/sets/";

        public ListResult<Set> GetSets(int page = 1, int pageSize = 100, int? themeID = null, int? minYear = null, int? maxYear = null, int? minParts = null, int? maxParts = null, string ordering = null, string search = null)
        {
            var parameters = new List<string>();
            parameters.Add($"page={page}");
            parameters.Add($"page_size={pageSize}");

            if(themeID.HasValue)
                parameters.Add($"theme_id={themeID.Value}");

            if (minYear.HasValue)
                parameters.Add($"min_year={minYear.Value}");

            if (maxYear.HasValue)
                parameters.Add($"max_year={maxYear.Value}");

            if (minParts.HasValue)
                parameters.Add($"min_parts={minParts.Value}");

            if (maxParts.HasValue)
                parameters.Add($"max_parts={maxParts.Value}");

            if(!string.IsNullOrEmpty(search))
                parameters.Add($"search={System.Uri.EscapeUriString(search)}");

            var request = RebrickableAPIv3.CreateRequest($"{Url}?{string.Join("&", parameters)}");
            try
            {
                var response = request.GetResponse();
                string jsonContent = null;

                using (var responseStream = response.GetResponseStream())
                {
                    var reader = new StreamReader(responseStream, System.Text.Encoding.UTF8);
                    jsonContent = reader.ReadToEnd();
                }

                return JsonConvert.DeserializeObject<ListResult<Set>>(jsonContent);
            }
            catch (WebException ex)
            {

            }
            return null;
        }

        public Set GetSet(string setNum)
        {
            var request = RebrickableAPIv3.CreateRequest($"{Url}{setNum}/");
            try
            {
                var response = request.GetResponse();
                string jsonContent = null;

                using (var responseStream = response.GetResponseStream())
                {
                    var reader = new StreamReader(responseStream, System.Text.Encoding.UTF8);
                    jsonContent = reader.ReadToEnd();
                }

                return JsonConvert.DeserializeObject<Set>(jsonContent);
            }
            catch(WebException ex)
            {
                
            }
            return null;
        }

        public ListResult<SetPart> GetSetParts(string setNum, int page = 1, int pageSize = 100)
        {
            var request = RebrickableAPIv3.CreateRequest($"{Url}{setNum}/parts/?page={page}&page_size={pageSize}");
            try
            {
                var response = request.GetResponse();
                string jsonContent = null;

                using (var responseStream = response.GetResponseStream())
                using (var reader = new StreamReader(responseStream, System.Text.Encoding.UTF8))
                    jsonContent = reader.ReadToEnd();

                return JsonConvert.DeserializeObject<ListResult<SetPart>>(jsonContent);
            }
            catch
            {

            }
            return null;
        }

        public ListResult<SetPart> GetAllSetParts(string setNum, int pageSize = 200)
        {
            return RebrickableAPIv3.GetAllResults<SetPart>($"{Url}{setNum}/parts/", pageSize);
        }
    }
}
