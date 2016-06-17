using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LDDModder.Rebrickable.Data
{
    public class GetUserSetsParameters : ApiFunctionParameters
    {
        /// <summary>
        /// The user's logon email.
        /// </summary>
        [ApiParameter("email", true)]
        public string Email { get; set; }
        /// <summary>
        /// The user's password.
        /// </summary>
        [ApiParameter("pass", true)]
        public string Password { get; set; }
        /// <summary>
        /// User Hash Key, alternate to providing email+pass.
        /// </summary>
        [ApiParameter("hash", true)]
        public string Hash { get; set; }
        /// <summary>
        /// The ID of the user's set list to retrieve (leave out to combine all setlists).
        /// </summary>
        [ApiParameter("setlist_id", true)]
        public string SetListID { get; set; }

        public GetUserSetsParameters(string email, string password)
        {
            Email = email;
            Password = password;
            Hash = String.Empty;
            SetListID = String.Empty;
        }

        public GetUserSetsParameters(string hash)
        {
            Hash = hash;
            Email = String.Empty;
            Password = String.Empty;
            SetListID = String.Empty;
        }

    }
}
