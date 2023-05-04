using HåndværkervognenAPI.Models;

namespace HåndværkervognenAPI.Managers
{
    public interface IAlarmService
    {
        AlarmInfoDto GetAlarmInfo(string alarmid);
        bool DeletePairing(string alarmId);
        bool AlertUser(string alarmId);
        bool GetStatus(string alarmId);
    }
}
