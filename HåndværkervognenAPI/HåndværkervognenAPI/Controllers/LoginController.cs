using HåndværkervognenAPI.Managers;
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
        public bool Login(string username, string pass)
        {
            loginService.AuthorizeLogin(username, pass);
            return true;
        }
        [HttpPost(Name = "CreateNewUser")]
        public void CreateNewUser(string username, string pass)
        {
        loginService.RegisterUser(username, pass);
        }
        [HttpPost(Name = "DeleteUser")]
        public void DeleteUser(string appId)
        {
            loginService.DeleteUser(appId);
        }
    }
}
