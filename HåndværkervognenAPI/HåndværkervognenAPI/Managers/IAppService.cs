using HåndværkervognenAPI.Model;

namespace HåndværkervognenAPI.Managers
{
    public interface IAppService
    {
        bool UpdateTimeSpan(string AppId, AlarmInfoDto alarmInfo);
        bool PairAlarm(PairInfo info);
        List<AlarmInfoDto>GetAlarms(string AppId);
        bool StopAlarm(string AlarmId);
    }
}
