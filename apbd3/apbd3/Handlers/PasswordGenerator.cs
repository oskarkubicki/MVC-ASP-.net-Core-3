using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace apbd3.Handlers
{
    public static class PasswordGenerator
    {
        public static string Create(string value, string salt)

        {
            var valueBytes = KeyDerivation.Pbkdf2(
                value,
                Encoding.UTF8.GetBytes(salt),
                KeyDerivationPrf.HMACSHA512,
                10000,
                256 / 8);

            return Convert.ToBase64String(valueBytes);
        }

        public static string CreateSalt()

        {
            var randomBytes = new byte[128 / 8];
            using var generator = RandomNumberGenerator.Create();
            generator.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }
}