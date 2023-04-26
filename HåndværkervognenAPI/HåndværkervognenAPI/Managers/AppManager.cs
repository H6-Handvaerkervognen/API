using HåndværkervognenAPI.Database;
using HåndværkervognenAPI.Models;
using HåndværkervognenAPI.Security;
using System.Security.Cryptography.Xml;

namespace HåndværkervognenAPI.Managers
{
    public class AppManager : IAppService
    {
        private IDatabase _database;
        private IEncryption _encryption;
        private IHashing _hashing;

        public AppManager(IDatabase database, IEncryption encryption,IHashing hashing)
        {
            _database= database;
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
                alarmInfoDtos.Add(new AlarmInfoDto(TimeSpan.Parse(_encryption.DecryptData(alarm.StartTime)), TimeSpan.Parse(_encryption.DecryptData(alarm.EndTime)), alarm.AlarmId, _encryption.DecryptData(alarm.Name)));
             
            }
            return alarmInfoDtos;
        }

        /// <summary>
        /// creates a new salt for alarmid if its the first time the alarm is paired. the n is hashes the alarm id and encrypts the rest of the data and stores it 
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool PairAlarm(PairInfo info)
        {
            //convert to alarmDal and hash AlarmId
           byte[] newSalt = _hashing.GenerateSalt();
            // _database.PairAlarms(info.AppId, info.);

            return true;
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
           
            //todo hash alarmId

            AlarmDal alarm = new AlarmDal(_encryption.EncryptData(alarmInfo.StartTime.ToString()), _encryption.EncryptData(alarmInfo.EndTime.ToString()),alarmInfo.AlarmId, alarmInfo.Name);
            _database.UpdateTimespan(username, alarm);
            return true;
        }
    }
}
