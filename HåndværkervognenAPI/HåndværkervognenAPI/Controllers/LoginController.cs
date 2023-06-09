﻿using HåndværkervognenAPI.Managers;
using HåndværkervognenAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace HåndværkervognenAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class LoginController : ControllerBase
    {
        private ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }


        /// <summary>
        /// post request for logging into the app
        /// </summary>
        /// <param name="loginCredentials"></param>
        /// <returns></returns>
        [HttpPost(Name = "Login")]
        public IActionResult Login(LoginCredentials loginCredentials)
        {
            var token = _loginService.AuthorizeLogin(loginCredentials);
            if (token != "Error")
            {
                //returnToken
                return Ok(token);
            }
            return BadRequest();
        }


        /// <summary>
        /// post request for registering in the app
        /// </summary>
        /// <param name="loginCredentials"></param>
        /// <returns></returns>
        [HttpPost(Name = "CreateNewUser")]
        public IActionResult CreateNewUser(LoginCredentials loginCredentials)
        {
            if (_loginService.RegisterUser(loginCredentials))
            {
                return Created("api/Login/CreateNewUser", loginCredentials);
            }
            return BadRequest("A user with that username already exists. Login or choose a new username");
        }

    }
}
