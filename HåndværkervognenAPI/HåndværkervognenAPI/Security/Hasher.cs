using System.Security.Cryptography;

namespace HåndværkervognenAPI.Security
{
    public class Hasher : IHashing
    {
        private const int SaltByteSize = 64;
        private const int HashByteSize = 64;
        private const int HashingIterationsCount = 50743;

        /// <summary>
        /// Computes the hash for the password that was input
        /// </summary>
        public byte[] GenerateHash(string password, byte[] salt)
        {
            PBKDF2 hashGenerator = new PBKDF2(password, salt, HashingIterationsCount, "HMACSHA256");
            byte[] hashedPassword = hashGenerator.GetBytes(HashByteSize);
            return hashedPassword;
        }



        /// <summary>
        /// Generates a salt using the cryptographic servise provider
        /// </summary>
        public byte[] GenerateSalt()
        {
            using (RNGCryptoServiceProvider saltGenerater = new RNGCryptoServiceProvider())
            {
                byte[] salt = new byte[SaltByteSize];
                saltGenerater.GetBytes(salt);
                return salt;
            }
        }

    }
}
