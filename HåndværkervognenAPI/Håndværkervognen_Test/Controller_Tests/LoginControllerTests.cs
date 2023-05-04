using HåndværkervognenAPI.Controllers;
using HåndværkervognenAPI.Managers;
using HåndværkervognenAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Håndværkervognen_Test.Controller_Tests
{
    public class LoginControllerTests
    {
        private readonly Mock<ILoginService> _mockLoginService;
        private readonly LoginController _controller;
        private readonly LoginCredentials _validLoginCredentials;

        public LoginControllerTests()
        {
            _mockLoginService = new Mock<ILoginService>();
            _controller = new LoginController(_mockLoginService.Object);
            _validLoginCredentials = new LoginCredentials("TestUsername", "TestPassword");
        }

        [Fact]
        public void Login_ReturnsOkResult_WhenLoginIsSuccessful()
        {
            // Arrange
            string validToken = "valid_token";
            _mockLoginService.Setup(service => service.AuthorizeLogin(_validLoginCredentials)).Returns(validToken);

            // Act
            var result = _controller.Login(_validLoginCredentials);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedToken = Assert.IsType<string>(okResult.Value);
            Assert.Equal(validToken, returnedToken);
        }

        [Fact]
        public void Login_ReturnsBadRequestResult_WhenLoginFails()
        {
            // Arrange
            _mockLoginService.Setup(service => service.AuthorizeLogin(_validLoginCredentials)).Returns("Error");

            // Act
            var result = _controller.Login(_validLoginCredentials);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void CreateNewUser_ReturnsCreatedResult_WhenUserIsSuccessfullyCreated()
        {
            // Arrange
            _mockLoginService.Setup(service => service.RegisterUser(_validLoginCredentials)).Returns(true);

            // Act
            var result = _controller.CreateNewUser(_validLoginCredentials);

            // Assert
            var createdResult = Assert.IsType<CreatedResult>(result);
            var returnedLoginCredentials = Assert.IsType<LoginCredentials>(createdResult.Value);
            Assert.Equal(_validLoginCredentials, returnedLoginCredentials);
        }

        [Fact]
        public void CreateNewUser_ReturnsBadRequestResult_WhenUserAlreadyExists()
        {
            // Arrange
            _mockLoginService.Setup(service => service.RegisterUser(_validLoginCredentials)).Returns(false);

            // Act
            var result = _controller.CreateNewUser(_validLoginCredentials);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
