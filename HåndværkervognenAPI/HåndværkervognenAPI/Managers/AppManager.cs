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
        private IHashing _hashing;

        public AppManager(IDatabase database, IEncryption encryption, IHashing hashing)
        {
            _database = database;
            _encryption = encryption;
            _hashing = hashing;
        }

        /// <summary>
        /// calls database and gets alarms for aspecific user and converts from alarmDal to alarminfoDTO before returning a list of alarminfoDTO
        /// </summary>
        /// <param name="username"></param>
        /// <returns>list of alarminfoDTO</returns>
        public List<AlarmInfoDto> GetAlarms(string username)
        {

            List<AlarmDal> alarms = _database.GetAlarms(username);

            List<AlarmInfoDto> alarmInfoDtos = new List<AlarmInfoDto>();
            foreach (AlarmDal alarm in alarms)
            {
                alarmInfoDtos.Add(new AlarmInfoDto(_encryption.DecryptData(alarm.StartTime, alarm.AlarmId), _encryption.DecryptData(alarm.EndTime, alarm.AlarmId), alarm.AlarmId, _encryption.DecryptData(alarm.Name, alarm.AlarmId)));
            }
            return alarmInfoDtos;
        }

        /// <summary>
        /// creates a new salt for alarmid if its the first time the alarm is paired. then it hashes the alarm id and encrypts the rest of the data and stores it 
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public string PairAlarm(PairInfo info)
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
                return "Mesaage: " + e.Message + " \n Stacktrace:" + e.StackTrace + "\n InnerException: "+ e.InnerException + "\n Source: " + e.Source + "\n HResult: " + e.HResult + "\n Data:" + e.Data;
            }
        }

        /// <summary>
        /// stops a specific alarm
        /// </summary>
        /// <param name="AlarmId"></param>
        /// <returns></returns>
        public bool StopAlarm(string alarmId)
        {
            _database.StopAlarm(alarmId);
            return true;
        }

        /// <summary>
        /// converts the data to the rigth objects and saves the new timespan
        /// </summary>
        /// <param name="username"></param>
        /// <param name="alarmInfo"></param>
        /// <returns></returns>
        public bool UpdateTimeSpan(string username, AlarmInfoDto alarmInfo)
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
    }
}
