using HåndværkervognenAPI.Database;
using HåndværkervognenAPI.Model;

namespace HåndværkervognenAPI.Managers
{
    public class AppManager : IAppService
    {
        private IDatabase _database;

        public IDatabase DataBase
        {
            get { return _database; }
            set { _database = value; }
        }


        public List<AlarmInfoDto> GetAlarms(string AppId)
        {
            throw new NotImplementedException();
        }

        public void PairAlarm(PairInfo info)
        {
            throw new NotImplementedException();
        }

        public void StopAlarm(string AlarmId)
        {
            throw new NotImplementedException();
        }

        public void UpdateTimeSpan(string AppId, AlarmInfoDto alarmInfo)
        {
            throw new NotImplementedException();
        }
    }
}
