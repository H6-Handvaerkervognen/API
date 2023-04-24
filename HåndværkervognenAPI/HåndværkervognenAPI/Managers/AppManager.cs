using HåndværkervognenAPI.Database;
using HåndværkervognenAPI.Model;
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


        public List<AlarmInfoDto> GetAlarms(string AppId)
        {
           List<AlarmDal> alarms = _database.GetAlarms(AppId);
            List<AlarmInfoDto> alarmInfoDtos = new List<AlarmInfoDto>();
            foreach (AlarmDal alarm in alarms)
            {
                alarmInfoDtos.Add(new AlarmInfoDto(TimeSpan.Parse(_encryption.DecryptData(alarm.StartTime)), TimeSpan.Parse(_encryption.DecryptData(alarm.EndTime)), alarm.AlarmId, _encryption.DecryptData(alarm.Name)));
             
            }
            return alarmInfoDtos;
        }

        public bool PairAlarm(PairInfo info)
        {
            //convert to alarmDal and hash AlarmId
           byte[] newSalt = _hashing.GenerateSalt();
            // _database.PairAlarms(info.AppId, info.);

            return true;
        }

        public bool StopAlarm(string AlarmId)
        {
            _database.StopAlarm(AlarmId);
            return true;
        }

        public bool UpdateTimeSpan(string AppId, AlarmInfoDto alarmInfo)
        {
           
            //todo hash alarmId

            AlarmDal alarm = new AlarmDal(_encryption.EncryptData(alarmInfo.StartTime.ToString()), _encryption.EncryptData(alarmInfo.EndTime.ToString()),alarmInfo.AlarmId, alarmInfo.Name);
            _database.UpdateTimespan(AppId, alarm);
            return true;
        }
    }
}
