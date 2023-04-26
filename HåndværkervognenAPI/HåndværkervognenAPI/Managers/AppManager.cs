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
        /// <param name="AppId"></param>
        /// <returns>list of alarminfoDTO</returns>
        public List<AlarmInfoDto> GetAlarms(string appId)
        {
           List<AlarmDal> alarms = _database.GetAlarms(appId);
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
            try
            {
                AlarmDal alarmDal;
                AlarmDal DBData = _database.GetAlarmInfo(info.AlarmInfo.AlarmId);

                if (DBData != null)
                {
                    alarmDal = new AlarmDal(_encryption.EncryptData(info.AlarmInfo.StartTime.ToString()), _encryption.EncryptData(info.AlarmInfo.EndTime.ToString()), _hashing.GenerateHash(info.AlarmInfo.AlarmId, DBData.Salt).ToString(), _encryption.EncryptData(info.AlarmInfo.Name));
                    _database.PairAlarms(info.AppId, alarmDal);
                    return true;
                }
                byte[] newSalt = _hashing.GenerateSalt();
                alarmDal = new AlarmDal(_encryption.EncryptData(info.AlarmInfo.StartTime.ToString()), _encryption.EncryptData(info.AlarmInfo.EndTime.ToString()), _hashing.GenerateHash(info.AlarmInfo.AlarmId, newSalt).ToString(), _encryption.EncryptData(info.AlarmInfo.Name));


                _database.PairAlarms(info.AppId, alarmDal);

                return true;
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
        /// converts the data to the rigth objects and saves the new timespan
        /// </summary>
        /// <param name="AppId"></param>
        /// <param name="alarmInfo"></param>
        /// <returns></returns>
        public bool UpdateTimeSpan(string appId, AlarmInfoDto alarmInfo)
        {

<<<<<<< Updated upstream
            AlarmDal alarm = new AlarmDal(_encryption.EncryptData(alarmInfo.StartTime.ToString()), _encryption.EncryptData(alarmInfo.EndTime.ToString()),alarmInfo.AlarmId, alarmInfo.Name);
            _database.UpdateTimespan(appId, alarm);
            return true;
=======
            

            var data = _database.GetAlarmInfo(alarmInfo.AlarmId);
            if (_hashing.GenerateHash(alarmInfo.AlarmId, data.Salt).ToString() == data.AlarmId)
            {
                AlarmDal alarm = new AlarmDal(_encryption.EncryptData(alarmInfo.StartTime.ToString()), _encryption.EncryptData(alarmInfo.EndTime.ToString()), _hashing.GenerateHash(alarmInfo.AlarmId, data.Salt).ToString(), _encryption.EncryptData(alarmInfo.Name));
                _database.UpdateTimespan(AppId, alarm);
                return true;
            }
            return false;
            
>>>>>>> Stashed changes
        }
    }
}
