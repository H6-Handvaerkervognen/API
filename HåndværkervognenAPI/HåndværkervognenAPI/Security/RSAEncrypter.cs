using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace HåndværkervognenAPI.Security
{
    public class RSAEncrypter : IEncryption
    {
        const int keySize = 2048;

        public void AssignNewKeys(string containerName)
        {
            CspParameters cspParams = new CspParameters(1);
            cspParams.KeyContainerName = "kontainer";
            cspParams.Flags = CspProviderFlags.UseMachineKeyStore;
            cspParams.ProviderName = "Microsoft Strong Cryptographic Provider";

            var rsa = new RSACryptoServiceProvider(keySize, cspParams) { PersistKeyInCsp = true };
        }

        /// <summary>
        /// decypts a string
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string DecryptData(byte[] data, string containerName)
        {
            byte[] cipherbytes;

            CspParameters cspParameters = new CspParameters
            {
                KeyContainerName = "kontainer"
            };

            using (var rsa = new RSACryptoServiceProvider(keySize, cspParameters))
            {
                
                cipherbytes = rsa.Decrypt(data, false);
            }

            return Encoding.ASCII.GetString(cipherbytes);
        }

        /// <summary>
        /// encypts a string
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public byte[] EncryptData(string data, string containerName)
        {
            byte[] plain;

            CspParameters cspParameters = new CspParameters
            {
                KeyContainerName = "kontainer"
            };

            using (var rsa = new RSACryptoServiceProvider(keySize, cspParameters))
            {
                
                plain = rsa.Encrypt(Encoding.ASCII.GetBytes(data), false);
            }
            return plain;
        }
    }
}
