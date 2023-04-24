using HåndværkervognenAPI.Models;

namespace HåndværkervognenAPI.Managers
{
    public interface ILoginService
    {
        bool AuthorizeLogin(LoginCredentials loginCredentials);
        void RegisterUser(LoginCredentials loginCredentials);
        void DeleteUser(string username);
    }
}
