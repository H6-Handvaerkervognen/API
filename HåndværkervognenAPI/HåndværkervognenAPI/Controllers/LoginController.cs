using HåndværkervognenAPI.Managers;
using HåndværkervognenAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace HåndværkervognenAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController
    {
        private ILoginService loginService;

        public LoginController(ILoginService loginService)
        {
            this.loginService = loginService;
        }

        [HttpPost(Name = "Login")]
        public bool Login(LoginCredentials loginCredentials)
        {
            loginService.AuthorizeLogin(username, pass);
            return true;
        }
        [HttpPost(Name = "CreateNewUser")]
        public void CreateNewUser(LoginCredentials loginCredentials)
        {
        loginService.RegisterUser(username, pass);
        }
        [HttpPost(Name = "DeleteUser")]
        public void DeleteUser(string username)
        {
            loginService.DeleteUser(username);
        }
    }
}
