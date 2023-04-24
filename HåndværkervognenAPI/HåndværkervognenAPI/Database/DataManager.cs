using HåndværkervognenAPI.Model;
using HåndværkervognenAPI.Models;
using System.Data.SqlClient;

namespace HåndværkervognenAPI.Database
{
    public class DataManager : IDatabase
    {
        string _connString = "";
        SqlConnection _sqlConnection;
        SqlCommand _sqlCommand;
        SqlDataReader _sqlDataReader;

        private void CommandCreate(string procedure)
        {
            _sqlCommand = _sqlConnection.CreateCommand();
            _sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            _sqlCommand.CommandText = procedure;
        }
        public void createUser(UserDal user)
        {
            using (_sqlConnection = new SqlConnection(_connString))
            {
                CommandCreate("CreateUser");
                _sqlCommand.Parameters.AddWithValue("username", user.UserName);
                _sqlCommand.Parameters.AddWithValue("username", user.HashPassword);
                _sqlCommand.Parameters.AddWithValue("username", user.Salt);
                _sqlCommand.Connection.Open();
                _sqlCommand.ExecuteNonQuery();
            }
        }

        public void DeletePairing(string alarmId)
        {
            using (_sqlConnection = new SqlConnection(_connString))
            {
                CommandCreate("DeletePairing");
                _sqlCommand.Parameters.AddWithValue("alarmId", alarmId);
                _sqlCommand.Connection.Open();
                _sqlCommand.ExecuteNonQuery();
            }
        }

        public void DeleteUser(string username)
        {
            using (_sqlConnection = new SqlConnection(_connString))
            {
                CommandCreate("DeleteUser");
                _sqlCommand.Parameters.AddWithValue("username", username);
                _sqlCommand.Connection.Open();
                _sqlCommand.ExecuteNonQuery();
            }
        }

        public AlarmDal GetAlarmInfo(string alarmId)
        {
            AlarmDal alarm = new AlarmDal();
            using (_sqlConnection = new SqlConnection(_connString))
            {
                CommandCreate("GetAlarmInfo");
                _sqlCommand.Parameters.AddWithValue("alarmId", alarmId);
                _sqlCommand.Connection.Open();
                _sqlDataReader = _sqlCommand.ExecuteReader();
                while (_sqlDataReader.Read())
                {
                    alarm.StartTime = _sqlDataReader.GetString(0);
                    alarm.EndTime = _sqlDataReader.GetString(1);
                    alarm.Name = _sqlDataReader.GetString(2);
                }
                _sqlDataReader.Close();
            }
            return alarm;
        }

        public List<AlarmDal> GetAlarms(string appId)
        {
            List<AlarmDal> alarms = new List<AlarmDal>();
            using (_sqlConnection = new SqlConnection(_connString))
            {
                CommandCreate("GetAlarmsByUser");
                _sqlCommand.Parameters.AddWithValue("username", appId);
                _sqlCommand.Connection.Open();
                _sqlDataReader = _sqlCommand.ExecuteReader();
                while (_sqlDataReader.Read())
                {
                    AlarmDal alarm = new AlarmDal();
                    alarm.AlarmId = _sqlDataReader.GetString(0);
                    alarm.StartTime = _sqlDataReader.GetString(1);
                    alarm.EndTime = _sqlDataReader.GetString(2);
                    alarm.Name = _sqlDataReader.GetString(3);
                    alarms.Add(alarm);
                }
                _sqlDataReader.Close();
            }
            return alarms;
        }

        public UserDal GetUser(string username)
        {
            using (_sqlConnection = new SqlConnection(_connString))
            {
                CommandCreate("GetUser");
                _sqlCommand.Parameters.AddWithValue("username", username);
                _sqlCommand.Connection.Open();
                _sqlDataReader = _sqlCommand.ExecuteReader();
                while (_sqlDataReader.Read())
                {
                    UserDal user = new UserDal(_sqlDataReader.GetString(0), _sqlDataReader.GetString(1), (byte[])_sqlDataReader["salt"]);
                    return user;
                }
                _sqlDataReader.Close();
            }
            return null;
        }

        public void PairAlarms(string appID, AlarmDal alarmInfo)
        {
            using (_sqlConnection = new SqlConnection(_connString))
            {
                CommandCreate("AddPair");
                _sqlCommand.Parameters.AddWithValue("username", appID);
                _sqlCommand.Parameters.AddWithValue("alarmId", alarmInfo.AlarmId);
                _sqlCommand.Parameters.AddWithValue("startTime", alarmInfo.StartTime);
                _sqlCommand.Parameters.AddWithValue("endTime", alarmInfo.EndTime);
                _sqlCommand.Parameters.AddWithValue("name", alarmInfo.Name);
                _sqlCommand.Connection.Open();
                _sqlCommand.ExecuteNonQuery();
            }
        }

        public void StopAlarm(string alarmId)
        {
            using (_sqlConnection = new SqlConnection(_connString))
            {
                CommandCreate("StopAlarm");
                _sqlCommand.Parameters.AddWithValue("AlarmOn", 0);
                _sqlCommand.Connection.Open();
                _sqlCommand.ExecuteNonQuery();
            }
        }

        public void UpdateTimespan(string appId, AlarmDal alarmDal)
        {
            using (_sqlConnection = new SqlConnection(_connString))
            {
                CommandCreate("UpdateActiveHours");
                _sqlCommand.Parameters.AddWithValue("username", appId);
                _sqlCommand.Parameters.AddWithValue("startTime", alarmInfo.StartTime);
                _sqlCommand.Parameters.AddWithValue("endTime", alarmInfo.EndTime);
                _sqlCommand.Connection.Open();
                _sqlCommand.ExecuteNonQuery();
            }
        }
    }
}
