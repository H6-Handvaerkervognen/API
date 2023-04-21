using HåndværkervognenAPI.Model;
using HåndværkervognenAPI.Models;

namespace HåndværkervognenAPI.Database
{
    public interface IDatabase
    {
        AlarmDal GetAlarmInfo(string alarmId);
        void PairAlarms(string appID, AlarmDal alarmInfo);
        List<AlarmDal> GetAlarms(string appId);
        void UpdateTimespan(string appId, AlarmInfoDto alarmInfo);
        UserDal GetUser(string username);
        void createUser(UserDal user);
        void DeleteUser(string username);
        void StopAlarm(string alarmId);
        void DeletePairing(string alarmId);
    }
}
