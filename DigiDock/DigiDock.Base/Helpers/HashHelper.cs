using System;
using System.Security.Cryptography;
using System.Text;

namespace DigiDock.Base.Helpers
{
    public static class HashHelper
    {
        public static string CreateMD5(string input)
        {
            using (var md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                return Convert.ToHexString(hashBytes).ToLower();
            }
        }
    }
}