using HåndværkervognenAPI.Database;
using HåndværkervognenAPI.Managers;
using HåndværkervognenAPI.Models;
using HåndværkervognenAPI.Security;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Håndværkervognen_Test.Manager_Tests
{
    public class AppManagerTests
    {
        private readonly Mock<IDatabase> _mockDatabase;
        private readonly Mock<IEncryption> _mockEncryption;
        private readonly AppManager _appManager;
        private readonly string _validUsername;
        private readonly string _validAlarmId;
        private readonly AlarmInfoDto _validAlarmInfoDto;
        private readonly AlarmDal _validAlarmDal;
        private readonly List<AlarmDal> _alarms;

        public AppManagerTests()
        {
            _mockDatabase = new Mock<IDatabase>();
            _mockEncryption = new Mock<IEncryption>();
            _appManager = new AppManager(_mockDatabase.Object, _mockEncryption.Object);
            _validUsername = "bananaGuy";
            _validAlarmId = "123";
            _validAlarmInfoDto = new AlarmInfoDto("08:00", "10:00", _validAlarmId, "Test Alarm");
            _validAlarmDal = new AlarmDal("08:00", "10:00", _validAlarmId, "Test Alarm");
            _alarms = new List<AlarmDal> { _validAlarmDal };
        }

        [Fact]
        public void GetAlarms_ReturnsListOfAlarms_WhenAlarmsAreFound()
        {
            // Arrange
            _mockDatabase.Setup(db => db.GetAlarms(_validUsername)).Returns(_alarms);
            _mockEncryption.Setup(enc => enc.DecryptData(It.IsAny<string>(), _validAlarmId)).Returns((string input, string id) => input);

            // Act
            var result = _appManager.GetAlarms(_validUsername);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(_validAlarmInfoDto.StartTime, result[0].StartTime);
            Assert.Equal(_validAlarmInfoDto.EndTime, result[0].EndTime);
            Assert.Equal(_validAlarmInfoDto.AlarmId, result[0].AlarmId);
            Assert.Equal(_validAlarmInfoDto.Name, result[0].Name);
        }
        [Fact]
        public void PairAlarm_ReturnsTrue_WhenPairingIsSuccessful()
        {
            // Arrange
            _mockDatabase.Setup(db => db.CheckIfPairExists(_validAlarmId, _validUsername)).Returns(false);
            _mockEncryption.Setup(enc => enc.EncryptData(It.IsAny<string>(), _validAlarmId)).Returns((string input, string id) => input);
            _mockDatabase.Setup(db => db.PairAlarms(_validUsername, _validAlarmDal));

            // Act
            var result = _appManager.PairAlarm(new PairInfo(_validAlarmInfoDto, _validUsername));

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void PairAlarm_ReturnsFalse_WhenPairingAlreadyExists()
        {
            // Arrange
            _mockDatabase.Setup(db => db.CheckIfPairExists(_validAlarmId, _validUsername)).Returns(true);

            // Act
            var result = _appManager.PairAlarm(new PairInfo(_validAlarmInfoDto, _validUsername));

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void StopAlarm_ReturnsTrue_WhenStoppingAlarmIsSuccessful()
        {
            // Arrange
            _mockDatabase.Setup(db => db.StopAlarm(_validAlarmId));

            // Act
            var result = _appManager.StopAlarm(_validAlarmId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void UpdateAlarmInfo_ReturnsTrue_WhenUpdatingAlarmInfoIsSuccessful()
        {
            // Arrange
            _mockDatabase.Setup(db => db.GetAlarmInfo(_validAlarmId)).Returns(new AlarmDal("08:00", "10:00", _validAlarmId, "Test Alarm"));
            _mockEncryption.Setup(enc => enc.EncryptData(It.IsAny<string>(), _validAlarmId)).Returns((string input, string id) => input);
            _mockDatabase.Setup(db => db.UpdateAlarmInfo(_validUsername, _validAlarmDal));

            // Act
            var result = _appManager.UpdateAlarmInfo(_validUsername, _validAlarmInfoDto);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void UpdateAlarmInfo_ReturnsFalse_WhenUpdatingAlarmInfoDidNotFindAlarm()
        {
            // Arrange
            _mockDatabase.Setup(db => db.GetAlarmInfo(_validAlarmId)).Returns((AlarmDal)null);

            // Act
            var result = _appManager.UpdateAlarmInfo(_validUsername, _validAlarmInfoDto);

            // Assert
            Assert.False(result);
        }

    }
}
