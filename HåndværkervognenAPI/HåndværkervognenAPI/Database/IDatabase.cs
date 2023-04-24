using HåndværkervognenAPI.Model;
using HåndværkervognenAPI.Models;

namespace HåndværkervognenAPI.Database
{
    public interface IDatabase
    {
        AlarmDal GetAlarmInfo(string alarmId);
        void PairAlarms(string appID, AlarmDal alarmInfo);
        List<AlarmDal> GetAlarms(string appId);
        void UpdateTimespan(string appId, AlarmDal alarmDal);
        UserDal GetUser(string username);
        void CreateUser(UserDal user);
        void DeleteUser(string username);
        void StopAlarm(string alarmId);
        void StartAlarm(string alarmId);
        void DeletePairing(string alarmId, string username);

    }
}
