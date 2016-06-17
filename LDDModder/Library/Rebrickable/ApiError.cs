using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LDDModder.Rebrickable
{
    public class ApiError
    {
        private string _Description;
        private string _Code;

        public string Code
        {
            get { return _Code; }
        }

        public string Description
        {
            get { return _Description; }
        }

        internal ApiError(string code, string description)
        {
            _Code = code;
            _Description = description;
        }
        
        public static readonly ApiError InvalidKey = new ApiError("INVALIDKEY", "The API Key is invalid");

        public static readonly ApiError InvalidPass = new ApiError("INVALIDPASS", "Invalid user email or password");

        public static readonly ApiError InvalidUserPass = new ApiError("INVALIDUSERPASS", "Invalid user email or password");

        public static readonly ApiError InvalidHash = new ApiError("INVALIDHASH", "The hash key does not match a user/password");

        public static readonly ApiError InvalidFormat = new ApiError("INVALIDFORMAT", "Only xml or json is supported for this call.");

        public static readonly ApiError UserNotFound = new ApiError("NOUSER", "The user could not be found");

        public static readonly ApiError PartNotFound = new ApiError("NOPART", "The part could not be found");

        public static readonly ApiError SetNotFound = new ApiError("NOSET", "The set could not be found");

        private static readonly ApiError[] ERRORS = new ApiError[] { InvalidKey, InvalidPass, InvalidUserPass, InvalidHash, InvalidFormat, UserNotFound, PartNotFound, SetNotFound };

        internal static ApiError FindError(string errorText)
        {
            for (int i = 0; i < ERRORS.Length; i++)
            {
                if (ERRORS[i].Code == errorText)
                    return ERRORS[i];
            }
            return null;
        }
    }
}
