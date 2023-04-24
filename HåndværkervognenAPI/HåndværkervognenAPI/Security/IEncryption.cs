namespace HåndværkervognenAPI.Security
{
    public interface IEncryption
    {
        public void AssignNewKeys();
        public string EncryptData(string data);
        public string DecryptData(string data);
    }
}
