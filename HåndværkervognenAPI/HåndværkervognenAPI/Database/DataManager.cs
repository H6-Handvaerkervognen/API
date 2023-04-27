using HåndværkervognenAPI.Models;
using System.Data.SqlClient;
using System.Security.Claims;

namespace HåndværkervognenAPI.Database
{
    public class DataManager : IDatabase
    {
        //SERVER
        //string _connString = "Server=WIN-MBE1GM5TV9Q;Database=haandvaerkervognen;Uid=sa;Pwd=straWb3rr%;";

        string _connString = "Server=localhost;Database=haandvaerkervognen;Trusted_Connection=True;";
        SqlConnection _sqlConnection;
        SqlCommand _sqlCommand;
        SqlDataReader _sqlDataReader;

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
        public void DeletePairing(string alarmId, string username)
        {
            using (_sqlConnection = new SqlConnection(_connString))
            {
                CommandCreate("DeletePairing");
                _sqlCommand.Parameters.AddWithValue("Username", username);
                _sqlCommand.Parameters.AddWithValue("AlarmId", alarmId);
                _sqlCommand.Connection.Open();
                _sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Deletes a user from the database
        /// </summary>
        /// <param name="username">username of the user to delete</param>
        public void DeleteUser(string username)
        {
            using (_sqlConnection = new SqlConnection(_connString))
            {
                CommandCreate("DeleteUser");
                _sqlCommand.Parameters.AddWithValue("Username", username);
                _sqlCommand.Connection.Open();
                _sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Get the info on an alaarm from the database
        /// </summary>
        /// <param name="alarmId">the id of the alarm to get info on</param>
        /// <returns></returns>
        public AlarmDal GetAlarmInfo(string alarmId)
        {
            AlarmDal alarm;
            using (_sqlConnection = new SqlConnection(_connString))
            {
                CommandCreate("GetAlarmInfo");
                _sqlCommand.Parameters.AddWithValue("AlarmId", alarmId);
                _sqlCommand.Connection.Open();
                _sqlDataReader = _sqlCommand.ExecuteReader();
                while (_sqlDataReader.Read())
                {

                    alarm = new AlarmDal(_sqlDataReader.GetString(1), _sqlDataReader.GetString(2), _sqlDataReader.GetString(0), _sqlDataReader.GetString(3));

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
                    AlarmDal alarm = new AlarmDal(_sqlDataReader.GetString(1), _sqlDataReader.GetString(2), _sqlDataReader.GetString(0), _sqlDataReader.GetString(3));
                    alarms.Add(alarm);
                }
                _sqlDataReader.Close();
            }
            return alarms;
        }

        /// <summary>
        /// Retrieves a user from the daatabase from a usename
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
    }
}
