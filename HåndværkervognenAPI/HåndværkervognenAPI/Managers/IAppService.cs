using HåndværkervognenAPI.Models;

namespace HåndværkervognenAPI.Managers
{
    public interface IAppService
    {
        bool UpdateTimeSpan(string username, AlarmInfoDto alarmInfo);
        string PairAlarm(PairInfo info);
        List<AlarmInfoDto> GetAlarms(string username);
        bool StopAlarm(string alarmId);
    }
}
