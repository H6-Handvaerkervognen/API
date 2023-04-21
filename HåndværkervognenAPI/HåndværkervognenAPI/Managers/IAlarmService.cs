using HåndværkervognenAPI.Model;

namespace HåndværkervognenAPI.Managers
{
    public interface IAlarmService
    {
        AlarmInfoDto GetAlarmInfo(string alarmid);
        void DeletePairing(string alarmId);
        void AlertUser(string alarmId);
    }
}
