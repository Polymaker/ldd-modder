using LDDModder.Rebrickable.Data;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;

namespace LDDModder.Rebrickable
{
    public static class RebrickableAPI
    {
        internal const string API_KEY = "aU49o5xulf";
        internal const string API_URL = "https://rebrickable.com/api/";
        internal static WebClient WC;
        internal static ApiError LastError;

        /// <summary>
        /// Use this service to get a user's hash key. 
        /// This is a 32-character key which can be used instead of an email/password combination when calling any of the below API functions. 
        /// It is designed to prevent having to store passwords in external applications. 
        /// The user's hash key is valid until the user changes their email address or password.
        /// </summary>
        public static readonly ApiFunction<GetUserHashParameters, string> GetUserHash;

        public static readonly ApiFunction<GetUserSetsParameters, GetUserSetsResult> GetUserSets;

        /// <summary>
        /// Use this service to get a list of all parts in a set. It includes both normal and spare parts.
        /// </summary>
        public static readonly ApiFunction<GetSetPartsParameters, GetSetPartsResult> GetSetParts;
        /// <summary>
        /// Use this service to get details about a part, such as its name, number of sets it appears in, which colors it appears in, etc. 
        /// Relationships provide a list of printed parts, different molds etc. 
        /// External IDs provide a list of the part ids for other systems where they are different to Rebrickable, such as Bricklink, Peeron, LDraw or LEGO element IDs. 
        /// If the field rebrickable_part_id exists, that means this part is not used in any Rebrickable inventories and you should use the specified part instead.
        /// </summary>
        public static readonly ApiFunction<GetPartParameters, GetPartResult> GetPart;
        /// <summary>
        /// Use this service to get details about a set.
        /// </summary>
        public static readonly ApiFunction<GetSetParameters, GetSetResult> GetSet;
        /// <summary>
        /// Use this service to get a list of the part type categories.
        /// </summary>
        public static readonly ApiFunction<PartTypeList> GetPartTypes;
        /// <summary>
        /// Get a list of Sets/MOCs/Parts that match the search criteria. Max entries returned is 1000.
        /// </summary>
        public static readonly ApiFunction<SearchParameters, SearchResult> Search;

        static RebrickableAPI()
        {
            WC = new WebClient() { Proxy = null };

            GetUserHash = new ApiFunction<GetUserHashParameters, string>("get_user_hash", RequestMethod.POST);
            GetUserSets = new ApiFunction<GetUserSetsParameters, GetUserSetsResult>("get_user_sets");

            GetSetParts = new ApiFunction<GetSetPartsParameters, GetSetPartsResult>("get_set_parts");
            GetPart = new ApiFunction<GetPartParameters, GetPartResult>("get_part");
            GetSet = new ApiFunction<GetSetParameters, GetSetResult>("get_set");
            GetPartTypes = new ApiFunction<PartTypeList>("get_part_types");
            Search = new ApiFunction<SearchParameters, SearchResult>("search");
            

            LastError = null;
        }

        public static ApiError GetLastError()
        {
            return LastError;
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
