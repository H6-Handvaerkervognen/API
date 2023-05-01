using HåndværkervognenAPI.Database;
using HåndværkervognenAPI.Models;
using HåndværkervognenAPI.Security;
using System.Security.Cryptography.Xml;
using System.Text;

namespace HåndværkervognenAPI.Managers
{
    public class AppManager : IAppService
    {
        private IDatabase _database;
        private IEncryption _encryption;

        public AppManager(IDatabase database, IEncryption encryption)
        {
            _database = database;
            _encryption = encryption;
        }

        /// <summary>
        /// Gets alarms for a specific user and converts from alarmDal to alarminfoDTO before returning a list of alarminfoDTO
        /// </summary>
        /// <param name="username"></param>
        /// <returns>list of alarminfoDTO</returns>
        public List<AlarmInfoDto> GetAlarms(string username, string token)
        {
            List<AlarmInfoDto> alarmInfoDtos = new List<AlarmInfoDto>();
            if (_database.CheckToken(username, token))
            {
                List<AlarmDal> alarms = _database.GetAlarms(username);

                
                foreach (AlarmDal alarm in alarms)
                {
                    alarmInfoDtos.Add(new AlarmInfoDto(_encryption.DecryptData(alarm.StartTime, alarm.AlarmId), _encryption.DecryptData(alarm.EndTime, alarm.AlarmId), alarm.AlarmId, _encryption.DecryptData(alarm.Name, alarm.AlarmId)));
                }
                
            }
            return alarmInfoDtos;
        }

        /// <summary>
        /// creates a new salt for alarmid if its the first time the alarm is paired. Encrypts the rest of the data and stores it
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public string PairAlarm(PairInfo info, string token)
        {
            if (_database.CheckToken(info.Username,token))
            {
                try
                {
                    AlarmDal alarmDal;
                    if (!_database.CheckIfPairExists(info.AlarmInfo.AlarmId, info.Username))
                    {
                        if (!_database.CheckIfAlarmExists(info.AlarmInfo.AlarmId))
                        {
                            _encryption.AssignNewKeys(info.AlarmInfo.AlarmId);
                        }
                        alarmDal = new AlarmDal(_encryption.EncryptData(info.AlarmInfo.StartTime, info.AlarmInfo.AlarmId), _encryption.EncryptData(info.AlarmInfo.EndTime, info.AlarmInfo.AlarmId), info.AlarmInfo.AlarmId, _encryption.EncryptData(info.AlarmInfo.Name, info.AlarmInfo.AlarmId));
                        _database.PairAlarms(info.Username, alarmDal);

                        return "Yes";
                    }
                    return "No";
                }
                catch (Exception e)
                {
                    return "Mesaage: " + e.Message + " \n Stacktrace:" + e.StackTrace + "\n InnerException: " + e.InnerException + "\n Source: " + e.Source + "\n HResult: " + e.HResult + "\n Data:" + e.Data;
                }
            }
            return "No";
        }

        /// <summary>
        /// stops a specific alarm
        /// </summary>
        /// <param name="AlarmId"></param>
        /// <returns></returns>
        public bool StopAlarm(string alarmId, string username, string token)
        {
            if (_database.CheckToken(username,token))
            {
                _database.StopAlarm(alarmId);
                return true;
            }
            return false;
        }

        /// <summary>
        /// converts the data into a <see cref="AlarmDal"/> and updates the new values
        /// </summary>
        /// <param name="username"></param>
        /// <param name="alarmInfo"></param>
        /// <returns></returns>
        public bool UpdateTimeSpan(string username, AlarmInfoDto alarmInfo, string token)
        {
            if (_database.CheckToken(username, token))
            {
                var data = _database.GetAlarmInfo(alarmInfo.AlarmId);
                if (alarmInfo.AlarmId == data.AlarmId)
                {
                    AlarmDal alarm = new AlarmDal(_encryption.EncryptData(alarmInfo.StartTime, alarmInfo.AlarmId), _encryption.EncryptData(alarmInfo.EndTime, alarmInfo.AlarmId), alarmInfo.AlarmId, _encryption.EncryptData(alarmInfo.Name, alarmInfo.AlarmId));
                    _database.UpdateTimespan(username, alarm);
                    return true;
                }

                return false;
            }
            return false;

            
        }
    }
}
