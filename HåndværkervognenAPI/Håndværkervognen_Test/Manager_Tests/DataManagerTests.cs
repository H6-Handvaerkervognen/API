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

       
    }
}
