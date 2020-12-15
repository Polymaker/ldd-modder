using LDDModder.PaletteMaker.Rebrickable.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.PaletteMaker.Rebrickable.Services
{
    public class DataService : RebrickableService
    {
        public DataService(string apiKey) : base(apiKey)
        {
        }

        public IEnumerable<Color> GetAllColors()
        {
            var request = new RestRequest("lego/colors/", Method.GET, DataFormat.Json);
            return GetRequestAllPages<Color>(request);
        }

        public IEnumerable<Category> GetAllCategories()
        {
            var request = new RestRequest("lego/part_categories/", Method.GET, DataFormat.Json);
            return GetRequestAllPages<Category>(request);
        }

        public IEnumerable<Theme> GetAllThemes()
        {
            var request = new RestRequest("lego/themes/", Method.GET, DataFormat.Json);
            return GetRequestAllPages<Theme>(request);
        }
    }
}
