using System.Diagnostics;
using System.Security.Cryptography;

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

        public byte[] DecryptData(byte[] data)
        {
            byte[] cipherbytes;

            using (var rsa = new RSACryptoServiceProvider(keySize))
            {
                rsa.PersistKeyInCsp = false;
                rsa.ImportParameters(_publicKey);

                cipherbytes = rsa.Encrypt(data, true);
            }
            return cipherbytes;
        }

        public byte[] EncryptData(byte[] data)
        {
            byte[] plain;

            using (var rsa = new RSACryptoServiceProvider(keySize))
            {
                rsa.PersistKeyInCsp = false;

                rsa.ImportParameters(_privateKey);
                plain = rsa.Decrypt(data, true);
            }
            return plain;
        }
    }
}
