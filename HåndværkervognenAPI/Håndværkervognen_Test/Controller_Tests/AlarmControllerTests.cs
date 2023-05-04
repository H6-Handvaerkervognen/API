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
    public class AlarmControllerTests
    {
        private readonly Mock<IAlarmService> _mockAlarmService;
        private readonly AlarmController _controller;

        public AlarmControllerTests()
        {
            _mockAlarmService = new Mock<IAlarmService>();
            _controller = new AlarmController(_mockAlarmService.Object);
        }

        [Fact]
        public void GetAlarmInfo_ReturnsOkResult_WhenAlarmInfoIsNotNull()
        {
            // Arrange
            string testAlarmId = "testAlarmId";
            var alarmInfoDto = new AlarmInfoDto("TestStartDate", "TestendDate", testAlarmId, "Test name");
            _mockAlarmService.Setup(service => service.GetAlarmInfo(testAlarmId)).Returns(alarmInfoDto);

            // Act
            var result = _controller.GetAlarmInfo(testAlarmId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedAlarmInfo = Assert.IsType<AlarmInfoDto>(okResult.Value);
            Assert.Equal(testAlarmId, returnedAlarmInfo.AlarmId);
        }

        [Fact]
        public void GetAlarmInfo_ReturnsNoContentResult_WhenAlarmInfoIsNull()
        {
            // Arrange
            string testAlarmId = "testAlarmId";
            _mockAlarmService.Setup(service => service.GetAlarmInfo(testAlarmId)).Returns((AlarmInfoDto)null);

            // Act
            var result = _controller.GetAlarmInfo(testAlarmId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }


        [Fact]
        public void DeletePairing_ReturnsOkResult_WhenPairingIsDeleted()
        {
            // Arrange
            string testAlarmId = "testAlarmId";
            _mockAlarmService.Setup(service => service.DeletePairing(testAlarmId)).Returns(true);

            // Act
            var result = _controller.DeletePairing(testAlarmId);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void DeletePairing_ReturnsBadRequestResult_WhenPairingIsNotDeleted()
        {
            // Arrange
            string testAlarmId = "testAlarmId";
            _mockAlarmService.Setup(service => service.DeletePairing(testAlarmId)).Returns(false);

            // Act
            var result = _controller.DeletePairing(testAlarmId);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void ActivateAlarm_ReturnsOkResult_WhenAlarmIsActivated()
        {
            // Arrange
            string testAlarmId = "testAlarmId";
            _mockAlarmService.Setup(service => service.AlertUser(testAlarmId)).Returns(true);

            // Act
            var result = _controller.ActivateAlarm(testAlarmId);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void ActivateAlarm_ReturnsBadRequestResult_WhenAlarmIsNotActivated()
        {
            // Arrange
            string testAlarmId = "testAlarmId";
            _mockAlarmService.Setup(service => service.AlertUser(testAlarmId)).Returns(false);

            // Act
            var result = _controller.ActivateAlarm(testAlarmId);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }
    }
}
