using System.Security.Cryptography;
using System.Text;

namespace TechnoSphere_2025.managers
{
    internal class PasswordManager
    {
        public static byte[] GenerateSalt()
        {
            var salt = new byte[16];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(salt);
            return salt;
        }

        public static byte[] HashPassword(string password, byte[] salt)
        {
            using var sha256 = SHA256.Create();
            var combined = Encoding.UTF8.GetBytes(password).Concat(salt).ToArray();
            return sha256.ComputeHash(combined);
        }
    }
}
