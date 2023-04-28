using HåndværkervognenAPI.Models;

namespace HåndværkervognenAPI.Managers
{
    public interface ILoginService
    {
        string AuthorizeLogin(LoginCredentials loginCredentials);
        bool RegisterUser(LoginCredentials loginCredentials);
  
    }
}
