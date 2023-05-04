using HåndværkervognenAPI.Models;
using System.Data.SqlClient;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting.Server;

namespace HåndværkervognenAPI.Database
{
    public class DataManager : IDatabase
    {
        private readonly IConfiguration _configuration;
        private string _connString;

        SqlConnection _sqlConnection;
        SqlCommand _sqlCommand;
        SqlDataReader _sqlDataReader;

        public DataManager(IConfiguration configuration)
        {
            _configuration = configuration;
            _connString = _configuration.GetConnectionString("serverConn");
        }

        /// <summary>
        /// Creates a new command, changes the type to stored procedure and sets the commandtext 
        /// </summary>
        /// <param name="procedure">name of the procedure to execute</param>
        private void CommandCreate(string procedure)
        {
            _sqlCommand = _sqlConnection.CreateCommand();
            _sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            _sqlCommand.CommandText = procedure;
        }



        /// <summary>
        /// Inserts a new user in the database
        /// </summary>
        /// <param name="user">the user to create</param>
        public void CreateUser(UserDal user)
        {
            using (_sqlConnection = new SqlConnection(_connString))
            {
                CommandCreate("CreateUser");
                _sqlCommand.Parameters.AddWithValue("Username", user.UserName);
                _sqlCommand.Parameters.AddWithValue("Password", user.HashPassword);
                _sqlCommand.Parameters.AddWithValue("Salt", user.Salt);
                _sqlCommand.Parameters.AddWithValue("Token", user.Token);
                _sqlCommand.Connection.Open();
                _sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Deletes a pairing from the database
        /// </summary>
        /// <param name="alarmId">the id of the alarm</param>
        /// <param name="username">the username</param>
        public void DeletePairing(string alarmId)
        {
            using (_sqlConnection = new SqlConnection(_connString))
            {
                CommandCreate("DeletePairing");
                _sqlCommand.Parameters.AddWithValue("AlarmId", alarmId);
                _sqlCommand.Connection.Open();
                _sqlCommand.ExecuteNonQuery();
            }
        }


        /// <summary>
        /// Get the info on an alarm from the database
        /// </summary>
        /// <param name="alarmId">the id of the alarm to get info on</param>
        /// <returns></returns>
        public AlarmDal GetAlarmInfo(string alarmId)
        {
            using (_sqlConnection = new SqlConnection(_connString))
            {
                CommandCreate("GetAlarmInfo");
                _sqlCommand.Parameters.AddWithValue("AlarmId", alarmId);
                _sqlCommand.Connection.Open();
                _sqlDataReader = _sqlCommand.ExecuteReader();
                while (_sqlDataReader.Read())
                {
                    return new AlarmDal((byte[])_sqlDataReader["startTime"], (byte[])_sqlDataReader["endTime"], (string)_sqlDataReader["Id"], (byte[])_sqlDataReader["Name"]);
                }
                _sqlDataReader.Close();

            }

            return null;
        }

        /// <summary>
        /// Gets all the alarms associated with a user
        /// </summary>
        /// <param name="username">the user</param>
        /// <returns></returns>
        public List<AlarmDal> GetAlarms(string username)
        {
            List<AlarmDal> alarms = new List<AlarmDal>();
            using (_sqlConnection = new SqlConnection(_connString))
            {
                CommandCreate("GetAlarmsByUser");
                _sqlCommand.Parameters.AddWithValue("Username", username);
                _sqlCommand.Connection.Open();
                _sqlDataReader = _sqlCommand.ExecuteReader();
                while (_sqlDataReader.Read())
                {
                    AlarmDal alarm = new AlarmDal((byte[])_sqlDataReader["startTime"], (byte[])_sqlDataReader["endTime"], (string)_sqlDataReader["Id"], (byte[])_sqlDataReader["Name"]);
                    alarms.Add(alarm);
                }
                _sqlDataReader.Close();
            }
            return alarms;
        }

        /// <summary>
        /// Retrieves a user from the database from a usename
        /// </summary>
        /// <param name="username">username of the user to get</param>
        /// <returns></returns>
        public UserDal GetUser(string username)
        {
            using (_sqlConnection = new SqlConnection(_connString))
            {
                CommandCreate("GetUser");
                _sqlCommand.Parameters.AddWithValue("Username", username);
                _sqlCommand.Connection.Open();
                _sqlDataReader = _sqlCommand.ExecuteReader();
                while (_sqlDataReader.Read())
                {
                    UserDal user = new UserDal(_sqlDataReader.GetString(0), _sqlDataReader.GetString(1), (byte[])_sqlDataReader["salt"], _sqlDataReader.GetString(3));
                    return user;
                }
                _sqlDataReader.Close();
            }
            return null;
        }

        /// <summary>
        /// Adds a user and alarm pair to the database
        /// </summary>
        /// <param name="username">the username of the user</param>
        /// <param name="alarmInfo">the id of the alarm</param>
        public void PairAlarms(string username, AlarmDal alarmInfo)
        {
            using (_sqlConnection = new SqlConnection(_connString))
            {
                CommandCreate("AddPair");
                _sqlCommand.Parameters.AddWithValue("Username", username);
                _sqlCommand.Parameters.AddWithValue("AlarmId", alarmInfo.AlarmId);
                _sqlCommand.Parameters.AddWithValue("StartTime", alarmInfo.StartTime);
                _sqlCommand.Parameters.AddWithValue("EndTime", alarmInfo.EndTime);
                _sqlCommand.Parameters.AddWithValue("Name", alarmInfo.Name);
                _sqlCommand.Connection.Open();
                _sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Updates the field in the database that indicates whether the alarm is on, to off
        /// </summary>
        /// <param name="alarmId"></param>
        public void StopAlarm(string alarmId)
        {
            using (_sqlConnection = new SqlConnection(_connString))
            {
                CommandCreate("StopAlarm");
                _sqlCommand.Parameters.AddWithValue("AlarmId", alarmId);
                _sqlCommand.Parameters.AddWithValue("AlarmOn", 0);
                _sqlCommand.Connection.Open();
                _sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Updates an alarms active hours
        /// </summary>
        /// <param name="username">username</param>
        /// <param name="alarmDal">The alarm to update</param>
        public void UpdateAlarmInfo(string username, AlarmDal alarmDal)
        {
            using (_sqlConnection = new SqlConnection(_connString))
            {
                CommandCreate("UpdateActiveHours");
                _sqlCommand.Parameters.AddWithValue("AlarmId", alarmDal.AlarmId);
                _sqlCommand.Parameters.AddWithValue("startTime", alarmDal.StartTime);
                _sqlCommand.Parameters.AddWithValue("EndTime", alarmDal.EndTime);
                _sqlCommand.Connection.Open();
                _sqlCommand.ExecuteNonQuery();
            }
        }


        /// <summary>
        /// Updates the field in the database that indicates whether the alarm is on, to on
        /// </summary>
        /// <param name="alarmId">the id of the alarm to update</param>
        public void StartAlarm(string alarmId)
        {
            using (_sqlConnection = new SqlConnection(_connString))
            {
                CommandCreate("StartAlarm");
                _sqlCommand.Parameters.AddWithValue("AlarmId", alarmId);
                _sqlCommand.Parameters.AddWithValue("AlarmOn", 1);
                _sqlCommand.Connection.Open();
                _sqlCommand.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// Checks to see if a user with the username exists in the database
        /// </summary>
        /// <param name="username">Username to check for</param>
        /// <returns>True if user with the username exists, false if user doesn't exists</returns>
        public bool CheckIfUserExists(string username)
        {
            using (_sqlConnection = new SqlConnection(_connString))
            {
                CommandCreate("CheckIfUserExists");
                _sqlCommand.Parameters.AddWithValue("Username", username);
                _sqlCommand.Connection.Open();
                _sqlDataReader = _sqlCommand.ExecuteReader();
                while (_sqlDataReader.Read())
                {
                    if (_sqlDataReader.GetInt32(0) == 1)
                    {
                        return true;
                    }
                }
                _sqlDataReader.Close();
            }
            return false;
        }

        /// <summary>
        /// Checks if the pair exists in the database
        /// </summary>
        /// <param name="alarmId">The alarms id</param>
        /// <param name="username">The user</param>
        /// <returns></returns>
        public bool CheckIfPairExists(string alarmId, string username)
        {
            using (_sqlConnection = new SqlConnection(_connString))
            {
                CommandCreate("CheckIfPairExists");
                _sqlCommand.Parameters.AddWithValue("AlarmId", alarmId);
                _sqlCommand.Parameters.AddWithValue("Username", username);
                _sqlCommand.Connection.Open();
                _sqlDataReader = _sqlCommand.ExecuteReader();
                while (_sqlDataReader.Read())
                {
                    if (_sqlDataReader.GetInt32(0) == 1)
                    {
                        return true;
                    }
                }
                _sqlDataReader.Close();
            }
            return false;
        }

        /// <summary>
        /// Checks if the alarm exists in the database
        /// </summary>
        /// <param name="alarmId">The alarms id</param>
        /// <returns></returns>
        public bool CheckIfAlarmExists(string alarmId)
        {
            using (_sqlConnection = new SqlConnection(_connString))
            {
                CommandCreate("CheckIfAlarmExists");
                _sqlCommand.Parameters.AddWithValue("AlarmId", alarmId);
                _sqlCommand.Connection.Open();
                _sqlDataReader = _sqlCommand.ExecuteReader();
                while (_sqlDataReader.Read())
                {
                    if (_sqlDataReader.GetInt32(0) == 1)
                    {
                        return true;
                    }
                }
                _sqlDataReader.Close();
            }
            return false;
        }

        /// <summary>
        /// Checks if the alarm is on or off
        /// </summary>
        /// <param name="alarmId">The alarms id</param>
        /// <returns></returns>
        public bool CheckAlarmStatus(string alarmId)
        {
            bool status = false;
            using (_sqlConnection = new SqlConnection(_connString))
            {
                CommandCreate("GetAlarmStatus");
                _sqlCommand.Parameters.AddWithValue("AlarmId", alarmId);
                _sqlCommand.Connection.Open();
                _sqlDataReader = _sqlCommand.ExecuteReader();
                while (_sqlDataReader.Read())
                {
                    status = _sqlDataReader.GetBoolean(0);
                }
                _sqlDataReader.Close();
            }
            return status;
        }

        /// <summary>
        /// Sees if there exists an entry in the database where both the username and token match
        /// </summary>
        /// <param name="username">The username</param>
        /// <param name="token">The token to look for</param>
        /// <returns></returns>
        public bool CheckToken(string username, string token)
        {
            using (_sqlConnection = new SqlConnection(_connString))
            {
                CommandCreate("CheckToken");
                _sqlCommand.Parameters.AddWithValue("Username", username);
                _sqlCommand.Parameters.AddWithValue("Token", token);
                _sqlCommand.Connection.Open();
                _sqlDataReader = _sqlCommand.ExecuteReader();
                while (_sqlDataReader.Read())
                {
                    if (_sqlDataReader.GetInt32(0) == 1)
                    {
                        return true;
                    }
                }
                _sqlDataReader.Close();
            }
            return false;
        }
    }
}
