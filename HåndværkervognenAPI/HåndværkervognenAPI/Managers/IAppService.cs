using HåndværkervognenAPI.Models;

namespace HåndværkervognenAPI.Managers
{
    public interface IAppService
    {
        bool UpdateAlarmInfo(string username, AlarmInfoDto alarmInfo);
        bool PairAlarm(PairInfo info);
        List<AlarmInfoDto>GetAlarms(string username);
        bool StopAlarm(string alarmId);
    }
}
