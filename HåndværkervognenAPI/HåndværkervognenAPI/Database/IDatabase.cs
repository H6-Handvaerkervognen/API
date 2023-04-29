using HåndværkervognenAPI.Models;

namespace HåndværkervognenAPI.Database
{
    public interface IDatabase
    {
        AlarmDal GetAlarmInfo(string alarmId);
        void PairAlarms(string username, AlarmDal alarmInfo);
        List<AlarmDal> GetAlarms(string username);
        void UpdateTimespan(string username, AlarmDal alarmDal);
        UserDal GetUser(string username);
        void CreateUser(UserDal user);

        void StopAlarm(string alarmId);
        void StartAlarm(string alarmId);
        void DeletePairing(string alarmId);
        bool CheckIfUserExists(string username);
        bool CheckIfPairExists(string alarmId, string username);
        bool CheckIfAlarmExists(string alarmId);
        bool CheckAlarmStatus(string alarmId);

    }
}
