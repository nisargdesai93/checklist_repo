using Infrastructure.Application.Implementation;
using Infrastructure.Entity;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Implementation
{
    public class EncryptionService
    {
        public const int SALT_BYTES = 32;

        public EncryptionService()
        {
        }


        public Tuple<string, string> CreatePasswordHash(String password)
        {
            string salt = GenerateSalt();
            byte[] bytes = Encoding.UTF8.GetBytes(password + salt);
            SHA256Managed sHA256String = new SHA256Managed();
            byte[] hash = sHA256String.ComputeHash(bytes);
            string passwordHash = ByteArrayToHexString(hash);
            return new Tuple<string, string>(salt, passwordHash);
        }

        private static string GenerateSalt()
        {
            var rng = new RNGCryptoServiceProvider();
            var buffer = new byte[SALT_BYTES];
            rng.GetBytes(buffer);
            return Convert.ToBase64String(buffer);
        }

        private static string ByteArrayToHexString(byte[] ba)
        {
            StringBuilder sb = new StringBuilder();

            foreach (byte b in ba)
            {
                sb.AppendFormat("{0:x2}", b);
            }
            return sb.ToString();
        }


        public string GetUserHashedPassword(string userSalt, string userPassword)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(userPassword + userSalt);
            SHA256Managed sHA256String = new SHA256Managed();
            byte[] hash = sHA256String.ComputeHash(bytes);
            string passwordHash = ByteArrayToHexString(hash);
            return passwordHash;
        }
    }
}
