using Isopoh.Cryptography.Argon2;

namespace ESA.Core.Utils
{
    public class Encrypt
    {
        public static string GetHash(string plainText)
        {
            return Argon2.Hash(plainText);
        }

        public static bool Verify(string passwordHash, string plainText)
        {
            return Argon2.Verify(passwordHash, plainText);
        }
    }
}
