using HåndværkervognenAPI.Model;
using HåndværkervognenAPI.Models;

namespace HåndværkervognenAPI.Database
{
    public class DataManager : IDatabase
    {
        public void createUser(UserDal user)
        {
            throw new NotImplementedException();
        }

        public void DeletePairing(string alarmId)
        {
            throw new NotImplementedException();
        }

        public void DeleteUser(string username)
        {
            throw new NotImplementedException();
        }

        public AlarmDal GetAlarmInfo(string alarmId)
        {
            throw new NotImplementedException();
        }

        public List<AlarmDal> GetAlarms(string appId)
        {
            throw new NotImplementedException();
        }

        public UserDal GetUser(string username)
        {
            throw new NotImplementedException();
        }

        public void PairAlarms(string appID, AlarmDal alarmInfo)
        {
            throw new NotImplementedException();
        }

        public void StopAlarm(string alarmId)
        {
            throw new NotImplementedException();
        }

        public void UpdateTimespan(string appId, AlarmDal alarmDal)
        {
            throw new NotImplementedException();
        }
    }
}
