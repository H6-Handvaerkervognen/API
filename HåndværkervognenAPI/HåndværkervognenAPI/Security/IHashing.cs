namespace HåndværkervognenAPI.Security
{
    public interface IHashing
    {
        byte[] GenerateHash(string password, byte[] salt);
        byte[] GenerateSalt();
    }
}
