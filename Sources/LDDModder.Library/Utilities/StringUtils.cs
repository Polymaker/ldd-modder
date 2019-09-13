using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.Utilities
{
    public static class StringUtils
    {
        public static string GenerateUUID(string uniqueID, int length = 16)
        {
            byte[] stringbytes = Encoding.UTF8.GetBytes(uniqueID);
            byte[] hashedBytes = new System.Security.Cryptography
                .SHA1CryptoServiceProvider()
                .ComputeHash(stringbytes);
            Array.Resize(ref hashedBytes, 16);
            var guid = new Guid(hashedBytes);
            if (length < 4)
                length = 4;
            if (length > 16)
                length = 16;
            return guid.ToString("N").Substring(0, length);
        }

        public static string GenerateUID(int length = 16)
        {
            var guid = Guid.NewGuid();
            if (length < 4)
                length = 4;
            if (length > 16)
                length = 16;
            return guid.ToString("N").Substring(0, length);
        }
    }
}
