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
        private readonly string _validToken;
        private readonly string _invalidToken;

        public AppManagerTests()
        {
            _mockDatabase = new Mock<IDatabase>();
            _mockEncryption = new Mock<IEncryption>();
            _appManager = new AppManager(_mockDatabase.Object, _mockEncryption.Object);
            _validUsername = "bananaGuy";
            _validAlarmId = "123";
            _validAlarmInfoDto = new AlarmInfoDto("08:00", "10:00", _validAlarmId, "Test Alarm");
            _validAlarmDal = new AlarmDal(new byte[0], new byte[0], _validAlarmId, new byte[0]);
            _alarms = new List<AlarmDal> { _validAlarmDal };
            _validToken = "valid_Token";
            _invalidToken = "invalid_Token";
        }

        [Fact]
        public void GetAlarms_ReturnsListOfAlarms_WhenAlarmsAreFound()
        {
            // Arrange
            _mockDatabase.Setup(db => db.GetAlarms(_validUsername)).Returns(_alarms);
            _mockDatabase.Setup(db => db.CheckToken(_validUsername, _validToken)).Returns(true);
            _mockEncryption.Setup(enc => enc.DecryptData(It.IsAny<byte[]>(), It.IsAny<string>())).Returns("decrypted_Data");

            // Act
            var result = _appManager.GetAlarms(_validUsername, _validToken);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
        }
        [Fact]
        public void PairAlarm_ReturnsTrue_WhenPairingIsSuccessful()
        {
            // Arrange
            _mockDatabase.Setup(db => db.CheckIfPairExists(_validAlarmId, _validUsername)).Returns(false);
            _mockDatabase.Setup(db => db.CheckToken(_validUsername, _validToken)).Returns(true);
            _mockEncryption.Setup(enc => enc.EncryptData(It.IsAny<string>(), _validAlarmId)).Returns(new byte[0]);
            _mockDatabase.Setup(db => db.PairAlarms(_validUsername, _validAlarmDal));

            // Act
            var result = _appManager.PairAlarm(new PairInfo(_validAlarmInfoDto, _validUsername), _validToken);

            // Assert
            Assert.True(result == "Yes");
        }

        [Fact]
        public void PairAlarm_ReturnsFalse_WhenPairingAlreadyExists()
        {
            // Arrange
            _mockDatabase.Setup(db => db.CheckIfPairExists(_validAlarmId, _validUsername)).Returns(true);

            // Act
            var result = _appManager.PairAlarm(new PairInfo(_validAlarmInfoDto, _validUsername), _validToken);

            // Assert
            Assert.True(result == "No");
        }

        [Fact]
        public void StopAlarm_ReturnsTrue_WhenStoppingAlarmIsSuccessful()
        {
            // Arrange
            _mockDatabase.Setup(db => db.StopAlarm(_validAlarmId));
            _mockDatabase.Setup(db => db.CheckToken(_validUsername, _validToken)).Returns(true);

            // Act
            var result = _appManager.StopAlarm(_validAlarmId, _validUsername, _validToken);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void UpdateAlarmInfo_ReturnsTrue_WhenUpdatingAlarmInfoIsSuccessful()
        {
            // Arrange
            _mockDatabase.Setup(db => db.GetAlarmInfo(_validAlarmId)).Returns(_validAlarmDal);
            _mockDatabase.Setup(db => db.CheckToken(_validUsername, _validToken)).Returns(true);
            _mockEncryption.Setup(enc => enc.EncryptData(It.IsAny<string>(), _validAlarmId)).Returns(new byte[0]);
            _mockDatabase.Setup(db => db.UpdateAlarmInfo(_validUsername, _validAlarmDal));

            // Act
            var result = _appManager.UpdateAlarmInfo(_validUsername, _validAlarmInfoDto, _validToken);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void UpdateAlarmInfo_ReturnsFalse_WhenUpdatingAlarmInfoDidNotFindAlarm()
        {
            // Arrange
            _mockDatabase.Setup(db => db.GetAlarmInfo(_validAlarmId)).Returns((AlarmDal)null);

            // Act
            var result = _appManager.UpdateAlarmInfo(_validUsername, _validAlarmInfoDto, _validToken);

            // Assert
            Assert.False(result);
        }

    }
}
