namespace HåndværkervognenAPI.Security
{
    public interface IEncryption
    {
        public void AssignNewKeys(string containerName);
        public string EncryptData(string data, string containerName);
        public string DecryptData(string data, string containerName);
    }
}
