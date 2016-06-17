using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LDDModder.Rebrickable.Data
{
    public class GetUserHashParameters : ApiFunctionParameters
    {
        /// <summary>
        /// The user's logon email.
        /// </summary>
        [ApiParameter("email")]
        public string Email { get; set; }
        /// <summary>
        /// The user's password.
        /// </summary>
        [ApiParameter("pass")]
        public string Password { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetUserHashParameters"/> class.
        /// </summary>
        /// <param name="email">The user's logon email.</param>
        /// <param name="password">The user's password.</param>
        public GetUserHashParameters(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
