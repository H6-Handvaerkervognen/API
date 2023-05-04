namespace HåndværkervognenAPI.Security
{
    public interface IEncryption
    {
        public void AssignNewKeys(string containerName);
        public byte[] EncryptData(string data, string containerName);
        public string DecryptData(byte[] data, string containerName);
    }
}
