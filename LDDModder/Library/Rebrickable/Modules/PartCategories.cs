using LDDModder.Rebrickable.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LDDModder.Rebrickable.Modules
{
    public class PartCategories : ApiModule
    {
        public override string Url => "lego/part_categories/";

        public ListResult<PartCategory> GetCategories(int page = 1, int pageSize = 100)
        {
            var request = RebrickableAPIv3.CreateRequest($"{Url}/?page={page}&page_size={pageSize}");
            try
            {
                var response = request.GetResponse();
                string jsonContent = null;

                using (var responseStream = response.GetResponseStream())
                using (var reader = new StreamReader(responseStream, System.Text.Encoding.UTF8))
                    jsonContent = reader.ReadToEnd();

                return JsonConvert.DeserializeObject<ListResult<PartCategory>>(jsonContent);
            }
            catch
            {

            }
            return null;
        }

    }
}
