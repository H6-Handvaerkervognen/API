using HåndværkervognenAPI.Models;

namespace HåndværkervognenAPI.Managers
{
    public interface IAppService
    {
        bool UpdateTimeSpan(string username, AlarmInfoDto alarmInfo,string token);
        bool PairAlarm(PairInfo info, string token);
        List<AlarmInfoDto> GetAlarms(string username, string token);
        bool StopAlarm(string alarmId, string username, string token);
    }
}
