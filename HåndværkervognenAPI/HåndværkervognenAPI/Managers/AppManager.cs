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
        /// creates a new salt for alarmid if its the first time the alarm is paired. Encrypts the rest of the data and stores it
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool PairAlarm(PairInfo info)
        {
            try
            {
                AlarmDal alarmDal;
                if (!_database.CheckIfPairExists(info.AlarmInfo.AlarmId, info.Username))
                {
                    alarmDal = new AlarmDal(
                        _encryption.EncryptData(info.AlarmInfo.StartTime, info.AlarmInfo.AlarmId),
                        _encryption.EncryptData(info.AlarmInfo.EndTime, info.AlarmInfo.AlarmId),
                        info.AlarmInfo.AlarmId,
                        _encryption.EncryptData(info.AlarmInfo.Name, info.AlarmInfo.AlarmId));
                    _database.PairAlarms(info.Username, alarmDal);

                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
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
        /// converts the data into a <see cref="AlarmDal"/> and updates the new values
        /// </summary>
        /// <param name="username"></param>
        /// <param name="alarmInfo"></param>
        /// <returns></returns>
        public bool UpdateAlarmInfo(string username, AlarmInfoDto alarmInfo)
        {
            AlarmDal data = _database.GetAlarmInfo(alarmInfo.AlarmId);
            if (data == null)
                return false;

            AlarmDal alarm = new AlarmDal(_encryption.EncryptData(alarmInfo.StartTime, alarmInfo.AlarmId), _encryption.EncryptData(alarmInfo.EndTime, alarmInfo.AlarmId), alarmInfo.AlarmId, _encryption.EncryptData(alarmInfo.Name, alarmInfo.AlarmId));
            _database.UpdateAlarmInfo(username, alarm);
            return true;
        }
    }
}
