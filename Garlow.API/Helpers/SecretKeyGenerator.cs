using System;
using System.Linq;

namespace Garlow.API.Helpers
{
    public class SecretKeyGenerator
    {
        private static Random random = new Random();
        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(
                Enumerable
                    .Repeat(chars, length)
                    .Select(s => s[random.Next(s.Length)])
                    .ToArray()
            );
        }

        public static string New()
        {
            return RandomString(32);
        }
    }
}