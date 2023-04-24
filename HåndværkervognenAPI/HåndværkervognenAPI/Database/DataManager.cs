using HåndværkervognenAPI.Model;
using HåndværkervognenAPI.Models;
using System.Data.SqlClient;

namespace HåndværkervognenAPI.Database
{
    public class DataManager : IDatabase
    {
        //SERVER
        //string _connString = "Server=ZBC-E-RO-23245;Database=haandvaerkervognen;Uid=sa;Pwd=straWb3rr%;";
        
        string _connString = "Server=ZBC-E-RO-23245;Database=haandvaerkervognen;Trusted_Connection=True;";
        SqlConnection _sqlConnection;
        SqlCommand _sqlCommand;
        SqlDataReader _sqlDataReader;

        private void CommandCreate(string procedure)
        {
            _sqlCommand = _sqlConnection.CreateCommand();
            _sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            _sqlCommand.CommandText = procedure;
        }
        public void CreateUser(UserDal user)
        {
            using (_sqlConnection = new SqlConnection(_connString))
            {
                CommandCreate("CreateUser");
                _sqlCommand.Parameters.AddWithValue("Username", user.UserName);
                _sqlCommand.Parameters.AddWithValue("Password", user.HashPassword);
                _sqlCommand.Parameters.AddWithValue("Salt", user.Salt);
                _sqlCommand.Connection.Open();
                _sqlCommand.ExecuteNonQuery();
            }
        }

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

        public List<AlarmDal> GetAlarms(string appId)
        {
            List<AlarmDal> alarms = new List<AlarmDal>();
            using (_sqlConnection = new SqlConnection(_connString))
            {
                CommandCreate("GetAlarmsByUser");
                _sqlCommand.Parameters.AddWithValue("Username", appId);
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
                _sqlCommand.Parameters.AddWithValue("Username", appID);
                _sqlCommand.Parameters.AddWithValue("AlarmId", alarmInfo.AlarmId);
                _sqlCommand.Parameters.AddWithValue("StartTime", alarmInfo.StartTime);
                _sqlCommand.Parameters.AddWithValue("EndTime", alarmInfo.EndTime);
                _sqlCommand.Parameters.AddWithValue("Name", alarmInfo.Name);
                _sqlCommand.Parameters.AddWithValue("Salt", alarmInfo.Salt);
                _sqlCommand.Connection.Open();
                _sqlCommand.ExecuteNonQuery();
            }
        }

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

        public void UpdateTimespan(string appId, AlarmDal alarmDal)
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
    }
}
