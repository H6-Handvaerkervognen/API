using HåndværkervognenAPI.Managers;
using HåndværkervognenAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace HåndværkervognenAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class LoginController:ControllerBase
    {
        private ILoginService loginService;

        public LoginController(ILoginService loginService)
        {
            this.loginService = loginService;
        }

        [HttpPost(Name = "Login")]
        public IActionResult Login(LoginCredentials loginCredentials)
        {
            loginService.AuthorizeLogin(loginCredentials);
            return Ok(true);
        }
        [HttpPost(Name = "CreateNewUser")]
        public IActionResult CreateNewUser(LoginCredentials loginCredentials)
        {
        loginService.RegisterUser(loginCredentials);
            return Created("api/Login/CreateNewUser", loginCredentials);
        }
        [HttpPost(Name = "DeleteUser")]
        public IActionResult DeleteUser(string username)
        {
            loginService.DeleteUser(username);
            return Ok();
        }
    }
}
