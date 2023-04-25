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
        
        /// <summary>
        /// post request for logging into the app
        /// </summary>
        /// <param name="loginCredentials"></param>
        /// <returns></returns>
        [HttpPost(Name = "Login")]
        public IActionResult Login(LoginCredentials loginCredentials)
        {
            //loginService.AuthorizeLogin(loginCredentials);
            return Ok(true);
        }

        /// <summary>
        /// post request for registering in the app
        /// </summary>
        /// <param name="loginCredentials"></param>
        /// <returns></returns>
        [HttpPost(Name = "CreateNewUser")]
        public IActionResult CreateNewUser(LoginCredentials loginCredentials)
        {
        loginService.RegisterUser(loginCredentials);
            return Created("api/Login/CreateNewUser", loginCredentials);
        }

        /// <summary>
        /// post request for deleteing a account
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpPost(Name = "DeleteUser")]
        public IActionResult DeleteUser(string username)
        {
            loginService.DeleteUser(username);
            return Ok();
        }
    }
}
