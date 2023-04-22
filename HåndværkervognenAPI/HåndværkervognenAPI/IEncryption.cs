namespace HåndværkervognenAPI
{
    public interface IEncryption
    {
        public void AssignNewKeys();
        public byte[] EncryptData(byte[] data);
        public byte[] DecryptData(byte[] data);
    }
}
