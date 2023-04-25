using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace HåndværkervognenAPI.Security
{
    public class RSAEncrypter : IEncryption
    {
        private RSAParameters _publicKey;
        private RSAParameters _privateKey;
        private int keySize = 1024;

        public void AssignNewKeys()
        {
            using (var rsa = new RSACryptoServiceProvider(keySize))
            {
                rsa.PersistKeyInCsp = false;
                _publicKey = rsa.ExportParameters(false);
                _privateKey = rsa.ExportParameters(true);
            }
        }

        /// <summary>
        /// decypts a string
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string DecryptData(string data)
        {
            byte[] cipherbytes;

            using (var rsa = new RSACryptoServiceProvider(keySize))
            {
                rsa.PersistKeyInCsp = false;
                rsa.ImportParameters(_publicKey);

                cipherbytes = rsa.Encrypt(Encoding.ASCII.GetBytes(data), true);
            }

            return cipherbytes.ToString();
        }

        /// <summary>
        /// encypts a string
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string EncryptData(string data)
        {
            byte[] plain;

            using (var rsa = new RSACryptoServiceProvider(keySize))
            {
                rsa.PersistKeyInCsp = false;

                rsa.ImportParameters(_privateKey);
                plain = rsa.Decrypt(Encoding.ASCII.GetBytes(data), true);
            }
            return plain.ToString();
        }
    }
}
