using HåndværkervognenAPI.Model;

namespace HåndværkervognenAPI.Managers
{
    public interface IAlarmService
    {
        AlarmInfoDto GetAlarmInfo(string alarmid);
        bool DeletePairing(string alarmId, string username);
        bool AlertUser(string alarmId);
    }
}
