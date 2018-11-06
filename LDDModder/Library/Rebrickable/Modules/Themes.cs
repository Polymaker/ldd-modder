using LDDModder.Rebrickable.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LDDModder.Rebrickable.Modules
{
    public class Themes : ApiModule
    {
        public override string Url => "lego/themes/";

        public ListResult<Theme> GetThemes(int page = 1, int pageSize = 100)
        {
            var request = RebrickableAPIv3.CreateRequest($"{Url}/?page={page}&page_size={pageSize}");
            try
            {
                var response = request.GetResponse();
                string jsonContent = null;

                using (var responseStream = response.GetResponseStream())
                using (var reader = new StreamReader(responseStream, System.Text.Encoding.UTF8))
                    jsonContent = reader.ReadToEnd();
                

                return JsonConvert.DeserializeObject<ListResult<Theme>>(jsonContent);
            }
            catch
            {

            }
            return null;
        }

        public ListResult<Theme> GetAllThemes(int pageSize = 300)
        {
            return RebrickableAPIv3.GetAllResults<Theme>(Url, pageSize);
        }
    }
}
