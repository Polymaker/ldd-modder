using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.PaletteMaker.Rebrickable
{
    public static class RebrickableAPI
    {
        internal const string API_KEY = "aU49o5xulf";
        internal const string API_URL = "https://rebrickable.com/api/";
        internal static WebClient WC;

        public static readonly ApiFunction<GetPartParameters, PartInfo> GetPart;
        public static readonly ApiFunction<GetSetPartsParameters, SetParts> GetSetParts;
        public static readonly ApiFunction<GetSetParameters, SetInfo> GetSet;
        public static readonly ApiFunction<PartTypeList> GetPartTypes;
        public static readonly ApiFunction<SearchParameters, SearchResult> Search;
        public static readonly ApiFunction<GetUserHashParameters, string> GetUserHash;

        static RebrickableAPI()
        {
            WC = new WebClient() { Proxy = null };
            GetPart = new ApiFunction<GetPartParameters, PartInfo>("get_part");
            GetSetParts = new ApiFunction<GetSetPartsParameters, SetParts>("get_set_parts");
            GetSet = new ApiFunction<GetSetParameters, SetInfo>("get_set", true);
            GetPartTypes = new ApiFunction<PartTypeList>("get_part_types", true);
            Search = new ApiFunction<SearchParameters, SearchResult>("search", true);
            GetUserHash = new ApiFunction<GetUserHashParameters, string>("get_user_hash", WebMethod.POST);
        }

        internal static byte[] DownloadWebPage(string url)
        {
            return WC.DownloadData(url);
        }

        internal static byte[] DownloadWebPage(string url, NameValueCollection postData)
        {
            return WC.UploadValues(url, "POST", postData);
        }
    }
}
