using HåndværkervognenAPI.Model;

namespace HåndværkervognenAPI.Managers
{
    public interface IAppService
    {
        void UpdateTimeSpan(string AppId, AlarmInfoDto alarmInfo);
        void PairAlarm(PairInfo info);
        List<AlarmInfoDto>GetAlarms(string AppId);
        void StopAlarm(string AlarmId);
    }
}
