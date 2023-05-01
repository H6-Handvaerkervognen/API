using HåndværkervognenAPI.Controllers;
using HåndværkervognenAPI.Managers;
using HåndværkervognenAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Håndværkervognen_Test.Controller_Tests
{
    public class AppControllerTests
    {
        private readonly Mock<IAppService> _mockAppService;
        private readonly AppController _controller;
        private readonly PairInfo _validPairInfo;
        private readonly List<AlarmInfoDto> _validAlarmsList;
        private readonly AlarmStopPOGO _alarmStopPOGO;
        private readonly string _validToken;
        private readonly string _invalidToken;

        public AppControllerTests()
        {
            _mockAppService = new Mock<IAppService>();
            _controller = new AppController(_mockAppService.Object);
            _validPairInfo = new PairInfo(new AlarmInfoDto ("startTime", "endTime", "alarmId", "testName"), "testUsername");
            _validAlarmsList = new List<AlarmInfoDto> { new AlarmInfoDto ("startTime", "endTime", "alarmId", "testName") };
            _alarmStopPOGO = new AlarmStopPOGO("testId", "testUsername");
            _validToken = "valid_Token";
            _invalidToken = "invalid_Token";
        }

        private void SetAuthorizedRequestHeader()
        {
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    Request = { Headers = { { "token", _validToken } } }
                }
            };
        }

        private void SetUnauthorizedRequestHeader()
        {
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    Request = { Headers = {  } }
                }
            };
        }

        [Fact]
        public void UpdateAlarmInfo_ReturnsOkResult_WhenUpdateIsSuccessful()
        {
            // Arrange
            SetAuthorizedRequestHeader();
            _mockAppService.Setup(service => service.UpdateAlarmInfo(_validPairInfo.Username, _validPairInfo.AlarmInfo, _validToken)).Returns(true);

            // Act
            var result = _controller.UpdateAlarmInfo(_validPairInfo);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void UpdateAlarmInfo_ReturnsNotFoundResult_WhenUpdateFails()
        {
            // Arrange
            SetAuthorizedRequestHeader();
            _mockAppService.Setup(service => service.UpdateAlarmInfo(_validPairInfo.Username, _validPairInfo.AlarmInfo, _validToken)).Returns(false);

            // Act
            var result = _controller.UpdateAlarmInfo(_validPairInfo);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void UpdateAlarmInfo_ReturnsUnauthorizedResult_WhenTokenIsMissing()
        {
            // Arrange
            SetUnauthorizedRequestHeader();
            _mockAppService.Setup(service => service.UpdateAlarmInfo(_validPairInfo.Username, _validPairInfo.AlarmInfo, _invalidToken)).Returns(true);

            // Act
            var result = _controller.UpdateAlarmInfo(_validPairInfo);

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public void GetAlarms_ReturnsOkResult_WhenAlarmsAreFound()
        {
            // Arrange
            SetAuthorizedRequestHeader();
            _mockAppService.Setup(service => service.GetAlarms(_validPairInfo.Username, _validToken)).Returns(_validAlarmsList);

            // Act
            var result = _controller.GetAlarms(_validPairInfo.Username);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedAlarms = Assert.IsType<List<AlarmInfoDto>>(okResult.Value);
            Assert.Equal(_validAlarmsList, returnedAlarms);
        }

        [Fact]
        public void GetAlarms_ReturnsNotFoundResult_WhenNoAlarmsAreFound()
        {
            // Arrange
            SetAuthorizedRequestHeader();
            _mockAppService.Setup(service => service.GetAlarms(_validPairInfo.Username, _validToken)).Returns(new List<AlarmInfoDto>());

            // Act
            var result = _controller.GetAlarms(_validPairInfo.Username);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void GetAlarms_ReturnsUnauthorizedResult_WhenTokenIsMissing()
        {
            // Arrange
            SetUnauthorizedRequestHeader();
            _mockAppService.Setup(service => service.GetAlarms(_validPairInfo.Username, _invalidToken)).Returns(_validAlarmsList);

            // Act
            var result = _controller.GetAlarms(_validPairInfo.Username);

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public void PairAlarm_ReturnsCreatedResult_WhenPairingIsSuccessful()
        {
            // Arrange
            SetAuthorizedRequestHeader();
            _mockAppService.Setup(service => service.PairAlarm(_validPairInfo, _validToken)).Returns("Yes");

            // Act
            var result = _controller.PairAlarm(_validPairInfo);

            // Assert
            var createdResult = Assert.IsType<CreatedResult>(result);
            var returnedPairInfo = Assert.IsType<PairInfo>(createdResult.Value);
            Assert.Equal(_validPairInfo, returnedPairInfo);
        }

        [Fact]
        public void PairAlarm_ReturnsNotFoundResult_WhenPairingAlreadyExists()
        {
            // Arrange
            SetAuthorizedRequestHeader();
            _mockAppService.Setup(service => service.PairAlarm(_validPairInfo, _validToken)).Returns("No");

            // Act
            var result = _controller.PairAlarm(_validPairInfo);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void PairAlarm_ReturnsUnauthorizedResult_WhenTokenIsMissing()
        {
            // Arrange
            SetUnauthorizedRequestHeader();
            _mockAppService.Setup(service => service.PairAlarm(_validPairInfo, _invalidToken)).Returns("No");

            // Act
            var result = _controller.PairAlarm(_validPairInfo);

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public void StopAlarm_ReturnsOkResult_WhenStoppingAlarmIsSuccessful()
        {
            // Arrange
            SetAuthorizedRequestHeader();
            
            _mockAppService.Setup(service => service.StopAlarm(_alarmStopPOGO.AlarmID, _alarmStopPOGO.Username ,_validToken)).Returns(true);

            // Act
            var result = _controller.StopAlarm(_alarmStopPOGO);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void StopAlarm_ReturnsNotFoundResult_WhenStoppingAlarmFails()
        {
            // Arrange
            SetAuthorizedRequestHeader();
            _mockAppService.Setup(service => service.StopAlarm(_alarmStopPOGO.AlarmID, _alarmStopPOGO.Username, _validToken)).Returns(false);

            // Act
            var result = _controller.StopAlarm(_alarmStopPOGO);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void StopAlarm_ReturnsUnauthorizedResult_WhenTokenIsMissing()
        {
            // Arrange
            SetUnauthorizedRequestHeader();
            _mockAppService.Setup(service => service.StopAlarm(_alarmStopPOGO.AlarmID, _alarmStopPOGO.Username, _invalidToken)).Returns(true);

            // Act
            var result = _controller.StopAlarm(_alarmStopPOGO);

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }
    }
}
