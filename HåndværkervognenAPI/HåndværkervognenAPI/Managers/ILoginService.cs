using HåndværkervognenAPI.Models;

namespace HåndværkervognenAPI.Managers
{
    public interface ILoginService
    {
        bool AuthorizeLogin(LoginCredentials loginCredentials);
        bool RegisterUser(LoginCredentials loginCredentials);
        void DeleteUser(string username);
    }
}
