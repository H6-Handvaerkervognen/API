using HåndværkervognenAPI.Database;
using HåndværkervognenAPI.Models;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Håndværkervognen_Test.Manager_Tests
{
    public class DataManagerTests
    {
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<SqlConnection> _mockSqlConnection;
        private readonly Mock<SqlCommand> _mockSqlCommand;
        private readonly Mock<SqlDataReader> _mockSqlDataReader;
        private readonly DataManager _dataManager;

        public DataManagerTests()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockSqlConnection = new Mock<SqlConnection>();
            _mockSqlCommand = new Mock<SqlCommand>();
            _mockSqlDataReader = new Mock<SqlDataReader>();
            _dataManager = new DataManager(_mockConfiguration.Object);

            _mockConfiguration.Setup(c => c.GetConnectionString(It.IsAny<string>())).Returns("TestConnectionString");
        }

        [Fact]
        public void CreateUser_Calls_CreateUser_StoredProcedure()
        {
            // Arrange
            _mockSqlCommand.SetupSet(c => c.CommandText = "CreateUser");
            _mockSqlCommand.Setup(c => c.ExecuteNonQuery()).Verifiable();
            _mockSqlConnection.Setup(c => c.CreateCommand()).Returns(_mockSqlCommand.Object);

            var user = new UserDal("TestUser", "TestHashedPassword", new byte[0], "TestToken");

            // Act
            _dataManager.CreateUser(user);

            // Assert
            _mockSqlCommand.Verify(c => c.ExecuteNonQuery(), Times.Once);
        }

        [Fact]
        public void DeletePairing_Calls_DeletePairing_StoredProcedure()
        {
            // Arrange
            _mockSqlCommand.SetupSet(c => c.CommandText = "DeletePairing");
            _mockSqlCommand.Setup(c => c.ExecuteNonQuery()).Verifiable();
            _mockSqlConnection.Setup(c => c.CreateCommand()).Returns(_mockSqlCommand.Object);

            // Act
            _dataManager.DeletePairing("TestAlarmId");

            // Assert
            _mockSqlCommand.Verify(c => c.ExecuteNonQuery(), Times.Once);
        }

        [Fact]
        public void GetAlarmInfo_Returns_AlarmDal_When_AlarmExists()
        {
            // Arrange
            _mockSqlCommand.SetupSet(c => c.CommandText = "GetAlarmInfo");
            _mockSqlCommand.Setup(c => c.ExecuteReader()).Returns(_mockSqlDataReader.Object);
            _mockSqlConnection.Setup(c => c.CreateCommand()).Returns(_mockSqlCommand.Object);

            _mockSqlDataReader.Setup(r => r.Read()).Returns(true);
            _mockSqlDataReader.Setup(r => r["startTime"]).Returns(new byte[] { 1, 2, 3 });
            _mockSqlDataReader.Setup(r => r["endTime"]).Returns(new byte[] { 4, 5, 6 });
            _mockSqlDataReader.Setup(r => r["Id"]).Returns("TestAlarmId");
            _mockSqlDataReader.Setup(r => r["Name"]).Returns(new byte[] { 7, 8, 9 });

            // Act
            var result = _dataManager.GetAlarmInfo("TestAlarmId");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("TestAlarmId", result.AlarmId);
        }

        [Fact]
        public void GetAlarms_Returns_ListOfAlarms_ForGivenUser()
        {
            // Arrange
            _mockSqlCommand.SetupSet(c => c.CommandText = "GetAlarmsByUser");
            _mockSqlCommand.Setup(c => c.ExecuteReader()).Returns(_mockSqlDataReader.Object);
            _mockSqlConnection.Setup(c => c.CreateCommand()).Returns(_mockSqlCommand.Object);

            _mockSqlDataReader.SetupSequence(r => r.Read()).Returns(true).Returns(false);
            _mockSqlDataReader.Setup(r => r["startTime"]).Returns(new byte[] { 1, 2, 3 });
            _mockSqlDataReader.Setup(r => r["endTime"]).Returns(new byte[] { 4, 5, 6 });
            _mockSqlDataReader.Setup(r => r["Id"]).Returns("TestAlarmId");
            _mockSqlDataReader.Setup(r => r["Name"]).Returns(new byte[] { 7, 8, 9 });

            // Act
            var result = _dataManager.GetAlarms("TestUser");

            // Assert
            Assert.Single(result);
            Assert.Equal("TestAlarmId", result[0].AlarmId);
        }

        [Fact]
        public void GetUser_Returns_UserDal_When_UserExists()
        {
            // Arrange
            _mockSqlCommand.SetupSet(c => c.CommandText = "GetUser");
            _mockSqlCommand.Setup(c => c.ExecuteReader()).Returns(_mockSqlDataReader.Object);
            _mockSqlConnection.Setup(c => c.CreateCommand()).Returns(_mockSqlCommand.Object);

            _mockSqlDataReader.Setup(r => r.Read()).Returns(true);
            _mockSqlDataReader.Setup(r => r.GetString(0)).Returns("TestUser");
            _mockSqlDataReader.Setup(r => r.GetString(1)).Returns("TestHashedPassword");
            _mockSqlDataReader.Setup(r => r["salt"]).Returns(new byte[] { 1, 2, 3 });
            _mockSqlDataReader.Setup(r => r.GetString(3)).Returns("TestToken");

            // Act
            var result = _dataManager.GetUser("TestUser");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("TestUser", result.UserName);
        }

        [Fact]
        public void PairAlarms_Calls_AddPair_StoredProcedure()
        {
            // Arrange
            _mockSqlCommand.SetupSet(c => c.CommandText = "AddPair");
            _mockSqlCommand.Setup(c => c.ExecuteNonQuery()).Verifiable();
            _mockSqlConnection.Setup(c => c.CreateCommand()).Returns(_mockSqlCommand.Object);

            var alarmInfo = new AlarmDal(new byte[] { 1, 2, 3 }, new byte[] { 4, 5, 6 }, "TestAlarmId", new byte[] { 7, 8, 9 });

            // Act
            _dataManager.PairAlarms("TestUser", alarmInfo);

            // Assert
            _mockSqlCommand.Verify(c => c.ExecuteNonQuery(), Times.Once);
        }

        [Fact]
        public void StopAlarm_Calls_StopAlarm_StoredProcedure()
        {
            // Arrange
            _mockSqlCommand.SetupSet(c => c.CommandText = "StopAlarm");
            _mockSqlCommand.Setup(c => c.ExecuteNonQuery()).Verifiable();
            _mockSqlConnection.Setup(c => c.CreateCommand()).Returns(_mockSqlCommand.Object);

            // Act
            _dataManager.StopAlarm("TestAlarmId");

            // Assert
            _mockSqlCommand.Verify(c => c.ExecuteNonQuery(), Times.Once);
        }

        [Fact]
        public void UpdateAlarmInfo_Calls_UpdateActiveHours_StoredProcedure()
        {
            // Arrange
            _mockSqlCommand.SetupSet(c => c.CommandText = "UpdateActiveHours");
            _mockSqlCommand.Setup(c => c.ExecuteNonQuery()).Verifiable();
            _mockSqlConnection.Setup(c => c.CreateCommand()).Returns(_mockSqlCommand.Object);

            var alarmInfo = new AlarmDal(new byte[] { 1, 2, 3 }, new byte[] { 4, 5, 6 }, "TestAlarmId", new byte[] { 7, 8, 9 });

            // Act
            _dataManager.UpdateAlarmInfo("TestUser", alarmInfo);

            // Assert
            _mockSqlCommand.Verify(c => c.ExecuteNonQuery(), Times.Once);
        }

        [Fact]
        public void StartAlarm_Calls_StartAlarm_StoredProcedure()
        {
            // Arrange
            _mockSqlCommand.SetupSet(c => c.CommandText = "StartAlarm");
            _mockSqlCommand.Setup(c => c.ExecuteNonQuery()).Verifiable();
            _mockSqlConnection.Setup(c => c.CreateCommand()).Returns(_mockSqlCommand.Object);

            // Act
            _dataManager.StartAlarm("TestAlarmId");

            // Assert
            _mockSqlCommand.Verify(c => c.ExecuteNonQuery(), Times.Once);
        }

        [Fact]
        public void CheckIfUserExists_Returns_True_If_User_Exists()
        {
            // Arrange
            _mockSqlCommand.SetupSet(c => c.CommandText = "CheckIfUserExists");
            _mockSqlCommand.Setup(c => c.ExecuteReader()).Returns(_mockSqlDataReader.Object);
            _mockSqlConnection.Setup(c => c.CreateCommand()).Returns(_mockSqlCommand.Object);

            _mockSqlDataReader.Setup(r => r.Read()).Returns(true);
            _mockSqlDataReader.Setup(r => r.GetInt32(0)).Returns(1);


            // Act
            bool userExists = _dataManager.CheckIfUserExists("TestUser");

            // Assert
            Assert.True(userExists);
        }

        [Fact]
        public void CheckIfPairExists_Returns_True_If_Pair_Exists()
        {
            // Arrange
            _mockSqlCommand.SetupSet(c => c.CommandText = "CheckIfPairExists");
            _mockSqlCommand.Setup(c => c.ExecuteReader()).Returns(_mockSqlDataReader.Object);
            _mockSqlConnection.Setup(c => c.CreateCommand()).Returns(_mockSqlCommand.Object);

            _mockSqlDataReader.Setup(r => r.Read()).Returns(true);
            _mockSqlDataReader.Setup(r => r.GetInt32(0)).Returns(1);


            // Act
            bool pairExists = _dataManager.CheckIfPairExists("TestAlarmId", "TestUser");

            // Assert
            Assert.True(pairExists);
        }

        [Fact]
        public void CheckIfAlarmExists_Returns_True_If_Alarm_Exists()
        {
            // Arrange
            _mockSqlCommand.SetupSet(c => c.CommandText = "CheckIfAlarmExists");
            _mockSqlCommand.Setup(c => c.ExecuteReader()).Returns(_mockSqlDataReader.Object);
            _mockSqlConnection.Setup(c => c.CreateCommand()).Returns(_mockSqlCommand.Object);

            _mockSqlDataReader.Setup(r => r.Read()).Returns(true);
            _mockSqlDataReader.Setup(r => r.GetInt32(0)).Returns(1);


            // Act
            bool alarmExists = _dataManager.CheckIfAlarmExists("TestAlarmId");

            // Assert
            Assert.True(alarmExists);
        }

        [Fact]
        public void CheckAlarmStatus_Returns_Status()
        {
            // Arrange
            _mockSqlCommand.SetupSet(c => c.CommandText = "GetAlarmStatus");
            _mockSqlCommand.Setup(c => c.ExecuteReader()).Returns(_mockSqlDataReader.Object);
            _mockSqlConnection.Setup(c => c.CreateCommand()).Returns(_mockSqlCommand.Object);

            _mockSqlDataReader.Setup(r => r.Read()).Returns(true);
            _mockSqlDataReader.Setup(r => r.GetBoolean(0)).Returns(true);


            // Act
            bool alarmStatus = _dataManager.CheckAlarmStatus("TestAlarmId");

            // Assert
            Assert.True(alarmStatus);
        }

        [Fact]
        public void CheckToken_Returns_True_If_Token_Is_Valid()
        {
            // Arrange
            _mockSqlCommand.SetupSet(c => c.CommandText = "CheckToken");
            _mockSqlCommand.Setup(c => c.ExecuteReader()).Returns(_mockSqlDataReader.Object);
            _mockSqlConnection.Setup(c => c.CreateCommand()).Returns(_mockSqlCommand.Object);

            _mockSqlDataReader.Setup(r => r.Read()).Returns(true);
            _mockSqlDataReader.Setup(r => r.GetInt32(0)).Returns(1);


            // Act
            bool tokenValid = _dataManager.CheckToken("TestUser", "TestToken");

            // Assert
            Assert.True(tokenValid);
        }
    }
}
