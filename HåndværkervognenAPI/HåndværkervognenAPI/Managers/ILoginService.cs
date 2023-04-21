namespace HåndværkervognenAPI.Managers
{
    public interface ILoginService
    {
        bool AuthorizeLogin(string username, string password);
        void RegisterUser(string username, string password);
        void DeleteUser(string uid);
    }
}
