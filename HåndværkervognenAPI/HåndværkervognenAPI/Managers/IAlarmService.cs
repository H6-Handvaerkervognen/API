using HåndværkervognenAPI.Model;

namespace HåndværkervognenAPI.Managers
{
    public interface IAlarmService
    {
        AlarmInfoDto GetAlarmInfo(string alarmid);
        bool DeletePairing(string alarmId);
        bool AlertUser(string alarmId);
    }
}
