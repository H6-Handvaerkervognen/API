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
                //alarmInfoDtos.Add(new AlarmInfoDto(TimeSpan.Parse(_encryption.DecryptData(alarm.StartTime)), TimeSpan.Parse(_encryption.DecryptData(alarm.EndTime)), alarm.AlarmId, _encryption.DecryptData(alarm.Name)));
                alarmInfoDtos.Add(new AlarmInfoDto(TimeSpan.Parse(alarm.StartTime), TimeSpan.Parse(alarm.EndTime), alarm.AlarmId, alarm.Name));
            }
            return alarmInfoDtos;
        }

        public void PairAlarm(PairInfo info)
        {
            //convert to alarmDal and hash AlarmId
           byte[] newSalt = _hashing.GenerateSalt();
           // _database.PairAlarms(info.AppId, info.);
        }

        public void StopAlarm(string AlarmId)
        {
            _database.StopAlarm(AlarmId);
        }

        public void UpdateTimeSpan(string AppId, AlarmInfoDto alarmInfo)
        {
           
            //todo hash alarmId

            AlarmDal alarm = new AlarmDal(_encryption.EncryptData(alarmInfo.StartTime.ToString()), _encryption.EncryptData(alarmInfo.EndTime.ToString()),alarmInfo.AlarmId, alarmInfo.Name);
            _database.UpdateTimespan(AppId, alarm);
        }
    }
}
